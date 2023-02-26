using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Main.Common;

namespace Main.Model
{
    /// <summary>
    /// モデル
    /// ゴールポイント
    /// </summary>
    public class GoalPointModel : LevelPhysicsSerializerCapsule
    {
        /// <summary>トリガーへ入る</summary>
        private readonly BoolReactiveProperty _isTriggerEntered = new BoolReactiveProperty();
        /// <summary>トリガーへ入る</summary>
        public IReactiveProperty<bool> IsTriggerEntered => _isTriggerEntered;
        /// <summary>トランスフォーム</summary>
        private Transform _transform;

        protected override void Reset()
        {
            base.Reset();
            distance = 0f;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(ConstTagNames.TAG_NAME_PLAYER))
            {
                _isTriggerEntered.Value = true;
            }
        }

        private void FixedUpdate()
        {
            origin = gameObject.transform.position;
            var result = Physics2D.CapsuleCast(origin, size, capsuleDirection, angle, direction, distance, LayerMask.GetMask(ConstLayerNames.LAYER_NAME_FLOOR));
            if (result.transform != null)
            {
                if (_transform == null)
                    _transform = transform;
                _transform.position += Physics.gravity * Time.deltaTime;
            }
        }
    }
}
