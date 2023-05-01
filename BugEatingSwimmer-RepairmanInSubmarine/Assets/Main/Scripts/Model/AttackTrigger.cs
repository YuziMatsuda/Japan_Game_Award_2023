using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

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
            circleCollider.enabled = false;
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
    }
}
