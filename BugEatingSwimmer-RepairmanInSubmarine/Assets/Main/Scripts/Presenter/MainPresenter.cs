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
using Fungus;

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
        /// <summary>プレイヤーのビュー</summary>
        [SerializeField] private PlayerView playerView;
        /// <summary>プレイヤーのモデル</summary>
        [SerializeField] private PlayerModel playerModel;
        /// <summary>移動先にポインタ表示のビュー</summary>
        [SerializeField] private TargetPointerView targetPointerView;
        /// <summary>準委任帳票のビュー</summary>
        [SerializeField] private AssignedSeastarCountView assignedSeastarCountView;
        /// <summary>CinemachineVirtualCameraのビュー</summary>
        [SerializeField] private CinemachineVirtualCameraView cinemachineVirtualCameraView;
        /// <summary>Fungusのレシーバー</summary>
        [SerializeField] private MessageReceived[] receivers;
        /// <summary>Fungusのフローチャートモデル</summary>
        [SerializeField] private FlowchartModel flowchartModel;
        /// <summary>メイン画面の背景のビュー</summary>
        [SerializeField] private MareBlanketSeaView mareBlanketSeaView;
        /// <summary>ショートカットキーUIのビュー</summary>
        [SerializeField] private ShortcutGuideView shortcutGuideView;

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
            assignedSeastarCountView = GameObject.Find("AssignedSeastarCount").GetComponent<AssignedSeastarCountView>();
            cinemachineVirtualCameraView = GameObject.Find("CinemachineVirtualCamera").GetComponent<CinemachineVirtualCameraView>();
            receivers = GameObject.FindObjectsOfType<Fungus.MessageReceived>();
            flowchartModel = GameObject.Find("Flowchart").GetComponent<FlowchartModel>();
            mareBlanketSeaView = GameObject.Find("MareBlanketSea").GetComponent<MareBlanketSeaView>();
            shortcutGuideView = GameObject.Find("ShortcutGuide").GetComponent<ShortcutGuideView>();
        }

        public void OnStart()
        {
            var common = new MainPresenterCommon();

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
            if (common.IsOpeningTutorialMode())
            {
                assignedSeastarCountView.gameObject.SetActive(false);
                shortcutGuideView.gameObject.SetActive(false);
            }
            if (!assignedSeastarCountView.SetCounterText(MainGameManager.Instance.GimmickOwner.GetAssinedCounter()))
                Debug.LogError("ヒトデ総配属人数をセット呼び出しの失敗");
            if (common.IsOpeningTutorialMode())
                if (!mareBlanketSeaView.SetBackgroundToTutorial())
                    Debug.LogError("チュートリアルモードの背景をセット呼び出しの失敗");

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
            var areaUnits = common.LoadSaveDatasCSVAndGetAreaUnits();
            // 「この感情（おもい）は、バグか？仕様か？」
            var isThisEmotionaBug_IsItASpecification = new BoolReactiveProperty();
            var mainSceneStagesState = MainGameManager.Instance.SceneOwner.GetMainSceneStagesState();
            var mainSceneStagesModulesState = MainGameManager.Instance.SceneOwner.GetMainSceneStagesModulesState();
            var isGoalReached = new BoolReactiveProperty();
            isGoalReached.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(async x =>
                {
                    if (x)
                    {
                        MainGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.me_game_clear);
                        // クリア済みデータの更新
                        mainSceneStagesState[currentStageDic[EnumSystemCommonCash.SceneId]][EnumMainSceneStagesState.State] = 2;
                        if (currentStageDic[EnumSystemCommonCash.SceneId] < mainSceneStagesState.Length - 1 &&
                            mainSceneStagesState[(currentStageDic[EnumSystemCommonCash.SceneId] + 1)][EnumMainSceneStagesState.State] < 1)
                            mainSceneStagesState[(currentStageDic[EnumSystemCommonCash.SceneId] + 1)][EnumMainSceneStagesState.State] = 1;
                        // ステージごとのクリア状態を保存
                        //Debug.Log(string.Join("/", MainGameManager.Instance.AlgorithmOwner.HistorySignalsPosted.Select(q => q.GetComponent<PivotConfig>().EnumNodeCodeID)));
                        if (!MainGameManager.Instance.SceneOwner.SaveMainSceneStagesState(mainSceneStagesState))
                            Debug.LogError("クリア済みデータ保存呼び出しの失敗");
                        if (!common.SaveDatasCSVOfAreaOpenedAndITStateAndOfMission())
                            Debug.LogError("エリア解放と実績一覧を更新呼び出しの失敗");
                        if (!MainGameManager.Instance.SceneOwner.SaveMainSceneStagesModulesState(mainSceneStagesModulesState))
                            Debug.LogError("ステージクリア条件の保存呼び出しの失敗");
                        if (!MainGameManager.Instance.GimmickOwner.SaveAssigned())
                            Debug.LogError("アサインを保存呼び出しの失敗");
                        if (!assignedSeastarCountView.SetCounterText(MainGameManager.Instance.GimmickOwner.GetAssinedCounter()))
                            Debug.LogError("ヒトデ総配属人数をセット呼び出しの失敗");
                        if (common.IsFinalLevel(areaUnits, currentStageDic))
                        {
                            var missions = common.LoadSaveDatasCSVAndGetMission();
                            if (isThisEmotionaBug_IsItASpecification.Value)
                            {
                                if (common.IsUnlocked(missions, EnumMissionID.MI0008))
                                {
                                    missions = common.SetUnlockState(missions, EnumMissionID.MI0008);
                                    if (!common.SaveDatasCSVOfMission(ConstResorcesNames.MISSION, missions))
                                        Debug.LogError("実績一覧管理データをCSVデータへ保存呼び出しの失敗");
                                }
                            }
                            else
                            {
                                if (common.IsUnlocked(missions, EnumMissionID.MI0009))
                                {
                                    missions = common.SetUnlockState(missions, EnumMissionID.MI0009);
                                    if (!common.SaveDatasCSVOfMission(ConstResorcesNames.MISSION, missions))
                                        Debug.LogError("実績一覧管理データをCSVデータへ保存呼び出しの失敗");
                                }
                            }
                        }
                        // 初期処理
                        clearView.gameObject.SetActive(true);
                        if (common.IsFinalLevel(areaUnits, currentStageDic))
                            if (!stageClearView.SetMessageCongratulations())
                                Debug.LogError("最終ステージ用のメッセージをセット呼び出しの失敗");
                        stageClearView.gameObject.SetActive(true);
                        gameProceedButtonView.gameObject.SetActive(false);
                        gameRetryButtonView.gameObject.SetActive(false);
                        gameSelectButtonView.gameObject.SetActive(false);
                        // 一定時間後に表示するUI
                        await System.Threading.Tasks.Task.Delay(clearContentsRenderingDelayTime);
                        // 初回のみ最初から拡大表示
                        if (!common.IsFinalLevelOrEndOfAreaAndNotReadedScenario())
                        {
                            gameProceedButtonView.gameObject.SetActive(true);
                            gameProceedButtonView.SetScale();
                        }
                        else
                        {
                            if (!cursorIconView.SetSelect(gameRetryButtonView.transform.position))
                                Debug.LogError("カーソル配置位置の変更呼び出しの失敗");
                            gameRetryButtonView.SetScale();
                        }
                        gameRetryButtonView.gameObject.SetActive(true);
                        gameSelectButtonView.gameObject.SetActive(true);
                        cursorIconView.gameObject.SetActive(true);
                        // 初回のみ最初から選択状態
                        if (!common.IsFinalLevelOrEndOfAreaAndNotReadedScenario())
                            gameProceedButtonModel.SetSelectedGameObject();
                        else
                            gameRetryButtonModel.SetSelectedGameObject();
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
                            MainGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_decided);
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
                                .Subscribe(_ =>
                                {
                                    // エリアシーン or セレクトシーン
                                    if (!common.IsFinalLevelOrEndOfAreaAndNotReadedScenario())
                                        MainGameManager.Instance.SceneOwner.LoadSelectScene();
                                    else
                                        MainGameManager.Instance.SceneOwner.LoadAreaScene();
                                })
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
            if (!common.IsOpeningTutorialMode())
            {
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
            }
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
                                    // プレイヤーの挙動によって発生するイベント無効　など
                                    if (!MainGameManager.Instance.InputSystemsOwner.Exit())
                                        Debug.LogError("InputSystem終了呼び出しの失敗");
                                    // シーン読み込み時のアニメーション
                                    Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Close))
                                        .Subscribe(_ => MainGameManager.Instance.SceneOwner.LoadSelectScene())
                                        .AddTo(gameObject);
                                    break;
                                case EnumShortcuActionMode.CheckAction:
                                    // 遊び方の確認を開く
                                    MainGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_decided);
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
                        // 暗闇
                        var darkLight = GameObject.FindGameObjectWithTag(ConstTagNames.TAG_NAME_DARKLIGHT);
                        var darkLightTriggers = GameObject.FindGameObjectsWithTag(ConstTagNames.TAG_NAME_DARKLIGHTTRIGGER);
                        if (darkLightTriggers != null)
                        {
                            var models = darkLightTriggers.Select(q => q.GetComponent<DarkLightTriggerModel>());
                            foreach (var item in models)
                            {
                                item.IsHit.ObserveEveryValueChanged(x => x.Value)
                                    .Subscribe(x =>
                                    {
                                        if (x)
                                            if (darkLight != null)
                                                if (darkLight.GetComponent<DarkLightView>().PlayLightDown() < 0)
                                                    Debug.LogError("暗闇レベルを一つ落とす呼び出しの失敗");
                                    });
                            }
                        }

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
                                                if (!cinemachineVirtualCameraView.SetBodyTrackedObjectOffsets(EnumBodyTrackedObjectOffsetIndex.PlayerTracked))
                                                    Debug.LogError("カメラのオフセットをセット呼び出しの失敗");
                                                if (!common.IsOpeningTutorialMode())
                                                {
                                                    if (!cinemachineVirtualCameraView.SetFollow(playerModel.transform))
                                                        Debug.LogError("フォローをセット呼び出しの失敗");
                                                    if (!cinemachineVirtualCameraView.SetLookAt(playerModel.transform))
                                                        Debug.LogError("フォローをセット呼び出しの失敗");
                                                }
                                                else
                                                {
                                                    //var target = playerView.InstanceGhost();
                                                    //if (!cinemachineVirtualCameraView.SetFollow(target))
                                                    //    Debug.LogError("フォローをセット呼び出しの失敗");
                                                    //if (!cinemachineVirtualCameraView.SetLookAt(target))
                                                    //    Debug.LogError("フォローをセット呼び出しの失敗");
                                                    //Observable.FromCoroutine<bool>(observer => cinemachineVirtualCameraView.SetFollowAnimation(observer, playerModel.transform))
                                                    //    .Subscribe(_ =>
                                                    //    {
                                                    //        if (!cinemachineVirtualCameraView.SetFollow(playerModel.transform))
                                                    //            Debug.LogError("フォローをセット呼び出しの失敗");
                                                    //    })
                                                    //    .AddTo(gameObject);
                                                    //Observable.FromCoroutine<bool>(observer => cinemachineVirtualCameraView.SetLookAtAnimation(observer, playerModel.transform))
                                                    //    .Subscribe(_ =>
                                                    //    {
                                                    //        if (!cinemachineVirtualCameraView.SetLookAt(playerModel.transform))
                                                    //            Debug.LogError("標準ををセット呼び出しの失敗");
                                                    //    })
                                                    //    .AddTo(gameObject);
                                                }
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
                                                if (darkLight != null)
                                                {
                                                    if (!darkLight.GetComponent<DarkLightView>().HoverTarget(playerView.transform))
                                                        Debug.LogError("ターゲットを追尾（一度のみ）呼び出しの失敗");
                                                    Observable.FromCoroutine<int>(observer => darkLight.GetComponent<DarkLightModel>().SetStartTimer(observer))
                                                        .Subscribe(x =>
                                                        {
                                                            if (-1 < x)
                                                                if (darkLight.GetComponent<DarkLightView>().PlayLightDown() < 0)
                                                                    Debug.LogError("暗闇レベルを一つ落とす呼び出しの失敗");
                                                        })
                                                        .AddTo(gameObject);
                                                }
                                                if (common.IsOpeningTutorialMode())
                                                {
                                                    var missions = common.LoadSaveDatasCSVAndGetMission();
                                                    foreach (var item in missions.Where(q => q[EnumMission.MissionID].Equals($"{EnumMissionID.MI0000}") &&
                                                        q[EnumMission.Unlock].Equals(ConstGeneric.DIGITFORM_FALSE))
                                                        .Select(q => q))
                                                        item[EnumMission.Unlock] = ConstGeneric.DIGITFORM_TRUE;
                                                    if (!missions.Equals(common.LoadSaveDatasCSVAndGetMission()))
                                                        if (!common.SaveDatasCSVOfMission(ConstResorcesNames.MISSION, missions))
                                                            Debug.LogError("実績一覧管理データをCSVデータへ保存呼び出しの失敗");
                                                    if (!playerModel.SetInputBan(true))
                                                        Debug.LogError("操作禁止フラグをセット呼び出しの失敗");
                                                    if (!playerModel.SetIsBanMoveVelocity(true))
                                                        Debug.LogError("移動制御禁止フラグをセット呼び出しの失敗");
                                                    if (!common.SendReceiver(receivers, flowchartModel, currentStageDic, 0))
                                                        Debug.LogError("シナリオのレシーバーへ送信呼び出しの失敗");
                                                }
                                            }
                                        });
                                    var attackTrigger = GameObject.FindGameObjectWithTag(ConstTagNames.TAG_NAME_ATTACK_TRIGGER).GetComponent<AttackTrigger>();
                                    playerModel.IsPlayingAction.ObserveEveryValueChanged(x => x.Value)
                                        .Subscribe(x =>
                                        {
                                            if (!attackTrigger.SetColliderEnabled(x))
                                                Debug.LogError("コライダーの有効／無効をセット呼び出しの失敗");
                                        });
                                    var chargePhase = new IntReactiveProperty(-1);
                                    playerModel.InputPowerChargeTime.ObserveEveryValueChanged(x => x.Value)
                                        .Subscribe(x =>
                                        {
                                            if (playerModel.PowerChargePhaseTimes[2] < x)
                                                chargePhase.Value = 2;
                                            else if (playerModel.PowerChargePhaseTimes[1] < x)
                                                chargePhase.Value = 1;
                                            else if (playerModel.PowerChargePhaseTimes[0] < x)
                                                chargePhase.Value = 0;
                                            else
                                            {
                                                chargePhase.Value = -1;
                                            }
                                            if (playerModel.PowerChargePhaseTimes[0] < x)
                                                if (!playerView.HoverCharge())
                                                    Debug.LogError("チャージのホバー呼び出しの失敗");
                                        });
                                    chargePhase.ObserveEveryValueChanged(x => x.Value)
                                        .Subscribe(x =>
                                        {
                                            switch (x)
                                            {
                                                case 0:
                                                    if (!playerView.StartCharge(x))
                                                        Debug.LogError("チャージ開始呼び出しの失敗");
                                                    if (!playerView.ChangeChargeMode(0, true))
                                                        Debug.LogError("チャージ開始呼び出しの失敗");
                                                    // パワーチャージSE
                                                    MainGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_energy_store, true);
                                                    break;
                                                case 1:
                                                    // 処理無し
                                                    break;
                                                case 2:
                                                    if (!playerView.ChangeChargeMode(1, true))
                                                        Debug.LogError("チャージ開始呼び出しの失敗");
                                                    break;
                                                case -1:
                                                    if (!playerView.StopCharge())
                                                        Debug.LogError("チャージ停止呼び出しの失敗");
                                                    // パワーチャージSE停止
                                                    MainGameManager.Instance.AudioOwner.StopSFX(ClipToPlay.se_energy_store);
                                                    for (var i = 0; i < playerView.Halos.PlayerHalos.Length; i++)
                                                        if (!playerView.ChangeChargeMode(i, false))
                                                            Debug.LogError("チャージ状態を切り替え呼び出しの失敗");
                                                    break;
                                                default:
                                                    break;
                                            }
                                        });
                                    playerModel.IsPressAndHoldAndReleased.ObserveEveryValueChanged(x => x.Value)
                                        .Subscribe(x =>
                                        {
                                            if (x)
                                            {
                                                // パワーチャージSE停止
                                                MainGameManager.Instance.AudioOwner.StopSFX(ClipToPlay.se_energy_store);
                                                // T.B.D パワー解放エフェクトは不要？一旦コメントアウト
                                                //if (!playerView.PlayPowerAttackEffect())
                                                //    Debug.LogError("パワーアタックのエフェクト発生呼び出しの失敗");
                                                // パワー解放SE
                                                MainGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_energy_release);
                                            }
                                        });
                                    playerModel.OnTurn.ObserveEveryValueChanged(x => x.Value)
                                        .Subscribe(x =>
                                        {
                                            if (x)
                                            {
                                                if (!playerModel.SetOnTrurn(false))
                                                    Debug.LogError("ターン状態をセット呼び出しの失敗");
                                                if (!playerView.PlayTurnAnimation())
                                                    Debug.LogError("ターン用のアニメーション再生呼び出しの失敗");
                                                // 泳ぐSE
                                                MainGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_swim);
                                                if (!playerView.InstanceBubble())
                                                    Debug.LogError("泡が発生呼び出しの失敗");
                                            }
                                        });
                                    playerModel.IsSwimming.ObserveEveryValueChanged(x => x.Value)
                                        .Subscribe(x =>
                                        {
                                            if (x)
                                            {
                                                // 泳ぐSE
                                                MainGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_swim);
                                                if (!playerView.InstanceBubble())
                                                    Debug.LogError("泡が発生呼び出しの失敗");
                                            }
                                        });
                                }
                            });
                        // ヒトデ
                        List<SeastarModel> seastarModels = new List<SeastarModel>();
                        List<SeastarView> seastarViews = new List<SeastarView>();
                        foreach (var item in GameObject.FindGameObjectsWithTag(ConstTagNames.TAG_NAME_SEASTAR)
                            .Where(q => q != null)
                            .Select(q => q))
                        {
                            seastarViews.Add(item.GetComponent<SeastarView>());
                            seastarModels.Add(item.GetComponent<SeastarModel>());
                        }
                        var seastarGageCount = new IntReactiveProperty(0);
                        var seastarGage = GameObject.FindGameObjectWithTag(ConstTagNames.TAG_NAME_SEASTARGAGE);
                        var seastarGageView = seastarGage == null ? null : seastarGage.GetComponent<SeastarGageView>();
                        if (!common.SetCounterBetweenAndFillAmount(seastarGageView, seastarGageCount))
                            Debug.LogError("ヒトデゲージのカウンターとフィルターカウンターを更新呼び出しの失敗");
                        for (var i = 0; i < seastarModels.Count; i++)
                        {
                            var idx = i;
                            seastarModels[idx].IsAssigned.ObserveEveryValueChanged(x => x.Value)
                                .Subscribe(x =>
                                {
                                    if (!MainGameManager.Instance.GimmickOwner.SetAssignState(seastarModels[idx].transform.GetComponent<SeastarConfig>().EnumSeastarID, x))
                                        Debug.LogError("アサイン呼び出しの失敗");
                                    if (x)
                                    {
                                        if (!seastarViews[idx].SetColorAssigned())
                                            Debug.LogError("アサイン済みのカラー設定呼び出しの失敗");
                                    }
                                    else
                                    {
                                        if (!seastarViews[idx].SetColorUnAssign())
                                            Debug.LogError("未アサイン状態のカラー設定呼び出しの失敗");
                                    }
                                });
                            seastarModels[idx].IsAssignedLocal.ObserveEveryValueChanged(x => x.Value)
                                .Subscribe(x =>
                                {
                                    if (x)
                                    {
                                        if (!common.SetCounterBetweenAndFillAmount(seastarGageView, seastarGageCount, 1))
                                            Debug.LogError("ヒトデゲージのカウンターとフィルターカウンターを更新呼び出しの失敗");
                                    }
                                    else
                                    {
                                        // リセットした時のみtrue⇒falseへ変化
                                        // カウンターもリセットさせる
                                        if (!common.PlayCounterBetweenAndFillAmountAnimation(seastarGageView, seastarGageCount, 0))
                                            Debug.LogError("ヒトデゲージのカウンターとフィルターカウンターを更新呼び出しの失敗");
                                        if (seastarGageView != null)
                                        {
                                            if (!seastarGageView.SetImageEnabled())
                                                Debug.LogError("イメージを非表示呼び出しの失敗");
                                            if (!seastarGageView.SetSpriteBubble())
                                                Debug.LogError("泡スプライトをセット呼び出しの失敗");

                                        }
                                    }
                                });
                        }
                        // エビダンス
                        List<ShrimpDanceModel> shrimpDanceModels = new List<ShrimpDanceModel>();
                        List<ShrimpDanceView> shrimpDanceViews = new List<ShrimpDanceView>();
                        foreach (var item in GameObject.FindGameObjectsWithTag(ConstTagNames.TAG_NAME_SHRIMPDANCE)
                            .Where(q => q != null)
                            .Select(q => q))
                        {
                            shrimpDanceViews.Add(item.GetComponent<ShrimpDanceView>());
                            shrimpDanceModels.Add(item.GetComponent<ShrimpDanceModel>());
                        }
                        var shrimpGageCount = new IntReactiveProperty(0);
                        var shrimpGageCountMax = shrimpDanceModels.Count;
                        for (var i = 0; i < shrimpGageCountMax; i++)
                        {
                            var idx = i;
                            shrimpDanceModels[idx].IsAssignedLocal.ObserveEveryValueChanged(x => x.Value)
                                .Subscribe(x =>
                                {
                                    if (x)
                                    {
                                        if (!shrimpDanceViews[idx].SetColorAssigned())
                                            Debug.LogError("アサイン済みのカラー設定呼び出しの失敗");
                                        if (!shrimpDanceViews[idx].PlayDanceAnimation())
                                            Debug.LogError("ダンスアニメーションを再生呼び出しの失敗");
                                        shrimpGageCount.Value++;
                                    }
                                    else
                                    {
                                        if (!shrimpDanceViews[idx].SetColorUnAssign())
                                            Debug.LogError("未アサイン状態のカラー設定呼び出しの失敗");
                                        if (!shrimpDanceViews[idx].StopDanceAnimation())
                                            Debug.LogError("ダンスアニメーションを停止呼び出しの失敗");
                                        // リセットした時のみtrue⇒falseへ変化
                                        // カウンターもリセットさせる
                                        shrimpGageCount.Value = 0;
                                    }
                                });
                        }
                        // ジョーシー
                        var jawsHis = GameObject.FindGameObjectsWithTag(ConstTagNames.TAG_NAME_JAWSHI);
                        if (jawsHis != null)
                        {
                            var jawsHiModels = jawsHis.Where(q => q.GetComponent<JawsHiModel>() != null)
                                .Select(q => q.GetComponent<JawsHiModel>())
                                .ToArray();
                            var jawsHiViews = jawsHis.Where(q => q.GetComponent<JawsHiView>() != null)
                                .Select(q => q.GetComponent<JawsHiView>())
                                .ToArray();
                            var isDeadDirectioned = new BoolReactiveProperty();
                            foreach (var item in jawsHiModels.Select((p, i) => new { Content = p, Index = i }))
                            {
                                item.Content.IsHit.ObserveEveryValueChanged(x => x.Value)
                                    .Subscribe(x =>
                                    {
                                        if (x &&
                                            !isDeadDirectioned.Value)
                                        {
                                            isDeadDirectioned.Value = true;
                                            // エラーSEを再生
                                            MainGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_code_error);
                                            Time.timeScale = 0f;
                                            Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Close))
                                                .Subscribe(_ => MainGameManager.Instance.SceneOwner.LoadMainScene())
                                                .AddTo(gameObject);
                                        }
                                    });
                                item.Content.HitPosition.ObserveEveryValueChanged(x => x.Value)
                                    .Subscribe(x =>
                                    {
                                        if (0f < Mathf.Abs(x.magnitude))
                                        {
                                            if (!jawsHiModels[item.Index].StopTrackingMoveAnimation())
                                                Debug.LogError("追跡移動を止める呼び出しの失敗");
                                            Observable.FromCoroutine<bool>(observer => jawsHiViews[item.Index].PlayWowAnimation(observer))
                                                .Subscribe(_ =>
                                                {
                                                    if (!jawsHiViews[item.Index].SetColorSpriteRendererTarget(true))
                                                        Debug.LogError("カラーを設定呼び出しの失敗");
                                                    Observable.FromCoroutine<bool>(observer => jawsHiViews[item.Index].PlayLockOnAnimation(observer, playerModel.transform.position/*x*/))
                                                        .Subscribe(_ => { })
                                                        .AddTo(gameObject);
                                                    Observable.FromCoroutine<int>(observer => jawsHiViews[item.Index].PlayAttackAnimation(observer, playerModel.transform.position/*x*/))
                                                        .Subscribe(x =>
                                                        {
                                                            switch ((EnumPlayAttackAnimation)x)
                                                            {
                                                                case EnumPlayAttackAnimation.Backing:
                                                                    if (!jawsHiModels[item.Index].SetLookAtDisabled(true))
                                                                        Debug.LogError("向き変更を無効状態をセット呼び出しの失敗");
                                                                    break;
                                                                case EnumPlayAttackAnimation.Rushing:
                                                                    // ジョーシーの攻撃モード
                                                                    if (!jawsHiModels[item.Index].SetLookAtDisabled(false))
                                                                        Debug.LogError("向き変更を無効状態をセット呼び出しの失敗");
                                                                    if (!jawsHiViews[item.Index].StopLockOnAnimation())
                                                                        Debug.LogError("ロックオンアニメーションを停止呼び出しの失敗");
                                                                    if (!jawsHiViews[item.Index].SetColorSpriteRendererTarget(false))
                                                                        Debug.LogError("カラーを設定呼び出しの失敗");
                                                                    if (!jawsHiViews[item.Index].SetSpriteIndex(EnumEnemySpriteIndex.Attack))
                                                                        Debug.LogError("スプライト配列の番号をセット呼び出しの失敗");
                                                                    if (!jawsHiModels[item.Index].SetCollider2DEnabled(true))
                                                                        Debug.LogError("向き変更を無効状態をセット呼び出しの失敗");
                                                                    break;
                                                                case EnumPlayAttackAnimation.Returning:
                                                                    if (!jawsHiViews[item.Index].SetSpriteIndex(EnumEnemySpriteIndex.Normal))
                                                                        Debug.LogError("スプライト配列の番号をセット呼び出しの失敗");
                                                                    if (!jawsHiModels[item.Index].SetCollider2DEnabled(false))
                                                                        Debug.LogError("向き変更を無効状態をセット呼び出しの失敗");
                                                                    break;
                                                                case EnumPlayAttackAnimation.OnCompleted:
                                                                    // ルーティングへ戻った後
                                                                    if (!jawsHiModels[item.Index].SetHitState(false))
                                                                        Debug.LogError("ヒット状態をセット呼び出しの失敗");
                                                                    break;
                                                                default:
                                                                    break;
                                                            }
                                                        })
                                                        .AddTo(gameObject);
                                                })
                                                .AddTo(gameObject);
                                        }
                                        else
                                        {
                                            // 初期状態は非表示とできるか
                                            if (!jawsHiViews[item.Index].SetColorSpriteRendererWow(false))
                                                Debug.LogError("カラーを設定呼び出しの失敗");
                                            if (!jawsHiModels[item.Index].PlayTrackingMoveAnimation())
                                                Debug.LogError("追跡移動するアニメーションを再生呼び出しの失敗");
                                        }
                                    });
                                item.Content.IsBack.ObserveEveryValueChanged(x => x.Value)
                                    .Subscribe(x =>
                                    {
                                        if (!jawsHiViews[item.Index].SetScale(x ? Vector3.one : new Vector3(-1f, 1f, 1f)))
                                            Debug.LogError("大きさをセット呼び出しの失敗");
                                    });
                            }
                        }
                        // コシギンチャク
                        var loincloths = GameObject.FindGameObjectsWithTag(ConstTagNames.TAG_NAME_LOINCLOTH);
                        if (loincloths != null)
                        {
                            var loinclothModels = loincloths.Where(q => q.GetComponent<LoinclothModel>() != null)
                                .Select(q => q.GetComponent<LoinclothModel>())
                                .ToArray();
                            var loinclothViews = loincloths.Where(q => q.GetComponent<LoinclothView>() != null)
                                .Select(q => q.GetComponent<LoinclothView>())
                                .ToArray();
                            foreach (var item in loinclothModels.Select((p, i) => new { Content = p, Index = i }))
                                if (!loinclothViews[item.Index].PlaySwing())
                                    Debug.LogError("揺らすアニメーションを再生呼び出しの失敗");
                        }
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
                            //startNode.GetComponent<StartNodeModel>().IsGetting.ObserveEveryValueChanged(x => x.Value)
                            //    .Subscribe(x =>
                            //    {
                            //        if (x)
                            //        {
                            //            Debug.Log($"GET実行中:[{startNode.name}]");
                            //            // ※※※GETがスタートノードまで到達しないため、未到達処理？※※※
                            //            if (!MainGameManager.Instance.AlgorithmOwner.AddHistorySignalsGeted(startNode.transform))
                            //                Debug.LogError("信号が送信された履歴へ追加呼び出しの失敗");
                            //            if (MainGameManager.Instance.AlgorithmOwner.MergeHistorySignalsGetedToPosted())
                            //            {
                            //                var goalNode = MainGameManager.Instance.AlgorithmOwner.HistorySignalsPosted.Where(q => q.GetComponent<GoalNodeModel>() != null).Select(q => q).ToArray()[0];
                            //                if (!goalNode.GetComponent<GoalNodeModel>().GetSignal())
                            //                    Debug.LogError("シグナル受信呼び出しの失敗");
                            //            }
                            //        }
                            //        else
                            //            Debug.Log($"GET実行停止:[{startNode.name}]");
                            //    });
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
                                        // ゴールノードが既に格納済みなら格納しない
                                        if (MainGameManager.Instance.AlgorithmOwner.HistorySignalsPosted.Where(q => q.Equals(goalNode.transform))
                                            .Select(q => q)
                                            .ToArray()
                                            .Length < 1)
                                        {
                                            if (!MainGameManager.Instance.AlgorithmOwner.AddHistorySignalsPosted(goalNode.transform))
                                                Debug.LogError("信号が送信された履歴へ追加呼び出しの失敗");
                                        }

                                        if (!common.SetDisableAllNodeCode(MainGameManager.Instance.AlgorithmOwner.HistorySignalsPosted, true))
                                            Debug.LogError("ノードコードの衝突判定を無効にする呼び出しの失敗");
                                        Observable.FromCoroutine<bool>(observer => MainGameManager.Instance.AlgorithmOwner.PlayRunLightningSignal(observer))
                                            .Subscribe(_ =>
                                            {
                                                if (common.IsOvercounterOfSeastarGage(seastarGageView) &&
                                                    common.IsOvercounterOfShrimpDance(shrimpGageCount.Value, shrimpGageCountMax))
                                                {
                                                    if (seastarGageView != null)
                                                    {
                                                        if (!seastarGageView.SetImageDisable())
                                                            Debug.LogError("イメージを非表示呼び出しの失敗");
                                                        if (!seastarGageView.SetSpriteBreak())
                                                            Debug.LogError("壊れるスプライトをセット呼び出しの失敗");
                                                    }
                                                    if (!common.SetDisableAllNodeCode(MainGameManager.Instance.AlgorithmOwner.HistorySignalsPosted, false))
                                                        Debug.LogError("ノードコードの衝突判定を無効にする呼び出しの失敗");
                                                    // スタートからゴールまで繋がっている状態ならリセットしない
                                                    Debug.Log($"クリア条件:{string.Join("/", MainGameManager.Instance.AlgorithmOwner.HistorySignalsPosted.Select(q => q.GetComponent<PivotConfig>().EnumNodeCodeID).ToArray())}");
                                                    var isBugFixed = false;
                                                    // HistorySignalsPostedからノードコードの組み合わせを参照
                                                    foreach (var item in mainSceneStagesModulesState.Where(q => q[EnumMainSceneStagesModulesState.SceneId].Equals(currentStageDic[EnumSystemCommonCash.SceneId] + "") &&
                                                        q[EnumMainSceneStagesModulesState.Terms].Equals(string.Join("/", MainGameManager.Instance.AlgorithmOwner.HistorySignalsPosted.Select(q => q.GetComponent<PivotConfig>().EnumNodeCodeID).ToArray()))).Select(q => q))
                                                    {
                                                        isBugFixed = item[EnumMainSceneStagesModulesState.Fixed].Equals(ConstGeneric.DIGITFORM_TRUE);
                                                    }
                                                    // 取り出したバグのモデルを監視
                                                    if (!goalNode.GetComponent<GoalNodeView>().bugfix())
                                                        Debug.LogError("バグフィックス呼び出しの失敗");
                                                    var bug = goalNode.GetComponent<GoalNodeView>().InstanceBug;
                                                    if (!bug.GetComponent<BugView>().SetColorCleared(isBugFixed))
                                                        Debug.LogError("カラーを設定呼び出しの失敗");
                                                    if (!bug.GetComponent<BugView>().PlayBugAura())
                                                        Debug.LogError("バグオーラを発生呼び出しの失敗");
                                                    bug.GetComponent<BugModel>().IsEated.ObserveEveryValueChanged(x => x.Value)
                                                        .Subscribe(x =>
                                                        {
                                                            if (x)
                                                            {
                                                                // HistorySignalsPostedの内容を保存する
                                                                foreach (var item in mainSceneStagesModulesState.Where(q => q[EnumMainSceneStagesModulesState.SceneId].Equals(currentStageDic[EnumSystemCommonCash.SceneId] + "") &&
                                                                    q[EnumMainSceneStagesModulesState.Terms].Equals(string.Join("/", MainGameManager.Instance.AlgorithmOwner.HistorySignalsPosted.Select(q => q.GetComponent<PivotConfig>().EnumNodeCodeID).ToArray()))).Select(q => q))
                                                                {
                                                                    item[EnumMainSceneStagesModulesState.Fixed] = ConstGeneric.DIGITFORM_TRUE;
                                                                }
                                                                if (!playerModel.AutoPlayPunchAction())
                                                                    Debug.LogError("自動攻撃をセット呼び出しの失敗");
                                                                if (!playerModel.SetInputBan(true))
                                                                    Debug.LogError("操作禁止フラグをセット呼び出しの失敗");
                                                                if (!playerModel.SetIsBanMoveVelocity(true))
                                                                    Debug.LogError("移動制御禁止フラグをセット呼び出しの失敗");
                                                                foreach (var item in jawsHis.Where(q => q != null)
                                                                    .Select(q => q))
                                                                    if (!item.GetComponent<JawsHiModel>().SetIsCollisionBan(true))
                                                                        Debug.LogError("コライダー禁止中フラグをセット呼び出しの失敗");
                                                                if (!bug.GetComponent<BugView>().PlayCorrectOrWrong())
                                                                    Debug.LogError("バグ消失パーティクルを再生呼び出しの失敗");
                                                                if (!bug.GetComponent<BugView>().StopBugAura())
                                                                    Debug.LogError("バグオーラを停止呼び出しの失敗");
                                                                Observable.FromCoroutine<bool>(observer => bug.GetComponent<BugView>().PlayFadeAnimation(observer))
                                                                    .Subscribe(_ =>
                                                                    {
                                                                        isGoalReached.Value = true;
                                                                    })
                                                                    .AddTo(gameObject);
                                                            }
                                                        });
                                                }
                                                else
                                                {
                                                    if (!common.SetDisableAllNodeCode(MainGameManager.Instance.AlgorithmOwner.HistorySignalsPosted, false))
                                                        Debug.LogError("ノードコードの衝突判定を無効にする呼び出しの失敗");
                                                    if (!common.ResetAllPostingState(MainGameManager.Instance.AlgorithmOwner.HistorySignalsPosted))
                                                        Debug.LogError("POSTのリセット呼び出しの失敗");
                                                    foreach (var item in seastarModels)
                                                        if (!item.ResetIsAssigned())
                                                            Debug.LogError("アサイン情報をリセット呼び出しの失敗");
                                                    foreach (var item in shrimpDanceModels)
                                                        if (!item.ResetIsAssigned())
                                                            Debug.LogError("アサイン情報をリセット呼び出しの失敗");
                                                    isThisEmotionaBug_IsItASpecification.Value = false;
                                                }
                                            });
                                    }
                                    else
                                        Debug.Log($"POST実行停止:[{goalNode.name}]");
                                });
                            //    // 帰納法のためのGETプロセス
                            //    goalNode.GetComponent<GoalNodeModel>().IsGetting.ObserveEveryValueChanged(x => x.Value)
                            //        .Subscribe(x =>
                            //        {
                            //            if (x)
                            //            {
                            //                Debug.Log($"GET実行中:[{goalNode.name}]");
                            //                if (!MainGameManager.Instance.AlgorithmOwner.AddHistorySignalsGeted(goalNode.transform, true))
                            //                    Debug.LogError("信号が送信された履歴へ追加呼び出しの失敗");
                            //                if (!goalNode.GetComponent<GoalNodeModel>().Getting())
                            //                    Debug.LogError("コード元を辿る呼び出しの失敗");
                            //            }
                            //            else
                            //                Debug.Log($"GET実行停止:[{goalNode.name}]");
                            //        });
                            //    goalNode.GetComponent<GoalNodeModel>().FromListLength.ObserveEveryValueChanged(x => x.Value)
                            //        .Subscribe(x =>
                            //        {
                            //            if (0 < x)
                            //            {
                            //                // ゴールノードの向き先は一つのみのため「FromLists配列番号」は0固定となる想定
                            //                foreach (var child in goalNode.GetComponent<GoalNodeModel>().FromList)
                            //                {
                            //                    if (child.GetComponent<PivotModel>() != null &&
                            //                        !child.GetComponent<PivotModel>().GetSignal(true))
                            //                        Debug.LogError("シグナル受信呼び出しの失敗");
                            //                }
                            //            }
                            //            else if (x == 0)
                            //            {
                            //                // ゴールノードの向き先は一つのみの場合、レベルデザイン上は必ず一つ以上のピボットへ繋がる想定のため、ここの処理は未到達？
                            //                isGetProcessStart.Value = false;
                            //            }
                            //            else
                            //            {
                            //                // 値がリセットされた場合
                            //                // コード側の処理は特になし
                            //            }
                            //        });
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
                                codeObjs[idx].GetComponent<PivotModel>().IsTurning.ObserveEveryValueChanged(x => x.Value)
                                    .Subscribe(x =>
                                    {
                                        if (x)
                                        {
                                            // コード回転のSE
                                            MainGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_code_normal);
                                            // IsPostingがTrueならバグフィックス状態
                                            // バグフィックス状態でコードをつつく　⇒　回転によりコードが繋がらなくなる
                                            // Histroyに含まないコード回転は無視する
                                            if (goalNode.GetComponent<GoalNodeModel>().IsPosting.Value &&
                                                0 < MainGameManager.Instance.AlgorithmOwner.HistorySignalsPosted.Where(q => q.Equals(codeObjs[idx].transform))
                                                    .Select(q => q)
                                                    .ToArray()
                                                    .Length)
                                            {
                                                var bug = goalNode.GetComponent<GoalNodeView>().InstanceBug;
                                                if (!bug.GetComponent<BugView>().StopBugAura())
                                                    Debug.LogError("バグオーラを停止呼び出しの失敗");
                                                Observable.FromCoroutine<bool>(observer => goalNode.GetComponent<GoalNodeView>().degrad(observer))
                                                    .Subscribe(_ =>
                                                    {
                                                        Destroy(bug.gameObject);
                                                        // 一度全てをリセット
                                                        if (!common.ResetAllPostingState(MainGameManager.Instance.AlgorithmOwner.HistorySignalsPosted))
                                                            Debug.LogError("POSTのリセット呼び出しの失敗");
                                                        foreach (var item in seastarModels)
                                                            if (!item.ResetIsAssigned())
                                                                Debug.LogError("アサイン情報をリセット呼び出しの失敗");
                                                        foreach (var item in shrimpDanceModels)
                                                            if (!item.ResetIsAssigned())
                                                                Debug.LogError("アサイン情報をリセット呼び出しの失敗");
                                                        if (common.IsFinalLevel(areaUnits, currentStageDic))
                                                            isThisEmotionaBug_IsItASpecification.Value = false;
                                                    })
                                                    .AddTo(gameObject);
                                            }
                                        }
                                    });
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
                                                if (child.GetComponent<PivotModel>() != null)
                                                    if (!child.GetComponent<PivotModel>().GetSignal())
                                                        Debug.LogError("シグナル受信呼び出しの失敗");
                                                if (child.GetComponent<GoalNodeModel>() != null)
                                                {
                                                    if (!child.GetComponent<GoalNodeModel>().GetSignal())
                                                        Debug.LogError("シグナル受信呼び出しの失敗");
                                                }
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
                                // 最終ステージのみ実行
                                if (common.IsFinalLevel(areaUnits, currentStageDic) &&
                                    codeObjs[idx].GetComponent<PivotConfig>().EmotionsCodeMode)
                                    codeObjs[idx].GetComponent<PivotModel>().IsPathEmotions.ObserveEveryValueChanged(x => x.Value)
                                        .Subscribe(x =>
                                        {
                                            isThisEmotionaBug_IsItASpecification.Value = x;
                                        });
                                //codeObjs[idx].GetComponent<PivotModel>().IsGetting.ObserveEveryValueChanged(x => x.Value)
                                //    .Subscribe(x =>
                                //    {
                                //        if (x)
                                //        {
                                //            Debug.Log($"GET実行中:[{codeObjs[idx].name}]");
                                //            if (!MainGameManager.Instance.AlgorithmOwner.AddHistorySignalsGeted(codeObjs[idx].transform))
                                //                Debug.LogError("信号が送信された履歴へ追加呼び出しの失敗");
                                //        }
                                //        else
                                //            Debug.Log($"GET実行停止:[{codeObjs[idx].name}]");
                                //    });
                                //codeObjs[idx].GetComponent<PivotModel>().FromListLength.ObserveEveryValueChanged(x => x.Value)
                                //    .Subscribe(x =>
                                //    {
                                //        if (0 < x)
                                //        {
                                //            // FromListLengthsのObserveEveryValueChangedからは、どのリストが更新されたか判断できないため全リストを繰り返し実行させる
                                //            // 一度、「GetSignal」されたものは、フラグが更新済みとなるため新たに処理が発生しない＝空実行となる
                                //            foreach (var child in codeObjs[idx].GetComponent<PivotModel>().FromList)
                                //            {
                                //                if (child.GetComponent<PivotModel>() != null &&
                                //                    !child.GetComponent<PivotModel>().GetSignal(true))
                                //                    Debug.LogError("シグナル受信呼び出しの失敗");
                                //                if (child.GetComponent<StartNodeModel>() != null &&
                                //                    !child.GetComponent<StartNodeModel>().GetSignal(true))
                                //                    Debug.LogError("シグナル受信呼び出しの失敗");
                                //            }
                                //        }
                                //        else if (x == 0)
                                //        {
                                //            if (MainGameManager.Instance.AlgorithmOwner.MergeHistorySignalsGetedToPosted())
                                //            {
                                //                var goalNode = MainGameManager.Instance.AlgorithmOwner.HistorySignalsPosted.Where(q => q.GetComponent<GoalNodeModel>() != null).Select(q => q).ToArray()[0];
                                //                if (!goalNode.GetComponent<GoalNodeModel>().GetSignal())
                                //                    Debug.LogError("シグナル受信呼び出しの失敗");
                                //            }
                                //            else
                                //                isGetProcessStart.Value = false;
                                //        }
                                //        else
                                //        {
                                //            // 値がリセットされた場合
                                //            // コード側の処理は特になし
                                //        }
                                //    });
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
                                    //// 帰納法処理：ゴールノードからスタートノードまでコードが繋がっているか
                                    //if (goalNode != null &&
                                    //    goalNode.GetComponent<PivotConfig>() != null &&
                                    //    goalNode.GetComponent<PivotConfig>().EnumAtomicMode.Equals(EnumAtomicMode.Molecules) &&
                                    //    goalNode.GetComponent<GoalNodeModel>() != null &&
                                    //    !goalNode.GetComponent<GoalNodeModel>().IsGetting.Value)
                                    //{
                                    //    if (!goalNode.GetComponent<GoalNodeModel>().SetIsGetting(true))
                                    //        Debug.LogError("信号受信フラグをセット呼び出しの失敗");
                                    //}
                                    //else
                                    //{
                                    // 上記でもモジュールが繋がっていない状態として判断された場合リセットする
                                    if (!common.SetDisableAllNodeCode(MainGameManager.Instance.AlgorithmOwner.HistorySignalsPosted, true))
                                        Debug.LogError("ノードコードの衝突判定を無効にする呼び出しの失敗");
                                    Observable.FromCoroutine<bool>(observer => MainGameManager.Instance.AlgorithmOwner.PlayRunLightningSignal(observer))
                                        .Subscribe(_ =>
                                        {
                                            if (!common.SetDisableAllNodeCode(MainGameManager.Instance.AlgorithmOwner.HistorySignalsPosted, false))
                                                Debug.LogError("ノードコードの衝突判定を無効にする呼び出しの失敗");
                                            if (!common.ResetAllPostingState(MainGameManager.Instance.AlgorithmOwner.HistorySignalsPosted))
                                                Debug.LogError("POSTのリセット呼び出しの失敗");
                                            foreach (var item in seastarModels)
                                                if (!item.ResetIsAssigned())
                                                    Debug.LogError("アサイン情報をリセット呼び出しの失敗");
                                            foreach (var item in shrimpDanceModels)
                                                if (!item.ResetIsAssigned())
                                                    Debug.LogError("アサイン情報をリセット呼び出しの失敗");
                                            isThisEmotionaBug_IsItASpecification.Value = false;
                                        });
                                    //}
                                }
                                else
                                {
                                    Debug.Log("Getプロセス初期状態／停止");

                                    // 帰納法処理ありの場合の信号発生演出用
                                    if (goalNode != null &&
                                        goalNode.GetComponent<PivotConfig>() != null &&
                                        goalNode.GetComponent<PivotConfig>().EnumAtomicMode.Equals(EnumAtomicMode.Molecules))
                                    {
                                        // ゴールまで繋がる⇒コード回転で再び未接続の状態は信号発生演出しない
                                        if (MainGameManager.Instance.AlgorithmOwner.HistorySignalsPosted != null &&
                                            0 < MainGameManager.Instance.AlgorithmOwner.HistorySignalsPosted.Where(q => q.GetComponent<GoalNodeModel>() != null)
                                            .Select(q => q)
                                            .ToArray().Length)
                                        {
                                            if (!common.ResetAllPostingState(MainGameManager.Instance.AlgorithmOwner.HistorySignalsPosted))
                                                Debug.LogError("POSTのリセット呼び出しの失敗");
                                            foreach (var item in seastarModels)
                                                if (!item.ResetIsAssigned())
                                                    Debug.LogError("アサイン情報をリセット呼び出しの失敗");
                                            foreach (var item in shrimpDanceModels)
                                                if (!item.ResetIsAssigned())
                                                    Debug.LogError("アサイン情報をリセット呼び出しの失敗");
                                            isThisEmotionaBug_IsItASpecification.Value = false;
                                        }
                                        else
                                        {
                                            //if (!common.SetDisableAllNodeCode(MainGameManager.Instance.AlgorithmOwner.HistorySignalsPosted, true))
                                            //    Debug.LogError("ノードコードの衝突判定を無効にする呼び出しの失敗");
                                            //Observable.FromCoroutine<bool>(observer => MainGameManager.Instance.AlgorithmOwner.PlayRunLightningSignal(observer))
                                            //    .Subscribe(_ =>
                                            //    {
                                            //        if (!common.SetDisableAllNodeCode(MainGameManager.Instance.AlgorithmOwner.HistorySignalsPosted, false))
                                            //            Debug.LogError("ノードコードの衝突判定を無効にする呼び出しの失敗");
                                            //        if (!common.ResetAllPostingState(MainGameManager.Instance.AlgorithmOwner.HistorySignalsPosted))
                                            //            Debug.LogError("POSTのリセット呼び出しの失敗");
                                            //        foreach (var item in seastarModels)
                                            //            if (!item.ResetIsAssigned())
                                            //                Debug.LogError("アサイン情報をリセット呼び出しの失敗");
                                            //        foreach (var item in shrimpDanceModels)
                                            //            if (!item.ResetIsAssigned())
                                            //                Debug.LogError("アサイン情報をリセット呼び出しの失敗");
                                            //    });
                                        }
                                    }
                                    //if (MainGameManager.Instance.AlgorithmOwner.HistorySignalsGeted != null)
                                    //{
                                    //    // GETのリセット
                                    //    foreach (var item in MainGameManager.Instance.AlgorithmOwner.HistorySignalsGeted)
                                    //    {
                                    //        foreach (var itemChild in item)
                                    //        {
                                    //            if (itemChild.GetComponent<GoalNodeModel>() != null)
                                    //            {
                                    //                // GETの場合はアニメーション不要のためリセット呼び出しのみ
                                    //                if (!itemChild.GetComponent<GoalNodeModel>().SetIsGetting(false))
                                    //                    Debug.LogError("信号受信中フラグをセット呼び出しの失敗");
                                    //                if (!itemChild.GetComponent<GoalNodeModel>().SetFromListLength(-1))
                                    //                    Debug.LogError("GET元のノードコードリスト数をセット呼び出しの失敗");
                                    //            }
                                    //            if (itemChild.GetComponent<PivotView>() != null)
                                    //            {
                                    //                // GETの場合はアニメーション不要のためリセット呼び出しのみ
                                    //                if (!itemChild.GetComponent<PivotModel>().SetIsGetting(false))
                                    //                    Debug.LogError("信号受信中フラグをセット呼び出しの失敗");
                                    //                if (!itemChild.GetComponent<PivotModel>().SetFromListLength(-1))
                                    //                    Debug.LogError("GET元のノードコードリスト数をセット呼び出しの失敗");
                                    //            }
                                    //            if (itemChild.GetComponent<StartNodeView>() != null)
                                    //            {
                                    //                // GETの場合はアニメーション不要のためリセット呼び出しのみ
                                    //                if (!itemChild.GetComponent<StartNodeModel>().SetIsGetting(false))
                                    //                    Debug.LogError("信号受信中フラグをセット呼び出しの失敗");
                                    //            }
                                    //        }
                                    //    }
                                    //}
                                }
                            });
                        // ルール貝
                        var ruleShellfish = GameObject.Find(ConstTagNames.RULESHELLFISH);
                        if (ruleShellfish != null)
                        {
                            if (common.IsOpeningTutorialMode())
                            {
                                if (!ruleShellfish.GetComponent<RuleShellfishView>().SetColorSpriteIsVisible(true))
                                    Debug.LogError("スプライトの表示／非表示設定呼び出しの失敗");
                            }
                            else
                            {
                                if (!ruleShellfish.GetComponent<RuleShellfishView>().SetColorSpriteIsVisible(!ruleShellfish.GetComponent<RuleShellfishModel>().IsOnlyOnceHint ||
                                    common.IsTutorialMode(currentStageDic, mainSceneStagesState)))
                                    Debug.LogError("スプライトの表示／非表示設定呼び出しの失敗");
                            }
                            if (common.IsOpeningTutorialMode())
                                if (!ruleShellfish.GetComponent<RuleShellfishModel>().SetColliderState(false))
                                    Debug.LogError("コライダーの状態をセット呼び出しの失敗");
                            if (ruleShellfish.GetComponent<RuleShellfishView>().IsVisible)
                                ruleShellfish.GetComponent<RuleShellfishModel>().IsInRange.ObserveEveryValueChanged(x => x.Value)
                                    .Subscribe(x =>
                                    {
                                        if (x)
                                        {
                                            if (!playerModel.SetInputBan(true))
                                                Debug.LogError("操作禁止フラグをセット呼び出しの失敗");
                                            if (!playerModel.SetIsBanMoveVelocity(true))
                                                Debug.LogError("移動制御禁止フラグをセット呼び出しの失敗");
                                            foreach (var item in jawsHis.Where(q => q != null)
                                                .Select(q => q))
                                                if (!item.GetComponent<JawsHiModel>().SetIsCollisionBan(true))
                                                    Debug.LogError("コライダー禁止中フラグをセット呼び出しの失敗");
                                            Observable.FromCoroutine<bool>(observer => ruleShellfish.GetComponent<RuleShellfishView>().PlayChangeSpriteCloseBetweenOpen(observer))
                                                .Subscribe(_ =>
                                                {
                                                    if (!common.SendReceiver(receivers, flowchartModel, currentStageDic, common.IsOpeningTutorialMode() ? 1 : -1))
                                                        Debug.LogError("シナリオのレシーバーへ送信呼び出しの失敗");
                                                })
                                                .AddTo(gameObject);
                                        }
                                    });
                            else
                                Debug.LogWarning("クリア済みのためルール貝非表示");
                        }
                        else
                        {
                            Debug.LogWarning("ルール貝無しステージ");
                        }
                        // ロボットヘッド（修理前）
                        var robotHeadBefore = GameObject.Find(ConstTagNames.TAG_NAME_ROBOTHEADBEFORE);
                        if (robotHeadBefore != null)
                        {
                            var model = robotHeadBefore.GetComponent<RobotHeadBeforeModel>();
                            model.IsCollision.ObserveEveryValueChanged(x => x.Value)
                                .Subscribe(x =>
                                {
                                    if (x)
                                    {
                                        //var target = playerView.InstanceGhost();
                                        //if (!cinemachineVirtualCameraView.SetFollow(target))
                                        //    Debug.LogError("フォローをセット呼び出しの失敗");
                                        //if (!cinemachineVirtualCameraView.SetLookAt(target))
                                        //    Debug.LogError("フォローをセット呼び出しの失敗");
                                        //Observable.FromCoroutine<bool>(observer => cinemachineVirtualCameraView.SetFollowAnimation(observer, robotHeadBefore.transform.GetChild(0)))
                                        //    .Subscribe(_ =>
                                        //    {
                                        //    })
                                        //    .AddTo(gameObject);
                                        //Observable.FromCoroutine<bool>(observer => cinemachineVirtualCameraView.SetLookAtAnimation(observer, robotHeadBefore.transform.GetChild(0)))
                                        //    .Subscribe(_ =>
                                        //    {
                                        //    })
                                        //    .AddTo(gameObject);
                                        if (!playerModel.SetInputBan(true))
                                            Debug.LogError("操作禁止フラグをセット呼び出しの失敗");
                                        if (!playerModel.SetIsBanMoveVelocity(true))
                                            Debug.LogError("移動制御禁止フラグをセット呼び出しの失敗");
                                        if (!common.SendReceiver(receivers, flowchartModel, currentStageDic, 2))
                                            Debug.LogError("シナリオのレシーバーへ送信呼び出しの失敗");
                                    }
                                });
                        }
                    }
                });
            // シナリオ管理
            flowchartModel.ReadedScenarioNo.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    if (0 < x)
                    {
                        switch (x)
                        {
                            case 1:
                                // 実績履歴を更新
                                if (common.AddMissionHistory() < 1)
                                    Debug.LogError("実績履歴を更新呼び出しの失敗");
                                var owner = MainGameManager.Instance.SceneOwner;
                                if (!owner.SetSystemCommonCash(owner.CountUpSceneId(currentStageDic)))
                                    Debug.LogError("シーンID更新呼び出しの失敗");
                                Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Close))
                                    .Subscribe(_ =>
                                    {
                                        // イベント完了後の処理
                                        MainGameManager.Instance.SceneOwner.LoadSelectScene();
                                    })
                                    .AddTo(gameObject);
                                break;
                            case 2:
                                if (!playerModel.SetInputBan(false, true))
                                    Debug.LogError("操作禁止フラグをセット呼び出しの失敗");
                                if (!playerModel.SetIsBanMoveVelocity(false))
                                    Debug.LogError("移動制御禁止フラグをセット呼び出しの失敗");
                                // ジョーシー
                                var jawsHis = GameObject.FindGameObjectsWithTag(ConstTagNames.TAG_NAME_JAWSHI);
                                foreach (var item in jawsHis.Where(q => q != null)
                                    .Select(q => q))
                                    if (!item.GetComponent<JawsHiModel>().SetIsCollisionBan(false))
                                        Debug.LogError("コライダー禁止中フラグをセット呼び出しの失敗");
                                if (common.IsOpeningTutorialMode())
                                {
                                    var ruleShellfish = GameObject.Find(ConstTagNames.RULESHELLFISH);
                                    if (!ruleShellfish.GetComponent<RuleShellfishModel>().SetColliderState(true))
                                        Debug.LogError("コライダーの状態をセット呼び出しの失敗");
                                    if (!flowchartModel.SetReadedScenarioNo(0))
                                        Debug.LogError("シナリオ番号をセット呼び出しの失敗");
                                }
                                break;
                            case 3:
                                Observable.FromCoroutine<bool>(observer => playerView.PlayMoveAnimation(observer, flowchartModel.AutoMoveTrackers[0].transform.position))
                                    .Subscribe(_ => { })
                                    .AddTo(gameObject);
                                break;
                            case 4:
                                Observable.FromCoroutine<bool>(observer => playerView.PlayMoveAnimation(observer, flowchartModel.AutoMoveTrackers[1].transform.position))
                                    .Subscribe(_ =>
                                    {
                                        MainGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_decided);
                                    })
                                    .AddTo(gameObject);
                                break;
                            default:
                                break;
                        }
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
