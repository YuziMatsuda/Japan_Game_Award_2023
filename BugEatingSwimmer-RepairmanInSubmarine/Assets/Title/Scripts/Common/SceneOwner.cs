using System.Collections;
using System.Collections.Generic;
using Title.Template;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Title.Common
{
    /// <summary>
    /// シーンオーナー
    /// </summary>
    public class SceneOwner : MonoBehaviour, ITitleGameManager
    {
        /// <summary>次のシーン名</summary>
        [SerializeField] private string nextSceneName = "AreaScene";
        /// <summary>次のシーン名（チュートリアルの場合）</summary>
        [SerializeField] private string nextTutorialSceneName = "MainScene";

        public void OnStart()
        {
            new TitleTemplateResourcesAccessory();
        }

        /// <summary>
        /// ステージクリア済みデータの削除
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool DestroyMainSceneStagesState()
        {
            try
            {
                var tTResources = new TitleTemplateResourcesAccessory();
                // ステージクリア済みデータのリセット
                var mainSceneStagesStateDatas = tTResources.LoadResourcesCSV(ConstResorcesNames.MAIN_SCENE_STAGES_STATE);
                var mainSceneStagesStateConfigMaps = tTResources.GetMainSceneStagesState(mainSceneStagesStateDatas);
                if (!tTResources.SaveDatasCSVOfMainSceneStagesState(ConstResorcesNames.MAIN_SCENE_STAGES_STATE, mainSceneStagesStateConfigMaps))
                    Debug.LogError("CSV保存呼び出しの失敗");
                // システム設定キャッシュのリセット
                var systemCommonCashDatas = tTResources.LoadResourcesCSV(ConstResorcesNames.SYSTEM_COMMON_CASH);
                var systemCommonCashConfigMaps = tTResources.GetSystemCommonCash(systemCommonCashDatas);
                if (!tTResources.SaveDatasCSVOfSystemCommonCash(ConstResorcesNames.SYSTEM_COMMON_CASH, systemCommonCashConfigMaps))
                    Debug.LogError("CSV保存呼び出しの失敗");
                // 実績一覧管理
                var missions = tTResources.LoadResourcesCSV(ConstResorcesNames.MISSION);
                var missionConfigMaps = tTResources.GetMission(missions);
                if (!tTResources.SaveDatasCSVOfMission(ConstResorcesNames.MISSION, missionConfigMaps))
                    Debug.LogError("CSV保存呼び出しの失敗");
                // 実績履歴
                var histories = tTResources.LoadResourcesCSV(ConstResorcesNames.MISSION_HISTORY);
                var histroyMaps = tTResources.GetMissionHistory(histories);
                if (!tTResources.SaveDatasCSVOfMissionHistory(ConstResorcesNames.MISSION_HISTORY, histroyMaps))
                    Debug.LogError("CSV保存呼び出しの失敗");
                // 準委任帳票
                var quasiAssignmentForm = tTResources.LoadResourcesCSV(ConstResorcesNames.QUASI_ASSIGNMENT_FORM);
                var quasiAssignmentFormMaps = tTResources.GetQuasiAssignmentForm(quasiAssignmentForm);
                if (!tTResources.SaveDatasCSVOfQuasiAssignmentForm(ConstResorcesNames.QUASI_ASSIGNMENT_FORM, quasiAssignmentFormMaps))
                    Debug.LogError("CSV保存呼び出しの失敗");
                // エリア解放・結合テスト
                var areaOpenedAndITStates = tTResources.LoadResourcesCSV(ConstResorcesNames.AREA_OPENED_AND_IT_STATE);
                var areaOpenedAndITStateMaps = tTResources.GetAreaOpenedAndITState(areaOpenedAndITStates);
                if (!tTResources.SaveDatasCSVOfAreaOpenedAndITState(ConstResorcesNames.AREA_OPENED_AND_IT_STATE, areaOpenedAndITStateMaps))
                    Debug.LogError("CSV保存呼び出しの失敗");
                // ステージクリア条件
                var mainSceneStagesModulesStates = tTResources.LoadResourcesCSV(ConstResorcesNames.MAIN_SCENE_STAGES_MODULES_STATE);
                var mainSceneStagesModulesStateMaps = tTResources.GetMainSceneStagesModulesState(mainSceneStagesModulesStates);
                if (!tTResources.SaveDatasCSVOfMainSceneStagesModulesState(ConstResorcesNames.MAIN_SCENE_STAGES_MODULES_STATE, mainSceneStagesModulesStateMaps))
                    Debug.LogError("CSV保存呼び出しの失敗");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// 全ステージの選択を有効にする
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool AllReleasedMainSceneStagesState()
        {
            try
            {
                var tTResources = new TitleTemplateResourcesAccessory();
                // ステージクリア済みデータのリセット
                var mainSceneStagesStateDatas = tTResources.LoadResourcesCSV(ConstResorcesNames.MAIN_SCENE_STAGES_STATE + ConstResorcesNames.ALL);
                var mainSceneStagesStateConfigMaps = tTResources.GetMainSceneStagesState(mainSceneStagesStateDatas);
                if (!tTResources.SaveDatasCSVOfMainSceneStagesState(ConstResorcesNames.MAIN_SCENE_STAGES_STATE, mainSceneStagesStateConfigMaps))
                    Debug.LogError("CSV保存呼び出しの失敗");
                // システム設定キャッシュのリセット
                var systemCommonCashDatas = tTResources.LoadResourcesCSV(ConstResorcesNames.SYSTEM_COMMON_CASH + ConstResorcesNames.ALL);
                var systemCommonCashConfigMaps = tTResources.GetSystemCommonCash(systemCommonCashDatas);
                if (!tTResources.SaveDatasCSVOfSystemCommonCash(ConstResorcesNames.SYSTEM_COMMON_CASH, systemCommonCashConfigMaps))
                    Debug.LogError("CSV保存呼び出しの失敗");
                // 実績一覧管理
                var missions = tTResources.LoadResourcesCSV(ConstResorcesNames.MISSION + ConstResorcesNames.ALL);
                var missionConfigMaps = tTResources.GetMission(missions);
                if (!tTResources.SaveDatasCSVOfMission(ConstResorcesNames.MISSION, missionConfigMaps))
                    Debug.LogError("CSV保存呼び出しの失敗");
                // 実績履歴
                var histories = tTResources.LoadResourcesCSV(ConstResorcesNames.MISSION_HISTORY + ConstResorcesNames.ALL);
                var histroyMaps = tTResources.GetMissionHistory(histories);
                if (!tTResources.SaveDatasCSVOfMissionHistory(ConstResorcesNames.MISSION_HISTORY, histroyMaps))
                    Debug.LogError("CSV保存呼び出しの失敗");
                // 準委任帳票
                var quasiAssignmentForm = tTResources.LoadResourcesCSV(ConstResorcesNames.QUASI_ASSIGNMENT_FORM + ConstResorcesNames.ALL);
                var quasiAssignmentFormMaps = tTResources.GetQuasiAssignmentForm(quasiAssignmentForm);
                if (!tTResources.SaveDatasCSVOfQuasiAssignmentForm(ConstResorcesNames.QUASI_ASSIGNMENT_FORM, quasiAssignmentFormMaps))
                    Debug.LogError("CSV保存呼び出しの失敗");
                // エリア解放・結合テスト
                var areaOpenedAndITStates = tTResources.LoadResourcesCSV(ConstResorcesNames.AREA_OPENED_AND_IT_STATE + ConstResorcesNames.ALL);
                var areaOpenedAndITStateMaps = tTResources.GetAreaOpenedAndITState(areaOpenedAndITStates);
                if (!tTResources.SaveDatasCSVOfAreaOpenedAndITState(ConstResorcesNames.AREA_OPENED_AND_IT_STATE, areaOpenedAndITStateMaps))
                    Debug.LogError("CSV保存呼び出しの失敗");
                // ステージクリア条件
                var mainSceneStagesModulesStates = tTResources.LoadResourcesCSV(ConstResorcesNames.MAIN_SCENE_STAGES_MODULES_STATE + ConstResorcesNames.ALL);
                var mainSceneStagesModulesStateMaps = tTResources.GetMainSceneStagesModulesState(mainSceneStagesModulesStates);
                if (!tTResources.SaveDatasCSVOfMainSceneStagesModulesState(ConstResorcesNames.MAIN_SCENE_STAGES_MODULES_STATE, mainSceneStagesModulesStateMaps))
                    Debug.LogError("CSV保存呼び出しの失敗");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// シーン読み込み
        /// </summary>
        public void LoadNextScene()
        {
            SceneManager.LoadScene(nextSceneName);
        }

        /// <summary>
        /// シーン読み込み（チュートリアル）
        /// </summary>
        public void LoadNextTutorialScene()
        {
            SceneManager.LoadScene(nextTutorialSceneName);
        }
    }
}
