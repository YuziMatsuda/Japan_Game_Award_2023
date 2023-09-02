using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Select.Common;
using UniRx;
using Select.Template;

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
        /// <summary>イメージ</summary>
        [SerializeField] private Image image;
        /// <summary>背景フレームイメージ</summary>
        [SerializeField] private FadeImageView backgroundFrame;

        private void Reset()
        {
            bodyText = GetComponentInChildren<BodyText>();
            stageStatus = GetComponentInChildren<StageStatus>();
            bodyImage = GetComponentInChildren<BodyImage>();
            image = GetComponent<Image>();
            backgroundFrame = GetComponentInChildren<FadeImageView>();
        }

        public bool RenderDisableMark()
        {
            try
            {
                if (!bodyText.SetTextEnabled(false))
                    throw new System.Exception("テキストのステータスを変更呼び出しの失敗");
                if (!stageStatus.SetTextEnabled(false))
                    throw new System.Exception("テキストのステータスを変更呼び出しの失敗");
                if (!SetRaycastTarget(false))
                    throw new System.Exception("レイキャストの接触可否を変更呼び出しの失敗");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// レイキャストの接触可否を変更
        /// </summary>
        /// <param name="isEnabled">有効／無効</param>
        /// <returns>有効／無効</returns>
        private bool SetRaycastTarget(bool isEnabled)
        {
            try
            {
                image.raycastTarget = isEnabled;

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

        public IEnumerator PlayRenderEnableBackgroundFrame(System.IObserver<bool> observer)
        {
            Observable.FromCoroutine<bool>(observer => backgroundFrame.PlayFadeAnimation(observer, EnumFadeState.Close))
                .Subscribe(_ => observer.OnNext(true))
                .AddTo(gameObject);
            yield return null;
        }

        public IEnumerator PlayRenderDisableBackgroundFrame(System.IObserver<bool> observer)
        {
            Observable.FromCoroutine<bool>(observer => backgroundFrame.PlayFadeAnimation(observer, EnumFadeState.Open))
                .Subscribe(_ => observer.OnNext(true))
                .AddTo(gameObject);
            yield return null;
        }

        public bool SetRenderEnableBackgroundFrame()
        {
            try
            {
                if (!backgroundFrame.SetColorSpriteRenderer(EnumFadeState.Close))
                    throw new System.Exception("カラーを設定呼び出しの失敗");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetRenderDisableBackgroundFrame()
        {
            try
            {
                if (!backgroundFrame.SetColorSpriteRenderer(EnumFadeState.Open))
                    throw new System.Exception("カラーを設定呼び出しの失敗");

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
    public interface ILogoStageView : ISelectContentsViewParent
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
