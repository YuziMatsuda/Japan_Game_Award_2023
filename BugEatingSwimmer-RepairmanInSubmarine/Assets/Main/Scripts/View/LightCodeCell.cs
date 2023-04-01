using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Main.Model;
using Main.Common;

namespace Main.View
{
    /// <summary>
    /// コード（明）
    /// </summary>
    public class LightCodeCell : ShadowCodeCellParent, IShadowCodeCell
    {
        /// <summary>描画対象のスプライト配列</summary>
        [SerializeField] private LightCodeCellSprite[] lightCodeCellSprites;
        /// <summary>コード</summary>
        [SerializeField] private ShadowCode shadowCode;

        private void Reset()
        {
            List<LightCodeCellSprite> lightCodeCellSpriteList = new List<LightCodeCellSprite>();
            lightCodeCellSpriteList.Add(transform.GetChild(0).GetComponent<LightCodeCellSprite>());
            lightCodeCellSpriteList.Add(transform.GetChild(1).GetComponent<LightCodeCellSprite>());
            foreach (Transform item in transform.GetChild(2))
            {
                lightCodeCellSpriteList.Add(item.GetComponent<LightCodeCellSprite>());
            }
            lightCodeCellSprites = lightCodeCellSpriteList.ToArray();
            shadowCode = transform.parent.GetChild(0).GetComponent<ShadowCode>();
        }

        /// <summary>
        /// 非表示の向きはライト点灯させない
        /// </summary>
        /// <param name="enumDirectionMode">方向モード</param>
        /// <returns>無効状態か否か</returns>
        private bool IsDisabledCodeCellsDirection(EnumDirectionMode enumDirectionMode)
        {
            return (!shadowCode.CodeCellsUp.gameObject.activeSelf && EnumDirectionMode.Up.Equals(enumDirectionMode)) ||
                (!shadowCode.CodeCellsRight.gameObject.activeSelf && EnumDirectionMode.Right.Equals(enumDirectionMode)) ||
                (!shadowCode.CodeCellsDown.gameObject.activeSelf && EnumDirectionMode.Down.Equals(enumDirectionMode)) ||
                (!shadowCode.CodeCellsLeft.gameObject.activeSelf && EnumDirectionMode.Left.Equals(enumDirectionMode));
        }

        public IEnumerator PlayLightAnimation(IObserver<bool> observer, EnumDirectionMode enumDirectionMode)
        {
            var idx = 0;
            if (IsDisabledCodeCellsDirection(enumDirectionMode))
            {
                // ライトが向いた先が非表示ならセル点灯させずに購読完了とする
                observer.OnNext(true);
            }
            else
            {
                // 支点
                Observable.FromCoroutine<bool>(observer => lightCodeCellSprites[idx++].PlayLightAnimation(observer, enumDirectionMode))
                    .Subscribe(_ =>
                    {
                        // ヘッド
                        Observable.FromCoroutine<bool>(observer => lightCodeCellSprites[idx++].PlayLightAnimation(observer, enumDirectionMode))
                            .Subscribe(_ =>
                            {
                                // セル
                                Observable.FromCoroutine<bool>(observer => lightCodeCellSprites[idx++].PlayLightAnimation(observer, enumDirectionMode))
                                    .Subscribe(_ =>
                                    {
                                        // セル
                                        Observable.FromCoroutine<bool>(observer => lightCodeCellSprites[idx++].PlayLightAnimation(observer, enumDirectionMode))
                                            .Subscribe(_ =>
                                            {
                                                // セル
                                                Observable.FromCoroutine<bool>(observer => lightCodeCellSprites[idx++].PlayLightAnimation(observer, enumDirectionMode))
                                                    .Subscribe(_ =>
                                                    {
                                                        // セル
                                                        Observable.FromCoroutine<bool>(observer => lightCodeCellSprites[idx++].PlayLightAnimation(observer, enumDirectionMode))
                                                            .Subscribe(_ =>
                                                            {
                                                                observer.OnNext(true);
                                                            })
                                                            .AddTo(gameObject);
                                                    })
                                                    .AddTo(gameObject);
                                            })
                                            .AddTo(gameObject);
                                    })
                                    .AddTo(gameObject);
                            })
                            .AddTo(gameObject);
                    })
                    .AddTo(gameObject);
            }

            yield return null;
        }

        public IEnumerator PlaySpinAnimation(IObserver<bool> observer, Vector3 vectorDirectionMode)
        {
            throw new NotImplementedException();
        }

        public bool SetSpinDirection(Vector3 vectorDirectionMode)
        {
            try
            {
                if (_transform == null)
                    _transform = transform;
                _transform.localEulerAngles = vectorDirectionMode;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetAlphaOff()
        {
            try
            {
                foreach (var item in lightCodeCellSprites)
                {
                    if (!item.SetAlphaOff())
                        throw new Exception("アルファ値をセット呼び出しの失敗");
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool InitializeLight(EnumDirectionMode enumDirectionMode)
        {
            try
            {
                if (IsDisabledCodeCellsDirection(enumDirectionMode))
                {
                    // ライトが向いた先が非表示ならセル点灯させない
                    return true;
                }
                else
                {
                    foreach (var item in lightCodeCellSprites)
                    {
                        if (!item.InitializeLight(enumDirectionMode))
                            throw new Exception("ライト点灯初期設定呼び出しの失敗");
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
    }
}
