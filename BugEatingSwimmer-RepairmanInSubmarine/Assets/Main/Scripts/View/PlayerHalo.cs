using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// プレイヤーのハロー
    /// </summary>
    public class PlayerHalo : ShadowCodeCellParent, IPlayerHalo
    {
        public bool ChangeChargeMode(int idx, bool enabled)
        {
            throw new System.NotImplementedException();
        }

        public bool SetHaloEnabled(bool enabled)
        {
            try
            {
                Behaviour halo = (Behaviour)GetComponent("Halo");
                halo.enabled = enabled;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }
    }

    /// <summary>
    /// プレイヤーのハロー
    /// インターフェース
    /// </summary>
    public interface IPlayerHalo
    {
        /// <summary>
        /// ハローの有効をセット
        /// </summary>
        /// <param name="enabled">有効状態</param>
        /// <returns>成功／失敗</returns>
        public bool SetHaloEnabled(bool enabled);

        /// <summary>
        /// チャージ状態を切り替え
        /// </summary>
        /// <param name="idx">インデックス</param>
        /// <param name="enabled">有効状態</param>
        /// <returns>成功／失敗</returns>
        public bool ChangeChargeMode(int idx, bool enabled);
    }
}
