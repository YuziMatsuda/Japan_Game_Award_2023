using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Model
{
    /// <summary>
    /// モデル
    /// ゴールノード
    /// </summary>
    [RequireComponent(typeof(BoxCollider2D))]
    public class GoalNodeModel : AbstractPivotModel
    {
        protected override void Start()
        {
            base.Start();
            _intDirectionModes = GetIntDirectionModes();
        }

        /// <summary>
        /// 方角モードをInt二次元配列へ変換して取得
        /// </summary>
        /// <returns>方角モード（Int二次元配列）</returns>
        private int[][] GetIntDirectionModes()
        {
            try
            {
                int[][] directionModesArray = { new int[3], new int[3], new int[3] };
                directionModesArray[0] = new int[3] { 0, 1, 0 };
                directionModesArray[1] = new int[3] { 1, 1, 1 };
                directionModesArray[2] = new int[3] { 0, 1, 0 };

                return directionModesArray;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }
    }
}
