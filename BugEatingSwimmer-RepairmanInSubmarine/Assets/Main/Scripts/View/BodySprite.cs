using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Main.View
{
    /// <summary>
    /// ボディのスプライト
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class BodySprite : MonoBehaviour, IBodySprite
    {
        /// <summary>フェードアニメーション時間</summary>
        [SerializeField] protected float fadeDuration = .5f;

        public IEnumerator PlayFadeAnimation(IObserver<bool> observer)
        {
            GetComponent<SpriteRenderer>().DOFade(endValue: 0f, fadeDuration)
                .SetUpdate(true)
                .OnComplete(() => observer.OnNext(true));
            yield return null;
        }

        public bool SetColorSpriteRenderer(Color color)
        {
            try
            {
                GetComponent<SpriteRenderer>().color = color;

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
    public interface IBodySprite
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
