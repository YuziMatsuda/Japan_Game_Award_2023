using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Area.Common
{
    /// <summary>
    /// キャスト
    /// 親
    /// </summary>
    public class CastsParent : MonoBehaviour
    {
        /// <summary>
        /// 歩くが開始された
        /// Animator->Animation - EndingCastOpen
        /// </summary>
        protected virtual void OnWark() { }
        /// <summary>
        /// 泡発生
        /// Animator->Animation - EndingCastOpen
        /// </summary>
        protected virtual void OnChangeText() { }
    }
}
