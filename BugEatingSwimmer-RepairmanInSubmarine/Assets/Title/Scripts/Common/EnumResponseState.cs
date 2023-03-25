using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Title.Common
{
    /// <summary>
    /// 入力値のチェックの結果
    /// </summary>
    public enum EnumResponseState
    {
        /// <summary>成功</summary>
        Success,
        /// <summary>警告</summary>
        Warning,
        /// <summary>エラー</summary>
        Error,
    }
}
