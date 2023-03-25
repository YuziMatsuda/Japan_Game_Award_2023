using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Main.View
{
    /// <summary>
    /// ビュー
    /// 始点
    /// </summary>
    public class PivotView : MonoBehaviour, IStartNodeView
    {
        /// <summary>信号発生アニメーション時間</summary>
        [SerializeField] private float postDuration = .5f;
        /// <summary>信号発生アニメーション時間</summary>
        private bool _isRuning;

        public IEnumerator PlayLightAnimation(IObserver<bool> observer)
        {
            if (!_isRuning)
            {
                _isRuning = true;

                DOVirtual.DelayedCall(postDuration, () =>
                {
                    // T.B.D 信号発生演出
                    Debug.Log($"T.B.D 信号発生演出:{name}");
                }).OnComplete(() =>
                {
                    observer.OnNext(true);
                    _isRuning = false;
                });
            }

            yield return null;
        }

        public bool SetIsRuning(bool isRuning)
        {
            try
            {
                _isRuning = isRuning;

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
