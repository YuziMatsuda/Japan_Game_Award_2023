using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Main.View
{
    /// <summary>
    /// ビュー
    /// もう一度遊ぶボタン
    /// </summary>
    public class GameRetryButtonView : MonoBehaviour
    {
        /// <summary>トランスフォーム</summary>
        private Transform _transform;
        /// <summary>アニメーション終了時間</summary>
        [SerializeField] private float duration = .05f;
        /// <summary>拡大率</summary>
        [SerializeField] private float size = 1.25f;

        /// <summary>
        /// 拡大アニメーションを再生
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool PlayScaleUpAnimation()
        {
            try
            {
                if (_transform == null)
                    _transform = transform;
                _transform.DOScale(size, duration)
                    .SetUpdate(true);

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// デフォルトサイズをセット
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool SetDefaultScale()
        {
            try
            {
                if (_transform == null)
                    _transform = transform;
                _transform.localScale = Vector3.one;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }
    }
}
