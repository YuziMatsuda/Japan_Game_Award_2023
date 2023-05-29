using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Area.View
{
    /// <summary>
    /// プレイヤー
    /// 設定
    /// </summary>
    public class PlayerConfig : MonoBehaviour
    {
        /// <summary>アンカー初期位置</summary>
        [SerializeField] private Vector2[] defaultAnchors;
        /// <summary>アンカー初期位置</summary>
        public Vector2[] DefaultAnchors => defaultAnchors;
    }
}
