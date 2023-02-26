using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Select.View
{
    /// <summary>
    /// ビュー
    /// ページ
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class PageView : MonoBehaviour
    {
        /// <summary>キャンバスグループ</summary>
        [SerializeField] private CanvasGroup canvasGroup;

        private void Reset()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        /// <summary>
        /// アルファ値を設定
        /// </summary>
        /// <param name="alpha">アルファ値</param>
        /// <returns>成功／失敗</returns>
        public bool SetVisible(bool isEnabled)
        {
            try
            {
                canvasGroup.alpha = isEnabled ? 1f : 0f;
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
