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
    public class PlayerView : SelectStageFrameViewParent, ISelectStageFrameView, INavigationCursor, ILogoCursorView
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
        /// <summary>ステージ選択のカーソルナビゲーション表示</summary>
        [SerializeField] private NavigationCursor navigationCursor;
        /// <summary>ボディのイメージ</summary>
        [SerializeField] private BodyImage bodyImage;

        private void Reset()
        {
            navigationCursor = GetComponentInChildren<NavigationCursor>();
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



        public bool RedererCursorDirectionAndDistance(Navigation navigation, EnumCursorDistance enumCursorDistance)
        {
            return ((INavigationCursor)navigationCursor).RedererCursorDirectionAndDistance(navigation, enumCursorDistance);
        }

        public bool SelectPlayer(Vector3 targetPosition, Transform currentTarget)
        {
            try
            {
                if (_transform == null)
                    _transform = transform;

                if (!bodyImage.SetLocalScaleX(_prevPosition.x <= targetPosition.x ? 1f : -1f))
                    throw new System.Exception("ローカルスケールをセット呼び出しの失敗");
                _transform.position = targetPosition;
                _prevPosition = targetPosition;
                _prevTarget = currentTarget;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetImageEnabled(bool isEnabled)
        {
            return ((ILogoCursorView)navigationCursor).SetImageEnabled(isEnabled);
        }

        public bool SetCursorDistance(EnumCursorDistance enumCursorDistance)
        {
            return ((ILogoCursorView)navigationCursor).SetCursorDistance(enumCursorDistance);
        }

        public bool SetColorAlpha(float alpha)
        {
            return ((ISelectStageFrameView)bodyImage).SetColorAlpha(alpha);
        }
    }
}
