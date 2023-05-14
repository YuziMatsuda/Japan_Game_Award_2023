using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Main.View
{
    /// <summary>
    /// ！！のスプライト
    /// </summary>
    public class BodySpriteWow : BodySprite, IBodySpriteWow
    {
        /// <summary>スプライトレンダラー</summary>
        [SerializeField] private SpriteRenderer spriteRenderer;
        /// <summary>アニメーション時間配列</summary>
        [SerializeField] private float[] subDurations = { .5f };
        /// <summary>トランスフォーム</summary>
        private Transform _transform;

        private void Reset()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            fadeDuration = .1f;
        }

        public IEnumerator PlayWowAnimation(System.IObserver<bool> observer)
        {
            Sequence sequence = DOTween.Sequence()
                .Append(spriteRenderer.DOFade(1f, fadeDuration)
                    .SetEase(Ease.InQuad))
                .AppendInterval(subDurations[0])
                .Append(spriteRenderer.DOFade(0f, fadeDuration)
                    .SetEase(Ease.InQuad))
                .OnComplete(() => observer.OnNext(true));

            yield return null;
        }

        public bool SetScale(Vector3 scale)
        {
            try
            {
                if (_transform == null)
                    _transform = transform;
                _transform.localScale = scale;

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
    /// ！！のスプライト
    /// インターフェース
    /// </summary>
    public interface IBodySpriteWow
    {
        /// <summary>
        /// 驚くアニメーションを再生
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayWowAnimation(System.IObserver<bool> observer);
        /// <summary>
        /// 大きさをセット
        /// </summary>
        /// <param name="scale">スケール</param>
        /// <returns>成功／失敗</returns>
        public bool SetScale(Vector3 scale);
    }
}
