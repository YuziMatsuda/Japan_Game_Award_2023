using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Area.View 
{
    [RequireComponent(typeof(Image))]
    ///<summary>
    ///各ユニットの制御
    /// </summary>
    public class Robot_UnitImage : MonoBehaviour
    {
        [SerializeField] private Vector3 pos;
        [SerializeField] private Vector3 rotate;
        [SerializeField] private float duration;

        public void SetPositionAndEulerAngle()
        {
            Transform myTransform = this.transform;
            myTransform.position = pos;
            myTransform.eulerAngles = rotate;
        }

        public void SetImageAltha(bool visibled)
        {
            var color = GetComponent<Image>().color;
            var colorwork = new Color(color.r, color.g, color.b, visibled ? 1.0f : 0.0f);
            GetComponent<Image>().color = colorwork;
        }

        public void PlayAnimationMove()
        {
            Transform myTransform = this.transform;
            myTransform.DOMove(pos, duration);
            //myTransform.position = pos;
            myTransform.eulerAngles = rotate;
        }

        /// <summary>
        /// フェードのDOTweenアニメーション再生
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <returns>成功／失敗</returns>
        public IEnumerator PlayFadeAnimation(System.IObserver<bool> observer, bool visibled)
        {
            GetComponent<Image>().DOFade(endValue: visibled ? 1.0f : 0.0f, duration)
                .SetUpdate(true)
                .OnComplete(() => observer.OnNext(true));
            yield return null;
        }
    }
}
