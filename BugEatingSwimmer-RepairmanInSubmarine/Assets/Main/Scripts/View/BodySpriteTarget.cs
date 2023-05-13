using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Main.View
{
    /// <summary>
    /// ターゲットのスプライト
    /// </summary>
    public class BodySpriteTarget : BodySprite, IBodySpriteTarget
    {
        /// <summary>実行中のアニメーション</summary>
        private Sequence _sequence;
        /// <summary>実行中のアニメーション</summary>
        private Transform _transform;
        /// <summary>回転アニメーション</summary>
        [SerializeField] private Vector3 localRotate = new Vector3(0f, 0f, 360f);

        private void Reset()
        {
            fadeDuration = .5f;
        }

        public IEnumerator PlayLockOnAnimation(System.IObserver<bool> observer, Vector3 target)
        {
            if (_transform == null)
                _transform = transform;
            _sequence = DOTween.Sequence()
                .Append(_transform.DOMove(target, fadeDuration))
                .Join(_transform.DOLocalRotate(localRotate, fadeDuration, RotateMode.FastBeyond360)
                    .SetEase(Ease.Linear)
                    .SetLoops(-1, LoopType.Restart)
                    .From(Vector3.zero))
                .SetLink(gameObject);

            yield return null;
        }

        public bool StopLockOnAnimation()
        {
            try
            {
                if (_transform == null)
                    _transform = transform;
                if (_sequence != null)
                    _sequence.Kill();

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool ResetPosition(Vector3 fromPosition)
        {
            try
            {
                if (_transform == null)
                    _transform = transform;
                _transform.position = fromPosition;

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
    /// ターゲットのスプライト
    /// インターフェース
    /// </summary>
    public interface IBodySpriteTarget
    {
        /// <summary>
        /// ロックオンアニメーションを再生
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <param name="target">ターゲット</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayLockOnAnimation(System.IObserver<bool> observer, Vector3 target);
        /// <summary>
        /// ロックオンアニメーションを停止
        /// </summary>
        /// <returns>コルーチン</returns>
        public bool StopLockOnAnimation();
        /// <summary>
        /// 位置をリセット
        /// </summary>
        /// <param name="fromPosition">ジョーシー位置</param>
        /// <returns>成功／失敗</returns>
        public bool ResetPosition(Vector3 fromPosition);
    }
}
