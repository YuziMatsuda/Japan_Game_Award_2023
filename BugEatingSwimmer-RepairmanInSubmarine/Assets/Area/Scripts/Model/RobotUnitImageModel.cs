using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Area.View;
using UniRx;
using UnityEngine.UI;
using Area.Common;
using System.Linq;
using UnityEngine.EventSystems;

namespace Area.Model
{
    /// <summary>
    /// 各ユニットの制御
    /// モデル
    /// </summary>
    [RequireComponent(typeof(RobotUnitImageConfig))]
    public class RobotUnitImageModel : UIEventController, IRobotUnitImageModel
    {
        /// <summary>各ユニットの制御設定</summary>
        [SerializeField] private RobotUnitImageConfig robotUnitImageConfig;
        /// <summary>各ユニットの制御設定</summary>
        public RobotUnitImageConfig RobotUnitImageConfig => robotUnitImageConfig;
        /// <summary>ステージの状態</summary>
        private readonly IntReactiveProperty stageState = new IntReactiveProperty();
        /// <summary>ステージの状態</summary>
        public IReactiveProperty<int> StageState => stageState;

        private void Reset()
        {
            robotUnitImageConfig = GetComponent<RobotUnitImageConfig>();
        }

        public bool LoadStateAndUpdateNavigation()
        {
            try
            {
                var areaOpenedAndITState = new AreaPresenterCommon().LoadSaveDatasCSVAndGetAreaOpenedAndITState();
                stageState.Value = int.Parse(areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == (int)robotUnitImageConfig.EnumUnitID)
                    .Select(q => q[EnumAreaOpenedAndITState.State])
                    .ToArray()[0]);
                var enumRobotPanel = new AreaPresenterCommon().GetStateOfRobotUnit();
                switch (enumRobotPanel)
                {
                    case EnumRobotPanel.FallingApart:
                        // 何もしない
                        break;
                    case EnumRobotPanel.OnStartBody:
                        if (_button == null)
                            _button = GetComponent<Button>();
                        if (robotUnitImageConfig.EnumUnitID.Equals(EnumUnitID.Head))
                            _button.navigation = robotUnitImageConfig.Navigations[0];
                        break;
                    case EnumRobotPanel.ConnectedHead:
                        if (_button == null)
                            _button = GetComponent<Button>();
                        if (robotUnitImageConfig.EnumUnitID.Equals(EnumUnitID.Head))
                            _button.navigation = robotUnitImageConfig.Navigations[1];
                        if (robotUnitImageConfig.EnumUnitID.Equals(EnumUnitID.Body))
                            _button.navigation = robotUnitImageConfig.Navigations[0];
                        break;
                    case EnumRobotPanel.ConnectedRightarm:
                        if (_button == null)
                            _button = GetComponent<Button>();
                        if (robotUnitImageConfig.EnumUnitID.Equals(EnumUnitID.Head))
                            _button.navigation = robotUnitImageConfig.Navigations[2];
                        else if (robotUnitImageConfig.EnumUnitID.Equals(EnumUnitID.Body))
                            _button.navigation = robotUnitImageConfig.Navigations[1];
                        break;
                    case EnumRobotPanel.Full:
                        if (_button == null)
                            _button = GetComponent<Button>();
                        if (robotUnitImageConfig.EnumUnitID.Equals(EnumUnitID.Head))
                            _button.navigation = robotUnitImageConfig.Navigations[2];
                        else if (robotUnitImageConfig.EnumUnitID.Equals(EnumUnitID.Body))
                            _button.navigation = robotUnitImageConfig.Navigations[2];
                        else if (robotUnitImageConfig.EnumUnitID.Equals(EnumUnitID.LeftArm))
                            _button.navigation = robotUnitImageConfig.Navigations[0];
                        break;
                    default:
                        throw new System.Exception("例外エラー");
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

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

    /// <summary>
    /// 各ユニットの制御
    /// モデル
    /// インターフェース
    /// </summary>
    public interface IRobotUnitImageModel
    {
        /// <summary>
        /// ステージ状態のロード及びナビゲーション更新
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool LoadStateAndUpdateNavigation();
        /// <summary>
        /// ボタンのステータスを変更
        /// </summary>
        /// <param name="enabled">有効／無効</param>
        /// <returns>成功／失敗</returns>
        public bool SetButtonEnabled(bool enabled);

        /// <summary>
        /// イベントトリガーのステータスを変更
        /// </summary>
        /// <param name="enabled">有効／無効</param>
        /// <returns>成功／失敗</returns>
        public bool SetEventTriggerEnabled(bool enabled);
    }
}
