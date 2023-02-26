using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Title.Common;

namespace Title.Accessory
{
    /// <summary>
    /// オプション機能にて入力値のチェックを行う
    /// タイトル用
    /// </summary>
    public class TitleOptionalInputValueValidation
    {
        /// <summary>
        /// BGMの設定値をチェックしてその結果を返却
        /// </summary>
        /// <param name="value">値</param>
        /// <returns>入力値のチェックの結果</returns>
        public EnumResponseState CheckBgmValueAndGetResultState(int value)
        {
            try
            {
                if (CheckRangeZeroBetweenTen(value))
                    return EnumResponseState.Success;
                else
                    throw new System.Exception("値が範囲外");
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return EnumResponseState.Error;
            }
        }

        /// <summary>
        /// SEの設定値をチェックしてその結果を返却
        /// </summary>
        /// <param name="value">値</param>
        /// <returns>入力値のチェックの結果</returns>
        public EnumResponseState CheckSeValueAndGetResultState(int value)
        {
            try
            {
                if (CheckRangeZeroBetweenTen(value))
                    return EnumResponseState.Success;
                else
                    throw new System.Exception("値が範囲外");
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return EnumResponseState.Error;
            }
        }

        /// <summary>
        /// バイブレーションの設定値をチェックしてその結果を返却
        /// </summary>
        /// <param name="value">値</param>
        /// <returns>入力値のチェックの結果</returns>
        public EnumResponseState CheckVibrationValueAndGetResultState(int value)
        {
            try
            {
                if (CheckEqualValuesTheSwitch(value))
                    return EnumResponseState.Success;
                else
                    throw new System.Exception("値が範囲外");
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return EnumResponseState.Error;
            }
        }

        /// <summary>
        /// 決められた値と等しいかのチェック
        /// スイッチ系
        /// </summary>
        /// <param name="value">値</param>
        /// <returns>成功／失敗</returns>
        private bool CheckEqualValuesTheSwitch(int value)
        {
            return -1 < value &&
                    value < 2;
        }

        /// <summary>
        /// 範囲チェック
        /// 0から10に含まれるか
        /// </summary>
        /// <param name="value">値</param>
        /// <returns>成功／失敗</returns>
        private bool CheckRangeZeroBetweenTen(int value)
        {
            return -1 < value &&
                    value < 11;
        }
    }
}
