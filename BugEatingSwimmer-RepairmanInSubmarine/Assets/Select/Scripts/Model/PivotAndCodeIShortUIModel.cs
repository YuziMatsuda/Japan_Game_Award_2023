using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Select.Common;

namespace Select.Model
{
    /// <summary>
    /// モデル
    /// 支点とコード
    /// </summary>
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(EventTrigger))]
    public class PivotAndCodeIShortUIModel : UIEventController, IPivotAndCodeIShortUIModel
    {
        public bool LoadStateAndUpdateNavigation()
        {
            throw new System.NotImplementedException();
        }

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
    /// モデル
    /// 支点とコード
    /// インターフェース
    /// </summary>
    public interface IPivotAndCodeIShortUIModel : ISelectContentsModelParent
    {
    }
}
