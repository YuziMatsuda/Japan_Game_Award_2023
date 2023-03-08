using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Common;
using UniRx;
using System.Linq;
using DG.Tweening;

namespace Main.Model
{
    /// <summary>
    /// モデル
    /// 始点／スタートノード／ゴールノードの親クラス
    /// オブジェクトごとにコライダーは複数存在する想定のためRequireComponentでコライダー有無はチェックしない
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    //[RequireComponent(typeof(PivotConfig))]
    public class AbstractPivotModel : MonoBehaviour
    {
        ///// <summary>ターンアニメーション実行中</summary>
        //private readonly BoolReactiveProperty _isTurning = new BoolReactiveProperty();
        ///// <summary>ターンアニメーション実行中</summary>
        //public IReactiveProperty<bool> IsTurning => _isTurning;
        /// <summary>接触対象のオブジェクトタグ</summary>
        [SerializeField] protected string[] tags = { ConstTagNames.TAG_NAME_ATTACK_TRIGGER };
        /// <summary>トランスフォーム</summary>
        protected Transform _transform;
        ///// <summary>方角モード</summary>
        //private readonly IntReactiveProperty enumDirectionMode = new IntReactiveProperty();
        ///// <summary>ターンアニメーション時間</summary>
        //[SerializeField] private float turnDuration = .5f;
        ///// <summary>方角モードのベクター配列</summary>
        //[SerializeField] private Vector3[] vectorDirectionModes = { new Vector3(0, 0, 0f), new Vector3(0, 0, -90f), new Vector3(0, 0, 180f), new Vector3(0, 0, 90f) };
        /// <summary>方角モード二次元配列</summary>
        protected int[][] _intDirectionModes = { new int[3], new int[3], new int[3] };
        /// <summary>方角モード二次元配列</summary>
        public int[][] IntDirectionModes => _intDirectionModes;
        ///// <summary>回転方角モード</summary>
        //[SerializeField] private EnumSpinDirectionMode isSpinDirectionMode = EnumSpinDirectionMode.Positive;

        private void Reset()
        {
            //var enumDirectionModeDefault = GetComponent<PivotConfig>().EnumDirectionModeDefault;
            //transform.localEulerAngles = vectorDirectionModes[(int)enumDirectionModeDefault];
        }

        protected virtual void Start()
        {
            if (_transform == null)
                _transform = transform;
            //enumDirectionMode.Value = (int)GetComponent<PivotConfig>().EnumDirectionModeDefault;
            //enumDirectionMode.ObserveEveryValueChanged(x => x.Value)
            //    .Subscribe(x =>
            //    {
            //        _intDirectionModes = GetIntDirectionModes((EnumDirectionMode)x);
            //    });
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            //if (0 < tags.Where(q => collision.CompareTag(q)).Select(q => q).ToArray().Length &&
            //    !_isTurning.Value)
            //{
            //    _isTurning.Value = true;
            //    var turnValue = GetTurnValue(_transform, collision.ClosestPoint(_transform.position), isSpinDirectionMode);
            //    if (turnValue == 0)
            //        Debug.LogError("ターン加算値の取得呼び出しの失敗");
            //    enumDirectionMode.Value = (int)GetAjustedEnumDirectionMode((EnumDirectionMode)enumDirectionMode.Value, turnValue);
            //    _transform.DOLocalRotate(vectorDirectionModes[enumDirectionMode.Value], turnDuration)
            //        .OnComplete(() => _isTurning.Value = false);
            //}
        }

        /// <summary>
        /// ターン加算値の取得
        /// </summary>
        /// <param name="transform">トランスフォーム</param>
        /// <param name="target">ターゲットの座標</param>
        /// <returns>ターン加算値</returns>
        private int GetTurnValue(Transform transform, Vector2 target, EnumSpinDirectionMode enumSpinDirectionMode)
        {
            try
            {
                switch (enumSpinDirectionMode)
                {
                    case EnumSpinDirectionMode.Auto:
                        if (transform.position.y == target.y ||
                               transform.position.x == target.x)
                            throw new System.Exception("同一座標位置は例外エラー");

                        if (transform.position.x < target.x)
                        {
                            // 上かつ、右からの衝突は反時計回り
                            // 下かつ、右からの衝突は時計回り
                            return transform.position.y < target.y ? -1 : 1;
                        }
                        else
                        {
                            // 上かつ、左からの衝突は時計回り
                            // 下かつ、左からの衝突は反時計回り
                            return transform.position.y < target.y ? 1 : -1;
                        }
                    case EnumSpinDirectionMode.Positive:
                        return 1;
                    case EnumSpinDirectionMode.Negative:
                        return -1;
                    default:
                        throw new System.Exception("例外エラー");
                }

            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return 0;
            }
        }

        /// <summary>
        /// 方角モードをInt二次元配列へ変換して取得
        /// </summary>
        /// <param name="enumDirectionMode">方角モード</param>
        /// <returns>方角モード（Int二次元配列）</returns>
        private int[][] GetIntDirectionModes(EnumDirectionMode enumDirectionMode)
        {
            try
            {
                int[][] directionModesArray = { new int[3], new int[3], new int[3] };

                switch (enumDirectionMode)
                {
                    case EnumDirectionMode.Up:
                        directionModesArray[0] = new int[3] { 0, 1, 0 };
                        directionModesArray[1] = new int[3] { 0, 1, 0 };
                        directionModesArray[2] = new int[3] { 0, 0, 0 };

                        return directionModesArray;
                    case EnumDirectionMode.Right:
                        directionModesArray[0] = new int[3] { 0, 0, 0 };
                        directionModesArray[1] = new int[3] { 0, 1, 1 };
                        directionModesArray[2] = new int[3] { 0, 0, 0 };

                        return directionModesArray;
                    case EnumDirectionMode.Down:
                        directionModesArray[0] = new int[3] { 0, 0, 0 };
                        directionModesArray[1] = new int[3] { 0, 1, 0 };
                        directionModesArray[2] = new int[3] { 0, 1, 0 };

                        return directionModesArray;
                    case EnumDirectionMode.Left:
                        directionModesArray[0] = new int[3] { 0, 0, 0 };
                        directionModesArray[1] = new int[3] { 1, 1, 0 };
                        directionModesArray[2] = new int[3] { 0, 0, 0 };

                        return directionModesArray;
                    default:
                        throw new System.Exception("例外エラー");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }

        /// <summary>
        /// 方角モード変換してを取得
        /// 限界値を超えた値はアジャストする
        /// </summary>
        /// <param name="enumDirectionMode">方角モード</param>
        /// <param name="addend">足す値</param>
        /// <returns>方角モード（正常値）</returns>
        private EnumDirectionMode GetAjustedEnumDirectionMode(EnumDirectionMode enumDirectionMode, int addend)
        {
            if ((enumDirectionMode + addend) < 0)
            {
                enumDirectionMode = EnumDirectionMode.Left;
                return enumDirectionMode;
            }
            else if (EnumDirectionMode.Left < (enumDirectionMode + addend))
            {
                enumDirectionMode = EnumDirectionMode.Up;
                return enumDirectionMode;
            }
            else
            {
                // 境界値チェックのため処理無しでも問題なし
                return enumDirectionMode + addend;
            }
        }
    }
}
