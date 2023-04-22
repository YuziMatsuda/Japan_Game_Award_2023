using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Select.Model
{
    /// <summary>
    /// コード
    /// </summary>
    public class ShadowCode : MonoBehaviour
    {
        /// <summary>コードの向きごとのトランスフォーム配列</summary>
        [SerializeField] private Transform[] codeCells;
        /// <summary>コード（上）トランスフォーム</summary>
        public Transform CodeCellsUp => codeCells[0];
        /// <summary>コード（右）トランスフォーム</summary>
        public Transform CodeCellsRight => codeCells[1];
        /// <summary>コード（下）トランスフォーム</summary>
        public Transform CodeCellsDown => codeCells[2];
        /// <summary>コード（左）トランスフォーム</summary>
        public Transform CodeCellsLeft => codeCells[3];

        private void Reset()
        {
            List<Transform> codeCellList = new List<Transform>();
            var idx = 1;
            codeCellList.Add(transform.GetChild(idx++));
            codeCellList.Add(transform.GetChild(idx++));
            codeCellList.Add(transform.GetChild(idx++));
            codeCellList.Add(transform.GetChild(idx++));
            codeCells = codeCellList.ToArray();
        }
    }
}
