using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// ビュー
    /// クリア画面
    /// </summary>
    public class ClearView : MonoBehaviour
    {
        private void OnEnable()
        {
            Time.timeScale = 0f;
        }

        private void OnDisable()
        {
            Time.timeScale = 1f;
        }
    }
}
