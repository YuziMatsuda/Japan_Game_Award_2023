using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Main.Model
{
    /// <summary>
    /// 暗闇トリガー
    /// </summary>
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class DarkLightTriggerModel : AbstractCollider
    {
        /// <summary>衝突フラグ</summary>
        private readonly BoolReactiveProperty _isHit = new BoolReactiveProperty();
        /// <summary>衝突フラグ</summary>
        public IReactiveProperty<bool> IsHit => _isHit;

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_isHit.Value &&
                IsCollisionToTags(collision, tags))
                _isHit.Value = true;
        }
    }
}
