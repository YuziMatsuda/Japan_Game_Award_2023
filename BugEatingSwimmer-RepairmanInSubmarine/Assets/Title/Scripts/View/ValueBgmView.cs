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
    /// BGMスライダーの設定値
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class ValueBgmView : MonoBehaviour
    {
        /// <summary>テキスト</summary>
        [SerializeField] private Text text;

        private void Reset()
        {
            text = GetComponent<Text>();
        }

        /// <summary>
        /// ラベルへ値をセット
        /// </summary>
        /// <param name="value">値</param>
        /// <returns>成功／失敗</returns>
        public bool SetLabelValue(int value)
        {
            try
            {
                var tTValidation = new TitleTemplateOptionalInputValueValidation();
                if (tTValidation.CheckBgmValueAndGetResultState(value).Equals(EnumResponseState.Success))
                    text.text = $"{value}";
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
