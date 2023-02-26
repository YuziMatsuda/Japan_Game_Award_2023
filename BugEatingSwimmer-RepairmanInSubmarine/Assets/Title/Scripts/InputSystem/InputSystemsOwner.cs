using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Title.Common;
using UnityEngine.InputSystem;

namespace Title.InputSystem
{
    /// <summary>
    /// InputSystemのオーナー
    /// </summary>
    public class InputSystemsOwner : MonoBehaviour, ITitleGameManager
    {
        /// <summary>ゲームパッド</summary>
        private Gamepad _gamepad;
        /// <summary>左モーター（低周波）の回転数</summary>
        [SerializeField] private float leftMotor = .8f;
        /// <summary>右モーター（高周波）の回転数</summary>
        [SerializeField] private float rightMotor = 0f;
        /// <summary>振動を停止するまでの時間</summary>
        [SerializeField] private float delayTime = .3f;

        public void OnStart()
        {
            // ゲームパッドの情報をセット
            _gamepad = Gamepad.current;
        }

        /// <summary>
        /// 振動の再生
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool PlayVibration()
        {
            try
            {
                if (_gamepad != null)
                    _gamepad.SetMotorSpeeds(leftMotor, rightMotor);
                DOVirtual.DelayedCall(delayTime, () =>
                {
                    if (!StopVibration())
                        Debug.LogError("振動停止の失敗");
                });
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
                return false;
            }
        }

        /// <summary>
        /// 振動停止
        /// </summary>
        /// <returns>成功／失敗</returns>
        private bool StopVibration()
        {
            try
            {
                _gamepad.ResetHaptics();

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
                return false;
            }
        }
    }
}
