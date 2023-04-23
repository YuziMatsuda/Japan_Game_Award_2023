using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// ビュー
    /// 準委任帳票
    /// </summary>
    public class AssignedSeastarCountView : MonoBehaviour, ISeastarCount
    {
        /// <summary>ヒトデ総配属人数</summary>
        [SerializeField] private SeastarCount seastarCount;

        private void Reset()
        {
            seastarCount = transform.GetChild(1).GetComponent<SeastarCount>();
        }

        public bool SetCounterText(int countValue)
        {
            return seastarCount.SetCounterText(countValue);
        }
    }
}
