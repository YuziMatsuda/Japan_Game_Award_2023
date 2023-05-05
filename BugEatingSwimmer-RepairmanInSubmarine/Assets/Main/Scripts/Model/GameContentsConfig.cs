using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Main.Model
{
    /// <summary>
    /// 次のステージに進む／もう一度遊ぶ／他のステージを選ぶ
    /// 設定
    /// </summary>
    public class GameContentsConfig : MonoBehaviour
    {
        /// <summary>ボタンナビゲーション配列</summary>
        [SerializeField] private Navigation[] navigations;
        /// <summary>ボタンナビゲーション配列</summary>
        public Navigation[] Navigations => navigations;
    }
}
