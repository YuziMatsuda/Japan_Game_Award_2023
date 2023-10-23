using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Select.Model;

namespace Select.View
{
    /// <summary>
    /// ビュー
    /// ページ
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class PageView : MonoBehaviour, IPageView
    {
        /// <summary>キャンバスグループ</summary>
        [SerializeField] private CanvasGroup canvasGroup;
        /// <summary>アクティブなステージか（対象ページか）</summary>
        public bool IsVisibled => canvasGroup.alpha == 1f;
        /// <summary>ロゴステージのビュー</summary>
        [SerializeField] private LogoStageView[] logoStageViews;

        private void Reset()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            logoStageViews = GetComponentsInChildren<LogoStageView>();
        }

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

        public bool SetBlocksRaycasts(bool isEnabled)
        {
            try
            {
                canvasGroup.blocksRaycasts = isEnabled;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool PlayRenderDisableMarkOfCurrentStages()
        {
            try
            {
                foreach (var item in logoStageViews.Where(q => q.transform.GetComponent<LogoStageModel>().IsPlayDirection)
                    .Select(q => q))
                    if (!item.PlayRenderDisableMark())
                        Debug.LogError("選択不可マーク表示呼び出しの失敗");

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
    /// ページ
    /// インターフェース
    /// </summary>
    public interface IPageView
    {
        /// <summary>
        /// アルファ値を設定
        /// </summary>
        /// <param name="isEnabled">有効か</param>
        /// <returns>成功／失敗</returns>
        public bool SetVisible(bool isEnabled);
        /// <summary>
        /// レイキャスト判定対象を設定
        /// </summary>
        /// <param name="alpha">有効か</param>
        /// <returns>成功／失敗</returns>
        public bool SetBlocksRaycasts(bool isEnabled);
        /// <summary>
        /// 選択不可マークを表示演出を再生
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool PlayRenderDisableMarkOfCurrentStages();
    }
}
