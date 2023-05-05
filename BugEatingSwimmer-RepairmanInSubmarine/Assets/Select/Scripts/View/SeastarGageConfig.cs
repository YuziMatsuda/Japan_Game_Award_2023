using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Select.Common;

namespace Select.View
{
    /// <summary>
    /// ヒトデゲージ
    /// 設定
    /// </summary>
    public class SeastarGageConfig : MonoBehaviour
    {
        /// <summary>エリアID</summary>
        [SerializeField] private EnumUnitID enumUnitID;
        /// <summary>エリアID</summary>
        public EnumUnitID EnumUnitID => enumUnitID;
    }
}
