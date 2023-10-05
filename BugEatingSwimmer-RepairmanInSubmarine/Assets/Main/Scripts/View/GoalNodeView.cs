using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;
using Main.Common;

namespace Main.View
{
    /// <summary>
    /// ビュー
    /// ゴールノード
    /// </summary>
    public class GoalNodeView : ShadowCodeCellParent, IGoalNodeView, IStartNodeView
    {
        /// <summary>バグ</summary>
        [SerializeField] private Transform bug;
        /// <summary>バグクローン</summary>
        private Transform _instancedBug;
        /// <summary>バグクローン</summary>
        public Transform InstanceBug => _instancedBug;
        /// <summary>移動アニメーション時間</summary>
        [SerializeField] private float bugMoveDuration = .5f;
        /// <summary>移動アニメーション方向</summary>
        [SerializeField] private Vector3 bugMoveDirection = Vector3.up;
        /// <summary>移動アニメーション距離</summary>
        [SerializeField] private float bugMoveDistance = 1.5f;
        /// <summary>移動アニメーション距離</summary>
        public float BugMoveDistance => bugMoveDistance;
        /// <summary>信号発生アニメーション時間</summary>
        private bool _isRuning;
        /// <summary>撤退移動アニメーション時間</summary>
        [SerializeField] private float bugReturnMoveDuration = .15f;

        public IEnumerator bugfix(System.IObserver<bool> observer, bool isBugFixed)
        {
            if (_instancedBug == null)
            {
                _instancedBug = Instantiate(bug, transform.position, Quaternion.identity, transform);
                // 未クリアかクリア済みでカラーを変更
                if (_instancedBug.GetComponent<BugView>() != null)
                {
                    if (!_instancedBug.GetComponent<BugView>().SetColorCleared(isBugFixed))
                        Debug.LogError("カラーを設定呼び出しの失敗");
                    if (!_instancedBug.GetComponent<BugView>().PlayBugAura())
                        Debug.LogError("バグオーラを発生呼び出しの失敗");
                }
                _instancedBug.DOLocalMove(bugMoveDirection * bugMoveDistance, bugMoveDuration)
                    .OnComplete(() => observer.OnNext(true));
            }
            else
                observer.OnNext(true);

            yield return null;
        }




        public bool SetIsRuning(bool isRuning)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator degrad(System.IObserver<bool> observer)
        {
            _instancedBug.DOLocalMove(Vector3.zero, bugReturnMoveDuration)
                .OnComplete(() => observer.OnNext(true));
            yield return null;
        }

        public IEnumerator PlayLightAnimation(System.IObserver<bool> observer)
        {
            throw new System.NotImplementedException();
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
        /// <param name="observer">バインド</param>
        /// <param name="isBugFixed">バグフィックス状態</param>
        /// <returns>成功／失敗</returns>
        public IEnumerator bugfix(System.IObserver<bool> observer, bool isBugFixed);
        /// <summary>
        /// デグレード
        /// バグを撤退させる
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <returns>コルーチン</returns>
        public IEnumerator degrad(System.IObserver<bool> observer);
    }
}
