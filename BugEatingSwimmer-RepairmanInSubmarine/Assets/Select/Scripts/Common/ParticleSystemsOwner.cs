using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Select.Common
{
    /// <summary>
    /// パーティクルシステムのオーナー
    /// </summary>
    public class ParticleSystemsOwner : MonoBehaviour, ISelectGameManager, IParticleSystemsOwner
    {
        /// <summary>トランスフォーム</summary>
        private Transform _transform;
        /// <summary>パーティクルシステムを配列管理</summary>
        [SerializeField] private Transform[] particleSystems;
        /// <summary>プール済みのパーティクルシステム情報マップ</summary>
        private Dictionary<string, int> _particleSystemsIdxDictionary = new Dictionary<string, int>();

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
                particle.position = position;
                if (!particle.gameObject.activeSelf)
                    particle.gameObject.SetActive(true);
                if (particle.GetComponent<ParticleSystem>() != null)
                    particle.GetComponent<ParticleSystem>().Play();
                else if (particle.GetComponent<ParticleSystemParent>() != null)
                    particle.GetComponent<ParticleSystemParent>().Play();

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
            return StopParticleSystems(instanceID, index, false);
        }

        public bool StopParticleSystems(int instanceID, EnumParticleSystemsIndex index, bool stopImmediately)
        {
            try
            {
                var particle = GetParticleSystems(instanceID, index);
                if (!stopImmediately)
                {
                    if (particle.GetComponent<ParticleSystem>() != null)
                        particle.GetComponent<ParticleSystem>().Stop();
                    else if (particle.GetComponent<ParticleSystemParent>() != null)
                        particle.GetComponent<ParticleSystemParent>().Play();
                }
                else
                    particle.gameObject.SetActive(false);

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// SFXのキーから対象のパーティクルシステムを持つオブジェクトを取得する
        /// </summary>
        /// <param name="instanceID">オブジェクト識別ID</param>
        /// <param name="index">パーティクルシステムのインデックス</param>
        /// <returns>パーティクルシステム</returns>
        private Transform GetParticleSystems(int instanceID, EnumParticleSystemsIndex index)
        {
            if (!_particleSystemsIdxDictionary.ContainsKey($"{instanceID + index}"))
            {
                var sfx = Instantiate(particleSystems[(int)index], _transform);
                _particleSystemsIdxDictionary.Add($"{instanceID + index}", _transform.childCount - 1);
                return sfx;
            }
            return _transform.GetChild(_particleSystemsIdxDictionary[$"{instanceID + index}"]);
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
        /// 指定されたパーティクルシステムを停止する
        /// </summary>
        /// <param name="instanceID">オブジェクト識別ID</param>
        /// <param name="index">パーティクルシステムのインデックス</param>
        /// <param name="stopImmediately">即時で止める</param>
        /// <returns>成功／失敗</returns>
        public bool StopParticleSystems(int instanceID, EnumParticleSystemsIndex index, bool stopImmediately);

        /// <summary>
        /// パーティクルシステムのトランスフォーム取得
        /// </summary>
        /// <param name="instanceID">オブジェクト識別ID</param>
        /// <param name="index">パーティクルシステムのインデックス</param>
        /// <returns>トランスフォーム</returns>
        public Transform GetParticleSystemsTransform(int instanceID, EnumParticleSystemsIndex index);
    }
}
