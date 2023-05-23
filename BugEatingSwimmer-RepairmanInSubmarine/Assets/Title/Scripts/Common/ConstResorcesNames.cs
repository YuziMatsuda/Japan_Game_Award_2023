using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Title.Common
{
    /// <summary>
    /// リソースフォルダ内のファイル名
    /// </summary>
    public class ConstResorcesNames
    {
        /// <summary>ホームディレクトリ（UnityEditor用）</summary>
        public static readonly string HOMEPATH_UNITYEDITOR = @".\Assets\SaveDatas\";
        /// <summary>ホームディレクトリ（ビルド用）</summary>
        public static readonly string HOMEPATH_BUILD = @".\SaveDatas\";
        /// <summary>システム設定キャッシュ</summary>
        public static readonly string SYSTEM_COMMON_CASH = "SystemCommonCash";
        /// <summary>システム設定</summary>
        public static readonly string SYSTEM_CONFIG = "SystemConfig";
        /// <summary>ステージクリア済みデータ</summary>
        public static readonly string MAIN_SCENE_STAGES_STATE = "MainSceneStagesState";
        /// <summary>実績一覧管理</summary>
        public static readonly string MISSION = "Mission";
        /// <summary>実績履歴</summary>
        public static readonly string MISSION_HISTORY = "MissionHistory";
        /// <summary>準委任帳票</summary>
        public static readonly string QUASI_ASSIGNMENT_FORM = "QuasiAssignmentForm";
        /// <summary>エリアユニット</summary>
        public static readonly string AREA_UNITS = "AreaUnits";
        /// <summary>エリア解放・結合テスト</summary>
        public static readonly string AREA_OPENED_AND_IT_STATE = "AreaOpenedAndITState";
        /// <summary>ステージクリア条件</summary>
        public static readonly string MAIN_SCENE_STAGES_MODULES_STATE = "MainSceneStagesModulesState";
        /// <summary>＋で全ステージ解放</summary>
        public static readonly string ALL = "All";
    }
}
