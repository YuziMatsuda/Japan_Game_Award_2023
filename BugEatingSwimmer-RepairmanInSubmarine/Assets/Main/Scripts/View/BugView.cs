using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Main.View
{
    /// <summary>
    /// ビュー
    /// バグ
    /// </summary>
    public class BugView : MonoBehaviour, IBugView
    {
        /// <summary>クリアカラー</summary>
        [SerializeField] private Color clearedColor = Color.yellow;
        /// <summary>既にクリア済みのクリアカラー</summary>
        [SerializeField] private Color alreadyClearedColor = Color.blue;

        /// <summary>トランスフォーム</summary>
        private Transform _transform;

        public bool SetColorCleared(bool isAlreadyCleared)
        {
            if (_transform == null)
                _transform = transform;
            return _transform.GetChild(0).GetComponent<BodySprite>().SetColorSpriteRenderer(isAlreadyCleared ? alreadyClearedColor : clearedColor);
        }
    }

    /// <summary>
    /// ビュー
    /// バグ
    /// インターフェース
    /// </summary>
    public interface IBugView
    {
        /// <summary>
        /// カラーを設定
        /// </summary>
        /// <param name="isAlreadyCleared">クリア済みならTrue</param>
        /// <returns>成功／失敗</returns>
        public bool SetColorCleared(bool isAlreadyCleared);
    }
}
