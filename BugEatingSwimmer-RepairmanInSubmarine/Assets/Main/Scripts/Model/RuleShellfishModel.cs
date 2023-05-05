using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Main.Common;
using System.Linq;

namespace Main.Model
{
    /// <summary>
    /// ルール貝
    /// モデル
    /// </summary>
    [RequireComponent(typeof(CircleCollider2D))]
    public class RuleShellfishModel : MonoBehaviour
    {
        /// <summary>接触対象のオブジェクトタグ</summary>
        [SerializeField] protected string[] tags = { ConstTagNames.TAG_NAME_TOUCH_TRIGGER };
        /// <summary>接触状態か</summary>
        private readonly BoolReactiveProperty _isInRange = new BoolReactiveProperty();
        /// <summary>接触状態か</summary>
        public IReactiveProperty<bool> IsInRange => _isInRange;
        /// <summary>一度のみヒントを表示させる（デフォルト：無効）</summary>
        [SerializeField] private bool isOnlyOnceHint = false;
        /// <summary>一度のみヒントを表示させる（デフォルト：無効）</summary>
        public bool IsOnlyOnceHint => isOnlyOnceHint;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (0 < tags.Where(q => collision.CompareTag(q)).Select(q => q).ToArray().Length)
            {
                _isInRange.Value = true;
            }
        }
    }
}
