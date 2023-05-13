using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Main.Common;
using System.Linq;

namespace Main.Model
{
    /// <summary>
    /// サーチコライダー
    /// </summary>
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class SearchCollider : AbstractCollider, ISearchCollider
    {
        /// <summary>接触するか</summary>
        private readonly BoolReactiveProperty _isHit = new BoolReactiveProperty();
        /// <summary>接触位置</summary>
        private readonly Vector3ReactiveProperty _hitPosition = new Vector3ReactiveProperty();
        /// <summary>接触位置</summary>
        public IReactiveProperty<Vector3> HitPosition => _hitPosition;

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_isHit.Value &&
                IsCollisionToTags(collision, tags))
            {
                _isHit.Value = true;
                _hitPosition.Value = collision.ClosestPoint(transform.position);
            }
        }

        public bool SetHitState(bool isEnabled)
        {
            try
            {
                _isHit.Value = isEnabled;
                if (!isEnabled)
                    _hitPosition.Value = Vector3.zero;

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
    /// サーチコライダー
    /// インターフェース
    /// </summary>
    public interface ISearchCollider
    {
        /// <summary>
        /// ヒット状態をセット
        /// </summary>
        /// <param name="isEnabled">有効／無効</param>
        /// <returns>成功／失敗</returns>
        public bool SetHitState(bool isEnabled);
    }
}
