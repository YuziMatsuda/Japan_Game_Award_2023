using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Select.Common
{
    /// <summary>
    /// ビュー
    /// 親
    /// インターフェース
    /// </summary>
    public interface ISelectContentsViewParent
    {
        /// <summary>
        /// 選択中フレームを表示アニメーション
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayRenderEnableBackgroundFrame(System.IObserver<bool> observer);
        /// <summary>
        /// 選択中フレームを非表示アニメーション
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayRenderDisableBackgroundFrame(System.IObserver<bool> observer);
        /// <summary>
        /// 選択中フレームを表示
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <returns>コルーチン</returns>
        public bool SetRenderEnableBackgroundFrame();
        /// <summary>
        /// 選択中フレームを非表示
        /// </summary>
        /// <returns>コルーチン</returns>
        public bool SetRenderDisableBackgroundFrame();
    }
}
