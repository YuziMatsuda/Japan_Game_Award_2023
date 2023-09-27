using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Area.Common;

namespace Area.View
{
    /// <summary>
    /// キャスト
    /// ビュー
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class CastsView : CastsParent
    {
        /// <summary>配下の名前オブジェクト</summary>
        [SerializeField] private RectTransform castName;
        /// <summary>配下の説明オブジェクト</summary>
        [SerializeField] private RectTransform castComment;
        /// <summary>パーティクル位置の補正</summary>
        [SerializeField] private Vector3[] offsetPositions = { Vector3.zero, new Vector3(4.912f, -0.729f, 0f), };

        private void Reset()
        {
            castName = transform.GetChild(0) as RectTransform;
            castComment = transform.GetChild(1) as RectTransform;
        }

        protected override void OnChangeText()
        {
            if (!AreaGameManager.Instance.ParticleSystemsOwner.PlayParticleSystems(castName.GetInstanceID(), EnumParticleSystemsIndex.ParticleJigglyBubbleSoapy, castName.position + offsetPositions[0]))
                Debug.LogError("指定されたパーティクルシステムを再生する呼び出しの失敗");
            if (!AreaGameManager.Instance.ParticleSystemsOwner.PlayParticleSystems(castComment.GetInstanceID(), EnumParticleSystemsIndex.ParticleJigglyBubbleSoapy, castComment.position + offsetPositions[1]))
                Debug.LogError("指定されたパーティクルシステムを再生する呼び出しの失敗");
        }
    }
}
