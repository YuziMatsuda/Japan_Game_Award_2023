using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Title.Template;
using System.Linq;

namespace Title.Common
{
    /// <summary>
    /// プレゼンタ
    /// タイトルシーン
    /// 共通処理
    /// </summary>
    public class TitlePresenterCommon : ITitlePresenterCommon
    {
        public string[] GetMissionHistories()
        {
            var temp = new TitleTemplateResourcesAccessory();
            // 実績履歴を取得
            var history = temp.GetMissionHistory(temp.LoadSaveDatasCSV(ConstResorcesNames.MISSION_HISTORY));
            return 0 < history.Length ? history.Select(q => q[EnumMissionHistory.History]).ToArray() : new string[0];
        }

        public Dictionary<EnumMission, string>[] LoadSaveDatasCSVAndGetMission()
        {
            var tResourcesAccessory = new TitleTemplateResourcesAccessory();
            var quasiAssignFormResources = tResourcesAccessory.LoadSaveDatasCSV(ConstResorcesNames.MISSION);
            return tResourcesAccessory.GetMission(quasiAssignFormResources);
        }
    }

    /// <summary>
    /// プレゼンタ
    /// タイトルシーン
    /// 共通処理
    /// インターフェース
    /// </summary>
    public interface ITitlePresenterCommon
    {
        /// <summary>
        /// 実績一覧管理データをオブジェクトへ一時セット
        /// </summary>
        /// <returns>格納オブジェクト配列</returns>
        public Dictionary<EnumMission, string>[] LoadSaveDatasCSVAndGetMission();
        /// <summary>
        /// 実績履歴を取得
        /// </summary>
        /// <returns>実績履歴配列</returns>
        public string[] GetMissionHistories();
    }
}
