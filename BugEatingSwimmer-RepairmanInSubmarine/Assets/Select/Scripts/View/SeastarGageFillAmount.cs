using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Select.View
{
    /// <summary>
    /// ヒトデゲージのサイクルゲージ
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class SeastarGageFillAmount : MonoBehaviour, ISeastarGageFillAmount
    {
        /// <summary>スプライトレンダラー</summary>
        private Image _image;
        /// <summary>アニメーション時間</summary>
        [SerializeField] private float duration = .5f;

        public bool PlayFillAmountAnimation(float fillAmountValue)
        {
            try
            {
                if (_image == null)
                    _image = GetComponent<Image>();
                var prev = _image.fillAmount;
                if (!prev.Equals(fillAmountValue))
                {
                    // 差異があった場合のみ更新
                    DOTween.To(() => prev, x => _image.fillAmount = x, fillAmountValue, duration);
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetFillAmount(float fillAmountValue)
        {
            try
            {
                if (_image == null)
                    _image = GetComponent<Image>();
                var prev = _image.fillAmount;
                if (!prev.Equals(fillAmountValue))
                {
                    _image.fillAmount = fillAmountValue;
                }

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
    /// ヒトデゲージのサイクルゲージ
    /// インターフェース
    /// </summary>
    public interface ISeastarGageFillAmount
    {
        /// <summary>
        /// フィルターゲージの値をセット
        /// </summary>
        /// <param name="fillAmountValue">ゲージ値（0～1）</param>
        /// <returns>成功／失敗</returns>
        public bool SetFillAmount(float fillAmountValue);
        /// <summary>
        /// フィルターゲージの値をセット
        /// </summary>
        /// <param name="fillAmountValue">ゲージ値（0～1）</param>
        /// <returns>成功／失敗</returns>
        public bool PlayFillAmountAnimation(float fillAmountValue);
    }
}
