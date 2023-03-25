using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Common;
using DG.Tweening;

namespace Main.View
{
    /// <summary>
    /// ビュー
    /// ジャンプ操作ガイド
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class JumpGuideView : MonoBehaviour
    {
        /// <summary>終了時間</summary>
        [SerializeField] private float duration = .3f;
        /// <summary>キャンバスグループ</summary>
        [SerializeField] CanvasGroup canvasGroup;

        private void Reset()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        /// <summary>
        /// アルファ値をセット
        /// </summary>
        /// <param name="state">ステータス</param>
        /// <returns>成功／失敗</returns>
        public bool SetAlpha(EnumFadeState state)
        {
            try
            {
                canvasGroup.alpha = state.Equals(EnumFadeState.Open) ? 1f : 0f;
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// フェードのDOTweenアニメーション再生
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <param name="state">ステータス</param>
        /// <returns>成功／失敗</returns>
        public IEnumerator PlayFadeAnimation(System.IObserver<bool> observer, EnumFadeState state)
        {
            canvasGroup.DOFade(endValue: state.Equals(EnumFadeState.Open) ? 1f : 0f, duration)
                .SetUpdate(true)
                .OnComplete(() => observer.OnNext(true));
            yield return null;
        }
    }
}
