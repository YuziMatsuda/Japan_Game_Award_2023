using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Title.Template;
using Title.Common;

namespace Title.Test
{
    /// <summary>
    /// テスト用
    /// タイトルシーンのリソース管理
    /// </summary>
    public class TestTitleResourcesAccessory : MonoBehaviour
    {
        [SerializeField] private int[] inputSystemConfig;
        [SerializeField] private int[] inputMainSceneStagesCleared;
        [SerializeField] private int[] inputSystemCommonCash;
        [SerializeField] private int testMode;

        private void Start()
        {
            new TitleTemplateResourcesAccessory();
        }

        public void OnClicked()
        {
            switch (testMode)
            {
                case 0:
                    TestCase_0();
                    break;
                case 1:
                    TestCase_1();
                    break;
                case 2:
                    TestCase_2();
                    break;
                case 3:
                    TestCase_3();
                    break;
                default:
                    Debug.LogError("例外ケース");
                    break;
            }
        }

        private void TestCase_0()
        {
            Debug.Log("---OnClicked---");
            var tTResources = new TitleTemplateResourcesAccessory();
            Debug.Log("---LoadResourcesCSV---");
            var datas = tTResources.LoadSaveDatasCSV(ConstResorcesNames.SYSTEM_CONFIG);
            if (datas == null)
                throw new System.Exception("リソース読み込みの失敗");
            for (var i = 0; i < datas.Count; i++)
            {
                for (var j = 0; j < datas[i].Length; j++)
                {
                    Debug.Log(datas[i][j]);
                }
            }
            Debug.Log("---GetSystemConfig---");
            var configMap = tTResources.GetSystemConfig(datas);
            foreach (var map in configMap)
            {
                Debug.Log($"Key:{map.Key}_Val:{map.Value}");
            }
            Debug.Log("---SaveResourcesCSVOfSystemConfig---");
            //var configMap = new Dictionary<EnumSystemConfig, int>();
            var idx = 0;
            configMap[EnumSystemConfig.AudioVolumeIndex] = inputSystemConfig[idx++];
            configMap[EnumSystemConfig.BGMVolumeIndex] = inputSystemConfig[idx++];
            configMap[EnumSystemConfig.SEVolumeIndex] = inputSystemConfig[idx++];
            configMap[EnumSystemConfig.VibrationEnableIndex] = inputSystemConfig[idx++];
            if (!tTResources.SaveDatasCSVOfSystemConfig(ConstResorcesNames.SYSTEM_CONFIG, configMap))
                Debug.LogError("CSV保存呼び出しの失敗");
        }

        private void TestCase_1()
        {
            Debug.Log("---OnClicked---");
            var tTResources = new TitleTemplateResourcesAccessory();
            Debug.Log("---LoadSaveDatasCSV---");
            var datas = tTResources.LoadSaveDatasCSV(ConstResorcesNames.MAIN_SCENE_STAGES_STATE);
            if (datas == null)
                throw new System.Exception("リソース読み込みの失敗");
            for (var i = 0; i < datas.Count; i++)
            {
                for (var j = 0; j < datas[i].Length; j++)
                {
                    Debug.Log(datas[i][j]);
                }
            }
            Debug.Log("---GetMainSceneStagesCleared---");
            var configMaps = tTResources.GetMainSceneStagesState(datas);
            foreach (var configMap in configMaps)
            {
                foreach (var map in configMap)
                {
                    Debug.Log($"Key:{map.Key}_Val:{map.Value}");
                }
            }
            Debug.Log("---SaveDatasCSVOfMainSceneStagesCleared---");
            configMaps[1][EnumMainSceneStagesState.State] = inputMainSceneStagesCleared[1];
            configMaps[2][EnumMainSceneStagesState.State] = inputMainSceneStagesCleared[2];
            if (!tTResources.SaveDatasCSVOfMainSceneStagesState(ConstResorcesNames.MAIN_SCENE_STAGES_STATE, configMaps))
                Debug.LogError("CSV保存呼び出しの失敗");
        }

        private void TestCase_2()
        {
            Debug.Log("---OnClicked---");
            var tTResources = new TitleTemplateResourcesAccessory();
            Debug.Log("---LoadResourcesCSV---");
            var datas = tTResources.LoadResourcesCSV(ConstResorcesNames.MAIN_SCENE_STAGES_STATE);
            //if (datas == null)
            //    throw new System.Exception("リソース読み込みの失敗");
            //for (var i = 0; i < datas.Count; i++)
            //{
            //    for (var j = 0; j < datas[i].Length; j++)
            //    {
            //        Debug.Log(datas[i][j]);
            //    }
            //}
            Debug.Log("---GetMainSceneStagesCleared---");
            var configMaps = tTResources.GetMainSceneStagesState(datas);
            //foreach (var configMap in configMaps)
            //{
            //    foreach (var map in configMap)
            //    {
            //        Debug.Log($"Key:{map.Key}_Val:{map.Value}");
            //    }
            //}
            Debug.Log("---SaveDatasCSVOfMainSceneStagesCleared---");
            if (!tTResources.SaveDatasCSVOfMainSceneStagesState(ConstResorcesNames.MAIN_SCENE_STAGES_STATE, configMaps))
                Debug.LogError("CSV保存呼び出しの失敗");
        }

        public void TestCase_3()
        {
            Debug.Log("---OnClicked---");
            var tSResources = new TitleTemplateResourcesAccessory();
            Debug.Log("---LoadResourcesCSV---");
            var datas = tSResources.LoadSaveDatasCSV(ConstResorcesNames.SYSTEM_COMMON_CASH);
            if (datas == null)
                throw new System.Exception("リソース読み込みの失敗");
            for (var i = 0; i < datas.Count; i++)
            {
                for (var j = 0; j < datas[i].Length; j++)
                {
                    Debug.Log(datas[i][j]);
                }
            }
            Debug.Log("---GetSystemCommonCash---");
            var configMap = tSResources.GetSystemCommonCash(datas);
            foreach (var map in configMap)
            {
                Debug.Log($"Key:{map.Key}_Val:{map.Value}");
            }
            Debug.Log("---SaveResourcesCSVOfSystemCommonCash---");
            //var configMap = new Dictionary<EnumSystemConfig, int>();
            var idx = 0;
            configMap[EnumSystemCommonCash.SceneId] = inputSystemCommonCash[idx++];
            if (!tSResources.SaveDatasCSVOfSystemCommonCash(ConstResorcesNames.SYSTEM_COMMON_CASH, configMap))
                Debug.LogError("CSV保存呼び出しの失敗");
        }
    }
}
