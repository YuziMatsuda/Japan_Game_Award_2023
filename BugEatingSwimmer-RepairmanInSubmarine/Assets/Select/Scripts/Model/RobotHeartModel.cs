using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Select.Common;

namespace Select.Model
{
    /// <summary>
    /// コア
    /// モデル
    /// </summary>
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(EventTrigger))]
    public class RobotHeartModel : UIEventController, IRobotHeartModel
    {
        /// <summary>選択された場合に遷移するユニット</summary>
        [SerializeField] private EnumUnitID enumUnitID = EnumUnitID.Core;
        /// <summary>選択された場合に遷移するユニット</summary>
        public EnumUnitID EnumUnitID => enumUnitID;

        public bool SetButtonEnabled(bool enabled)
        {
            try
            {
                if (_button == null)
                    _button = GetComponent<Button>();
                _button.enabled = enabled;
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetEventTriggerEnabled(bool enabled)
        {
            try
            {
                if (_eventTrigger == null)
                    _eventTrigger = GetComponent<EventTrigger>();
                _eventTrigger.enabled = enabled;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }
    }

    /// <summary>
    /// コア
    /// モデル
    /// インターフェース
    /// </summary>
    public interface IRobotHeartModel : ISelectContentsModelParent
    {
    }
}
