using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Area.Common;
using System;

namespace Area.View
{
    /// <summary>
    /// 全てのユニットの制御
    /// ビュー
    /// </summary>
    public class RobotPanelView : MonoBehaviour, IRobotPanelView
    {
        /// <summary>各ユニットの制御配列</summary>
        [SerializeField] private RobotUnitImageView[] robotUnitImageViews;

        private void Reset()
        {
            robotUnitImageViews = GetComponentsInChildren<RobotUnitImageView>();
        }

        public bool SetPositionAndEulerAngleOfAllUnit(EnumRobotPanel enumRobotPanel)
        {
            try
            {
                switch (enumRobotPanel)
                {
                    case EnumRobotPanel.FallingApart:
                        break;
                    case EnumRobotPanel.ConnectedHead:
                        robotUnitImageViews[0].SetPositionAndEulerAngle();
                        robotUnitImageViews[1].SetPositionAndEulerAngle();
                        break;
                    case EnumRobotPanel.ConnectedRightarm:
                        robotUnitImageViews[0].SetPositionAndEulerAngle();
                        robotUnitImageViews[1].SetPositionAndEulerAngle();
                        robotUnitImageViews[2].SetPositionAndEulerAngle();
                        break;
                    case EnumRobotPanel.ConnectedLeftarm:
                        robotUnitImageViews[0].SetPositionAndEulerAngle();
                        robotUnitImageViews[1].SetPositionAndEulerAngle();
                        robotUnitImageViews[3].SetPositionAndEulerAngle();
                        break;
                    case EnumRobotPanel.ConnectedDoublearm:
                        robotUnitImageViews[0].SetPositionAndEulerAngle();
                        robotUnitImageViews[1].SetPositionAndEulerAngle();
                        robotUnitImageViews[2].SetPositionAndEulerAngle();
                        robotUnitImageViews[3].SetPositionAndEulerAngle();
                        break;
                    case EnumRobotPanel.Full:
                        robotUnitImageViews[0].SetPositionAndEulerAngle();
                        robotUnitImageViews[1].SetPositionAndEulerAngle();
                        robotUnitImageViews[2].SetPositionAndEulerAngle();
                        robotUnitImageViews[3].SetPositionAndEulerAngle();
                        robotUnitImageViews[4].SetPositionAndEulerAngle();
                        robotUnitImageViews[4].SetImageAltha(true);
                        break;
                    default:
                        throw new System.Exception("例外エラー");
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public IEnumerator PlayAnimationOfAllUnit(EnumRobotPanel enumRobotPanel, System.IObserver<bool> observer)
        {
            switch (enumRobotPanel)
            {
                case EnumRobotPanel.FallingApart:
                    observer.OnNext(true);
                    break;
                case EnumRobotPanel.ConnectedFailureHead:
                    // T.B.D 接続失敗演出
                    observer.OnNext(false);
                    break;
                case EnumRobotPanel.ConnectedHead:
                    var connectedBody = new IntReactiveProperty();
                    connectedBody.ObserveEveryValueChanged(x => x.Value)
                        .Subscribe(x =>
                        {
                            if (1 < connectedBody.Value)
                                observer.OnNext(true);
                        });
                    Observable.FromCoroutine<bool>(observer => robotUnitImageViews[0].PlayAnimationMove(observer))
                        .Subscribe(_ => connectedBody.Value++)
                        .AddTo(gameObject);
                    Observable.FromCoroutine<bool>(observer => robotUnitImageViews[1].PlayAnimationMove(observer))
                        .Subscribe(_ => connectedBody.Value++)
                        .AddTo(gameObject);
                    break;
                case EnumRobotPanel.ConnectedRightarm:
                    Observable.FromCoroutine<bool>(observer => robotUnitImageViews[2].PlayAnimationMove(observer))
                        .Subscribe(_ => observer.OnNext(true))
                        .AddTo(gameObject);
                    break;
                case EnumRobotPanel.ConnectedLeftarm:
                    Observable.FromCoroutine<bool>(observer => robotUnitImageViews[3].PlayAnimationMove(observer))
                        .Subscribe(_ => observer.OnNext(true))
                        .AddTo(gameObject);
                    break;
                case EnumRobotPanel.ConnectedDoublearm:
                    var connectedArms = new IntReactiveProperty();
                    connectedArms.ObserveEveryValueChanged(x => x.Value)
                        .Subscribe(x =>
                        {
                            if (1 < connectedArms.Value)
                                observer.OnNext(true);
                        });
                    Observable.FromCoroutine<bool>(observer => robotUnitImageViews[2].PlayAnimationMove(observer))
                        .Subscribe(_ => connectedArms.Value++)
                        .AddTo(gameObject);
                    Observable.FromCoroutine<bool>(observer => robotUnitImageViews[3].PlayAnimationMove(observer))
                        .Subscribe(_ => connectedArms.Value++)
                        .AddTo(gameObject);
                    break;
                case EnumRobotPanel.Full:
                    robotUnitImageViews[4].SetPositionAndEulerAngle();
                    // シーン読み込み時のアニメーション
                    Observable.FromCoroutine<bool>(observer => robotUnitImageViews[4].PlayFadeAnimation(observer, true))
                        .Subscribe(_ => observer.OnNext(true))
                        .AddTo(gameObject);
                    break;
                default:
                    throw new System.Exception("例外エラー");
            }

            yield return null;
        }

        public bool RendererDisableMode(EnumUnitID enumUnitID)
        {
            try
            {
                switch (enumUnitID)
                {
                    case EnumUnitID.Head:
                        robotUnitImageViews[0].RendererDisableMode();
                        break;
                    case EnumUnitID.Body:
                        robotUnitImageViews[1].RendererDisableMode();
                        break;
                    case EnumUnitID.RightArm:
                        robotUnitImageViews[2].RendererDisableMode();
                        break;
                    case EnumUnitID.LeftArm:
                        robotUnitImageViews[3].RendererDisableMode();
                        break;
                    case EnumUnitID.Core:
                        // 処理無し
                        break;
                    default:
                        throw new System.Exception("例外エラー");
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public IEnumerator PlayRenderEnable(EnumUnitID enumUnitID, System.IObserver<bool> observer)
        {
            switch (enumUnitID)
            {
                case EnumUnitID.Head:
                    Observable.FromCoroutine<bool>(observer => robotUnitImageViews[0].PlayRenderEnable(observer))
                        .Subscribe(_ => observer.OnNext(true))
                        .AddTo(gameObject);
                    break;
                case EnumUnitID.Body:
                    Observable.FromCoroutine<bool>(observer => robotUnitImageViews[1].PlayRenderEnable(observer))
                        .Subscribe(_ => observer.OnNext(true))
                        .AddTo(gameObject);
                    break;
                case EnumUnitID.RightArm:
                    Observable.FromCoroutine<bool>(observer => robotUnitImageViews[2].PlayRenderEnable(observer))
                        .Subscribe(_ => observer.OnNext(true))
                        .AddTo(gameObject);
                    break;
                case EnumUnitID.LeftArm:
                    Observable.FromCoroutine<bool>(observer => robotUnitImageViews[3].PlayRenderEnable(observer))
                        .Subscribe(_ => observer.OnNext(true))
                        .AddTo(gameObject);
                    break;
                case EnumUnitID.Core:
                    // 処理無し
                    break;
                default:
                    throw new System.Exception("例外エラー");
            }

            yield return null;
        }

        public bool RendererEnableMode(EnumUnitID enumUnitID)
        {
            try
            {
                switch (enumUnitID)
                {
                    case EnumUnitID.Head:
                        robotUnitImageViews[0].RendererEnableMode();
                        break;
                    case EnumUnitID.Body:
                        robotUnitImageViews[1].RendererEnableMode();
                        break;
                    case EnumUnitID.RightArm:
                        robotUnitImageViews[2].RendererEnableMode();
                        break;
                    case EnumUnitID.LeftArm:
                        robotUnitImageViews[3].RendererEnableMode();
                        break;
                    case EnumUnitID.Core:
                        // 処理無し
                        break;
                    default:
                        throw new System.Exception("例外エラー");
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public IEnumerator PlayRenderEnable(EnumUnitID[] enumUnitIDs, IObserver<bool> observer)
        {
            var count = new IntReactiveProperty();
            count.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    if (enumUnitIDs.Length <= x)
                        observer.OnNext(true);
                });
            foreach (var item in enumUnitIDs)
                Observable.FromCoroutine<bool>(observer => PlayRenderEnable(item, observer))
                    .Subscribe(_ => count.Value++)
                    .AddTo(gameObject);

            yield return null;
        }
    }

    /// <summary>
    /// 全てのユニットの制御
    /// ビュー
    /// インターフェース
    /// </summary>
    public interface IRobotPanelView
    {
        /// <summary>
        /// 各ユニットに対して配置変更する
        /// </summary>
        /// <param name="enumRobotPanel">ロボットの結合状態</param>
        /// <returns>成功／失敗</returns>
        public bool SetPositionAndEulerAngleOfAllUnit(EnumRobotPanel enumRobotPanel);
        /// <summary>
        /// 各ユニットに対してアニメーション再生
        /// </summary>
        /// <param name="enumRobotPanel">ロボットの結合状態</param>
        /// <param name="observer">バインド</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayAnimationOfAllUnit(EnumRobotPanel enumRobotPanel, System.IObserver<bool> observer);
        /// <summary>
        /// 対象ユニットを非選択状態にする
        /// </summary>
        /// <param name="enumUnitID">ユニットID</param>
        /// <returns>成功／失敗</returns>
        public bool RendererDisableMode(EnumUnitID enumUnitID);
        /// <summary>
        /// 対象ユニットを選択状態にする
        /// </summary>
        /// <param name="enumUnitID">ユニットID</param>
        /// <returns>成功／失敗</returns>
        public bool RendererEnableMode(EnumUnitID enumUnitID);
        /// <summary>
        /// 対象ユニットを選択状態へ切り替える演出
        /// </summary>
        /// <param name="enumUnitID">ユニットID</param>
        /// <param name="observer">バインド</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayRenderEnable(EnumUnitID enumUnitID, System.IObserver<bool> observer);
        /// <summary>
        /// 対象ユニットを選択状態へ切り替える演出
        /// </summary>
        /// <param name="enumUnitIDs">ユニットID配列</param>
        /// <param name="observer">バインド</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayRenderEnable(EnumUnitID[] enumUnitIDs, System.IObserver<bool> observer);
    }
}

