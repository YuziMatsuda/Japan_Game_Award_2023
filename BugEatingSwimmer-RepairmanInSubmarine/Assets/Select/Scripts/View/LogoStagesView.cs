using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;

namespace Select.View
{
    /// <summary>
    /// ビュー
    /// ロゴステージの統括パネル
    /// </summary>
    public class LogoStagesView : SelectStageFrameViewParent, ILogoStagesView
    {
        /// <summary>拡大中フラグ</summary>
        private readonly BoolReactiveProperty _isZoomed = new BoolReactiveProperty();
        /// <summary>拡大中フラグ</summary>
        public IReactiveProperty<bool> IsZoomed => _isZoomed;
        /// <summary>移動アニメーションの位置リスト</summary>
        [SerializeField] private Vector3[] offsetPositions;
        /// <summary>拡大アニメーションのスケールリスト</summary>
        [SerializeField] private Vector3[] offsetScales;
        /// <summary>位置デフォルト</summary>
        private Vector3 _defaultOffsetLocalPosition;
        /// <summary>スケールデフォルト</summary>
        private Vector3 _defaultOffsetScale;

        protected override void Start()
        {
            base.Start();
            if (_transform == null)
                _transform = transform;
            _defaultOffsetLocalPosition = _transform.localPosition;
            _defaultOffsetScale = _transform.localScale;
        }

        public bool ZoomInOutPanel(int index)
        {
            return ZoomInOutPanel(index, false);
        }

        public bool ZoomInOutPanel(int index, bool zoomOutmode)
        {
            try
            {
                if (!zoomOutmode)
                {
                    if (!_isZoomed.Value)
                    {
                        _isZoomed.Value = true;
                        if (_transform == null)
                            _transform = transform;
                        var seq = DOTween.Sequence();
                        seq.Append(_transform.DOLocalMove(offsetPositions[index], duration))
                            .Join(_transform.DOScale(offsetScales[index], duration))
                        ;
                    }
                }
                else
                {
                    if (_isZoomed.Value)
                    {
                        _isZoomed.Value = false;
                        if (_transform == null)
                            _transform = transform;
                        var seq = DOTween.Sequence();
                        seq.Append(_transform.DOLocalMove(_defaultOffsetLocalPosition, duration))
                            .Join(_transform.DOScale(_defaultOffsetScale, duration))
                        ;
                    }
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
    /// ビュー
    /// ロゴステージの統括パネル
    /// インターフェース
    /// </summary>
    public interface ILogoStagesView
    {
        /// <summary>
        /// 該当ステージを拡大させる
        /// </summary>
        /// <param name="index">ステージ番号</param>
        /// <returns>成功／失敗</returns>
        public bool ZoomInOutPanel(int index);

        /// <summary>
        /// 該当ステージを拡大させる
        /// </summary>
        /// <param name="index">ステージ番号</param>
        /// <param name="zoomOutmode">ズームアウトする</param>
        /// <returns>成功／失敗</returns>
        public bool ZoomInOutPanel(int index, bool zoomOutmode);
    }
}
