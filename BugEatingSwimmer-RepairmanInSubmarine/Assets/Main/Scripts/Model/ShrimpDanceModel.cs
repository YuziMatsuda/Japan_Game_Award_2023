using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Common;
using System.Linq;
using UniRx;

namespace Main.Model
{
    /// <summary>
    /// モデル
    /// エビダンス
    /// </summary>
    [RequireComponent(typeof(CircleCollider2D))]
    public class ShrimpDanceModel : MonoBehaviour, IShrimpDanceModel
    {
        /// <summary>接触対象のオブジェクトタグ</summary>
        [SerializeField] protected string[] tags = { ConstTagNames.TAG_NAME_DUSTCONNECTSIGNAL };
        /// <summary>アサインされたか（ローカルカウント）</summary>
        private readonly BoolReactiveProperty _isAssignedLocal = new BoolReactiveProperty();
        /// <summary>アサインされたか（ローカルカウント）</summary>
        public IReactiveProperty<bool> IsAssignedLocal => _isAssignedLocal;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (0 < tags.Where(q => collision.CompareTag(q)).Select(q => q).ToArray().Length)
            {
                _isAssignedLocal.Value = true;
            }
        }

        public bool ResetIsAssigned()
        {
            try
            {
                _isAssignedLocal.Value = false;

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
    /// エビダンス
    /// インターフェース
    /// </summary>
    public interface IShrimpDanceModel
    {
        /// <summary>
        /// アサイン情報をリセット
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool ResetIsAssigned();
    }
}
