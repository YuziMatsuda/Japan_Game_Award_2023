using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Select.Common;
using UniRx;
using Select.Template;
using System.Linq;

namespace Select.Model
{
    /// <summary>
    /// モデル
    /// ヒトデ
    /// </summary>
    [RequireComponent(typeof(SeastarConfig))]
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
        /// <summary>ヒトデ管理設定</summary>
        public SeastarConfig SeastarConfig => seastarConfig;

        private void Reset()
        {
            seastarConfig = GetComponent<SeastarConfig>();
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
                _isAssigned.Value = SelectGameManager.Instance.GimmickOwner.IsAssigned(seastarConfig.EnumSeastarID);
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
