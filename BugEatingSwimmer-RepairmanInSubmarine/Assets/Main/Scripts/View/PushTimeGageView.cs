using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Main.Common;
using UniRx;

namespace Main.View
{
    /// <summary>
    /// ビュー
    /// ショートカットキーの押下中ゲージ
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class PushTimeGageView : MonoBehaviour
    {
        /// <summary>コマンドを実行するまで押下し続ける時間</summary>
        [SerializeField] private float pushTimeLimit = 1.5f;
        /// <summary>ゲージ用イメージ</summary>
        [SerializeField] private Image image;
        /// <summary>ゲージ残りのパラメータ</summary>
        private readonly FloatReactiveProperty _floatReactiveProperty = new FloatReactiveProperty();
        /// <summary>ゲージ残りのパラメータ</summary>
        public IReactiveProperty<float> FloatReactiveProperty => _floatReactiveProperty;

        private void Reset()
        {
            image = GetComponent<Image>();
        }

        /// <summary>
        /// 対象の項目を有効にして、プッシュゲージを表示する
        /// 渡された押下時間に応じてゲージを変化させる値を返す
        /// </summary>
        /// <param name="time">押下時間</param>
        /// <returns>成功／失敗</returns>
        public bool EnabledPushGageAndGetFillAmount(float time)
        {
            try
            {
                _floatReactiveProperty.Value = time / pushTimeLimit;
                image.fillAmount = _floatReactiveProperty.Value;

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
