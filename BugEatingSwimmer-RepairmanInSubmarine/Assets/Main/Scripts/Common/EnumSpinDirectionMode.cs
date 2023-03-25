using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Common
{
    /// <summary>
    /// 回転方角モード
    /// 上をゼロにして時計回りに設定
    /// </summary>
    public enum EnumSpinDirectionMode
    {
        /// <summary>どちらにも回る</summary>
        Auto,
        /// <summary>時計回り</summary>
        Positive,
        /// <summary>反時計回り</summary>
        Negative,
    }
}
