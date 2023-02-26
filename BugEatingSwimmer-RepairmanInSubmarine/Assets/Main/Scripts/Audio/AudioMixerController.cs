using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Main.Common;
using Main.Template;

namespace Main.Audio
{
    /// <summary>
    /// オーディオミキサー
    /// </summary>
    [RequireComponent(typeof(AudioMixer))]
    public class AudioMixerController : MonoBehaviour, IMainGameManager
    {
        /// <summary>ミキサー</summary>
        [SerializeField] private AudioMixer audioMixer;
        /// <summary>音量調整の間隔</summary>
        [SerializeField] private float volumeSpan = 10f;

        public void OnStart()
        {
            var tMResources = new MainTemplateResourcesAccessory();
            var datas = tMResources.LoadSaveDatasCSV(ConstResorcesNames.SYSTEM_CONFIG);
            var configMap = tMResources.GetSystemConfig(datas);
            if (!OutPutAudios(configMap[EnumSystemConfig.BGMVolumeIndex], ConstAudioMixerGroupsNames.GROUP_NAME_BGM))
                Debug.LogError($"{ConstAudioMixerGroupsNames.GROUP_NAME_BGM}設定呼び出しの失敗");
            if (!OutPutAudios(configMap[EnumSystemConfig.SEVolumeIndex], ConstAudioMixerGroupsNames.GROUP_NAME_SE))
                Debug.LogError($"{ConstAudioMixerGroupsNames.GROUP_NAME_SE}設定呼び出しの失敗");
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
                //x段階補正
                value /= volumeSpan;
                //-80~0に変換
                var volume = Mathf.Clamp(Mathf.Log10(value) * 20f, -80f, 0f);
                //audioMixerに代入
                audioMixer.SetFloat(groupsName, volume);

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
