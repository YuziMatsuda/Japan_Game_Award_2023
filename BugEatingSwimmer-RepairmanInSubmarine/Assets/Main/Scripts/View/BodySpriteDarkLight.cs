using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// 暗闇のスプライト
    /// </summary>
    public class BodySpriteDarkLight : BodySprite, IBodySpriteDarkLight, IBodySpriteMask
    {
        /// <summary>スプライトマスク</summary>
        [SerializeField] private BodySpriteMask bodySpriteMask;

        public bool HoverTarget(Transform target)
        {
            return ((IBodySpriteMask)bodySpriteMask).HoverTarget(target);
        }

        public int PlayLightDown()
        {
            return ((IBodySpriteMask)bodySpriteMask).PlayLightDown();
        }

        private void Reset()
        {
            bodySpriteMask = GetComponentInChildren<BodySpriteMask>();
        }
    }

    /// <summary>
    /// 暗闇のスプライト
    /// インターフェース
    /// </summary>
    public interface IBodySpriteDarkLight
    {
    }
}
