using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;
using Select.Common;

namespace Select.View
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

        public bool SetColorSpriteRenderer(EnumFadeState state)
        {
            try
            {
                switch (state)
                {
                    case EnumFadeState.Open:
                        var io = image.color;
                        io.a = 0f;
                        image.color = io;

                        break;
                    case EnumFadeState.Default:
                        throw new System.Exception("例外エラー");
                    case EnumFadeState.Close:
                        var ic = image.color;
                        ic.a = 1f;
                        image.color = ic;

                        break;
                    default:
                        throw new System.Exception("例外エラー");
                }

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
        /// <returns>コルーチン</returns>
        public IEnumerator PlayFadeAnimation(System.IObserver<bool> observer, EnumFadeState state);
        /// <summary>
        /// カラーを設定
        /// </summary>
        /// <param name="state">ステータス</param>
        /// <returns>成功／失敗</returns>
        public bool SetColorSpriteRenderer(EnumFadeState state);
    }
}
