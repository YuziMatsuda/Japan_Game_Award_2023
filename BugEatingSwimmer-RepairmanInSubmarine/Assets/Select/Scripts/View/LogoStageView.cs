using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Select.Common;

namespace Select.View
{
    /// <summary>
    /// ビュー
    /// ロゴステージ
    /// </summary>
    public class LogoStageView : MonoBehaviour, ILogoStageView
    {
        /// <summary>ボディのテキスト</summary>
        [SerializeField] private BodyText bodyText;
        /// <summary>ボディのテキスト配列</summary>
        [SerializeField] private StageStatus stageStatus;
        /// <summary>ボディのイメージ</summary>
        [SerializeField] private BodyImage bodyImage;

        private void Reset()
        {
            bodyText = GetComponentInChildren<BodyText>();
            stageStatus = GetComponentInChildren<StageStatus>();
            bodyImage = GetComponentInChildren<BodyImage>();
        }

        public bool RenderDisableMark()
        {
            try
            {
                if (!bodyText.SetTextEnabled(false))
                    throw new System.Exception("テキストのステータスを変更呼び出しの失敗");
                if (!stageStatus.SetTextEnabled(false))
                    throw new System.Exception("テキストのステータスを変更呼び出しの失敗");
                
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool RenderClearMark()
        {
            try
            {
                if (!stageStatus.SetTextMessage(EnumStageStatusMessage.Clear))
                    throw new System.Exception("テキストメッセージを表示呼び出しの失敗");
                if (!stageStatus.SetColorTextMessage(EnumStageStatusMessage.Clear))
                    throw new System.Exception("テキストメッセージカラーをセット呼び出しの失敗");
                if (!bodyImage.SetImageEnabled(false))
                    throw new System.Exception("イメージのステータスを変更呼び出しの失敗");
                
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool RenderEnabled()
        {
            try
            {
                if (!stageStatus.SetTextMessage(EnumStageStatusMessage.Error))
                    throw new System.Exception("テキストメッセージを表示呼び出しの失敗");
                if (!stageStatus.SetColorTextMessage(EnumStageStatusMessage.Error))
                    throw new System.Exception("テキストメッセージカラーをセット呼び出しの失敗");
                if (!bodyImage.SetImageEnabled(false))
                    throw new System.Exception("イメージのステータスを変更呼び出しの失敗");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }
    }

    /// <summary>
    /// ビュー
    /// ロゴステージ
    /// インターフェース
    /// </summary>
    public interface ILogoStageView
    {
        /// <summary>
        /// 選択不可マークを表示
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool RenderDisableMark();
        /// <summary>
        /// 選択可表示
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool RenderEnabled();
        /// <summary>
        /// クリア済みマークを表示
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool RenderClearMark();
    }
}
