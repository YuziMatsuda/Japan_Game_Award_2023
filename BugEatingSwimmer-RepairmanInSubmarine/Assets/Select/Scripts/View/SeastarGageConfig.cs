using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Select.Common;

namespace Select.View
{
    /// <summary>
    /// ヒトデゲージ
    /// 設定
    /// </summary>
    public class SeastarGageConfig : MonoBehaviour
    {
        /// <summary>エリアID</summary>
        [SerializeField] private EnumUnitID enumUnitID;
        /// <summary>エリアID</summary>
        public EnumUnitID EnumUnitID => enumUnitID;
        /// <summary>ゲージオープンの移動幅（左 or 右）</summary>
        [SerializeField] private float openedAnchorPosX = 170.0f;
        /// <summary>ゲージオープンの移動幅（左 or 右）</summary>
        public float OpenedAnchorPosX => openedAnchorPosX;
    }
}
