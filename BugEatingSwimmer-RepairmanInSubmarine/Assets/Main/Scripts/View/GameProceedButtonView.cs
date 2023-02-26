using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Main.View
{
    /// <summary>
    /// ビュー
    /// 次のステージへ進む
    /// </summary>
    public class GameProceedButtonView : MonoBehaviour
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
        /// 拡大アニメーションを再生
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool SetScale()
        {
            try
            {
                if (_transform == null)
                    _transform = transform;
                _transform.localScale = Vector3.one * size;

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
