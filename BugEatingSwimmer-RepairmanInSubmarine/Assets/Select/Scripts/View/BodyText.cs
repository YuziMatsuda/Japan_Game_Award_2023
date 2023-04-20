using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Select.View
{
    /// <summary>
    /// ボディのテキスト
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class BodyText : MonoBehaviour, IBodyText
    {
        /// <summary>テキスト</summary>
        [SerializeField] protected Text text;

        public bool SetColorText(Color color)
        {
            try
            {
                text.color = color;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetTextEnabled(bool isEnabled)
        {
            try
            {
                text.enabled = isEnabled;
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        private void Reset()
        {
            text = GetComponent<Text>();
        }
    }

    /// <summary>
    /// ボディのテキスト
    /// インターフェース
    /// </summary>
    public interface IBodyText
    {
        /// <summary>
        /// テキストのステータスを変更
        /// </summary>
        /// <param name="isEnabled">有効／無効</param>
        /// <returns>成功／失敗</returns>
        public bool SetTextEnabled(bool isEnabled);
        /// <summary>
        /// テキストのカラーを変更
        /// </summary>
        /// <param name="isEnabled">有効／無効</param>
        /// <returns>成功／失敗</returns>
        public bool SetColorText(Color color);
    }
}
