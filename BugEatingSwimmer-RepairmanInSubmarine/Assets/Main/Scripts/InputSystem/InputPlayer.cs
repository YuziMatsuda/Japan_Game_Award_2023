using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Main.InputSystem
{
    /// <summary>
    /// プレイヤー用のInputAction
    /// </summary>
    public class InputPlayer : MonoBehaviour, IInputSystemsOwner
    {
        /// <summary>移動量</summary>
        private Vector2 _moved;
        /// <summary>移動量</summary>
        public Vector2 Moved => _moved;

        public void OnMoved(InputAction.CallbackContext context)
        {
            _moved = context.ReadValue<Vector2>();
        }

        /// <summary>アタック</summary>
        private bool _attacked;
        /// <summary>アタック</summary>
        public bool Attacked => _attacked;

        public void OnAttacked(InputAction.CallbackContext context)
        {
            if (context.canceled && _attacked)
            {
                _attacked = false;
                return;
            }
            if (!context.performed) return;
            _attacked = true;
        }

        /// <summary>キャンセル</summary>
        private bool _canceled;
        /// <summary>キャンセル</summary>
        public bool Canceled => _canceled;

        public void OnCanceled(InputAction.CallbackContext context)
        {
            if (context.canceled && _canceled)
            {
                _canceled = false;
                return;
            }
            if (!context.performed) return;
            _canceled = true;
        }

        public void DisableAll()
        {
            _moved = new Vector2();
            _attacked = false;
            _canceled = false;
        }
    }
}
