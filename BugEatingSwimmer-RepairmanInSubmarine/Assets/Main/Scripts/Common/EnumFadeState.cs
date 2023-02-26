using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Common
{
    /// <summary>
    /// フェードステータス
    /// </summary>
    public enum EnumFadeState
    {
        /// <summary>オープンモード</summary>
        Open = 1,
        /// <summary>デフォルト</summary>
        Default = 0,
        /// <summary>クローズ</summary>
        Close = -1,
    }
}
