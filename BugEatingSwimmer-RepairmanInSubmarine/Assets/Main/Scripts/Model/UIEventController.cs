using System.Collections;
using System.Collections.Generic;
using Main.Common;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Main.Model
{
    /// <summary>
    /// イベントコントローラー
    /// </summary>
    public class UIEventController : MonoBehaviour
    {
        /// <summary>
        /// 実行イベントの監視
        /// -1 : Default
        /// 0 : Selected
        /// 1 : DeSelected
        /// 2 : Submited
        /// 3 : Canceled
        /// 4 : AnyKeysPushed
        /// </summary>
        protected readonly IntReactiveProperty _eventState = new IntReactiveProperty((int)EnumEventCommand.Default);
        /// <summary>
        /// 実行イベントの監視
        /// -1 : Default
        /// 0 : Selected
        /// 1 : DeSelected
        /// 2 : Submited
        /// 3 : Canceled
        /// 4 : AnyKeysPushed
        /// </summary>
        public IReactiveProperty<int> EventState => _eventState;
        /// <summary>イベントシステム</summary>
        private EventSystem _eventSystem;

        protected virtual void OnEnable()
        {
            if (!EnumEventCommand.Default.Equals((EnumEventCommand)_eventState.Value))
                _eventState.Value = (int)EnumEventCommand.Default;
        }

        /// <summary>
        /// デフォルト選択切り替え
        /// </summary>
        public void SetSelectedGameObject()
        {
            if (_eventSystem == null)
                _eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
            _eventSystem.SetSelectedGameObject(gameObject);
        }

        /// <summary>
        /// 選択された時に発火するイベント
        /// </summary>
        public void Selected()
        {
            _eventState.Value = (int)EnumEventCommand.Selected;
        }

        /// <summary>
        /// 選択されなかった時に発火するイベント
        /// </summary>
        public void DeSelected()
        {
            _eventState.Value = (int)EnumEventCommand.DeSelected;
        }

        /// <summary>
        /// 確定時に発火するイベント
        /// </summary>
        public void Submited()
        {
            _eventState.Value = (int)EnumEventCommand.Submited;
        }

        /// <summary>
        /// キャンセル時に発火するイベント
        /// </summary>
        public void Canceled()
        {
            _eventState.Value = (int)EnumEventCommand.Canceled;
        }
    }
}
