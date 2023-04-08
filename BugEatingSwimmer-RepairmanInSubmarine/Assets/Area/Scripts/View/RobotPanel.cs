using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Area.View
{
    public enum EnumRobotPanel
    {
        FallingApart, 
        ConnectedBody, 
        ConnectedLeftarm, 
        ConnectedRightarm, 
        Full
    }
    /// <summary>
    /// 全てのユニットの制御
    /// </summary>
    public class RobotPanel : MonoBehaviour
    {
        [SerializeField] private Robot_UnitImage[] robot_UnitImages;
        [SerializeField] private EnumRobotPanel enumrobotpanel;


        void Update()
        {
            switch (enumrobotpanel)
            {
                case EnumRobotPanel.FallingApart:
                    robot_UnitImages[4].SetImageAltha(false);
                    break;
                case EnumRobotPanel.ConnectedBody:
                    robot_UnitImages[0].SetPositionAndEulerAngle();
                    robot_UnitImages[1].SetPositionAndEulerAngle();
                    robot_UnitImages[4].SetImageAltha(false);
                    break;
                case EnumRobotPanel.ConnectedLeftarm:
                    robot_UnitImages[0].SetPositionAndEulerAngle();
                    robot_UnitImages[1].SetPositionAndEulerAngle();
                    robot_UnitImages[2].SetPositionAndEulerAngle();
                    robot_UnitImages[4].SetImageAltha(false);
                    break;
                case EnumRobotPanel.ConnectedRightarm:
                    robot_UnitImages[0].SetPositionAndEulerAngle();
                    robot_UnitImages[1].SetPositionAndEulerAngle();
                    robot_UnitImages[2].SetPositionAndEulerAngle();
                    robot_UnitImages[3].SetPositionAndEulerAngle();
                    robot_UnitImages[4].SetPositionAndEulerAngle();
                    robot_UnitImages[4].SetImageAltha(false);
                    break;
                case EnumRobotPanel.Full:
                    robot_UnitImages[0].SetPositionAndEulerAngle();
                    robot_UnitImages[1].SetPositionAndEulerAngle();
                    robot_UnitImages[2].SetPositionAndEulerAngle();
                    robot_UnitImages[3].SetPositionAndEulerAngle();
                    robot_UnitImages[4].SetPositionAndEulerAngle();
                    robot_UnitImages[4].SetImageAltha(true);
                    break;
                default:

                    break;
            }
        }
    }
}

