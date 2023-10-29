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
    public class PlayerModel : MonoBehaviour, IPlayerModel
    {
        /// <summary>移動速度</summary>
        [SerializeField] private float moveSpeed = 4f;
        /// <summary>操作禁止フラグ</summary>
        private bool _inputBan;
        /// <summary>操作禁止フラグ</summary>
        public bool InputBan => _inputBan;
        /// <summary>移動制御禁止フラグ</summary>
        private readonly BoolReactiveProperty _isBanMoveVelocity = new BoolReactiveProperty();
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
        /// <summary>正面と判断する入力角度</summary>
        [SerializeField, Range(0f, 180f)] private float inputAngleFrontal = 60f;
        /// <summary>パワー状態</summary>
        private readonly BoolReactiveProperty _isPower = new BoolReactiveProperty();
        /// <summary>パワー状態</summary>
        public IReactiveProperty<bool> IsPower => _isPower;
        /// <summary>パワーチャージ時間</summary>
        private readonly FloatReactiveProperty _inputPowerChargeTime = new FloatReactiveProperty();
        /// <summary>パワーチャージ時間</summary>
        public IReactiveProperty<float> InputPowerChargeTime => _inputPowerChargeTime;
        /// <summary>パワーチャージの段階を変える時間</summary>
        [SerializeField] private float[] powerChargePhaseTimes = { 0f, .5f, 1f };
        /// <summary>パワーチャージの段階を変える時間</summary>
        public float[] PowerChargePhaseTimes => powerChargePhaseTimes;
        /// <summary>押し続けて離す</summary>
        private readonly BoolReactiveProperty _isPressAndHoldAndReleased = new BoolReactiveProperty();
        /// <summary>押し続けて離す</summary>
        public IReactiveProperty<bool> IsPressAndHoldAndReleased => _isPressAndHoldAndReleased;
        /// <summary>押し続けて離すクールタイム</summary>
        [SerializeField] private float isPressAndHoldAndReleasedCoolTime = .1f;
        /// <summary>ターン実行状態</summary>
        private readonly BoolReactiveProperty _onTurn = new BoolReactiveProperty();
        /// <summary>ターン実行状態</summary>
        public IReactiveProperty<bool> OnTurn => _onTurn;
        /// <summary>遅延実行の時間</summary>
        [SerializeField] private float delayDoDuration = .25f;
        /// <summary>泳ぐ状態であるか</summary>
        private readonly BoolReactiveProperty _isSwimming = new BoolReactiveProperty();
        /// <summary>泳ぐ状態であるか</summary>
        public IReactiveProperty<bool> IsSwimming => _isSwimming;
        /// <summary>最後に入力された速度ベクター（向きの判定用）</summary>
        private Vector2ReactiveProperty _moveVelocityLast = new Vector2ReactiveProperty();
        /// <summary>攻撃先のターゲット</summary>
        private Vector3 _targetAttackingPosition;
        /// <summary>攻撃先のターゲット</summary>
        public Vector3 TargetAttackingPosition => _targetAttackingPosition;
        /// <summary>攻撃の予測稼働範囲の補正値</summary>
        [SerializeField] private float attackingDistance = 1.785f;

        public bool SetInputBan(bool unactive)
        {
            return SetInputBan(unactive, false);
        }

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
            // 最後に参照された移動向き（デフォルトは右向き）
            var toDirectionLast = new Vector3ReactiveProperty(Vector3.right);
            // 位置・スケールのキャッシュ
            var transform = base.transform;
            // 移動入力に応じて移動座標をセット
            this.UpdateAsObservable()
                .Subscribe(_ =>
                {

                    if (!_inputBan)
                    {
                        if (!AttackMovement(_isPlayingAction, moveVelocity, _moveVelocityLast, movementDirectionMode, _isPower, _inputPowerChargeTime, _isPressAndHoldAndReleased, powerChargePhaseTimes, toDirectionLast, transform))
                            Debug.LogError("攻撃のモーション呼び出しの失敗");
                        // 移動方向に合わせて向きを変える
                        if (0f < moveVelocity.Value.magnitude)
                            transform.rotation = GetQuaternionDirection(movementDirectionMode, transform, _onTurn, toDirectionLast);
                    }
                    else
                        _inputPowerChargeTime.Value = 0f;
                });
            // 移動制御
            this.FixedUpdateAsObservable()
                .Subscribe(_ =>
                {
                    if (!_isBanMoveVelocity.Value)
                    {
                        // 加速（泳ぎ出す）状態ならSEとパーティクルを発生させる
                        if (!_isSwimming.Value &&
                            0f < moveVelocity.Value.magnitude)
                        {
                            _isSwimming.Value = true;
                        }
                        else if (moveVelocity.Value.magnitude == 0f)
                            _isSwimming.Value = false;
                        // 歩く走る挙動
                        rigidbody.AddForce(moveVelocity.Value * (forceMode.Equals(ForceMode2D.Force) ? 1f : Time.fixedDeltaTime), forceMode);
                        // Axis指定がゼロなら位置移動の減衰値を高めて止まり安くする
                        rigidbody.drag = !_isPlayingAction.Value &&
                            0f < moveVelocity.Value.magnitude &&
                            !CheckDrift(moveVelocity.Value, rigidbody.velocity, inputAngleFrontal) ?
                            0f : brake;
                    }
                    else
                        rigidbody.velocity = Vector3.zero;
                });
        }

        /// <summary>
        /// ドリフト状態かチェック
        /// ※有効角度の範囲を超えているならドリフト状態とみなす
        /// </summary>
        /// <param name="moveVelocity">移動制御入力</param>
        /// <param name="rigidbodyVelocity">オブジェクト自身のベロシティ</param>
        /// <param name="effectiveAngle">有効角度</param>
        /// <returns>ドリフト状態が有効</returns>
        private bool CheckDrift(Vector2 moveVelocity, Vector2 rigidbodyVelocity, float effectiveAngle)
        {
            return effectiveAngle < Vector3.Angle(moveVelocity, rigidbodyVelocity);
        }

        /// <summary>
        /// 攻撃のモーション
        /// </summary>
        /// <param name="isPlayingAction">攻撃モーション中</param>
        /// <param name="moveVelocity">移動ベロシティ</param>
        /// <param name="moveVelocityLast">最後に向いた移動ベロシティ</param>
        /// <param name="movementDirectionMode">移動向き</param>
        /// <param name="isPower">パワー状態</param>
        /// <param name="inputPowerChargeTime">パワーチャージ時間</param>
        /// <param name="isPressAndHoldAndReleased">押し続けて離す</param>
        /// <param name="powerChargePhaseTimes">パワーチャージの段階を変える時間</param>
        /// <param name="toDirectionLast">最後に参照された移動向き（デフォルトは右向き）</param>
        /// <param name="transform">トランスフォーム</param>
        /// <returns>成功／失敗</returns>
        private bool AttackMovement(BoolReactiveProperty isPlayingAction, Vector2ReactiveProperty moveVelocity, Vector2ReactiveProperty moveVelocityLast, EnumMovementDirectionMode movementDirectionMode, BoolReactiveProperty isPower, FloatReactiveProperty inputPowerChargeTime, BoolReactiveProperty isPressAndHoldAndReleased, float[] powerChargePhaseTimes, Vector3ReactiveProperty toDirectionLast, Transform transform)
        {
            try
            {
                if (!isPlayingAction.Value)
                    moveVelocity.Value = new Vector2(MainGameManager.Instance.InputSystemsOwner.InputPlayer.Moved.x, MainGameManager.Instance.InputSystemsOwner.InputPlayer.Moved.y) * moveSpeed;
                if (0 < moveVelocity.Value.magnitude)
                    // ベロシティ0だとパンチアクションが実行できないための対策
                    moveVelocityLast.Value = moveVelocity.Value;
                _moveVelocityReactiveProperty.Value = moveVelocity.Value;
                if (!isPower.Value)
                {
                    if (!isPlayingAction.Value &&
                        MainGameManager.Instance.InputSystemsOwner.InputPlayer.Attacked)
                    {
                        if (!PlayPunchAction(isPlayingAction, moveVelocityLast, movementDirectionMode, toDirectionLast, transform))
                            Debug.LogError("パンチアクション呼び出しの失敗");
                    }
                }
                else
                {
                    if (!isPressAndHoldAndReleased.Value)
                    {
                        if (MainGameManager.Instance.InputSystemsOwner.InputPlayer.Attacked &&
                            !MainGameManager.Instance.InputSystemsOwner.InputPlayer.Canceled)
                        {
                            inputPowerChargeTime.Value += Time.deltaTime;
                        }
                        else if (0f < inputPowerChargeTime.Value &&
                            !isPlayingAction.Value &&
                            powerChargePhaseTimes[2] < inputPowerChargeTime.Value &&
                            !MainGameManager.Instance.InputSystemsOwner.InputPlayer.Canceled)
                        {
                            inputPowerChargeTime.Value = 0f;
                            if (!PlayPunchAction(isPlayingAction, moveVelocityLast, movementDirectionMode, toDirectionLast, transform))
                                Debug.LogError("パンチアクション呼び出しの失敗");
                            isPressAndHoldAndReleased.Value = true;
                            // 再押下までの待機時間
                            DOVirtual.DelayedCall(isPressAndHoldAndReleasedCoolTime, () => isPressAndHoldAndReleased.Value = false);
                        }
                        else if (0f < inputPowerChargeTime.Value)
                        {
                            inputPowerChargeTime.Value = 0f;
                            if (!PlayPunchAction(isPlayingAction, moveVelocityLast, movementDirectionMode, toDirectionLast, transform))
                                Debug.LogError("パンチアクション呼び出しの失敗");
                        }
                        // float型の最大値丸め込み
                        if (1037f < inputPowerChargeTime.Value)
                            inputPowerChargeTime.Value = 1.1f;
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

        public bool AutoPlayPunchAction()
        {
            try
            {
                var isPlayingAction = new BoolReactiveProperty();
                PlayPunchAction(isPlayingAction, _moveVelocityLast, movementDirectionMode);

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// パンチアクション
        /// </summary>
        /// <param name="isPlayingAction">攻撃モーション中</param>
        /// <param name="moveVelocityLast">最後に向いた移動ベロシティ</param>
        /// <param name="movementDirectionMode">移動向き</param>
        /// <returns>成功／失敗</returns>
        private bool PlayPunchAction(BoolReactiveProperty isPlayingAction, Vector2ReactiveProperty moveVelocityLast, EnumMovementDirectionMode movementDirectionMode)
        {
            return PlayPunchAction(isPlayingAction, moveVelocityLast, movementDirectionMode, null, transform);
        }

        /// <summary>
        /// パンチアクション
        /// </summary>
        /// <param name="isPlayingAction">攻撃モーション中</param>
        /// <param name="moveVelocityLast">最後に向いた移動ベロシティ</param>
        /// <param name="movementDirectionMode">移動向き</param>
        /// <param name="toDirectionLast">最後に参照された移動向き（デフォルトは右向き）</param>
        /// <param name="transform">トランスフォーム</param>
        /// <returns>成功／失敗</returns>
        private bool PlayPunchAction(BoolReactiveProperty isPlayingAction, Vector2ReactiveProperty moveVelocityLast, EnumMovementDirectionMode movementDirectionMode, Vector3ReactiveProperty toDirectionLast, Transform transform)
        {
            try
            {
                isPlayingAction.Value = true;
                switch (enumAttackMotionMode)
                {
                    case EnumAttackMotionMode.OneAttack:
                        var point = GetNormalized(moveVelocityLast.Value, movementDirectionMode) * attackDistance;
                        point.x = Mathf.Abs(point.x) * transform.rotation.y == 0f ? 1f : -1f;
                        var t = transform.DOPunchPosition(point,
                            attackDuration, 1, 1f)
                            .OnComplete(() => isPlayingAction.Value = false);
                        if (toDirectionLast != null)
                            SetTargetAttackingWorldPosition(transform,
                                point * (toDirectionLast.Value.Equals(Vector3.right) ? 1f : -1f) * attackingDistance);
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

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// 攻撃先のターゲットへワールド座標をセット
        /// DOPunchPositionはローカル座標を指定するため、ローカル座標を返却
        /// </summary>
        /// <param name="transform">トランスフォーム</param>
        /// <param name="targetAttackingPosition">攻撃先のターゲット（ローカル座標）</param>
        /// <returns>攻撃先のターゲット（ローカル座標）</returns>
        private Vector3 SetTargetAttackingWorldPosition(Transform transform, Vector3 targetAttackingPosition)
        {
            _targetAttackingPosition = transform.TransformPoint(targetAttackingPosition);
            return targetAttackingPosition;
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
        /// <param name="onTurn">ターン実行状態</param>
        /// <param name="toDirectionLast">最後に参照された移動向き</param>
        private Quaternion GetQuaternionDirection(EnumMovementDirectionMode mode, Transform transform, BoolReactiveProperty onTurn, Vector3ReactiveProperty toDirectionLast)
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

                    // 左右の切り替えが発生した場合は都度更新
                    if (!toDirectionLast.Value.Equals(toDirection))
                    {
                        onTurn.Value = true;
                        toDirectionLast.Value = toDirection;
                    }

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

        public bool SetIsPower(bool enabled)
        {
            try
            {
                // パワーシェル実装時に使用する
                _isPower.Value = enabled;
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetOnTrurn(bool enabled)
        {
            try
            {
                _onTurn.Value = enabled;
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetIsBanMoveVelocity(bool unactive)
        {
            try
            {
                _isBanMoveVelocity.Value = unactive;
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetInputBan(bool unactive, bool isDelayMode)
        {
            try
            {
                if (!isDelayMode)
                    _inputBan = unactive;
                else
                    DOVirtual.DelayedCall(delayDoDuration, () => _inputBan = unactive);

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
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

    public interface IPlayerModel
    {
        /// <summary>
        /// 操作禁止フラグをセット
        /// </summary>
        /// <param name="unactive">許可／禁止</param>
        /// <returns>成功／失敗</returns>
        public bool SetInputBan(bool unactive);

        /// <summary>
        /// 操作禁止フラグをセット
        /// </summary>
        /// <param name="unactive">許可／禁止</param>
        /// <param name="isDelayMode">遅延実行（つつく入力の対策）</param>
        /// <returns>成功／失敗</returns>
        public bool SetInputBan(bool unactive, bool isDelayMode);

        /// <summary>
        /// パワー状態をセット
        /// </summary>
        /// <param name="enabled">有効</param>
        /// <returns>成功／失敗</returns>
        public bool SetIsPower(bool enabled);

        /// <summary>
        /// ターン状態をセット
        /// </summary>
        /// <param name="enabled">有効</param>
        /// <returns>成功／失敗</returns>
        public bool SetOnTrurn(bool enabled);

        /// <summary>
        /// 移動禁止禁止フラグをセット
        /// </summary>
        /// <param name="unactive">許可／禁止</param>
        /// <returns>成功／失敗</returns>
        public bool SetIsBanMoveVelocity(bool unactive);
    }
}
