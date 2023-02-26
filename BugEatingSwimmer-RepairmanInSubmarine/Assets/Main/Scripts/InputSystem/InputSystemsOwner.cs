using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Common;
using UnityEngine.InputSystem;
using System;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;
using Main.Template;

namespace Main.InputSystem
{
    /// <summary>
    /// InputSystemのオーナー
    /// </summary>
    public class InputSystemsOwner : MonoBehaviour, IMainGameManager
    {
        /// <summary>プレイヤー用のインプットイベント</summary>
        [SerializeField] private InputPlayer inputPlayer;
        /// <summary>プレイヤー用のインプットイベント</summary>
        public InputPlayer InputPlayer => inputPlayer;
        /// <summary>UI用のインプットイベント</summary>
        [SerializeField] private InputUI inputUI;
        /// <summary>UI用のインプットイベント</summary>
        public InputUI InputUI => inputUI;
        /// <summary>インプットアクション</summary>
        private FutureContents3D_Main _inputActions;
        /// <summary>インプットアクション</summary>
        public FutureContents3D_Main InputActions => _inputActions;
        /// <summary>監視管理</summary>
        private CompositeDisposable _compositeDisposable;
        /// <summary>現在の入力モード（コントローラー／キーボード）</summary>
        private IntReactiveProperty _currentInputMode;
        /// <summary>現在の入力モード（コントローラー／キーボード）</summary>
        public IntReactiveProperty CurrentInputMode => _currentInputMode;
        /// <summary>ゲームパッド</summary>
        private Gamepad _gamepad;
        /// <summary>左モーター（低周波）の回転数</summary>
        [SerializeField] private float leftMotor = .8f;
        /// <summary>右モーター（高周波）の回転数</summary>
        [SerializeField] private float rightMotor = 0f;
        /// <summary>振動を停止するまでの時間</summary>
        [SerializeField] private float delayTime = .3f;
        /// <summary>振動を有効フラグ</summary>
        [SerializeField] private bool isVibrationEnabled;

        private void Reset()
        {
            inputPlayer = GetComponent<InputPlayer>();
            inputUI = GetComponent<InputUI>();
        }

        public void OnStart()
        {
            _inputActions = new FutureContents3D_Main();
            _inputActions.Player.MoveLeft.started += inputPlayer.OnMovedLeft;
            _inputActions.Player.MoveLeft.performed += inputPlayer.OnMovedLeft;
            _inputActions.Player.MoveLeft.canceled += inputPlayer.OnMovedLeft;
            _inputActions.Player.MoveRight.started += inputPlayer.OnMovedRight;
            _inputActions.Player.MoveRight.performed += inputPlayer.OnMovedRight;
            _inputActions.Player.MoveRight.canceled += inputPlayer.OnMovedRight;
            _inputActions.Player.Jump.started += inputPlayer.OnJumped;
            _inputActions.Player.Jump.performed += inputPlayer.OnJumped;
            _inputActions.Player.Jump.canceled += inputPlayer.OnJumped;
            _inputActions.UI.Pause.started += inputUI.OnPaused;
            _inputActions.UI.Pause.performed += inputUI.OnPaused;
            _inputActions.UI.Pause.canceled += inputUI.OnPaused;
            _inputActions.UI.Space.started += inputUI.OnSpaced;
            _inputActions.UI.Space.performed += inputUI.OnSpaced;
            _inputActions.UI.Space.canceled += inputUI.OnSpaced;
            _inputActions.UI.Undo.started += inputUI.OnUndoed;
            _inputActions.UI.Undo.canceled += inputUI.OnUndoed;
            _inputActions.UI.Select.started += inputUI.OnSelected;
            _inputActions.UI.Select.canceled += inputUI.OnSelected;
            _inputActions.UI.Manual.started += inputUI.OnManualed;
            _inputActions.UI.Manual.canceled += inputUI.OnManualed;

            _inputActions.Enable();

            _compositeDisposable = new CompositeDisposable();
            _currentInputMode = new IntReactiveProperty((int)InputMode.Gamepad);
            // 入力モード 0:キーボード 1:コントローラー
            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
                    {
                        _currentInputMode.Value = (int)InputMode.Keyboard;
                    }
                    else if (Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame)
                    {
                        _currentInputMode.Value = (int)InputMode.Gamepad;
                    }
                })
                .AddTo(_compositeDisposable);
            // ゲームパッドの情報をセット
            _gamepad = Gamepad.current;

            var tResourcesAccessory = new MainTemplateResourcesAccessory();
            // ステージ共通設定の取得
            var mainSceneStagesConfResources = tResourcesAccessory.LoadSaveDatasCSV(ConstResorcesNames.SYSTEM_CONFIG);
            var mainSceneStagesConfs = tResourcesAccessory.GetSystemConfig(mainSceneStagesConfResources);

            isVibrationEnabled = mainSceneStagesConfs[EnumSystemConfig.VibrationEnableIndex] == 1;
        }

        public bool Exit()
        {
            try
            {
                _inputActions.Disable();
                inputPlayer.DisableAll();
                inputUI.DisableAll();
                _compositeDisposable.Clear();

                return true;
            }
            catch
            {
                return false;
            }
        }

        private void OnDestroy()
        {
            if (!StopVibration())
                Debug.LogError("振動停止の失敗");
        }

        /// <summary>
        /// 振動の再生
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool PlayVibration()
        {
            try
            {
                if (isVibrationEnabled)
                {
                    if (_gamepad != null)
                        _gamepad.SetMotorSpeeds(leftMotor, rightMotor);
                    DOVirtual.DelayedCall(delayTime, () =>
                    {
                        if (!StopVibration())
                            Debug.LogError("振動停止の失敗");
                    });
                }
                else
                    Debug.Log("振動オフ設定済み");
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
                return false;
            }
        }

        /// <summary>
        /// 振動停止
        /// </summary>
        /// <returns>成功／失敗</returns>
        private bool StopVibration()
        {
            try
            {
                _gamepad.ResetHaptics();

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
                return false;
            }
        }
    }

    /// <summary>
    /// 各インプットのインターフェース
    /// </summary>
    public interface IInputSystemsOwner
    {
        /// <summary>
        /// 全ての入力をリセット
        /// </summary>
        public void DisableAll();
    }

    /// <summary>
    /// 入力モード
    /// </summary>
    public enum InputMode
    {
        /// <summary>コントローラー</summary>
        Gamepad,
        /// <summary>キーボード</summary>
        Keyboard,
    }
}
