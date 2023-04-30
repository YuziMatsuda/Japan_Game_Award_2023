using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Area.Common;
using Area.Template;
using System.Linq;

namespace Area.Model
{
    /// <summary>
    /// Fungusのフローチャート
    /// ADVの台詞でコールバックさせる
    /// モデル
    /// </summary>
    public class FlowchartModel : MonoBehaviour, IFlowchartModel
    {
        /// <summary>シナリオ番号</summary>
        private readonly IntReactiveProperty _readedScenarioNo = new IntReactiveProperty();
        /// <summary>シナリオ番号</summary>
        public IReactiveProperty<int> ReadedScenarioNo => _readedScenarioNo;

        /// <summary>
        /// ミッションIDに紐づくシナリオブロック名称を管理
        /// </summary>
        [System.Serializable]
        public struct BlockNamesFromMissionID
        {
            /// <summary>ミッションID</summary>
            public EnumMissionID enumMissionID;
            /// <summary>シナリオブロック名</summary>
            public string blockName;
            // Scenario0OPbetweenArea1Clear
        }

        /// <summary>
        /// ミッションIDに紐づくシナリオブロック名称を管理
        /// </summary>
        [SerializeField] private BlockNamesFromMissionID[] blockNamesFromMissionIDs;

        /// <summary>
        /// シナリオ読み込まれた
        /// </summary>
        public void OnReadedScenerio()
        {
            // T.B.D シナリオの管理方法を検討
            // シナリオごとにメソッドを用意する必要があるか？
            // シナリオが最後まで読まれた時に渡す番号や特定のイベントで渡す番号などを管理すれば可能？
            _readedScenarioNo.Value = 1;
        }

        public string GetBlockName()
        {
            var missionID = "";
            var temp = new AreaTemplateResourcesAccessory();
            // 実績一覧管理を取得
            var mission = temp.GetMission(temp.LoadSaveDatasCSV(ConstResorcesNames.MISSION));
            // 実績履歴を取得
            var history = temp.GetMissionHistory(temp.LoadSaveDatasCSV(ConstResorcesNames.MISSION_HISTORY));
            // 実績一覧管理のアンロック解除している実績IDが履歴に見つからない場合は演出を実行する
            foreach (var item in mission.Where(q => q[EnumMission.CategoryID].Equals($"{EnumCategoryID.CA0000}") &&
                q[EnumMission.Unlock].Equals(ConstGeneric.DIGITFORM_TRUE))
                .Select(q => q))
                if (history.Where(q => q[EnumMissionHistory.History].Equals($"{item[EnumMission.MissionID]}"))
                    .Select(q => q)
                    .ToArray().Length < 1)
                {
                    missionID = $"{item[EnumMission.MissionID]}";

                    break;
                }
            if (string.IsNullOrEmpty(missionID))
                return "";

            if (blockNamesFromMissionIDs.Length < 1)
                throw new System.Exception("データが空");

            return 0 < blockNamesFromMissionIDs.Where(q => $"{q.enumMissionID}".Equals(missionID))
                .Select(q => q.blockName)
                .Distinct()
                .ToArray().Length ? blockNamesFromMissionIDs.Where(q => $"{q.enumMissionID}".Equals(missionID))
                .Select(q => q.blockName)
                .Distinct()
                .ToArray()[0] : "";
        }
    }

    /// <summary>
    /// Fungusのフローチャート
    /// ADVの台詞でコールバックさせる
    /// モデル
    /// インターフェース
    /// </summary>
    public interface IFlowchartModel
    {
        /// <summary>
        /// ミッションIDから対象のブロック名を取得
        /// </summary>
        /// <param name="enumMissionID">ミッションID</param>
        /// <returns>ブロック名</returns>
        public string GetBlockName();
    }
}
