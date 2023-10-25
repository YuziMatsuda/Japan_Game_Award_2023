using Main.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// 支点ライトの見た目
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class PivotDynamic : ShadowCodeCellParent, IShadowCodeCell
    {
        /// <summary>スプライト配列</summary>
        [SerializeField] private Sprite[] sprites;
        /// <summary>スプライトレンダラー</summary>
        private SpriteRenderer _spriteRenderer;

        public bool InitializeLight(EnumDirectionMode enumDirectionMode)
        {
            throw new NotImplementedException();
        }

        public IEnumerator PlayErrorLightFlashAnimation(IObserver<bool> observer)
        {
            throw new NotImplementedException();
        }

        public IEnumerator PlayLightAnimation(IObserver<bool> observer, EnumDirectionMode enumDirectionMode, bool restartMode)
        {
            throw new NotImplementedException();
        }

        public IEnumerator PlayLockSpinAnimation(IObserver<bool> observer)
        {
            throw new NotImplementedException();
        }

        public IEnumerator PlaySpinAnimation(IObserver<bool> observer, Vector3 vectorDirectionMode, bool restartMode)
        {
            throw new NotImplementedException();
        }

        public bool SetAlphaOff()
        {
            throw new NotImplementedException();
        }

        public bool SetDefaultDirection()
        {
            throw new NotImplementedException();
        }

        public bool SetSpinDirection(Vector3 vectorDirectionMode)
        {
            throw new NotImplementedException();
        }

        public bool SetSprite(EnumPivotDynamic index)
        {
            try
            {
                if (_spriteRenderer == null)
                    _spriteRenderer = GetComponent<SpriteRenderer>();
                _spriteRenderer.sprite = sprites[(int)index];

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
    /// 支点ライトの見た目モード
    /// </summary>
    public enum EnumPivotDynamic
    {
        /// <summary>通常</summary>
        PivotLight,
        /// <summary>サン（ゴ）ショウ</summary>
        PivotLightOnCoral,
    }
}
