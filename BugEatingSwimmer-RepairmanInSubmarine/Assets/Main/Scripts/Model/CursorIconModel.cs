using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Model
{
    /// <summary>
    /// モデル
    /// カーソル
    /// </summary>
    public class CursorIconModel : MonoBehaviour
    {
        /// <summary>ルール貝の設定</summary>
        [SerializeField] private RuleShellfishConfig ruleShellfishConfig;
        /// <summary>ルール貝の設定</summary>
        public RuleShellfishConfig RuleShellfishConfig => ruleShellfishConfig;

        private void Reset()
        {
            ruleShellfishConfig = GetComponent<RuleShellfishConfig>();
        }
    }
}
