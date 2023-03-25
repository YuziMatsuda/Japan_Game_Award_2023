using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Title.Common;
using Title.Accessory;

namespace Title.Template
{
    /// <summary>
    /// オプション機能にて入力値のチェックを行うテンプレート
    /// タイトル用
    /// </summary>
    public class TitleTemplateOptionalInputValueValidation
    {
        /// <summary>
        /// BGMの設定値をチェックしてその結果を返却
        /// </summary>
        /// <param name="value">値</param>
        /// <returns>入力値のチェックの結果</returns>
        public EnumResponseState CheckBgmValueAndGetResultState(int value)
        {
            return new TitleOptionalInputValueValidation().CheckBgmValueAndGetResultState(value);
        }

        /// <summary>
        /// SEの設定値をチェックしてその結果を返却
        /// </summary>
        /// <param name="value">値</param>
        /// <returns>入力値のチェックの結果</returns>
        public EnumResponseState CheckSeValueAndGetResultState(int value)
        {
            return new TitleOptionalInputValueValidation().CheckSeValueAndGetResultState(value);
        }

        /// <summary>
        /// バイブレーションの設定値をチェックしてその結果を返却
        /// </summary>
        /// <param name="value">値</param>
        /// <returns>入力値のチェックの結果</returns>
        public EnumResponseState CheckVibrationValueAndGetResultState(int value)
        {
            return new TitleOptionalInputValueValidation().CheckVibrationValueAndGetResultState(value);
        }
    }
}
