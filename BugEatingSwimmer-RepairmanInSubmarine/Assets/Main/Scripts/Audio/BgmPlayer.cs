using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Template;
using Main.Common;

namespace Main.Audio
{
    /// <summary>
    /// BGMのプレイヤー
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class BgmPlayer : MonoBehaviour, IBgmPlayer
    {
        /// <summary>オーディオソース</summary>
        [SerializeField] private AudioSource audioSource;
        /// <summary>効果音のクリップ</summary>
        [SerializeField] private AudioClip[] clip;

        private void Reset()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.loop = true;
        }

        /// <summary>
        /// 指定されたBGMを再生する
        /// </summary>
        /// <param name="clipToPlay">BGM</param>
        public void PlayBGM(ClipToPlayBGM clipToPlay)
        {
            try
            {
                if ((int)clipToPlay <= (clip.Length - 1))
                {
                    audioSource.clip = clip[(int)clipToPlay];

                    // BGMを再生
                    audioSource.Play();
                }
                else
                    throw new System.Exception($"対象のファイルが見つかりません:[{clipToPlay}]");
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e);
            }
        }

        /// <summary>
        /// BGMを再生
        /// ※ステージ開始時に呼ばれる
        /// </summary>
        public void OnStartAndPlayBGM()
        {
            var tResourcesAccessory = new MainTemplateResourcesAccessory();
            // ステージIDの取得
            var sysComCashResources = tResourcesAccessory.LoadSaveDatasCSV(ConstResorcesNames.SYSTEM_COMMON_CASH);
            var sysComCash = tResourcesAccessory.GetSystemCommonCash(sysComCashResources);
            // ステージ共通設定の取得
            var mainSceneStagesConfResources = tResourcesAccessory.LoadSaveDatasCSV(ConstResorcesNames.MAIN_SCENE_STAGES_CONFIG);
            var mainSceneStagesConfs = tResourcesAccessory.GetMainSceneStagesConfig(mainSceneStagesConfResources);
            var clipToPlay = mainSceneStagesConfs[sysComCash[EnumSystemCommonCash.SceneId]][EnumMainSceneStagesConfig.PlayBgmNames];

            if (clipToPlay <= (clip.Length - 1))
            {
                audioSource.clip = clip[clipToPlay];

                // BGMを再生
                audioSource.Play();
            }
        }
    }
}
