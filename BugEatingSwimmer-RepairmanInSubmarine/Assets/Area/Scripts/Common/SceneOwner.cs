using Area.Template;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Area.Common
{
    /// <summary>
    /// シーンオーナー
    /// </summary>
    public class SceneOwner : MonoBehaviour, IAreaGameManager, ISceneOwner
    {
        /// <summary>次のシーン名</summary>
        [SerializeField] private string nextSceneName = "SelectScene";
        /// <summary>前のシーン名</summary>
        [SerializeField] private string backSceneName = "TitleScene";
        /// <summary>現在のシーン名</summary>
        [SerializeField] private string currentSceneName = "AreaScene";

        public void OnStart()
        {
            new AreaTemplateResourcesAccessory();
        }

        /// <summary>
        /// シーンIDを取得
        /// </summary>
        /// <returns>シーンID</returns>
        public Dictionary<EnumSystemCommonCash, int> GetSystemCommonCash()
        {
            try
            {
                var tSResources = new AreaTemplateResourcesAccessory();
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

        public Dictionary<EnumMainSceneStagesState, int>[] GetMainSceneStagesState()
        {
            try
            {
                var tSResources = new AreaTemplateResourcesAccessory();
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

        public bool SetSystemCommonCash(Dictionary<EnumSystemCommonCash, int> configMap)
        {
            try
            {
                var tSResources = new AreaTemplateResourcesAccessory();
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
        public void LoadBackScene()
        {
            SceneManager.LoadScene(backSceneName);
        }

        /// <summary>
        /// メインシーンをロード
        /// </summary>
        public void LoadNextScene()
        {
            SceneManager.LoadScene(nextSceneName);
        }

        public Dictionary<EnumAreaOpenedAndITState, string>[] GetAreaOpenedAndITState()
        {
            try
            {
                var tSResources = new AreaTemplateResourcesAccessory();
                var datas = tSResources.LoadSaveDatasCSV(ConstResorcesNames.AREA_OPENED_AND_IT_STATE);
                if (datas == null)
                    throw new System.Exception("リソース読み込みの失敗");

                return tSResources.GetAreaOpenedAndITState(datas);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }

        public Dictionary<EnumAreaUnits, int>[] GetAreaUnits()
        {
            try
            {
                var tSResources = new AreaTemplateResourcesAccessory();
                var datas = tSResources.LoadSaveDatasCSV(ConstResorcesNames.AREA_UNITS);
                if (datas == null)
                    throw new System.Exception("リソース読み込みの失敗");

                return tSResources.GetAreaUnits(datas);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }

        public bool SetAreaOpenedAndITState(Dictionary<EnumAreaOpenedAndITState, string>[] configMaps)
        {
            try
            {
                var tSResources = new AreaTemplateResourcesAccessory();
                if (!tSResources.SaveDatasCSVOfAreaOpenedAndITState(ConstResorcesNames.AREA_OPENED_AND_IT_STATE, configMaps))
                    Debug.LogError("CSV保存呼び出しの失敗");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public void ReLoadScene()
        {
            SceneManager.LoadScene(currentSceneName);
        }
    }

    public interface ISceneOwner
    {
        /// <summary>
        /// ステージクリア済みデータを取得
        /// </summary>
        /// <returns>ステージクリア済みデータ</returns>
        public Dictionary<EnumMainSceneStagesState, int>[] GetMainSceneStagesState();
        /// <summary>
        /// エリア解放・結合テスト済みデータを取得
        /// </summary>
        /// <returns>エリア解放・結合テスト済みデータ</returns>
        public Dictionary<EnumAreaOpenedAndITState, string>[] GetAreaOpenedAndITState();
        /// <summary>
        /// エリアユニットファイルデータを取得
        /// </summary>
        /// <returns>エリアユニットファイルデータ</returns>
        public Dictionary<EnumAreaUnits, int>[] GetAreaUnits();
        /// <summary>
        /// シーンIDを更新
        /// </summary>
        /// <param name="configMap">シーン設定</param>
        /// <returns>成功／失敗</returns>
        public bool SetSystemCommonCash(Dictionary<EnumSystemCommonCash, int> configMap);
        /// <summary>
        /// エリア解放・結合テスト済みデータを更新
        /// </summary>
        /// <param name="configMap">格納オブジェクト配列</param>
        /// <returns>成功／失敗</returns>
        public bool SetAreaOpenedAndITState(Dictionary<EnumAreaOpenedAndITState, string>[] configMaps);
        /// <summary>
        /// タイトルシーンをロード
        /// </summary>
        public void LoadBackScene();
        /// <summary>
        /// メインシーンをロード
        /// </summary>
        public void LoadNextScene();
        /// <summary>
        /// シーンをリロード
        /// </summary>
        public void ReLoadScene();
    }
}
