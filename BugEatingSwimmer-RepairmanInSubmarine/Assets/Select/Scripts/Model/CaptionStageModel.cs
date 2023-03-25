using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Select.Model
{
    /// <summary>
    /// モデル
    /// ステージキャプション
    /// </summary>
    public class CaptionStageModel : UIEventController, ILogoStageModel
    {
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
}
