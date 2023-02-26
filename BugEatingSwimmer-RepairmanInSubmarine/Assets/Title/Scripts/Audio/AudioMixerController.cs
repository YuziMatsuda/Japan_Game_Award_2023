using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Title.Common;
using UnityEngine.Audio;
using Title.Template;

namespace Title.Audio
{
    /// <summary>
    /// オーディオミキサー
    /// </summary>
    [RequireComponent(typeof(AudioMixer))]
    public class AudioMixerController : MonoBehaviour, ITitleGameManager
    {
        /// <summary>ミキサー</summary>
        [SerializeField] private AudioMixer audioMixer;
        /// <summary>音量調整の間隔</summary>
        [SerializeField] private float volumeSpan = 10f;

        public void OnStart()
        {
            var tTResources = new TitleTemplateResourcesAccessory();
            var datas = tTResources.LoadSaveDatasCSV(ConstResorcesNames.SYSTEM_CONFIG);
            var configMap = tTResources.GetSystemConfig(datas);
            if (!OutPutAudios(configMap[EnumSystemConfig.BGMVolumeIndex], ConstAudioMixerGroupsNames.GROUP_NAME_BGM))
                Debug.LogError($"{ConstAudioMixerGroupsNames.GROUP_NAME_BGM}設定呼び出しの失敗");
            if (!OutPutAudios(configMap[EnumSystemConfig.SEVolumeIndex], ConstAudioMixerGroupsNames.GROUP_NAME_SE))
                Debug.LogError($"{ConstAudioMixerGroupsNames.GROUP_NAME_SE}設定呼び出しの失敗");
        }

        /// <summary>
        /// ボリュームをセット
        /// </summary>
        /// <param name="configMap">システム設定</param>
        /// <returns>成功／失敗</returns>
        public bool SetVolume(Dictionary<EnumSystemConfig, int> configMap)
        {
            try
            {
                if (!OutPutAudios(configMap[EnumSystemConfig.BGMVolumeIndex], ConstAudioMixerGroupsNames.GROUP_NAME_BGM))
                    Debug.LogError($"{ConstAudioMixerGroupsNames.GROUP_NAME_BGM}設定呼び出しの失敗");
                if (!OutPutAudios(configMap[EnumSystemConfig.SEVolumeIndex], ConstAudioMixerGroupsNames.GROUP_NAME_SE))
                    Debug.LogError($"{ConstAudioMixerGroupsNames.GROUP_NAME_SE}設定呼び出しの失敗");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// オーディオ情報をリロード
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool ReLoadAudios()
        {
            try
            {
                var tTResources = new TitleTemplateResourcesAccessory();
                var datas = tTResources.LoadSaveDatasCSV(ConstResorcesNames.SYSTEM_CONFIG);
                var configMap = tTResources.GetSystemConfig(datas);
                if (!OutPutAudios(configMap[EnumSystemConfig.BGMVolumeIndex], ConstAudioMixerGroupsNames.GROUP_NAME_BGM))
                    Debug.LogError($"{ConstAudioMixerGroupsNames.GROUP_NAME_BGM}設定呼び出しの失敗");
                if (!OutPutAudios(configMap[EnumSystemConfig.SEVolumeIndex], ConstAudioMixerGroupsNames.GROUP_NAME_SE))
                    Debug.LogError($"{ConstAudioMixerGroupsNames.GROUP_NAME_SE}設定呼び出しの失敗");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// オーディオ情報を保存
        /// </summary>
        /// <param name="configMap">システム設定</param>
        /// <returns>成功／失敗</returns>
        public bool SaveAudios(Dictionary<EnumSystemConfig, int> configMap)
        {
            try
            {
                var tTResources = new TitleTemplateResourcesAccessory();
                if (!tTResources.SaveDatasCSVOfSystemConfig(ConstResorcesNames.SYSTEM_CONFIG, configMap))
                    Debug.LogError("CSV保存呼び出しの失敗");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
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
