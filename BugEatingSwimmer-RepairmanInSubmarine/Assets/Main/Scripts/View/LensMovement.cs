using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

namespace Main.View
{
    /// <summary>
    /// レンズ制御
    /// <see href="https://nekojara.city/unity-cinemachine-mouse-zoom#%E6%BB%91%E3%82%89%E3%81%8B%E3%81%AA%E3%82%BA%E3%83%BC%E3%83%A0%E6%93%8D%E4%BD%9C%E3%81%AB%E5%AF%BE%E5%BF%9C%E3%81%99%E3%82%8B">
    /// 【Unity】Cinemachineでマウスホイールのズームを拡張機能で実装する > マウスホイールでズーム操作する拡張機能
    /// </see>
    /// </summary>
    public class LensMovement : CinemachineExtension, ILensMovement
    {
        /// <summary>スクロール量の設定値</summary>
        [SerializeField] private float[] scrollDeltas = { -10f };
        /// <summary>アジャストするFOV</summary>
        private float _adjustFOV;
        /// <summary>アニメーション終了時間</summary>
        [SerializeField] private float[] durations = { .5f };

        public bool PlayScrollDeltaZoomIn()
        {
            try
            {
                DOTween.To(() => 0f, x => _adjustFOV = x, scrollDeltas[0], durations[0]);

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            // Aimの直後だけ処理を実施
            if (stage != CinemachineCore.Stage.Aim)
                return;

            var lens = state.Lens;
            // stateの内容は毎回リセットされるので、
            // 毎回補正する必要がある
            lens.FieldOfView += _adjustFOV;
            state.Lens = lens;
        }
    }

    /// <summary>
    /// レンズ制御
    /// インターフェース
    /// </summary>
    public interface ILensMovement
    {
        /// <summary>
        /// スクロール量をセット
        /// ズームイン
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool PlayScrollDeltaZoomIn();
    }
}
