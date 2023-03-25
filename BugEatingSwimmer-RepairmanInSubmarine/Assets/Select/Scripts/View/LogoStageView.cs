using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Select.View
{
    /// <summary>
    /// ビュー
    /// ロゴステージ
    /// </summary>
    public class LogoStageView : MonoBehaviour
    {
        /// <summary>
        /// T.B.D 選択不可マークを表示
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool RenderDisableMark()
        {
            try
            {
                Debug.Log("選択不可");
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// T.B.D クリア済みマークを表示
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool RenderClearMark()
        {
            try
            {
                Debug.Log("クリア済み");
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }
    }
}
