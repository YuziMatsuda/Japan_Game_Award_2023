using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Title.Common
{
    /// <summary>
    /// 準委任帳票のカラム
    /// </summary>
    public enum EnumQuasiAssignmentForm
    {
        /// <summary>ステージクリア条件のセーブファイルのインデックス</summary>
        MainSceneStagesModulesStateIndex,
        /// <summary>ヒトデ管理ID</summary>
        SeastarID,
        /// <summary>ヒトデが配属済みか</summary>
        Assigned,
        /// <summary>ヒトデが配属済みか（デフォルト）</summary>
        AssignedDefault,
    }
}
