using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Accessory;
using Main.Common;

namespace Main.Template
{
    /// <summary>
    /// リソースアクセスのテンプレート
    /// メイン用
    /// </summary>
    public class MainTemplateResourcesAccessory
    {
        /// <summary>
        /// リソースアクセスのテンプレート
        /// メイン用
        /// コンストラクタ
        /// </summary>
        public MainTemplateResourcesAccessory()
        {
            new MainResourcesAccessory().Initialize();
        }

        /// <summary>
        /// タイトルカラム名を含むCSVデータの取得
        /// </summary>
        /// <param name="resourcesLoadName">リソースCSVファイル名</param>
        /// <returns>二次元配列の文字列データ</returns>
        public List<string[]> LoadSaveDatasCSV(string resourcesLoadName)
        {
            return new MainResourcesAccessory().LoadSaveDatasCSV(resourcesLoadName);
        }

        /// <summary>
        /// システムオプション設定をオブジェクトへ一時セット
        /// </summary>
        /// <param name="datas">二次元配列の文字列データ</param>
        /// <returns>格納オブジェクト</returns>
        public Dictionary<EnumSystemConfig, int> GetSystemConfig(List<string[]> datas)
        {
            return new MainResourcesAccessory().GetSystemConfig(datas);
        }

        /// <summary>
        /// システムオプション設定をオブジェクトへ一時セット
        /// </summary>
        /// <param name="datas">二次元配列の文字列データ</param>
        /// <returns>格納オブジェクト</returns>
        public Dictionary<EnumSystemCommonCash, int> GetSystemCommonCash(List<string[]> datas)
        {
            return new MainResourcesAccessory().GetSystemCommonCash(datas);
        }

        /// <summary>
        /// ステージ設定をオブジェクトへ一時セット
        /// </summary>
        /// <param name="datas">二次元配列の文字列データ</param>
        /// <returns>格納オブジェクト</returns>
        public Dictionary<EnumMainSceneStagesConfig, int>[] GetMainSceneStagesConfig(List<string[]> datas)
        {
            return new MainResourcesAccessory().GetMainSceneStagesConfig(datas);
        }

        /// <summary>
        /// ステージクリア済みデータをオブジェクトへ一時セット
        /// </summary>
        /// <param name="datas">二次元配列の文字列データ</param>
        /// <returns>格納オブジェクト配列</returns>
        public Dictionary<EnumMainSceneStagesState, int>[] GetMainSceneStagesState(List<string[]> datas)
        {
            return new MainResourcesAccessory().GetMainSceneStagesState(datas);
        }

        /// <summary>
        /// システムオプション設定をCSVデータへ保存
        /// </summary>
        /// <param name="resourcesLoadName">リソースCSVファイル名</param>
        /// <param name="configMap">格納オブジェクト</param>
        /// <returns>成功／失敗</returns>
        public bool SaveDatasCSVOfSystemCommonCash(string resourcesLoadName, Dictionary<EnumSystemCommonCash, int> configMap)
        {
            return new MainResourcesAccessory().SaveDatasCSVOfSystemCommonCash(resourcesLoadName, configMap);
        }

        /// <summary>
        /// ステージクリア済みデータをCSVデータへ保存
        /// </summary>
        /// <param name="resourcesLoadName">リソースCSVファイル名</param>
        /// <param name="configMaps">格納オブジェクト配列</param>
        /// <returns>成功／失敗</returns>
        public bool SaveDatasCSVOfMainSceneStagesState(string resourcesLoadName, Dictionary<EnumMainSceneStagesState, int>[] configMaps)
        {
            return new MainResourcesAccessory().SaveDatasCSVOfMainSceneStagesState(resourcesLoadName, configMaps);
        }
    }
}
