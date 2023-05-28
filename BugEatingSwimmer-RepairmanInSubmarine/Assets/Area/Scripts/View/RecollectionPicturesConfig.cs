using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Area.View
{
    /// <summary>
    /// 回想シーンのイメージを表示
    /// 設定
    /// </summary>
    public class RecollectionPicturesConfig : MonoBehaviour
    {
        /// <summary>画像配列</summary>
        [SerializeField] private Sprite[] sprites;
        /// <summary>画像配列</summary>
        public Sprite[] Sprites => sprites;
        /// <summary>アニメーション終了時間配列</summary>
        [SerializeField] private float[] duration;
        /// <summary>アニメーション終了時間配列</summary>
        public float[] Duration => duration;
    }
}
