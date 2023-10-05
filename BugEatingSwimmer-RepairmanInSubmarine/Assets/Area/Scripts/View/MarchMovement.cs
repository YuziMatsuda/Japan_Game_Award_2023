using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;

namespace Area.View
{
    /// <summary>
    /// マーチ移動制御
    /// ビュー
    /// </summary>
    public class MarchMovement : MarchMovementParent
    {
        protected override void PlayMarching()
        {
            if (_transform == null)
                _transform = transform;
            _transform.DOLocalMoveX(marchDistance, durations[0])
                .SetEase(Ease.Linear);
        }

        /// <summary>
        /// スプライトを閉じた状態から開く状態へ切り替えるアニメーションを再生
        /// 呼び出しメソッド
        /// </summary>
        public void OnPlayChangeSpriteCloseBetweenOpen()
        {
            Observable.FromCoroutine<bool>(observer => GetComponent<Main.View.RuleShellfishView>().PlayChangeSpriteCloseBetweenOpen(observer))
                .Subscribe(_ => { })
                .AddTo(gameObject);
        }
    }
}
