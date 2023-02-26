using Select.Template;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Select.Common
{
    /// <summary>
    /// シーンオーナー
    /// </summary>
    public class SceneOwner : MonoBehaviour, ISelectGameManager
    {
        /// <summary>次のシーン名</summary>
        [SerializeField] private string nextSceneName = "MainScene";
        /// <summary>前のシーン名</summary>
        [SerializeField] private string backSceneName = "TitleScene";

        public void OnStart()
        {
            new SelectTemplateResourcesAccessory();
        }

        /// <summary>
        /// シーンIDを取得
        /// </summary>
        /// <returns>シーンID</returns>
        public Dictionary<EnumSystemCommonCash, int> GetSystemCommonCash()
        {
            try
            {
                var tSResources = new SelectTemplateResourcesAccessory();
                var datas = tSResources.LoadSaveDatasCSV(ConstResorcesNames.SYSTEM_COMMON_CASH);
                if (datas == null)
                    throw new System.Exception("リソース読み込みの失敗");

                return tSResources.GetSystemCommonCash(datas);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }

        /// <summary>
        /// ステージクリア済みデータを取得
        /// </summary>
        /// <returns>ステージクリア済みデータ</returns>
        public Dictionary<EnumMainSceneStagesState, int>[] GetMainSceneStagesState()
        {
            try
            {
                var tSResources = new SelectTemplateResourcesAccessory();
                var datas = tSResources.LoadSaveDatasCSV(ConstResorcesNames.MAIN_SCENE_STAGES_STATE);
                if (datas == null)
                    throw new System.Exception("リソース読み込みの失敗");

                return tSResources.GetMainSceneStagesState(datas);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }

        /// <summary>
        /// シーンIDを更新
        /// </summary>
        /// <param name="configMap">シーン設定</param>
        /// <returns>成功／失敗</returns>
        public bool SetSystemCommonCash(Dictionary<EnumSystemCommonCash, int> configMap)
        {
            try
            {
                var tSResources = new SelectTemplateResourcesAccessory();
                if (!tSResources.SaveDatasCSVOfSystemCommonCash(ConstResorcesNames.SYSTEM_COMMON_CASH, configMap))
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
        /// タイトルシーンをロード
        /// </summary>
        public void LoadTitleScene()
        {
            SceneManager.LoadScene(backSceneName);
        }

        /// <summary>
        /// メインシーンをロード
        /// </summary>
        public void LoadMainScene()
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
