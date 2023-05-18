using Select.Template;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Select.Model;
using System.Linq;
using Select.View;

namespace Select.Common
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
    /// プレゼンタ
    /// セレクトシーン
    /// </summary>
    public class SelectPresenterCommon : ISelectPresenterCommon
    {
        public bool CheckMissionAndSaveDatasCSVOfMission()
        {
            try
            {
                var currentUnitID = GetContentsCountInPage();
                var areaOpenedAndITState = LoadSaveDatasCSVAndGetAreaOpenedAndITState();
                var quasiAssignmentForm = LoadSaveDatasCSVAndGetQuasiAssignmentForm();
                var temp = new SelectTemplateResourcesAccessory();
                var mission = LoadSaveDatasCSVAndGetMission();
                var missionHistory = LoadSaveDatasCSVAndGetMissionHistory();
                var updateCount = 0;
                List<EnumMissionID> targetMissionIDs = new List<EnumMissionID>();
                targetMissionIDs.Add(EnumMissionID.MI0000); targetMissionIDs.Add(EnumMissionID.MI0001); targetMissionIDs.Add(EnumMissionID.MI0002);
                targetMissionIDs.Add(EnumMissionID.MI0003); targetMissionIDs.Add(EnumMissionID.MI0004); targetMissionIDs.Add(EnumMissionID.MI0005);
                // ボディのエリアIDが選択されている
                if (currentUnitID == (int)EnumUnitID.Body &&
                    // ヘッドがIT済み、ボディがクリア済み、ライトアームがIT済み、レフトアームがIT済み
                    0 < GetLengthUnitIDEqualsState(EnumUnitID.Head, EnumAreaOpenedAndITStateState.ITFixed, areaOpenedAndITState) &&
                    0 < GetLengthUnitIDEqualsState(EnumUnitID.Body, EnumAreaOpenedAndITStateState.Cleared, areaOpenedAndITState) &&
                    0 < GetLengthUnitIDEqualsState(EnumUnitID.RightArm, EnumAreaOpenedAndITStateState.ITFixed, areaOpenedAndITState) &&
                    0 < GetLengthUnitIDEqualsState(EnumUnitID.LeftArm, EnumAreaOpenedAndITStateState.ITFixed, areaOpenedAndITState) &&
                    // ヒトデが10個以上獲得済み
                    9 < quasiAssignmentForm.Where(q => q[EnumQuasiAssignmentForm.Assigned].Equals(ConstGeneric.DIGITFORM_TRUE))
                    .Select(q => q)
                    .ToArray()
                    .Length &&
                    // 実績MI0000～MI0005がアンロック状態かつ、実績履歴に当該IDが存在する
                    GetIsUnlockedAndFindHistory(targetMissionIDs.ToArray(), mission, missionHistory) &&
                    // 実績MI0006がロック状態
                    0 < mission.Where(q => q[EnumMission.MissionID].Equals($"{EnumMissionID.MI0006}") &&
                        q[EnumMission.Unlock].Equals(ConstGeneric.DIGITFORM_FALSE))
                    .Select(q => q)
                    .ToArray()
                    .Length)
                {
                    // MI0006
                    mission.Where(q => q[EnumMission.MissionID].Equals($"{EnumMissionID.MI0006}"))
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
        /// アンロック状態チェックして更新履歴にも存在するか
        /// </summary>
        /// <param name="enumMissionIDs">チェック対象エリアID配列</param>
        /// <param name="mission">実績一覧</param>
        /// <param name="missionHistory">実績履歴</param>
        /// <returns></returns>
        private bool GetIsUnlockedAndFindHistory(EnumMissionID[] enumMissionIDs, Dictionary<EnumMission, string>[] mission, Dictionary<EnumMissionHistory, string>[] missionHistory)
        {
            var isCompleted = true;
            foreach (var missionID in enumMissionIDs)
            {
                isCompleted = 0 < mission.Where(q => q[EnumMission.MissionID].Equals($"{missionID}") &&
                    q[EnumMission.Unlock].Equals(ConstGeneric.DIGITFORM_TRUE))
                .Select(q => q)
                .ToArray()
                .Length &&
                0 < missionHistory.Where(q => q[EnumMissionHistory.History].Equals($"{missionID}"))
                .Select(q => q)
                .ToArray()
                .Length;
                if (!isCompleted)
                    return false;
            }

            return isCompleted;
        }

        /// <summary>
        /// エリアIDのが引数の状態と一致している場合、その要素数を返す
        /// </summary>
        /// <param name="enumUnitID">エリアID</param>
        /// <param name="enumAreaOpenedAndITStateState">クリア済み／IT済み</param>
        /// <param name="areaOpenedAndITState">エリア解放・結合テスト済みデータ</param>
        /// <returns>該当要素数</returns>
        private int GetLengthUnitIDEqualsState(EnumUnitID enumUnitID, EnumAreaOpenedAndITStateState enumAreaOpenedAndITStateState, Dictionary<EnumAreaOpenedAndITState, string>[] areaOpenedAndITState)
        {
            return areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == (int)enumUnitID &&
                int.Parse(q[EnumAreaOpenedAndITState.State]) == (int)enumAreaOpenedAndITStateState)
                .Select(q => q)
                .ToArray()
                .Length;
        }

        public int GetContentsCountInPage()
        {
            try
            {
                var temp = new SelectTemplateResourcesAccessory();
                // 現在選択されているステージ番号を取得
                var systemCommonCash = SelectGameManager.Instance.SceneOwner.GetSystemCommonCash();
                var areaUnits = temp.GetAreaUnits(temp.LoadSaveDatasCSV(ConstResorcesNames.AREA_UNITS));
                // ステージIDを元にエリアIDを取得
                return areaUnits.Where(q => q[EnumAreaUnits.StageID] == systemCommonCash[EnumSystemCommonCash.SceneId])
                    .Select(q => q[EnumAreaUnits.UnitID])
                    .Distinct()
                    .ToArray()[0];
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return -1;
            }
        }

        public Dictionary<EnumAreaOpenedAndITState, string>[] LoadSaveDatasCSVAndGetAreaOpenedAndITState()
        {
            var tResourcesAccessory = new SelectTemplateResourcesAccessory();
            var quasiAssignFormResources = tResourcesAccessory.LoadSaveDatasCSV(ConstResorcesNames.AREA_OPENED_AND_IT_STATE);
            return tResourcesAccessory.GetAreaOpenedAndITState(quasiAssignFormResources);
        }

        public Dictionary<EnumAreaUnits, int>[] LoadSaveDatasCSVAndGetAreaUnits()
        {
            var tResourcesAccessory = new SelectTemplateResourcesAccessory();
            var quasiAssignFormResources = tResourcesAccessory.LoadSaveDatasCSV(ConstResorcesNames.AREA_UNITS);
            return tResourcesAccessory.GetAreaUnits(quasiAssignFormResources);
        }

        public Dictionary<EnumMainSceneStagesState, int>[] LoadSaveDatasCSVAndGetMainSceneStagesState()
        {
            var tResourcesAccessory = new SelectTemplateResourcesAccessory();
            var quasiAssignFormResources = tResourcesAccessory.LoadSaveDatasCSV(ConstResorcesNames.MAIN_SCENE_STAGES_STATE);
            return tResourcesAccessory.GetMainSceneStagesState(quasiAssignFormResources);
        }

        public Dictionary<EnumMission, string>[] LoadSaveDatasCSVAndGetMission()
        {
            var tResourcesAccessory = new SelectTemplateResourcesAccessory();
            var quasiAssignFormResources = tResourcesAccessory.LoadSaveDatasCSV(ConstResorcesNames.MISSION);
            return tResourcesAccessory.GetMission(quasiAssignFormResources);
        }

        public Dictionary<EnumMissionHistory, string>[] LoadSaveDatasCSVAndGetMissionHistory()
        {
            var tResourcesAccessory = new SelectTemplateResourcesAccessory();
            var quasiAssignFormResources = tResourcesAccessory.LoadSaveDatasCSV(ConstResorcesNames.MISSION_HISTORY);
            return tResourcesAccessory.GetMissionHistory(quasiAssignFormResources);
        }

        public Dictionary<EnumQuasiAssignmentForm, string>[] LoadSaveDatasCSVAndGetQuasiAssignmentForm()
        {
            var tResourcesAccessory = new SelectTemplateResourcesAccessory();
            var quasiAssignFormResources = tResourcesAccessory.LoadSaveDatasCSV(ConstResorcesNames.QUASI_ASSIGNMENT_FORM);
            return tResourcesAccessory.GetQuasiAssignmentForm(quasiAssignFormResources);
        }

        public bool SetCounterBetweenAndFillAmountAllGage(SeastarGageView[] seastarGageViews)
        {
            try
            {
                foreach (var item in seastarGageViews.Where(q => q != null))
                {
                    var counter = item.SeastarGageConfig.EnumUnitID.Equals(EnumUnitID.Head) ?
                        SelectGameManager.Instance.GimmickOwner.GetAssinedCounter((int)EnumUnitID.Head) :
                        SelectGameManager.Instance.GimmickOwner.GetAssinedCounter();
                    if (!item.PlayCounterBetweenAnimation(counter))
                        throw new System.Exception("カウンターをセット呼び出しの失敗");
                    float seastarGageCountValue = (float)counter;
                    if (!item.PlayFillAmountAnimation(seastarGageCountValue))
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
                            var ary = LoadSaveDatasCSVAndGetQuasiAssignmentForm()
                                    .Where(q => q[EnumQuasiAssignmentForm.MainSceneStagesModulesStateIndex].Equals($"{sceneId}") &&
                                        q[EnumQuasiAssignmentForm.SeastarID].Equals($"{itemChild.SeastarConfig.EnumSeastarID}"))
                                    .Select(q => q)
                                    .ToArray();
                            if (0 < ary.Length)
                                // ステージ番号ごとのキャプション配列の中でアサイン情報を反映
                                itemChild.SetIsAssigned(ary[0][EnumQuasiAssignmentForm.Assigned].Equals(ConstGeneric.DIGITFORM_TRUE));
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

        public bool IsConnectedAnimation()
        {
            try
            {
                var temp = new SelectTemplateResourcesAccessory();
                // 実績一覧管理を取得
                var mission = temp.GetMission(temp.LoadSaveDatasCSV(ConstResorcesNames.MISSION));
                // 実績履歴を取得
                var history = temp.GetMissionHistory(temp.LoadSaveDatasCSV(ConstResorcesNames.MISSION_HISTORY));

                return IsFindMission(mission, history, false);
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 実績一覧と実績履歴に存在するか
        /// </summary>
        /// <param name="mission">実績一覧</param>
        /// <param name="history">実績履歴</param>
        /// <param name="findMode">実績一覧ありかつ、実績履歴にある／実績一覧ありかつ、実績履歴にない</param>
        /// <returns>条件一致する／条件一致しない</returns>
        private bool IsFindMission(Dictionary<EnumMission, string>[] mission, Dictionary<EnumMissionHistory, string>[] history, bool findMode)
        {
            if (!findMode)
            {
                // 実績一覧管理のアンロック解除している実績IDが履歴に見つからない場合は演出を実行する
                foreach (var item in mission.Where(q => q[EnumMission.CategoryID].Equals($"{EnumCategoryID.CA0000}") &&
                    q[EnumMission.Unlock].Equals(ConstGeneric.DIGITFORM_TRUE))
                    .Select(q => q))
                    if (history.Where(q => q[EnumMissionHistory.History].Equals($"{item[EnumMission.MissionID]}"))
                        .Select(q => q)
                        .ToArray().Length < 1)
                        return true;
            }
            else
            {
                // 実績一覧管理のアンロック解除している実績IDが履歴に見つからない場合は演出を実行する
                foreach (var item in mission.Where(q => q[EnumMission.CategoryID].Equals($"{EnumCategoryID.CA0000}") &&
                    q[EnumMission.Unlock].Equals(ConstGeneric.DIGITFORM_TRUE))
                    .Select(q => q))
                    if (0 < history.Where(q => q[EnumMissionHistory.History].Equals($"{item[EnumMission.MissionID]}"))
                        .Select(q => q)
                        .ToArray().Length)
                        return true;
            }
            return false;
        }

        public int AddMissionHistory()
        {
            try
            {
                List<EnumMissionID> addHistoryList = new List<EnumMissionID>();
                var temp = new SelectTemplateResourcesAccessory();
                // 実績一覧管理を取得
                var mission = temp.GetMission(temp.LoadSaveDatasCSV(ConstResorcesNames.MISSION));
                // 実績履歴を取得
                var histories = temp.GetMissionHistory(temp.LoadSaveDatasCSV(ConstResorcesNames.MISSION_HISTORY));
                var missionOwner = SelectGameManager.Instance.MissionOwner;
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

        public bool IsMissionUnlockAndFoundHistory(EnumMissionID enumMissionID)
        {
            try
            {
                var temp = new SelectTemplateResourcesAccessory();
                // 実績一覧管理を取得
                var mission = temp.GetMission(temp.LoadSaveDatasCSV(ConstResorcesNames.MISSION));
                // 実績履歴を取得
                var history = temp.GetMissionHistory(temp.LoadSaveDatasCSV(ConstResorcesNames.MISSION_HISTORY));

                // 実績一覧管理のアンロック解除している実績IDが履歴に見つからない場合は演出を実行する
                foreach (var item in mission.Where(q => q[EnumMission.CategoryID].Equals($"{EnumCategoryID.CA0000}") &&
                    q[EnumMission.Unlock].Equals(ConstGeneric.DIGITFORM_TRUE) &&
                    q[EnumMission.MissionID].Equals($"{enumMissionID/*EnumMissionID.MI0006*/}"))
                    .Select(q => q))
                    if (0 < history.Where(q => q[EnumMissionHistory.History].Equals($"{item[EnumMission.MissionID]}") &&
                        q[EnumMissionHistory.History].Equals($"{enumMissionID/*EnumMissionID.MI0006*/}"))
                        .Select(q => q)
                        .ToArray().Length)
                        return true;

                return false;
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        public bool IsCoreOpened()
        {
            try
            {
                var temp = new SelectTemplateResourcesAccessory();
                // 実績一覧管理を取得
                var mission = temp.GetMission(temp.LoadSaveDatasCSV(ConstResorcesNames.MISSION));
                // 実績履歴を取得
                var history = temp.GetMissionHistory(temp.LoadSaveDatasCSV(ConstResorcesNames.MISSION_HISTORY));

                // 実績一覧管理のからコア解放済みかチェック
                return 0 < mission.Where(q => q[EnumMission.CategoryID].Equals($"{EnumCategoryID.CA0000}") &&
                    q[EnumMission.Unlock].Equals(ConstGeneric.DIGITFORM_TRUE) &&
                    q[EnumMission.MissionID].Equals($"{EnumMissionID.MI0006}"))
                    .Select(q => q)
                    .ToArray()
                    .Length;
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        public bool SetSystemCommonCashAndDefaultStageIndex(EnumUnitID enumUnitID)
        {
            try
            {
                var temp = new SelectTemplateResourcesAccessory();
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
    }

    /// <summary>
    /// プレゼンタ
    /// セレクトシーン
    /// インターフェース
    /// </summary>
    public interface ISelectPresenterCommon
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
        /// <summary>
        /// 全ヒトデゲージのカウンターとフィルターをセット
        /// </summary>
        /// <param name="seastarGageViews">ヒトデゲージのビュー配列</param>
        /// <returns>成功／失敗</returns>
        public bool SetCounterBetweenAndFillAmountAllGage(SeastarGageView[] seastarGageViews);
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
        /// ステージIDに基づいたページ番号を取得する
        /// </summary>
        /// <returns>ページ番号</returns>
        public int GetContentsCountInPage();
        /// <summary>
        /// ミッションの更新チェック
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool CheckMissionAndSaveDatasCSVOfMission();
        /// <summary>
        /// 実績一覧管理データをオブジェクトへ一時セット
        /// </summary>
        /// <returns>格納オブジェクト配列</returns>
        public Dictionary<EnumMission, string>[] LoadSaveDatasCSVAndGetMission();
        /// <summary>
        /// 実績履歴データをオブジェクトへ一時セット
        /// </summary>
        /// <returns>格納オブジェクト配列</returns>
        public Dictionary<EnumMissionHistory, string>[] LoadSaveDatasCSVAndGetMissionHistory();
        /// <summary>
        /// 新エリア解放演出を実行するか
        /// </summary>
        /// <returns>演出あり／なし</returns>
        public bool IsConnectedAnimation();
        /// <summary>
        /// 実績履歴を更新
        /// </summary>
        /// <returns>追加した件数</returns>
        public int AddMissionHistory();
        /// <summary>
        /// 実績一覧と実績履歴に対象IDが存在するか
        /// </summary>
        /// <param name="enumMissionID">ミッションID</param>
        /// <returns>実行済み／実行しない</returns>
        public bool IsMissionUnlockAndFoundHistory(EnumMissionID enumMissionID);
        /// <summary>
        /// コア解放済みか
        /// </summary>
        /// <returns>実行済み／実行しない</returns>
        public bool IsCoreOpened();
        /// <summary>
        /// キャッシュをセット
        /// ユニットIDからステージ番号初期値を取得
        /// 同じエリアへ移動する場合はステージ番号をリセットしない
        /// </summary>
        /// <param name="enumUnitID">エリアID</param>
        /// <returns>成功／失敗</returns>
        public bool SetSystemCommonCashAndDefaultStageIndex(EnumUnitID enumUnitID);
    }
}
