using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Main.Common;

namespace Main.View
{
    /// <summary>
    /// ビュー
    /// 遊び方確認スクロール画面
    /// </summary>
    [RequireComponent(typeof(ScrollRect))]
    public class GameManualScrollView : MonoBehaviour
    {
        /// <summary>ページスクロールのインデックス</summary>
        [SerializeField] private float[] pagesPos = { 0f, .3325f, .666f, 1f };
        /// <summary>スクロール制御</summary>
        [SerializeField] private ScrollRect scrollRect;
        /// <summary>アニメーション再生時間</summary>
        [SerializeField] private float duration = .1f;
        /// <summary>閉じるまでの時間</summary>
        [SerializeField] private float closedTime = .5f;

        private void Reset()
        {
            scrollRect = GetComponent<ScrollRect>();
        }

        /// <summary>
        /// フェードのDOTweenアニメーション再生
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <param name="state">ステータス</param>
        /// <returns>成功／失敗</returns>
        public IEnumerator PlayCloseAnimation(System.IObserver<bool> observer)
        {
            DOVirtual.DelayedCall(closedTime, () =>
            {
                observer.OnNext(true);
            });
            yield return null;
        }

        /// <summary>
        /// ページ位置の変更
        /// </summary>
        /// <param name="pageIndex">ページ番号</param>
        /// <returns>成功／失敗</returns>
        public bool SetPage(EnumGameManualPagesIndex pageIndex)
        {
            try
            {
                scrollRect.horizontalNormalizedPosition = pagesPos[(int)pageIndex];
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// ページングアニメーション
        /// </summary>
        /// <param name="pageIndex">ページ番号</param>
        /// <returns>成功／失敗</returns>
        public bool PlayPagingAnimation(EnumGameManualPagesIndex pageIndex)
        {
            try
            {
                scrollRect.DOHorizontalNormalizedPos(pagesPos[(int)pageIndex], duration)
                    .SetUpdate(true);
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
