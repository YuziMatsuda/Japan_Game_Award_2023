using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Area.Common;
using UnityEngine.UI;

namespace Area.View
{
    /// <summary>
    /// 各ユニットの制御設定
    /// </summary>
    public class RobotUnitImageConfig : MonoBehaviour
    {
        /// <summary>位置</summary>
        [SerializeField] private Vector3 pos;
        /// <summary>位置</summary>
        public Vector3 Pos => pos;
        /// <summary>角度</summary>
        [SerializeField] private Vector3 rotate;
        /// <summary>角度</summary>
        public Vector3 Rotate => rotate;
        /// <summary>実行時間</summary>
        [SerializeField] private float duration;
        /// <summary>実行時間</summary>
        public float Duration => duration;
        /// <summary>ユニットID</summary>
        [SerializeField] private EnumUnitID enumUnitID;
        /// <summary>ユニットID</summary>
        public EnumUnitID EnumUnitID => enumUnitID;
        /// <summary>ナビゲーションパターン</summary>
        [SerializeField] private Navigation[] navigations;
        /// <summary>ナビゲーションパターン</summary>
        public Navigation[] Navigations => navigations;
    }
}
