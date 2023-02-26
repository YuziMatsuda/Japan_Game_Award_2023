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
        /// <summary>
        /// Moveのアクションに応じて移動量を更新
        /// 左移動用
        /// </summary>
        /// <param name="context">コールバック</param>
        public void OnMovedLeft(InputAction.CallbackContext context)
        {
            if (_moved.Equals(Vector2.right) && !context.ReadValueAsButton())
                return;
            _moved = context.ReadValueAsButton() ? Vector2.left : Vector2.zero;
        }

        /// <summary>
        /// Moveのアクションに応じて移動量を更新
        /// 右移動用
        /// </summary>
        /// <param name="context">コールバック</param>
        public void OnMovedRight(InputAction.CallbackContext context)
        {
            if (_moved.Equals(Vector2.left) && !context.ReadValueAsButton())
                return;
            _moved = context.ReadValueAsButton() ? Vector2.right : Vector2.zero;
        }

        /// <summary>ジャンプ入力</summary>
        private bool _jumped;
        /// <summary>ジャンプ入力</summary>
        public bool Jumped => _jumped;
        /// <summary>
        /// Jumpのアクションに応じてフラグを更新
        /// </summary>
        /// <param name="context">コールバック</param>
        public void OnJumped(InputAction.CallbackContext context)
        {
            if (context.phase.Equals(InputActionPhase.Started))
            {
                _jumped = true;
                if (this != null)
                    // Destroyされたクラスからは呼ばない
                    StartCoroutine(WaitAndDisabledJumped());
            }
        }

        /// <summary>ジャンプ入力の時間差リセット</summary>
        private bool _isWaitAndDisabledJumped;

        /// <summary>
        /// ジャンプ入力の時間差リセット
        /// </summary>
        /// <returns>コルーチン</returns>
        private IEnumerator WaitAndDisabledJumped()
        {
            if (_isWaitAndDisabledJumped)
                yield return null;

            _isWaitAndDisabledJumped = true;
            yield return new WaitForSeconds(.1f);
            _jumped = false;
        }

        public void DisableAll()
        {
            _moved = new Vector2();
            _jumped = false;
        }
    }
}
