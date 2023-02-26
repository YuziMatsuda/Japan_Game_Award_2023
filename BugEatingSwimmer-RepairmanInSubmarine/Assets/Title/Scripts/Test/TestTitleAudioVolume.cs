using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Title.Common;
using Title.Template;
using Title.Audio;

namespace Title.Test
{
    /// <summary>
    /// テスト用
    /// オーディオの音量調整
    /// </summary>
    public class TestTitleAudioVolume : MonoBehaviour
    {
        //[SerializeField] private Slider slider;
        [SerializeField] private TestAudioMode mode;
        private Dictionary<EnumSystemConfig, int> configMap = new Dictionary<EnumSystemConfig, int>();

        private void Start()
        {
            TitleGameManager.Instance.AudioOwner.PlayBGM(ClipToPlayBGM.bgm_title);
            //slider.onValueChanged.AddListener(SetAudioMixer);
            var tTResources = new TitleTemplateResourcesAccessory();
            var datas = tTResources.LoadSaveDatasCSV(ConstResorcesNames.SYSTEM_CONFIG);
            configMap = tTResources.GetSystemConfig(datas);
        }

        public void SetAudioMixer(float value)
        {
            switch (mode)
            {
                case TestAudioMode.BGM:
                    configMap[EnumSystemConfig.BGMVolumeIndex] = (int)value;
                    break;
                case TestAudioMode.SE:
                    configMap[EnumSystemConfig.SEVolumeIndex] = (int)value;
                    break;
                default:
                    break;
            }
            if (!TitleGameManager.Instance.AudioOwner.SetVolume(configMap))
                Debug.LogError("ボリューム調整呼び出しの失敗");
        }

        public void OnClickCancel()
        {
            if (!TitleGameManager.Instance.AudioOwner.ReLoadAudios())
                Debug.LogError("オーディオ設定キャンセル呼び出しの失敗");
        }

        public void OnClickSubmit()
        {
            if (!TitleGameManager.Instance.AudioOwner.SaveAudios(configMap))
                Debug.LogError("オーディオ設定保存呼び出しの失敗");
        }

        public void OnClickSample()
        {
            TitleGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_decided);
        }

        private enum TestAudioMode
        {
            BGM,
            SE,
        }
    }
}
