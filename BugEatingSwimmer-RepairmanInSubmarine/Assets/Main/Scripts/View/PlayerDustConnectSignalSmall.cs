using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// プレイヤーのシグナル
    /// </summary>
    [RequireComponent(typeof(ParticleSystem))]
    public class PlayerDustConnectSignalSmall : ShadowCodeCellParent, IPlayerHalo
    {
        /// <summary>パーティクル</summary>
        [SerializeField] private new ParticleSystem particleSystem;

        private void Reset()
        {
            particleSystem = GetComponent<ParticleSystem>();
        }

        public bool ChangeChargeMode(int idx, bool enabled)
        {
            throw new System.NotImplementedException();
        }

        public bool SetHaloEnabled(bool enabled)
        {
            try
            {
                gameObject.SetActive(enabled);

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }
    }
}
