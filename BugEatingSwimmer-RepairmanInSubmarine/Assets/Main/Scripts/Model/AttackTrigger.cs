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
    public class AttackTrigger : MonoBehaviour, IAttackTrigger
    {
        /// <summary>コライダー</summary>
        [SerializeField] CircleCollider2D circleCollider;

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
