using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Area.Common;
using DG.Tweening;

namespace Area.View
{
    /// <summary>
    /// エンディング
    /// ビュー
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class EndingView : MonoBehaviour, IEndingView, IFadeImageView
    {
        [SerializeField] private float[] durations = { 5.0f, 2.0f };
        [SerializeField] private CanvasGroup canvasGroup;

        private void Reset()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public IEnumerator PlayEndingCut(System.IObserver<int> observer)
        {
            yield return new WaitForSeconds(durations[0]);
            observer.OnNext(1);
        }

        public IEnumerator PlayFadeAnimation(System.IObserver<bool> observer, EnumFadeState state)
        {
            canvasGroup.DOFade(endValue: state.Equals(EnumFadeState.Open) ? 1f : 0f, durations[1])
                .SetUpdate(true)
                .OnComplete(() => observer.OnNext(true));
            yield return null;
        }

        public IEnumerator PlayFadeAnimationOfRecollection(System.IObserver<bool> observer, EnumFadeState state)
        {
            throw new System.NotImplementedException();
        }

        public bool SetAlpha(EnumFadeState state)
        {
            throw new System.NotImplementedException();
        }
    }

    /// <summary>
    /// エンディング
    /// ビュー
    /// インターフェース
    /// </summary>
    public interface IEndingView
    {
        /// <summary>
        /// エンディングのカットを再生
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <returns>成功／失敗</returns>
        public IEnumerator PlayEndingCut(System.IObserver<int> observer);
    }
}
