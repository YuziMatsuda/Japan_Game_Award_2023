using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;

namespace Main.View
{
    /// <summary>
    /// ビュー
    /// カーソル
    /// </summary>
    public class CursorIconView : MonoBehaviour, ICursorIconView
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
        /// <summary>カーソル移動の距離値</summary>
        [SerializeField] private float toDistance = 30f;
        /// <summary>アニメーション再生時間（その他）</summary>
        [SerializeField] private float[] durations = { .75f };
        /// <summary>アニメーション再生中状態</summary>
        private Tweener _isPlayingRoundMoveAnimationState;

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

        public bool PlayRoundMoveAnimation()
        {
            try
            {
                if (isActiveAndEnabled)
                {
                    if (_transform == null)
                        _transform = transform;
                    if (_isPlayingRoundMoveAnimationState != null &&
                        _isPlayingRoundMoveAnimationState.IsPlaying())
                        // 再生中なら再生しない
                        return true;

                    var current = (_transform as RectTransform).anchoredPosition;
                    _isPlayingRoundMoveAnimationState = (_transform as RectTransform).DOAnchorPos(current + Vector2.down * toDistance, durations[0])
                        .SetLoops(-1, LoopType.Yoyo)
                        .SetLink(gameObject)
                        ;
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool RewindRoundMoveAnimation()
        {
            try
            {
                if (isActiveAndEnabled)
                {
                    if (_transform == null)
                        _transform = transform;
                    if (_isPlayingRoundMoveAnimationState != null &&
                        _isPlayingRoundMoveAnimationState.IsPlaying())
                    {
                        _isPlayingRoundMoveAnimationState.Rewind();
                        _isPlayingRoundMoveAnimationState = null;
                        // 再生中なら再生しない
                        return true;
                    }
                    else
                        return false;
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool RePlayRoundMoveAnimation()
        {
            try
            {
                if (isActiveAndEnabled)
                {
                    if (_transform == null)
                        _transform = transform;
                    if (_isPlayingRoundMoveAnimationState != null)
                    {
                        DOVirtual.DelayedCall(durations[1], () => _isPlayingRoundMoveAnimationState.Play());
                        // 再生中なら再生しない
                        return true;
                    }
                    else
                        return false;
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

    /// <summary>
    /// ビュー
    /// カーソル
    /// インターフェース
    /// </summary>
    public interface ICursorIconView
    {
        /// <summary>
        /// カーソル配置位置の変更
        /// </summary>
        /// <param name="position">移動先のポジション</param>
        /// <returns>成功／失敗</returns>
        public bool SetSelect(Vector3 position);
        /// <summary>
        /// カーソル移動アニメーション
        /// </summary>
        /// <param name="position">移動先のポジション</param>
        /// <returns>成功／失敗</returns>
        public bool PlaySelectAnimation(Vector3 position);
        /// <summary>
        /// カーソル往復移動アニメーション
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool PlayRoundMoveAnimation();
        /// <summary>
        /// カーソル往復移動アニメーション・一時停止
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool RewindRoundMoveAnimation();
        /// <summary>
        /// カーソル往復移動アニメーション・再生
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool RePlayRoundMoveAnimation();
    }
}
