using Main.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Common
{
    /// <summary>
    /// アルゴリズムの共通処理
    /// </summary>
    public class AlgorithmCommon
    {
        /// <summary>
        /// ※※※ 原子、分子関係なく常に3を返却 ※※※
        /// 可能性カウンターを取得
        /// 原子なら4つの分岐から「ゴールノード」を除いた3つの分岐となる想定
        /// 分子なら通常は1つ
        /// </summary>
        /// <param name="nodeCode">ノードコード</param>
        /// <returns>分岐数</returns>
        public int GetPossibilityCnt(Transform nodeCode)
        {
            if (nodeCode == null ||
                (nodeCode != null && nodeCode.GetComponent<PivotConfig>() == null))
                return -1;

            return 3;
        }

        /// <summary>
        /// 方角モード変換してを取得
        /// 限界値を超えた値はアジャストする
        /// </summary>
        /// <param name="enumDirectionMode">方角モード</param>
        /// <param name="addend">足す値</param>
        /// <returns>方角モード（正常値）</returns>
        public EnumDirectionMode GetAjustedEnumDirectionMode(EnumDirectionMode enumDirectionMode, int addend)
        {
            return (EnumDirectionMode)(((int)enumDirectionMode + addend) % 4);
        }
    }
}
