using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Main.Common;
using System.Linq;
using UniRx;

namespace Main.Model
{
    /// <summary>
    /// モデル
    /// バグ
    /// </summary>
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class BugModel : MonoBehaviour
    {
        /// <summary>接触対象のオブジェクトタグ</summary>
        [SerializeField] private string[] tags = { ConstTagNames.TAG_NAME_ATTACK_TRIGGER };
        /// <summary>食べられたフラグ</summary>
        private readonly BoolReactiveProperty _isEated = new BoolReactiveProperty();
        /// <summary>食べられたフラグ</summary>
        public IReactiveProperty<bool> IsEated => _isEated;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (0 < tags.Where(q => collision.CompareTag(q)).Select(q => q).ToArray().Length &&
                !_isEated.Value)
            {
                _isEated.Value = true;
            }
        }
    }
}
