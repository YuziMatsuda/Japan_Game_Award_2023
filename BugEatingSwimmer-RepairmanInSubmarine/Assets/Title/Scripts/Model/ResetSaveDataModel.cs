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
    /// セーブデータ消去
    /// </summary>
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(EventTrigger))]
    public class ResetSaveDataModel : UIEventController
    {
        /// <summary>ボタン</summary>
        private Button _button;
        /// <summary>イベントトリガー</summary>
        private EventTrigger _eventTrigger;

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

        /// <summary>
        /// イベント状態をセット
        /// </summary>
        /// <param name="state">入力イベント</param>
        /// <returns>成功／失敗</returns>
        public bool SetEventState(EnumEventCommand state)
        {
            try
            {
                _eventState.Value = (int)state;

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
