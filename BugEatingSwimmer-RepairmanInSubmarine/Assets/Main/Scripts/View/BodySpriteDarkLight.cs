using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// 暗闇のスプライト
    /// </summary>
    public class BodySpriteDarkLight : BodySprite, IBodySpriteDarkLight
    {
        [SerializeField] private Vector3[] scales;


    }

    /// <summary>
    /// 暗闇のスプライト
    /// インターフェース
    /// </summary>
    public interface IBodySpriteDarkLight
    {
        
    }
}
