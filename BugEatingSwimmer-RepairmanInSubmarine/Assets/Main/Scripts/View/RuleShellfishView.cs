using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Common;
using UniRx;

namespace Main.View
{
    /// <summary>
    /// ルール貝
    /// ビュー
    /// </summary>
    public class RuleShellfishView : MonoBehaviour, IBodySpriteRuleShellfish, IRuleShellfishView
    {
        /// <summary>ルール貝スプライト</summary>
        [SerializeField] private BodySpriteRuleShellfish bodySpriteRuleShellfish;
        /// <summary>表示状態か</summary>
        public bool IsVisible => bodySpriteRuleShellfish.IsVisible;
        /// <summary>吹き出しスプライト</summary>
        [SerializeField] private BodySprite bodySprite;
        /// <summary>表示</summary>
        [SerializeField] private Color visibledColorOfChild = new Color(1f, 1f, 1f, 1f);
        /// <summary>非表示</summary>
        [SerializeField] private Color disabledColorOfChild = new Color(1f, 1f, 1f, 0f);

        public bool InstanceBubble()
        {
            try
            {
                if (!MainGameManager.Instance.ParticleSystemsOwner.PlayParticleSystems(GetInstanceID(), EnumParticleSystemsIndex.ParticleJigglyBubbleSoapy, transform.position))
                    throw new System.Exception("指定されたパーティクルシステムを再生する呼び出しの失敗");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public IEnumerator PlayChangeSpriteCloseBetweenOpen(System.IObserver<bool> observer)
        {
            if (!SetColorSpriteRendererSpeechBubble(false))
                Debug.LogError("吹き出しスプライトの表示／非表示呼び出しの失敗");
            Observable.FromCoroutine<bool>(observer => bodySpriteRuleShellfish.PlayChangeSpriteCloseBetweenOpen(observer))
                .Subscribe(_ => observer.OnNext(true))
                .AddTo(gameObject);

            yield return null;
        }

        public IEnumerator PlayChangeSpriteOpenBetweenClose(System.IObserver<bool> observer)
        {
            Observable.FromCoroutine<bool>(observer => bodySpriteRuleShellfish.PlayChangeSpriteOpenBetweenClose(observer))
                .Subscribe(_ =>
                {
                    observer.OnNext(true);
                    if (!SetColorSpriteRendererSpeechBubble(true))
                        Debug.LogError("吹き出しスプライトの表示／非表示呼び出しの失敗");
                })
                .AddTo(gameObject);

            yield return null;
        }

        public bool SetColorSpriteIsVisible(bool isVisible)
        {
            if (!SetColorSpriteRendererSpeechBubble(isVisible))
                Debug.LogError("吹き出しスプライトの表示／非表示呼び出しの失敗");
            return ((IBodySpriteRuleShellfish)bodySpriteRuleShellfish).SetColorSpriteIsVisible(isVisible);
        }

        private void Reset()
        {
            bodySpriteRuleShellfish = GetComponentInChildren<BodySpriteRuleShellfish>();
            bodySprite = transform.GetChild(1).GetComponent<BodySprite>();
        }

        /// <summary>
        /// 吹き出しスプライトの表示／非表示
        /// </summary>
        /// <param name="isEnabled">表示／非表示</param>
        /// <returns>成功／失敗</returns>
        private bool SetColorSpriteRendererSpeechBubble(bool isEnabled)
        {
            return bodySprite.SetColorSpriteRenderer(isEnabled ? visibledColorOfChild : disabledColorOfChild);
        }
    }

    /// <summary>
    /// ルール貝
    /// ビュー
    /// インターフェース
    /// </summary>
    public interface IRuleShellfishView
    {
        /// <summary>
        /// 泡を生成
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool InstanceBubble();
    }
}
