using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Title.Common
{
    /// <summary>
    /// カーソル表示制御
    /// </summary>
    public class CursorVisible : MonoBehaviour, ITitleGameManager
    {
        /// <summary>デフォルト表示</summary>
        [SerializeField] private bool defaultVisible = false;

        public void OnStart()
        {
            Cursor.visible = defaultVisible;
        }
    }
}
