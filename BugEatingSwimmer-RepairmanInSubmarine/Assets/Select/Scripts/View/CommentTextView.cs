using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Select.View
{
    /// <summary>
    /// ビュー
    /// コメント用テキスト
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class CommentTextView : MonoBehaviour
    {
        /// <summary>テキスト</summary>
        [SerializeField] private Text text;
        /// <summary>テキストの有効／無効</summary>
        public bool TextIsActiveAndEnabled => text.isActiveAndEnabled;

        private void Reset()
        {
            text = GetComponent<Text>();
        }

        /// <summary>
        /// テキストのステータスを変更
        /// </summary>
        /// <param name="isEnabled">有効／無効</param>
        /// <returns>成功／失敗</returns>
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
    }
}
