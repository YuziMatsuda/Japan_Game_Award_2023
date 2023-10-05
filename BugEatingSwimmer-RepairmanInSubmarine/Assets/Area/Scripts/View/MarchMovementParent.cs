using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Area.View
{
    /// <summary>
    /// マーチ移動制御の親
    /// ビュー
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class MarchMovementParent : MonoBehaviour
    {
        /// <summary>トランスフォーム</summary>
        protected Transform _transform;
        /// <summary>トランスフォームUI</summary>
        protected RectTransform _rectTransform;
        /// <summary>移動する距離</summary>
        [SerializeField] protected float marchDistance = -21.86f;
        /// <summary>演出</summary>
        [SerializeField] protected float[] durations = { 12.5f };

        /// <summary>
        /// マーチを再生
        /// Animationのトリガー検知
        /// </summary>
        protected virtual void PlayMarching() { }
    }
}
