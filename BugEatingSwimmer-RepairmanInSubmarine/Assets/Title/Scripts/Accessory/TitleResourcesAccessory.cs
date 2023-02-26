using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using Title.Common;
using System.Linq;

namespace Title.Accessory
{
    /// <summary>
    /// リソースアクセス
    /// タイトル用
    /// </summary>
    public class TitleResourcesAccessory
    {
        /// <summary>
        /// 初期処理
        /// </summary>
        public void Initialize()
        {
            // リソース管理ディレクトリが存在しない場合は作成
            if (!Directory.Exists(GetHomePath()))
            {
                Directory.CreateDirectory(GetHomePath());
            }
            // システム設定キャッシュが存在しない場合は作成
            if (!File.Exists($"{GetHomePath()}{ConstResorcesNames.SYSTEM_COMMON_CASH}.csv"))
            {
                using (File.Create($"{GetHomePath()}{ConstResorcesNames.SYSTEM_COMMON_CASH}.csv")) { }
            }
            // システム設定ファイルが存在しない場合は作成
            if (!File.Exists($"{GetHomePath()}{ConstResorcesNames.SYSTEM_CONFIG}.csv"))
            {
                using (File.Create($"{GetHomePath()}{ConstResorcesNames.SYSTEM_CONFIG}.csv")) { }
            }
            // ステージクリア済みデータが存在しない場合は作成
            if (!File.Exists($"{GetHomePath()}{ConstResorcesNames.MAIN_SCENE_STAGES_STATE}.csv"))
            {
                using (File.Create($"{GetHomePath()}{ConstResorcesNames.MAIN_SCENE_STAGES_STATE}.csv")) { }
            }
        }

        /// <summary>
        /// ホームディレクトリを取得
        /// </summary>
        /// <returns>ホームディレクトリ</returns>
        private string GetHomePath()
        {
            var path = "";
#if UNITY_EDITOR
            path = ConstResorcesNames.HOMEPATH_UNITYEDITOR;
#elif UNITY_STANDALONE
                path = ConstResorcesNames.HOMEPATH_BUILD;
#endif
            return path;
        }

        /// <summary>
        /// タイトルカラム名を含むCSVデータの取得
        /// </summary>
        /// <param name="resourcesLoadName">リソースCSVファイル名</param>
        /// <returns>二次元配列の文字列データ</returns>
        public List<string[]> LoadSaveDatasCSV(string resourcesLoadName)
        {
            try
            {
                var csvDatas = new List<string[]>();
                var path = GetHomePath();
                // 設定内容を保存
                using (var sr = new StreamReader($"{path}{resourcesLoadName}.csv", Encoding.GetEncoding("UTF-8")))
                {
                    while (sr.Peek() != -1)
                    {
                        var l = sr.ReadLine();
                        csvDatas.Add(l.Split(','));
                    }
                }

                if (csvDatas.Count < 1)
                {
                    Debug.LogWarning("不正データのため初期値を取得");
                    return LoadResourcesCSVDefault(resourcesLoadName);
                }

                return csvDatas;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }

        /// <summary>
        /// タイトルカラム名を含むCSVリソースの取得
        /// </summary>
        /// <param name="resourcesLoadName">リソースCSVファイル名</param>
        /// <returns>二次元配列の文字列データ</returns>
        public List<string[]> LoadResourcesCSV(string resourcesLoadName)
        {
            return LoadResourcesCSVDefault(resourcesLoadName);
        }

        /// <summary>
        /// ※デフォルト用※
        /// 必ず「設定ファイル名＋Default」という設定ファイルを作成しておく
        /// タイトルカラム名を含むCSVデータの取得
        /// </summary>
        /// <param name="resourcesLoadName">リソースCSVファイル名</param>
        /// <returns>二次元配列の文字列データ</returns>
        private List<string[]> LoadResourcesCSVDefault(string resourcesLoadName)
        {
            try
            {
                var csvDatas = new List<string[]>();
                var ta = Resources.Load($"{resourcesLoadName}Default") as TextAsset;
                var sr = new StringReader(ta.text);

                while (sr.Peek() != -1)
                {
                    var l = sr.ReadLine();
                    csvDatas.Add(l.Split(','));
                }

                if (csvDatas.Count < 1)
                    throw new System.Exception("ファイル読み込みの失敗");

                return csvDatas;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }

        /// <summary>
        /// システム設定キャッシュをオブジェクトへ一時セット
        /// </summary>
        /// <param name="datas">二次元配列の文字列データ</param>
        /// <returns>格納オブジェクト</returns>
        public Dictionary<EnumSystemCommonCash, int> GetSystemCommonCash(List<string[]> datas)
        {
            try
            {
                var configMap = new Dictionary<EnumSystemCommonCash, int>();
                // 一行目はカラム名なのでスキップ
                for (var i = 1; i < datas.Count; i++)
                {
                    var child = datas[i];
                    for (var j = 0; j < child.Length; j++)
                    {
                        configMap[(EnumSystemCommonCash)j] = int.Parse(child[j]);
                    }
                }

                return configMap;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }

        /// <summary>
        /// システムオプション設定をオブジェクトへ一時セット
        /// </summary>
        /// <param name="datas">二次元配列の文字列データ</param>
        /// <returns>格納オブジェクト</returns>
        public Dictionary<EnumSystemConfig, int> GetSystemConfig(List<string[]> datas)
        {
            try
            {
                var configMap = new Dictionary<EnumSystemConfig, int>();
                // 一行目はカラム名なのでスキップ
                for (var i = 1; i < datas.Count; i++)
                {
                    var child = datas[i];
                    for (var j = 0; j < child.Length; j++)
                    {
                        configMap[(EnumSystemConfig)j] = int.Parse(child[j]);
                    }
                }

                return configMap;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }

        /// <summary>
        /// ステージクリア済みデータをオブジェクトへ一時セット
        /// </summary>
        /// <param name="datas">二次元配列の文字列データ</param>
        /// <returns>格納オブジェクト配列</returns>
        public Dictionary<EnumMainSceneStagesState, int>[] GetMainSceneStagesState(List<string[]> datas)
        {
            try
            {
                var configMapList = new List<Dictionary<EnumMainSceneStagesState, int>>();
                // 配列のインデックスとステージIDを揃えるため、0番目はダミーデータを格納
                var dummy = new Dictionary<EnumMainSceneStagesState, int>();
                dummy[EnumMainSceneStagesState.State] = -1;
                configMapList.Add(dummy);
                for (var i = 0; i < datas.Count; i++)
                {
                    if (i == 0)
                        // 一行目はカラム名なのでスキップ
                        continue;
                    var child = datas[i];
                    for (var j = 0; j < child.Length; j++)
                    {
                        var configMap = new Dictionary<EnumMainSceneStagesState, int>();
                        configMap[(EnumMainSceneStagesState)j] = int.Parse(child[j]);
                        configMapList.Add(configMap);
                    }
                }

                return configMapList.ToArray();
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }

        /// <summary>
        /// システム設定キャッシュをCSVデータへ保存
        /// </summary>
        /// <param name="resourcesLoadName">リソースCSVファイル名</param>
        /// <param name="configMap">格納オブジェクト</param>
        /// <returns>成功／失敗</returns>
        public bool SaveDatasCSVOfSystemCommonCash(string resourcesLoadName, Dictionary<EnumSystemCommonCash, int> configMap)
        {
            try
            {
                var path = GetHomePath();
                // 一度ファイル内のデータを削除
                using (var fileStream = new FileStream($"{path}{resourcesLoadName}.csv", FileMode.Open))
                {
                    fileStream.SetLength(0);
                }
                // 設定内容を保存
                using (var sw = new StreamWriter($"{path}{resourcesLoadName}.csv", true, Encoding.GetEncoding("UTF-8")))
                {
                    sw.WriteLine(string.Join(",", GetKeysRecord(configMap)));
                    sw.WriteLine(string.Join(",", GetValuesRecord(configMap)));
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// システムオプション設定をCSVデータへ保存
        /// </summary>
        /// <param name="resourcesLoadName">リソースCSVファイル名</param>
        /// <param name="configMap">格納オブジェクト</param>
        /// <returns>成功／失敗</returns>
        public bool SaveDatasCSVOfSystemConfig(string resourcesLoadName, Dictionary<EnumSystemConfig, int> configMap)
        {
            try
            {
                var path = GetHomePath();
                // 一度ファイル内のデータを削除
                using (var fileStream = new FileStream($"{path}{resourcesLoadName}.csv", FileMode.Open))
                {
                    fileStream.SetLength(0);
                }
                // 設定内容を保存
                using (var sw = new StreamWriter($"{path}{resourcesLoadName}.csv", true, Encoding.GetEncoding("UTF-8")))
                {
                    sw.WriteLine(string.Join(",", GetKeysRecord(configMap)));
                    sw.WriteLine(string.Join(",", GetValuesRecord(configMap)));
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// ステージクリア済みデータをCSVデータへ保存
        /// </summary>
        /// <param name="resourcesLoadName">リソースCSVファイル名</param>
        /// <param name="configMaps">格納オブジェクト配列</param>
        /// <returns>成功／失敗</returns>
        public bool SaveDatasCSVOfMainSceneStagesState(string resourcesLoadName, Dictionary<EnumMainSceneStagesState, int>[] configMaps)
        {
            try
            {
                var path = GetHomePath();
                // 一度ファイル内のデータを削除
                using (var fileStream = new FileStream($"{path}{resourcesLoadName}.csv", FileMode.Open))
                {
                    fileStream.SetLength(0);
                }
                // 設定内容を保存
                using (var sw = new StreamWriter($"{path}{resourcesLoadName}.csv", true, Encoding.GetEncoding("UTF-8")))
                {
                    for (var i = 0; i < configMaps.Length; i++)
                    {
                        if (i == 0)
                        {
                            sw.WriteLine(string.Join(",", GetKeysRecord(configMaps[i])));
                            // 1行目は空データのためスキップ
                            continue;
                        }
                        sw.WriteLine(string.Join(",", GetValuesRecord(configMaps[i])));
                    }
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// キーのレコードを取得
        /// </summary>
        /// <param name="configMap">格納オブジェクト</param>
        /// <returns>CSVのタイトル箇所</returns>
        private string[] GetKeysRecord(Dictionary<EnumSystemConfig, int> configMap)
        {
            return configMap.Select(q => q.Key + "").ToArray();
        }

        /// <summary>
        /// キーのレコードを取得
        /// </summary>
        /// <param name="configMap">格納オブジェクト</param>
        /// <returns>CSVのタイトル箇所</returns>
        private string[] GetKeysRecord(Dictionary<EnumMainSceneStagesState, int> configMap)
        {
            return configMap.Select(q => q.Key + "").ToArray();
        }

        /// <summary>
        /// Valueのレコードを取得
        /// </summary>
        /// <param name="configMap">格納オブジェクト</param>
        /// <returns>一行分のレコード</returns>
        private string[] GetValuesRecord(Dictionary<EnumSystemConfig, int> configMap)
        {
            return configMap.Select(q => q.Value + "").ToArray();
        }

        /// <summary>
        /// Valueのレコードを取得
        /// </summary>
        /// <param name="configMap">格納オブジェクト</param>
        /// <returns>一行分のレコード</returns>
        private string[] GetValuesRecord(Dictionary<EnumMainSceneStagesState, int> configMap)
        {
            return configMap.Select(q => q.Value + "").ToArray();
        }
        /// <summary>
        /// キーのレコードを取得
        /// </summary>
        /// <param name="configMap">格納オブジェクト</param>
        /// <returns>CSVのタイトル箇所</returns>
        private string[] GetKeysRecord(Dictionary<EnumSystemCommonCash, int> configMap)
        {
            return configMap.Select(q => q.Key + "").ToArray();
        }

        /// <summary>
        /// Valueのレコードを取得
        /// </summary>
        /// <param name="configMap">格納オブジェクト</param>
        /// <returns>一行分のレコード</returns>
        private string[] GetValuesRecord(Dictionary<EnumSystemCommonCash, int> configMap)
        {
            return configMap.Select(q => q.Value + "").ToArray();
        }
    }
}
