using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Main.Model
{
    /// <summary>
    /// ロボットヘッド（修理前）
    /// モデル
    /// </summary>
    [RequireComponent(typeof(CircleCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class RobotHeadBeforeModel : AbstractCollider
    {
        /// <summary>衝突したか</summary>
        private readonly BoolReactiveProperty _isCollision = new BoolReactiveProperty();
        /// <summary>衝突したか</summary>
        public IReactiveProperty<bool> IsCollision => _isCollision;

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (IsCollisionToTags(collision, tags) &&
                !_isCollision.Value)
            {
                _isCollision.Value = true;
            }
        }
    }
}
