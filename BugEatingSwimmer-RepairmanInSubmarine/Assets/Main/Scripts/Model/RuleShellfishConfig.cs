using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Model
{
    /// <summary>
    /// ルール貝
    /// 設定
    /// </summary>
    public class RuleShellfishConfig : MonoBehaviour
    {
        /// <summary>再び話しかけることが可能</summary>
        [SerializeField] private bool isRetake;
        /// <summary>再び話しかけることが可能</summary>
        public bool IsRetake => isRetake;
        /// <summary>再び話しかけることが可能プロパティ</summary>
        [SerializeField] private RetakeProperty retakeProperty;
        /// <summary>チュートリアル読了後に表示するオブジェクトがある</summary>
        [SerializeField] private bool isInstanceOfReadedScenarioTutorial;
        /// <summary>チュートリアル読了後に表示するオブジェクトがある</summary>
        public bool IsInstanceOfReadedScenarioTutorial => isInstanceOfReadedScenarioTutorial;
        /// <summary>チュートリアル読了後に表示するオブジェクトのプロパティ</summary>
        [SerializeField] private InstanceOfReadedScenarioTutorialProperty instanceOfReadedScenarioTutorialProperty;
        /// <summary>チュートリアル読了後に表示するオブジェクトのプロパティ</summary>
        public InstanceOfReadedScenarioTutorialProperty InstanceOfReadedScenarioTutorialProperty => instanceOfReadedScenarioTutorialProperty;
    }

    /// <summary>
    /// 再び話しかけることが可能な場合のプロパティ
    /// </summary>
    [System.Serializable]
    public struct RetakeProperty
    {
        /// <summary>会話ブロック番号</summary>
        public int blockNamesFromSceneIdsOfRetakeIdx;
    }

    /// <summary>
    /// チュートリアル読了後に表示するオブジェクトのプロパティ
    /// </summary>
    [System.Serializable]
    public struct InstanceOfReadedScenarioTutorialProperty
    {
        /// <summary>ガイドUI番号</summary>
        public int[] guideUIIdxes;
    }
}
