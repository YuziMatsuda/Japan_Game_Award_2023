using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;
using Area.Common;

namespace Area.View
{
    /// <summary>
    /// ビュー
    /// フェードイメージ
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class FadeImageView : MonoBehaviour, IFadeImageView
    {
        /// <summary>終了時間</summary>
        [SerializeField] private float duration = 2.0f;
        /// <summary>イメージ</summary>
        [SerializeField] private Image image;
        /// <summary>イージング</summary>
        [SerializeField] private Ease[] eases;

        private void Reset()
        {
            image = GetComponent<Image>();
        }

        public IEnumerator PlayFadeAnimation(System.IObserver<bool> observer, EnumFadeState state)
        {
            image.DOFade(endValue: state.Equals(EnumFadeState.Open) ? 0f : 1f, duration)
                .SetUpdate(true)
                .OnComplete(() => observer.OnNext(true));
            yield return null;
        }

        public IEnumerator PlayFadeAnimationOfRecollection(System.IObserver<bool> observer, EnumFadeState state)
        {
            image.DOFade(endValue: state.Equals(EnumFadeState.Open) ? 1f : 0f, duration)
                .SetUpdate(true)
                .SetEase(eases[0])
                .OnComplete(() => observer.OnNext(true));

            yield return null;
        }

        public bool SetAlpha(EnumFadeState state)
        {
            try
            {
                var color = image.color;
                color.a = state.Equals(EnumFadeState.Open) ? 1f : 0f;
                image.color = color;

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
    /// フェードイメージ
    /// インターフェース
    /// </summary>
    public interface IFadeImageView
    {
        /// <summary>
        /// フェードのDOTweenアニメーション再生
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <param name="state">ステータス</param>
        /// <returns>成功／失敗</returns>
        public IEnumerator PlayFadeAnimation(System.IObserver<bool> observer, EnumFadeState state);
        /// <summary>
        /// フェードのDOTweenアニメーション再生
        /// 回想シーン用
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <param name="state">ステータス</param>
        /// <returns>成功／失敗</returns>
        public IEnumerator PlayFadeAnimationOfRecollection(System.IObserver<bool> observer, EnumFadeState state);
        /// <summary>
        /// アルファ値をセット
        /// </summary>
        /// <param name="state">ステータス</param>
        /// <returns>成功／失敗</returns>
        public bool SetAlpha(EnumFadeState state);
    }
}
