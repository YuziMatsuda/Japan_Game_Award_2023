using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Common;
using UniRx;
using System.Linq;
using DG.Tweening;

namespace Main.Model
{
    /// <summary>
    /// モデル
    /// 始点／スタートノード／ゴールノードの親クラス
    /// オブジェクトごとにコライダーは複数存在する想定のためRequireComponentでコライダー有無はチェックしない
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class AbstractPivotModel : MonoBehaviour
    {
        /// <summary>接触対象のオブジェクトタグ</summary>
        [SerializeField] protected string[] tags = { ConstTagNames.TAG_NAME_ATTACK_TRIGGER };
        /// <summary>トランスフォーム</summary>
        protected Transform _transform;
        /// <summary>信号発生アニメーション実行中</summary>
        protected readonly BoolReactiveProperty _isPosting = new BoolReactiveProperty();
        /// <summary>信号発生アニメーション実行中</summary>
        public IReactiveProperty<bool> IsPosting => _isPosting;
        /// <summary>信号受信フラグ</summary>
        protected readonly BoolReactiveProperty _isGetting = new BoolReactiveProperty();
        /// <summary>信号受信フラグ</summary>
        public IReactiveProperty<bool> IsGetting => _isGetting;
        /// <summary>POST先のノードコードリスト項目数</summary>
        protected readonly IntReactiveProperty _toListLength = new IntReactiveProperty(-1);
        /// <summary>POST先のノードコードリスト項目数</summary>
        public IReactiveProperty<int> ToListLength => _toListLength;
        /// <summary>POST先のノードコードリスト</summary>
        protected Transform[] _toList;
        /// <summary>POST先のノードコードリスト</summary>
        public Transform[] ToList => _toList;
        /// <summary>GET元のノードコードリスト項目数</summary>
        protected readonly IntReactiveProperty _fromListLength = new IntReactiveProperty(-1);
        /// <summary>GET元のノードコードリスト項目数</summary>
        public IReactiveProperty<int> FromListLength => _fromListLength;
        /// <summary>GET元のノードコードリスト</summary>
        protected Transform[] _fromList;
        /// <summary>GET元のノードコードリスト</summary>
        public Transform[] FromList => _fromList;
        /// <summary>レイの距離</summary>
        [SerializeField] protected float rayDistance = 3f;
        /// <summary>レイのレイヤーマスク</summary>
        [SerializeField] protected LayerMask rayLayerMask;

        protected virtual void Reset()
        {
            rayLayerMask = 1 << LayerMask.NameToLayer(ConstTagNames.TAG_NAME_MOLECULES);
        }

        protected virtual void Start()
        {
            if (_transform == null)
                _transform = transform;
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision) { }
    }
}
