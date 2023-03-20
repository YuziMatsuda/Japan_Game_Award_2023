using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Main.View
{
    /// <summary>
    /// ビュー
    /// スタートノード
    /// </summary>
    public class StartNodeView : MonoBehaviour, IStartNodeView
    {
        /// <summary>信号発生アニメーション時間</summary>
        [SerializeField] private float postDuration = .5f;
        /// <summary>信号発生アニメーション時間</summary>
        private bool _isRuning;

        public IEnumerator PlayLightAnimation(System.IObserver<bool> observer)
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

    /// <summary>
    /// ビュー
    /// スタートノード
    /// インターフェース
    /// </summary>
    public interface IStartNodeView
    {
        /// <summary>
        /// 信号発生アニメーションの再生
        /// </summary>
        /// <param name="observer">オブサーバー</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayLightAnimation(System.IObserver<bool> observer);
    }
}
