using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// ビュー
    /// 移動先にポインタ表示
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class TargetPointerView : MonoBehaviour, ITargetPointerView
    {
        /// <summary>スプライトレンダラー</summary>
        [SerializeField] private SpriteRenderer spriteRenderer;
        /// <summary>トランスフォーム</summary>
        private Transform _transform;
        /// <summary>ターゲットポインタを表示させるか</summary>
        [SerializeField] private bool isVisibled = true;

        private void Reset()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            if (!SetFadeValue(isVisibled ? 255f : 0f))
                Debug.LogError("フェード値をセット呼び出しの失敗");
            if (!isVisibled)
                Debug.LogWarning("ポインタ非表示モード");
        }

        public bool RenderVisible()
        {
            try
            {
                if (isVisibled)
                {
                    if (!SetFadeValue(255f))
                        throw new System.Exception("フェード値をセット呼び出しの失敗");
                }
                else
                    Debug.LogWarning("ポインタ非表示モード");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool RenderUnVisible()
        {
            try
            {
                if (!SetFadeValue(0f))
                    throw new System.Exception("フェード値をセット呼び出しの失敗");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// フェード値をセット
        /// </summary>
        /// <param name="alphaValue">アルファ値</param>
        /// <returns>成功／失敗</returns>
        private bool SetFadeValue(float alphaValue)
        {
            try
            {
                Color c = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alphaValue);
                spriteRenderer.color = c;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetPosition(Vector3 originPosition, Vector2 movePosition)
        {
            try
            {
                if (_transform == null)
                    _transform = transform;
                _transform.position = originPosition + new Vector3(movePosition.x, movePosition.y, 0f);
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
    /// 移動先にポインタ表示
    /// </summary>
    public interface ITargetPointerView
    {
        /// <summary>
        /// 見える状態に描画
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool RenderVisible();
        /// <summary>
        /// 見えない状態に描画
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool RenderUnVisible();
        /// <summary>
        /// 位置情報をセット
        /// </summary>
        /// <param name="originPosition">対象位置</param>
        /// <param name="position">移動先の位置</param>
        /// <returns>成功／失敗</returns>
        public bool SetPosition(Vector3 originPosition, Vector2 movePosition);
    }
}
