using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Common
{
    /// <summary>
    /// ギミックのオーナー
    /// </summary>
    [RequireComponent(typeof(SeastarLeader))]
    [RequireComponent(typeof(PowerShellLeader))]
    public class GimmickOwner : MonoBehaviour, IMainGameManager, ISeastarLeader
    {
        /// <summary>ヒトデのリーダー</summary>
        [SerializeField] private SeastarLeader seastarLeader;
        /// <summary>パワーシェルのリーダー</summary>
        [SerializeField] private PowerShellLeader powerShellLeader;

        private void Reset()
        {
            seastarLeader = GetComponent<SeastarLeader>();
            powerShellLeader = GetComponent<PowerShellLeader>();
        }

        public void OnStart()
        {
            seastarLeader.OnStart();
            powerShellLeader.OnStart();
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
