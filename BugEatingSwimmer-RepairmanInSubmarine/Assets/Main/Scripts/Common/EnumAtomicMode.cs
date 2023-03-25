using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Common
{
    /// <summary>
    /// アトミックモード
    /// 一つの要素か複数の要素を含むか
    /// （例） 支点のみ　⇒　Atoms
    ///        支点＋コード　⇒　Molecules
    /// </summary>
    public enum EnumAtomicMode
    {
        /// <summary>原子</summary>
        Atoms,
        /// <summary>分子</summary>
        Molecules,
    }
}
