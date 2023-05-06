using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Presenter;

namespace Main.Common
{
    /// <summary>
    /// 実績一覧管理オーナー
    /// </summary>
    public class MissionOwner : MonoBehaviour
    {
        /// <summary>ミッションIDに紐づく「ロボットの結合状態」「エリアID」</summary>
        [SerializeField] private Mission[] missions;
        /// <summary>ミッションIDに紐づく「ロボットの結合状態」「エリアID」</summary>
        public Mission[] Missions => missions;
    }
}
