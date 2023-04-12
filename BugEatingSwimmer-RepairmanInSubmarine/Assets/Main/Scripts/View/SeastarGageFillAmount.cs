using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Main.View
{
    /// <summary>
    /// ヒトデゲージのサイクルゲージ
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class SeastarGageFillAmount : MonoBehaviour, ISeastarGageFillAmount
    {
        /// <summary>スプライトレンダラー</summary>
        private SpriteRenderer _spriteRenderer;
        /// <summary>アニメーション時間</summary>
        [SerializeField] private float duration = 1.25f;

        public bool PlayFillAmountAnimation(float fillAmountValue)
        {
            try
            {
                if (_spriteRenderer == null)
                    _spriteRenderer = GetComponent<SpriteRenderer>();
                var mat = _spriteRenderer.material;
                var prev = mat.GetFloat("_FillAmount");
                if (!prev.Equals(fillAmountValue))
                {
                    DOVirtual.DelayedCall(duration, () =>
                    {
                        // 差異があった場合のみ更新
                        DOTween.To(() => prev, x => mat.SetFloat("_FillAmount", x), fillAmountValue, duration);
                    });
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetFillAmount(float fillAmountValue)
        {
            try
            {
                if (_spriteRenderer == null)
                    _spriteRenderer = GetComponent<SpriteRenderer>();
                var mat = _spriteRenderer.material;
                var prev = mat.GetFloat("_FillAmount");
                if (!prev.Equals(fillAmountValue))
                    // 差異があった場合のみ更新
                    mat.SetFloat("_FillAmount", fillAmountValue);

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
    /// ヒトデゲージのサイクルゲージ
    /// インターフェース
    /// </summary>
    public interface ISeastarGageFillAmount
    {
        /// <summary>
        /// フィルターゲージの値をセット
        /// </summary>
        /// <param name="fillAmountValue">ゲージ値（0～1）</param>
        /// <returns>成功／失敗</returns>
        public bool SetFillAmount(float fillAmountValue);
        /// <summary>
        /// フィルターゲージの値をセット
        /// </summary>
        /// <param name="fillAmountValue">ゲージ値（0～1）</param>
        /// <returns>成功／失敗</returns>
        public bool PlayFillAmountAnimation(float fillAmountValue);
    }
}
