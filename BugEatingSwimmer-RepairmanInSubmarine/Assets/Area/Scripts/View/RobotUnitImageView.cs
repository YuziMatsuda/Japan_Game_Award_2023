using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Area.View 
{
    ///<summary>
    /// 各ユニットの制御
    /// </summary>
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(RobotUnitImageConfig))]
    public class RobotUnitImageView : MonoBehaviour, IRobotUnitImageView
    {
        /// <summary>各ユニットの制御設定</summary>
        [SerializeField] private RobotUnitImageConfig robotUnitImageConfig;
        /// <summary>画像</summary>
        [SerializeField] private Image image;
        /// <summary>非選択状態のカラー</summary>
        [SerializeField] private Color disabledColor = new Color(.4f, .4f, .4f);
        /// <summary>選択状態のカラー</summary>
        [SerializeField] private Color enabledColor = Color.white;

        private void Reset()
        {
            robotUnitImageConfig = GetComponent<RobotUnitImageConfig>();
            image = GetComponent<Image>();
        }

        public void SetPositionAndEulerAngle()
        {
            Transform myTransform = this.transform;
            myTransform.position = robotUnitImageConfig.Pos;
            myTransform.eulerAngles = robotUnitImageConfig.Rotate;
        }

        public void SetImageAltha(bool visibled)
        {
            var color = image.color;
            var colorwork = new Color(color.r, color.g, color.b, visibled ? 1.0f : 0.0f);
            image.color = colorwork;
        }

        public IEnumerator PlayFadeAnimation(System.IObserver<bool> observer, bool visibled)
        {
            image.DOFade(endValue: visibled ? 1.0f : 0.0f, robotUnitImageConfig.Duration)
                .SetUpdate(true)
                .OnComplete(() => observer.OnNext(true));
            yield return null;
        }

        public IEnumerator PlayAnimationMove(System.IObserver<bool> observer)
        {
            Transform myTransform = this.transform;
            myTransform.DOMove(robotUnitImageConfig.Pos, robotUnitImageConfig.Duration)
                .SetUpdate(true)
                .OnComplete(() => observer.OnNext(true));
            myTransform.eulerAngles = robotUnitImageConfig.Rotate;

            yield return null;
        }

        public void RendererDisableMode()
        {
            image.color = disabledColor;
        }

        public IEnumerator PlayRenderEnable(System.IObserver<bool> observer)
        {
            image.DOColor(enabledColor, robotUnitImageConfig.Duration)
                .From(disabledColor)
                .OnComplete(() => observer.OnNext(true));

            yield return null;
        }

        public void RendererEnableMode()
        {
            image.color = enabledColor;
        }
    }

    ///<summary>
    /// 各ユニットの制御
    /// インターフェース
    /// </summary>
    public interface IRobotUnitImageView
    {
        /// <summary>
        /// 位置と角度をセット
        /// </summary>
        public void SetPositionAndEulerAngle();
        /// <summary>
        /// アルファ値をセット
        /// </summary>
        /// <param name="visibled">表示／非表示</param>
        public void SetImageAltha(bool visibled);
        /// <summary>
        /// 非選択状態
        /// </summary>
        public void RendererDisableMode();
        /// <summary>
        /// 選択状態
        /// </summary>
        public void RendererEnableMode();
        /// <summary>
        /// 選択状態解放演出
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayRenderEnable(System.IObserver<bool> observer);
        /// <summary>
        /// フェードのDOTweenアニメーション再生
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <returns>成功／失敗</returns>
        public IEnumerator PlayFadeAnimation(System.IObserver<bool> observer, bool visibled);
        /// <summary>
        /// 位置を動かすアニメーションを再生
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayAnimationMove(System.IObserver<bool> observer);
    }
}
