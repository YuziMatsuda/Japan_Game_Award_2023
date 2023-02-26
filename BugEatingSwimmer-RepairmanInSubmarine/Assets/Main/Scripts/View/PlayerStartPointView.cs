using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Common;
using UniRx;

namespace Main.View
{
    /// <summary>
    /// ビュー
    /// プレイヤー生成ポイント
    /// </summary>
    public class PlayerStartPointView : MonoBehaviour
    {
        /// <summary>プレイヤーのプレハブ</summary>
        [SerializeField] private GameObject playerPrefab;
        /// <summary>プレイヤーがインスタンス済みか</summary>
        private readonly BoolReactiveProperty _isInstanced = new BoolReactiveProperty();
        /// <summary>プレイヤーがインスタンス済みか</summary>
        public IReactiveProperty<bool> IsInstanced => _isInstanced;

        /// <summary>
        /// プレイヤーを生成
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool InstancePlayer()
        {
            try
            {
                if (playerPrefab == null)
                    throw new System.Exception("プレイヤーのプレハブを設定して下さい");
                var tForm = transform;
                if (!tForm.parent.CompareTag(ConstTagNames.TAG_NAME_LEVEL))
                    throw new System.Exception($"タグ:[{ConstTagNames.TAG_NAME_LEVEL}]を持つオブジェクトの子オブジェクトではありません");
                var g = Instantiate(playerPrefab, tForm.position, Quaternion.identity, tForm.parent);
                if (g != null)
                    _isInstanced.Value = true;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }
    }
}
