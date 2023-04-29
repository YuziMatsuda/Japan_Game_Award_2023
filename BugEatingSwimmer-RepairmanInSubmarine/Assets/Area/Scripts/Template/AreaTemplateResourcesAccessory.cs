using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Area.Accessory;
using Area.Common;

namespace Area.Template
{
    /// <summary>
    /// リソースアクセスのテンプレート
    /// エリア用
    /// </summary>
    public class AreaTemplateResourcesAccessory
    {
        /// <summary>
        /// リソースアクセスのテンプレート
        /// エリア用
        /// コンストラクタ
        /// </summary>
        public AreaTemplateResourcesAccessory()
        {
            new AreaResourcesAccessory().Initialize();
        }

        /// <summary>
        /// タイトルカラム名を含むCSVデータの取得
        /// </summary>
        /// <param name="resourcesLoadName">リソースCSVファイル名</param>
        /// <returns>二次元配列の文字列データ</returns>
        public List<string[]> LoadSaveDatasCSV(string resourcesLoadName)
        {
            return new AreaResourcesAccessory().LoadSaveDatasCSV(resourcesLoadName);
        }

        /// <summary>
        /// システムオプション設定をオブジェクトへ一時セット
        /// </summary>
        /// <param name="datas">二次元配列の文字列データ</param>
        /// <returns>格納オブジェクト</returns>
        public Dictionary<EnumSystemConfig, int> GetSystemConfig(List<string[]> datas)
        {
            return new AreaResourcesAccessory().GetSystemConfig(datas);
        }

        /// <summary>
        /// システム設定キャッシュをオブジェクトへ一時セット
        /// </summary>
        /// <param name="datas">二次元配列の文字列データ</param>
        /// <returns>格納オブジェクト</returns>
        public Dictionary<EnumSystemCommonCash, int> GetSystemCommonCash(List<string[]> datas)
        {
            return new AreaResourcesAccessory().GetSystemCommonCash(datas);
        }

        /// <summary>
        /// ステージクリア済みデータをオブジェクトへ一時セット
        /// </summary>
        /// <param name="datas">二次元配列の文字列データ</param>
        /// <returns>格納オブジェクト配列</returns>
        public Dictionary<EnumMainSceneStagesState, int>[] GetMainSceneStagesState(List<string[]> datas)
        {
            return new AreaResourcesAccessory().GetMainSceneStagesState(datas);
        }

        /// <summary>
        /// 準委任帳票をオブジェクトへ一時セット
        /// </summary>
        /// <param name="datas">二次元配列の文字列データ</param>
        /// <returns>格納オブジェクト配列</returns>
        public Dictionary<EnumQuasiAssignmentForm, string>[] GetQuasiAssignmentForm(List<string[]> datas)
        {
            return new AreaResourcesAccessory().GetQuasiAssignmentForm(datas);
        }

        /// <summary>
        /// エリア解放・結合テスト済みデータをオブジェクトへ一時セット
        /// </summary>
        /// <param name="datas">二次元配列の文字列データ</param>
        /// <returns>格納オブジェクト配列</returns>
        public Dictionary<EnumAreaOpenedAndITState, string>[] GetAreaOpenedAndITState(List<string[]> datas)
        {
            return new AreaResourcesAccessory().GetAreaOpenedAndITState(datas);
        }

        /// <summary>
        /// エリアユニットファイルへ一時セット
        /// </summary>
        /// <param name="datas">二次元配列の文字列データ</param>
        /// <returns>格納オブジェクト配列</returns>
        public Dictionary<EnumAreaUnits, int>[] GetAreaUnits(List<string[]> datas)
        {
            return new AreaResourcesAccessory().GetAreaUnits(datas);
        }

        /// <summary>
        /// エリア解放・結合テストの演出ヒストリーデータをオブジェクトへ一時セット
        /// </summary>
        /// <param name="datas">二次元配列の文字列データ</param>
        /// <returns>格納オブジェクト配列</returns>
        public Dictionary<EnumAreaOpenedAndITStateHistory, int>[] GetAreaOpenedAndITStateHistory(List<string[]> datas)
        {
            return new AreaResourcesAccessory().GetAreaOpenedAndITStateHistory(datas);
        }

        /// <summary>
        /// システム設定キャッシュをCSVデータへ保存
        /// </summary>
        /// <param name="resourcesLoadName">リソースCSVファイル名</param>
        /// <param name="configMap">格納オブジェクト</param>
        /// <returns>成功／失敗</returns>
        public bool SaveDatasCSVOfSystemCommonCash(string resourcesLoadName, Dictionary<EnumSystemCommonCash, int> configMap)
        {
            return new AreaResourcesAccessory().SaveDatasCSVOfSystemCommonCash(resourcesLoadName, configMap);
        }

        /// <summary>
        /// 準委任帳票をCSVデータへ保存
        /// </summary>
        /// <param name="resourcesLoadName">リソースCSVファイル名</param>
        /// <param name="configMaps">格納オブジェクト配列</param>
        /// <returns>成功／失敗</returns>
        public bool SaveDatasCSVOfQuasiAssignmentForm(string resourcesLoadName, Dictionary<EnumQuasiAssignmentForm, string>[] configMaps)
        {
            return new AreaResourcesAccessory().SaveDatasCSVOfQuasiAssignmentForm(resourcesLoadName, configMaps);
        }

        /// <summary>
        /// エリア解放・結合テスト済みデータをCSVデータへ保存
        /// </summary>
        /// <param name="resourcesLoadName">リソースCSVファイル名</param>
        /// <param name="configMaps">格納オブジェクト配列</param>
        /// <returns>成功／失敗</returns>
        public bool SaveDatasCSVOfAreaOpenedAndITState(string resourcesLoadName, Dictionary<EnumAreaOpenedAndITState, string>[] configMaps)
        {
            return new AreaResourcesAccessory().SaveDatasCSVOfAreaOpenedAndITState(resourcesLoadName, configMaps);
        }

        /// <summary>
        /// エリア解放・結合テストの演出ヒストリーデータをCSVデータへ保存
        /// </summary>
        /// <param name="resourcesLoadName">リソースCSVファイル名</param>
        /// <param name="configMaps">格納オブジェクト配列</param>
        /// <returns>成功／失敗</returns>
        public bool SaveDatasCSVOfAreaOpenedAndITStateHistory(string resourcesLoadName, Dictionary<EnumAreaOpenedAndITStateHistory, int>[] configMaps)
        {
            return new AreaResourcesAccessory().SaveDatasCSVOfAreaOpenedAndITStateHistory(resourcesLoadName, configMaps);
        }
    }
}
