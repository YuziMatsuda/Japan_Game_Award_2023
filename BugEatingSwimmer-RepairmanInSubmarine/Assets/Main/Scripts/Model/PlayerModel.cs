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
        [SerializeField] private ForceMode2D forceMode = ForceMode2D.Impulse;
        /// <summary>移動向き</summary>
        [SerializeField] private EnumMovementDirectionMode movementDirectionMode = EnumMovementDirectionMode.LeftOrRight;
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
        /// <summary>アタックモーション時間</summary>
        [SerializeField] private float attackDuration = .5f;
        /// <summary>アタックモーション距離</summary>
        [SerializeField] private float attackDistance = 1f;
        /// <summary>アタックモーションモード</summary>
        [SerializeField] private EnumAttackMotionMode enumAttackMotionMode = EnumAttackMotionMode.OneAttack;
        /// <summary>つつくアクション実行中フラグ</summary>
        private readonly BoolReactiveProperty _isPlayingAction = new BoolReactiveProperty();
        /// <summary>つつくアクション実行中フラグ</summary>
        public IReactiveProperty<bool> IsPlayingAction => _isPlayingAction;

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
        [SerializeField] private Vector3 normalizedOutput;
        [SerializeField] private bool isPlayingActionOutput;

        private void Start()
        {
            _targetPointerClone = Instantiate(targetPointer);
            if (_targetPointerClone != null)
                _isInstanced.Value = true;
            // Rigidbody
            var rigidbody = GetComponent<Rigidbody2D>();
            var rigidbodyGravityScale = rigidbody.gravityScale;
            // 移動制御のベロシティ
            var moveVelocity = new Vector2ReactiveProperty();
            var moveVelocityLast = new Vector2ReactiveProperty();
            // 位置・スケールのキャッシュ
            var transform = base.transform;
            // 移動入力に応じて移動座標をセット
            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    if (!_inputBan)
                    {
                        moveVelocityOutput = moveVelocity.Value;
                        magnitudeOutput = moveVelocityOutput.magnitude;
                        normalizedOutput = moveVelocityOutput.normalized;
                        if (!AttackMovement(_isPlayingAction, moveVelocity, moveVelocityLast, movementDirectionMode))
                            Debug.LogError("攻撃のモーション呼び出しの失敗");
                    }
                });
            // 移動制御
            this.FixedUpdateAsObservable()
                .Subscribe(_ =>
                {
                    // 歩く走る挙動
                    rigidbody.AddForce(moveVelocity.Value * (forceMode.Equals(ForceMode2D.Force) ? 1f : Time.fixedDeltaTime), forceMode);
                    // Axis指定がゼロなら位置移動の減衰値を高めて止まり安くする
                    rigidbody.drag = 0f < moveVelocity.Value.magnitude && !_isPlayingAction.Value ? 0f : brake;
                    // 移動方向に合わせて向きを変える
                    if (0f < moveVelocity.Value.magnitude)
                        transform.rotation = GetQuaternionDirection(movementDirectionMode, transform);
                });
        }

        /// <summary>
        /// 攻撃のモーション
        /// </summary>
        /// <param name="isPlayingAction">攻撃モーション中</param>
        /// <param name="moveVelocity">移動ベロシティ</param>
        /// <param name="moveVelocityLast">最後に向いた移動ベロシティ</param>
        /// <param name="movementDirectionMode">移動向き</param>
        /// <returns>成功／失敗</returns>
        private bool AttackMovement(BoolReactiveProperty isPlayingAction, Vector2ReactiveProperty moveVelocity, Vector2ReactiveProperty moveVelocityLast, EnumMovementDirectionMode movementDirectionMode)
        {
            try
            {
                if (!isPlayingAction.Value)
                    moveVelocity.Value = new Vector2(MainGameManager.Instance.InputSystemsOwner.InputPlayer.Moved.x, MainGameManager.Instance.InputSystemsOwner.InputPlayer.Moved.y) * moveSpeed;
                if (0 < moveVelocity.Value.magnitude)
                    moveVelocityLast.Value = moveVelocity.Value;
                _moveVelocityReactiveProperty.Value = moveVelocity.Value;
                isPlayingActionOutput = isPlayingAction.Value;
                if (!isPlayingAction.Value &&
                    MainGameManager.Instance.InputSystemsOwner.InputPlayer.Attacked)
                {
                    isPlayingAction.Value = true;
                    switch (enumAttackMotionMode)
                    {
                        case EnumAttackMotionMode.OneAttack:
                            transform.DOPunchPosition(GetNormalized(moveVelocityLast.Value, movementDirectionMode) * attackDistance, attackDuration, 1, 1f)
                                .OnComplete(() => isPlayingAction.Value = false);
                            break;
                        case EnumAttackMotionMode.SeveralAttack:
                            // DOPunchPositionはもっとダイナミックに動かさないと振動数の引数が作用しない？
                            transform.DOPunchPosition(GetNormalized(moveVelocityLast.Value, movementDirectionMode) * attackDistance, attackDuration, 5, 1f)
                                .OnComplete(() => isPlayingAction.Value = false);
                            break;
                        default:
                            Debug.LogError("例外エラー");
                            break;
                    }
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// ベクターのノーマライズを取得
        /// </summary>
        /// <param name="moveVelocityLast">最後に向いた移動ベロシティ</param>
        /// <param name="movementDirectionMode">移動向き</param>
        /// <returns>ベクター（ノーマライズ）</returns>
        private Vector2 GetNormalized(Vector2 moveVelocityLast, EnumMovementDirectionMode movementDirectionMode)
        {
            if (!movementDirectionMode.Equals(EnumMovementDirectionMode.LeftOrRight))
                return moveVelocityLast.normalized;
            else
            {
                return new Vector2(moveVelocityLast.normalized.x, 0f);
            }
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

        /// <summary>
        /// 攻撃アクションモーションモード
        /// </summary>
        private enum EnumAttackMotionMode
        {
            /// <summary>一回つつく</summary>
            OneAttack,
            /// <summary>複数つつく</summary>
            SeveralAttack,
        }
    }
}
