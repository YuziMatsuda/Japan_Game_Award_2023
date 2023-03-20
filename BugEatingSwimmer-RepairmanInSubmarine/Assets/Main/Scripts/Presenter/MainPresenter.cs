using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Common;
using Main.View;
using Main.Model;
using UniRx;
using UniRx.Triggers;
using Main.Audio;
using System.Linq;
using Main.InputSystem;
using System.Threading.Tasks;
using Main.Test;

namespace Main.Presenter
{
    /// <summary>
    /// プレゼンタ
    /// セレクトシーン
    /// </summary>
    public class MainPresenter : MonoBehaviour, IMainGameManager
    {
        /// <summary>ポーズ画面のビュー</summary>
        [SerializeField] private PauseView pauseView;
        /// <summary>ポーズボタンのビュー</summary>
        [SerializeField] private GamePauseView gamePauseView;
        /// <summary>ポーズボタンのモデル</summary>
        [SerializeField] private GamePauseModel gamePauseModel;
        /// <summary>クリア画面のビュー</summary>
        [SerializeField] private ClearView clearView;
        /// <summary>ステージクリアロゴのビュー</summary>
        [SerializeField] private StageClearView stageClearView;
        /// <summary>クリア画面のメニュー描画までの時間</summary>
        [SerializeField] private int clearContentsRenderingDelayTime = 3000;
        /// <summary>次のステージへ進むのビュー</summary>
        [SerializeField] private GameProceedButtonView gameProceedButtonView;
        /// <summary>次のステージへ進むのモデル</summary>
        [SerializeField] private GameProceedButtonModel gameProceedButtonModel;
        /// <summary>もう一度遊ぶボタンのビュー</summary>
        [SerializeField] private GameRetryButtonView gameRetryButtonView;
        /// <summary>もう一度遊ぶボタンのモデル</summary>
        [SerializeField] private GameRetryButtonModel gameRetryButtonModel;
        /// <summary>ステージ選択へ戻るのビュー</summary>
        [SerializeField] private GameSelectButtonView gameSelectButtonView;
        /// <summary>ステージ選択へ戻るのモデル</summary>
        [SerializeField] private GameSelectButtonModel gameSelectButtonModel;
        /// <summary>カーソルのビュー</summary>
        [SerializeField] private CursorIconView cursorIconView;
        /// <summary>カーソルのモデル</summary>
        [SerializeField] private CursorIconModel cursorIconModel;
        /// <summary>ショートカットキー押下ゲージのビュー</summary>
        [SerializeField] private PushTimeGageView[] pushTimeGageViews;
        /// <summary>ショートカットキー押下ゲージのビュー</summary>
        [SerializeField] private GameManualScrollView gameManualScrollView;
        /// <summary>遊び方確認ページのビュー</summary>
        [SerializeField] private GameManualViewPageView[] gameManualViewPageViews;
        /// <summary>遊び方確認ページのモデル</summary>
        [SerializeField] private GameManualViewPageModel[] gameManualViewPageModels;
        /// <summary>移動操作ガイドのビュー</summary>
        [SerializeField] private MoveGuideView moveGuideView;
        /// <summary>ジャンプ操作ガイドのビュー</summary>
        [SerializeField] private JumpGuideView jumpGuideView;
        /// <summary>フェードのビュー</summary>
        [SerializeField] private FadeImageView fadeImageView;
        /// <summary>フェードのモデル</summary>
        [SerializeField] private FadeImageModel fadeImageModel;
        /// <summary>プレイヤー開始位置のビュー</summary>
        [SerializeField] private PlayerStartPointView playerStartPointView;
        /// <summary>セーフゾーンのモデル</summary>
        [SerializeField] private SafeZoneModel safeZoneModel;
        /// <summary>ゴールポイントのビュー</summary>
        [SerializeField] private GoalPointView goalPointView;
        /// <summary>ゴールポイントのモデル</summary>
        [SerializeField] private GoalPointModel goalPointModel;
        /// <summary>プレイヤーのビュー</summary>
        [SerializeField] private PlayerView playerView;
        /// <summary>プレイヤーのモデル</summary>
        [SerializeField] private PlayerModel playerModel;
        /// <summary>移動先にポインタ表示のビュー</summary>
        [SerializeField] private TargetPointerView targetPointerView;

        private void Reset()
        {
            pauseView = GameObject.Find("Pause").GetComponent<PauseView>();
            gamePauseView = GameObject.Find("GamePause").GetComponent<GamePauseView>();
            gamePauseModel = GameObject.Find("GamePause").GetComponent<GamePauseModel>();
            clearView = GameObject.Find("Clear").GetComponent<ClearView>();
            stageClearView = GameObject.Find("StageClear").GetComponent<StageClearView>();
            gameProceedButtonView = GameObject.Find("GameProceedButton").GetComponent<GameProceedButtonView>();
            gameProceedButtonModel = GameObject.Find("GameProceedButton").GetComponent<GameProceedButtonModel>();
            gameRetryButtonView = GameObject.Find("GameRetryButton").GetComponent<GameRetryButtonView>();
            gameRetryButtonModel = GameObject.Find("GameRetryButton").GetComponent<GameRetryButtonModel>();
            gameSelectButtonView = GameObject.Find("GameSelectButton").GetComponent<GameSelectButtonView>();
            gameSelectButtonModel = GameObject.Find("GameSelectButton").GetComponent<GameSelectButtonModel>();
            cursorIconView = GameObject.Find("CursorIcon").GetComponent<CursorIconView>();
            cursorIconModel = GameObject.Find("CursorIcon").GetComponent<CursorIconModel>();
            var ptGameIdx = 0;
            pushTimeGageViews = new PushTimeGageView[3];
            pushTimeGageViews[ptGameIdx++] = GameObject.Find("GULPushTimeGage").GetComponent<PushTimeGageView>();
            pushTimeGageViews[ptGameIdx++] = GameObject.Find("GSLPushTimeGage").GetComponent<PushTimeGageView>();
            pushTimeGageViews[ptGameIdx++] = GameObject.Find("GCLPushTimeGage").GetComponent<PushTimeGageView>();
            gameManualScrollView = GameObject.Find("GameManualScroll").GetComponent<GameManualScrollView>();
            var gmvPageVIdx = 0;
            var gmvPageMIdx = 0;
            gameManualViewPageViews = new GameManualViewPageView[4];
            gameManualViewPageModels = new GameManualViewPageModel[4];
            gameManualViewPageViews[gmvPageVIdx++] = GameObject.Find("GameManualViewPage_1").GetComponent<GameManualViewPageView>();
            gameManualViewPageModels[gmvPageMIdx++] = GameObject.Find("GameManualViewPage_1").GetComponent<GameManualViewPageModel>();
            gameManualViewPageViews[gmvPageVIdx++] = GameObject.Find("GameManualViewPage_2").GetComponent<GameManualViewPageView>();
            gameManualViewPageModels[gmvPageMIdx++] = GameObject.Find("GameManualViewPage_2").GetComponent<GameManualViewPageModel>();
            gameManualViewPageViews[gmvPageVIdx++] = GameObject.Find("GameManualViewPage_3").GetComponent<GameManualViewPageView>();
            gameManualViewPageModels[gmvPageMIdx++] = GameObject.Find("GameManualViewPage_3").GetComponent<GameManualViewPageModel>();
            gameManualViewPageViews[gmvPageVIdx++] = GameObject.Find("GameManualViewPage_4").GetComponent<GameManualViewPageView>();
            gameManualViewPageModels[gmvPageMIdx++] = GameObject.Find("GameManualViewPage_4").GetComponent<GameManualViewPageModel>();
            moveGuideView = GameObject.Find("MoveGuide").GetComponent<MoveGuideView>();
            jumpGuideView = GameObject.Find("JumpGuide").GetComponent<JumpGuideView>();
            fadeImageView = GameObject.Find("FadeImage").GetComponent<FadeImageView>();
            fadeImageModel = GameObject.Find("FadeImage").GetComponent<FadeImageModel>();
        }

        public void OnStart()
        {
            // 初期設定
            pauseView.gameObject.SetActive(false);
            gameProceedButtonView.gameObject.SetActive(false);
            gameRetryButtonView.gameObject.SetActive(false);
            gameSelectButtonView.gameObject.SetActive(false);
            cursorIconView.gameObject.SetActive(false);
            clearView.gameObject.SetActive(false);
            gameManualScrollView.gameObject.SetActive(false);
            moveGuideView.SetAlpha(EnumFadeState.Close);
            moveGuideView.gameObject.SetActive(false);
            jumpGuideView.SetAlpha(EnumFadeState.Close);
            jumpGuideView.gameObject.SetActive(false);

            MainGameManager.Instance.AudioOwner.OnStartAndPlayBGM();
            // T.B.D ステージ開始演出
            var isStartDirectionCompleted = new IntReactiveProperty();
            // シーン読み込み時のアニメーション
            Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Open))
                .Subscribe(_ =>
                {
                    // T.B.D ステージ開始演出
                    isStartDirectionCompleted.Value++;
                    Debug.Log($"フェード完了:[{isStartDirectionCompleted.Value}]");
                })
                .AddTo(gameObject);
            // ショートカットキーの押下（有効／無効）状態
            var isInputUIActionsEnabled = new BoolReactiveProperty();
            // ポーズボタンの押下（有効／無効）状態
            var isInputUIPausedEnabled = new BoolReactiveProperty();
            // T.B.D ステージ開始演出
            isStartDirectionCompleted.ObserveEveryValueChanged(x => x.Value)
                .Where(x => x == 2)
                .Subscribe(_ =>
                {
                    // プレイヤーを開始ポイントへ生成
                    if (playerStartPointView != null)
                        if (!playerStartPointView.InstancePlayer())
                            Debug.LogError("プレイヤー生成呼び出しの失敗");
                    isInputUIActionsEnabled.Value = true;
                    isInputUIPausedEnabled.Value = true;
                });
            // 実行中のショートカットキーのアクション
            var inProcess = EnumShortcuActionMode.None;
            // ポーズ押下
            var inputUIPausedState = new BoolReactiveProperty();
            inputUIPausedState.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    // ポーズ画面が閉じている　かつ、
                    // クリア画面が閉じている
                    if (x &&
                        !pauseView.gameObject.activeSelf &&
                        !clearView.gameObject.activeSelf)
                    {
                        MainGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_play_open);
                        // 遊び方確認ページを開いているなら閉じる
                        if (gameManualScrollView.gameObject.activeSelf)
                        {
                            if (!gameManualViewPageModels[(int)EnumShortcuActionMode.CheckAction].SetButtonEnabled(false))
                                Debug.LogError("ボタン有効／無効切り替え呼び出しの失敗");
                            if (!gameManualViewPageModels[(int)EnumShortcuActionMode.CheckAction].SetEventTriggerEnabled(false))
                                Debug.LogError("イベント有効／無効切り替え呼び出しの失敗");
                            // 遊び方を確認クローズのアニメーション
                            Observable.FromCoroutine<bool>(observer => gameManualScrollView.PlayCloseAnimation(observer))
                                .Subscribe(_ =>
                                {
                                    gameManualScrollView.gameObject.SetActive(false);
                                    inProcess = EnumShortcuActionMode.None;
                                })
                                .AddTo(gameObject);
                        }
                        pauseView.gameObject.SetActive(true);
                        gamePauseModel.SetSelectedGameObject();
                    }
                });
            // ポーズ画面表示中の操作
            gamePauseModel.EventState.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    switch ((EnumEventCommand)x)
                    {
                        case EnumEventCommand.Default:
                            // 処理無し
                            break;
                        case EnumEventCommand.Selected:
                            // 処理無し
                            break;
                        case EnumEventCommand.DeSelected:
                            // 処理無し
                            break;
                        case EnumEventCommand.Submited:
                            // 処理無し
                            break;
                        case EnumEventCommand.Canceled:
                            MainGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_cancel);
                            if (!gamePauseModel.SetButtonEnabled(false))
                                Debug.LogError("ボタン有効／無効切り替え呼び出しの失敗");
                            if (!gamePauseModel.SetEventTriggerEnabled(false))
                                Debug.LogError("イベント有効／無効切り替え呼び出しの失敗");
                            // ポーズ画面クローズのアニメーション
                            Observable.FromCoroutine<bool>(observer => pauseView.PlayCloseAnimation(observer))
                                .Subscribe(_ =>
                                {
                                    pauseView.gameObject.SetActive(false);
                                })
                                .AddTo(gameObject);
                            break;
                        default:
                            Debug.LogWarning("例外ケース");
                            break;
                    }
                });
            // クリア画面表示のため、ゴール到達のフラグ更新
            var currentStageDic = MainGameManager.Instance.SceneOwner.GetSystemCommonCash();
            var mainSceneStagesState = MainGameManager.Instance.SceneOwner.GetMainSceneStagesState();
            var isGoalReached = new BoolReactiveProperty();
            isGoalReached.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(async x =>
                {
                    if (x)
                    {
                        MainGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.me_game_clear);
                        // クリア済みデータの更新
                        mainSceneStagesState[currentStageDic[EnumSystemCommonCash.SceneId]][EnumMainSceneStagesState.State] = 2;
                        if (currentStageDic[EnumSystemCommonCash.SceneId] < mainSceneStagesState.Length - 1)
                            mainSceneStagesState[(currentStageDic[EnumSystemCommonCash.SceneId] + 1)][EnumMainSceneStagesState.State] = 1;
                        if (!MainGameManager.Instance.SceneOwner.SaveMainSceneStagesState(mainSceneStagesState))
                            Debug.LogError("クリア済みデータ保存呼び出しの失敗");
                        // 初期処理
                        clearView.gameObject.SetActive(true);
                        stageClearView.gameObject.SetActive(true);
                        gameProceedButtonView.gameObject.SetActive(false);
                        gameRetryButtonView.gameObject.SetActive(false);
                        gameSelectButtonView.gameObject.SetActive(false);
                        // 一定時間後に表示するUI
                        await Task.Delay(clearContentsRenderingDelayTime);
                        gameProceedButtonView.gameObject.SetActive(true);
                        // 初回のみ最初から拡大表示
                        gameProceedButtonView.SetScale();
                        gameRetryButtonView.gameObject.SetActive(true);
                        gameSelectButtonView.gameObject.SetActive(true);
                        cursorIconView.gameObject.SetActive(true);
                        // 初回のみ最初から選択状態
                        gameProceedButtonModel.SetSelectedGameObject();
                    }
                });

            // クリア画面 -> 次のステージへ進む
            gameProceedButtonModel.EventState.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    switch ((EnumEventCommand)x)
                    {
                        case EnumEventCommand.Default:
                            // 処理無し
                            break;
                        case EnumEventCommand.Selected:
                            MainGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_select);
                            if (!gameProceedButtonView.PlayScaleUpAnimation())
                                Debug.LogError("拡大アニメーション呼び出しの失敗");
                            if (!cursorIconView.PlaySelectAnimation(gameProceedButtonView.transform.position))
                                Debug.LogError("カーソル移動アニメーション呼び出しの失敗");
                            break;
                        case EnumEventCommand.DeSelected:
                            if (!gameProceedButtonView.SetDefaultScale())
                                Debug.LogError("デフォルトサイズへ変更呼び出しの失敗");
                            break;
                        case EnumEventCommand.Submited:
                            MainGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_decided);
                            if (!gameProceedButtonModel.SetButtonEnabled(false))
                                Debug.LogError("ボタン有効／無効切り替え呼び出しの失敗");
                            if (!gameProceedButtonModel.SetEventTriggerEnabled(false))
                                Debug.LogError("イベント有効／無効切り替え呼び出しの失敗");
                            var owner = MainGameManager.Instance.SceneOwner;
                            if (!owner.SetSystemCommonCash(owner.CountUpSceneId(currentStageDic)))
                                Debug.LogError("シーンID更新呼び出しの失敗");
                            // シーン読み込み時のアニメーション
                            Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Close))
                                .Subscribe(_ => owner.LoadMainScene())
                                .AddTo(gameObject);
                            break;
                        case EnumEventCommand.Canceled:
                            // 処理無し
                            break;
                        default:
                            Debug.LogWarning("例外ケース");
                            break;
                    }
                });
            // クリア画面 -> もう一度遊ぶ
            gameRetryButtonModel.EventState.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    switch ((EnumEventCommand)x)
                    {
                        case EnumEventCommand.Default:
                            // 処理無し
                            break;
                        case EnumEventCommand.Selected:
                            MainGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_select);
                            if (!gameRetryButtonView.PlayScaleUpAnimation())
                                Debug.LogError("拡大アニメーション呼び出しの失敗");
                            if (!cursorIconView.PlaySelectAnimation(gameRetryButtonView.transform.position))
                                Debug.LogError("カーソル移動アニメーション呼び出しの失敗");
                            break;
                        case EnumEventCommand.DeSelected:
                            if (!gameRetryButtonView.SetDefaultScale())
                                Debug.LogError("デフォルトサイズへ変更呼び出しの失敗");
                            break;
                        case EnumEventCommand.Submited:
                            MainGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_retry);
                            if (!gameRetryButtonModel.SetButtonEnabled(false))
                                Debug.LogError("ボタン有効／無効切り替え呼び出しの失敗");
                            if (!gameRetryButtonModel.SetEventTriggerEnabled(false))
                                Debug.LogError("イベント有効／無効切り替え呼び出しの失敗");
                            // シーン読み込み時のアニメーション
                            Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Close))
                                .Subscribe(_ => MainGameManager.Instance.SceneOwner.LoadMainScene())
                                .AddTo(gameObject);
                            break;
                        case EnumEventCommand.Canceled:
                            // 処理無し
                            break;
                        default:
                            Debug.LogWarning("例外ケース");
                            break;
                    }
                });
            // クリア画面 -> ステージ選択画面へ戻る
            gameSelectButtonModel.EventState.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    switch ((EnumEventCommand)x)
                    {
                        case EnumEventCommand.Default:
                            // 処理無し
                            break;
                        case EnumEventCommand.Selected:
                            MainGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_select);
                            if (!gameSelectButtonView.PlayScaleUpAnimation())
                                Debug.LogError("拡大アニメーション呼び出しの失敗");
                            if (!cursorIconView.PlaySelectAnimation(gameSelectButtonView.transform.position))
                                Debug.LogError("カーソル移動アニメーション呼び出しの失敗");
                            break;
                        case EnumEventCommand.DeSelected:
                            if (!gameSelectButtonView.SetDefaultScale())
                                Debug.LogError("デフォルトサイズへ変更呼び出しの失敗");
                            break;
                        case EnumEventCommand.Submited:
                            MainGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_decided);
                            if (!gameSelectButtonModel.SetButtonEnabled(false))
                                Debug.LogError("ボタン有効／無効切り替え呼び出しの失敗");
                            if (!gameSelectButtonModel.SetEventTriggerEnabled(false))
                                Debug.LogError("イベント有効／無効切り替え呼び出しの失敗");
                            // シーン読み込み時のアニメーション
                            Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Close))
                                .Subscribe(_ => MainGameManager.Instance.SceneOwner.LoadSelectScene())
                                .AddTo(gameObject);
                            break;
                        case EnumEventCommand.Canceled:
                            // 処理無し
                            break;
                        default:
                            Debug.LogWarning("例外ケース");
                            break;
                    }
                });
            // ショートカットキー
            var inputUIPushedTime = new FloatReactiveProperty();
            var inputUIActionsState = new IntReactiveProperty((int)EnumShortcuActionMode.None);
            inputUIActionsState.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    // 押下されるボタンが切り替わったら押下時間リセット
                    inputUIPushedTime.Value = 0f;
                });
            inputUIPushedTime.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    if (0f < x)
                    {
                        // いずれかのボタンが押されている
                        if (!((EnumShortcuActionMode)inputUIActionsState.Value).Equals(EnumShortcuActionMode.None))
                            for (var j = 0; j < pushTimeGageViews.Length; j++)
                                if (!pushTimeGageViews[j].EnabledPushGageAndGetFillAmount(j == inputUIActionsState.Value ? x : 0f))
                                    Debug.LogError("ゲージ更新呼び出しの失敗");
                    }
                    else
                        // 全てのボタンから指を離している
                        for (var j = 0; j < pushTimeGageViews.Length; j++)
                            if (!pushTimeGageViews[j].EnabledPushGageAndGetFillAmount(0f))
                                Debug.LogError("ゲージ更新呼び出しの失敗");
                });
            // ショートカットキー -> 実行中のアクションを管理
            for (var i = 0; i < pushTimeGageViews.Length; i++)
            {
                var tmpIdx = i;
                pushTimeGageViews[tmpIdx].FloatReactiveProperty.ObserveEveryValueChanged(x => x.Value)
                    .Subscribe(x =>
                    {
                        // ゲージ満タンで各モードを実行
                        if (inProcess.Equals(EnumShortcuActionMode.None) && 1f <= x)
                        {
                            inProcess = (EnumShortcuActionMode)tmpIdx;
                            switch (inProcess)
                            {
                                case EnumShortcuActionMode.UndoAction:
                                    MainGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_retry);
                                    // チュートリアルUIを開いていたら閉じる
                                    if (moveGuideView.isActiveAndEnabled)
                                        // 移動操作クローズのアニメーション
                                        Observable.FromCoroutine<bool>(observer => moveGuideView.PlayFadeAnimation(observer, EnumFadeState.Close))
                                            .Subscribe(_ => moveGuideView.gameObject.SetActive(false))
                                            .AddTo(gameObject);
                                    if (jumpGuideView.isActiveAndEnabled)
                                        // ジャンプ操作クローズのアニメーション
                                        Observable.FromCoroutine<bool>(observer => jumpGuideView.PlayFadeAnimation(observer, EnumFadeState.Close))
                                            .Subscribe(_ => jumpGuideView.gameObject.SetActive(false))
                                            .AddTo(gameObject);
                                    if (playerModel != null)
                                        if (!playerModel.SetInputBan(true))
                                            Debug.LogError("操作禁止フラグ更新呼び出しの失敗");
                                    // T.B.D プレイヤーの挙動によって発生するイベント無効　など
                                    if (!MainGameManager.Instance.InputSystemsOwner.Exit())
                                        Debug.LogError("InputSystem終了呼び出しの失敗");
                                    // シーン読み込み時のアニメーション
                                    Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Close))
                                        .Subscribe(_ => MainGameManager.Instance.SceneOwner.LoadMainScene())
                                        .AddTo(gameObject);
                                    break;
                                case EnumShortcuActionMode.SelectAction:
                                    MainGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_decided);
                                    // チュートリアルUIを開いていたら閉じる
                                    if (moveGuideView.isActiveAndEnabled)
                                        // 移動操作クローズのアニメーション
                                        Observable.FromCoroutine<bool>(observer => moveGuideView.PlayFadeAnimation(observer, EnumFadeState.Close))
                                            .Subscribe(_ => moveGuideView.gameObject.SetActive(false))
                                            .AddTo(gameObject);
                                    if (jumpGuideView.isActiveAndEnabled)
                                        // ジャンプ操作クローズのアニメーション
                                        Observable.FromCoroutine<bool>(observer => jumpGuideView.PlayFadeAnimation(observer, EnumFadeState.Close))
                                            .Subscribe(_ => jumpGuideView.gameObject.SetActive(false))
                                            .AddTo(gameObject);
                                    if (playerModel != null)
                                        if (!playerModel.SetInputBan(true))
                                            Debug.LogError("操作禁止フラグ更新呼び出しの失敗");
                                    // T.B.D プレイヤーの挙動によって発生するイベント無効　など
                                    if (!MainGameManager.Instance.InputSystemsOwner.Exit())
                                        Debug.LogError("InputSystem終了呼び出しの失敗");
                                    // シーン読み込み時のアニメーション
                                    Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Close))
                                        .Subscribe(_ => MainGameManager.Instance.SceneOwner.LoadSelectScene())
                                        .AddTo(gameObject);
                                    break;
                                case EnumShortcuActionMode.CheckAction:
                                    // 遊び方の確認を開く
                                    MainGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_play_open);
                                    gameManualScrollView.gameObject.SetActive(true);
                                    if (!gameManualScrollView.SetPage(EnumGameManualPagesIndex.Page_1))
                                        Debug.LogError("ページ変更呼び出しの失敗");
                                    gameManualViewPageModels[(int)EnumGameManualPagesIndex.Page_1].SetSelectedGameObject();
                                    break;
                                default:
                                    Debug.LogWarning("例外ケース");
                                    break;
                            }
                        }
                    });
            }
            // 遊び方を確認
            for (var i = 0; i < gameManualViewPageModels.Length; i++)
            {
                var tmpIdx = i;
                gameManualViewPageModels[tmpIdx].EventState.ObserveEveryValueChanged(x => x.Value)
                    .Subscribe(x =>
                    {
                        switch ((EnumEventCommand)x)
                        {
                            case EnumEventCommand.Default:
                                // 処理無し
                                break;
                            case EnumEventCommand.Selected:
                                MainGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_select);
                                if (!gameManualScrollView.PlayPagingAnimation((EnumGameManualPagesIndex)tmpIdx))
                                    Debug.LogError("ページ変更アニメーション呼び出しの失敗");
                                break;
                            case EnumEventCommand.DeSelected:
                                // 処理無し
                                break;
                            case EnumEventCommand.Submited:
                                // 処理無し
                                break;
                            case EnumEventCommand.Canceled:
                                MainGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_cancel);
                                if (!gameManualViewPageModels[tmpIdx].SetButtonEnabled(false))
                                    Debug.LogError("ボタン有効／無効切り替え呼び出しの失敗");
                                if (!gameManualViewPageModels[tmpIdx].SetEventTriggerEnabled(false))
                                    Debug.LogError("イベント有効／無効切り替え呼び出しの失敗");
                                // 遊び方を確認クローズのアニメーション
                                Observable.FromCoroutine<bool>(observer => gameManualScrollView.PlayCloseAnimation(observer))
                                    .Subscribe(_ =>
                                    {
                                        gameManualScrollView.gameObject.SetActive(false);
                                        inProcess = EnumShortcuActionMode.None;
                                    })
                                    .AddTo(gameObject);
                                break;
                            default:
                                Debug.LogWarning("例外ケース");
                                break;
                        }
                    });
            }
            // チュートリアルUI -> 移動操作
            var isTriggerEnteredMoveGuide = new BoolReactiveProperty();
            isTriggerEnteredMoveGuide.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    if (x)
                    {
                        moveGuideView.gameObject.SetActive(true);
                        // 移動操作オープンのアニメーション
                        Observable.FromCoroutine<bool>(observer => moveGuideView.PlayFadeAnimation(observer, EnumFadeState.Open))
                            .Subscribe(_ => { })
                            .AddTo(gameObject);
                    }
                    else
                    {
                        // 移動操作クローズのアニメーション
                        Observable.FromCoroutine<bool>(observer => moveGuideView.PlayFadeAnimation(observer, EnumFadeState.Close))
                            .Subscribe(_ => moveGuideView.gameObject.SetActive(false))
                            .AddTo(gameObject);
                    }
                });
            // チュートリアルUI -> ジャンプ操作
            var isTriggerEnteredJumpGuide = new BoolReactiveProperty();
            isTriggerEnteredJumpGuide.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    if (x)
                    {
                        jumpGuideView.gameObject.SetActive(true);
                        // ジャンプ操作オープンのアニメーション
                        Observable.FromCoroutine<bool>(observer => jumpGuideView.PlayFadeAnimation(observer, EnumFadeState.Open))
                            .Subscribe(_ => { })
                            .AddTo(gameObject);
                    }
                    else
                    {
                        // ジャンプ操作クローズのアニメーション
                        Observable.FromCoroutine<bool>(observer => jumpGuideView.PlayFadeAnimation(observer, EnumFadeState.Close))
                            .Subscribe(_ => jumpGuideView.gameObject.SetActive(false))
                            .AddTo(gameObject);
                    }
                });
            // レベルのインスタンスに合わせてメンバー変数をセット
            MainGameManager.Instance.LevelOwner.IsInstanced.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    if (x)
                    {
                        // プレイヤーがインスタンス状態
                        playerStartPointView = GameObject.Find(ConstGameObjectNames.GAMEOBJECT_NAME_PLAYERSTARTPOINT).GetComponent<PlayerStartPointView>();
                        isStartDirectionCompleted.Value++;
                        Debug.Log($"スタート開始位置を生成完了:[{isStartDirectionCompleted.Value}]");
                        playerStartPointView.IsInstanced.ObserveEveryValueChanged(x => x.Value)
                            .Subscribe(x =>
                            {
                                if (x)
                                {
                                    var player = GameObject.FindGameObjectWithTag(ConstTagNames.TAG_NAME_PLAYER);
                                    playerView = player.GetComponent<PlayerView>();
                                    playerModel = player.GetComponent<PlayerModel>();
                                    playerModel.IsInstanced.ObserveEveryValueChanged(x => x.Value)
                                        .Subscribe(x =>
                                        {
                                            if (x)
                                            {
                                                targetPointerView = playerModel.TargetPointer.GetComponent<TargetPointerView>();
                                                playerModel.MoveVelocityReactiveProperty.ObserveEveryValueChanged(x => x.Value)
                                                    .Subscribe(x =>
                                                    {
                                                        if (!targetPointerView.SetPosition(playerModel.transform.position, x))
                                                            Debug.LogError("位置情報をセット呼び出しの失敗");
                                                        if (0 < x.magnitude)
                                                        {
                                                            if (!targetPointerView.RenderVisible())
                                                                Debug.LogError("見える状態に描画呼び出しの失敗");
                                                        }
                                                        else
                                                        {
                                                            if (!targetPointerView.RenderUnVisible())
                                                                Debug.LogError("見えない状態に描画呼び出しの失敗");
                                                        }
                                                    });
                                            }
                                        });
                                    var attackTrigger = GameObject.FindGameObjectWithTag(ConstTagNames.TAG_NAME_ATTACK_TRIGGER).GetComponent<AttackTrigger>();
                                    playerModel.IsPlayingAction.ObserveEveryValueChanged(x => x.Value)
                                        .Subscribe(x =>
                                        {
                                            if (!attackTrigger.SetColliderEnabled(x))
                                                Debug.LogError("コライダーの有効／無効をセット呼び出しの失敗");
                                        });
                                }
                            });
                        // Getプロセスの実行状態（false:初期状態／停止、true:実行中）
                        var isGetProcessStart = new BoolReactiveProperty();
                        // スタートノード
                        var startNode = GameObject.FindGameObjectWithTag(ConstTagNames.TAG_NAME_STARTNODE);
                        if (startNode != null)
                        {
                            startNode.GetComponent<StartNodeModel>().IsPosting.ObserveEveryValueChanged(x => x.Value)
                                .Subscribe(x =>
                                {
                                    if (x)
                                    {
                                        Debug.Log($"POST実行中:[{startNode.name}]");
                                        if (!MainGameManager.Instance.AlgorithmOwner.AddHistorySignalsPosted(startNode.transform, true))
                                            Debug.LogError("信号が送信された履歴へ追加呼び出しの失敗");
                                    }
                                    else
                                        Debug.Log($"POST実行停止:[{startNode.name}]");
                                });
                            startNode.GetComponent<StartNodeModel>().ToListLength.ObserveEveryValueChanged(x => x.Value)
                                .Subscribe(x =>
                                {
                                    if (0 < x)
                                    {
                                        foreach (var child in startNode.GetComponent<StartNodeModel>().ToList)
                                        {
                                            if (child.GetComponent<PivotModel>() != null &&
                                                !child.GetComponent<PivotModel>().GetSignal())
                                                Debug.LogError("シグナル受信呼び出しの失敗");
                                        }
                                    }
                                    else if (x == 0)
                                    {
                                        isGetProcessStart.Value = true;
                                    }
                                    else
                                    {
                                        // 値がリセットされた場合
                                        // スタートイベント時にも呼ばれる
                                        isGetProcessStart.Value = false;
                                        if (!startNode.GetComponent<StartNodeModel>().WaitSignalPost())
                                            Debug.LogError("信号発生まで待つ呼び出しの失敗");
                                    }
                                });
                            startNode.GetComponent<StartNodeModel>().IsGetting.ObserveEveryValueChanged(x => x.Value)
                                .Subscribe(x =>
                                {
                                    if (x)
                                    {
                                        Debug.Log($"GET実行中:[{startNode.name}]");
                                        if (!MainGameManager.Instance.AlgorithmOwner.AddHistorySignalsGeted(startNode.transform))
                                            Debug.LogError("信号が送信された履歴へ追加呼び出しの失敗");
                                        if (!MainGameManager.Instance.AlgorithmOwner.MergeHistorySignalsGetedToPosted())
                                            Debug.LogError("信号が受信された履歴の配列を信号が送信された履歴の配列へマージ呼び出しの失敗");
                                        var goalNode = MainGameManager.Instance.AlgorithmOwner.HistorySignalsPosted.Where(q => q.GetComponent<GoalNodeModel>() != null).Select(q => q).ToArray()[0];
                                        if (!goalNode.GetComponent<GoalNodeModel>().GetSignal())
                                            Debug.LogError("シグナル受信呼び出しの失敗");
                                    }
                                    else
                                        Debug.Log($"GET実行停止:[{startNode.name}]");
                                });
                        }
                        else
                        {
                            Debug.LogWarning("ノードのオブジェクト取得に失敗");
                        }
                        // ゴールノード
                        var goalNode = GameObject.FindGameObjectWithTag(ConstTagNames.TAG_NAME_GOALNODE);
                        if (goalNode != null)
                        {
                            // バグフィックス
                            goalNode.GetComponent<GoalNodeModel>().IsPosting.ObserveEveryValueChanged(x => x.Value)
                                .Subscribe(x =>
                                {
                                    if (x)
                                    {
                                        Debug.Log($"POST実行中:[{goalNode.name}]");
                                        if (!MainGameManager.Instance.AlgorithmOwner.AddHistorySignalsPosted(goalNode.transform))
                                            Debug.LogError("信号が送信された履歴へ追加呼び出しの失敗");
                                        foreach (var item in MainGameManager.Instance.AlgorithmOwner.HistorySignalsPosted)
                                        {
                                            if (item.GetComponent<StartNodeView>() != null)
                                            {
                                                Observable.FromCoroutine<bool>(observer => item.GetComponent<StartNodeView>().PlayLightAnimation(observer))
                                                    .Subscribe(_ =>
                                                    {
                                                        // スタートからゴールまで繋がっている状態ならリセットしない
                                                    })
                                                    .AddTo(gameObject);
                                            }
                                            if (item.GetComponent<PivotView>() != null)
                                            {
                                                Observable.FromCoroutine<bool>(observer => item.GetComponent<PivotView>().PlayLightAnimation(observer))
                                                    .Subscribe(_ =>
                                                    {
                                                        // スタートからゴールまで繋がっている状態ならリセットしない
                                                    })
                                                    .AddTo(gameObject);
                                            }
                                            if (item.GetComponent<GoalNodeView>() != null)
                                            {
                                                Observable.FromCoroutine<bool>(observer => item.GetComponent<GoalNodeView>().PlayLightAnimation(observer))
                                                    .Subscribe(_ =>
                                                    {
                                                        // スタートからゴールまで繋がっている状態ならリセットしない
                                                    })
                                                    .AddTo(gameObject);
                                            }
                                        }

                                        var goalNodeView = goalNode.GetComponent<GoalNodeView>();
                                        // 取り出したバグのモデルを監視
                                        if (!goalNodeView.bugfix())
                                            Debug.LogError("バグフィックス呼び出しの失敗");
                                        var bug = goalNodeView.InstanceBug;
                                        bug.GetComponent<BugModel>().IsEated.ObserveEveryValueChanged(x => x.Value)
                                            .Subscribe(x =>
                                            {
                                                if (x)
                                                    isGoalReached.Value = true;
                                            });
                                    }
                                    else
                                        Debug.Log($"POST実行停止:[{goalNode.name}]");
                                });
                            // 帰納法のためのGETプロセス
                            goalNode.GetComponent<GoalNodeModel>().IsGetting.ObserveEveryValueChanged(x => x.Value)
                                .Subscribe(x =>
                                {
                                    if (x)
                                    {
                                        Debug.Log($"GET実行中:[{goalNode.name}]");
                                        if (!MainGameManager.Instance.AlgorithmOwner.AddHistorySignalsGeted(goalNode.transform, true))
                                            Debug.LogError("信号が送信された履歴へ追加呼び出しの失敗");
                                        if (!goalNode.GetComponent<GoalNodeModel>().Getting())
                                            Debug.LogError("コード元を辿る呼び出しの失敗");
                                    }
                                    else
                                        Debug.Log($"GET実行停止:[{goalNode.name}]");
                                });
                            goalNode.GetComponent<GoalNodeModel>().FromListLength.ObserveEveryValueChanged(x => x.Value)
                                .Subscribe(x =>
                                {
                                    if (0 < x)
                                    {
                                        // ゴールノードの向き先は一つのみのため「FromLists配列番号」は0固定となる想定
                                        foreach (var child in goalNode.GetComponent<GoalNodeModel>().FromList)
                                        {
                                            if (child.GetComponent<PivotModel>() != null &&
                                                !child.GetComponent<PivotModel>().GetSignal(true))
                                                Debug.LogError("シグナル受信呼び出しの失敗");
                                        }
                                    }
                                    else if (x == 0)
                                    {
                                        // ゴールノードの向き先は一つのみの場合、レベルデザイン上は必ず一つ以上のピボットへ繋がる想定のため、ここの処理は未到達？
                                        isGetProcessStart.Value = false;
                                    }
                                    else
                                    {
                                        // 値がリセットされた場合
                                        // コード側の処理は特になし
                                    }
                                });
                        }
                        // コード系
                        var moleculesObjs = GameObject.FindGameObjectsWithTag(ConstTagNames.TAG_NAME_MOLECULES);
                        var atomsObjs = GameObject.FindGameObjectsWithTag(ConstTagNames.TAG_NAME_ATOMS);
                        var codeObjList = new List<GameObject>();
                        codeObjList.AddRange(moleculesObjs);
                        codeObjList.AddRange(atomsObjs);
                        var codeObjs = codeObjList.ToArray();
                        if (codeObjs != null)
                        {
                            for (var i = 0; i < codeObjs.Length; i++)
                            {
                                int idx = i;
                                codeObjs[idx].GetComponent<PivotModel>().IsPosting.ObserveEveryValueChanged(x => x.Value)
                                    .Subscribe(x =>
                                    {
                                        if (x)
                                        {
                                            Debug.Log($"POST実行中:[{codeObjs[idx].name}]");
                                            if (!MainGameManager.Instance.AlgorithmOwner.AddHistorySignalsPosted(codeObjs[idx].transform))
                                                Debug.LogError("信号が送信された履歴へ追加呼び出しの失敗");
                                        }
                                        else
                                            Debug.Log($"POST実行停止:[{codeObjs[idx].name}]");
                                    });
                                codeObjs[idx].GetComponent<PivotModel>().ToListLength.ObserveEveryValueChanged(x => x.Value)
                                    .Subscribe(x =>
                                    {
                                        if (0 < x)
                                        {
                                            foreach (var child in codeObjs[idx].GetComponent<PivotModel>().ToList)
                                            {
                                                if (child.GetComponent<PivotModel>() != null &&
                                                    !child.GetComponent<PivotModel>().GetSignal())
                                                    Debug.LogError("シグナル受信呼び出しの失敗");
                                                if (child.GetComponent<GoalNodeModel>() != null &&
                                                    !child.GetComponent<GoalNodeModel>().GetSignal())
                                                    Debug.LogError("シグナル受信呼び出しの失敗");
                                            }
                                        }
                                        else if (x == 0)
                                        {
                                            isGetProcessStart.Value = true;
                                        }
                                        else
                                        {
                                            // 値がリセットされた場合
                                            isGetProcessStart.Value = false;
                                        }
                                    });
                                codeObjs[idx].GetComponent<PivotModel>().IsGetting.ObserveEveryValueChanged(x => x.Value)
                                    .Subscribe(x =>
                                    {
                                        if (x)
                                        {
                                            Debug.Log($"GET実行中:[{codeObjs[idx].name}]");
                                            if (!MainGameManager.Instance.AlgorithmOwner.AddHistorySignalsGeted(codeObjs[idx].transform))
                                                Debug.LogError("信号が送信された履歴へ追加呼び出しの失敗");
                                        }
                                        else
                                            Debug.Log($"GET実行停止:[{codeObjs[idx].name}]");
                                    });
                                codeObjs[idx].GetComponent<PivotModel>().FromListLength.ObserveEveryValueChanged(x => x.Value)
                                    .Subscribe(x =>
                                    {
                                        if (0 < x)
                                        {
                                            // FromListLengthsのObserveEveryValueChangedからは、どのリストが更新されたか判断できないため全リストを繰り返し実行させる
                                            // 一度、「GetSignal」されたものは、フラグが更新済みとなるため新たに処理が発生しない＝空実行となる
                                            foreach (var child in codeObjs[idx].GetComponent<PivotModel>().FromList)
                                            {
                                                if (child.GetComponent<PivotModel>() != null &&
                                                    !child.GetComponent<PivotModel>().GetSignal(true))
                                                    Debug.LogError("シグナル受信呼び出しの失敗");
                                                if (child.GetComponent<StartNodeModel>() != null &&
                                                    !child.GetComponent<StartNodeModel>().GetSignal(true))
                                                    Debug.LogError("シグナル受信呼び出しの失敗");
                                            }
                                        }
                                        else if (x == 0 &&
                                            !MainGameManager.Instance.AlgorithmOwner.GetGetProcessState(codeObjs))
                                        {
                                            isGetProcessStart.Value = false;
                                        }
                                        else
                                        {
                                            // 値がリセットされた場合
                                            // コード側の処理は特になし
                                        }
                                    });
                            }
                        }
                        else
                        {
                            Debug.LogWarning("コードのオブジェクト取得に失敗");
                        }
                        isGetProcessStart.ObserveEveryValueChanged(x => x.Value)
                            .Subscribe(x =>
                            {
                                if (x)
                                {
                                    // 帰納法処理：ゴールノードからスタートノードまでコードが繋がっているか
                                    if (goalNode != null &&
                                        goalNode.GetComponent<PivotConfig>() != null &&
                                        goalNode.GetComponent<PivotConfig>().EnumAtomicMode.Equals(EnumAtomicMode.Molecules) &&
                                        goalNode.GetComponent<GoalNodeModel>() != null &&
                                        !goalNode.GetComponent<GoalNodeModel>().IsGetting.Value)
                                    {
                                        if (!goalNode.GetComponent<GoalNodeModel>().SetIsGetting(true))
                                            Debug.LogError("信号受信フラグをセット呼び出しの失敗");
                                    }
                                    else
                                    {
                                        // 上記でもモジュールが繋がっていない状態として判断された場合リセットする

                                        // POSTのリセット
                                        foreach (var item in MainGameManager.Instance.AlgorithmOwner.HistorySignalsPosted)
                                        {
                                            if (item.GetComponent<StartNodeView>() != null)
                                            {
                                                Observable.FromCoroutine<bool>(observer => item.GetComponent<StartNodeView>().PlayLightAnimation(observer))
                                                    .Subscribe(_ =>
                                                    {
                                                        if (!item.GetComponent<StartNodeModel>().SetIsPosting(false))
                                                            Debug.LogError("信号発生アニメーション実行中フラグをセット呼び出しの失敗");
                                                        if (!item.GetComponent<StartNodeModel>().SetToListLength(-1))
                                                            Debug.LogError("POST先のノードコードリスト数をセット呼び出しの失敗");
                                                    })
                                                    .AddTo(gameObject);
                                            }
                                            if (item.GetComponent<PivotView>() != null)
                                            {
                                                Observable.FromCoroutine<bool>(observer => item.GetComponent<PivotView>().PlayLightAnimation(observer))
                                                    .Subscribe(_ =>
                                                    {
                                                        if (!item.GetComponent<PivotModel>().SetIsPosting(false))
                                                            Debug.LogError("信号発生アニメーション実行中フラグをセット呼び出しの失敗");
                                                        if (!item.GetComponent<PivotModel>().SetToListLength(-1))
                                                            Debug.LogError("POST先のノードコードリスト数をセット呼び出しの失敗");
                                                    })
                                                    .AddTo(gameObject);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    Debug.Log("Getプロセス初期状態／停止");

                                    if (MainGameManager.Instance.AlgorithmOwner.HistorySignalsPosted != null)
                                    {
                                        // POSTのリセット
                                        foreach (var item in MainGameManager.Instance.AlgorithmOwner.HistorySignalsPosted)
                                        {
                                            if (item.GetComponent<StartNodeView>() != null)
                                            {
                                                Observable.FromCoroutine<bool>(observer => item.GetComponent<StartNodeView>().PlayLightAnimation(observer))
                                                    .Subscribe(_ =>
                                                    {
                                                        if (!item.GetComponent<StartNodeModel>().SetIsPosting(false))
                                                            Debug.LogError("信号発生アニメーション実行中フラグをセット呼び出しの失敗");
                                                        if (!item.GetComponent<StartNodeModel>().SetToListLength(-1))
                                                            Debug.LogError("POST先のノードコードリスト数をセット呼び出しの失敗");
                                                    })
                                                    .AddTo(gameObject);
                                            }
                                            if (item.GetComponent<PivotView>() != null)
                                            {
                                                Observable.FromCoroutine<bool>(observer => item.GetComponent<PivotView>().PlayLightAnimation(observer))
                                                    .Subscribe(_ =>
                                                    {
                                                        if (!item.GetComponent<PivotModel>().SetIsPosting(false))
                                                            Debug.LogError("信号発生アニメーション実行中フラグをセット呼び出しの失敗");
                                                        if (!item.GetComponent<PivotModel>().SetToListLength(-1))
                                                            Debug.LogError("POST先のノードコードリスト数をセット呼び出しの失敗");
                                                    })
                                                    .AddTo(gameObject);
                                            }
                                        }
                                    }
                                    if (MainGameManager.Instance.AlgorithmOwner.HistorySignalsGeted != null)
                                    {
                                        // GETのリセット
                                        foreach (var item in MainGameManager.Instance.AlgorithmOwner.HistorySignalsGeted)
                                        {
                                            foreach (var itemChild in item)
                                            {
                                                if (itemChild.GetComponent<GoalNodeModel>() != null)
                                                {
                                                    // GETの場合はアニメーション不要のためリセット呼び出しのみ
                                                    if (!itemChild.GetComponent<GoalNodeModel>().SetIsGetting(false))
                                                        Debug.LogError("信号受信中フラグをセット呼び出しの失敗");
                                                    if (!itemChild.GetComponent<GoalNodeModel>().SetFromListLength(-1))
                                                        Debug.LogError("GET元のノードコードリスト数をセット呼び出しの失敗");
                                                }
                                                if (itemChild.GetComponent<PivotView>() != null)
                                                {
                                                    // GETの場合はアニメーション不要のためリセット呼び出しのみ
                                                    if (!itemChild.GetComponent<PivotModel>().SetIsGetting(false))
                                                        Debug.LogError("信号受信中フラグをセット呼び出しの失敗");
                                                    if (!itemChild.GetComponent<PivotModel>().SetFromListLength(-1))
                                                        Debug.LogError("GET元のノードコードリスト数をセット呼び出しの失敗");
                                                }
                                            }
                                        }
                                    }
                                }
                            });
                        var goalPointObj = GameObject.Find(ConstGameObjectNames.GAMEOBJECT_NAME_GOALPOINT);
                        goalPointView = goalPointObj.GetComponent<GoalPointView>();
                        goalPointModel = goalPointObj.GetComponent<GoalPointModel>();
                        goalPointModel.IsTriggerEntered.ObserveEveryValueChanged(x => x.Value)
                            .Subscribe(x =>
                            {
                                if (x)
                                    isGoalReached.Value = true;
                            });
                    }
                });

            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    if (isInputUIPausedEnabled.Value)
                        inputUIPausedState.Value = MainGameManager.Instance.InputSystemsOwner.GetComponent<InputSystemsOwner>().InputUI.Paused;
                    if (isInputUIActionsEnabled.Value)
                    {
                        if (((EnumShortcuActionMode)inputUIActionsState.Value).Equals(EnumShortcuActionMode.None))
                        {
                            // ショートカットキーの押下が None -> Any へ変わる
                            if (MainGameManager.Instance.InputSystemsOwner.GetComponent<InputSystemsOwner>().InputUI.Undoed &&
                                !MainGameManager.Instance.InputSystemsOwner.GetComponent<InputSystemsOwner>().InputUI.Selected &&
                                !MainGameManager.Instance.InputSystemsOwner.GetComponent<InputSystemsOwner>().InputUI.Manualed)
                                inputUIActionsState.Value = (int)EnumShortcuActionMode.UndoAction;
                            else if (MainGameManager.Instance.InputSystemsOwner.GetComponent<InputSystemsOwner>().InputUI.Selected &&
                                !MainGameManager.Instance.InputSystemsOwner.GetComponent<InputSystemsOwner>().InputUI.Manualed)
                                inputUIActionsState.Value = (int)EnumShortcuActionMode.SelectAction;
                            else if (MainGameManager.Instance.InputSystemsOwner.GetComponent<InputSystemsOwner>().InputUI.Manualed)
                                inputUIActionsState.Value = (int)EnumShortcuActionMode.CheckAction;
                        }
                        else if ((((EnumShortcuActionMode)inputUIActionsState.Value).Equals(EnumShortcuActionMode.UndoAction) &&
                            !MainGameManager.Instance.InputSystemsOwner.GetComponent<InputSystemsOwner>().InputUI.Undoed) ||
                            (((EnumShortcuActionMode)inputUIActionsState.Value).Equals(EnumShortcuActionMode.SelectAction) &&
                            !MainGameManager.Instance.InputSystemsOwner.GetComponent<InputSystemsOwner>().InputUI.Selected) ||
                            (((EnumShortcuActionMode)inputUIActionsState.Value).Equals(EnumShortcuActionMode.CheckAction) &&
                            !MainGameManager.Instance.InputSystemsOwner.GetComponent<InputSystemsOwner>().InputUI.Manualed))
                        {
                            // ショートカットキーの押下が Any -> None へ変わる
                            inputUIActionsState.Value = (int)EnumShortcuActionMode.None;
                        }
                        if (!((EnumShortcuActionMode)inputUIActionsState.Value).Equals(EnumShortcuActionMode.None))
                            inputUIPushedTime.Value += Time.deltaTime;
                        else if (0f < inputUIPushedTime.Value)
                            // ショートカットキーの押下状態がNoneへ戻ったらリセット
                            // 既に0fなら何度も更新は行わない
                            inputUIPushedTime.Value = 0f;
                    }
                });
        }
    }
}
