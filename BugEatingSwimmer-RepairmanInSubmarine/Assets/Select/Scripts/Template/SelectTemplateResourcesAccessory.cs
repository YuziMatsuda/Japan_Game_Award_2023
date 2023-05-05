using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Select.Accessory;
using Select.Common;

namespace Select.Template
{
    /// <summary>
    /// リソースアクセスのテンプレート
    /// セレクト用
    /// </summary>
    public class SelectTemplateResourcesAccessory
    {
        /// <summary>
        /// リソースアクセスのテンプレート
        /// セレクト用
        /// コンストラクタ
        /// </summary>
        public SelectTemplateResourcesAccessory()
        {
            new SelectResourcesAccessory().Initialize();
        }

        /// <summary>
        /// タイトルカラム名を含むCSVデータの取得
        /// </summary>
        /// <param name="resourcesLoadName">リソースCSVファイル名</param>
        /// <returns>二次元配列の文字列データ</returns>
        public List<string[]> LoadSaveDatasCSV(string resourcesLoadName)
        {
            return new SelectResourcesAccessory().LoadSaveDatasCSV(resourcesLoadName);
        }

        /// <summary>
        /// システムオプション設定をオブジェクトへ一時セット
        /// </summary>
        /// <param name="datas">二次元配列の文字列データ</param>
        /// <returns>格納オブジェクト</returns>
        public Dictionary<EnumSystemConfig, int> GetSystemConfig(List<string[]> datas)
        {
            return new SelectResourcesAccessory().GetSystemConfig(datas);
        }

        /// <summary>
        /// システム設定キャッシュをオブジェクトへ一時セット
        /// </summary>
        /// <param name="datas">二次元配列の文字列データ</param>
        /// <returns>格納オブジェクト</returns>
        public Dictionary<EnumSystemCommonCash, int> GetSystemCommonCash(List<string[]> datas)
        {
            return new SelectResourcesAccessory().GetSystemCommonCash(datas);
        }

        /// <summary>
        /// ステージクリア済みデータをオブジェクトへ一時セット
        /// </summary>
        /// <param name="datas">二次元配列の文字列データ</param>
        /// <returns>格納オブジェクト配列</returns>
        public Dictionary<EnumMainSceneStagesState, int>[] GetMainSceneStagesState(List<string[]> datas)
        {
            return new SelectResourcesAccessory().GetMainSceneStagesState(datas);
        }

        /// <summary>
        /// 準委任帳票をオブジェクトへ一時セット
        /// </summary>
        /// <param name="datas">二次元配列の文字列データ</param>
        /// <returns>格納オブジェクト配列</returns>
        public Dictionary<EnumQuasiAssignmentForm, string>[] GetQuasiAssignmentForm(List<string[]> datas)
        {
            return new SelectResourcesAccessory().GetQuasiAssignmentForm(datas);
        }

        /// <summary>
        /// エリア解放・結合テスト済みデータをオブジェクトへ一時セット
        /// </summary>
        /// <param name="datas">二次元配列の文字列データ</param>
        /// <returns>格納オブジェクト配列</returns>
        public Dictionary<EnumAreaOpenedAndITState, string>[] GetAreaOpenedAndITState(List<string[]> datas)
        {
            return new SelectResourcesAccessory().GetAreaOpenedAndITState(datas);
        }

        /// <summary>
        /// エリアユニットファイルへ一時セット
        /// </summary>
        /// <param name="datas">二次元配列の文字列データ</param>
        /// <returns>格納オブジェクト配列</returns>
        public Dictionary<EnumAreaUnits, int>[] GetAreaUnits(List<string[]> datas)
        {
            return new SelectResourcesAccessory().GetAreaUnits(datas);
        }

        /// <summary>
        /// 実績一覧管理データをオブジェクトへ一時セット
        /// </summary>
        /// <param name="datas">二次元配列の文字列データ</param>
        /// <returns>格納オブジェクト配列</returns>
        public Dictionary<EnumMission, string>[] GetMission(List<string[]> datas)
        {
            return new SelectResourcesAccessory().GetMission(datas);
        }

        /// <summary>
        /// 実績履歴データをオブジェクトへ一時セット
        /// </summary>
        /// <param name="datas">二次元配列の文字列データ</param>
        /// <returns>格納オブジェクト配列</returns>
        public Dictionary<EnumMissionHistory, string>[] GetMissionHistory(List<string[]> datas)
        {
            return new SelectResourcesAccessory().GetMissionHistory(datas);
        }

        /// <summary>
        /// システム設定キャッシュをCSVデータへ保存
        /// </summary>
        /// <param name="resourcesLoadName">リソースCSVファイル名</param>
        /// <param name="configMap">格納オブジェクト</param>
        /// <returns>成功／失敗</returns>
        public bool SaveDatasCSVOfSystemCommonCash(string resourcesLoadName, Dictionary<EnumSystemCommonCash, int> configMap)
        {
            return new SelectResourcesAccessory().SaveDatasCSVOfSystemCommonCash(resourcesLoadName, configMap);
        }

        /// <summary>
        /// 準委任帳票をCSVデータへ保存
        /// </summary>
        /// <param name="resourcesLoadName">リソースCSVファイル名</param>
        /// <param name="configMaps">格納オブジェクト配列</param>
        /// <returns>成功／失敗</returns>
        public bool SaveDatasCSVOfQuasiAssignmentForm(string resourcesLoadName, Dictionary<EnumQuasiAssignmentForm, string>[] configMaps)
        {
            return new SelectResourcesAccessory().SaveDatasCSVOfQuasiAssignmentForm(resourcesLoadName, configMaps);
        }

        /// <summary>
        /// エリア解放・結合テスト済みデータをCSVデータへ保存
        /// </summary>
        /// <param name="resourcesLoadName">リソースCSVファイル名</param>
        /// <param name="configMaps">格納オブジェクト配列</param>
        /// <returns>成功／失敗</returns>
        public bool SaveDatasCSVOfAreaOpenedAndITState(string resourcesLoadName, Dictionary<EnumAreaOpenedAndITState, string>[] configMaps)
        {
            return new SelectResourcesAccessory().SaveDatasCSVOfAreaOpenedAndITState(resourcesLoadName, configMaps);
        }

        /// <summary>
        /// 実績一覧管理データをCSVデータへ保存
        /// </summary>
        /// <param name="resourcesLoadName">リソースCSVファイル名</param>
        /// <param name="configMaps">格納オブジェクト配列</param>
        /// <returns>成功／失敗</returns>
        public bool SaveDatasCSVOfMission(string resourcesLoadName, Dictionary<EnumMission, string>[] configMaps)
        {
            return new SelectResourcesAccessory().SaveDatasCSVOfMission(resourcesLoadName, configMaps);
        }

        /// <summary>
        /// 実績履歴データをCSVデータへ保存
        /// </summary>
        /// <param name="resourcesLoadName">リソースCSVファイル名</param>
        /// <param name="configMaps">格納オブジェクト配列</param>
        /// <returns>成功／失敗</returns>
        public bool SaveDatasCSVOfMissionHistory(string resourcesLoadName, Dictionary<EnumMissionHistory, string>[] configMaps)
        {
            return new SelectResourcesAccessory().SaveDatasCSVOfMissionHistory(resourcesLoadName, configMaps);
        }
    }
}
