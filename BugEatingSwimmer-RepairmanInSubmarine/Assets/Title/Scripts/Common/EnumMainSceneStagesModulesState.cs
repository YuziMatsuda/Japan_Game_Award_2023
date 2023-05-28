using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Title.Common
{
    /// <summary>
    /// ステージクリア条件
    /// </summary>
    public enum EnumMainSceneStagesModulesState
    {
        /// <summary>ステージクリア条件ID</summary>
        PartId,
        /// <summary>ステージ番号</summary>
        SceneId,
        /// <summary>クリア条件の組み合わせ</summary>
        Terms,
        /// <summary>クリア状態</summary>
        Fixed,
    }
}
