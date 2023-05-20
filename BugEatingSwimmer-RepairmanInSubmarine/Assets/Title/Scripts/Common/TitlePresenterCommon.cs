using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Title.Template;

namespace Title.Common
{
    /// <summary>
    /// プレゼンタ
    /// タイトルシーン
    /// 共通処理
    /// </summary>
    public class TitlePresenterCommon : ITitlePresenterCommon
    {
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
    }
}
