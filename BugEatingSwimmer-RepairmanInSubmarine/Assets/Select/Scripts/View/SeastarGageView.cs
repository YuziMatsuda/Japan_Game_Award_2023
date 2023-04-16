using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Select.View
{
    /// <summary>
    /// ビュー
    /// ヒトデゲージ
    /// </summary>
    public class SeastarGageView : PivotAndCodeIShortUIViewParent, ISeastarGageFillAmount, ISeastarGageCounter
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
        }
    }
}
