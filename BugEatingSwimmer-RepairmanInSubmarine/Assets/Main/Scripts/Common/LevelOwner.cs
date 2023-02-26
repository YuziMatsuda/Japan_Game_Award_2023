using Main.Template;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Main.Common
{
    /// <summary>
    /// レベルのオーナー
    /// </summary>
    public class LevelOwner : MonoBehaviour, IMainGameManager
    {
        /// <summary>レベルの親オブジェクト</summary>
        [SerializeField] private Transform level;
        /// <summary>各ステージのプレハブ</summary>
        [SerializeField] private GameObject[] levelPrefabs;
        /// <summary>レベルがインスタンス済みか</summary>
        private readonly BoolReactiveProperty _isInstanced = new BoolReactiveProperty();
        /// <summary>レベルがインスタンス済みか</summary>
        public IReactiveProperty<bool> IsInstanced => _isInstanced;

        private void Reset()
        {
            level = GameObject.Find("Level").transform;
        }

        public void OnStart()
        {
            var disableLevels = GameObject.FindGameObjectsWithTag(ConstTagNames.TAG_NAME_LEVEL);
            if (0 < disableLevels.Length)
            {
                Debug.LogWarning("完成版ではレベルのプレハブをヒエラルキーから削除して下さい");
                foreach (var level in disableLevels)
                {
                    Debug.LogWarning($"レベル:[{level.name}]を無効化しました");
                    level.SetActive(false);
                }
            }
            // シーンIDを取得してステージをLevelオブジェクトの子要素としてインスタンスする
            var tResourcesAccessory = new MainTemplateResourcesAccessory();
            // ステージIDの取得
            var sysComCashResources = tResourcesAccessory.LoadSaveDatasCSV(ConstResorcesNames.SYSTEM_COMMON_CASH);
            var sysComCash = tResourcesAccessory.GetSystemCommonCash(sysComCashResources);

            var g = Instantiate(levelPrefabs[sysComCash[EnumSystemCommonCash.SceneId]], Vector3.zero, Quaternion.identity, level);
            if (g != null)
                _isInstanced.Value = true;
        }
    }
}
