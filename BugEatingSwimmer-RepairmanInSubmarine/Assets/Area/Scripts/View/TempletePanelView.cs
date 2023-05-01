using Area.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

namespace Area.View
{
    /// <summary>
    /// テンプレートパネル
    /// ビュー
    /// </summary>
    public class TempletePanelView : MonoBehaviour, ITempletePanelView
    {
        /// <summary>各ユニットの制御配列</summary>
        [SerializeField] private RobotUnitImageView[] robotUnitImageViews;
        /// <summary>ユニットの制御設定</summary>
        [SerializeField] private RobotUnitImageConfig robotUnitImageConfig;
        /// <summary>一つ前の位置</summary>
        private Vector3 _prevPosition;
        /// <summary>一つ前のスケール</summary>
        private Vector3 _prevScale;

        private void Reset()
        {
            robotUnitImageViews = GetComponentsInChildren<RobotUnitImageView>();
            robotUnitImageConfig = GetComponent<RobotUnitImageConfig>();
        }

        public IEnumerator PlayAnimationZoomInUnit(EnumUnitID enumUnitID, System.IObserver<bool> observer)
        {
            var rectTransform = transform as RectTransform;
            var origin = rectTransform.position;
            Sequence sequence = DOTween.Sequence()
                .Append(rectTransform.DOAnchorPos(origin - robotUnitImageViews.Where(q => q.RobotUnitImageConfig.EnumUnitID.Equals(enumUnitID))
                    .Select(q => q)
                    .ToArray()[0].transform.position, robotUnitImageConfig.Durations[0]))
                .Join(rectTransform.DOScale(robotUnitImageConfig.ScaleZoom, robotUnitImageConfig.Durations[0]))
                .SetUpdate(true)
                .OnComplete(() => observer.OnNext(true));

            yield return null;
        }

        public IEnumerator PlayAnimationZoomOutUnit(EnumUnitID enumUnitID, System.IObserver<bool> observer)
        {
            observer.OnNext(true);
            // T.B.D 位置補正を調整
            //var rectTransform = transform as RectTransform;
            ////var origin = rectTransform.position;
            ////var moved = rectTransform.position - robotUnitImageViews.Where(q => q.RobotUnitImageConfig.EnumUnitID.Equals(enumUnitID))
            ////        .Select(q => q)
            ////        .ToArray()[0].transform.position;
            ////var originScale = rectTransform.localScale;
            //Sequence sequence = DOTween.Sequence()
            //    .Append(rectTransform.DOAnchorPos(_prevPosition, robotUnitImageConfig.Durations[0]))
            //    .Join(rectTransform.DOScale(_prevScale, robotUnitImageConfig.Durations[0]))
            //    .SetUpdate(true)
            //    .OnComplete(() => observer.OnNext(true));

            yield return null;
        }

        public bool SetPositionAndScaleAfterPrevSaveTransform(EnumUnitID enumUnitID)
        {
            try
            {
                // T.B.D 位置補正を調整
                //var rectTransform = transform as RectTransform;
                //_prevPosition = rectTransform.position;
                //_prevScale = rectTransform.localScale;
                //rectTransform.position = _prevPosition - robotUnitImageViews.Where(q => q.RobotUnitImageConfig.EnumUnitID.Equals(enumUnitID))
                //        .Select(q => q)
                //        .ToArray()[0].transform.position;
                //rectTransform.localScale = robotUnitImageConfig.ScaleZoom;

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
    /// テンプレートパネル
    /// ビュー
    /// インターフェース
    /// </summary>
    public interface ITempletePanelView
    {
        /// <summary>
        /// ユニットを拡大させるアニメーションを再生
        /// </summary>
        /// <param name="enumUnitID">エリアID</param>
        /// <param name="observer">バインド</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayAnimationZoomInUnit(EnumUnitID enumUnitID, System.IObserver<bool> observer);
        /// <summary>
        /// ユニットを拡大⇒縮小させるアニメーションを再生
        /// </summary>
        /// <param name="enumUnitID">エリアID</param>
        /// <param name="observer">バインド</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayAnimationZoomOutUnit(EnumUnitID enumUnitID, System.IObserver<bool> observer);
        /// <summary>
        /// 位置とスケールをセットして一つ前の状態を保存する
        /// </summary>
        /// <param name="enumUnitID">エリアID</param>
        /// <returns>成功／失敗</returns>
        public bool SetPositionAndScaleAfterPrevSaveTransform(EnumUnitID enumUnitID);
    }
}
