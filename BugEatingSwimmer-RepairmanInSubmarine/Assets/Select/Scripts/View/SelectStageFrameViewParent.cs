using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Select.View
{
    /// <summary>
    /// ビュー
    /// ステージセレクトのフレーム
    /// 親
    /// </summary>
    public class SelectStageFrameViewParent : MonoBehaviour
    {
        /// <summary>トランスフォーム</summary>
        protected Transform _transform;
        /// <summary>移動アニメーション時間</summary>
        [SerializeField] protected float duration = .5f;
        /// <summary>イメージのコンポーネント</summary>
        protected Image _image;

        protected virtual void Start()
        {

        }
    }
}
