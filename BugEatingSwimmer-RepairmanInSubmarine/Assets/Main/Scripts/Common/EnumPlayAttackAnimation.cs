using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Common
{
    /// <summary>
    /// ジョーシーの攻撃ステップ情報
    /// </summary>
    public enum EnumPlayAttackAnimation
    {
        /// <summary>Step_0:後退</summary>
        Backing,
        /// <summary>Step_1:突進</summary>
        Rushing,
        /// <summary>Step_2:戻る</summary>
        Returning,
        /// <summary>Step_3:終了</summary>
        OnCompleted,
    }
}
