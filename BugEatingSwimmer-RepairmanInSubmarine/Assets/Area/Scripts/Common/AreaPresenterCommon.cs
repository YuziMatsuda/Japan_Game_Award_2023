using Area.Template;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Select.Model;
using System.Linq;
using Area.View;

namespace Area.Common
{
    /// <summary>
    /// ミッション
    /// </summary>
    [System.Serializable]
    public struct Mission
    {
        /// <summary>ミッションID</summary>
        public EnumMissionID enumMissionID;
        /// <summary>ロボットの結合状態</summary>
        public EnumRobotPanel enumRobotPanel;
        /// <summary>エリアID</summary>
        public EnumUnitID enumUnitID;
    }

    /// <summary>
    /// プレゼンタ
    /// セレクトシーン
    /// </summary>
    public class AreaPresenterCommon : IAreaPresenterCommon
    {
        public EnumRobotPanel GetStateOfRobotUnitConnect()
        {
            var areaOpenedAndITState = LoadSaveDatasCSVAndGetAreaOpenedAndITState();
            var temp = new AreaTemplateResourcesAccessory();
            var mission = temp.GetMission(temp.LoadSaveDatasCSV(ConstResorcesNames.MISSION));
            var history = GetMissionHistories();

            if (0 < areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == 1 &&
                (int)EnumAreaOpenedAndITStateState.ITFixed <= int.Parse(q[EnumAreaOpenedAndITState.State]))
                .Select(q => q)
                .ToArray()
                .Length &&
                0 < areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == 2 &&
                (int)EnumAreaOpenedAndITStateState.Cleared <= int.Parse(q[EnumAreaOpenedAndITState.State]))
                .Select(q => q)
                .ToArray()
                .Length &&
                0 < areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == 3 &&
                (int)EnumAreaOpenedAndITStateState.ITFixed <= int.Parse(q[EnumAreaOpenedAndITState.State]))
                .Select(q => q)
                .ToArray()
                .Length &&
                0 < areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == 4 &&
                (int)EnumAreaOpenedAndITStateState.ITFixed <= int.Parse(q[EnumAreaOpenedAndITState.State]))
                .Select(q => q)
                .ToArray()
                .Length &&
                0 < areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == 5 &&
                (int)EnumAreaOpenedAndITStateState.Select <= int.Parse(q[EnumAreaOpenedAndITState.State]))
                .Select(q => q)
                .ToArray()
                .Length &&
                0 < mission.Where(q => q[EnumMission.MissionID].Equals($"{EnumMissionID.MI0006}") &&
                    q[EnumMission.Unlock].Equals(ConstGeneric.DIGITFORM_TRUE))
                .Select(q => q)
                .ToArray()
                .Length &&
                0 < history.Where(q => q.Equals($"{EnumMissionID.MI0006}"))
                .Select(q => q)
                .ToArray()
                .Length)
            {
                // ヘッドIT済み
                // ボディIT済み
                // ライトアームIT済み
                // レフトアームIT済み
                // コア解放済み
                // ミッション「MI0006」が達成済みかつ、履歴にも存在する
                return EnumRobotPanel.Full;
            }
            else if (0 < areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == 1 &&
                (int)EnumAreaOpenedAndITStateState.ITFixed <= int.Parse(q[EnumAreaOpenedAndITState.State]))
                .Select(q => q)
                .ToArray()
                .Length &&
                0 < areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == 2 &&
                (int)EnumAreaOpenedAndITStateState.Cleared <= int.Parse(q[EnumAreaOpenedAndITState.State]))
                .Select(q => q)
                .ToArray()
                .Length &&
                0 < areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == 3 &&
                (int)EnumAreaOpenedAndITStateState.ITFixed <= int.Parse(q[EnumAreaOpenedAndITState.State]))
                .Select(q => q)
                .ToArray()
                .Length &&
                0 < areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == 4 &&
                (int)EnumAreaOpenedAndITStateState.ITFixed <= int.Parse(q[EnumAreaOpenedAndITState.State]))
                .Select(q => q)
                .ToArray()
                .Length)
            {
                // ヘッドIT済み
                // ボディクリア済み
                // ライトアームIT済み
                // レフトアームIT済み
                return EnumRobotPanel.ConnectedDoublearm;
            }
            else if (0 < areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == 1 &&
                (int)EnumAreaOpenedAndITStateState.ITFixed <= int.Parse(q[EnumAreaOpenedAndITState.State]))
                .Select(q => q)
                .ToArray()
                .Length &&
                0 < areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == 2 &&
                (int)EnumAreaOpenedAndITStateState.Cleared <= int.Parse(q[EnumAreaOpenedAndITState.State]))
                .Select(q => q)
                .ToArray()
                .Length &&
                0 < areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == 3 &&
                (int)EnumAreaOpenedAndITStateState.Select <= int.Parse(q[EnumAreaOpenedAndITState.State]))
                .Select(q => q)
                .ToArray()
                .Length &&
                0 < areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == 4 &&
                (int)EnumAreaOpenedAndITStateState.ITFixed <= int.Parse(q[EnumAreaOpenedAndITState.State]))
                .Select(q => q)
                .ToArray()
                .Length)
            {
                // ヘッドIT済み
                // ボディクリア済み
                // ライトアーム解放
                // レフトアームIT済み
                return EnumRobotPanel.ConnectedLeftarm;
            }
            else if (0 < areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == 1 &&
                (int)EnumAreaOpenedAndITStateState.ITFixed <= int.Parse(q[EnumAreaOpenedAndITState.State]))
                .Select(q => q)
                .ToArray()
                .Length &&
                0 < areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == 2 &&
                (int)EnumAreaOpenedAndITStateState.Cleared <= int.Parse(q[EnumAreaOpenedAndITState.State]))
                .Select(q => q)
                .ToArray()
                .Length &&
                0 < areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == 3 &&
                (int)EnumAreaOpenedAndITStateState.ITFixed <= int.Parse(q[EnumAreaOpenedAndITState.State]))
                .Select(q => q)
                .ToArray()
                .Length &&
                0 < areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == 4 &&
                (int)EnumAreaOpenedAndITStateState.Select <= int.Parse(q[EnumAreaOpenedAndITState.State]))
                .Select(q => q)
                .ToArray()
                .Length)
            {
                // ヘッドIT済み
                // ボディクリア済み
                // ライトアームIT済み
                // レフトアーム解放
                return EnumRobotPanel.ConnectedRightarm;
            }
            else if (0 < areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == 1 &&
                (int)EnumAreaOpenedAndITStateState.ITFixed <= int.Parse(q[EnumAreaOpenedAndITState.State]))
                .Select(q => q)
                .ToArray()
                .Length &&
                0 < areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == 2 &&
                (int)EnumAreaOpenedAndITStateState.Cleared <= int.Parse(q[EnumAreaOpenedAndITState.State]))
                .Select(q => q)
                .ToArray()
                .Length)
            {
                // ヘッドIT済み
                // ボディ解放
                return EnumRobotPanel.ConnectedHead;
            }
            else if (0 < areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == 1 &&
                (int)EnumAreaOpenedAndITStateState.Cleared <= int.Parse(q[EnumAreaOpenedAndITState.State]))
                .Select(q => q)
                .ToArray()
                .Length &&
                0 < areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == 2 &&
                (int)EnumAreaOpenedAndITStateState.Cleared <= int.Parse(q[EnumAreaOpenedAndITState.State]))
                .Select(q => q)
                .ToArray()
                .Length)
            {
                // ヘッドIT失敗
                // ボディ解放
                return EnumRobotPanel.ConnectedFailureHead;
            }

            return EnumRobotPanel.FallingApart;
        }

        public EnumRobotPanel GetStateOfRobotUnit()
        {
            var areaOpenedAndITState = LoadSaveDatasCSVAndGetAreaOpenedAndITState();
            if (0 < areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == 1 &&
                (int)EnumAreaOpenedAndITStateState.ITFixed <= int.Parse(q[EnumAreaOpenedAndITState.State]))
                .Select(q => q)
                .ToArray()
                .Length &&
                0 < areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == 2 &&
                (int)EnumAreaOpenedAndITStateState.ITFixed <= int.Parse(q[EnumAreaOpenedAndITState.State]))
                .Select(q => q)
                .ToArray()
                .Length &&
                0 < areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == 3 &&
                (int)EnumAreaOpenedAndITStateState.ITFixed <= int.Parse(q[EnumAreaOpenedAndITState.State]))
                .Select(q => q)
                .ToArray()
                .Length &&
                0 < areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == 4 &&
                (int)EnumAreaOpenedAndITStateState.ITFixed <= int.Parse(q[EnumAreaOpenedAndITState.State]))
                .Select(q => q)
                .ToArray()
                .Length &&
                0 < areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == 5 &&
                (int)EnumAreaOpenedAndITStateState.Select <= int.Parse(q[EnumAreaOpenedAndITState.State]))
                .Select(q => q)
                .ToArray()
                .Length)
            {
                // ヘッドIT済み
                // ボディIT済み
                // ライトアームIT済み
                // レフトアームIT済み
                // コア解放済み
                return EnumRobotPanel.Full;
            }
            else if (0 < areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == 1 &&
                (int)EnumAreaOpenedAndITStateState.ITFixed <= int.Parse(q[EnumAreaOpenedAndITState.State]))
                .Select(q => q)
                .ToArray()
                .Length &&
                0 < areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == 2 &&
                (int)EnumAreaOpenedAndITStateState.Cleared <= int.Parse(q[EnumAreaOpenedAndITState.State]))
                .Select(q => q)
                .ToArray()
                .Length &&
                0 < areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == 3 &&
                (int)EnumAreaOpenedAndITStateState.Select <= int.Parse(q[EnumAreaOpenedAndITState.State]))
                .Select(q => q)
                .ToArray()
                .Length &&
                0 < areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == 4 &&
                (int)EnumAreaOpenedAndITStateState.Select <= int.Parse(q[EnumAreaOpenedAndITState.State]))
                .Select(q => q)
                .ToArray()
                .Length)
            {
                // ヘッドIT済み
                // ボディクリア済み
                // ライトアーム解放
                // レフトアーム解放
                return EnumRobotPanel.ConnectedRightarm;
            }
            else if (0 < areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == 1 &&
                (int)EnumAreaOpenedAndITStateState.ITFixed <= int.Parse(q[EnumAreaOpenedAndITState.State]))
                .Select(q => q)
                .ToArray()
                .Length &&
                0 < areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == 2 &&
                (int)EnumAreaOpenedAndITStateState.Select <= int.Parse(q[EnumAreaOpenedAndITState.State]))
                .Select(q => q)
                .ToArray()
                .Length)
            {
                // ヘッドIT済み
                // ボディ解放
                return EnumRobotPanel.ConnectedHead;
            }
            else if (0 < areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == 1 &&
                (int)EnumAreaOpenedAndITStateState.Cleared <= int.Parse(q[EnumAreaOpenedAndITState.State]))
                .Select(q => q)
                .ToArray()
                .Length &&
                0 < areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == 2 &&
                (int)EnumAreaOpenedAndITStateState.Select <= int.Parse(q[EnumAreaOpenedAndITState.State]))
                .Select(q => q)
                .ToArray()
                .Length)
            {
                // ヘッドクリア済み
                // ボディ解放
                return EnumRobotPanel.OnStartBody;
            }

            return EnumRobotPanel.FallingApart;
        }

        public Dictionary<EnumAreaOpenedAndITState, string>[] LoadSaveDatasCSVAndGetAreaOpenedAndITState()
        {
            var tResourcesAccessory = new AreaTemplateResourcesAccessory();
            var quasiAssignFormResources = tResourcesAccessory.LoadSaveDatasCSV(ConstResorcesNames.AREA_OPENED_AND_IT_STATE);
            return tResourcesAccessory.GetAreaOpenedAndITState(quasiAssignFormResources);
        }

        public Dictionary<EnumAreaUnits, int>[] LoadSaveDatasCSVAndGetAreaUnits()
        {
            var tResourcesAccessory = new AreaTemplateResourcesAccessory();
            var quasiAssignFormResources = tResourcesAccessory.LoadSaveDatasCSV(ConstResorcesNames.AREA_UNITS);
            return tResourcesAccessory.GetAreaUnits(quasiAssignFormResources);
        }

        public Dictionary<EnumMainSceneStagesState, int>[] LoadSaveDatasCSVAndGetMainSceneStagesState()
        {
            var tResourcesAccessory = new AreaTemplateResourcesAccessory();
            var quasiAssignFormResources = tResourcesAccessory.LoadSaveDatasCSV(ConstResorcesNames.MAIN_SCENE_STAGES_STATE);
            return tResourcesAccessory.GetMainSceneStagesState(quasiAssignFormResources);
        }

        public Dictionary<EnumQuasiAssignmentForm, string>[] LoadSaveDatasCSVAndGetQuasiAssignmentForm()
        {
            var tResourcesAccessory = new AreaTemplateResourcesAccessory();
            var quasiAssignFormResources = tResourcesAccessory.LoadSaveDatasCSV(ConstResorcesNames.QUASI_ASSIGNMENT_FORM);
            return tResourcesAccessory.GetQuasiAssignmentForm(quasiAssignFormResources);
        }

        //public bool SetCounterBetweenAndFillAmountAllGage(SeastarGageView[] seastarGageViews)
        //{
        //    try
        //    {
        //        foreach (var item in seastarGageViews.Where(q => q != null))
        //        {
        //            if (!item.PlayCounterBetweenAnimation(SelectGameManager.Instance.GimmickOwner.GetAssinedCounter()))
        //                throw new System.Exception("カウンターをセット呼び出しの失敗");
        //            float seastarGageCountValue = (float)SelectGameManager.Instance.GimmickOwner.GetAssinedCounter();
        //            if (!item.PlayFillAmountAnimation(seastarGageCountValue))
        //                throw new System.Exception("フィルターゲージの値をセット呼び出しの失敗");
        //        }

        //        return true;
        //    }
        //    catch (System.Exception e)
        //    {
        //        Debug.LogError(e);
        //        return false;
        //    }
        //}

        public bool SetIsAssignedAllCaption(CaptionStageModel[] captionStageModels)
        {
            try
            {
                foreach (var item in LoadSaveDatasCSVAndGetMainSceneStagesState()
                    .Select((p, i) => new { Content = p, Index = i })
                    .Where(q => 0 < q.Index))
                {
                    // ステージ番号を取り出す
                    var sceneId = item.Index;
                    foreach (var itemChilds in captionStageModels.Where(q => q != null &&
                            q.Index == sceneId &&
                            q.SeastarModels != null &&
                            0 < q.SeastarModels.Length)
                        .Select(q => q.SeastarModels))
                    {
                        // ステージ番号ごとにSeastar情報の配列を取得
                        foreach (var itemChild in itemChilds)
                        {
                            // ステージ番号ごとのキャプション配列の中でアサイン情報を反映
                            itemChild.SetIsAssigned(LoadSaveDatasCSVAndGetQuasiAssignmentForm()
                                .Where(q => q[EnumQuasiAssignmentForm.MainSceneStagesModulesStateIndex].Equals($"{sceneId}") &&
                                    q[EnumQuasiAssignmentForm.SeastarID].Equals($"{itemChild.SeastarConfig.EnumSeastarID}"))
                                .Select(q => q)
                                .ToArray()[0][EnumQuasiAssignmentForm.Assigned].Equals(ConstGeneric.DIGITFORM_TRUE));
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

        public Mission[] GetMissions()
        {
            List<Mission> missions = new List<Mission>();
            // GetMissionHistoryLastから実績履歴の最後のデータを取り出す
            var historyLast = GetMissionHistories();
            if (historyLast.Length < 1)
            {
                // 空の場合は初期値を返す
                return missions.ToArray();
            }
            // 実績IDに紐づく「ロボットの結合状態」「エリアID」を構造体リストで返す
            foreach (var item in historyLast)
                missions.AddRange(AreaGameManager.Instance.MissionOwner.Missions.Where(q => $"{q.enumMissionID}".Equals(item))
                    .Select(q => q)
                    .ToArray());

            return missions.ToArray();
        }

        private string[] GetMissionHistories()
        {
            var temp = new AreaTemplateResourcesAccessory();
            // 実績履歴を取得
            var history = temp.GetMissionHistory(temp.LoadSaveDatasCSV(ConstResorcesNames.MISSION_HISTORY));
            return 0 < history.Length ? history.Select(q => q[EnumMissionHistory.History]).ToArray() : new string[0];
        }

        public bool IsConnectedAnimation()
        {
            try
            {
                var temp = new AreaTemplateResourcesAccessory();
                // 実績一覧管理を取得
                var mission = temp.GetMission(temp.LoadSaveDatasCSV(ConstResorcesNames.MISSION));
                // 実績履歴を取得
                var history = temp.GetMissionHistory(temp.LoadSaveDatasCSV(ConstResorcesNames.MISSION_HISTORY));
                // 実績一覧管理のアンロック解除している実績IDが履歴に見つからない場合は演出を実行する
                foreach (var item in mission.Where(q => q[EnumMission.CategoryID].Equals($"{EnumCategoryID.CA0000}") &&
                    q[EnumMission.Unlock].Equals(ConstGeneric.DIGITFORM_TRUE))
                    .Select(q => q))
                    if (history.Where(q => q[EnumMissionHistory.History].Equals($"{item[EnumMission.MissionID]}"))
                        .Select(q => q)
                        .ToArray().Length < 1)
                        return true;

                return false;
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        public EnumUnitID[] GetPlayRenderEnables()
        {
            List<EnumUnitID> enumUnitIDs = new List<EnumUnitID>();
            var temp = new AreaTemplateResourcesAccessory();
            // 実績一覧管理を取得
            var mission = temp.GetMission(temp.LoadSaveDatasCSV(ConstResorcesNames.MISSION));
            // 実績履歴を取得
            var history = temp.GetMissionHistory(temp.LoadSaveDatasCSV(ConstResorcesNames.MISSION_HISTORY));
            var missionOwner = AreaGameManager.Instance.MissionOwner;
            // 実績一覧管理のアンロック解除している実績IDが履歴に見つからない場合は演出を実行する
            foreach (var item in mission.Where(q => q[EnumMission.CategoryID].Equals($"{EnumCategoryID.CA0000}") &&
                q[EnumMission.Unlock].Equals(ConstGeneric.DIGITFORM_TRUE))
                .Select(q => q))
                if (history.Where(q => q[EnumMissionHistory.History].Equals($"{item[EnumMission.MissionID]}"))
                    .Select(q => q)
                    .ToArray().Length < 1)
                    enumUnitIDs.AddRange(missionOwner.Missions.Where(q => $"{q.enumMissionID}".Equals(item[EnumMission.MissionID]))
                        .Select(q => q.enumUnitID)
                        .ToArray());

            return enumUnitIDs.ToArray();
        }

        public int AddMissionHistory()
        {
            try
            {
                List<EnumMissionID> addHistoryList = new List<EnumMissionID>();
                var temp = new AreaTemplateResourcesAccessory();
                // 実績一覧管理を取得
                var mission = temp.GetMission(temp.LoadSaveDatasCSV(ConstResorcesNames.MISSION));
                // 実績履歴を取得
                var histories = temp.GetMissionHistory(temp.LoadSaveDatasCSV(ConstResorcesNames.MISSION_HISTORY));
                var missionOwner = AreaGameManager.Instance.MissionOwner;
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

        public bool SetSystemCommonCashAndDefaultStageIndex(EnumUnitID enumUnitID)
        {
            try
            {
                var temp = new AreaTemplateResourcesAccessory();
                var SystemCommonCash = temp.GetSystemCommonCash(temp.LoadSaveDatasCSV(ConstResorcesNames.SYSTEM_COMMON_CASH));
                var areaUnits = temp.GetAreaUnits(temp.LoadSaveDatasCSV(ConstResorcesNames.AREA_UNITS));
                var currentUnitID = (EnumUnitID)areaUnits.Where(q => q[EnumAreaUnits.StageID] == SystemCommonCash[EnumSystemCommonCash.SceneId])
                    .Distinct()
                    .Select(q => q[EnumAreaUnits.UnitID])
                    .ToArray()[0];
                if (currentUnitID.Equals(enumUnitID))
                    // 同じエリアへ移動する場合はステージ番号をリセットしない
                    return true;

                // ユニットIDからステージ番号初期値を取得
                var defaultStageIndex = areaUnits.Where(q => q[EnumAreaUnits.UnitID] == (int)enumUnitID)
                    .Select(q => q[EnumAreaUnits.StageID])
                    .OrderBy(q => q)
                    .ToArray()[0];
                // キャッシュをセット
                SystemCommonCash[EnumSystemCommonCash.SceneId] = defaultStageIndex;
                if (!temp.SaveDatasCSVOfSystemCommonCash(ConstResorcesNames.SYSTEM_COMMON_CASH, SystemCommonCash))
                    throw new System.Exception("システム設定キャッシュをCSVデータへ保存呼び出しの失敗");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool CheckMissionAndSaveDatasCSVOfMission()
        {
            try
            {
                var temp = new AreaTemplateResourcesAccessory();
                var areaOpenedAndITState = temp.GetAreaOpenedAndITState(temp.LoadSaveDatasCSV(ConstResorcesNames.AREA_OPENED_AND_IT_STATE));
                var mission = temp.GetMission(temp.LoadSaveDatasCSV(ConstResorcesNames.MISSION));
                var updateCount = 0;
                // T.B.D ミッションごとに条件が異なるため洗いだす
                if (CheckMissionUnitClearAndMissionUnlock(EnumUnitID.Head, EnumMissionID.MI0001, areaOpenedAndITState, mission))
                {
                    // MI0001
                    mission.Where(q => q[EnumMission.MissionID].Equals($"{EnumMissionID.MI0001}"))
                        .Select(q => q)
                        .ToArray()[0][EnumMission.Unlock] = ConstGeneric.DIGITFORM_TRUE;
                    updateCount++;
                }
                else if (CheckMissionUnitClearAndMissionUnlock(EnumUnitID.Body, EnumMissionID.MI0002, areaOpenedAndITState, mission))
                {
                    // MI0002
                    mission.Where(q => q[EnumMission.MissionID].Equals($"{EnumMissionID.MI0002}"))
                        .Select(q => q)
                        .ToArray()[0][EnumMission.Unlock] = ConstGeneric.DIGITFORM_TRUE;
                    updateCount++;
                }
                else if (CheckMissionUnitITAndMissionUnlock(EnumUnitID.Head, EnumMissionID.MI0003, areaOpenedAndITState, mission))
                {
                    // MI0003
                    mission.Where(q => q[EnumMission.MissionID].Equals($"{EnumMissionID.MI0003}"))
                        .Select(q => q)
                        .ToArray()[0][EnumMission.Unlock] = ConstGeneric.DIGITFORM_TRUE;
                    updateCount++;
                }
                else if (CheckMissionUnitITAndMissionUnlock(EnumUnitID.RightArm, EnumMissionID.MI0004, areaOpenedAndITState, mission))
                {
                    // MI0004
                    mission.Where(q => q[EnumMission.MissionID].Equals($"{EnumMissionID.MI0004}"))
                        .Select(q => q)
                        .ToArray()[0][EnumMission.Unlock] = ConstGeneric.DIGITFORM_TRUE;
                    updateCount++;
                }
                else if (CheckMissionUnitITAndMissionUnlock(EnumUnitID.LeftArm, EnumMissionID.MI0005, areaOpenedAndITState, mission))
                {
                    // MI0005
                    mission.Where(q => q[EnumMission.MissionID].Equals($"{EnumMissionID.MI0005}"))
                        .Select(q => q)
                        .ToArray()[0][EnumMission.Unlock] = ConstGeneric.DIGITFORM_TRUE;
                    updateCount++;
                }
                else if (CheckMissionUnitClearAndMissionUnlock(EnumUnitID.Core, EnumMissionID.MI0007, areaOpenedAndITState, mission)/*currentUnitID == (int)EnumUnitID.Core*/)
                {
                    // MI0007
                    mission.Where(q => q[EnumMission.MissionID].Equals($"{EnumMissionID.MI0007}"))
                        .Select(q => q)
                        .ToArray()[0][EnumMission.Unlock] = ConstGeneric.DIGITFORM_TRUE;
                    updateCount++;
                }
                if (0 < updateCount)
                    if (!temp.SaveDatasCSVOfMission(ConstResorcesNames.MISSION, mission))
                        throw new System.Exception("実績一覧管理データをCSVデータへ保存呼び出しの失敗");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// ミッションチェック
        /// ユニットのクリア状態かつ、ミッションのアンロック状態
        /// </summary>
        /// <param name="enumUnitID">エリアID</param>
        /// <param name="enumMissionID">ミッションID</param>
        /// <param name="areaOpenedAndITState">エリア解放・結合テスト済みデータ</param>
        /// <param name="mission">実績一覧管理データ</param>
        /// <returns>ミッションの更新対象か</returns>
        private bool CheckMissionUnitClearAndMissionUnlock(EnumUnitID enumUnitID, EnumMissionID enumMissionID, Dictionary<EnumAreaOpenedAndITState, string>[] areaOpenedAndITState, Dictionary<EnumMission, string>[] mission)
        {
            return 0 < areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == (int)enumUnitID &&
                    int.Parse(q[EnumAreaOpenedAndITState.State]) == (int)EnumAreaOpenedAndITStateState.Cleared)
                    .Select(q => q)
                    .ToArray().Length &&
                    0 < mission.Where(q => q[EnumMission.MissionID].Equals($"{enumMissionID}") &&
                    q[EnumMission.Unlock].Equals(ConstGeneric.DIGITFORM_FALSE))
                    .Select(q => q)
                    .ToArray().Length;
        }

        /// <summary>
        /// ミッションチェック
        /// ユニットのIT状態かつ、ミッションのアンロック状態
        /// </summary>
        /// <param name="enumUnitID">エリアID</param>
        /// <param name="enumMissionID">ミッションID</param>
        /// <param name="areaOpenedAndITState">エリア解放・結合テスト済みデータ</param>
        /// <param name="mission">実績一覧管理データ</param>
        /// <returns>ミッションの更新対象か</returns>
        private bool CheckMissionUnitITAndMissionUnlock(EnumUnitID enumUnitID, EnumMissionID enumMissionID, Dictionary<EnumAreaOpenedAndITState, string>[] areaOpenedAndITState, Dictionary<EnumMission, string>[] mission)
        {
            return 0 < areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == (int)enumUnitID &&
                    int.Parse(q[EnumAreaOpenedAndITState.State]) == (int)EnumAreaOpenedAndITStateState.ITFixed)
                    .Select(q => q)
                    .ToArray().Length &&
                    0 < mission.Where(q => q[EnumMission.MissionID].Equals($"{enumMissionID}") &&
                    q[EnumMission.Unlock].Equals(ConstGeneric.DIGITFORM_FALSE))
                    .Select(q => q)
                    .ToArray().Length;
        }

        public int GetUnitIDsButIgnoreVoidInCore(Dictionary<EnumAreaUnits, int>[] areaUnits, int stageIndex)
        {
            var unitID = areaUnits.Where(q => q[EnumAreaUnits.StageID] == stageIndex)
                .Select(q => q[EnumAreaUnits.UnitID])
                .ToArray()[0];
            return unitID == (int)EnumUnitID.VoidInCore ? (int)EnumUnitID.Core : unitID;
        }
    }

    /// <summary>
    /// プレゼンタ
    /// セレクトシーン
    /// インターフェース
    /// </summary>
    public interface IAreaPresenterCommon
    {
        /// <summary>
        /// 準委任帳票の取得
        /// </summary>
        /// <returns>格納オブジェクト配列</returns>
        public Dictionary<EnumQuasiAssignmentForm, string>[] LoadSaveDatasCSVAndGetQuasiAssignmentForm();
        /// <summary>
        /// ステージクリア済みデータをオブジェクトへ一時セット
        /// </summary>
        /// <returns>格納オブジェクト配列</returns>
        public Dictionary<EnumMainSceneStagesState, int>[] LoadSaveDatasCSVAndGetMainSceneStagesState();
        /// <summary>
        /// 全ステージキャプションのアサイン情報をセット
        /// </summary>
        /// <param name="captionStageModels">ステージキャプション配列</param>
        /// <returns>成功／失敗</returns>
        public bool SetIsAssignedAllCaption(CaptionStageModel[] captionStageModels);
        ///// <summary>
        ///// 全ヒトデゲージのカウンターとフィルターをセット
        ///// </summary>
        ///// <param name="seastarGageViews">ヒトデゲージのビュー配列</param>
        ///// <returns>成功／失敗</returns>
        //public bool SetCounterBetweenAndFillAmountAllGage(SeastarGageView[] seastarGageViews);
        /// <summary>
        /// エリアユニットファイルへ一時セット
        /// </summary>
        /// <returns>格納オブジェクト配列</returns>
        public Dictionary<EnumAreaUnits, int>[] LoadSaveDatasCSVAndGetAreaUnits();
        /// <summary>
        /// エリア解放・結合テスト済みデータをオブジェクトへ一時セット
        /// </summary>
        /// <returns>格納オブジェクト配列</returns>
        public Dictionary<EnumAreaOpenedAndITState, string>[] LoadSaveDatasCSVAndGetAreaOpenedAndITState();
        /// <summary>
        /// ステータス確認（結合・解放）
        /// </summary>
        /// <returns>ロボットの結合・解放状態</returns>
        public EnumRobotPanel GetStateOfRobotUnit();
        /// <summary>
        /// ステータス確認（結合のみ）
        /// </summary>
        /// <returns>ロボットの結合・解放状態</returns>
        public EnumRobotPanel GetStateOfRobotUnitConnect();
        /// <summary>
        /// 実績履歴を取得
        /// </summary>
        /// <returns>実績履歴配列</returns>
        public Mission[] GetMissions();
        /// <summary>
        /// 新エリア解放演出を実行するか
        /// </summary>
        /// <returns>演出あり／なし</returns>
        public bool IsConnectedAnimation();
        /// <summary>
        /// 解放演出の対象ユニットを取得
        /// </summary>
        /// <returns>エリアID配列</returns>
        public EnumUnitID[] GetPlayRenderEnables();
        /// <summary>
        /// 実績履歴を更新
        /// </summary>
        /// <returns>追加した件数</returns>
        public int AddMissionHistory();
        /// <summary>
        /// キャッシュをセット
        /// ユニットIDからステージ番号初期値を取得
        /// 同じエリアへ移動する場合はステージ番号をリセットしない
        /// </summary>
        /// <param name="enumUnitID">エリアID</param>
        /// <returns>成功／失敗</returns>
        public bool SetSystemCommonCashAndDefaultStageIndex(EnumUnitID enumUnitID);
        /// <summary>
        /// ミッションの更新チェック
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool CheckMissionAndSaveDatasCSVOfMission();
        /// <summary>
        /// ユニットIDを取得
        /// 但し、コア（深部）は除く
        /// </summary>
        /// <param name="areaUnits">エリアユニット配列</param>
        /// <param name="stageIndex">ステージ番号</param>
        /// <returns>ユニットID</returns>
        public int GetUnitIDsButIgnoreVoidInCore(Dictionary<EnumAreaUnits, int>[] areaUnits, int stageIndex);
    }
}
