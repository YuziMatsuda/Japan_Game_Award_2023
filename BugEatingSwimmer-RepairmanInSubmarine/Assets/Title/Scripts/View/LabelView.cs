using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Title.View
{
    /// <summary>
    /// ビュー
    /// ラベルテキスト
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class LabelView : MonoBehaviour
    {
        /// <summary>フォント</summary>
        [SerializeField] private Text _text;
        /// <summary>フォントスタイル（選択）</summary>
        [SerializeField] private FontStyle fontStyleSelected = FontStyle.Bold;
        /// <summary>フォントスタイル（非選択）</summary>
        [SerializeField] private FontStyle fontStyleDeSelected = FontStyle.Normal;

        private void Reset()
        {
            _text = GetComponent<Text>();
        }

        /// <summary>
        /// フォントスタイルを変更
        /// </summary>
        /// <param name="fontStyle">フォントスタイル</param>
        /// <returns>成功／失敗</returns>
        public bool SetFontStyleSelected()
        {
            try
            {
                UpdateFontStyle(fontStyleSelected);

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// フォントスタイルを変更
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool SetFontStyleDeSelected()
        {
            try
            {
                UpdateFontStyle(fontStyleDeSelected);

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// フォントスタイルを更新
        /// </summary>
        /// <param name="fontStyle">フォントスタイル</param>
        private void UpdateFontStyle(FontStyle fontStyle)
        {
            _text.fontStyle = fontStyle;
        }
    }
}
