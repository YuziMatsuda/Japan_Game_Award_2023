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
            Observable.FromCoroutine<bool>(observer => bodySpriteRuleShellfish.PlayChangeSpriteCloseBetweenOpen(observer))
                .Subscribe(_ => observer.OnNext(true))
                .AddTo(gameObject);

            yield return null;
        }

        public IEnumerator PlayChangeSpriteOpenBetweenClose(System.IObserver<bool> observer)
        {
            Observable.FromCoroutine<bool>(observer => bodySpriteRuleShellfish.PlayChangeSpriteOpenBetweenClose(observer))
                .Subscribe(_ => observer.OnNext(true))
                .AddTo(gameObject);

            yield return null;
        }

        public bool SetColorSpriteIsVisible(bool isVisible)
        {
            return ((IBodySpriteRuleShellfish)bodySpriteRuleShellfish).SetColorSpriteIsVisible(isVisible);
        }

        private void Reset()
        {
            bodySpriteRuleShellfish = GetComponentInChildren<BodySpriteRuleShellfish>();
        }
    }

    /// <summary>
    /// 
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
