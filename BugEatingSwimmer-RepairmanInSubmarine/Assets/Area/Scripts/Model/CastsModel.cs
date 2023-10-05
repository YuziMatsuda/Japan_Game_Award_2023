using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Area.Common;

namespace Area.Model
{
    /// <summary>
    /// キャスト
    /// モデル
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class CastsModel : CastsParent
    {
        /// <summary>キャスト入場か</summary>
        private BoolReactiveProperty _isBeginMarch = new BoolReactiveProperty();
        /// <summary>キャスト入場か</summary>
        public IReactiveProperty<bool> IsBeginMarch => _isBeginMarch;
        /// <summary>
        /// 歩くが開始された
        /// Animator->Animation - EndingCastOpen
        /// </summary>
        protected override void OnWark()
        {
            _isBeginMarch.Value = true;
        }
    }
}
