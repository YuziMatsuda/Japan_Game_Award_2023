using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Select.Model
{
    /// <summary>
    /// モデル
    /// プレイヤー
    /// </summary>
    public class PlayerModel : MonoBehaviour
    {
        /// <summary>ボディのイメージ</summary>
        [SerializeField] private BodyImage bodyImage;
        /// <summary>ボディのイメージ</summary>
        public BodyImage BodyImage => bodyImage;

        private void Reset()
        {
            bodyImage = GetComponentInChildren<BodyImage>();
        }
    }
}
