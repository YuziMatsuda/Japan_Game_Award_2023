using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

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


        void Start()
        {
            SetPositionAndEulerAngle();
        }

        public void SetPositionAndEulerAngle()
        {
            switch (enumrobotpanel)
            {
                case EnumRobotPanel.FallingApart:
                    break;
                case EnumRobotPanel.ConnectedBody:
                    robot_UnitImages[0].SetPositionAndEulerAngle();
                    robot_UnitImages[1].SetPositionAndEulerAngle();
                    break;
                case EnumRobotPanel.ConnectedLeftarm:
                    robot_UnitImages[0].SetPositionAndEulerAngle();
                    robot_UnitImages[1].SetPositionAndEulerAngle();
                    robot_UnitImages[2].SetPositionAndEulerAngle();
                    break;
                case EnumRobotPanel.ConnectedRightarm:
                    robot_UnitImages[0].SetPositionAndEulerAngle();
                    robot_UnitImages[1].SetPositionAndEulerAngle();
                    robot_UnitImages[2].SetPositionAndEulerAngle();
                    robot_UnitImages[3].SetPositionAndEulerAngle();
                    robot_UnitImages[4].SetPositionAndEulerAngle();
                    break;
                case EnumRobotPanel.Full:
                    robot_UnitImages[0].SetPositionAndEulerAngle();
                    robot_UnitImages[1].SetPositionAndEulerAngle();
                    robot_UnitImages[2].SetPositionAndEulerAngle();
                    robot_UnitImages[3].SetPositionAndEulerAngle();
                    robot_UnitImages[4].SetPositionAndEulerAngle();
                    // シーン読み込み時のアニメーション
                    Observable.FromCoroutine<bool>(observer => robot_UnitImages[4].PlayFadeAnimation(observer, true))
                        .Subscribe(_ =>
                        {
                        })
                        .AddTo(gameObject);
                    break;
                default:

                    break;
            }
        }
    }
}

