using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Area.View
{
    /// <summary>
    /// ボディのイメージ
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class BodyImage : MonoBehaviour, IBodyImageForColor, IBodyImageForTransform, ISelectStageFrameView
    {
        /// <summary>フェードアニメーション時間</summary>
        [SerializeField] private float fadeDuration = .5f;
        /// <summary>トランスフォーム</summary>
        private Transform _transform;
        /// <summary>画像</summary>
        private Image _image;

        public IEnumerator MoveSelectPlayer(Vector3 targetPosition, Transform currentTarget, IObserver<bool> observer)
        {
            throw new NotImplementedException();
        }

        public bool MoveSelectStageFrame(Vector3 targetPosition, Vector2 sizeDelta)
        {
            throw new NotImplementedException();
        }

        public IEnumerator PlayFadeAnimation(IObserver<bool> observer)
        {
            GetComponent<Image>().DOFade(endValue: 0f, fadeDuration)
                .SetUpdate(true)
                .OnComplete(() => observer.OnNext(true));
            yield return null;
        }

        public bool SelectPlayer(Vector3 targetPosition, Transform currentTarget)
        {
            throw new NotImplementedException();
        }

        public bool SetColorAlpha(float alpha)
        {
            try
            {
                if (_image == null)
                    _image = GetComponent<Image>();
                var color = _image.color;
                color.a = alpha;
                _image.color = color;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetColorSpriteRenderer(Color color)
        {
            try
            {
                GetComponent<Image>().color = color;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetImageEnabled(bool isEnabled)
        {
            try
            {
                if (_image == null)
                    _image = GetComponent<Image>();
                _image.enabled = isEnabled;
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetLocalScaleX(float scaleX)
        {
            try
            {
                if (_transform == null)
                    _transform = transform;

                var scale = _transform.localScale;
                scale.x = scaleX;
                _transform.localScale = scale;

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }
    }

    /// <summary>
    /// ボディのイメージのカラー
    /// インターフェース
    /// </summary>
    public interface IBodyImageForColor
    {
        /// <summary>
        /// カラーを設定
        /// </summary>
        /// <param name="color">カラー情報</param>
        /// <returns>成功／失敗</returns>
        public bool SetColorSpriteRenderer(Color color);
        /// <summary>
        /// フェードのDOTweenアニメーション再生
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayFadeAnimation(System.IObserver<bool> observer);
        /// <summary>
        /// イメージのステータスを変更
        /// </summary>
        /// <param name="isEnabled">有効／無効</param>
        /// <returns>成功／失敗</returns>
        public bool SetImageEnabled(bool isEnabled);
    }

    /// <summary>
    /// ボディのイメージのトランスフォーム
    /// インターフェース
    /// </summary>
    public interface IBodyImageForTransform
    {
        /// <summary>
        /// ローカルスケールをセット
        /// </summary>
        /// <param name="scaleX">スケールX</param>
        /// <returns>成功／失敗</returns>
        public bool SetLocalScaleX(float scaleX);
    }
}
