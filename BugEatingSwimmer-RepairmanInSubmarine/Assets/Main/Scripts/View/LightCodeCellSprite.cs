using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Main.Common;

namespace Main.View
{
    /// <summary>
    /// コード（明）
    /// 個々の制御
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class LightCodeCellSprite : ShadowCodeCellParent, IShadowCodeCell
    {
        /// <summary>スプライト</summary>
        private SpriteRenderer _spriteRenderer;
        /// <summary>アニメーション終了時間</summary>
        [SerializeField] private float duration = .05f;
        /// <summary>フェード値（暗）</summary>
        [SerializeField] private float offFade = 0f;
        /// <summary>フェード値（明）</summary>
        [SerializeField] private float onFade = 1f;

        public bool InitializeLight(EnumDirectionMode enumDirectionMode)
        {
            try
            {
                if (_spriteRenderer == null)
                    _spriteRenderer = GetComponent<SpriteRenderer>();
                var color = _spriteRenderer.color;
                color.a = onFade * 255f;
                _spriteRenderer.color = color;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public IEnumerator PlayLightAnimation(IObserver<bool> observer, EnumDirectionMode enumDirectionMode)
        {
            if (_spriteRenderer == null)
                _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.DOFade(endValue: onFade, duration)
                .SetUpdate(true)
                .OnComplete(() => observer.OnNext(true));

            yield return null;
        }

        public IEnumerator PlaySpinAnimation(IObserver<bool> observer, Vector3 vectorDirectionMode)
        {
            throw new NotImplementedException();
        }

        public bool SetAlphaOff()
        {
            try
            {
                if (_spriteRenderer == null)
                    _spriteRenderer = GetComponent<SpriteRenderer>();
                var color = _spriteRenderer.color;
                color.a = offFade;
                _spriteRenderer.color = color;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetSpinDirection(Vector3 vectorDirectionMode)
        {
            throw new NotImplementedException();
        }
    }
}
