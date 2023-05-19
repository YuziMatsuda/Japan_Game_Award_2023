using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// ビュー
    /// ステージクリアロゴ
    /// </summary>
    public class StageClearView : MonoBehaviour, IStageClearView
    {
        /// <summary>テキストメッセージ</summary>
        [SerializeField] private TextMessage textMessage;

        public bool SetMessageCongratulations()
        {
            return textMessage.SetMessage(0);
        }

        private void Reset()
        {
            textMessage = GetComponentInChildren<TextMessage>();
        }
    }

    /// <summary>
    /// ビュー
    /// ステージクリアロゴ
    /// インターフェース
    /// </summary>
    public interface IStageClearView
    {
        /// <summary>最終ステージ用のメッセージをセット</summary>
        /// <returns>成功／失敗</returns>
        public bool SetMessageCongratulations();
    }
}
