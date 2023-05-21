using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;

namespace Main.View
{
    /// <summary>
    /// ヒトデゲージのサイクルゲージ
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class SeastarGageFillAmountUI : MonoBehaviour, ISeastarGageFillAmount, ISeastarGageFillAmountUI
    {
        /// <summary>トランスフォーム</summary>
        private Transform _transform;
        /// <summary>ターゲット</summary>
        private Transform _target;
        /// <summary>イメージ</summary>
        [SerializeField] private Image image;
        /// <summary>アニメーション終了時間</summary>
        [SerializeField] private float[] durations = { 1.25f };

        private void Reset()
        {
            image = GetComponent<Image>();
        }

        private void Start()
        {
            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    if (_transform == null)
                        _transform = transform;

                    if (_target != null)
                        _transform.position = _target.position;
                    else
                        image.enabled = false;
                });
        }

        public bool PlayFillAmountAnimation(float fillAmountValue)
        {
            try
            {
                var prev = image.fillAmount;
                if (!prev.Equals(fillAmountValue))
                {
                    DOVirtual.DelayedCall(durations[0], () =>
                    {
                        // 差異があった場合のみ更新
                        DOTween.To(() => prev, x => image.fillAmount = x, fillAmountValue, durations[0]);
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
                var prev = image.fillAmount;
                if (!prev.Equals(fillAmountValue))
                    // 差異があった場合のみ更新
                    image.fillAmount = fillAmountValue;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetTarget(Transform target)
        {
            try
            {
                _target = target;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetImageEnabled(bool enabled)
        {
            try
            {
                image.enabled = enabled;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }
    }

    public interface ISeastarGageFillAmountUI
    {
        /// <summary>
        /// ターゲットをセット
        /// </summary>
        /// <param name="target">追尾対象</param>
        /// <returns>成功／失敗</returns>
        public bool SetTarget(Transform target);
        /// <summary>
        /// イメージの表示状態をセット
        /// </summary>
        /// <param name="enabled">有効／無効</param>
        /// <returns>成功／失敗</returns>
        public bool SetImageEnabled(bool enabled);
    }
}
