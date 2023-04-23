using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Common
{
    /// <summary>
    /// 親パーティクルとして子パーティクルを制御
    /// </summary>
    public class ParticleSystemParent : MonoBehaviour, IParticleSystemParent
    {
        /// <summary>トランスフォーム</summary>
        private Transform _transform;

        public void Play()
        {
            if (_transform == null)
                _transform = transform;

            foreach (Transform child in _transform)
            {
                child.GetComponent<ParticleSystem>().Play();
            }
        }
    }

    /// <summary>
    /// 親パーティクルとして子パーティクルを制御
    /// インターフェース
    /// </summary>
    public interface IParticleSystemParent
    {
        public void Play();
    }
}
