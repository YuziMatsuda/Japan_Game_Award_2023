using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Area.Model
{
    /// <summary>
    /// 全てのユニットの制御
    /// モデル
    /// </summary>
    public class RobotPanelModel : MonoBehaviour
    {
        /// <summary>各ユニットの制御配列</summary>
        [SerializeField] private RobotUnitImageModel[] robotUnitImageModels;
        /// <summary>各ユニットの制御配列</summary>
        public RobotUnitImageModel[] RobotUnitImageModels => robotUnitImageModels;

        private void Reset()
        {
            robotUnitImageModels = GetComponentsInChildren<RobotUnitImageModel>();
        }
    }
}
