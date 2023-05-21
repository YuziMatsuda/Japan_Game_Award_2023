using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// ヒトデゲージの設定
    /// </summary>
    public class SeastarGageConfig : MonoBehaviour
    {
        /// <summary>スプライト配列</summary>
        [SerializeField] private Sprite[] sprites;
        /// <summary>スプライト配列</summary>
        public Sprite[] Sprites => sprites;
    }
}
