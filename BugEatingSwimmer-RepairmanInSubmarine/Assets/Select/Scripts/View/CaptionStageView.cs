using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;
using System;

namespace Select.View
{
    /// <summary>
    /// ビュー
    /// ステージキャプション
    /// </summary>
    public class CaptionStageView : SelectStageFrameViewParent, ILogoStagesView, ICaptionStageView
    {
        /// <summary>再生する直前にセットする位置リスト</summary>
        [SerializeField] private Vector3[] offsetPositions;
        /// <summary>再生する直前にセットするスケールリスト</summary>
        [SerializeField] private Vector3[] offsetScales;
        /// <summary>ヒトデのビュー配列</summary>
        [SerializeField] private SeastarView[] seastarViews;

        protected override void Start()
        {
            base.Start();
            if (_transform == null)
                _transform = transform;
        }

        public bool ZoomInOutPanel(int index)
        {
            try
            {
                if (_transform == null)
                    _transform = transform;
                _transform.localPosition = offsetPositions[1];
                _transform.localScale = offsetScales[1];
                var seq = DOTween.Sequence();
                seq.Append(_transform.DOLocalMove(offsetPositions[0], duration))
                    .Join(_transform.DOScale(offsetScales[0], duration))
                ;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public IEnumerator ZoomInOutPanel(int index, IObserver<bool> observer)
        {
            var seq = DOTween.Sequence();
            seq.Append(_transform.DOLocalMove(offsetPositions[1], duration))
                .Join(_transform.DOScale(offsetScales[1], duration))
                .OnComplete(() => observer.OnNext(true));

            yield return null;
        }

        public bool ZoomInOutPanel(int index, bool zoomOutmode)
        {
            throw new NotImplementedException();
        }

        public bool SetColorAssigned(int index)
        {
            return seastarViews[index].SetColorAssigned();
        }

        public bool SetColorUnAssign(int index)
        {
            return seastarViews[index].SetColorUnAssign();
        }
    }

    /// <summary>
    /// ビュー
    /// ステージキャプション
    /// </summary>
    public interface ICaptionStageView
    {
        /// <summary>
        /// フェードのDOTweenアニメーション再生
        /// </summary>
        /// <param name="index">ステージ番号</param>
        /// <param name="observer">バインド</param>
        /// <returns>成功／失敗</returns>
        public IEnumerator ZoomInOutPanel(int index, System.IObserver<bool> observer);
        /// <summary>
        /// アサイン済みのカラー設定
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <returns>成功／失敗</returns>
        public bool SetColorAssigned(int index);
        /// <summary>
        /// 未アサイン状態のカラー設定
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <returns>成功／失敗</returns>
        public bool SetColorUnAssign(int index);
    }
}
