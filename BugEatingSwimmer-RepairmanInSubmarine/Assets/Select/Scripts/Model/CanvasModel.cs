using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Select.Model
{
    /// <summary>
    /// キャンバスのモデル
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    public class CanvasModel : MonoBehaviour
    {
        /// <summary>キャンバス</summary>
        [SerializeField] private Canvas canvas;
        /// <summary>キャンバススケールファクター</summary>
        private readonly FloatReactiveProperty _scaleFactor = new FloatReactiveProperty();
        /// <summary>キャンバススケールファクター</summary>
        public IReactiveProperty<float> ScaleFactor => _scaleFactor;

        private void Reset()
        {
            canvas = GetComponent<Canvas>();
        }

        private void Start()
        {
            canvas.ObserveEveryValueChanged(x => x.scaleFactor)
                .Subscribe(x => _scaleFactor.Value = x);
        }
    }
}
