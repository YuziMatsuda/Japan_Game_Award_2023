using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriWare;
using CRIACLE_BGM.CueSheet_1;
using Area.Common;
using Area.Template;

namespace Area.Audio
{
    /// <summary>
    /// BGMのプレイヤー
    /// </summary>
    [RequireComponent(typeof(CriAtomSource))]
    public class BgmPlayer : MonoBehaviour, IBgmPlayer, IAreaGameManager
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
            try
            {
                criAtomSource.Play((int)clipToPlay);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e);
            }
        }

        public void StopBGM()
        {
            criAtomSource.Stop();
        }

        public void OnStart()
        {
            var tResourcesAccessory = new AreaTemplateResourcesAccessory();
            var configMap = tResourcesAccessory.GetSystemConfig(tResourcesAccessory.LoadSaveDatasCSV(ConstResorcesNames.SYSTEM_CONFIG));
            if (!OutPutAudios(configMap[EnumSystemConfig.BGMVolumeIndex], ConstAudioMixerGroupsNames.GROUP_NAME_BGM))
                Debug.LogError($"{ConstAudioMixerGroupsNames.GROUP_NAME_BGM}設定呼び出しの失敗");
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
                CriAtom.SetCategoryVolume(groupsName, 1 * (value / 10));

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
