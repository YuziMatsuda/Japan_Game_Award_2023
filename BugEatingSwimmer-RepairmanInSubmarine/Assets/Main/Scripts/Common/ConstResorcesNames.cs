using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Common
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
        /// <summary>システム設定</summary>
        public static readonly string SYSTEM_CONFIG = "SystemConfig";
        /// <summary>システム設定</summary>
        public static readonly string SYSTEM_COMMON_CASH = "SystemCommonCash";
        /// <summary>ステージ設定</summary>
        public static readonly string MAIN_SCENE_STAGES_CONFIG = "MainSceneStagesConfig";
        /// <summary>ステージクリア済みデータ</summary>
        public static readonly string MAIN_SCENE_STAGES_STATE = "MainSceneStagesState";
    }
}
