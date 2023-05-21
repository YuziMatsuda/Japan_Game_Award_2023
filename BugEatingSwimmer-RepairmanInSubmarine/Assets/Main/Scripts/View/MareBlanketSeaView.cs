using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Main.View
{
    /// <summary>
    /// メイン画面の背景
    /// ビュー
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class MareBlanketSeaView : MonoBehaviour, IMareBlanketSeaView
    {
        /// <summary>イメージ</summary>
        [SerializeField] private Image image;
        /// <summary>イメージ設定</summary>
        [SerializeField] private ImageConfig imageConfig;

        public bool SetBackgroundToTutorial()
        {
            try
            {
                image.sprite = imageConfig.Sprites[0];

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        private void Reset()
        {
            image = GetComponent<Image>();
            imageConfig = GetComponent<ImageConfig>();
        }
    }

    /// <summary>
    /// メイン画面の背景
    /// ビュー
    /// インターフェース
    /// </summary>
    public interface IMareBlanketSeaView
    {
        /// <summary>
        /// チュートリアルモードの背景をセット
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool SetBackgroundToTutorial();
    }
}
