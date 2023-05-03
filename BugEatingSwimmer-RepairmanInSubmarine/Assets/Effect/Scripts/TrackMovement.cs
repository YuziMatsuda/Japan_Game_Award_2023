using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Effect
{
    /// <summary>
    /// 追尾挙動
    /// </summary>
    public class TrackMovement : MonoBehaviour, ITrackMovement
    {
        /// <summary>追尾対象オブジェクト</summary>
        private Transform _target;
        /// <summary>追尾対象オブジェクト</summary>
        public Transform Target => _target;

        public bool SetTarget(Transform target)
        {
            try
            {
                _target = target;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        private void Start()
        {
            var t = transform;

            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    if (_target != null)
                        t.position = _target.position;
                });
        }
    }

    /// <summary>
    /// 追尾挙動
    /// インターフェース
    /// </summary>
    public interface ITrackMovement
    {
        /// <summary>
        /// ターゲットをセットする
        /// </summary>
        /// <param name="target">ターゲット</param>
        /// <returns>成功／失敗</returns>
        public bool SetTarget(Transform target);
    }
}
