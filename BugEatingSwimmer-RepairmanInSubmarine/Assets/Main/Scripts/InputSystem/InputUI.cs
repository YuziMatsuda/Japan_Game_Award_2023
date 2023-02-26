using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Main.InputSystem
{
    /// <summary>
    /// UI用のInputAction
    /// </summary>
    public class InputUI : MonoBehaviour, IInputSystemsOwner
    {
        /// <summary>ナビゲーション入力</summaryf>
        private Vector2 _navigated;
        /// <summary>ナビゲーション入力</summaryf>
        public Vector2 Navigated => _navigated;
        /// <summary>
        /// Navigationのアクションに応じてフラグを更新
        /// </summary>
        /// <param name="context">コールバック</param>
        public void OnNavigated(InputAction.CallbackContext context)
        {
            _navigated = context.ReadValue<Vector2>();
        }

        /// <summary>決定入力</summary>
        private bool _submited;
        /// <summary>決定入力</summary>
        public bool Submited => _submited;
        /// <summary>
        /// Pauseのアクションに応じてフラグを更新
        /// </summary>
        /// <param name="context">コールバック</param>
        public void OnSubmited(InputAction.CallbackContext context)
        {
            _submited = context.ReadValueAsButton();
        }

        /// <summary>キャンセル入力</summary>
        private bool _canceled;
        /// <summary>キャンセル入力</summary>
        public bool Canceled => _canceled;
        /// <summary>
        /// Cancelのアクションに応じてフラグを更新
        /// </summary>
        /// <param name="context">コールバック</param>
        public void OnCanceled(InputAction.CallbackContext context)
        {
            _canceled = context.ReadValueAsButton();
        }

        /// <summary>ポーズ入力</summary>
        private bool _paused;
        /// <summary>ポーズ入力</summary>
        public bool Paused => _paused;
        /// <summary>
        /// Pauseのアクションに応じてフラグを更新
        /// </summary>
        /// <param name="context">コールバック</param>
        public void OnPaused(InputAction.CallbackContext context)
        {
            _paused = context.ReadValueAsButton();
        }

        /// <summary>スペース入力</summary>
        private bool _spaced;
        /// <summary>スペース入力</summary>
        public bool Spaced => _spaced;
        /// <summary>
        /// Spaceのアクションに応じてフラグを更新
        /// </summary>
        /// <param name="context">コールバック</param>
        public void OnSpaced(InputAction.CallbackContext context)
        {
            _spaced = context.ReadValueAsButton();
        }

        /// <summary>アンドゥ入力</summary>
        private bool _undoed;
        /// <summary>アンドゥ入力</summary>
        public bool Undoed => _undoed;
        /// <summary>
        /// Undoのアクションに応じてフラグを更新
        /// </summary>
        /// <param name="context">コールバック</param>
        public void OnUndoed(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    _undoed = true;
                    break;
                case InputActionPhase.Canceled:
                    _undoed = false;
                    break;
            }
        }

        /// <summary>セレクト入力</summary>
        private bool _selected;
        /// <summary>セレクト入力</summary>
        public bool Selected => _selected;
        /// <summary>
        /// Selectのアクションに応じてフラグを更新
        /// </summary>
        /// <param name="context">コールバック</param>
        public void OnSelected(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    _selected = true;
                    break;
                case InputActionPhase.Canceled:
                    _selected = false;
                    break;
            }
        }

        /// <summary>マニュアル入力</summary>
        private bool _manualed;
        /// <summary>マニュアル入力</summary>
        public bool Manualed => _manualed;
        /// <summary>
        /// Manualのアクションに応じてフラグを更新
        /// </summary>
        /// <param name="context">コールバック</param>
        public void OnManualed(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    _manualed = true;
                    break;
                case InputActionPhase.Canceled:
                    _manualed = false;
                    break;
            }
        }

        public void DisableAll()
        {
            _navigated = new Vector2();
            _submited = false;
            _canceled = false;
            _paused = false;
            _spaced = false;
            _undoed = false;
            _selected = false;
            _manualed = false;
        }
    }
}
