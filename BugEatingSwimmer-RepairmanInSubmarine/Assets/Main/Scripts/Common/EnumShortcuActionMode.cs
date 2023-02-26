using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Common
{
    /// <summary>
    /// ショートカット入力
    /// HierarchyにあるShortcuGuideScreen > BackScreen内のゲームオブジェクトを配列した場合の順番と揃える
    ///     [0]GameUndoLabel    ★
    ///     [1]GameSelectLabel  ★
    ///     [2]GameCheckLabel   ★
    /// </summary>
    public enum EnumShortcuActionMode
    {
        /// <summary>ステージをやり直す</summary>
        UndoAction,
        /// <summary>他のステージを選ぶ</summary>
        SelectAction,
        /// <summary>遊び方の確認</summary>
        CheckAction,
        /// <summary>未実施</summary>
        None = -1,
    }
}
