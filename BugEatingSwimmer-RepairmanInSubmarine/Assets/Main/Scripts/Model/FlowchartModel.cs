using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Main.Common;
using Main.Template;
using System.Linq;

namespace Main.Model
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
        /// ステージ番号に紐づくシナリオブロック名称を管理
        /// </summary>
        [System.Serializable]
        public struct BlockNamesFromSceneId
        {
            /// <summary>ステージ番号</summary>
            public int sceneId;
            /// <summary>シナリオブロック名</summary>
            public string blockName;
        }

        /// <summary>
        /// ステージ番号に紐づくシナリオブロック名称を管理
        /// </summary>
        [SerializeField] private BlockNamesFromSceneId[] blockNamesFromSceneIds;

        /// <summary>
        /// ミッションIDに紐づくシナリオブロック名称を管理
        /// </summary>
        [System.Serializable]
        public struct BlockNamesFromMissionID
        {
            /// <summary>ミッションID</summary>
            public EnumMissionID enumMissionID;
            /// <summary>サブ番号</summary>
            public int subNumber;
            /// <summary>シナリオブロック名</summary>
            public string blockName;
        }

        /// <summary>
        /// ミッションIDに紐づくシナリオブロック名称を管理
        /// </summary>
        [SerializeField] private BlockNamesFromMissionID[] blockNamesFromMissionIDs;

        /// <summary>
        /// シナリオ読み込まれた
        /// </summary>
        public void OnReadedScenario()
        {
            // T.B.D シナリオの管理方法を検討
            // シナリオごとにメソッドを用意する必要があるか？
            // シナリオが最後まで読まれた時に渡す番号や特定のイベントで渡す番号などを管理すれば可能？
            _readedScenarioNo.Value = 1;
        }

        /// <summary>
        /// チュートリアルのシナリオ読み込まれた
        /// </summary>
        public void OnReadedScenarioTutorial()
        {
            // T.B.D シナリオの管理方法を検討
            // シナリオごとにメソッドを用意する必要があるか？
            // シナリオが最後まで読まれた時に渡す番号や特定のイベントで渡す番号などを管理すれば可能？
            _readedScenarioNo.Value = 2;
        }

        public void OnOtherAdditions_1()
        {
            // T.B.D ステータスを管理
            //_readedScenarioNo.Value = ?;
        }

        public void OnOtherAdditions_2()
        {
            // T.B.D ステータスを管理
            //_readedScenarioNo.Value = ?;
        }

        public void OnOtherAdditions_3()
        {
            // T.B.D ステータスを管理
            //_readedScenarioNo.Value = ?;
        }

        public void OnOtherAdditions_4()
        {
            // T.B.D ステータスを管理
            //_readedScenarioNo.Value = ?;
        }

        public void OnOtherAdditions_5()
        {
            // T.B.D ステータスを管理
            //_readedScenarioNo.Value = ?;
        }

        public void OnOtherAdditions_6()
        {
            // T.B.D ステータスを管理
            //_readedScenarioNo.Value = ?;
        }

        public string GetBlockName(int sceneId)
        {
            return GetBlockName(sceneId, -1);
        }

        public string GetBlockName(int sceneId, int subNumber)
        {
            if (blockNamesFromSceneIds.Length < 1)
                throw new System.Exception("データが空");

            // シーンごとのチュートリアルはサブ番号を使わない想定
            if (0 < sceneId &&
                subNumber < 0)
            {
                return 0 < blockNamesFromSceneIds.Where(q => q.sceneId == sceneId)
                    .Select(q => q.blockName)
                    .Distinct()
                    .ToArray().Length ? blockNamesFromSceneIds.Where(q => q.sceneId == sceneId)
                    .Select(q => q.blockName)
                    .Distinct()
                    .ToArray()[0] : "";
            }
            else
            {
                // チュートリアル
                var missionID = "";
                var temp = new MainTemplateResourcesAccessory();
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

                return 0 < blockNamesFromMissionIDs.Where(q => $"{q.enumMissionID}".Equals(missionID) &&
                        q.subNumber == subNumber)
                    .Select(q => q.blockName)
                    .Distinct()
                    .ToArray().Length ? blockNamesFromMissionIDs.Where(q => $"{q.enumMissionID}".Equals(missionID) &&
                        q.subNumber == subNumber)
                    .Select(q => q.blockName)
                    .Distinct()
                    .ToArray()[0] : "";
            }
        }

        public bool SetReadedScenarioNo(int scenarioNo)
        {
            try
            {
                _readedScenarioNo.Value = scenarioNo;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
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
        /// ステージ番号から対象のブロック名を取得
        /// </summary>
        /// <param name="sceneId">ステージ番号</param>
        /// <returns>ブロック名</returns>
        public string GetBlockName(int sceneId);
        /// <summary>
        /// ステージ番号から対象のブロック名を取得
        /// </summary>
        /// <param name="sceneId">ステージ番号</param>
        /// <param name="subNumber">サブ番号</param>
        /// <returns>ブロック名</returns>
        public string GetBlockName(int sceneId, int subNumber);
        /// <summary>
        /// シナリオ番号をセット
        /// </summary>
        /// <param name="scenarioNo">シナリオ番号</param>
        /// <returns>成功／失敗</returns>
        public bool SetReadedScenarioNo(int scenarioNo);
    }
}
