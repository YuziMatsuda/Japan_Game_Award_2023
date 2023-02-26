using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Template;
using Main.Common;

namespace Main.Test
{
    /// <summary>
    /// テスト用
    /// メインシーンのリソース管理
    /// </summary>
    public class TestMainResourcesAccessory : MonoBehaviour
    {
        [SerializeField] private int[] inputConfigDatas;
        [SerializeField] private bool testMode;

        private void Start()
        {
            new MainTemplateResourcesAccessory();
        }

        public void OnClicked()
        {
            if (testMode)
                TestCase_1();
        }

        public void TestCase_1()
        {
            Debug.Log("---OnClicked---");
            var tSResources = new MainTemplateResourcesAccessory();
            Debug.Log("---LoadResourcesCSV---");
            var datas = tSResources.LoadSaveDatasCSV(ConstResorcesNames.MAIN_SCENE_STAGES_CONFIG);
            if (datas == null)
                throw new System.Exception("リソース読み込みの失敗");
            for (var i = 0; i < datas.Count; i++)
            {
                for (var j = 0; j < datas[i].Length; j++)
                {
                    Debug.Log(datas[i][j]);
                }
            }
            Debug.Log("---GetMainSceneStagesConfig---");
            var configMaps = tSResources.GetMainSceneStagesConfig(datas);
            for (var i = 0; i < configMaps.Length; i++)
            {
                foreach (var map in configMaps[i])
                {
                    Debug.Log($"Idx:[{i}]Key:[{map.Key}]_Val:[{map.Value}]");
                }
            }
        }
    }
}
