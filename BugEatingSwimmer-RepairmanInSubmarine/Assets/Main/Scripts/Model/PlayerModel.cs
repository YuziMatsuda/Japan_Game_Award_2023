using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Main.Common;
using Main.Audio;
using DG.Tweening;

namespace Main.Model
{
    /// <summary>
    /// モデル
    /// プレイヤー
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class PlayerModel : MonoBehaviour
    {
        /// <summary>移動速度</summary>
        [SerializeField] private float moveSpeed = 4f;
        /// <summary>操作禁止フラグ</summary>
        private bool _inputBan;
        /// <summary>操作禁止フラグ</summary>
        public bool InputBan => _inputBan;
        /// <summary>ブレーキ</summary>
        [SerializeField] private float brake = 5f;
        /// <summary>移動速度挙動</summary>
        [SerializeField] private ForceMode2D forceMode = ForceMode2D.Force;
        /// <summary>移動向き</summary>
        [SerializeField] private EnumMovementDirectionMode movementDirectionMode = EnumMovementDirectionMode.Seamless;
        /// <summary>ターゲットポインタ</summary>
        [SerializeField] private Transform targetPointer;
        /// <summary>ターゲットポインタ</summary>
        private Transform _targetPointerClone;
        /// <summary>ターゲットポインタ</summary>
        public Transform TargetPointer => _targetPointerClone;
        /// <summary>移動ベロシティ</summary>
        private readonly Vector2ReactiveProperty _moveVelocityReactiveProperty = new Vector2ReactiveProperty();
        /// <summary>移動ベロシティ</summary>
        public IReactiveProperty<Vector2> MoveVelocityReactiveProperty => _moveVelocityReactiveProperty;
        /// <summary>プレイヤーがインスタンス済みか</summary>
        private readonly BoolReactiveProperty _isInstanced = new BoolReactiveProperty();
        /// <summary>プレイヤーがインスタンス済みか</summary>
        public IReactiveProperty<bool> IsInstanced => _isInstanced;

        /// <summary>
        /// 操作禁止フラグをセット
        /// </summary>
        /// <param name="unactive">許可／禁止</param>
        /// <returns>成功／失敗</returns>
        public bool SetInputBan(bool unactive)
        {
            try
            {
                _inputBan = unactive;
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        [SerializeField] private Vector3 moveVelocityOutput;
        [SerializeField] private float magnitudeOutput;

        private void Start()
        {
            _targetPointerClone = Instantiate(targetPointer);
            if (_targetPointerClone != null)
                _isInstanced.Value = true;
            // Rigidbody
            var rigidbody = GetComponent<Rigidbody2D>();
            var rigidbodyGravityScale = rigidbody.gravityScale;
            // 移動制御のベロシティ
            Vector2 moveVelocity = new Vector2();
            // 位置・スケールのキャッシュ
            var transform = base.transform;
            // 移動入力に応じて移動座標をセット
            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    if (!_inputBan)
                    {
                        moveVelocityOutput = moveVelocity;
                        magnitudeOutput = moveVelocityOutput.magnitude;
                        moveVelocity = new Vector2(MainGameManager.Instance.InputSystemsOwner.InputPlayer.Moved.x, MainGameManager.Instance.InputSystemsOwner.InputPlayer.Moved.y) * moveSpeed;
                        _moveVelocityReactiveProperty.Value = moveVelocity;
                    }
                });
            // 移動制御
            this.FixedUpdateAsObservable()
                .Subscribe(_ =>
                {
                    // 歩く走る挙動
                    rigidbody.AddForce(moveVelocity * (forceMode.Equals(ForceMode2D.Force) ? 1f : Time.fixedDeltaTime), forceMode);
                    // Axis指定がゼロなら位置移動の減衰値を高めて止まり安くする
                    rigidbody.drag = 0f < moveVelocity.magnitude ? 0f : brake;
                    // 移動方向に合わせて向きを変える
                    transform.rotation = GetQuaternionDirection(movementDirectionMode, transform);
                });
        }

        /// <summary>
        /// 向き先をクォータニオンで取得
        /// </summary>
        /// <param name="mode">移動向きモード</param>
        /// <param name="transform">トランスフォーム</param>
        /// <returns>クォータニオン</returns>
        private Quaternion GetQuaternionDirection(EnumMovementDirectionMode mode, Transform transform)
        {
            switch (mode)
            {
                case EnumMovementDirectionMode.Seamless:
                    var toDirection = _targetPointerClone.position - transform.position;
                    
                    return Quaternion.FromToRotation(Vector2.right, toDirection);
                case EnumMovementDirectionMode.LeftOrRight:
                    toDirection = _targetPointerClone.position - transform.position;
                    if (toDirection.x < Vector3.left.x)
                        toDirection = Vector3.left;
                    else if (Vector3.right.x <= toDirection.x)
                        toDirection = Vector3.right;
                    else
                        return transform.rotation;

                    return Quaternion.FromToRotation(Vector2.right, toDirection);
                case EnumMovementDirectionMode.LeftOrRightOrUpOrDown:
                    toDirection = _targetPointerClone.position - transform.position;
                    if (Mathf.Abs(toDirection.x) < Mathf.Abs(toDirection.y))
                    {
                        if (toDirection.y < Vector3.down.y)
                            toDirection = Vector3.down;
                        else if (Vector3.up.y <= toDirection.y)
                            toDirection = Vector3.up;
                        else
                            return transform.rotation;
                    }
                    else if (Mathf.Abs(toDirection.y) < Mathf.Abs(toDirection.x))
                    {
                        if (toDirection.x < Vector3.left.x)
                            toDirection = Vector3.left;
                        else if (Vector3.right.x <= toDirection.x)
                            toDirection = Vector3.right;
                        else
                            return transform.rotation;
                    }
                    else
                        return transform.rotation;

                    return Quaternion.FromToRotation(Vector2.right, toDirection);
                default:
                    throw new System.Exception("未到達エラー");
            }
        }

        /// <summary>
        /// 移動向きモード
        /// </summary>
        private enum EnumMovementDirectionMode
        {
            /// <summary>シームレス</summary>
            Seamless,
            /// <summary>左右</summary>
            LeftOrRight,
            /// <summary>上下左右</summary>
            LeftOrRightOrUpOrDown,
        }
    }
}
