using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Common;

namespace Main.Model
{
    /// <summary>
    /// コシギンチャク・ジョーシー
    /// 設定
    /// </summary>
    public class LoinclothConfig : MonoBehaviour
    {
        /// <summary>部署コード</summary>
        [SerializeField] private EnumDepartmentCode enumDepartmentCode;
        /// <summary>部署コード</summary>
        public EnumDepartmentCode EnumDepartmentCode => enumDepartmentCode;
        /// <summary>コシギンチャクの詳細</summary>
        [SerializeField] private LoinclothDetail loinclothDetail;
        /// <summary>コシギンチャクの詳細</summary>
        public LoinclothDetail LoinclothDetail => loinclothDetail;
        /// <summary>ジョーシーの詳細</summary>
        [SerializeField] private JawsHiDetail jawsHiDetail;
        /// <summary>ジョーシーの詳細</summary>
        public JawsHiDetail JawsHiDetail => jawsHiDetail;
    }

    /// <summary>
    /// コシギンチャクの詳細
    /// </summary>
    [System.Serializable]
    public struct LoinclothDetail
    {
        /// <summary>差分スプライト配列</summary>
        public Sprite[] sprites;
    }

    /// <summary>
    /// ジョーシーの詳細
    /// </summary>
    [System.Serializable]
    public struct JawsHiDetail
    {
        /// <summary>ターゲットのプレハブ</summary>
        public Transform targetPrefab;
        /// <summary>差分スプライト配列</summary>
        public Sprite[] sprites;
        /// <summary>評判タイプ</summary>
        public EnumReputationType enumReputationType;
    }
}
