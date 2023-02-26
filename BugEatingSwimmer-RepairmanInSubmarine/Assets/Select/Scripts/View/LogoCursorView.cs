using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Select.View
{
    /// <summary>
    /// ビュー
    /// ロゴカーソル
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class LogoCursorView : MonoBehaviour
    {
        /// <summary>イメージ</summary>
        [SerializeField] private Image image;

        private void Reset()
        {
            image = GetComponent<Image>();
        }

        /// <summary>
        /// イメージのステータスを変更
        /// </summary>
        /// <param name="isEnabled">有効／無効</param>
        /// <returns>成功／失敗</returns>
        public bool SetImageEnabled(bool isEnabled)
        {
            try
            {
                image.enabled = isEnabled;
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
