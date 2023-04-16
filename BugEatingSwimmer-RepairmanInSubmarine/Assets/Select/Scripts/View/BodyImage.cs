using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Select.View
{
    /// <summary>
    /// ボディのスプライト
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class BodyImage : MonoBehaviour, IBodyImage
    {
        /// <summary>フェードアニメーション時間</summary>
        [SerializeField] private float fadeDuration = .5f;

        public IEnumerator PlayFadeAnimation(IObserver<bool> observer)
        {
            GetComponent<Image>().DOFade(endValue: 0f, fadeDuration)
                .SetUpdate(true)
                .OnComplete(() => observer.OnNext(true));
            yield return null;
        }

        public bool SetColorSpriteRenderer(Color color)
        {
            try
            {
                GetComponent<Image>().color = color;

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
    /// ボディのスプライト
    /// インターフェース
    /// </summary>
    public interface IBodyImage
    {
        /// <summary>
        /// カラーを設定
        /// </summary>
        /// <param name="color">カラー情報</param>
        /// <returns>成功／失敗</returns>
        public bool SetColorSpriteRenderer(Color color);

        /// <summary>
        /// フェードのDOTweenアニメーション再生
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayFadeAnimation(System.IObserver<bool> observer);
    }
}
