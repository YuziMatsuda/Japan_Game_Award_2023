using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Main.Common;
using Main.Model;

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
        /// <summary>設定</summary>
        [SerializeField] private PivotConfig pivotConfig;

        private void Reset()
        {
            pivotConfig = GetComponent<PivotConfig>();
        }

        private void Start()
        {
            if (pivotConfig.EnumAtomicMode.Equals(EnumAtomicMode.Molecules) &&
                0 < pivotConfig.ErrorDirections.Length)
            {
                // T.B.D 泡出す？
            }
        }

        public IEnumerator PlayLightAnimation(System.IObserver<bool> observer)
        {
            throw new System.NotImplementedException();
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
