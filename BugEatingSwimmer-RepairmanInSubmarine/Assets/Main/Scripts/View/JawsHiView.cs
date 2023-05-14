using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Main.Model;
using DG.Tweening;
using System.Linq;
using Main.Common;

namespace Main.View
{
    /// <summary>
    /// ジョーシー
    /// モデル
    /// </summary>
    [RequireComponent(typeof(LoinclothConfig))]
    public class JawsHiView : MonoBehaviour, IBodySpriteJawsHi, IJawsHiView, IBodySpriteWow, IBodySpriteTarget
    {
        /// <summary>スプライト</summary>
        [SerializeField] private BodySpriteJawsHi bodySpriteJawsHi;
        /// <summary>設定</summary>
        [SerializeField] private LoinclothConfig loinclothConfig;
        /// <summary>ターゲットプレハブ（インスタンス済み）</summary>
        private Transform _instancedTarget;
        /// <summary>表示</summary>
        [SerializeField] private Color visibledColorOfChild = new Color(1f, 1f, 1f, 1f);
        /// <summary>非表示</summary>
        [SerializeField] private Color disabledColorOfChild = new Color(1f, 1f, 1f, 0f);
        /// <summary>トランスフォーム</summary>
        private Transform _transform;
        /// <summary>アニメーション終了時間</summary>
        [SerializeField] private float[] durations = { 1.5f, 1.2f, 1.5f };
        /// <summary>後退補正値</summary>
        [SerializeField] private float backCorrection = .6f;
        /// <summary>オーバーシュート</summary>
        [SerializeField] private float overshort = 5f;

        public IEnumerator PlayAttackAnimation(System.IObserver<int> observer, Vector3 target)
        {
            if (_transform == null)
                _transform = transform;
            // 後退
            // ターゲットへ向かって突進
            var endValue = Vector3.MoveTowards(_transform.position, target, -1f * backCorrection);
            var originPosition = _transform.position;
            DOTween.Sequence()
                // Step_0:後退
                .AppendCallback(() => observer.OnNext((int)EnumPlayAttackAnimation.Backing))
                .Append(_transform.DOMove(endValue, durations[0])
                    .SetEase(Ease.InQuad))
                // Step_1:突進
                .AppendCallback(() => observer.OnNext((int)EnumPlayAttackAnimation.Rushing))
                .Append(_transform.DOMove(target, durations[1])
                    .SetEase(Ease.OutBack, overshort))
                // Step_2:戻る
                .AppendCallback(() => observer.OnNext((int)EnumPlayAttackAnimation.Returning))
                .Append(_transform.DOMove(originPosition, durations[2]))
                // Step_3:終了
                .OnComplete(() => observer.OnNext((int)EnumPlayAttackAnimation.OnCompleted))
                .SetLink(gameObject);

            yield return null;
        }

        public IEnumerator PlayLockOnAnimation(System.IObserver<bool> observer, Vector3 target)
        {
            if (_instancedTarget == null)
                _instancedTarget = Instantiate(loinclothConfig.JawsHiDetail.targetPrefab, transform.position, Quaternion.identity);
            _instancedTarget.position = transform.position;

            Observable.FromCoroutine<bool>(observer => _instancedTarget.GetComponent<BodySpriteTarget>().PlayLockOnAnimation(observer, target))
                .Subscribe(_ => observer.OnNext(true))
                .AddTo(gameObject);

            yield return null;
        }

        public IEnumerator PlayWowAnimation(System.IObserver<bool> observer)
        {
            Observable.FromCoroutine<bool>(observer => bodySpriteJawsHi.PlayWowAnimation(observer))
                .Subscribe(_ => observer.OnNext(true))
                .AddTo(gameObject);

            yield return null;
        }

        public bool SetColorSpriteRendererTarget(bool isEnabled)
        {
            if (_instancedTarget == null)
                _instancedTarget = Instantiate(loinclothConfig.JawsHiDetail.targetPrefab, transform.position, Quaternion.identity);
            _instancedTarget.position = transform.position;

            return _instancedTarget.GetComponent<BodySpriteTarget>().SetColorSpriteRenderer(isEnabled ? visibledColorOfChild : disabledColorOfChild);
        }

        public bool SetColorSpriteRendererWow(bool isEnabled)
        {
            return ((IBodySpriteJawsHi)bodySpriteJawsHi).SetColorSpriteRendererWow(isEnabled);
        }

        public bool StopLockOnAnimation()
        {
            if (_instancedTarget == null)
            {
                Debug.LogWarning($"インスタンス無し:[{_instancedTarget}]");
                return true;
            }
            return _instancedTarget.GetComponent<BodySpriteTarget>().StopLockOnAnimation();
        }

        private void Reset()
        {
            bodySpriteJawsHi = GetComponentInChildren<BodySpriteJawsHi>();
            loinclothConfig = GetComponent<LoinclothConfig>();
        }

        public bool ResetPosition(Vector3 fromPosition)
        {
            if (_instancedTarget == null)
            {
                Debug.LogWarning($"インスタンス無し:[{_instancedTarget}]");
                return true;
            }
            return _instancedTarget.GetComponent<BodySpriteTarget>().ResetPosition(fromPosition);
        }

        public bool SetScale(Vector3 scale)
        {
            return ((IBodySpriteWow)bodySpriteJawsHi).SetScale(scale);
        }

        public bool SetSpriteIndex(EnumEnemySpriteIndex enumEnemySpriteIndex)
        {
            return ((IBodySpriteJawsHi)bodySpriteJawsHi).SetSpriteIndex(enumEnemySpriteIndex);
        }
    }

    /// <summary>
    /// ジョーシー
    /// モデル
    /// インターフェース
    /// </summary>
    public interface IJawsHiView
    {
        /// <summary>
        /// カラーを設定
        /// </summary>
        /// <param name="isEnabled">表示／非表示</param>
        /// <returns>成功／失敗</returns>
        public bool SetColorSpriteRendererTarget(bool isEnabled);
        /// <summary>
        /// 攻撃アニメーションを再生
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <param name="target">ターゲット</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayAttackAnimation(System.IObserver<int> observer, Vector3 target);
    }
}
