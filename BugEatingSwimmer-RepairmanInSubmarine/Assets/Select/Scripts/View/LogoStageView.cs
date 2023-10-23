using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Select.Common;
using UniRx;
using UniRx.Triggers;

namespace Select.View
{
    /// <summary>
    /// ビュー
    /// ロゴステージ
    /// </summary>
    public class LogoStageView : MonoBehaviour, ILogoStageView
    {
        /// <summary>ボディのテキスト</summary>
        [SerializeField] private BodyText bodyText;
        /// <summary>ボディのテキスト配列</summary>
        [SerializeField] private StageStatus stageStatus;
        /// <summary>ボディのイメージ</summary>
        [SerializeField] private BodyImage bodyImage;
        /// <summary>イメージ</summary>
        [SerializeField] private Image image;
        /// <summary>背景フレームイメージ</summary>
        [SerializeField] private FadeImageView backgroundFrame;
        /// <summary>アクティブなステージか（対象ページか）</summary>
        public bool IsVisibledInPage => transform.parent.GetComponent<PageView>().IsVisibled;
        /// <summary>アニメーション終了時間</summary>
        [SerializeField] private float[] durations = { .85f };

        private void Reset()
        {
            bodyText = GetComponentInChildren<BodyText>();
            stageStatus = GetComponentInChildren<StageStatus>();
            bodyImage = GetComponentInChildren<BodyImage>();
            image = GetComponent<Image>();
            backgroundFrame = GetComponentInChildren<FadeImageView>();
        }

        public bool RenderDisableMark()
        {
            try
            {
                if (!bodyText.SetTextEnabled(false))
                    throw new System.Exception("テキストのステータスを変更呼び出しの失敗");
                if (!stageStatus.SetTextEnabled(false))
                    throw new System.Exception("テキストのステータスを変更呼び出しの失敗");
                if (!SetRaycastTarget(false))
                    throw new System.Exception("レイキャストの接触可否を変更呼び出しの失敗");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// レイキャストの接触可否を変更
        /// </summary>
        /// <param name="isEnabled">有効／無効</param>
        /// <returns>有効／無効</returns>
        private bool SetRaycastTarget(bool isEnabled)
        {
            try
            {
                image.raycastTarget = isEnabled;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool RenderClearMark()
        {
            try
            {
                SetClear();
                if (!bodyImage.SetImageEnabled(false))
                    throw new System.Exception("イメージのステータスを変更呼び出しの失敗");
                
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// クリアをセット
        /// </summary>
        private void SetClear()
        {
            if (!stageStatus.SetTextMessage(EnumStageStatusMessage.Clear))
                throw new System.Exception("テキストメッセージを表示呼び出しの失敗");
            if (!stageStatus.SetColorTextMessage(EnumStageStatusMessage.Clear))
                throw new System.Exception("テキストメッセージカラーをセット呼び出しの失敗");
        }

        public bool RenderEnabled()
        {
            try
            {
                SetError();
                if (!bodyImage.SetImageEnabled(false))
                    throw new System.Exception("イメージのステータスを変更呼び出しの失敗");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// エラーをセット
        /// </summary>
        private void SetError()
        {
            if (!stageStatus.SetTextMessage(EnumStageStatusMessage.Error))
                throw new System.Exception("テキストメッセージを表示呼び出しの失敗");
            if (!stageStatus.SetColorTextMessage(EnumStageStatusMessage.Error))
                throw new System.Exception("テキストメッセージカラーをセット呼び出しの失敗");
        }

        public IEnumerator PlayRenderEnableBackgroundFrame(System.IObserver<bool> observer)
        {
            Observable.FromCoroutine<bool>(observer => backgroundFrame.PlayFadeAnimation(observer, EnumFadeState.Close))
                .Subscribe(_ => observer.OnNext(true))
                .AddTo(gameObject);
            yield return null;
        }

        public IEnumerator PlayRenderDisableBackgroundFrame(System.IObserver<bool> observer)
        {
            Observable.FromCoroutine<bool>(observer => backgroundFrame.PlayFadeAnimation(observer, EnumFadeState.Open))
                .Subscribe(_ => observer.OnNext(true))
                .AddTo(gameObject);
            yield return null;
        }

        public bool SetRenderEnableBackgroundFrame()
        {
            try
            {
                if (!backgroundFrame.SetColorSpriteRenderer(EnumFadeState.Close))
                    throw new System.Exception("カラーを設定呼び出しの失敗");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetRenderDisableBackgroundFrame()
        {
            try
            {
                if (!backgroundFrame.SetColorSpriteRenderer(EnumFadeState.Open))
                    throw new System.Exception("カラーを設定呼び出しの失敗");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool PlayRenderDisableMark()
        {
            try
            {
                var target = transform;
                if (!SelectGameManager.Instance.ParticleSystemsOwner.PlayParticleSystems(GetInstanceID(), EnumParticleSystemsIndex.BugAura, target.position))
                    throw new System.Exception("指定されたパーティクルシステムを再生する フェードアウトモード呼び出しの失敗");
                var particle = SelectGameManager.Instance.ParticleSystemsOwner.GetParticleSystemsTransform(GetInstanceID(), EnumParticleSystemsIndex.BugAura);
                this.UpdateAsObservable()
                    .Subscribe(_ => particle.transform.position = target.position);

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public IEnumerator PlayRenderEnabled(System.IObserver<bool> observer)
        {
            try
            {
                if (IsVisibledInPage)
                {
                    SetAll();
                    var completedAnimCnt = new IntReactiveProperty(0);
                    completedAnimCnt.ObserveEveryValueChanged(x => x.Value)
                        .Subscribe(x =>
                        {
                            // 3つのアニメーションの完了を待機する
                            if (2 < x)
                                observer.OnNext(true);
                        });
                    SetError();
                    PlayAllAnimation(completedAnimCnt);
                }
                else
                    observer.OnNext(true);
            }
            catch (System.Exception e)
            {
                observer.OnNext(true);
                throw e;
            }
            yield return null;
        }

        /// <summary>
        /// 全てのアニメーションを再生
        /// </summary>
        /// <param name="completedAnimCnt">完了済みアニメーション数</param>
        private void PlayAllAnimation(IntReactiveProperty completedAnimCnt)
        {
            Observable.FromCoroutine<bool>(observer => stageStatus.PlayFadeColorTextMessage(observer, EnumFadeState.Open))
                .Subscribe(_ => completedAnimCnt.Value++)
                .AddTo(gameObject);
            Observable.FromCoroutine<bool>(observer => bodyImage.PlayFadeAnimation(observer))
                .Subscribe(_ =>
                {
                    if (!bodyImage.SetImageEnabled(false))
                        throw new System.Exception("イメージのステータスを変更呼び出しの失敗");
                    completedAnimCnt.Value++;
                })
                .AddTo(gameObject);
            if (!SelectGameManager.Instance.ParticleSystemsOwner.PlayParticleSystems(GetInstanceID(), EnumParticleSystemsIndex.BugAura, transform.position))
                throw new System.Exception("指定されたパーティクルシステムを再生する フェードアウトモード呼び出しの失敗");
            DOVirtual.DelayedCall(durations[0], () =>
            {
                if (!SelectGameManager.Instance.ParticleSystemsOwner.StopParticleSystems(GetInstanceID(), EnumParticleSystemsIndex.BugAura))
                    Debug.LogError("指定されたパーティクルシステムを再生する フェードアウトモード呼び出しの失敗");
                completedAnimCnt.Value++;
            });
        }

        /// <summary>
        /// テキストとレイキャストを有効にする
        /// </summary>
        private void SetAll()
        {
            if (!bodyText.SetTextEnabled(true))
                throw new System.Exception("テキストのステータスを変更呼び出しの失敗");
            if (!stageStatus.SetTextEnabled(true))
                throw new System.Exception("テキストのステータスを変更呼び出しの失敗");
            if (!SetRaycastTarget(true))
                throw new System.Exception("レイキャストの接触可否を変更呼び出しの失敗");
        }

        public IEnumerator PlayRenderClearMark(System.IObserver<bool> observer)
        {
            try
            {
                if (IsVisibledInPage)
                {
                    SetAll();
                    var completedAnimCnt = new IntReactiveProperty(0);
                    completedAnimCnt.ObserveEveryValueChanged(x => x.Value)
                        .Subscribe(x =>
                        {
                            // 3つのアニメーションの完了を待機する
                            if (2 < x)
                                observer.OnNext(true);
                        });
                    SetClear();
                    PlayAllAnimation(completedAnimCnt);
                }
                else
                    observer.OnNext(true);
            }
            catch (System.Exception e)
            {
                observer.OnNext(true);
                throw e;
            }
            yield return null;
        }
    }

    /// <summary>
    /// ビュー
    /// ロゴステージ
    /// インターフェース
    /// </summary>
    public interface ILogoStageView : ISelectContentsViewParent
    {
        /// <summary>
        /// 選択不可マークを表示
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool RenderDisableMark();
        /// <summary>
        /// 選択可表示
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool RenderEnabled();
        /// <summary>
        /// 選択不可マークを表示演出を再生
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool PlayRenderDisableMark();
        /// <summary>
        /// 選択可表示演出を再生
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayRenderEnabled(System.IObserver<bool> observer);
        /// <summary>
        /// クリア済みマークを表示
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool RenderClearMark();
        /// <summary>
        /// クリア済みマークを表示
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayRenderClearMark(System.IObserver<bool> observer);
    }
}
