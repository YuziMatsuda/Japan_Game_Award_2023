using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Select.Common
{
    /// <summary>
    /// ステージのステータス（0：選択不可、1：選択可、2：結合テスト済み、3：クリア済み、-1：ダミー）
    /// </summary>
    public enum EnumAreaOpenedAndITStateState
    {
        DeSelect,
        Select,
        ITFixed,
        Cleared,
        Dummy = -1,
    }
}
