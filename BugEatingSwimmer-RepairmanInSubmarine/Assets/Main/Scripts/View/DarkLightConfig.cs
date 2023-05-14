using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// 暗闇設定
    /// </summary>
    public class DarkLightConfig : MonoBehaviour
    {
        /// <summary>大きさ配列</summary>
        [SerializeField] private Vector3[] scales;
        /// <summary>大きさ配列</summary>
        public Vector3[] Scales => scales;
        /// <summary>タイマーを有効にするか</summary>
        [SerializeField] private bool timerMode;
        /// <summary>タイマーを有効にするか</summary>
        public bool TimerMode => timerMode;
        /// <summary>暗くする時間間隔</summary>
        [SerializeField] private float playLightDownRate;
        /// <summary>暗くする時間間隔</summary>
        public float PlayLightDownRate => playLightDownRate;
    }
}
