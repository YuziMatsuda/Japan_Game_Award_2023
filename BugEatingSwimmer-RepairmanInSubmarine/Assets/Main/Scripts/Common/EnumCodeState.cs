using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Common
{
    /// <summary>
    /// コード状態管理
    /// </summary>
    public enum EnumCodeState
    {
        /// <summary>値なし</summary>
        Empty,
        /// <summary>通常コード</summary>
        Normal,
        /// <summary>重要コード</summary>
        Important,
        /// <summary>エラーコード</summary>
        Error = 9,
    }
}
