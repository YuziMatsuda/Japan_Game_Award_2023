using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Main.Model
{
    /// <summary>
    /// 攻撃時のトリガー判定
    /// </summary>
    [RequireComponent(typeof(CircleCollider2D))]
    public class AttackTrigger : MonoBehaviour, IAttackTrigger, IPlayerModel
    {
        /// <summary>コライダー</summary>
        [SerializeField] CircleCollider2D circleCollider;
        /// <summary>パワー状態</summary>
        public IReactiveProperty<bool> IsPower => transform.parent.GetComponent<PlayerModel>().IsPower;
        /// <summary>押し続けて離す</summary>
        public IReactiveProperty<bool> IsPressAndHoldAndReleased => transform.parent.GetComponent<PlayerModel>().IsPressAndHoldAndReleased;
        /// <summary>ターゲット位置</summary>
        private Vector3 _targetPosition;

        public bool SetAutoAttack(bool isEnabled)
        {
            throw new System.NotImplementedException();
        }

        public bool SetColliderEnabled(bool enabled)
        {
            try
            {
                circleCollider.enabled = enabled;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetInputBan(bool unactive)
        {
            throw new System.NotImplementedException();
        }

        public bool SetInputBan(bool unactive, bool isDelayMode)
        {
            throw new System.NotImplementedException();
        }

        public bool SetIsBanMoveVelocity(bool unactive)
        {
            throw new System.NotImplementedException();
        }

        public bool SetIsPower(bool enabled)
        {
            return transform.parent.GetComponent<PlayerModel>().SetIsPower(enabled);
        }

        public bool SetOnTrurn(bool enabled)
        {
            throw new System.NotImplementedException();
        }

        private void Reset()
        {
            circleCollider = GetComponent<CircleCollider2D>();
        }

        private void Start()
        {
            var transform = this.transform;
            circleCollider.enabled = false;
            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    if (circleCollider.enabled)
                        if (_targetPosition != null &&
                            transform.position != _targetPosition)
                            transform.position = _targetPosition;
                });
        }

        public bool SetTarget(Vector3 targetPosition)
        {
            try
            {
                _targetPosition = targetPosition;

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
    /// 攻撃時のトリガー判定インターフェース
    /// </summary>
    public interface IAttackTrigger
    {
        /// <summary>
        /// コライダーの有効／無効をセット
        /// </summary>
        /// <param name="enabled">有効／無効</param>
        /// <returns>成功／失敗</returns>
        public bool SetColliderEnabled(bool enabled);
        /// <summary>
        /// ターゲットをセット
        /// </summary>
        /// <param name="transform">トランスフォーム</param>
        /// <returns>成功／失敗</returns>
        public bool SetTarget(Vector3 targetPosition);
    }
}
