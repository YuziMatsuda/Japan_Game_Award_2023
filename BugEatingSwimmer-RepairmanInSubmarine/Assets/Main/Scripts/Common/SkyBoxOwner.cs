using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Template;

namespace Main.Common
{
    /// <summary>
    /// SkyBoxのオーナー
    /// </summary>
    public class SkyBoxOwner : MonoBehaviour, IMainGameManager
    {
        /// <summary>ステージごとのSkybox</summary>
        [SerializeField] private Material[] skyboxs;

        public void OnStart()
        {
            var tResourcesAccessory = new MainTemplateResourcesAccessory();
            // ステージIDの取得
            var sysComCashResources = tResourcesAccessory.LoadSaveDatasCSV(ConstResorcesNames.SYSTEM_COMMON_CASH);
            var sysComCash = tResourcesAccessory.GetSystemCommonCash(sysComCashResources);
            // ステージ共通設定の取得
            var mainSceneStagesConfResources = tResourcesAccessory.LoadSaveDatasCSV(ConstResorcesNames.MAIN_SCENE_STAGES_CONFIG);
            var mainSceneStagesConfs = tResourcesAccessory.GetMainSceneStagesConfig(mainSceneStagesConfResources);

            // Skyboxの設定
            RenderSettings.skybox = skyboxs[mainSceneStagesConfs[sysComCash[EnumSystemCommonCash.SceneId]][EnumMainSceneStagesConfig.SkyBoxs]];
        }
    }
}
