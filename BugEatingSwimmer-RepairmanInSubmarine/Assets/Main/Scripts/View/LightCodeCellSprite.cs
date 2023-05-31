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
        /// <summary>コードセルのパターン</summary>
        [SerializeField] private Sprite[] codeCellsPattern;
        /// <summary>点滅アニメーション終了時間</summary>
        [SerializeField] private float flashDuration = .75f;
        /// <summary>点滅アニメーションループ回数</summary>
        [SerializeField] private int loops = 2;
        /// <summary>
        /// エラーのTween
        /// 見た目のみの挙動のため、他のアニメーションが再生されたら即終了させてもよい想定
        /// </summary>
        private Tweener _errorTweener;

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

        public IEnumerator PlayErrorLightFlashAnimation(IObserver<bool> observer)
        {
            // スプライトをエラーへ変更
            // イメージの点滅アニメーション
            // アニメーション完了時にスプライトを戻す
            if (_spriteRenderer == null)
                _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = codeCellsPattern[1];
            _errorTweener = _spriteRenderer.DOFade(endValue: onFade, flashDuration)
                .SetUpdate(true)
                .SetEase(Ease.OutBounce)
                .SetLoops(loops, LoopType.Yoyo)
                .OnComplete(() =>
                {
                    _spriteRenderer.sprite = codeCellsPattern[0];
                    var color = _spriteRenderer.color;
                    color.a = onFade;
                    _spriteRenderer.color = color;
                    observer.OnNext(true);
                });

            yield return null;
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

        public IEnumerator PlayLockSpinAnimation(IObserver<bool> observer)
        {
            throw new NotImplementedException();
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
                if (_errorTweener != null &&
                    _errorTweener.IsActive() &&
                    _errorTweener.IsPlaying())
                    _errorTweener.Complete();
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
            throw new NotImplementedException();
        }
    }
}
