using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

namespace Select.View
{
    /// <summary>
    /// ヒトデゲージ背景
    /// ビュー
    /// </summary>
    public class SeastarGageBackgroundView : MonoBehaviour, ISeastarGageBackgroundView
    {
        /// <summary>ヒトデゲージ解放演出</summary>
        [SerializeField] private SeastarGageOpenDirection[] seastarGageOpenDirections;

        public IEnumerator PlayOpenDirectionAnimations(System.IObserver<bool> observer)
        {
            var count = new IntReactiveProperty();
            count.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    if (seastarGageOpenDirections.Length <= x)
                        observer.OnNext(true);
                });
            foreach (var item in seastarGageOpenDirections.Select((p, i) => new { Content = p, Index = i }))
                Observable.FromCoroutine<bool>(observer => item.Content.PlayOpenDirectionAnimation(observer))
                    .Subscribe(_ => count.Value++)
                    .AddTo(gameObject);

            yield return null;
        }

        private void Reset()
        {
            seastarGageOpenDirections = GetComponentsInChildren<SeastarGageOpenDirection>();
        }
    }

    /// <summary>
    /// ヒトデゲージ背景
    /// ビュー
    /// インターフェース
    /// </summary>
    public interface ISeastarGageBackgroundView
    {
        /// <summary>
        /// ゲージオープン演出を再生
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayOpenDirectionAnimations(System.IObserver<bool> observer);
    }
}
