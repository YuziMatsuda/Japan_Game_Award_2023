using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Main.View
{
    /// <summary>
    /// テキストメッセージ
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class TextMessage : MonoBehaviour, ITextMessage
    {
        /// <summary>テキスト</summary>
        [SerializeField] private Text text;
        /// <summary>設定</summary>
        [SerializeField] private TextConfig textConfig;

        public bool SetMessage(int index)
        {
            try
            {
                text.text = textConfig.Messages[index].text;
                text.fontSize = textConfig.Messages[index].fontSize;
                text.color = textConfig.Messages[index].color;

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
            textConfig = GetComponent<TextConfig>();
        }
    }

    /// <summary>
    /// テキストメッセージ
    /// インターフェース
    /// </summary>
    public interface ITextMessage
    {
        /// <summary>メッセージをセット</summary>
        /// <param name="index">メッセージ番号</param>
        /// <returns>成功／失敗</returns>
        public bool SetMessage(int index);
    }
}
