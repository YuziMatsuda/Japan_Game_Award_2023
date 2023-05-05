using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Main.View
{
    /// <summary>
    /// エビダンスのボディのスプライト
    /// </summary>
    public class BodySpriteShrimpDance : BodySprite, IBodySpriteShrimpDance
    {
        /// <summary>シークエンス</summary>
        private Sequence _sequence;
        /// <summary>トランスフォーム</summary>
        private Transform _transform;
        /// <summary>ジャンプ力</summary>
        [SerializeField] private float jumpPower = .5f;
        /// <summary>ジャンプ回数</summary>
        [SerializeField] private int numJump = 3;
        /// <summary>アニメーション終了時間</summary>
        [SerializeField] private float[] durations = { .8f };

        public bool PlayDanceAnimation()
        {
            try
            {
                if (_transform == null)
                    _transform = transform;
                _sequence = _transform.DOLocalJump(Vector3.zero, jumpPower, numJump, durations[0])
                    .SetEase(Ease.Linear)
                    .SetLoops(-1, LoopType.Restart)
                    .Join(DOTween.To(() => 0, x => GetComponent<SpriteRenderer>().flipX = x == 1, 1, durations[0])
                        .SetLoops(-1, LoopType.Yoyo));
                _sequence.Play();

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool StopDanceAnimation()
        {
            try
            {
                if (_sequence != null)
                    _sequence.Rewind();

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
    /// エビダンスのボディのスプライト
    /// インターフェース
    /// </summary>
    public interface IBodySpriteShrimpDance
    {
        /// <summary>
        /// ダンスアニメーションを再生
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool PlayDanceAnimation();
        /// <summary>
        /// ダンスアニメーションを停止
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool StopDanceAnimation();
    }
}
