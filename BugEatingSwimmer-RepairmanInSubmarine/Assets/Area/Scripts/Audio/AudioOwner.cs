using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Area.Common;

namespace Area.Audio
{
    /// <summary>
    /// オーディオのオーナー
    /// </summary>
    public class AudioOwner : MonoBehaviour, IAreaGameManager, ISfxPlayer, IBgmPlayer
    {
        /// <summary>SEのプレイヤー</summary>
        [SerializeField] private SfxPlayer sfxPlayer;
        /// <summary>BGMのプレイヤー</summary>
        [SerializeField] private BgmPlayer bgmPlayer;
        /// <summary>オーディオミキサー</summary>
        [SerializeField] private AudioMixerController audioMixer;

        private void Reset()
        {
            sfxPlayer = GameObject.Find("SfxPlayer").GetComponent<SfxPlayer>();
            bgmPlayer = GameObject.Find("BgmPlayer").GetComponent<BgmPlayer>();
            audioMixer = GameObject.Find("AudioMixer").GetComponent<AudioMixerController>();
        }

        public void OnStart()
        {
            sfxPlayer.OnStart();
            audioMixer.OnStart();
        }

        public void PlaySFX(ClipToPlay clipToPlay)
        {
            sfxPlayer.PlaySFX(clipToPlay);
        }

        public void PlayBGM(ClipToPlayBGM clipToPlay)
        {
            bgmPlayer.PlayBGM(clipToPlay);
        }

        public void StopBGM()
        {
            bgmPlayer.StopBGM();
        }
    }

    /// <summary>
    /// SE・ME用インターフェース
    /// </summary>
    public interface ISfxPlayer
    {
        /// <summary>
        /// 指定されたSEを再生する
        /// </summary>
        /// <param name="clipToPlay">SE</param>
        public void PlaySFX(ClipToPlay clipToPlay) { }
    }

    /// <summary>
    /// SFX・MEオーディオクリップリストのインデックス
    /// </summary>
    public enum ClipToPlay
    {
        /// <summary>キャンセル</summary>
        se_cancel,
        /// <summary>項目の決定</summary>
        se_decided,
        /// <summary>ステージセレクト</summary>
        se_select,
        /// <summary>プレイヤーが泳ぐ音</summary>
        se_swim,
        /// <summary>コード回転</summary>
        se_code_normal,
    }

    /// <summary>
    /// BGM用インターフェース
    /// </summary>
    public interface IBgmPlayer
    {
        /// <summary>
        /// 指定されたBGMを再生する
        /// </summary>
        /// <param name="clipToPlay">BGM</param>
        public void PlayBGM(ClipToPlayBGM clipToPlay) { }
        /// <summary>
        /// 指定されたBGMを停止する
        /// </summary>
        public void StopBGM() { }
    }

    /// <summary>
    /// BGMオーディオクリップリストのインデックス
    /// </summary>
    public enum ClipToPlayBGM
    {
        /// <summary>回想</summary>
        bgm_kaiso,
        /// <summary>セレクト</summary>
        bgm_select,
        /// <summary>エンディング</summary>
        bgm_ending,
    }
}