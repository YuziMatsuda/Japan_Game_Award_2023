using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Title.View
{
    /// <summary>
    /// ビュー
    /// 全ステージ解放
    /// </summary>
    public class AllLevelReleasedMessageView : MonoBehaviour
    {
        /// <summary>位置の配列</summary>
        [SerializeField] private Vector2[] points = new Vector2[2];
        /// <summary>終了時間の配列</summary>
        [SerializeField] private float[] durations = { .25f, 1.5f, 1.25f };
        /// <summary>トランスフォーム</summary>
        private Transform _transform;
        /// <summary>DOTweenアニメーション管理</summary>
        private Sequence _sequence;

        private void Reset()
        {
            points[0] = (transform as RectTransform).anchoredPosition + Vector2.down * 50f + new Vector2(0f, (transform as RectTransform).sizeDelta.y / 2 * -1f);
            points[1] = (transform as RectTransform).anchoredPosition;
        }

        private void OnEnable()
        {
            if (_sequence != null)
                _sequence.Kill(true);
        }

        /// <summary>
        /// テロップ表示アニメーション
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayBoundAnimation(System.IObserver<bool> observer)
        {
            if (_transform == null)
                _transform = transform;
            // 手前に表示させる
            _transform.SetAsLastSibling();
            _sequence = DOTween.Sequence()
                .Append((_transform as RectTransform).DOAnchorPos(points[0], durations[0]))
                .Insert(durations[1], (_transform as RectTransform).DOAnchorPos(points[1], durations[2]))
                .SetUpdate(true)
                .OnComplete(() => observer.OnNext(true));
            _sequence.Play();

            yield return null;
        }
    }
}
