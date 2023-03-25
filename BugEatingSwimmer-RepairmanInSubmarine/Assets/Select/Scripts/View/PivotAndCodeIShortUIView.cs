using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Select.View
{
    /// <summary>
    /// ビュー
    /// 支点とコード
    /// </summary>
    public class PivotAndCodeIShortUIView : MonoBehaviour
    {
        public bool RenderClearMark()
        {
            throw new System.NotImplementedException();
        }

        public bool RenderDisableMark()
        {
            throw new System.NotImplementedException();
        }
    }

    /// <summary>
    /// ビュー
    /// 支点とコード
    /// インターフェース
    /// </summary>
    public interface IPivotAndCodeIShortUIView
    {
        /// <summary>
        /// T.B.D 選択不可マークを表示
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool RenderDisableMark();

        /// <summary>
        /// T.B.D クリア済みマークを表示
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool RenderClearMark();
    }
}
