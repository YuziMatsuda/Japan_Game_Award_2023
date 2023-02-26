using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Title.Common;
using Title.Template;
using UniRx;

namespace Title.Model
{
    /// <summary>
    /// モデル
    /// バイブレーション機能ラジオボタン
    /// </summary>
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(EventTrigger))]
    public class RadioVibrationModel : UIEventController
    {
        /// <summary>ボタン</summary>
        private Button _button;
        /// <summary>イベントトリガー</summary>
        private EventTrigger _eventTrigger;
        /// <summary>振動設定</summary>
        private readonly IntReactiveProperty _vibrationState = new IntReactiveProperty();
        /// <summary>振動設定</summary>
        public IReactiveProperty<int> VibrationState => _vibrationState;

        protected override void OnEnable()
        {
            base.OnEnable();
            var tTResources = new TitleTemplateResourcesAccessory();
            var datas = tTResources.LoadSaveDatasCSV(ConstResorcesNames.SYSTEM_CONFIG);
            var configMap = tTResources.GetSystemConfig(datas);
            _vibrationState.Value = configMap[EnumSystemConfig.VibrationEnableIndex];
        }

        /// <summary>
        /// 振動状態をセット
        /// </summary>
        /// <param name="value">値</param>
        /// <returns>成功／失敗</returns>
        public bool SetVibrationState(int value)
        {
            try
            {
                var tTValidation = new TitleTemplateOptionalInputValueValidation();
                if (tTValidation.CheckVibrationValueAndGetResultState(value).Equals(EnumResponseState.Success))
                    _vibrationState.Value = value;
                else
                    throw new System.Exception("値チェックのエラー");

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
