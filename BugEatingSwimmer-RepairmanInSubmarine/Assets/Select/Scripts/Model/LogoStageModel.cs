using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Select.Common;
using UniRx;
using System.Linq;

namespace Select.Model
{
    /// <summary>
    /// モデル
    /// ロゴステージ
    /// </summary>
    public class LogoStageModel : UIEventController, ILogoStageModel
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
        /// <summary>ステージの状態</summary>
        private readonly IntReactiveProperty stageState = new IntReactiveProperty();
        /// <summary>ステージの状態</summary>
        public IReactiveProperty<int> StageState => stageState;
        /// <summary>エラー状態のナビゲーション（未クリア）</summary>
        [SerializeField] private Navigation navigationOfError;
        /// <summary>IT実施後のナビゲーション</summary>
        [SerializeField] private Navigation navigationOfIT;
        /// <summary>エリア内の最終ステージか</summary>
        [SerializeField] private bool isFinalInArea;

        private void Reset()
        {
            navigationOfError = GetComponent<Button>().navigation;
            navigationOfIT = GetComponent<Button>().navigation;
        }

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
                var areaUnits = SelectGameManager.Instance.SceneOwner.GetAreaUnits();
                var areaOpenedAndITState = SelectGameManager.Instance.SceneOwner.GetAreaOpenedAndITState();
                var currentUnitID = areaUnits.Where(q => q[EnumAreaUnits.StageID] == Index)
                    .Select(q => q[EnumAreaUnits.UnitID])
                    .Distinct()
                    .ToArray();
                var currentState = areaOpenedAndITState.Where(q => q.ContainsKey(EnumAreaOpenedAndITState.UnitID) &&
                    int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == currentUnitID[0])
                    .Select(q => q[EnumAreaOpenedAndITState.State])
                    .ToArray()[0];
                // 未クリアの場合次のステージへナビゲーションしない
                if (stageState.Value != 2)
                {
                    if (_button == null)
                        _button = GetComponent<Button>();
                    _button.navigation = navigationOfError;
                }
                else if (isFinalInArea &&
                    ((EnumAreaOpenedAndITStateState)int.Parse(currentState)).Equals(EnumAreaOpenedAndITStateState.ITFixed))
                {
                    // 対象ステージに紐づくエリア状態が結合済みかつ、最終ステージの場合IT済み後のボタンナビゲーションへ更新
                    if (_button == null)
                        _button = GetComponent<Button>();
                    _button.navigation = navigationOfIT;
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
    /// モデル
    /// ロゴステージ
    /// インターフェース
    /// </summary>
    public interface ILogoStageModel
    {
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
