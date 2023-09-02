using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;
using Select.Model;
using Select.Common;
using System;

namespace Select.View
{
    /// <summary>
    /// ビュー
    /// 支点とコード
    /// </summary>
    [RequireComponent(typeof(PivotConfig))]
    public class PivotAndCodeIShortUIView : PivotAndCodeIShortUIViewParent, IPivotAndCodeIShortUIView, ISelectGameManager
    {
        /// <summary>ターンアニメーション実行中</summary>
        private readonly BoolReactiveProperty _isTurning = new BoolReactiveProperty();
        /// <summary>方角モード</summary>
        private readonly IntReactiveProperty _enumDirectionMode = new IntReactiveProperty();
        /// <summary>方角モード</summary>
        public IReactiveProperty<int> EnumDirectionMode => _enumDirectionMode;
        /// <summary>方角モードのベクター配列</summary>
        [SerializeField] private Vector3[] vectorDirectionModes = { new Vector3(0, 0, 0f), new Vector3(0, 0, -90f), new Vector3(0, 0, 180f), new Vector3(0, 0, 90f) };
        /// <summary>コード挙動制御</summary>
        [SerializeField] private ShadowCodeCell shadowCodeCell;
        /// <summary>コード（明）挙動制御</summary>
        [SerializeField] private LightCodeCell lightCodeCell;
        /// <summary>アルゴリズムの共通処理</summary>
        private AlgorithmCommon _algorithmCommon = new AlgorithmCommon();
        /// <summary>背景フレームイメージ</summary>
        [SerializeField] private FadeImageView backgroundFrame;

        public void OnStart()
        {
            if (_transform == null)
                _transform = transform;
            _enumDirectionMode.Value = (int)GetComponent<PivotConfig>().EnumDirectionModeDefault;
            if (!lightCodeCell.SetAlphaOff())
                Debug.LogError("アルファ値をセット呼び出しの失敗");
            if (!lightCodeCell.InitializeLight((EnumDirectionMode)_enumDirectionMode.Value))
                Debug.LogError("アルファ値をセット呼び出しの失敗");
        }

        public IEnumerator PlaySpinAnimationAndUpdateTurnValue(IObserver<bool> observer)
        {
            // 通常コードの振る舞い
            _isTurning.Value = true;
            var turnValue = 1;
            if (turnValue == 0)
                Debug.LogError("ターン加算値の取得呼び出しの失敗");
            _enumDirectionMode.Value = (int)_algorithmCommon.GetAjustedEnumDirectionMode((EnumDirectionMode)_enumDirectionMode.Value, turnValue);
            // 回転アニメーション
            if (!lightCodeCell.SetAlphaOff())
                Debug.LogError("アルファ値をセット呼び出しの失敗");
            Observable.FromCoroutine<bool>(observer => shadowCodeCell.PlaySpinAnimation(observer, vectorDirectionModes[_enumDirectionMode.Value]))
                .Subscribe(_ =>
                {
                    if (!lightCodeCell.SetSpinDirection(vectorDirectionModes[_enumDirectionMode.Value]))
                        Debug.LogError("回転方角セット呼び出しの失敗");
                    Observable.FromCoroutine<bool>(observer => lightCodeCell.PlayLightAnimation(observer, (EnumDirectionMode)_enumDirectionMode.Value))
                        .Subscribe(_ =>
                        {
                            _isTurning.Value = false;
                            observer.OnNext(true);
                        })
                        .AddTo(gameObject);
                })
                .AddTo(gameObject);

            yield return null;
        }

        public bool SetAsSemiLastSibling()
        {
            try
            {
                _transform.SetSiblingIndex(_transform.parent.childCount - 2);

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        private void Reset()
        {
            shadowCodeCell = transform.GetChild(0).GetChild(0).GetComponent<ShadowCodeCell>() != null ? transform.GetChild(0).GetChild(0).GetComponent<ShadowCodeCell>() : null;
            lightCodeCell = transform.GetChild(1).GetComponent<LightCodeCell>() != null ? transform.GetChild(1).GetComponent<LightCodeCell>() : null;
            var enumDirectionModeDefault = GetComponent<PivotConfig>().EnumDirectionModeDefault;
            shadowCodeCell.transform.localEulerAngles = vectorDirectionModes[(int)enumDirectionModeDefault];
            lightCodeCell.transform.localEulerAngles = vectorDirectionModes[(int)enumDirectionModeDefault];
            backgroundFrame = GetComponentInChildren<FadeImageView>();
        }

        public IEnumerator PlayRenderEnableBackgroundFrame(IObserver<bool> observer)
        {
            Observable.FromCoroutine<bool>(observer => backgroundFrame.PlayFadeAnimation(observer, EnumFadeState.Close))
                .Subscribe(_ => observer.OnNext(true))
                .AddTo(gameObject);
            yield return null;
        }

        public IEnumerator PlayRenderDisableBackgroundFrame(IObserver<bool> observer)
        {
            Observable.FromCoroutine<bool>(observer => backgroundFrame.PlayFadeAnimation(observer, EnumFadeState.Open))
                .Subscribe(_ => observer.OnNext(true))
                .AddTo(gameObject);
            yield return null;
        }

        public bool SetRenderEnableBackgroundFrame()
        {
            try
            {
                if (!backgroundFrame.SetColorSpriteRenderer(EnumFadeState.Close))
                    throw new System.Exception("カラーを設定呼び出しの失敗");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetRenderDisableBackgroundFrame()
        {
            try
            {
                if (!backgroundFrame.SetColorSpriteRenderer(EnumFadeState.Open))
                    throw new System.Exception("カラーを設定呼び出しの失敗");

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
    /// 支点とコード
    /// インターフェース
    /// </summary>
    public interface IPivotAndCodeIShortUIView : ISelectContentsViewParent
    {
        /// <summary>
        /// 回転角度の更新と
        /// 回転アニメーションの再生
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlaySpinAnimationAndUpdateTurnValue(System.IObserver<bool> observer);
        /// <summary>
        /// SetSiblingIndexでparent配下の子オブジェクト数-1へ配置
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool SetAsSemiLastSibling();
    }
}
