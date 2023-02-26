using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Model
{
    /// <summary>
    /// レベル内のレイ判定の共通設定
    /// カプセルキャスト
    /// </summary>
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class LevelPhysicsSerializerCapsule : MonoBehaviour
    {
        /// <summary>オリジン</summary>
        [SerializeField] protected Vector2 origin;
        /// <summary>サイズ</summary>
        [SerializeField] protected Vector2 size;
        /// <summary>カプセル形状の向き</summary>
        [SerializeField] protected CapsuleDirection2D capsuleDirection;
        /// <summary>角度</summary>
        [SerializeField] protected float angle;
        /// <summary>レイの向き</summary>
        [SerializeField] protected Vector2 direction;
        /// <summary>レイの距離</summary>
        [SerializeField] protected float distance;
        /// <summary>プレビュー表示（選択時）の色</summary>
        [SerializeField] protected Color previewColor;

        protected virtual void Reset()
        {
            // 規定値
            previewColor = Color.green;

            origin = gameObject.transform.position;
            size = gameObject.transform.lossyScale * GetComponent<CapsuleCollider2D>().size;
            capsuleDirection = CapsuleDirection2D.Vertical;
            angle = 0f;
            direction = Vector3.zero;
            distance = 6.0f;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = previewColor;
            Gizmos.DrawWireSphere(capsuleDirection.Equals(CapsuleDirection2D.Vertical) ? origin - new Vector2(0f, size.y / 4) : origin - new Vector2(size.x / 4, 0f), size.x / 2);
            Gizmos.DrawWireSphere((capsuleDirection.Equals(CapsuleDirection2D.Vertical) ? origin + new Vector2(0f, size.y / 4) : origin + new Vector2(size.x / 4, 0f)) + direction * distance, size.x / 2);
        }
    }
}
