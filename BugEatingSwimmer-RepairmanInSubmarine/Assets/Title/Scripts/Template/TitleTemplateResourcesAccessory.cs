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
        /// 実績一覧管理データをオブジェクトへ一時セット
        /// </summary>
        /// <param name="datas">二次元配列の文字列データ</param>
        /// <returns>格納オブジェクト配列</returns>
        public Dictionary<EnumMission, string>[] GetMission(List<string[]> datas)
        {
            return new TitleResourcesAccessory().GetMission(datas);
        }

        /// <summary>
        /// 準委任帳票をオブジェクトへ一時セット
        /// </summary>
        /// <param name="datas">二次元配列の文字列データ</param>
        /// <returns>格納オブジェクト配列</returns>
        public Dictionary<EnumQuasiAssignmentForm, string>[] GetQuasiAssignmentForm(List<string[]> datas)
        {
            return new TitleResourcesAccessory().GetQuasiAssignmentForm(datas);
        }

        /// <summary>
        /// エリア解放・結合テスト済みデータをオブジェクトへ一時セット
        /// </summary>
        /// <param name="datas">二次元配列の文字列データ</param>
        /// <returns>格納オブジェクト配列</returns>
        public Dictionary<EnumAreaOpenedAndITState, string>[] GetAreaOpenedAndITState(List<string[]> datas)
        {
            return new TitleResourcesAccessory().GetAreaOpenedAndITState(datas);
        }

        /// <summary>
        /// エリアユニットファイルへ一時セット
        /// </summary>
        /// <param name="datas">二次元配列の文字列データ</param>
        /// <returns>格納オブジェクト配列</returns>
        public Dictionary<EnumAreaUnits, int>[] GetAreaUnits(List<string[]> datas)
        {
            return new TitleResourcesAccessory().GetAreaUnits(datas);
        }

        /// <summary>
        /// 実績履歴データをオブジェクトへ一時セット
        /// </summary>
        /// <param name="datas">二次元配列の文字列データ</param>
        /// <returns>格納オブジェクト配列</returns>
        public Dictionary<EnumMissionHistory, string>[] GetMissionHistory(List<string[]> datas)
        {
            return new TitleResourcesAccessory().GetMissionHistory(datas);
        }

        /// <summary>
        /// ステージクリア条件をオブジェクトへ一時セット
        /// </summary>
        /// <param name="datas">二次元配列の文字列データ</param>
        /// <returns>格納オブジェクト配列</returns>
        public Dictionary<EnumMainSceneStagesModulesState, string>[] GetMainSceneStagesModulesState(List<string[]> datas)
        {
            return new TitleResourcesAccessory().GetMainSceneStagesModulesState(datas);
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

        /// <summary>
        /// 準委任帳票をCSVデータへ保存
        /// </summary>
        /// <param name="resourcesLoadName">リソースCSVファイル名</param>
        /// <param name="configMaps">格納オブジェクト配列</param>
        /// <returns>成功／失敗</returns>
        public bool SaveDatasCSVOfQuasiAssignmentForm(string resourcesLoadName, Dictionary<EnumQuasiAssignmentForm, string>[] configMaps)
        {
            return new TitleResourcesAccessory().SaveDatasCSVOfQuasiAssignmentForm(resourcesLoadName, configMaps);
        }

        /// <summary>
        /// エリア解放・結合テスト済みデータをCSVデータへ保存
        /// </summary>
        /// <param name="resourcesLoadName">リソースCSVファイル名</param>
        /// <param name="configMaps">格納オブジェクト配列</param>
        /// <returns>成功／失敗</returns>
        public bool SaveDatasCSVOfAreaOpenedAndITState(string resourcesLoadName, Dictionary<EnumAreaOpenedAndITState, string>[] configMaps)
        {
            return new TitleResourcesAccessory().SaveDatasCSVOfAreaOpenedAndITState(resourcesLoadName, configMaps);
        }

        /// <summary>
        /// 実績一覧管理データをCSVデータへ保存
        /// </summary>
        /// <param name="resourcesLoadName">リソースCSVファイル名</param>
        /// <param name="configMaps">格納オブジェクト配列</param>
        /// <returns>成功／失敗</returns>
        public bool SaveDatasCSVOfMission(string resourcesLoadName, Dictionary<EnumMission, string>[] configMaps)
        {
            return new TitleResourcesAccessory().SaveDatasCSVOfMission(resourcesLoadName, configMaps);
        }

        /// <summary>
        /// 実績履歴データをCSVデータへ保存
        /// </summary>
        /// <param name="resourcesLoadName">リソースCSVファイル名</param>
        /// <param name="configMaps">格納オブジェクト配列</param>
        /// <returns>成功／失敗</returns>
        public bool SaveDatasCSVOfMissionHistory(string resourcesLoadName, Dictionary<EnumMissionHistory, string>[] configMaps)
        {
            return new TitleResourcesAccessory().SaveDatasCSVOfMissionHistory(resourcesLoadName, configMaps);
        }

        /// <summary>
        /// ステージクリア条件をCSVデータへ保存
        /// </summary>
        /// <param name="resourcesLoadName">リソースCSVファイル名</param>
        /// <param name="configMaps">格納オブジェクト配列</param>
        /// <returns>成功／失敗</returns>
        public bool SaveDatasCSVOfMainSceneStagesModulesState(string resourcesLoadName, Dictionary<EnumMainSceneStagesModulesState, string>[] configMaps)
        {
            return new TitleResourcesAccessory().SaveDatasCSVOfMainSceneStagesModulesState(resourcesLoadName, configMaps);
        }
    }
}
