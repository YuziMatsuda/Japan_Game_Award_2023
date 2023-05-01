using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Area.Common
{
    /// <summary>
    /// エリア解放・結合テストのセーブファイル
    /// </summary>
    public enum EnumAreaOpenedAndITState
    {
        /// <summary>エリアID</summary>
        UnitID,
        /// <summary>ステージのステータス（0：選択不可、1：選択可、2：クリア済み、3：結合テスト済み、-1：ダミー）</summary>
        State,
    }
}
