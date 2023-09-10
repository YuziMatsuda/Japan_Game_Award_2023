using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

namespace Main.View
{
    /// <summary>
    /// ビュー
    /// ヒトデ
    /// </summary>
    public class SeastarView : AbstractGimmickView, ISeastarView
    {
        /// <summary>Rigidbody2D</summary>
        [SerializeField] private new Rigidbody2D rigidbody2D;
        /// <summary>アニメーション終了時間</summary>
        [SerializeField] private float[] durations = { .3f, .6f, .3f, 1f, 1f };
        /// <summary>トランスフォーム情報</summary>
        [SerializeField] private Vector3[] transformStates = { Vector3.one * 1.3f, new Vector3(0f, 0f, -360f), Vector3.one, new Vector3(0f, 0f, -30f), new Vector3(0f, 0f, 30f) };
        /// <summary>シークエンス</summary>
        private List<Sequence> _sequence = new List<Sequence>();
        /// <summary>トランスフォーム</summary>
        private Transform _transform;

        protected override void Reset()
        {
            base.Reset();
            rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public bool SetColorSpriteRenderer(Color color)
        {
            return bodySprite.SetColorSpriteRenderer(color);
        }

        public bool SetColorAssigned()
        {
            return bodySprite.SetColorSpriteRenderer(assignedColor);
        }

        public bool SetColorUnAssign()
        {
            return bodySprite.SetColorSpriteRenderer(unAssignColor);
        }

        public IEnumerator PlaySpinAndScaling(System.IObserver<bool> observer)
        {
            if (_transform == null)
                _transform = transform;
            _sequence.Add(DOTween.Sequence()
                .Append(_transform.DOScale(transformStates[0], durations[0]))
                .Join(_transform.DOLocalRotate(transformStates[1], durations[1], RotateMode.FastBeyond360))
                .Insert(durations[1], _transform.DOScale(transformStates[2], durations[2]))
                .OnComplete(() => observer.OnNext(true))
                .SetLink(gameObject));

            yield return null;
        }

        public bool StopSpinAndScaling()
        {
            try
            {
                if (_transform == null)
                    _transform = transform;
                if (0 < _sequence.Count)
                {
                    foreach (var item in _sequence.Select((p, i) => new { Content = p, Index = i }))
                        if (item.Index == 0)
                            item.Content.Rewind();
                        else if (item.Index == 1)
                        {
                            item.Content.Kill();
                            _transform.localEulerAngles = Vector3.zero;
                        }
                    _sequence = new List<Sequence>();
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public IEnumerator PlaySwinging(System.IObserver<bool> observer)
        {
            if (_transform == null)
                _transform = transform;
            _sequence.Add(DOTween.Sequence()
                .Append(_transform.DOLocalRotate(transformStates[3], durations[3])
                    .From(transformStates[4]))
                .Append(_transform.DOLocalRotate(transformStates[4], durations[4]))
                .SetLoops(-1)
                .SetLink(gameObject));
            observer.OnNext(true);

            yield return null;
        }
    }

    public interface ISeastarView
    {
        /// <summary>
        /// アサイン済みのカラー設定
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool SetColorAssigned();

        /// <summary>
        /// 未アサイン状態のカラー設定
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool SetColorUnAssign();
        /// <summary>
        /// 回転しつつ拡大縮小アニメーションを再生
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlaySpinAndScaling(System.IObserver<bool> observer);
        /// <summary>
        /// ゆらゆらするアニメーションを再生
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlaySwinging(System.IObserver<bool> observer);
        /// <summary>
        /// 回転しつつ拡大縮小アニメーションを停止
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool StopSpinAndScaling();
    }
}
