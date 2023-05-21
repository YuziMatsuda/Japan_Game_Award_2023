using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;

namespace Main.View
{
    /// <summary>
    /// ヒトデゲージカウンター
    /// </summary>
    [RequireComponent(typeof(TextMeshPro))]
    public class SeastarGageCounter : MonoBehaviour, ISeastarGageCounter, ISeastarGageFillAmountUI
    {
        /// <summary>デフォルト表示</summary>
        [SerializeField] private string slashAndSpace = " / ";
        /// <summary>分母</summary>
        private int _denominator = -1;
        /// <summary>テキストメッシュプロ</summary>
        [SerializeField] private TextMeshPro textMeshPro;
        /// <summary>アニメーション時間</summary>
        [SerializeField] private float duration = 1.25f;
        /// <summary>トランスフォーム</summary>
        private Transform _transform;
        /// <summary>ターゲット</summary>
        private Transform _target;
        /// <summary>位置補正</summary>
        [SerializeField] private Vector3 positionOffSet = new Vector3(0f, .5f, 0f);

        private void Reset()
        {
            textMeshPro = GetComponent<TextMeshPro>();
        }

        private void Start()
        {
            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    if (_transform == null)
                        _transform = transform;

                    if (_target != null)
                        _transform.position = _target.position + positionOffSet;
                    else
                        textMeshPro.enabled = false;
                });
        }

        public bool PlayCounterBetweenAnimation(int numerator)
        {
            return PlayCounterBetweenAnimation(numerator, _denominator);
        }

        public bool PlayCounterBetweenAnimation(int numerator, int denominator)
        {
            try
            {
                if (denominator < 0)
                    throw new System.Exception("分母の未設定");
                _denominator = denominator;
                var prev = textMeshPro.text;
                var update = $"{numerator}{slashAndSpace}{_denominator}";
                if (!prev.Equals(update))
                {
                    DOVirtual.DelayedCall(duration, () =>
                    {
                        // 差異があった場合のみ更新
                        DOTween.To(() => int.Parse(prev.Replace($"{slashAndSpace}{_denominator}", "")),
                            x => textMeshPro.text = $"{x}{slashAndSpace}{_denominator}",
                            numerator,
                            duration);
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

        public bool SetCounterBetween(int numerator)
        {
            return SetCounterBetween(numerator, _denominator);
        }

        public bool SetCounterBetween(int numerator, int denominator)
        {
            try
            {
                if (denominator < 0)
                    throw new System.Exception("分母の未設定");
                _denominator = denominator;
                var prev = textMeshPro.text;
                var update = $"{numerator}{slashAndSpace}{_denominator}";
                if (!prev.Equals(update))
                    // 差異があった場合のみ更新
                    textMeshPro.text = update;

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
                textMeshPro.enabled = enabled;

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
    /// ヒトデゲージカウンター
    /// インターフェース
    /// </summary>
    public interface ISeastarGageCounter
    {
        /// <summary>
        /// カウンターをセット
        /// </summary>
        /// <param name="numerator">分子</param>
        /// <returns>成功／失敗</returns>
        public bool SetCounterBetween(int numerator);
        /// <summary>
        /// カウンターをセット
        /// </summary>
        /// <param name="numerator">分子</param>
        /// <param name="denominator">分母</param>
        /// <returns>成功／失敗</returns>
        public bool SetCounterBetween(int numerator, int denominator);
        /// <summary>
        /// カウンターセットアニメーション
        /// </summary>
        /// <param name="numerator">分子</param>
        /// <returns>成功／失敗</returns>
        public bool PlayCounterBetweenAnimation(int numerator);
        /// <summary>
        /// カウンターセットアニメーション
        /// </summary>
        /// <param name="numerator">分子</param>
        /// <param name="denominator">分母</param>
        /// <returns>成功／失敗</returns>
        public bool PlayCounterBetweenAnimation(int numerator, int denominator);
    }
}
