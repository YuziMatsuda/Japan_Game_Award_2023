using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Common;
using UniRx;
using Main.Template;
using System.Linq;

namespace Main.Model
{
    /// <summary>
    /// モデル
    /// ヒトデ
    /// </summary>
    [RequireComponent(typeof(SeastarConfig))]
    [RequireComponent(typeof(CircleCollider2D))]
    public class SeastarModel : MonoBehaviour, ISeastarModel
    {
        /// <summary>アサインされたか</summary>
        private readonly BoolReactiveProperty _isAssigned = new BoolReactiveProperty();
        /// <summary>アサインされたか</summary>
        public IReactiveProperty<bool> IsAssigned => _isAssigned;
        /// <summary>アサインされたか（ローカルカウント）</summary>
        private readonly BoolReactiveProperty _isAssignedLocal = new BoolReactiveProperty();
        /// <summary>アサインされたか（ローカルカウント）</summary>
        public IReactiveProperty<bool> IsAssignedLocal => _isAssignedLocal;
        /// <summary>ヒトデ管理設定</summary>
        [SerializeField] private SeastarConfig seastarConfig;
        /// <summary>接触対象のオブジェクトタグ</summary>
        [SerializeField] protected string[] tags = { ConstTagNames.TAG_NAME_DUSTCONNECTSIGNAL };

        private void Reset()
        {
            seastarConfig = GetComponent<SeastarConfig>();
        }

        private void Start()
        {
            _isAssigned.Value = false/*MainGameManager.Instance.GimmickOwner.IsAssigned(seastarConfig.EnumSeastarID)*/;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (0 < tags.Where(q => collision.CompareTag(q)).Select(q => q).ToArray().Length)
            {
                _isAssigned.Value = true;
                _isAssignedLocal.Value = true;
            }
        }

        public bool SetIsAssigned(bool assign)
        {
            try
            {
                _isAssigned.Value = assign;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool ResetIsAssigned()
        {
            try
            {
                _isAssigned.Value = false/*MainGameManager.Instance.GimmickOwner.IsAssigned(seastarConfig.EnumSeastarID)*/;
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
    /// ヒトデ
    /// インターフェース
    /// </summary>
    public interface ISeastarModel
    {
        /// <summary>
        /// アサイン情報をセット
        /// </summary>
        /// <param name="assign">アサイン</param>
        /// <returns>成功／失敗</returns>
        public bool SetIsAssigned(bool assign);

        /// <summary>
        /// アサイン情報をリセット
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool ResetIsAssigned();
    }
}
