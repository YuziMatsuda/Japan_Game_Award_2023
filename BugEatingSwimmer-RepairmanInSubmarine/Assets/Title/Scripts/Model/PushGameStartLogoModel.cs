using System.Collections;
using System.Collections.Generic;
using Title.Common;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Title.Model
{
    /// <summary>
    /// モデル
    /// プッシュゲームスタート
    /// </summary>
    public class PushGameStartLogoModel : UIEventController
    {
        /// <summary>
        /// プレゼンタから開始タイミングを制御
        /// </summary>
        public bool OnStart()
        {
            try
            {
                this.UpdateAsObservable()
                    .Subscribe(_ =>
                    {
                        if ((Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame) ||
                            (Gamepad.current != null && (Gamepad.current.buttonSouth.wasPressedThisFrame ||
                                Gamepad.current.buttonNorth.wasPressedThisFrame ||
                                Gamepad.current.buttonEast.wasPressedThisFrame ||
                                Gamepad.current.buttonWest.wasPressedThisFrame ||
                                Gamepad.current.leftShoulder.wasPressedThisFrame ||
                                Gamepad.current.rightShoulder.wasPressedThisFrame ||
                                Gamepad.current.leftTrigger.wasPressedThisFrame ||
                                Gamepad.current.rightTrigger.wasPressedThisFrame ||
                                Gamepad.current.startButton.wasPressedThisFrame ||
                                Gamepad.current.selectButton.wasPressedThisFrame)))
                        {
                            _eventState.Value = (int)EnumEventCommand.AnyKeysPushed;
                        }
                    });

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }
    }
}
