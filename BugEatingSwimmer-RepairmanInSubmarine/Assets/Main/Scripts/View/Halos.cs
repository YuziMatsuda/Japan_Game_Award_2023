using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// ハローグループ
    /// </summary>
    public class Halos : ShadowCodeCellParent, IPlayerHalo
    {
        /// <summary>プレイヤーのハロー配列</summary>
        [SerializeField] private PlayerHalo[] playerHalos;
        /// <summary>プレイヤーのハロー配列</summary>
        public PlayerHalo[] PlayerHalos => playerHalos;

        public bool ChangeChargeMode(int idx, bool enabled)
        {
            try
            {
                if (playerHalos.Length <= idx)
                    throw new System.Exception($"範囲外インデックス:{idx}");
                if (!playerHalos[idx].SetHaloEnabled(enabled))
                    throw new System.Exception("ハローの有効をセット呼び出しの失敗");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetHaloEnabled(bool enabled)
        {
            throw new System.NotImplementedException();
        }

        private void Reset()
        {
            if (_transform == null)
                _transform = transform;
            List<PlayerHalo> playerHaloList = new List<PlayerHalo>();
            foreach (Transform item in _transform)
            {
                playerHaloList.Add(item.GetComponent<PlayerHalo>());
            }
            playerHalos = playerHaloList.ToArray();
        }

        private void Start()
        {
            if (_transform == null)
                _transform = transform;
            foreach (var item in playerHalos)
            {
                item.SetHaloEnabled(false);
            }
        }
    }
}
