using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Main.Model
{
    /// <summary>
    /// 死亡判定のコライダー
    /// </summary>
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class DangerCollider : AbstractCollider, IDangerCollider
    {
        /// <summary>接触するか</summary>
        private readonly BoolReactiveProperty _isHit = new BoolReactiveProperty();
        /// <summary>接触するか</summary>
        public IReactiveProperty<bool> IsHit => _isHit;
        /// <summary>コライダー</summary>
        [SerializeField] private CapsuleCollider2D capsuleCollider2d;

        private void Reset()
        {
            capsuleCollider2d = GetComponent<CapsuleCollider2D>();
            capsuleCollider2d.enabled = false;
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_isHit.Value &&
                IsCollisionToTags(collision, tags))
            {
                _isHit.Value = true;
            }
        }

        public bool SetCollider2DEnabled(bool isEnabled)
        {
            try
            {
                capsuleCollider2d.enabled = isEnabled;

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
    /// 死亡判定のコライダー
    /// インターフェース
    /// </summary>
    public interface IDangerCollider
    {
        /// <summary>
        /// コライダーの状態をセット
        /// </summary>
        /// <param name="isEnabled">有効／無効</param>
        /// <returns>成功／失敗</returns>
        public bool SetCollider2DEnabled(bool isEnabled);
    }
}
