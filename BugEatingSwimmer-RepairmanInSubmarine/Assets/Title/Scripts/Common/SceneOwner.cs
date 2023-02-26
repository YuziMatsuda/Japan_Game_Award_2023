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
        [SerializeField] private string nextSceneName = "SelectScene";

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
                var mainSceneStagesStateDatas = tTResources.LoadResourcesCSV(ConstResorcesNames.MAIN_SCENE_STAGES_STATE_ALL);
                var mainSceneStagesStateConfigMaps = tTResources.GetMainSceneStagesState(mainSceneStagesStateDatas);
                if (!tTResources.SaveDatasCSVOfMainSceneStagesState(ConstResorcesNames.MAIN_SCENE_STAGES_STATE, mainSceneStagesStateConfigMaps))
                    Debug.LogError("CSV保存呼び出しの失敗");
                // システム設定キャッシュのリセット
                var systemCommonCashDatas = tTResources.LoadResourcesCSV(ConstResorcesNames.SYSTEM_COMMON_CASH);
                var systemCommonCashConfigMaps = tTResources.GetSystemCommonCash(systemCommonCashDatas);
                if (!tTResources.SaveDatasCSVOfSystemCommonCash(ConstResorcesNames.SYSTEM_COMMON_CASH, systemCommonCashConfigMaps))
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
    }
}
