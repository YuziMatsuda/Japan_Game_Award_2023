using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Title.Common;

namespace Title.Model
{
    /// <summary>
    /// モデル
    /// バイブレーション機能ラジオボタンのOFF
    /// </summary>
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(EventTrigger))]
    public class OffVibrationModel : UIEventController
    {
        /// <summary>ボタン</summary>
        private Button _button;
        /// <summary>イベントトリガー</summary>
        private EventTrigger _eventTrigger;

        /// <summary>
        /// 選択する
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool Select()
        {
            try
            {
                _eventSystem.SetSelectedGameObject(gameObject);
                _eventState.Value = (int)EnumEventCommand.Selected;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// ボタンのステータスを変更
        /// </summary>
        /// <param name="enabled">有効／無効</param>
        /// <returns>成功／失敗</returns>
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

        /// <summary>
        /// イベントトリガーのステータスを変更
        /// </summary>
        /// <param name="enabled">有効／無効</param>
        /// <returns>成功／失敗</returns>
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
