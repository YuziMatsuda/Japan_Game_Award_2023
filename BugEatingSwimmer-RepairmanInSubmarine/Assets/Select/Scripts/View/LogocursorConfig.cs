using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Select.Common;

namespace Select.View
{
    /// <summary>
    /// ロゴカーソル設定
    /// </summary>
    public class LogocursorConfig : MonoBehaviour
    {
        /// <summary>方角モード</summary>
        [SerializeField] private EnumDirectionMode enumDirectionMode;
        /// <summary>方角モード</summary>
        public EnumDirectionMode EnumDirectionMode => enumDirectionMode;
        /// <summary>位置（配列番号が若いほど 短 ＞ 長）</summary>
        [SerializeField] private Vector3[] positions;
        /// <summary>位置（配列番号が若いほど 短 ＞ 長）</summary>
        public Vector3[] Positions => positions;
    }
}
