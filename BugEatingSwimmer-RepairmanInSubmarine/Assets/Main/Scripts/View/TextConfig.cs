using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// テキストメッセージの設定
    /// </summary>
    public class TextConfig : MonoBehaviour
    {
        /// <summary>メッセージ配列</summary>
        [SerializeField] private Message[] messages;
        /// <summary>メッセージ配列</summary>
        public Message[] Messages => messages;
    }

    /// <summary>
    /// メッセージ構造体
    /// </summary>
    [System.Serializable]
    public struct Message
    {
        /// <summary>テキスト</summary>
        public string text;
        /// <summary>フォントサイズ</summary>
        public int fontSize;
        /// <summary>カラー</summary>
        public Color color;
    }
}
