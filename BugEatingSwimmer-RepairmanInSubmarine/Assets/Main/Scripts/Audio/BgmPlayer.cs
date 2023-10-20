using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Template;
using Main.Common;
using CriWare;
using CRIACLE_BGM.CueSheet_0;

namespace Main.Audio
{
    /// <summary>
    /// BGMのプレイヤー
    /// </summary>
    [RequireComponent(typeof(CriAtomSource))]
    public class BgmPlayer : MonoBehaviour, IBgmPlayer
    {
        /// <summary>CRIアトムソース</summary>
        [SerializeField] private CriAtomSource criAtomSource;

        private void Reset()
        {
            criAtomSource = GetComponent<CriAtomSource>();
        }

        /// <summary>
        /// 指定されたBGMを再生する
        /// </summary>
        /// <param name="clipToPlay">BGM</param>
        public void PlayBGM(Cue clipToPlay)
        {
            PlayAudioSource(clipToPlay);
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
            var common = new MainPresenterCommon();
            // チュートリアルモードの場合はステージ１のBGMを再生する
            var clipToPlay = mainSceneStagesConfs[!common.IsOpeningTutorialMode() ? sysComCash[EnumSystemCommonCash.SceneId] : 1][EnumMainSceneStagesConfig.PlayBgmNames];
            var configMap = tResourcesAccessory.GetSystemConfig(tResourcesAccessory.LoadSaveDatasCSV(ConstResorcesNames.SYSTEM_CONFIG));
            if (!OutPutAudios(configMap[EnumSystemConfig.BGMVolumeIndex], ConstAudioMixerGroupsNames.GROUP_NAME_BGM))
                Debug.LogError($"{ConstAudioMixerGroupsNames.GROUP_NAME_BGM}設定呼び出しの失敗");

            PlayAudioSource((Cue)clipToPlay);
        }

        /// <summary>
        /// 指定されたBGMを再生する
        /// </summary>
        /// <param name="clipToPlay">BGM</param>
        private void PlayAudioSource(Cue clipToPlay)
        {
            try
            {
                criAtomSource.Play((int)clipToPlay);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e);
            }
        }

        /// <summary>
        /// ミキサーへ反映
        /// </summary>
        /// <param name="value">音量の値</param>
        /// <param name="groupsName">オーディオグループ名</param>
        /// <returns>成功／失敗</returns>
        private bool OutPutAudios(float value, string groupsName)
        {
            try
            {
                CriAtom.SetCategoryVolume(groupsName, 1 * (value/10));

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
