using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;

namespace Main.View
{
    /// <summary>
    /// ビュー
    /// ゴールノード
    /// </summary>
    public class GoalNodeView : MonoBehaviour, IGoalNodeView
    {
        /// <summary>バグ</summary>
        [SerializeField] private Transform bug;
        /// <summary>バグクローン</summary>
        private Transform _instancedBug;
        /// <summary>バグクローン</summary>
        public Transform InstanceBug => _instancedBug;
        /// <summary>移動アニメーション時間</summary>
        [SerializeField] private float bugMoveDutation = .5f;
        /// <summary>移動アニメーション方向</summary>
        [SerializeField] private Vector3 bugMoveDirection = Vector3.up;
        /// <summary>移動アニメーション距離</summary>
        [SerializeField] private float bugMoveDistance = 1.5f;

        public bool bugfix()
        {
            try
            {
                if (_instancedBug == null)
                {
                    _instancedBug = Instantiate(bug, transform.position, Quaternion.identity, transform);
                    _instancedBug.DOLocalMove(bugMoveDirection, bugMoveDutation * bugMoveDistance);
                }

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
    /// インターフェース
    /// ゴールノード
    /// </summary>
    public interface IGoalNodeView
    {
        /// <summary>
        /// バグフィックス
        /// バグを出現させる
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool bugfix();
    }
}
