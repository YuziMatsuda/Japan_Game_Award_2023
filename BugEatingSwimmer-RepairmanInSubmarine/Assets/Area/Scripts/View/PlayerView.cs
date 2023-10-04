using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;
using Area.Common;

namespace Area.View
{
    /// <summary>
    /// ビュー
    /// プレイヤー
    /// </summary>
    public class PlayerView : SelectStageFrameViewParent, ISelectStageFrameView, INavigationCursor, ILogoCursorView, IPlayerView
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
        /// <summary>スキップモード（キャプション選択のキャンセル）</summary>
        private bool _isSkipMode;
        /// <summary>スキップモード（キャプション選択のキャンセル）</summary>
        public bool IsSkipMode => _isSkipMode;
        /// <summary>設定</summary>
        [SerializeField] private PlayerConfig playerConfig;
        /// <summary>ダイブする距離</summary>
        [SerializeField] private float jumpDistance = 116f;
        /// <summary>アニメーション終了時間</summary>
        [SerializeField] private float[] durations = { .75f, .75f, .25f };

        private void Reset()
        {
            navigationCursor = GetComponentInChildren<NavigationCursor>();
            bodyImage = GetComponentInChildren<BodyImage>();
            playerConfig = GetComponent<PlayerConfig>();
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
            if (_isSkipMode)
                // ステージのキャプション選択／コードを突いた後にSEを鳴らさない
                return true;
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

        public bool SetAnchorPosition(int index)
        {
            try
            {
                if (_transform == null)
                    _transform = transform;
                (_transform as RectTransform).anchoredPosition = playerConfig.DefaultAnchors[index];

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public IEnumerator PlayDiveAnimation(System.IObserver<bool> observer)
        {
            bodyImage.transform.localEulerAngles = GetAdjustEulerAngles(EnumDiveStates.Jump, bodyImage.transform.localScale);
            var fromAnchor = (bodyImage.transform as RectTransform).anchoredPosition;
            DOTween.Sequence()
                .Append((bodyImage.transform as RectTransform).DOAnchorPos(fromAnchor + Vector2.up * jumpDistance, durations[0]))
                .Append((bodyImage.transform as RectTransform).DOAnchorPos(fromAnchor, durations[1])
                    .OnStart(() => bodyImage.transform.localEulerAngles = GetAdjustEulerAngles(EnumDiveStates.Fall, bodyImage.transform.localScale)))
                // イージングをいれるとOnCompleteを発行するタイミングが想定と異なるため別タイミングで制御
                .Join(DOVirtual.DelayedCall(durations[2], () => observer.OnNext(true)))
                .SetEase(Ease.OutCirc);

            yield return null;
        }

        /// <summary>
        /// ジャンプまたは、落下時の角度をアジャストした値を取得
        /// </summary>
        /// <param name="state">ダイブの状態</param>
        /// <param name="bodyImageScale">ボディのスケール</param>
        /// <returns>返還後の角度</returns>
        private Vector3 GetAdjustEulerAngles(EnumDiveStates state, Vector3 bodyImageScale)
        {
            switch (state)
            {
                case EnumDiveStates.Jump:
                    return bodyImageScale.Equals(Vector3.one) ? new Vector3(0, 0, 30f) : new Vector3(0, 0, -30f);
                case EnumDiveStates.Fall:
                    return bodyImageScale.Equals(Vector3.one) ? new Vector3(0, 0, -30f) : new Vector3(0, 0, 30f);
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
        /// アンカー位置をセット
        /// </summary>
        /// <param name="index">配列番号</param>
        /// <returns>成功／失敗</returns>
        public bool SetAnchorPosition(int index);
        /// <summary>
        /// 飛び込みアニメーションを再生
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayDiveAnimation(System.IObserver<bool> observer);
    }
}
