using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Select.Common
{
    /// <summary>
    /// モデル
    /// 親
    /// インターフェース
    /// </summary>
    public interface ISelectContentsModelParent
    {
        /// <summary>
        /// ボタンのステータスを変更
        /// </summary>
        /// <param name="enabled">有効／無効</param>
        /// <returns>成功／失敗</returns>
        public bool SetButtonEnabled(bool enabled);

        /// <summary>
        /// イベントトリガーのステータスを変更
        /// </summary>
        /// <param name="enabled">有効／無効</param>
        /// <returns>成功／失敗</returns>
        public bool SetEventTriggerEnabled(bool enabled);
    }
}
