using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

namespace Main.View
{
    /// <summary>
    /// ビュー
    /// CinemachineVirtualCamera
    /// </summary>
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CinemachineVirtualCameraView : MonoBehaviour, ICinemachineVirtualCameraView
    {
        /// <summary>CinemachineVirtualCamera</summary>
        private CinemachineVirtualCamera _cinemachineVirtualCamera;
        /// <summary>デフォルトのターゲット</summary>
        [SerializeField] private Transform defaultTarget;
        /// <summary>カメラのオフセット</summary>
        [SerializeField] private Vector3[] bodyTrackedObjectOffsets = {new Vector3(0f, 5f, 0f), new Vector3(0f, 0f, 0f) };
        /// <summary>アニメーション終了時間</summary>
        [SerializeField] private float[] durations = { .5f };

        public bool SetBodyTrackedObjectOffsets(EnumBodyTrackedObjectOffsetIndex index)
        {
            try
            {
                if (_cinemachineVirtualCamera == null)
                    _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
                _cinemachineVirtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body)
                    .GetComponent<CinemachineFramingTransposer>()
                    .m_TrackedObjectOffset = bodyTrackedObjectOffsets[(int)index];

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetFollow(Transform target)
        {
            try
            {
                if (_cinemachineVirtualCamera == null)
                    _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
                _cinemachineVirtualCamera.Follow = target;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetFollowAnimation(Transform target)
        {
            try
            {
                if (_cinemachineVirtualCamera == null)
                    _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
                _cinemachineVirtualCamera.Follow.DOMove(target.position, durations[0]);

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetLookAt(Transform target)
        {
            try
            {
                if (_cinemachineVirtualCamera == null)
                    _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
                _cinemachineVirtualCamera.LookAt = target;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetLookAtAnimation(Transform target)
        {
            try
            {
                if (_cinemachineVirtualCamera == null)
                    _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
                _cinemachineVirtualCamera.LookAt.DOMove(target.position, durations[0]);

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        private void Reset()
        {
            defaultTarget = GameObject.Find("Level").transform;
        }
    }

    /// <summary>
    /// ビュー
    /// CinemachineVirtualCamera
    /// インターフェース
    /// </summary>
    public interface ICinemachineVirtualCameraView
    {
        /// <summary>
        /// フォローをセット
        /// </summary>
        /// <param name="target">追従するターゲット</param>
        /// <returns>成功／失敗</returns>
        public bool SetFollow(Transform target);
        /// <summary>
        /// 標準ををセット
        /// </summary>
        /// <param name="target">追従するターゲット</param>
        /// <returns>成功／失敗</returns>
        public bool SetLookAt(Transform target);
        /// <summary>
        /// フォローをセットアニメーション
        /// </summary>
        /// <param name="target">追従するターゲット</param>
        /// <returns>成功／失敗</returns>
        public bool SetFollowAnimation(Transform target);
        /// <summary>
        /// 標準ををセットアニメーション
        /// </summary>
        /// <param name="target">追従するターゲット</param>
        /// <returns>成功／失敗</returns>
        public bool SetLookAtAnimation(Transform target);
        /// <summary>
        /// カメラのオフセットをセット
        /// </summary>
        /// <param name="index">カメラのオフセットのインデックス</param>
        /// <returns>成功／失敗</returns>
        public bool SetBodyTrackedObjectOffsets(EnumBodyTrackedObjectOffsetIndex index);
    }

    /// <summary>
    /// カメラのオフセットのインデックス
    /// </summary>
    public enum EnumBodyTrackedObjectOffsetIndex
    {
        /// <summary>デフォルト</summary>
        Default,
        /// <summary>プレイヤー追尾</summary>
        PlayerTracked,
        /// <summary>イベント１</summary>
        PlayerTrackedEvent_1,
    }
}
