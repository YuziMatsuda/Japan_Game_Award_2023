using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Title.Common
{
    /// <summary>
    /// オプション項目選択表示ステータス
    /// </summary>
    public enum EnumOptionContentState
    {
        /// <summary>選択</summary>
        Selected = 1,
        /// <summary>デフォルト</summary>
        Default = 0,
        /// <summary>非選択</summary>
        DeSelected = -1,
    }
}
