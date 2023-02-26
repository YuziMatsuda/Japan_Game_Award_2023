using System.Collections;
using System.Collections.Generic;
using Title.Common;
using Title.Template;
using UnityEngine;
using UnityEngine.UI;

namespace Title.View
{
    /// <summary>
    /// ビュー
    /// BGMスライダー
    /// </summary>
    [RequireComponent(typeof(Slider))]
    public class SliderBgmView : MonoBehaviour
    {
        /// <summary>スライダー</summary>
        [SerializeField] private Slider _slider;

        private void Reset()
        {
            if (_slider == null)
                _slider = GetComponent<Slider>();
        }

        /// <summary>
        /// スライダーの値をセット
        /// </summary>
        /// <param name="value">値</param>
        /// <returns>成功／失敗</returns>
        public bool SetSliderValue(int value)
        {
            try
            {
                var tTValidation = new TitleTemplateOptionalInputValueValidation();
                if (tTValidation.CheckBgmValueAndGetResultState(value).Equals(EnumResponseState.Success))
                    _slider.value = value;
                else
                    throw new System.Exception("値チェックのエラー");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }
    }
}
