using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Title.View
{
    /// <summary>
    /// キャンバスのフェードコントローラー
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasFadeController : MonoBehaviour
    {
        /// <summary>イメージ</summary>
        [SerializeField] protected CanvasGroup canvasGroup;
        /// <summary>透明度</summary>
        [SerializeField] protected float fadeValue = .5f;
        /// <summary>デフォルト選択させるかフラグ</summary>
        [SerializeField] protected bool defaultSelectedGameObject;

        private void Reset()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        protected virtual void OnEnable()
        {
            canvasGroup.alpha = defaultSelectedGameObject ? 1f : fadeValue;
        }
    }
}
