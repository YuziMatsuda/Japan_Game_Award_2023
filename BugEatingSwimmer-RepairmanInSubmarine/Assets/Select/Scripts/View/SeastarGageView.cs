using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Select.View
{
    /// <summary>
    /// ビュー
    /// ヒトデゲージ
    /// </summary>
    [RequireComponent(typeof(SeastarGageConfig))]
    public class SeastarGageView : PivotAndCodeIShortUIViewParent, ISeastarGageFillAmount, ISeastarGageCounter, ISeastarGageBackgroundView
    {
        /// <summary>ヒトデゲージのサイクルゲージ</summary>
        [SerializeField] private SeastarGageFillAmount seastarGageFillAmount;
        /// <summary>ヒトデゲージカウンター</summary>
        [SerializeField] private SeastarGageCounter seastarGageCounter;
        /// <summary>カウンター最大値</summary>
        [SerializeField, Range(1, 10)] private int denominator = 10;
        /// <summary>カウンター変動値</summary>
        private int _numerator;
        /// <summary>カウント中</summary>
        public bool IsCounting => _numerator < denominator;
        /// <summary>設定</summary>
        [SerializeField] private SeastarGageConfig seastarGageConfig;
        /// <summary>設定</summary>
        public SeastarGageConfig SeastarGageConfig => seastarGageConfig;
        /// <summary>ヒトデゲージ背景</summary>
        [SerializeField] private SeastarGageBackgroundView seastarGageBackgroundView;

        public bool PlayCounterBetweenAnimation(int numerator)
        {
            _numerator = numerator < denominator ? numerator : denominator;
            return ((ISeastarGageCounter)seastarGageCounter).PlayCounterBetweenAnimation(numerator, denominator);
        }

        public bool PlayCounterBetweenAnimation(int numerator, int denominator)
        {
            throw new System.NotImplementedException();
        }

        public bool PlayFillAmountAnimation(float fillAmountValue)
        {
            float d = (float)denominator;
            var workFillAmountValue = fillAmountValue < d ? fillAmountValue : d;
            return ((ISeastarGageFillAmount)seastarGageFillAmount).PlayFillAmountAnimation(workFillAmountValue / d);
        }

        public bool SetCounterBetween(int numerator)
        {
            _numerator = numerator < denominator ? numerator : denominator;
            return ((ISeastarGageCounter)seastarGageCounter).SetCounterBetween(numerator, denominator);
        }

        public bool SetCounterBetween(int numerator, int denominator)
        {
            throw new System.NotImplementedException();
        }

        public bool SetFillAmount(float fillAmountValue)
        {
            float d = (float)denominator;
            var workFillAmountValue = fillAmountValue < d ? fillAmountValue : d;
            return ((ISeastarGageFillAmount)seastarGageFillAmount).SetFillAmount(workFillAmountValue / d);
        }

        private void Reset()
        {
            seastarGageFillAmount = transform.GetComponentInChildren<SeastarGageFillAmount>();
            seastarGageCounter = transform.GetComponentInChildren<SeastarGageCounter>();
            seastarGageConfig = GetComponent<SeastarGageConfig>();
            seastarGageBackgroundView = GetComponentInChildren<SeastarGageBackgroundView>();
        }

        public IEnumerator PlayOpenDirectionAnimations(System.IObserver<bool> observer)
        {
            Observable.FromCoroutine<bool>(observer => seastarGageBackgroundView.PlayOpenDirectionAnimations(observer))
                .Subscribe(_ => observer.OnNext(true))
                .AddTo(gameObject);

            yield return null;
        }
    }
}
