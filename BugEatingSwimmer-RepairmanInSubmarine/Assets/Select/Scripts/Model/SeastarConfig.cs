using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Select.Common;

namespace Select.Model
{
    /// <summary>
    /// ヒトデ管理設定
    /// </summary>
    public class SeastarConfig : MonoBehaviour
    {
        /// <summary>ヒトデの識別ID</summary>
        [SerializeField] private EnumSeastarID enumSeastar;
        /// <summary>ヒトデの識別ID</summary>
        public EnumSeastarID EnumSeastarID => enumSeastar;
    }
}
