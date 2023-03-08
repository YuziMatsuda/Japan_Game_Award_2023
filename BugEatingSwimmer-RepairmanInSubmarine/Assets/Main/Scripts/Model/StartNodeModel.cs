using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;
using DG.Tweening;

namespace Main.Model
{
    /// <summary>
    /// モデル
    /// スタートノード
    /// </summary>
    [RequireComponent(typeof(BoxCollider2D))]
    public class StartNodeModel : AbstractPivotModel
    {
        /// <summary>信号発生アニメーション実行中</summary>
        private readonly BoolReactiveProperty _isPosting = new BoolReactiveProperty();
        /// <summary>信号発生アニメーション実行中</summary>
        public IReactiveProperty<bool> IsPosting => _isPosting;
        /// <summary>信号発生アニメーション時間</summary>
        [SerializeField] private float postDuration = .5f;
        /// <summary>信号発生インターバル</summary>
        [SerializeField] private float postIntervalSeconds = 5f;

        protected override void Start()
        {
            base.Start();
            _intDirectionModes = GetIntDirectionModes();
            Observable.Interval(System.TimeSpan.FromSeconds(postIntervalSeconds))
                .Subscribe(x =>
                {
                    if (!Posting(_isPosting))
                        Debug.LogError("送信呼び出しの失敗");
                }).AddTo(this);
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (0 < tags.Where(q => collision.CompareTag(q)).Select(q => q).ToArray().Length)
            {
                if (!Posting(_isPosting))
                    Debug.LogError("送信呼び出しの失敗");
            }
        }

        /// <summary>
        /// 方角モードをInt二次元配列へ変換して取得
        /// </summary>
        /// <returns>方角モード（Int二次元配列）</returns>
        private int[][] GetIntDirectionModes()
        {
            try
            {
                int[][] directionModesArray = { new int[3], new int[3], new int[3] };
                directionModesArray[0] = new int[3] { 0, 1, 0 };
                directionModesArray[1] = new int[3] { 1, 1, 1 };
                directionModesArray[2] = new int[3] { 0, 1, 0 };

                return directionModesArray;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }

        /// <summary>
        /// 送信
        /// </summary>
        /// <param name="isPosting">信号発生アニメーション実行中</param>
        /// <returns>成功／失敗</returns>
        private bool Posting(BoolReactiveProperty isPosting)
        {
            try
            {
                if (!isPosting.Value)
                {
                    isPosting.Value = true;
                    DOVirtual.DelayedCall(postDuration, () =>
                    {
                        // T.B.D 信号発生演出
                        Debug.Log("T.B.D 信号発生演出");
                    }).OnComplete(() => isPosting.Value = false);
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
}
