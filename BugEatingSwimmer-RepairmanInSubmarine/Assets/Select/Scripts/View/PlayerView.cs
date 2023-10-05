using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;
using Select.Common;
using GameManagers.Common;

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
        /// <summary>ダイブする距離</summary>
        [SerializeField] private float jumpDistance = 116f;
        /// <summary>アニメーション終了時間</summary>
        [SerializeField] private float[] durations = { .75f, .75f, .25f };

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

        public IEnumerator PlayDiveAnimation(System.IObserver<bool> observer, int direction)
        {
            var scale = bodyImage.transform.localScale;
            bodyImage.transform.localEulerAngles = GetAdjustEulerAngles(EnumDiveStates.Jump, direction);
            var fromAnchor = (bodyImage.transform as RectTransform).anchoredPosition;
            DOTween.Sequence()
                .Append((bodyImage.transform as RectTransform).DOAnchorPos(fromAnchor + Vector2.up * jumpDistance, durations[0]))
                .Append((bodyImage.transform as RectTransform).DOAnchorPos(fromAnchor, durations[1])
                    .OnStart(() => bodyImage.transform.localEulerAngles = GetAdjustEulerAngles(EnumDiveStates.Fall, direction)))
                // イージングをいれるとOnCompleteを発行するタイミングが想定と異なるため別タイミングで制御
                .Join(DOVirtual.DelayedCall(durations[2], () =>
                {
                    bodyImage.transform.localEulerAngles = GetAdjustEulerAngles(EnumDiveStates.Swim, direction);
                    observer.OnNext(true);
                }))
                .SetEase(Ease.OutCirc);

            yield return null;
        }

        /// <summary>
        /// ジャンプまたは、落下時の角度をアジャストした値を取得
        /// </summary>
        /// <param name="state">ダイブの状態</param>
        /// <param name="direction">角度</param>
        /// <returns>返還後の角度</returns>
        private Vector3 GetAdjustEulerAngles(EnumDiveStates state, int direction)
        {
            switch (state)
            {
                case EnumDiveStates.Jump:
                    return IsForward(direction) ? new Vector3(0, 0, 30f) : new Vector3(0, 0, -30f);
                case EnumDiveStates.Fall:
                    return IsForward(direction) ? new Vector3(0, 0, -30f) : new Vector3(0, 0, 30f);
                case EnumDiveStates.Swim:
                    return Vector3.zero;
                default:
                    throw new System.Exception("例外エラー");
            }
        }

        /// <summary>
        /// 正面か
        /// </summary>
        /// <param name="direction">角度</param>
        /// <returns>正面か</returns>
        private bool IsForward(int direction)
        {
            switch ((EnumDirectionMode2D)direction)
            {
                case EnumDirectionMode2D.Back:
                    return false;
                case EnumDirectionMode2D.Default:
                    return true;
                case EnumDirectionMode2D.Forward:
                    return true;
                default:
                    throw new System.Exception("例外エラー");
            }
        }

        /// <summary>
        /// ダイブの状態
        /// </summary>
        private enum EnumDiveStates
        {
            /// <summary>ジャンプ</summary>
            Jump,
            /// <summary>落下</summary>
            Fall,
            /// <summary>泳ぐ（通常）</summary>
            Swim,
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
        /// <summary>
        /// 飛び込みアニメーションを再生
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <param name="direction">角度</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayDiveAnimation(System.IObserver<bool> observer, int direction);
    }
}
