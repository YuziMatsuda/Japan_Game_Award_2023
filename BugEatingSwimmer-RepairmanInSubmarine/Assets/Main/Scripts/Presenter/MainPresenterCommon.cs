using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Model;
using Main.View;
using UniRx;
using Main.Template;
using System.Linq;

namespace Main.Common
{
    /// <summary>
    /// ミッション
    /// </summary>
    [System.Serializable]
    public struct Mission
    {
        /// <summary>ミッションID</summary>
        public EnumMissionID enumMissionID;
    }

    /// <summary>
    /// プレゼンタの共通処理
    /// </summary>
    public class MainPresenterCommon : IMainPresenterCommon
    {
        public bool SetDisableAllNodeCode(Transform[] transforms, bool onTriggerEnter2DDisabled)
        {
            try
            {
                if (transforms != null)
                {
                    foreach (var item in transforms)
                    {
                        if (item.GetComponent<StartNodeModel>() != null)
                        {
                            if (!item.GetComponent<StartNodeModel>().SetOnTriggerEnter2DDisabled(onTriggerEnter2DDisabled))
                                throw new System.Exception("OnTriggerEnter判定をセット呼び出しの失敗");
                        }
                        if (item.GetComponent<PivotModel>() != null)
                        {
                            if (!item.GetComponent<PivotModel>().SetOnTriggerEnter2DDisabled(onTriggerEnter2DDisabled))
                                throw new System.Exception("OnTriggerEnter判定をセット呼び出しの失敗");
                        }
                    }
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool IsInductiveMethod(GameObject transform)
        {
            return transform != null &&
                transform.GetComponent<PivotConfig>() != null &&
                transform.GetComponent<PivotConfig>().EnumAtomicMode.Equals(EnumAtomicMode.Molecules) &&
                transform.GetComponent<GoalNodeModel>() != null &&
                !transform.GetComponent<GoalNodeModel>().IsGetting.Value;
        }

        public bool ResetAllPostingState(Transform[] transforms)
        {
            try
            {
                if (transforms != null)
                {
                    foreach (var item in transforms)
                    {
                        if (item.GetComponent<StartNodeView>() != null)
                        {
                            if (!item.GetComponent<StartNodeModel>().SetIsPosting(false))
                                throw new System.Exception("信号発生アニメーション実行中フラグをセット呼び出しの失敗");
                            if (!item.GetComponent<StartNodeModel>().SetToListLength(-1))
                                throw new System.Exception("POST先のノードコードリスト数をセット呼び出しの失敗");
                        }
                        if (item.GetComponent<PivotView>() != null)
                        {
                            if (!item.GetComponent<PivotModel>().SetIsPosting(false))
                                throw new System.Exception("信号発生アニメーション実行中フラグをセット呼び出しの失敗");
                            if (!item.GetComponent<PivotModel>().SetToListLength(-1))
                                throw new System.Exception("POST先のノードコードリスト数をセット呼び出しの失敗");
                        }
                        if (item.GetComponent<GoalNodeView>() != null)
                        {
                            if (!item.GetComponent<GoalNodeModel>().SetIsPosting(false))
                                throw new System.Exception("信号発生アニメーション実行中フラグをセット呼び出しの失敗");
                        }
                    }
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetCounterBetweenAndFillAmount(SeastarGageView seastarGageView, IntReactiveProperty seastarGageCount)
        {
            return SetCounterBetweenAndFillAmount(seastarGageView, seastarGageCount, -1);
        }

        public bool SetCounterBetweenAndFillAmount(SeastarGageView seastarGageView, IntReactiveProperty seastarGageCount, int addCountValue)
        {
            try
            {
                if (seastarGageView != null)
                {
                    if (addCountValue == 1)
                        seastarGageCount.Value += addCountValue;
                    else if (addCountValue == 0)
                        seastarGageCount.Value = addCountValue;
                    else
                    {
                        // 更新なし
                    }
                    if (!seastarGageView.SetCounterBetween(seastarGageCount.Value))
                        throw new System.Exception("カウンターをセット呼び出しの失敗");
                    float seastarGageCountValue = (float)seastarGageCount.Value;
                    if (!seastarGageView.SetFillAmount(seastarGageCountValue))
                        throw new System.Exception("フィルターゲージの値をセット呼び出しの失敗");
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool PlayCounterBetweenAndFillAmountAnimation(SeastarGageView seastarGageView, IntReactiveProperty seastarGageCount)
        {
            return PlayCounterBetweenAndFillAmountAnimation(seastarGageView, seastarGageCount, -1);
        }

        public bool PlayCounterBetweenAndFillAmountAnimation(SeastarGageView seastarGageView, IntReactiveProperty seastarGageCount, int addCountValue)
        {
            try
            {
                if (seastarGageView != null)
                {
                    if (addCountValue == 1)
                        seastarGageCount.Value += addCountValue;
                    else if (addCountValue == 0)
                        seastarGageCount.Value = addCountValue;
                    else
                    {
                        // 更新なし
                    }
                    if (!seastarGageView.PlayCounterBetweenAnimation(seastarGageCount.Value))
                        throw new System.Exception("カウンターをセット呼び出しの失敗");
                    float seastarGageCountValue = (float)seastarGageCount.Value;
                    if (!seastarGageView.PlayFillAmountAnimation(seastarGageCountValue))
                        throw new System.Exception("フィルターゲージの値をセット呼び出しの失敗");
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SaveDatasCSVOfAreaOpenedAndITStateAndOfMission()
        {
            try
            {
                // ステージのクリア状態をチェック
                var temp = new MainTemplateResourcesAccessory();
                var mainSceneStagesState = temp.GetMainSceneStagesState(temp.LoadSaveDatasCSV(ConstResorcesNames.MAIN_SCENE_STAGES_STATE));
                // 各エリアのステージがクリア済みならエリアのステータスを更新する
                var areaOpenedAndITState = temp.GetAreaOpenedAndITState(temp.LoadSaveDatasCSV(ConstResorcesNames.AREA_OPENED_AND_IT_STATE));
                var areaUnitsDefault = temp.GetAreaUnits(temp.LoadSaveDatasCSV(ConstResorcesNames.AREA_UNITS));
                foreach (var unitID in areaUnitsDefault.Select(q => q[EnumAreaUnits.UnitID])
                    .Distinct())
                {
                    bool allCleared = false;
                    foreach (var stageID in areaUnitsDefault.Where(q => q[EnumAreaUnits.UnitID] == unitID)
                        .Select(q => q[EnumAreaUnits.StageID]))
                        allCleared = mainSceneStagesState[stageID][EnumMainSceneStagesState.State] == 2;
                    if (allCleared)
                    {
                        if (0 < areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == unitID &&
                            int.Parse(q[EnumAreaOpenedAndITState.State]) < (int)EnumAreaOpenedAndITStateState.Cleared)
                            .Select(q => q)
                            .ToArray().Length)
                            areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == unitID &&
                                int.Parse(q[EnumAreaOpenedAndITState.State]) < (int)EnumAreaOpenedAndITStateState.Cleared)
                                .Select(q => q)
                                .ToArray()[0][EnumAreaOpenedAndITState.State] = $"{(int)EnumAreaOpenedAndITStateState.Cleared}";
                        // 最終エリアでないなら次のエリアを1にする
                        if (unitID != (int)EnumUnitID.Body)
                        {
                            var nextUnitID = unitID + 1;
                            areaOpenedAndITState = UpdateEnumAreaOpenedAndITStateStateToSelect(areaOpenedAndITState, nextUnitID);
                        }
                        else
                        {
                            var nextUnitID = unitID;
                            // ボディの場合はライトアーム／レフトアームのエリアを解放
                            for (var i = 0; i < 2; i++)
                            {
                                nextUnitID++;
                                areaOpenedAndITState = UpdateEnumAreaOpenedAndITStateStateToSelect(areaOpenedAndITState, nextUnitID);
                            }
                        }
                    }
                }
                if (!temp.SaveDatasCSVOfAreaOpenedAndITState(ConstResorcesNames.AREA_OPENED_AND_IT_STATE, areaOpenedAndITState))
                    throw new System.Exception("エリア解放・結合テスト済みデータをCSVデータへ保存呼び出しの失敗");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }

        }

        /// <summary>
        /// エリア解放・結合テスト済みデータを更新する
        /// 非選択状態を選択可能状態へ
        /// </summary>
        /// <param name="areaOpenedAndITState">エリア解放・結合テスト済みデータ</param>
        /// <param name="nextUnitID">次のエリアID</param>
        /// <returns></returns>
        private Dictionary<EnumAreaOpenedAndITState, string>[] UpdateEnumAreaOpenedAndITStateStateToSelect(Dictionary<EnumAreaOpenedAndITState, string>[] areaOpenedAndITState, int nextUnitID)
        {
            if (0 < areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == nextUnitID &&
                int.Parse(q[EnumAreaOpenedAndITState.State]) < (int)EnumAreaOpenedAndITStateState.Select)
                .Select(q => q)
                .ToArray().Length)
                areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == nextUnitID &&
                    int.Parse(q[EnumAreaOpenedAndITState.State]) < (int)EnumAreaOpenedAndITStateState.Select)
                    .Select(q => q)
                    .ToArray()[0][EnumAreaOpenedAndITState.State] = $"{(int)EnumAreaOpenedAndITStateState.Select}";

            return areaOpenedAndITState;
        }

        public int AddMissionHistory()
        {
            try
            {
                List<EnumMissionID> addHistoryList = new List<EnumMissionID>();
                var temp = new MainTemplateResourcesAccessory();
                // 実績一覧管理を取得
                var mission = temp.GetMission(temp.LoadSaveDatasCSV(ConstResorcesNames.MISSION));
                // 実績履歴を取得
                var histories = temp.GetMissionHistory(temp.LoadSaveDatasCSV(ConstResorcesNames.MISSION_HISTORY));
                var missionOwner = MainGameManager.Instance.MissionOwner;
                // 実績一覧管理のアンロック解除している実績IDが履歴に見つからない場合は演出を実行する
                foreach (var item in mission.Where(q => q[EnumMission.CategoryID].Equals($"{EnumCategoryID.CA0000}") &&
                    q[EnumMission.Unlock].Equals(ConstGeneric.DIGITFORM_TRUE))
                    .Select(q => q))
                    if (histories.Where(q => q[EnumMissionHistory.History].Equals($"{item[EnumMission.MissionID]}"))
                        .Select(q => q)
                        .ToArray().Length < 1)
                        addHistoryList.AddRange(missionOwner.Missions.Where(q => $"{q.enumMissionID}".Equals(item[EnumMission.MissionID]))
                            .Select(q => q.enumMissionID)
                            .ToArray());
                var historyList = histories.ToList();
                foreach (var item in addHistoryList)
                {
                    Dictionary<EnumMissionHistory, string> m = new Dictionary<EnumMissionHistory, string>();
                    m[EnumMissionHistory.History] = $"{item}";
                    historyList.Add(m);
                }
                if (!temp.SaveDatasCSVOfMissionHistory(ConstResorcesNames.MISSION_HISTORY, historyList.ToArray()))
                    throw new System.Exception("実績履歴データをCSVデータへ保存呼び出しの失敗");

                return addHistoryList.Count;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return -1;
            }
        }

        public bool IsTutorialMode(Dictionary<EnumSystemCommonCash, int> currentStageDic, Dictionary<EnumMainSceneStagesState, int>[] mainSceneStagesState)
        {
            return (0 < currentStageDic[EnumSystemCommonCash.SceneId] &&
                            mainSceneStagesState[currentStageDic[EnumSystemCommonCash.SceneId]][EnumMainSceneStagesState.State] < 2) ||
                        currentStageDic[EnumSystemCommonCash.SceneId] == 0;
        }

        public bool IsFinalLevelOrEndOfAreaAndNotReadedScenario()
        {
            var temp = new MainTemplateResourcesAccessory();
            // 現在選択されているステージ番号を取得
            var systemCommonCash = MainGameManager.Instance.SceneOwner.GetSystemCommonCash();
            // 最終ステージかどうか
            var areaUnits = temp.GetAreaUnits(temp.LoadSaveDatasCSV(ConstResorcesNames.AREA_UNITS));
            if (areaUnits.Select(q => q[EnumAreaUnits.StageID])
                .OrderByDescending(q => q)
                .ToArray()[0] == systemCommonCash[EnumSystemCommonCash.SceneId])
                return true;

            // エリアの最後かどうか
            var currentAreaID = areaUnits.Where(q => q[EnumAreaUnits.StageID] == systemCommonCash[EnumSystemCommonCash.SceneId])
                .Select(q => q[EnumAreaUnits.UnitID])
                .ToArray()[0];
            if (areaUnits.Where(q => q[EnumAreaUnits.UnitID] == currentAreaID)
                .Select(q => q[EnumAreaUnits.StageID])
                .OrderByDescending(q => q)
                .ToArray()[0] == systemCommonCash[EnumSystemCommonCash.SceneId])
            {
                // ミッションを読んでいない
                // ヒストリーにもない
                var mission = temp.GetMission(temp.LoadSaveDatasCSV(ConstResorcesNames.MISSION));
                var missionHistory = temp.GetMissionHistory(temp.LoadSaveDatasCSV(ConstResorcesNames.MISSION_HISTORY));
                switch ((EnumUnitID)currentAreaID)
                {
                    case EnumUnitID.Head:
                        // MI0001が未読
                        EnumMissionID[] enumMissionIDsHead = { EnumMissionID.MI0001 };
                        return IsEndOfAreaAndNotReadedScenario(enumMissionIDsHead, mission, missionHistory);
                    case EnumUnitID.Body:
                        // MI0002とMI0003が未読
                        EnumMissionID[] enumMissionIDsBody = { EnumMissionID.MI0002, EnumMissionID.MI0003 };
                        return IsEndOfAreaAndNotReadedScenario(enumMissionIDsBody, mission, missionHistory);
                    case EnumUnitID.RightArm:
                        // MI0004が未読
                        EnumMissionID[] enumMissionIDsRightArm = { EnumMissionID.MI0004 };
                        return IsEndOfAreaAndNotReadedScenario(enumMissionIDsRightArm, mission, missionHistory);
                    case EnumUnitID.LeftArm:
                        // MI0006が未読
                        EnumMissionID[] enumMissionIDsLeftArm = { EnumMissionID.MI0006 };
                        return IsEndOfAreaAndNotReadedScenario(enumMissionIDsLeftArm, mission, missionHistory);
                    case EnumUnitID.Core:
                        // MI0007が未読
                        EnumMissionID[] enumMissionIDsCore = { EnumMissionID.MI0007 };
                        return IsEndOfAreaAndNotReadedScenario(enumMissionIDsCore, mission, missionHistory);
                    case EnumUnitID.VoidInCore:
                        // 最終ステージ（未到達？）
                        return true;
                    default:
                        throw new System.Exception("例外エラー");
                }
            }

            return false;
        }

        private bool IsEndOfAreaAndNotReadedScenario(EnumMissionID[] enumMissionIDs, Dictionary<EnumMission, string>[] mission, Dictionary<EnumMissionHistory, string>[] missionHistory)
        {
            if (enumMissionIDs.Length == 1)
            {
                if (0 < mission.Where(q => q[EnumMission.MissionID].Equals($"{enumMissionIDs[0]}"))
                    .Select(q => q)
                    .Where(q => q[EnumMission.Unlock].Equals(ConstGeneric.DIGITFORM_FALSE))
                    .Select(q => q)
                    .ToArray()
                    .Length)
                    // 実績がロック状態
                    return true;
                else if (0 < mission.Where(q => q[EnumMission.MissionID].Equals($"{enumMissionIDs[0]}"))
                    .Select(q => q)
                    .ToArray()
                    .Length)
                {
                    var missionID = mission.Where(q => q[EnumMission.MissionID].Equals($"{enumMissionIDs[0]}"))
                        .Select(q => q)
                        .Where(q => q[EnumMission.Unlock].Equals(ConstGeneric.DIGITFORM_TRUE))
                        .Select(q => q[EnumMission.MissionID])
                        .ToArray()[0];
                    // 状態がアンロック状態だがイベントを見ていない（アンロック後にゲームを強制終了する等）
                    return missionHistory.Where(q => q[EnumMissionHistory.History].Equals(missionID))
                        .Select(q => q)
                        .ToArray()
                        .Length < 1;
                }
                else
                    throw new System.Exception("例外エラー");
            }
            else if (enumMissionIDs.Length == 2)
            {
                foreach (var enumMissionID in enumMissionIDs)
                    if (0 < mission.Where(q => q[EnumMission.MissionID].Equals($"{enumMissionID}"))
                        .Select(q => q)
                        .Where(q => q[EnumMission.Unlock].Equals(ConstGeneric.DIGITFORM_FALSE))
                        .Select(q => q)
                        .ToArray()
                        .Length)
                        // どちらかの実績がロック状態
                        return true;

                foreach (var enumMissionID in enumMissionIDs)
                    if (0 < mission.Where(q => q[EnumMission.MissionID].Equals($"{enumMissionID}"))
                        .Select(q => q)
                        .Where(q => q[EnumMission.Unlock].Equals(ConstGeneric.DIGITFORM_TRUE))
                        .Select(q => q)
                        .ToArray()
                        .Length)
                    {
                        var missionID = mission.Where(q => q[EnumMission.MissionID].Equals($"{enumMissionID}"))
                            .Select(q => q)
                            .Where(q => q[EnumMission.Unlock].Equals(ConstGeneric.DIGITFORM_TRUE))
                            .Select(q => q[EnumMission.MissionID])
                            .ToArray()[0];
                        if (missionHistory.Where(q => q[EnumMissionHistory.History].Equals(missionID))
                            .Select(q => q)
                            .ToArray()
                            .Length < 1)
                            // 状態がアンロック状態だがイベントを見ていない（アンロック後にゲームを強制終了する等）
                            return true;
                    }

                return false;
            }
            else
                throw new System.Exception("例外エラー");
            throw new System.Exception("例外エラー");
        }

        public bool IsOvercounterOfSeastarGage(SeastarGageView seastarGageView)
        {
            return seastarGageView == null ||
                (seastarGageView != null &&
                !seastarGageView.IsCounting);
        }

        public bool IsOvercounterOfShrimpDance(int shrimpGageCount, int shrimpGageCountMax)
        {
            if (shrimpGageCountMax < 1)
                return true;

            return shrimpGageCountMax <= shrimpGageCount;
        }
    }

    /// <summary>
    /// プレゼンタの共通処理
    /// インターフェース
    /// </summary>
    public interface IMainPresenterCommon
    {
        /// <summary>
        /// 帰納法かチェック
        /// </summary>
        /// <param name="transform">オブジェクト</param>
        /// <returns>帰納法モードが有効</returns>
        public bool IsInductiveMethod(GameObject transform);

        /// <summary>
        /// POSTのリセット
        /// スタートノード、コード、ゴールノードが対象
        /// </summary>
        /// <param name="transforms">スタートノード、コード、ゴールノードのトランスフォーム配列</param>
        /// <returns>成功／失敗</returns>
        public bool ResetAllPostingState(Transform[] transforms);

        /// <summary>
        /// ノードコードの衝突判定を無効にする
        /// </summary>
        /// <param name="transforms">スタートノード、コード、ゴールノードのトランスフォーム配列</param>
        /// <param name="onTriggerEnter2DDisabled">無効とするか</param>
        /// <returns>成功／失敗</returns>
        public bool SetDisableAllNodeCode(Transform[] transforms, bool onTriggerEnter2DDisabled);
        /// <summary>
        /// ヒトデゲージのカウンターとフィルターカウンターを更新
        /// </summary>
        /// <param name="seastarGageView">ヒトデゲージのビュー</param>
        /// <param name="seastarGageCount">ヒトデカウンター</param>
        /// <returns>成功／失敗</returns>
        public bool SetCounterBetweenAndFillAmount(SeastarGageView seastarGageView, IntReactiveProperty seastarGageCount);
        /// <summary>
        /// ヒトデゲージのカウンターとフィルターカウンターを更新
        /// </summary>
        /// <param name="seastarGageView">ヒトデゲージのビュー</param>
        /// <param name="seastarGageCount">ヒトデカウンター</param>
        /// <param name="addCountValue">カウンター値（1は加算、0はリセット、-1は更新なし）</param>
        /// <returns>成功／失敗</returns>
        public bool SetCounterBetweenAndFillAmount(SeastarGageView seastarGageView, IntReactiveProperty seastarGageCount, int addCountValue);
        /// <summary>
        /// ヒトデゲージのカウンターとフィルターカウンターを更新アニメーション
        /// </summary>
        /// <param name="seastarGageView">ヒトデゲージのビュー</param>
        /// <param name="seastarGageCount">ヒトデカウンター</param>
        /// <returns>成功／失敗</returns>
        public bool PlayCounterBetweenAndFillAmountAnimation(SeastarGageView seastarGageView, IntReactiveProperty seastarGageCount);
        /// <summary>
        /// ヒトデゲージのカウンターとフィルターカウンターを更新アニメーション
        /// </summary>
        /// <param name="seastarGageView">ヒトデゲージのビュー</param>
        /// <param name="seastarGageCount">ヒトデカウンター</param>
        /// <param name="addCountValue">カウンター値（1は加算、0はリセット、-1は更新なし）</param>
        /// <returns>成功／失敗</returns>
        public bool PlayCounterBetweenAndFillAmountAnimation(SeastarGageView seastarGageView, IntReactiveProperty seastarGageCount, int addCountValue);
        /// <summary>
        /// エリア解放と実績一覧を更新
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool SaveDatasCSVOfAreaOpenedAndITStateAndOfMission();
        /// <summary>
        /// 実績履歴を更新
        /// </summary>
        /// <returns>追加した件数</returns>
        public int AddMissionHistory();
        /// <summary>
        /// チュートリアルモードか
        /// </summary>
        /// <param name="currentStageDic">現在選択されたステージ番号のディクショナリ</param>
        /// <param name="mainSceneStagesState">ステージクリア状態のディクショナリ</param>
        /// <returns></returns>
        public bool IsTutorialMode(Dictionary<EnumSystemCommonCash, int> currentStageDic, Dictionary<EnumMainSceneStagesState, int>[] mainSceneStagesState);
        /// <summary>
        /// 最終ステージである
        /// または、各エリアの最終ステージかつシナリオ未読である
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool IsFinalLevelOrEndOfAreaAndNotReadedScenario();
        /// <summary>
        /// ヒトデゲージが上限を超えているか
        /// </summary>
        /// <param name="seastarGageView">ヒトデゲージのビュー</param>
        /// <returns>超えている</returns>
        public bool IsOvercounterOfSeastarGage(SeastarGageView seastarGageView);
        /// <summary>
        /// エビダンスの数が上限か
        /// </summary>
        /// <param name="shrimpGageCount">エビダンス通過数</param>
        /// <param name="shrimpGageCountMax">エビダンス総数</param>
        /// <returns></returns>
        public bool IsOvercounterOfShrimpDance(int shrimpGageCount, int shrimpGageCountMax);
    }
}
