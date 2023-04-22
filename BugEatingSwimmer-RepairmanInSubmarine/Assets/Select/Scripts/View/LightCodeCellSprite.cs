using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Select.Common;

namespace Select.View
{
    /// <summary>
    /// コード（明）
    /// 個々の制御
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class LightCodeCellSprite : PivotAndCodeIShortUIViewParent, IShadowCodeCell
    {
        ///// <summary>スプライト</summary>
        //private SpriteRenderer _spriteRenderer;
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
        /// <summary>イメージ</summary>
        [SerializeField] private Image image;

        private void Reset()
        {
            image = GetComponent<Image>();
        }

        public bool InitializeLight(EnumDirectionMode enumDirectionMode)
        {
            try
            {
                var color = image.color;
                color.a = onFade * 255f;
                image.color = color;

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
            image.sprite = codeCellsPattern[1];
            _errorTweener = image.DOFade(endValue: onFade, flashDuration)
                .SetUpdate(true)
                .SetEase(Ease.OutBounce)
                .SetLoops(loops, LoopType.Yoyo)
                .OnComplete(() =>
                {
                    image.sprite = codeCellsPattern[0];
                    var color = image.color;
                    color.a = onFade;
                    image.color = color;
                    observer.OnNext(true);
                });

            yield return null;
        }

        public IEnumerator PlayLightAnimation(IObserver<bool> observer, EnumDirectionMode enumDirectionMode)
        {
            image.DOFade(endValue: onFade, duration)
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
                if (_errorTweener != null &&
                    _errorTweener.IsActive() &&
                    _errorTweener.IsPlaying())
                    _errorTweener.Complete();
                var color = image.color;
                color.a = offFade;
                image.color = color;

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

        public bool SetSprite(EnumPivotDynamic index)
        {
            throw new NotImplementedException();
        }
    }
}
