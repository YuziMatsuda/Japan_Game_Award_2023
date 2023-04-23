using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// ビュー
    /// ヒトデゲージ
    /// </summary>
    public class SeastarGageView : ShadowCodeCellParent, ISeastarGageFillAmount, ISeastarGageCounter
    {
        /// <summary>ヒトデゲージのサイクルゲージ</summary>
        [SerializeField] private SeastarGageFillAmount seastarGageFillAmount;
        /// <summary>ヒトデゲージカウンター</summary>
        [SerializeField] private SeastarGageCounter seastarGageCounter;
        /// <summary>カウンター最大値</summary>
        [SerializeField, Range(1, 9)] private int denominator = 9;
        /// <summary>カウンター変動値</summary>
        private int _numerator;
        /// <summary>カウント中</summary>
        public bool IsCounting => _numerator < denominator;

        public bool PlayCounterBetweenAnimation(int numerator)
        {
            _numerator = numerator;
            return ((ISeastarGageCounter)seastarGageCounter).PlayCounterBetweenAnimation(numerator, denominator);
        }

        public bool PlayCounterBetweenAnimation(int numerator, int denominator)
        {
            throw new System.NotImplementedException();
        }

        public bool PlayFillAmountAnimation(float fillAmountValue)
        {
            float d = (float)denominator;
            return ((ISeastarGageFillAmount)seastarGageFillAmount).PlayFillAmountAnimation(fillAmountValue / d);
        }

        public bool SetCounterBetween(int numerator)
        {
            _numerator = numerator;
            return ((ISeastarGageCounter)seastarGageCounter).SetCounterBetween(numerator, denominator);
        }

        public bool SetCounterBetween(int numerator, int denominator)
        {
            throw new System.NotImplementedException();
        }

        public bool SetFillAmount(float fillAmountValue)
        {
            float d = (float)denominator;
            return ((ISeastarGageFillAmount)seastarGageFillAmount).SetFillAmount(fillAmountValue / d);
        }

        private void Reset()
        {
            seastarGageFillAmount = transform.GetChild(1).GetComponent<SeastarGageFillAmount>();
            seastarGageCounter = transform.GetChild(3).GetChild(0).GetComponent<SeastarGageCounter>();
        }
    }
}
