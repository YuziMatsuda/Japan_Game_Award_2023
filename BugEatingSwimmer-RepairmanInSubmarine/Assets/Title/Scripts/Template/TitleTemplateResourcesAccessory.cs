using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Title.Accessory;
using Title.Common;

namespace Title.Template
{
    /// <summary>
    /// リソースアクセスのテンプレート
    /// タイトル用
    /// </summary>
    public class TitleTemplateResourcesAccessory
    {
        /// <summary>
        /// リソースアクセスのテンプレート
        /// タイトル用
        /// コンストラクタ
        /// </summary>
        public TitleTemplateResourcesAccessory()
        {
            new TitleResourcesAccessory().Initialize();
        }

        /// <summary>
        /// タイトルカラム名を含むCSVデータの取得
        /// </summary>
        /// <param name="resourcesLoadName">リソースCSVファイル名</param>
        /// <returns>二次元配列の文字列データ</returns>
        public List<string[]> LoadSaveDatasCSV(string resourcesLoadName)
        {
            return new TitleResourcesAccessory().LoadSaveDatasCSV(resourcesLoadName);
        }

        /// <summary>
        /// タイトルカラム名を含むCSVリソースの取得
        /// </summary>
        /// <param name="resourcesLoadName">リソースCSVファイル名</param>
        /// <returns>二次元配列の文字列データ</returns>
        public List<string[]> LoadResourcesCSV(string resourcesLoadName)
        {
            return new TitleResourcesAccessory().LoadResourcesCSV(resourcesLoadName);
        }

        /// <summary>
        /// システムオプション設定をオブジェクトへ一時セット
        /// </summary>
        /// <param name="datas">二次元配列の文字列データ</param>
        /// <returns>格納オブジェクト</returns>
        public Dictionary<EnumSystemCommonCash, int> GetSystemCommonCash(List<string[]> datas)
        {
            return new TitleResourcesAccessory().GetSystemCommonCash(datas);
        }

        /// <summary>
        /// システムオプション設定をオブジェクトへ一時セット
        /// </summary>
        /// <param name="datas">二次元配列の文字列データ</param>
        /// <returns>格納オブジェクト</returns>
        public Dictionary<EnumSystemConfig, int> GetSystemConfig(List<string[]> datas)
        {
            return new TitleResourcesAccessory().GetSystemConfig(datas);
        }

        /// <summary>
        /// ステージクリア済みデータをオブジェクトへ一時セット
        /// </summary>
        /// <param name="datas">二次元配列の文字列データ</param>
        /// <returns>格納オブジェクト配列</returns>
        public Dictionary<EnumMainSceneStagesState, int>[] GetMainSceneStagesState(List<string[]> datas)
        {
            return new TitleResourcesAccessory().GetMainSceneStagesState(datas);
        }

        /// <summary>
        /// システム設定キャッシュをCSVデータへ保存
        /// </summary>
        /// <param name="resourcesLoadName">リソースCSVファイル名</param>
        /// <param name="configMap">格納オブジェクト</param>
        /// <returns>成功／失敗</returns>
        public bool SaveDatasCSVOfSystemCommonCash(string resourcesLoadName, Dictionary<EnumSystemCommonCash, int> configMap)
        {
            return new TitleResourcesAccessory().SaveDatasCSVOfSystemCommonCash(resourcesLoadName, configMap);
        }

        /// <summary>
        /// システムオプション設定をCSVデータへ保存
        /// </summary>
        /// <param name="resourcesLoadName">リソースCSVファイル名</param>
        /// <param name="configMap">格納オブジェクト</param>
        /// <returns>成功／失敗</returns>
        public bool SaveDatasCSVOfSystemConfig(string resourcesLoadName, Dictionary<EnumSystemConfig, int> configMap)
        {
            return new TitleResourcesAccessory().SaveDatasCSVOfSystemConfig(resourcesLoadName, configMap);
        }

        /// <summary>
        /// ステージクリア済みデータをCSVデータへ保存
        /// </summary>
        /// <param name="resourcesLoadName">リソースCSVファイル名</param>
        /// <param name="configMaps">格納オブジェクト配列</param>
        /// <returns>成功／失敗</returns>
        public bool SaveDatasCSVOfMainSceneStagesState(string resourcesLoadName, Dictionary<EnumMainSceneStagesState, int>[] configMaps)
        {
            return new TitleResourcesAccessory().SaveDatasCSVOfMainSceneStagesState(resourcesLoadName, configMaps);
        }
    }
}
