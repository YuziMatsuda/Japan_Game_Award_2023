using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Main.Common;

namespace Main.View
{
    /// <summary>
    /// ビュー
    /// 始点
    /// </summary>
    public class PivotView : ShadowCodeCellParent, IStartNodeView
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
}
