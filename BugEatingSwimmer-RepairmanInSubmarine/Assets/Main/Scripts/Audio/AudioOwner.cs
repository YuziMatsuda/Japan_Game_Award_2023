using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Common;

namespace Main.Audio
{
    /// <summary>
    /// オーディオのオーナー
    /// </summary>
    public class AudioOwner : MonoBehaviour, IMainGameManager, ISfxPlayer, IBgmPlayer
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
            sfxPlayer.PlaySFX(clipToPlay, false);
        }

        public void PlaySFX(ClipToPlay clipToPlay, bool isLoopmode)
        {
            sfxPlayer.PlaySFX(clipToPlay, isLoopmode);
        }

        public void PlayBGM(ClipToPlayBGM clipToPlay)
        {
            bgmPlayer.PlayBGM(clipToPlay);
        }

        public void StopSFX(ClipToPlay clipToPlay)
        {
            sfxPlayer.StopSFX(clipToPlay);
        }

        /// <summary>
        /// BGMを再生
        /// ※ステージ開始時に呼ばれる
        /// </summary>
        public void OnStartAndPlayBGM()
        {
            bgmPlayer.OnStartAndPlayBGM();
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
        /// <summary>
        /// 指定されたSEを再生する
        /// </summary>
        /// <param name="clipToPlay">SE</param>
        /// <param name="isLoopmode">ループモード</param>
        public void PlaySFX(ClipToPlay clipToPlay, bool isLoopmode) { }
        /// <summary>
        /// 指定されたSEを停止する
        /// </summary>
        /// <param name="clipToPlay">SE</param>
        public void StopSFX(ClipToPlay clipToPlay) { }
    }

    /// <summary>
    /// SFX・MEオーディオクリップリストのインデックス
    /// </summary>
    public enum ClipToPlay
    {
        /// <summary>ゲームクリア</summary>
        me_game_clear,
        /// <summary>キャンセル</summary>
        se_cancel,
        /// <summary>項目の決定</summary>
        se_decided,
        /// <summary>遊び方表_開く音</summary>
        se_play_open,
        /// <summary>圧死音</summary>
        se_player_dead,
        /// <summary>落下音</summary>
        se_player_fall,
        /// <summary>ジャンプ</summary>
        se_player_jump,
        /// <summary>リトライ</summary>
        se_retry,
        /// <summary>ステージセレクト</summary>
        se_select,
        /// <summary>スイマーが泳いでいる時、泡が出る音</summary>
        se_swim,
        /// <summary>スイマーがコードを突いた時に発生する音</summary>
        se_code_normal,
        /// <summary>信号のエフェクトがエラーコードに到達した時に発生する音</summary>
        se_code_error,
        /// <summary>信号がコードの上を走るように流れる音</summary>
        se_signal,
        /// <summary>スイマーがパワーシェルを消費してエネルギーをチャージする音</summary>
        se_energy_store,
        /// <summary>スイマーがサンショウコードを壊す為にエネルギーを解放する時の音</summary>
        se_energy_release,
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
    }

    /// <summary>
    /// BGMオーディオクリップリストのインデックス
    /// </summary>
    public enum ClipToPlayBGM
    {
        /// <summary>メインBGM（エリア１～４）</summary>
        bgm_main,
        /// <summary>深海BGM（エリア５）</summary>
        bgm_shinkai,
    }
}
