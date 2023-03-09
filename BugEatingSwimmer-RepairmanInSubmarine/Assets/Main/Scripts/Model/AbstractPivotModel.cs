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
        /// <summary>方角モード二次元配列</summary>
        protected int[][] _intDirectionModes = { new int[3], new int[3], new int[3] };
        /// <summary>方角モード二次元配列</summary>
        public int[][] IntDirectionModes => _intDirectionModes;

        protected virtual void Start()
        {
            if (_transform == null)
                _transform = transform;
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision) { }
    }
}
