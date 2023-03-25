using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;

namespace Title.View
{
    /// <summary>
    /// ビュー
    /// カーソル
    /// </summary>
    public class CursorIconView : MonoBehaviour
    {
        /// <summary>DoTweenアニメーション再生状態</summary>
        private readonly BoolReactiveProperty _isAnimationPlaying = new BoolReactiveProperty();
        /// <summary>DoTweenアニメーション再生状態</summary>
        public IReactiveProperty<bool> IsAnimationPlaying => _isAnimationPlaying;
        /// <summary>トランスフォーム</summary>
        private Transform _transform;
        /// <summary>アニメーション再生時間</summary>
        [SerializeField] private float duration = .1f;
        /// <summary>カーソル位置の補正値</summary>
        [SerializeField] private Vector3 positionOffSet = new Vector3(-360f, 0, 0);

        /// <summary>
        /// カーソル配置位置の変更
        /// </summary>
        /// <param name="position">移動先のポジション</param>
        /// <returns>成功／失敗</returns>
        public bool SetSelect(Vector3 position)
        {
            try
            {
                if (isActiveAndEnabled)
                {
                    if (_transform == null)
                        _transform = transform;
                    _transform.position = position + positionOffSet;
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// カーソル移動アニメーション
        /// </summary>
        /// <param name="position">移動先のポジション</param>
        /// <returns>成功／失敗</returns>
        public bool PlaySelectAnimation(Vector3 position)
        {
            try
            {
                if (isActiveAndEnabled)
                {
                    if (!_isAnimationPlaying.Value)
                        _isAnimationPlaying.Value = true;
                    if (_transform == null)
                        _transform = transform;
                    _transform.DOMove(position + positionOffSet, duration)
                        .SetUpdate(true)
                        .OnComplete(() => _isAnimationPlaying.Value = false);
                }

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
