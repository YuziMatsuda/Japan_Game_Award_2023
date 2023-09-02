using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GameManagers.Common;

namespace Select.Common
{
    /// <summary>
    /// ソフトウェアのカーソル制御ビュー
    /// <see href="https://nekojara.city/unity-input-system-virtual-mouse#Canvas%E3%82%B5%E3%82%A4%E3%82%BA%E3%81%AB%E5%9F%BA%E3%81%A5%E3%81%84%E3%81%A6Processor%E3%82%92%E9%81%A9%E7%94%A8%E3%81%99%E3%82%8B%E3%82%B9%E3%82%AF%E3%83%AA%E3%83%97%E3%83%88%E3%81%AE%E5%AE%9F%E8%A3%85">
    /// 【Unity】Input Systemからマウスカーソルを操作する > Canvasサイズに基づいてProcessorを適用するスクリプトの実装
    /// </see>
    /// </summary>
    public class SoftwareCursorPositionAdjusterView : MonoBehaviour, ISoftwareCursorPositionAdjusterView, IGameManager
    {
        /// <summary>VirtualMouseInput</summary>
        [SerializeField] private VirtualMouseInput virtualMouse;
        /// <summary>EventSystemのInputSystemモジュール</summary>
        [SerializeField] private InputSystemUIInputModule inputSystemUIInputModule;
        /// <summary>カーソルのアクティブ速度</summary>
        [SerializeField] private float cursorSpeedActive = 400f;
        /// <summary>カーソルの停止速度</summary>
        [SerializeField] private float cursorSpeedStop = 0f;
        /// <summary>対象のシーン名</summary>
        [SerializeField] private string targetSceneName = "SelectScene";

        public bool ChangeCursorScaleProcessor(float scale)
        {
            try
            {
                if (inputSystemUIInputModule == null)
                    inputSystemUIInputModule = GameObject.Find("EventSystem").GetComponent<InputSystemUIInputModule>();
                inputSystemUIInputModule.point.action.ApplyBindingOverride(new InputBinding
                {
                    overrideProcessors = $"VirtualMouseScaler(scale={scale})"
                });

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public void OnStart()
        {
            if (SceneManager.GetActiveScene().name.Equals(targetSceneName))
            {
                if (inputSystemUIInputModule == null)
                    inputSystemUIInputModule = GameObject.Find("EventSystem").GetComponent<InputSystemUIInputModule>();
                if (virtualMouse.cursorGraphic == null)
                    virtualMouse.cursorGraphic = GameObject.Find("BodyImage").GetComponent<Image>();
                if (virtualMouse.cursorTransform == null)
                    virtualMouse.cursorTransform = GameObject.Find("BodyImage").GetComponent<RectTransform>();
            }
        }

        public bool SetCursorEnabled(bool enabled)
        {
            try
            {
                virtualMouse.enabled = enabled;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetCursorSpeedState(bool isActive)
        {
            try
            {
                virtualMouse.cursorSpeed = isActive ? cursorSpeedActive : cursorSpeedStop;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetCursorTransform(Vector2 anchor)
        {
            try
            {
                if (virtualMouse.cursorTransform == null)
                    virtualMouse.cursorTransform = GameObject.Find("BodyImage").GetComponent<RectTransform>();
                virtualMouse.cursorTransform.position = anchor;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        private void Reset()
        {
            virtualMouse = GetComponent<VirtualMouseInput>();
            if (inputSystemUIInputModule == null)
                inputSystemUIInputModule = GameObject.Find("EventSystem").GetComponent<InputSystemUIInputModule>();
            if (virtualMouse.cursorGraphic == null)
                virtualMouse.cursorGraphic = GameObject.Find("BodyImage").GetComponent<Image>();
            if (virtualMouse.cursorTransform == null)
                virtualMouse.cursorTransform = GameObject.Find("BodyImage").GetComponent<RectTransform>();
        }
    }

    /// <summary>
    /// ソフトウェアのカーソル制御ビュー
    /// インターフェース
    /// </summary>
    public interface ISoftwareCursorPositionAdjusterView
    {
        /// <summary>
        /// VirtualMouseInputのカーソルのスケールを変更するProcessorを適用
        /// </summary>
        /// <param name="scale">スケール</param>
        /// <returns>成功／失敗</returns>
        public bool ChangeCursorScaleProcessor(float scale);
        /// <summary>
        /// カーソルのアンカーをセットする
        /// </summary>
        /// <param name="anchor">アンカー位置</param>
        /// <returns>成功／失敗</returns>
        public bool SetCursorTransform(Vector2 anchor);
        /// <summary>
        /// カーソルのステータスを変更
        /// </summary>
        /// <param name="enabled">有効／無効</param>
        /// <returns>成功／失敗</returns>
        public bool SetCursorEnabled(bool enabled);
        /// <summary>
        /// カーソルの速度を変更
        /// </summary>
        /// <param name="isActive">アクティブ／停止</param>
        /// <returns>成功／失敗</returns>
        public bool SetCursorSpeedState(bool isActive);
    }
}
