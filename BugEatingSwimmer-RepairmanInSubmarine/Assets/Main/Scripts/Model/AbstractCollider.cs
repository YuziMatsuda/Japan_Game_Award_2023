using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Common;
using System.Linq;

namespace Main.Model
{
    /// <summary>
    /// コライダーの親
    /// </summary>
    public class AbstractCollider : MonoBehaviour
    {
        /// <summary>接触対象の配列</summary>
        [SerializeField] protected string[] tags = { ConstTagNames.TAG_NAME_PLAYER };

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            
        }

        public bool IsCollisionToTags(Collider2D collision, string[] tags)
        {
            return 0 < tags.Where(q => collision.CompareTag(q)).Select(q => q).ToArray().Length;
        }
    }
}
