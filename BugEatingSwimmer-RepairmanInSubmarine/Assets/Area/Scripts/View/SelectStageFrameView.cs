using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Area.View
{
    /// <summary>
    /// ビュー
    /// ステージセレクトのフレーム
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class SelectStageFrameView : SelectStageFrameViewParent, ISelectStageFrameView
    {
        public IEnumerator MoveSelectPlayer(Vector3 targetPosition, Transform currentTarget, System.IObserver<bool> observer)
        {
            throw new System.NotImplementedException();
        }

        public bool MoveSelectStageFrame(Vector3 targetPosition, Vector2 sizeDelta)
        {
            try
            {
                if (_transform == null)
                    _transform = transform;
                _transform.position = targetPosition;
                (_transform as RectTransform).sizeDelta = sizeDelta;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SelectPlayer(Vector3 targetPosition, Transform currentTarget)
        {
            throw new System.NotImplementedException();
        }

        public bool SetColorAlpha(float alpha)
        {
            try
            {
                if (_image == null)
                    _image = GetComponent<Image>();
                var color = _image.color;
                color.a = alpha;
                _image.color = color;

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
    /// ステージセレクトのフレーム
    /// インターフェース
    /// </summary>
    public interface ISelectStageFrameView
    {
        /// <summary>
        /// ステージ選択のフレームを移動して選択させる
        /// </summary>
        /// <param name="targetPosition">移動先</param>
        /// <param name="sizeDelta">サイズ</param>
        /// <returns>成功／失敗</returns>
        public bool MoveSelectStageFrame(Vector3 targetPosition, Vector2 sizeDelta);

        /// <summary>
        /// ステージ選択のプレイヤーを移動して選択させる
        /// </summary>
        /// <param name="targetPosition">移動先</param>
        /// <param name="currentTarget">今のターゲット</param>
        /// <returns>コルーチン</returns>
        public IEnumerator MoveSelectPlayer(Vector3 targetPosition, Transform currentTarget, System.IObserver<bool> observer);

        /// <summary>
        /// ステージ選択のプレイヤーを移動して選択させる
        /// </summary>
        /// <param name="targetPosition">移動先</param>
        /// <param name="currentTarget">今のターゲット</param>
        /// <returns>成功／失敗</returns>
        public bool SelectPlayer(Vector3 targetPosition, Transform currentTarget);

        /// <summary>
        /// 透明度をセット
        /// </summary>
        /// <param name="alpha">アルファ値</param>
        /// <returns>成功／失敗</returns>
        public bool SetColorAlpha(float alpha);
    }
}
