using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Select.Common
{
    /// <summary>
    /// カーソル表示制御
    /// </summary>
    public class CursorVisible : MonoBehaviour, ISelectGameManager
    {
        /// <summary>デフォルト表示</summary>
        [SerializeField] private bool defaultVisible = false;

        public void OnStart()
        {
            Cursor.visible = defaultVisible;
        }
    }
}
