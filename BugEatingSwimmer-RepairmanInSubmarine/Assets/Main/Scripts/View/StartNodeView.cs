using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Main.Common;
using System;

namespace Main.View
{
    /// <summary>
    /// ビュー
    /// スタートノード
    /// </summary>
    public class StartNodeView : ShadowCodeCellParent, IStartNodeView
    {
        /// <summary>信号発生アニメーション時間</summary>
        private bool _isRuning;

        public IEnumerator PlayLightAnimation(IObserver<bool> observer)
        {
            throw new NotImplementedException();
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
