using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Common;

namespace Main.View
{
    /// <summary>
    /// ビュー
    /// プレイヤー
    /// </summary>
    public class PlayerView : ShadowCodeCellParent, IPlayerView, IPlayerHalo, IBodySpritePlayer
    {
        /// <summary>プレイヤーのハロー</summary>
        [SerializeField] private Halos halos;
        /// <summary>プレイヤーのハロー</summary>
        public Halos Halos => halos;
        /// <summary>プレイヤーのボディのスプライト</summary>
        [SerializeField] private BodySpritePlayer bodySpritePlayer;

        private void Reset()
        {
            halos = transform.GetChild(3).GetComponent<Halos>();
            bodySpritePlayer = transform.GetChild(0).GetComponent<BodySpritePlayer>();
        }

        public bool StartCharge(float inputPowerChargeTime)
        {
            try
            {
                if (_transform == null)
                    _transform = transform;

                if (!MainGameManager.Instance.ParticleSystemsOwner.PlayParticleSystems(GetInstanceID(), EnumParticleSystemsIndex.ParticlesOfLightGatherAround, _transform.position))
                    Debug.LogError("指定されたパーティクルシステムを再生する呼び出しの失敗");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool StopCharge()
        {
            try
            {
                if (!MainGameManager.Instance.ParticleSystemsOwner.StopParticleSystems(GetInstanceID(), EnumParticleSystemsIndex.ParticlesOfLightGatherAround, true))
                    Debug.LogError("指定されたパーティクルシステムを再生する呼び出しの失敗");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetHaloEnabled(bool enabled)
        {
            throw new System.NotImplementedException();
        }

        public bool ChangeChargeMode(int idx, bool enabled)
        {
            return halos.ChangeChargeMode(idx, enabled);
        }

        public bool HoverCharge()
        {
            try
            {
                if (_transform == null)
                    _transform = transform;

                var particlesOflight = MainGameManager.Instance.ParticleSystemsOwner.GetParticleSystemsTransform(GetInstanceID(), EnumParticleSystemsIndex.ParticlesOfLightGatherAround);
                particlesOflight.transform.position = _transform.position;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool PlayPowerAttackEffect()
        {
            try
            {
                if (_transform == null)
                    _transform = transform;

                if (!MainGameManager.Instance.ParticleSystemsOwner.PlayParticleSystems(GetInstanceID(), EnumParticleSystemsIndex.Explosion, _transform.position))
                    throw new System.Exception("指定されたパーティクルシステムを再生する呼び出しの失敗");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool PlayTurnAnimation()
        {
            return bodySpritePlayer.PlayTurnAnimation();
        }
    }

    public interface IPlayerView
    {
        /// <summary>
        /// チャージ開始
        /// </summary>
        /// <param name="inputPowerChargeTime">パワーチャージ時間</param>
        /// <returns>成功／失敗</returns>
        public bool StartCharge(float inputPowerChargeTime);

        /// <summary>
        /// チャージのホバー
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool HoverCharge();

        /// <summary>
        /// チャージ停止
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool StopCharge();

        /// <summary>
        /// パワーアタックのエフェクト発生
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool PlayPowerAttackEffect();
    }
}
