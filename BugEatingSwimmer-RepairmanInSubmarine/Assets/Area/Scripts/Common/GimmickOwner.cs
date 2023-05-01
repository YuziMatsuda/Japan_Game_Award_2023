using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Area.Common
{
    /// <summary>
    /// ギミックのオーナー
    /// </summary>
    [RequireComponent(typeof(SeastarLeader))]
    public class GimmickOwner : MonoBehaviour, IAreaGameManager, ISeastarLeader
    {
        /// <summary>ヒトデのリーダー</summary>
        [SerializeField] private SeastarLeader seastarLeader;

        private void Reset()
        {
            seastarLeader = GetComponent<SeastarLeader>();
        }

        public void OnStart()
        {
            seastarLeader.OnStart();
        }

        public bool IsAssigned(EnumSeastarID enumSeastarID)
        {
            return seastarLeader.IsAssigned(enumSeastarID);
        }

        public bool SetAssignState(EnumSeastarID enumSeastarID, bool assignState)
        {
            return seastarLeader.SetAssignState(enumSeastarID, assignState);
        }

        public bool SaveAssigned()
        {
            return seastarLeader.SaveAssigned();
        }

        public int GetAssinedCounter()
        {
            return seastarLeader.GetAssinedCounter();
        }
    }
}
