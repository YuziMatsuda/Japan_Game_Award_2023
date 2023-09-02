using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;
using Select.Common;

namespace Select.View
{
    /// <summary>
    /// ビュー
    /// プレイヤー
    /// </summary>
    public class PlayerView : SelectStageFrameViewParent, ISelectStageFrameView, IPlayerView, IBodyImageForTransform
    {
        /// <summary>一つ前の位置</summary>
        private Vector3 _prevPosition;
        /// <summary>一つ前のターゲット</summary>
        private Transform _prevTarget;
        /// <summary>一つ前のターゲット（デフォルト）</summary>
        [SerializeField] private Transform defaultTarget;
        /// <summary>移動元オブジェクトリスト</summary>
        /// <summary>アニメーション再生</summary>
        private readonly BoolReactiveProperty _isPlaying = new BoolReactiveProperty();
        /// <summary>アニメーション再生</summary>
        public IReactiveProperty<bool> IsPlaying => _isPlaying;
        /// <summary>ボディのイメージ</summary>
        [SerializeField] private BodyImage bodyImage;
        /// <summary>スキップモード（キャプション選択のキャンセル）</summary>
        private bool _isSkipMode;
        /// <summary>スキップモード（キャプション選択のキャンセル）</summary>
        public bool IsSkipMode => _isSkipMode;

        private void Reset()
        {
            bodyImage = GetComponentInChildren<BodyImage>();
        }

        protected override void Start()
        {
            base.Start();
            _prevPosition = transform.position;
            _prevTarget = defaultTarget;
        }

        public IEnumerator MoveSelectPlayer(Vector3 targetPosition, Transform currentTarget, System.IObserver<bool> observer)
        {
            if (_isSkipMode)
            {
                observer.OnNext(true);
                // ステージのキャプション選択／コードを突いた後にSEを鳴らさない
                yield return null;
            }

            if (!_isPlaying.Value)
            {
                _isPlaying.Value = true;
                if (_transform == null)
                    _transform = transform;

                if (!bodyImage.SetLocalScaleX(_prevPosition.x <= targetPosition.x ? 1f : -1f))
                    throw new System.Exception("ローカルスケールをセット呼び出しの失敗");
                _transform.DOMove(targetPosition, duration)
                    .OnComplete(() =>
                    {
                        _isPlaying.Value = false;
                        observer.OnNext(true);
                    });
                _prevPosition = targetPosition;
                _prevTarget = currentTarget;
            }
            yield return null;
        }

        public bool MoveSelectStageFrame(Vector3 targetPosition, Vector2 sizeDelta)
        {
            throw new System.NotImplementedException();
        }

        public bool SelectPlayer(Vector3 targetPosition, Transform currentTarget)
        {
            return bodyImage.SelectPlayer(targetPosition, currentTarget);
        }

        public bool SetColorAlpha(float alpha)
        {
            return ((ISelectStageFrameView)bodyImage).SetColorAlpha(alpha);
        }

        public bool SetSkipMode(bool isSkipMode)
        {
            try
            {
                _isSkipMode = isSkipMode;
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetLocalScaleX(float scaleX)
        {
            return ((IBodyImageForTransform)bodyImage).SetLocalScaleX(scaleX);
        }
    }

    /// <summary>
    /// ビュー
    /// プレイヤー
    /// インターフェース
    /// </summary>
    public interface IPlayerView
    {
        /// <summary>
        /// スキップモードのセット
        /// </summary>
        /// <param name="isSkipMode">スキップモード</param>
        /// <returns>成功／失敗</returns>
        public bool SetSkipMode(bool isSkipMode);
    }
}
