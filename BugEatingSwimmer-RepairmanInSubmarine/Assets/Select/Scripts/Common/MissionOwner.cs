using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Select.Template;

namespace Select.Common
{
    /// <summary>
    /// 実績一覧管理オーナー
    /// </summary>
    public class MissionOwner : MonoBehaviour, ISelectGameManager, IMissionOwner
    {
        /// <summary>ミッションIDに紐づく「ロボットの結合状態」「エリアID」</summary>
        [SerializeField] private Mission[] missions;
        /// <summary>ミッションIDに紐づく「ロボットの結合状態」「エリアID」</summary>
        public Mission[] Missions => missions;
        /// <summary>ミッション一覧</summary>
        private Dictionary<EnumMission, string>[] _missionsDic;
        /// <summary>ミッション一覧</summary>
        public Dictionary<EnumMission, string>[] MissionsDic => _missionsDic == null ? Load() : _missionsDic;
        /// <summary>バグ修正演出</summary>
        [SerializeField] private Mission[] bugFixedDirectionsInStages;

        public void OnStart()
        {
            Load();
        }

        /// <summary>
        /// セーブデータをロード
        /// </summary>
        private Dictionary<EnumMission, string>[] Load()
        {
            var common = new SelectPresenterCommon();
            if (_missionsDic == null)
                _missionsDic = common.LoadSaveDatasCSVAndGetMission();

            return _missionsDic;
        }

        public bool IsLocked(int idx)
        {
            return 0 < MissionsDic.Where(q => q[EnumMission.MissionID].Equals($"{GetMissionIDFromStageIndex(idx)}") &&
                q[EnumMission.Unlock].Equals(ConstGeneric.DIGITFORM_FALSE))
                .Select(q => q)
                .ToArray()
                .Length;
        }

        /// <summary>
        /// ステージ番号に該当する実績IDを取得
        /// </summary>
        /// <param name="idx">ステージ番号</param>
        /// <returns>実績ID</returns>
        private EnumMissionID GetMissionIDFromStageIndex(int idx)
        {
            try
            {
                var ary = bugFixedDirectionsInStages.Where(q => q.stageIndex == idx)
                    .Select(q => q.enumMissionID)
                    .ToArray();
                if (ary.Length <= 0 ||
                    1 < ary.Length)
                    throw new System.Exception($"該当番号が未設定または重複_idx:[{idx}]");

                return ary[0];
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        public bool ReLoad()
        {
            try
            {
                var common = new SelectPresenterCommon();
                _missionsDic = common.LoadSaveDatasCSVAndGetMission();

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SaveTempOfDirection(int idx)
        {
            try
            {
                MissionsDic.Where(q => q[EnumMission.MissionID].Equals($"{GetMissionIDFromStageIndex(idx)}") &&
                    q[EnumMission.Unlock].Equals(ConstGeneric.DIGITFORM_FALSE))
                    .Select(q => q)
                    .ToArray()[0][EnumMission.Unlock] = ConstGeneric.DIGITFORM_TRUE;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        private void OnDestroy()
        {
            var temp = new SelectTemplateResourcesAccessory();
            if (!temp.SaveDatasCSVOfMission(ConstResorcesNames.MISSION, MissionsDic))
                throw new System.Exception("実績一覧管理データをCSVデータへ保存呼び出しの失敗");
        }
    }

    /// <summary>
    /// 実績一覧管理オーナー
    /// インターフェース
    /// </summary>
    public interface IMissionOwner
    {
        /// <summary>
        /// 実績解除済みか
        /// </summary>
        /// <param name="idx">ステージ番号</param>
        /// <returns>実績解除済みか</returns>
        public bool IsLocked(int idx);
        /// <summary>
        /// リロード
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool ReLoad();
        /// <summary>
        /// 実績の一時保存
        /// </summary>
        /// <param name="idx">ステージ番号</param>
        /// <returns>成功／失敗</returns>
        public bool SaveTempOfDirection(int idx);
    }
}
