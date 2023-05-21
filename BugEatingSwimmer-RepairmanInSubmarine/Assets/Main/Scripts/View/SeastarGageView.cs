using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Common;

namespace Main.View
{
    /// <summary>
    /// ビュー
    /// ヒトデゲージ
    /// </summary>
    public class SeastarGageView : ShadowCodeCellParent, ISeastarGageFillAmount, ISeastarGageCounter, ISeastarGageView
    {
        /// <summary>ヒトデゲージのサイクルゲージ</summary>
        [SerializeField] private SeastarGageFillAmountUI seastarGageFillAmount;
        /// <summary>ヒトデゲージカウンター</summary>
        [SerializeField] private SeastarGageCounter seastarGageCounter;
        /// <summary>カウンター最大値</summary>
        [SerializeField, Range(1, 9)] private int denominator = 9;
        /// <summary>カウンター変動値</summary>
        private int _numerator;
        /// <summary>カウント中</summary>
        public bool IsCounting => _numerator < denominator;
        /// <summary>ヒトデゲージの背景</summary>
        [SerializeField] private SeastartGageBackground seastartGageBackground;

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
            seastartGageBackground = GetComponentInChildren<SeastartGageBackground>();
        }

        private void Start()
        {
            var obj = GameObject.FindGameObjectWithTag(ConstTagNames.TAG_NAME_SEASTARGAGEUI);
            seastarGageFillAmount = obj.GetComponentInChildren<SeastarGageFillAmountUI>();
            if (!seastarGageFillAmount.SetTarget(transform))
                Debug.LogError("ターゲットをセット呼び出しの失敗");
            seastarGageCounter = obj.GetComponentInChildren<SeastarGageCounter>();
            if (!seastarGageCounter.SetTarget(transform))
                Debug.LogError("ターゲットをセット呼び出しの失敗");
        }

        public bool SetSpriteBreak()
        {
            return seastartGageBackground.SetSprite(0);
        }

        public bool SetSpriteBubble()
        {
            return seastartGageBackground.SetSprite(1);
        }

        public bool SetImageEnabled()
        {
            try
            {
                if (!seastarGageFillAmount.SetImageEnabled(true))
                    throw new System.Exception("イメージの表示状態をセット呼び出しの失敗");

                if (!seastarGageCounter.SetImageEnabled(true))
                    throw new System.Exception("イメージの表示状態をセット呼び出しの失敗");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetImageDisable()
        {
            try
            {
                if (!seastarGageFillAmount.SetImageEnabled(false))
                    throw new System.Exception("イメージの表示状態をセット呼び出しの失敗");

                if (!seastarGageCounter.SetImageEnabled(false))
                    throw new System.Exception("イメージの表示状態をセット呼び出しの失敗");

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
    /// ヒトデゲージ
    /// インターフェース
    /// </summary>
    public interface ISeastarGageView
    {
        /// <summary>
        /// 壊れるスプライトをセット
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool SetSpriteBreak();
        /// <summary>
        /// 泡スプライトをセット
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool SetSpriteBubble();
        /// <summary>
        /// イメージを表示
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool SetImageEnabled();
        /// <summary>
        /// イメージを非表示
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool SetImageDisable();
    }
}
