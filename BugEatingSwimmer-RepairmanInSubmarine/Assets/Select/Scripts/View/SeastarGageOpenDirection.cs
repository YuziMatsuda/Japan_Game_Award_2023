using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Select.View
{
    /// <summary>
    /// ヒトデゲージ解放演出
    /// ビュー
    /// </summary>
    public class SeastarGageOpenDirection : MonoBehaviour, ISeastarGageOpenDirection
    {
        /// <summary>トランスフォーム</summary>
        private Transform _transform;
        /// <summary>設定</summary>
        [SerializeField] private SeastarGageConfig seastarGageConfig;
        /// <summary>アニメーション終了時間</summary>
        [SerializeField] private float[] durations = { .5f };

        private void Reset()
        {
            seastarGageConfig = GetComponent<SeastarGageConfig>();
        }

        public IEnumerator PlayOpenDirectionAnimation(System.IObserver<bool> observer)
        {
            if (_transform == null)
                _transform = transform;
            (_transform as RectTransform).DOAnchorPosX(seastarGageConfig.OpenedAnchorPosX, durations[0])
                .SetEase(Ease.InQuad)
                .OnComplete(() => observer.OnNext(true));

            yield return null;
        }
    }

    /// <summary>
    /// ヒトデゲージ解放演出
    /// ビュー
    /// インターフェース
    /// </summary>
    public interface ISeastarGageOpenDirection
    {
        /// <summary>
        /// ゲージオープン演出を再生
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayOpenDirectionAnimation(System.IObserver<bool> observer);
    }
}
