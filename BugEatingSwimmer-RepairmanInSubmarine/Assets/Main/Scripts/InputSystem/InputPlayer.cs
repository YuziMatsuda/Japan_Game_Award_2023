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

        public void DisableAll()
        {
            _moved = new Vector2();
        }
    }
}
