using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Select.Common;
using UniRx;

namespace Select.Model
{
    /// <summary>
    /// モデル
    /// ロゴステージ
    /// </summary>
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(EventTrigger))]
    public class LogoStageModel : UIEventController
    {
        /// <summary>ステージ番号</summary>
        private int _index = -1;
        /// <summary>番号置換の正規表現</summary>
        private readonly Regex _regex = new Regex("^.*_");
        /// <summary>ステージ番号</summary>
        public int Index
        {
            get
            {
                // ステージ番号セットの初期処理
                if (_index < 0)
                    _index = int.Parse(_regex.Replace(name, ""));
                if (_index < 0)
                    throw new System.Exception("置換失敗");
                return _index;
            }
        }
        /// <summary>ボタン</summary>
        private Button _button;
        /// <summary>イベントトリガー</summary>
        private EventTrigger _eventTrigger;
        /// <summary>ステージの状態</summary>
        private readonly IntReactiveProperty stageState = new IntReactiveProperty();
        /// <summary>ステージの状態</summary>
        public IReactiveProperty<int> StageState => stageState;

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        /// <summary>
        /// ステージ状態のロード及びナビゲーション更新
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool LoadStateAndUpdateNavigation()
        {
            try
            {
                var mainSceneStagesState = SelectGameManager.Instance.SceneOwner.GetMainSceneStagesState();
                stageState.Value = mainSceneStagesState[Index][EnumMainSceneStagesState.State];
                // 未クリアの場合次のステージへナビゲーションしない
                if (stageState.Value != 2)
                {
                    if (_button == null)
                        _button = GetComponent<Button>();
                    Navigation navigation = _button.navigation;
                    navigation.selectOnRight = null;
                    _button.navigation = navigation;
                }

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
