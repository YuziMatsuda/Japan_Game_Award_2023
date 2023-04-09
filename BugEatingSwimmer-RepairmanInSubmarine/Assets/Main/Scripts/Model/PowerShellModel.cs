using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Main.Common;
using System.Linq;

namespace Main.Model
{
    /// <summary>
    /// モデル
    /// パワーシェル
    /// </summary>
    [RequireComponent(typeof(CircleCollider2D))]
    public class PowerShellModel : MonoBehaviour, IPowerShellModel
    {
        /// <summary>衝突されたか</summary>
        private readonly BoolReactiveProperty _isCollisioned = new BoolReactiveProperty();
        /// <summary>衝突されたか</summary>
        public IReactiveProperty<bool> IsCollisioned => _isCollisioned;
        /// <summary>接触対象のオブジェクトタグ</summary>
        [SerializeField] protected string[] tags = { ConstTagNames.TAG_NAME_PLAYER };

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // プレイヤーのみ衝突かつ、プレイヤーがパワー状態ではない
            if (!_isCollisioned.Value &&
                0 < tags.Where(q => collision.CompareTag(q)).Select(q => q).ToArray().Length &&
                !collision.GetComponent<PlayerModel>().IsPower.Value)
            {
                if (!collision.GetComponent<PlayerModel>().SetIsPower(true))
                    Debug.LogError("パワー状態をセット呼び出しの失敗");
                _isCollisioned.Value = true;
            }
        }

        public bool DestroyItem()
        {
            try
            {
                Destroy(gameObject);

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
    /// モデル
    /// パワーシェル
    /// インターフェース
    /// </summary>
    public interface IPowerShellModel
    {
        /// <summary>
        /// アイテムを削除
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool DestroyItem();
    }
}
