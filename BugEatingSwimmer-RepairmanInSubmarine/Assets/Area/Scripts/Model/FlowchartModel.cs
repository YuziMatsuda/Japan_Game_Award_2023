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
        public void OnReadedScenario()
        {
            // T.B.D シナリオの管理方法を検討
            // シナリオごとにメソッドを用意する必要があるか？
            // シナリオが最後まで読まれた時に渡す番号や特定のイベントで渡す番号などを管理すれば可能？
            _readedScenarioNo.Value = 1;
        }

        /// <summary>
        /// （ロボ、信号を飛ばす。ボディの電源が入る）
        /// </summary>
        public void OnSendSignalAndBodyTurnsOn()
        {
            // T.B.D ステータスを管理
            //_readedScenarioNo.Value = ?;
        }

        /// <summary>
        /// （回想終了）
        /// </summary>
        public void OnRecollectionEnds()
        {
            // T.B.D ステータスを管理
            //_readedScenarioNo.Value = ?;
        }

        /// <summary>
        /// （ロボ、信号を飛ばす。右腕、左腕の電源が入る）
        /// </summary>
        public void OnSendSignalAndRightArmAndLeftArmTurnsOn()
        {
            // T.B.D ステータスを管理
            //_readedScenarioNo.Value = ?;
        }

        /// <summary>
        /// （画面が揺れる ロボ、びっくり）
        /// </summary>
        public void OnScreenShakesLoboIsStartled()
        {
            // T.B.D ステータスを管理
            //_readedScenarioNo.Value = ?;
        }

        /// <summary>
        /// （画面が揺れて土砂やゴミが投入される ロボ、再度びっくり）
        /// </summary>
        public void OnScreenShakesLoboIsStartledAgain()
        {
            // T.B.D ステータスを管理
            //_readedScenarioNo.Value = ?;
        }

        /// <summary>
        /// （画面転換 一枚絵 夜、湖の畔の工事現場が燃えている 鳴り響くサイレンと爆発音）
        /// </summary>
        public void OnScreenShiftSinglePictureAtNightConstructionSiteByTheLakeIsOnFireSirensBlaringAndExplosions()
        {
            // T.B.D ステータスを管理
            //_readedScenarioNo.Value = ?;
        }

        /// <summary>
        /// （画面転換 フェードアウト（真っ暗） 泡の音）
        /// </summary>
        public void OnScreenChangeFadeOut_PitchBlack_SoundOfBubbles()
        {
            // T.B.D ステータスを管理
            //_readedScenarioNo.Value = ?;
        }

        /// <summary>
        /// （画面転換 一枚絵 どこかの家のリビング）
        /// </summary>
        public void OnScreenChangeSinglePictureLivingRoomOfAHouseSomewhere()
        {
            // T.B.D ステータスを管理
            //_readedScenarioNo.Value = ?;
        }

        /// <summary>
        /// （タイトルがバーンと出てエンディング）
        /// </summary>
        public void OnTitleBlastsOutEnding()
        {
            // T.B.D ステータスを管理
            //_readedScenarioNo.Value = ?;
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
