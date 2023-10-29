using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Main.Common;

namespace Main.View
{
    /// <summary>
    /// コードセル
    /// </summary>
    public class ShadowCodeCell : ShadowCodeCellParent, IShadowCodeCell
    {
        /// <summary>ターンアニメーション時間</summary>
        [SerializeField] private float turnDuration = .35f;
        /// <summary>ターンロックアニメーション時間</summary>
        [SerializeField] private float lockDuration = .1f;
        /// <summary>ターンロックアニメーション角度</summary>
        [SerializeField] private Vector3 lockDirection = new Vector3(0f, 0f, -15f);
        /// <summary>ターンロックアニメーションループ回数</summary>
        [SerializeField] private int lockLoopCount = 4;
        /// <summary>支点ライトの見た目</summary>
        [SerializeField] private PivotDynamic pivotDynamic;
        /// <summary>デフォルト角度（サン（ゴ）ショウコードでのみ使用）</summary>
        private Vector3 _defaultDirection;
        /// <summary>ターンロックアニメーション再生中</summary>
        private bool _isPlayingLockAnimation;

        private void Reset()
        {
            pivotDynamic = transform.GetChild(0).GetComponent<PivotDynamic>();
        }

        public IEnumerator PlaySpinAnimation(IObserver<bool> observer, Vector3 vectorDirectionMode, bool restartMode)
        {
            if (_transform == null)
                _transform = transform;
            if (restartMode)
                _transform.DOComplete();
            _transform.DOLocalRotate(vectorDirectionMode, turnDuration)
                .OnComplete(() => observer.OnNext(true));

            yield return null;
        }

        public bool SetSpinDirection(Vector3 vectorDirectionMode)
        {
            throw new NotImplementedException();
        }

        public bool SetAlphaOff()
        {
            throw new NotImplementedException();
        }

        public bool InitializeLight(EnumDirectionMode enumDirectionMode)
        {
            throw new NotImplementedException();
        }

        public IEnumerator PlayErrorLightFlashAnimation(IObserver<bool> observer)
        {
            throw new NotImplementedException();
        }

        public bool SetSprite(EnumPivotDynamic index)
        {
            return pivotDynamic.SetSprite(index);
        }

        public IEnumerator PlayLockSpinAnimation(IObserver<bool> observer)
        {
            if (!_isPlayingLockAnimation)
            {
                _isPlayingLockAnimation = true;

                _transform.DOLocalRotate(_defaultDirection + lockDirection, lockDuration)
                    .From(_defaultDirection)
                    .SetLoops(lockLoopCount, LoopType.Yoyo)
                    .OnComplete(() =>
                    {
                        observer.OnNext(true);
                        _isPlayingLockAnimation = false;
                    });
            }

            yield return null;
        }

        public bool SetDefaultDirection()
        {
            try
            {
                if (_transform == null)
                    _transform = transform;
                _defaultDirection = _transform.localEulerAngles;

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public IEnumerator PlayLightAnimation(IObserver<bool> observer, EnumDirectionMode enumDirectionMode, bool restartMode)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// コード
    /// インターフェース
    /// </summary>
    public interface IShadowCodeCell
    {
        /// <summary>
        /// 回転アニメーション
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <param name="vectorDirectionMode">方角モードのベクター</param>
        /// <param name="restartMode">リスタートモード</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlaySpinAnimation(System.IObserver<bool> observer, Vector3 vectorDirectionMode, bool restartMode);

        /// <summary>
        /// 回転方角セット
        /// </summary>
        /// <param name="vectorDirectionMode">方角モードのベクター</param>
        /// <returns>成功／失敗</returns>
        public bool SetSpinDirection(Vector3 vectorDirectionMode);

        /// <summary>
        /// ライト点灯アニメーション
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <param name="enumDirectionMode">方角モード</param>
        /// <param name="restartMode">リスタートモード</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayLightAnimation(System.IObserver<bool> observer, EnumDirectionMode enumDirectionMode, bool restartMode);

        /// <summary>
        /// ライト点灯初期設定
        /// </summary>
        /// <param name="enumDirectionMode">方角モード</param>
        /// <returns>成功／失敗</returns>
        public bool InitializeLight(EnumDirectionMode enumDirectionMode);

        /// <summary>
        /// アルファ値をセット
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool SetAlphaOff();

        /// <summary>
        /// エラーライト点滅アニメーション
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayErrorLightFlashAnimation(System.IObserver<bool> observer);

        /// <summary>
        /// スプライトをセット
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <returns>成功／失敗</returns>
        public bool SetSprite(EnumPivotDynamic index);

        /// <summary>
        /// 回転ロックアニメーション
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayLockSpinAnimation(System.IObserver<bool> observer);
        /// <summary>
        /// デフォルト角度をセット
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool SetDefaultDirection();
    }
}
