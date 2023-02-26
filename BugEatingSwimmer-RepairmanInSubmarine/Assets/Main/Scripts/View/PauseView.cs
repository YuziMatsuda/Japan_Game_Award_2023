using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;

namespace Main.View
{
    /// <summary>
    /// ビュー
    /// ポーズ画面
    /// </summary>
    public class PauseView : MonoBehaviour
    {
        /// <summary>閉じるまでの時間</summary>
        [SerializeField] private float closedTime = .5f;

        /// <summary>
        /// フェードのDOTweenアニメーション再生
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <param name="state">ステータス</param>
        /// <returns>成功／失敗</returns>
        public IEnumerator PlayCloseAnimation(System.IObserver<bool> observer)
        {
            DOVirtual.DelayedCall(closedTime, () =>
            {
                observer.OnNext(true);
            });
            yield return null;
        }
    }
}
