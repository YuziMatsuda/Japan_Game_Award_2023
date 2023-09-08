using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;
using Main.Common;

namespace Main.View
{
    /// <summary>
    /// ルール貝スプライト
    /// ビュー
    /// </summary>
    public class BodySpriteRuleShellfish : BodySprite, IBodySpriteRuleShellfish
    {
        /// <summary>
        /// ルール貝の画像
        /// [0] 閉じた状態
        /// [1] 開いた状態
        /// </summary>
        [SerializeField] private Sprite[] sprites;
        /// <summary>表示状態か</summary>
        public bool IsVisible => GetComponent<SpriteRenderer>().color == new Color(1f, 1f, 1f, 1f);
        /// <summary>遅延実行時間</summary>
        [SerializeField] private float delayDuration = 1.5f;

        private void Reset()
        {
            fadeDuration = 1f;
        }

        public IEnumerator PlayChangeSpriteCloseBetweenOpen(System.IObserver<bool> observer)
        {
            DOTween.To(() => 0, x => GetComponent<SpriteRenderer>().sprite = sprites[x], 1, fadeDuration)
                .OnComplete(() =>
                {
                    if (!MainGameManager.Instance.ParticleSystemsOwner.PlayParticleSystems(GetInstanceID(), EnumParticleSystemsIndex.ParticleJigglyBubbleSoapy, transform.position))
                        throw new System.Exception("指定されたパーティクルシステムを再生する呼び出しの失敗");
                    // 泡を出すSE
                    MainGameManager.Instance.AudioOwner.PlaySFX(Audio.ClipToPlay.se_swim);
                    DOVirtual.DelayedCall(delayDuration, () => observer.OnNext(true));
                });

            yield return null;
        }

        public bool SetColorSpriteIsVisible(bool isVisible)
        {
            try
            {
                if (isVisible)
                {
                    var c = new Color(1f, 1f, 1f, 1f);
                    if (!SetColorSpriteRenderer(c))
                        throw new System.Exception("カラーを設定呼び出しの失敗");
                }
                else
                {
                    var c = new Color(1f, 1f, 1f, 0f);
                    if (!SetColorSpriteRenderer(c))
                        throw new System.Exception("カラーを設定呼び出しの失敗");
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
                return false;
            }
        }

        public IEnumerator PlayChangeSpriteOpenBetweenClose(System.IObserver<bool> observer)
        {
            if (!GetComponent<SpriteRenderer>().sprite.Equals(sprites[0]))
            {
                DOTween.To(() => 1, x => GetComponent<SpriteRenderer>().sprite = sprites[x], 0, fadeDuration)
                    .OnComplete(() => observer.OnNext(true));
            }
            else
                // 既に閉じている場合は発行するのみ
                observer.OnNext(true);

            yield return null;
        }
    }

    /// <summary>
    /// ルール貝スプライト
    /// ビュー
    /// </summary>
    public interface IBodySpriteRuleShellfish
    {
        /// <summary>
        /// スプライトの表示／非表示設定
        /// </summary>
        /// <param name="isVisible">表示させるか</param>
        /// <returns>成功／失敗</returns>
        public bool SetColorSpriteIsVisible(bool isVisible);
        /// <summary>
        /// スプライトを閉じた状態から開く状態へ切り替えるアニメーションを再生
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayChangeSpriteCloseBetweenOpen(System.IObserver<bool> observer);
        /// <summary>
        /// スプライトを開いた状態から閉じる状態へ切り替えるアニメーションを再生
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayChangeSpriteOpenBetweenClose(System.IObserver<bool> observer);
    }
}
