using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Main.Test
{
    /// <summary>
    /// 疑似的にサクセス状態にする
    /// </summary>
    public class TestSuccess : MonoBehaviour
    {
        /// <summary>押下検知</summary>
        private readonly BoolReactiveProperty _isClicked = new BoolReactiveProperty();
        /// <summary>押下検知</summary>
        public IReactiveProperty<bool> IsClicked => _isClicked;

        public void OnClick()
        {
            _isClicked.Value = true;
        }
    }
}
