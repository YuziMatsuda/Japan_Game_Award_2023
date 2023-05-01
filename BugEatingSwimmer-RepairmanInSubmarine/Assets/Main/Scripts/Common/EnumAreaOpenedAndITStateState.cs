using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Common
{
    /// <summary>
    /// ステージのステータス（0：選択不可、1：選択可、2：クリア済み、3：結合テスト済み、-1：ダミー）
    /// </summary>
    public enum EnumAreaOpenedAndITStateState
    {
        /// <summary>選択不可</summary>
        DeSelect,
        /// <summary>選択可</summary>
        Select,
        /// <summary>クリア済み</summary>
        Cleared,
        /// <summary>結合テスト済み</summary>
        ITFixed,
        /// <summary>ダミー</summary>
        Dummy = -1,
    }
}
