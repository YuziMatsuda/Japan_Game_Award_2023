using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Common
{
    /// <summary>
    /// パーティクルシステムのオーナー
    /// </summary>
    public class ParticleSystemsOwner : MonoBehaviour, IMainGameManager, IParticleSystemsOwner
    {
        /// <summary>トランスフォーム</summary>
        private Transform _transform;
        /// <summary>パーティクルシステムを配列管理</summary>
        [SerializeField] private Transform[] particleSystems;
        /// <summary>プール済みのパーティクルシステム情報マップ</summary>
        private Dictionary<int, int> _particleSystemsIdxDictionary = new Dictionary<int, int>();

        public Transform GetParticleSystemsTransform(int instanceID, EnumParticleSystemsIndex index)
        {
            return GetParticleSystems(instanceID, index).transform;
        }

        public void OnStart()
        {
            if (_transform == null)
                _transform = transform;
        }

        public bool PlayParticleSystems(int instanceID, EnumParticleSystemsIndex index, Vector3 position)
        {
            try
            {
                var particle = GetParticleSystems(instanceID, index);
                particle.transform.position = position;
                particle.Play();

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool StopParticleSystems(int instanceID, EnumParticleSystemsIndex index)
        {
            try
            {
                var particle = GetParticleSystems(instanceID, index);
                particle.Stop();

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// SFXのキーから対象のパーティクルシステムを取得する
        /// </summary>
        /// <param name="instanceID">オブジェクト識別ID</param>
        /// <param name="index">パーティクルシステムのインデックス</param>
        /// <returns>パーティクルシステム</returns>
        private ParticleSystem GetParticleSystems(int instanceID, EnumParticleSystemsIndex index)
        {
            if (!_particleSystemsIdxDictionary.ContainsKey(instanceID))
            {
                var sfx = Instantiate(particleSystems[(int)index], _transform);
                _particleSystemsIdxDictionary.Add(instanceID, _transform.childCount - 1);
                return sfx.GetComponent<ParticleSystem>();
            }
            return _transform.GetChild(_particleSystemsIdxDictionary[instanceID]).GetComponent<ParticleSystem>();
        }
    }

    /// <summary>
    /// パーティクルシステムのオーナー
    /// インターフェース
    /// </summary>
    public interface IParticleSystemsOwner
    {
        /// <summary>
        /// 指定されたパーティクルシステムを再生する
        /// </summary>
        /// <param name="instanceID">オブジェクト識別ID</param>
        /// <param name="index">パーティクルシステムのインデックス</param>
        /// <param name="position">配置位置</param>
        /// <returns>成功／失敗</returns>
        public bool PlayParticleSystems(int instanceID, EnumParticleSystemsIndex index, Vector3 position);

        /// <summary>
        /// 指定されたパーティクルシステムを停止する
        /// </summary>
        /// <param name="instanceID">オブジェクト識別ID</param>
        /// <param name="index">パーティクルシステムのインデックス</param>
        /// <returns>成功／失敗</returns>
        public bool StopParticleSystems(int instanceID, EnumParticleSystemsIndex index);

        /// <summary>
        /// パーティクルシステムのトランスフォーム取得
        /// </summary>
        /// <param name="instanceID">オブジェクト識別ID</param>
        /// <param name="index">パーティクルシステムのインデックス</param>
        /// <returns>トランスフォーム</returns>
        public Transform GetParticleSystemsTransform(int instanceID, EnumParticleSystemsIndex index);
    }
}
