using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Common;
using Effect;
using DG.Tweening;
using Main.Audio;

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
        /// <summary>アニメーション終了時間</summary>
        [SerializeField] private float[] durations = { 1.5f, .75f, .75f, .25f };
        /// <summary>SFXを再生 チャージSEをセット</summary>
        [SerializeField] private ClipToPlay[] playSFXChargeModes = { ClipToPlay.se_energy_store, ClipToPlay.se_scene_change_slide_exp_04 };

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

        public bool InstanceBubble()
        {
            try
            {
                if (_transform == null)
                    _transform = transform;

                // 泡をプレイヤーへ追尾させる
                if (!MainGameManager.Instance.ParticleSystemsOwner.PlayParticleSystems(GetInstanceID(), EnumParticleSystemsIndex.ParticleJigglyBubbleSoapyPlayer, _transform.position))
                    throw new System.Exception("指定されたパーティクルシステムを再生する呼び出しの失敗");
                var bubble = MainGameManager.Instance.ParticleSystemsOwner.GetParticleSystemsTransform(GetInstanceID(), EnumParticleSystemsIndex.ParticleJigglyBubbleSoapyPlayer);
                if (bubble.GetComponent<TrackMovement>() == null)
                {
                    bubble.gameObject.AddComponent<TrackMovement>();
                    if (bubble.GetComponent<TrackMovement>().Target == null ||
                        (bubble.GetComponent<TrackMovement>().Target != null &&
                            !bubble.GetComponent<TrackMovement>().Target.Equals(transform)))
                        if (!bubble.GetComponent<TrackMovement>().SetTarget(transform))
                            throw new System.Exception("ターゲットをセットする呼び出しの失敗");
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public Transform InstanceGhost()
        {
            var ghost = GameObject.Find("PlayerGhost");
            if (ghost == null)
            {
                ghost = new GameObject();
                ghost.name = "PlayerGhost";
                return ghost.transform;
            }
            else
            {
                ghost.transform.position = transform.position;
                return ghost.transform;
            }
        }

        public IEnumerator PlayMoveAnimation(System.IObserver<bool> observer, Vector3 target)
        {
            if (_transform == null)
                _transform = transform;

            _transform.localEulerAngles = Vector3.right;
            _transform.DOMove(target, durations[0])
                .SetEase(Ease.InOutQuad)
                .OnComplete(() => observer.OnNext(true));

            yield return null;
        }

        public IEnumerator PlayDiveAnimation(System.IObserver<bool> observer, Vector3[] targets)
        {
            if (_transform == null)
                _transform = transform;

            _transform.localEulerAngles = new Vector3(0, 0, 30f);
            DOTween.Sequence()
                .Append(_transform.DOMove(targets[0], durations[1]))
                .Append(_transform.DOMove(targets[1], durations[2])
                    .OnStart(() => _transform.localEulerAngles = new Vector3(0, 0, -30f)))
                // イージングをいれるとOnCompleteを発行するタイミングが想定と異なるため別タイミングで制御
                .Join(DOVirtual.DelayedCall(durations[3], () => observer.OnNext(true)))
                .SetEase(Ease.OutCirc);

            yield return null;
        }

        public bool PlaySFXChargeMode(int idx)
        {
            try
            {
                if (playSFXChargeModes.Length <= idx)
                    throw new System.Exception($"範囲外インデックス:{idx}");
                // パワーチャージSEならループ再生
                // パワーチャージフルSE再生
                MainGameManager.Instance.AudioOwner.PlaySFX(playSFXChargeModes[idx], idx == 0 ? true : false);

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool StopSFXChargeMode(int idx)
        {
            try
            {
                if (playSFXChargeModes.Length <= idx)
                    throw new System.Exception($"範囲外インデックス:{idx}");
                // パワーチャージSE停止
                MainGameManager.Instance.AudioOwner.StopSFX(playSFXChargeModes[idx]);

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
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
        /// <summary>
        /// 泡が発生
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool InstanceBubble();
        /// <summary>
        /// 疑似プレイヤーを生成
        /// ※イベント内でカメラの移動で使用
        /// </summary>
        /// <returns>疑似プレイヤーのトランスフォーム</returns>
        public Transform InstanceGhost();
        /// <summary>
        /// 移動アニメーションを再生
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <param name="target">位置</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayMoveAnimation(System.IObserver<bool> observer, Vector3 target);
        /// <summary>
        /// 飛び込みアニメーションを再生
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <param name="targets">位置</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayDiveAnimation(System.IObserver<bool> observer, Vector3[] targets);
        /// <summary>
        /// SFXを再生
        /// チャージ
        /// </summary>
        /// <param name="idx">インデックス</param>
        /// <returns>成功／失敗</returns>
        public bool PlaySFXChargeMode(int idx);
        /// <summary>
        /// SFXを停止
        /// チャージ
        /// </summary>
        /// <param name="idx">インデックス</param>
        /// <returns>成功／失敗</returns>
        public bool StopSFXChargeMode(int idx);
    }
}
