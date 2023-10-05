using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Select.Common;
using Select.View;
using Select.Model;
using UniRx;
using UniRx.Triggers;
using Select.Audio;
using System.Linq;
using Fungus;
using GameManagers.Common;

namespace Select.Presenter
{
    /// <summary>
    /// プレゼンタ
    /// セレクトシーン
    /// </summary>
    public class SelectPresenter : MonoBehaviour, ISelectGameManager
    {
        /// <summary>ページのビュー</summary>
        [SerializeField] private PageView[] pageViews;
        /// <summary>ページのモデル</summary>
        [SerializeField] private PageModel[] pageModels;
        /// <summary>ロゴステージのビュー</summary>
        [SerializeField] private LogoStageView[] logoStageViews;
        /// <summary>ロゴステージのモデル</summary>
        [SerializeField] private LogoStageModel[] logoStageModels;
        /// <summary>支点とコードのビュー</summary>
        [SerializeField] private PivotAndCodeIShortUIView[] pivotAndCodeIShortUIViews;
        /// <summary>支点とコードのモデル</summary>
        [SerializeField] private PivotAndCodeIShortUIModel[] pivotAndCodeIShortUIModels;
        /// <summary>ステージキャプションのビュー</summary>
        [SerializeField] private CaptionStageView[] captionStageViews;
        /// <summary>ステージキャプションのモデル</summary>
        [SerializeField] private CaptionStageModel[] captionStageModels;
        /// <summary>FadeImageのビュー</summary>
        [SerializeField] private FadeImageView fadeImageView;
        /// <summary>Fadeimageのモデル</summary>
        [SerializeField] private FadeImageModel fadeImageModel;
        /// <summary>プレイヤーのフレームのビュー</summary>
        [SerializeField] private PlayerView playerView;
        /// <summary>ロゴステージの統括パネルのビュー</summary>
        [SerializeField] private LogoStagesView logoStagesView;
        /// <summary>準委任帳票のビュー</summary>
        [SerializeField] private AssignedSeastarCountView assignedSeastarCountView;
        /// <summary>ヒトデゲージのビュー</summary>
        [SerializeField] private SeastarGageView[] seastarGageViews;
        /// <summary>Fungusのレシーバー</summary>
        [SerializeField] private MessageReceived[] receivers;
        /// <summary>Fungusのフローチャートモデル</summary>
        [SerializeField] private FlowchartModel flowchartModel;
        /// <summary>コアのビュー</summary>
        [SerializeField] private RobotHeartView[] robotHeartViews;
        /// <summary>コアのモデル</summary>
        [SerializeField] private RobotHeartModel[] robotHeartModels;
        /// <summary>キャンバスのモデル</summary>
        [SerializeField] private CanvasModel canvasModel;
        /// <summary>プレイヤーのモデル</summary>
        [SerializeField] private PlayerModel playerModel;

        private void Reset()
        {
            var logoStages = GameObject.Find("LogoStages").transform;

            // ページのビューとモデルをセット
            List<PageView> pageViewList = new List<PageView>();
            List<PageModel> pageModelList = new List<PageModel>();
            foreach (Transform child in logoStages)
            {
                pageViewList.Add(child.GetComponent<PageView>());
                pageModelList.Add(child.GetComponent<PageModel>());
            }
            pageViews = pageViewList.ToArray();
            pageModels = pageModelList.ToArray();

            // ロゴステージのビューとモデルをセット
            List<LogoStageView> logoStageViewList = new List<LogoStageView>();
            List<LogoStageModel> logoStageModelList = new List<LogoStageModel>();
            // 支点とコードのビューとモデルをセット
            List<PivotAndCodeIShortUIView> pivotAndCodeIShortUIViewList = new List<PivotAndCodeIShortUIView>();
            List<PivotAndCodeIShortUIModel> pivotAndCodeIShortUIModelList = new List<PivotAndCodeIShortUIModel>();
            // コアのビューとモデルをセット
            List<RobotHeartView> robotHeartViewList = new List<RobotHeartView>();
            List<RobotHeartModel> robotHeartModelList = new List<RobotHeartModel>();
            foreach (Transform pages in logoStages)
            {
                foreach (Transform child in pages)
                {
                    if (-1 < child.name.IndexOf("LogoStage"))
                    {
                        logoStageViewList.Add(child.GetComponent<LogoStageView>());
                        logoStageModelList.Add(child.GetComponent<LogoStageModel>());
                    }
                    else if (-1 < child.name.IndexOf("PivotAndCodeIShortUI"))
                    {
                        pivotAndCodeIShortUIViewList.Add(child.GetComponent<PivotAndCodeIShortUIView>());
                        pivotAndCodeIShortUIModelList.Add(child.GetComponent<PivotAndCodeIShortUIModel>());
                    }
                    else if (-1 < child.name.IndexOf("RobotHeart"))
                    {
                        robotHeartViewList.Add(child.GetComponent<RobotHeartView>());
                        robotHeartModelList.Add(child.GetComponent<RobotHeartModel>());
                    }
                }
            }
            logoStageViews = logoStageViewList.ToArray();
            logoStageModels = logoStageModelList.ToArray();
            pivotAndCodeIShortUIViews = pivotAndCodeIShortUIViewList.ToArray();
            pivotAndCodeIShortUIModels = pivotAndCodeIShortUIModelList.ToArray();
            robotHeartViews = robotHeartViewList.ToArray();
            robotHeartModels = robotHeartModelList.ToArray();

            // ステージキャプションのビューとモデルをセット
            List<CaptionStageView> captionStageViewList = new List<CaptionStageView>();
            List<CaptionStageModel> captionStageModelList = new List<CaptionStageModel>();
            var caption = GameObject.Find("Caption").transform;
            foreach (Transform child in caption)
            {
                captionStageViewList.Add(child.GetComponent<CaptionStageView>());
                captionStageModelList.Add(child.GetComponent<CaptionStageModel>());
            }
            captionStageViews = captionStageViewList.ToArray();
            captionStageModels = captionStageModelList.ToArray();

            // フェードのビューとモデルをセット
            fadeImageView = GameObject.Find("FadeImage").GetComponent<FadeImageView>();
            fadeImageModel = GameObject.Find("FadeImage").GetComponent<FadeImageModel>();

            playerView = GameObject.Find("Player").GetComponent<PlayerView>();
            logoStagesView = GameObject.Find("LogoStages").GetComponent<LogoStagesView>();
            List<SeastarGageView> seastarGageViewList = new List<SeastarGageView>();
            foreach (Transform item in logoStagesView.transform)
            {
                seastarGageViewList.Add(item.GetComponent<PageView>() != null ? item.GetComponentInChildren<SeastarGageView>() : null);
            }
            seastarGageViews = seastarGageViewList.ToArray();
            assignedSeastarCountView = GameObject.Find("AssignedSeastarCount").GetComponent<AssignedSeastarCountView>();
            receivers = GameObject.FindObjectsOfType<Fungus.MessageReceived>();
            flowchartModel = GameObject.Find("Flowchart").GetComponent<FlowchartModel>();
            canvasModel = GameObject.Find("Canvas").GetComponent<CanvasModel>();
            playerModel = GameObject.Find("Player").GetComponent<PlayerModel>();
        }

        public void OnStart()
        {
            var common = new SelectPresenterCommon();
            // 初期設定
            foreach (var child in logoStageModels)
                if (child != null)
                {
                    if (!child.SetButtonEnabled(false))
                        Debug.LogError("ボタンのステータスを変更呼び出しの失敗");
                    if (!child.SetEventTriggerEnabled(false))
                        Debug.LogError("イベントトリガーのステータスを変更呼び出しの失敗");
                    if (!child.LoadStateAndUpdateNavigation())
                        Debug.LogError("ステージ状態のロード及びナビゲーション更新呼び出しの失敗");
                }
            GameManager.Instance.SoftwareCursorPositionAdjusterView.OnStart();
            if (!GameManager.Instance.SoftwareCursorPositionAdjusterView.SetCursorEnabled(false))
                Debug.LogError("カーソルのステータスを変更呼び出しの失敗");
            foreach (var child in pageViews)
                if (child != null)
                {
                    if (!child.SetVisible(false))
                        Debug.LogError("アルファ値切り替え処理の失敗");
                    if (!child.SetBlocksRaycasts(false))
                        Debug.LogError("レイキャスト判定対象を設定の失敗");
                }
            foreach (var child in robotHeartModels)
                if (child != null)
                {
                    if (!child.SetButtonEnabled(false))
                        Debug.LogError("ボタンのステータスを変更呼び出しの失敗");
                    if (!child.SetEventTriggerEnabled(false))
                        Debug.LogError("イベントトリガーのステータスを変更呼び出しの失敗");
                    //if (!child.LoadStateAndUpdateNavigation())
                    //    Debug.LogError("ステージ状態のロード及びナビゲーション更新呼び出しの失敗");
                }
            // シーンIDに紐づくキャプションかつ、そのキャプション内にもつSeastarIDをもつオブジェクトを抽出
            if (!common.SetIsAssignedAllCaption(captionStageModels))
                Debug.LogError("全ステージキャプションのアサイン情報をセット呼び出しの失敗");
            foreach (var item in captionStageModels.Where(q => q != null).Select(q => q))
                item.gameObject.SetActive(false);
            if (!assignedSeastarCountView.SetCounterText(SelectGameManager.Instance.GimmickOwner.GetAssinedCounter()))
                Debug.LogError("ヒトデ総配属人数をセット呼び出しの失敗");
            foreach (var item in pivotAndCodeIShortUIViews)
                item.OnStart();
            if (SelectGameManager.Instance.AlgorithmOwner.SetPivotAndCodeIShortUIs(pivotAndCodeIShortUIViews.Select(q => q.transform).ToArray()) < 1)
                Debug.LogError("支点とコード配列をセット呼び出しの失敗");
            // 会話イベント発生
            if (!common.CheckMissionAndSaveDatasCSVOfMission())
                Debug.LogError("ミッションの更新チェック呼び出しの失敗");

            SelectGameManager.Instance.AudioOwner.PlayBGM(ClipToPlayBGM.bgm_select);
            // シーン読み込み時のアニメーション
            Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Open))
                .Subscribe(_ =>
                {
                    // UI操作を許可
                    foreach (var child in logoStageModels)
                        if (child != null)
                        {
                            child.SetButtonEnabled(true);
                            child.SetEventTriggerEnabled(true);
                        }
                    if (!GameManager.Instance.SoftwareCursorPositionAdjusterView.SetCursorEnabled(true))
                        Debug.LogError("カーソルのステータスを変更呼び出しの失敗");
                    if (!common.SetCounterBetweenAndFillAmountAllGage(seastarGageViews))
                        Debug.LogError("全ヒトデゲージのカウンターとフィルターをセット呼び出しの失敗");
                    // コア解放演出
                    if (common.IsConnectedAnimation())
                    {
                        // 実績履歴を更新
                        if (common.AddMissionHistory() < 1)
                            Debug.LogError("実績履歴を更新呼び出しの失敗");
                        if (!common.OpenCoreReleaseDirections(logoStageModels, seastarGageViews, robotHeartModels, gameObject))
                            Debug.LogError("コア解放演出後に各オブジェクトステータスを変更呼び出しの失敗");
                    }
                    else
                    {
                        if (common.IsMissionUnlockAndFoundHistory(EnumMissionID.MI0006))
                        {
                            foreach (var child in logoStageModels)
                                if (child != null)
                                    if (!child.LoadStateAndUpdateNavigation())
                                        Debug.LogError("ステージ状態のロード及びナビゲーション更新呼び出しの失敗");
                            if (!common.OpenCoreReleaseDirections(logoStageModels, seastarGageViews, robotHeartModels, gameObject))
                                Debug.LogError("コア解放演出後に各オブジェクトステータスを変更呼び出しの失敗");
                        }
                        if (common.IsMissionUnlockAndFoundHistory(EnumMissionID.MI0007))
                        {
                            // 二つ目のコア解放
                            if (!robotHeartModels[1].SetButtonEnabled(true))
                                Debug.LogError("ボタンのステータスを変更呼び出しの失敗");
                            if (!robotHeartModels[1].SetEventTriggerEnabled(true))
                                Debug.LogError("イベントトリガーのステータスを変更呼び出しの失敗");
                        }
                    }
                })
                .AddTo(gameObject);

            // エリア解放・結合テスト済みデータ
            var areaOpenedAndITState = SelectGameManager.Instance.SceneOwner.GetAreaOpenedAndITState();
            // ステージ番号を取得する処理を追加する
            var sysCommonCash = SelectGameManager.Instance.SceneOwner.GetSystemCommonCash();
            var stageIndex = new IntReactiveProperty(sysCommonCash[EnumSystemCommonCash.SceneId]);
            if (!playerView.SelectPlayer(logoStageViews[stageIndex.Value].transform.position, logoStageViews[stageIndex.Value].transform))
                Debug.LogError("ステージ選択のプレイヤーを移動して選択させる呼び出しの失敗");
            if (!GameManager.Instance.SoftwareCursorPositionAdjusterView.SetCursorTransform(logoStageViews[stageIndex.Value].transform.position))
                Debug.LogError("カーソルのアンカーをセットする呼び出しの失敗");

            // クリア済みマークの表示
            for (var i = 0; i < logoStageModels.Length; i++)
            {
                if (i == 0)
                    // 0番目は空データのためスキップ
                    continue;
                var idx = i;
                logoStageModels[idx].StageState.ObserveEveryValueChanged(x => x.Value)
                    .Subscribe(x =>
                    {
                        switch (x)
                        {
                            case -1:
                                // 処理無し
                                break;
                            case 0:
                                if (!logoStageViews[idx].RenderDisableMark())
                                    Debug.LogError("選択不可マーク表示呼び出しの失敗");
                                break;
                            case 1:
                                // 選択可
                                if (!logoStageViews[idx].RenderEnabled())
                                    Debug.LogError("選択可表示呼び出しの失敗");
                                break;
                            case 2:
                                if (!logoStageViews[idx].RenderClearMark())
                                    Debug.LogError("クリア済みマーク表示呼び出しの失敗");
                                break;
                            default:
                                Debug.LogWarning("例外ケース");
                                break;
                        }
                    });
            }
            // 選択ステージ番号の更新
            stageIndex.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    // ページ表示切り替え
                    var pageIdx = common.GetContentsCountInPage();
                    if (pageIdx < 1)
                        Debug.LogError("ステージIDに基づいたページ番号を取得する呼び出しの失敗");
                    for (var i = 0; i < pageViews.Length; i++)
                    {
                        if (i == 0)
                            // 0ページは存在しないためスキップ
                            continue;
                        // 該当ページのみ表示させる
                        if (!pageViews[i].SetVisible(i == pageIdx))
                            Debug.LogError("アルファ値切り替え処理の失敗");
                        if (!pageViews[i].SetBlocksRaycasts(i == pageIdx))
                            Debug.LogError("レイキャスト判定対象を設定の失敗");
                    }
                });

            // パーティクルを生成対象が動く為、一時的にUpdateで監視
            var updateObservable = this.UpdateAsObservable().Subscribe(_ => { });
            // ステージ選択の操作
            foreach (var child in logoStageModels.Where(q => q != null).Select(q => q))
            {
                if (child.Index == stageIndex.Value)
                {
                    if (!logoStageViews[child.Index].SetRenderEnableBackgroundFrame())
                        Debug.LogError("選択中フレームを表示呼び出しの失敗");
                }
                else
                {
                    if (!logoStageViews[child.Index].SetRenderDisableBackgroundFrame())
                        Debug.LogError("選択中フレームを非表示呼び出しの失敗");
                }

                child.EventState.ObserveEveryValueChanged(x => x.Value)
                    .Subscribe(x =>
                    {
                        switch ((EnumEventCommand)x)
                        {
                            case EnumEventCommand.Default:
                                // 処理無し
                                break;
                            case EnumEventCommand.Selected:
                                // 選択SEを再生
                                if (!playerView.IsSkipMode)
                                {
                                    SelectGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_select);
                                    Observable.FromCoroutine<bool>(observer => logoStageViews[child.Index].PlayRenderEnableBackgroundFrame(observer))
                                        .Subscribe(_ => { })
                                        .AddTo(gameObject);
                                }
                                else
                                {
                                    if (!playerView.SetSkipMode(false))
                                        Debug.LogError("スキップモードのセット呼び出しの失敗");
                                    if (!logoStageViews[child.Index].SetRenderEnableBackgroundFrame())
                                        Debug.LogError("選択中フレームを表示呼び出しの失敗");
                                }
                                logoStageModels[child.Index].SetSelectedGameObject();
                                stageIndex.Value = child.Index;

                                break;
                            case EnumEventCommand.DeSelected:
                                logoStageModels[child.Index].SetDeSelectedGameObject();
                                Observable.FromCoroutine<bool>(observer => logoStageViews[child.Index].PlayRenderDisableBackgroundFrame(observer))
                                    .Subscribe(_ => { })
                                    .AddTo(gameObject);

                                break;
                            case EnumEventCommand.Canceled:

                                break;
                            case EnumEventCommand.Submited:
                                if (!logoStageModels[child.Index].SetButtonEnabled(false))
                                    Debug.LogError("ボタンのステータスを変更呼び出しの失敗");
                                if (!logoStageModels[child.Index].SetEventTriggerEnabled(false))
                                    Debug.LogError("イベントトリガーのステータスを変更呼び出しの失敗");
                                if (!GameManager.Instance.SoftwareCursorPositionAdjusterView.SetCursorEnabled(false))
                                    Debug.LogError("カーソルの速度を変更呼び出しの失敗");
                                Observable.FromCoroutine<bool>(observer => playerView.PlayDiveAnimation(observer, playerModel.BodyImage.Direction.Value))
                                    .Subscribe(_ =>
                                    {
                                        // 決定SEを再生
                                        SelectGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_decided);
                                        updateObservable.Dispose();
                                        if (!SelectGameManager.Instance.ParticleSystemsOwner.PlayParticleSystems(playerView.GetInstanceID(), EnumParticleSystemsIndex.Ripples, logoStageModels[child.Index].transform.position + Vector3.forward * 16f))
                                            Debug.LogError("指定されたパーティクルシステムを再生する呼び出しの失敗");
                                        var particle = SelectGameManager.Instance.ParticleSystemsOwner.GetParticleSystemsTransform(playerView.GetInstanceID(), EnumParticleSystemsIndex.Ripples);
                                        updateObservable = this.UpdateAsObservable()
                                            .Subscribe(_ => particle.position = logoStageModels[child.Index].transform.position + Vector3.forward * 16f);
                                        if (!logoStagesView.ZoomInOutPanel(child.Index))
                                            Debug.LogError("該当ステージを拡大させる呼び出しの失敗");
                                        if (!logoStageViews[child.Index].SetRenderDisableBackgroundFrame())
                                            Debug.LogError("選択中フレームを非表示呼び出しの失敗");
                                    })
                                    .AddTo(gameObject);

                                break;
                            default:
                                Debug.LogWarning("例外ケース");
                                break;
                        }
                    });
            }

            // キャンセル入力を許可／禁止
            var isEnabledCancel = new BoolReactiveProperty(true);
            // ステージが選択された状態なら、キャプションを表示にする
            logoStagesView.IsZoomed.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    if (x)
                    {
                        if (!captionStageViews[stageIndex.Value].isActiveAndEnabled)
                            captionStageViews[stageIndex.Value].gameObject.SetActive(true);
                        if (!captionStageViews[stageIndex.Value].ZoomInOutPanel(stageIndex.Value))
                            Debug.LogError("該当ステージを拡大させる呼び出しの失敗");
                        if (!captionStageModels[stageIndex.Value].SetButtonEnabled(true))
                            Debug.LogError("ボタンのステータスを変更呼び出しの失敗");
                        if (!captionStageModels[stageIndex.Value].SetEventTriggerEnabled(true))
                            Debug.LogError("イベントトリガーのステータスを変更呼び出しの失敗");
                        if (!GameManager.Instance.SoftwareCursorPositionAdjusterView.SetCursorEnabled(false))
                            Debug.LogError("カーソルの速度を変更呼び出しの失敗");
                        // デフォルト選択
                        captionStageModels[stageIndex.Value].SetSelectedGameObject();
                        if (!playerView.SetColorAlpha(0f))
                            Debug.LogError("透明度をセット呼び出しの失敗");
                        if (!playerView.SetSkipMode(true))
                            Debug.LogError("透明度をセット呼び出しの失敗");
                        isEnabledCancel.Value = false;
                    }
                    else
                    {
                        if (captionStageViews[stageIndex.Value].isActiveAndEnabled)
                        {
                            // ズームアウトされた状態
                            Observable.FromCoroutine<bool>(observer => captionStageViews[stageIndex.Value].ZoomInOutPanel(stageIndex.Value, observer))
                                .Subscribe(_ =>
                                {
                                    captionStageViews[stageIndex.Value].gameObject.SetActive(false);
                                    if (!logoStageModels[stageIndex.Value].SetButtonEnabled(true))
                                        Debug.LogError("ボタンのステータスを変更呼び出しの失敗");
                                    if (!logoStageModels[stageIndex.Value].SetEventTriggerEnabled(true))
                                        Debug.LogError("イベントトリガーのステータスを変更呼び出しの失敗");
                                    if (!GameManager.Instance.SoftwareCursorPositionAdjusterView.SetCursorEnabled(true))
                                        Debug.LogError("カーソルの速度を変更呼び出しの失敗");
                                    // デフォルト選択
                                    logoStageModels[stageIndex.Value].SetSelectedGameObject();
                                    if (!playerView.SetColorAlpha(255f))
                                        Debug.LogError("透明度をセット呼び出しの失敗");
                                    isEnabledCancel.Value = true;
                                })
                                .AddTo(gameObject);
                        }
                    }
                });

            // キャプションの状態管理
            foreach (var item in captionStageModels.Select((p, i) => new { Content = p, Index = i })
                .Where(q => 0 < q.Index))
            {
                foreach (var itemChild in item.Content.IsAssigned.Where(q => q != null)
                    .Select((p, i) => new { Content = p, Index = i }))
                {
                    itemChild.Content.ObserveEveryValueChanged(x => x.Value)
                        .Subscribe(x =>
                        {
                            if (!captionStageViews[item.Index].SetColorOfAssignedCount(item.Content.IsAssignedCount))
                                Debug.LogError("アサイン済みの数に合わせてカラー設定呼び出しの失敗");
                        });
                }
                item.Content.EventState.ObserveEveryValueChanged(x => x.Value)
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
                            case EnumEventCommand.Canceled:
                                // キャンセルSEを再生
                                SelectGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_cancel);
                                // UI操作を許可しない
                                foreach (var child in captionStageModels)
                                    if (child != null)
                                    {
                                        child.SetButtonEnabled(false);
                                        child.SetEventTriggerEnabled(false);
                                    }
                                if (!GameManager.Instance.SoftwareCursorPositionAdjusterView.SetCursorEnabled(false))
                                    Debug.LogError("カーソルの速度を変更呼び出しの失敗");
                                if (!logoStagesView.ZoomInOutPanel(stageIndex.Value, true))
                                    Debug.LogError("該当ステージを拡大させる呼び出しの失敗");

                                break;
                            case EnumEventCommand.Submited:
                                // 決定SEを再生
                                SelectGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_decided);
                                // メインシーンへの遷移
                                sysCommonCash[EnumSystemCommonCash.SceneId] = stageIndex.Value;
                                if (!SelectGameManager.Instance.SceneOwner.SetSystemCommonCash(sysCommonCash))
                                    Debug.LogError("シーンID更新処理呼び出しの失敗");
                                // UI操作を許可しない
                                foreach (var child in captionStageModels)
                                    if (child != null)
                                    {
                                        child.SetButtonEnabled(false);
                                        child.SetEventTriggerEnabled(false);
                                    }
                                if (!GameManager.Instance.SoftwareCursorPositionAdjusterView.SetCursorEnabled(false))
                                    Debug.LogError("カーソルのステータスを変更呼び出しの失敗");
                                if (!SelectGameManager.Instance.InputSystemsOwner.Exit())
                                    Debug.LogError("InputSystem終了呼び出しの失敗");
                                // シーン読み込み時のアニメーション
                                Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Close))
                                    .Subscribe(_ =>
                                    {
                                        // メインシーンを実装
                                        SelectGameManager.Instance.SceneOwner.LoadMainScene();
                                    })
                                    .AddTo(gameObject);

                                break;
                            default:
                                Debug.LogWarning("例外ケース");
                                break;
                        }
                    });
            }

            // 支点とコード選択の操作
            for (var i = 0; i < pivotAndCodeIShortUIModels.Length; i++)
            {
                var idx = i;
                if (!pivotAndCodeIShortUIViews[idx].SetRenderDisableBackgroundFrame())
                    Debug.LogError("選択中フレームを非表示呼び出しの失敗");
                pivotAndCodeIShortUIModels[idx].EventState.ObserveEveryValueChanged(x => x.Value)
                    .Subscribe(x =>
                    {
                        switch ((EnumEventCommand)x)
                        {
                            case EnumEventCommand.Default:
                                // 処理無し
                                break;
                            case EnumEventCommand.Selected:
                                // 選択SEを再生
                                if (!playerView.IsSkipMode)
                                {
                                    SelectGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_select);
                                    Observable.FromCoroutine<bool>(observer => pivotAndCodeIShortUIViews[idx].PlayRenderEnableBackgroundFrame(observer))
                                        .Subscribe(_ => { })
                                        .AddTo(gameObject);
                                }
                                else
                                {
                                    if (!playerView.SetSkipMode(false))
                                        Debug.LogError("スキップモードのセット呼び出しの失敗");
                                    if (!pivotAndCodeIShortUIViews[idx].SetRenderEnableBackgroundFrame())
                                        Debug.LogError("選択中フレームを表示呼び出しの失敗");
                                }

                                break;
                            case EnumEventCommand.DeSelected:
                                Observable.FromCoroutine<bool>(observer => pivotAndCodeIShortUIViews[idx].PlayRenderDisableBackgroundFrame(observer))
                                    .Subscribe(_ => { })
                                    .AddTo(gameObject);

                                break;
                            case EnumEventCommand.Canceled:

                                break;
                            case EnumEventCommand.Submited:
                                // 決定SEを再生
                                SelectGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_code_normal);
                                if (!pivotAndCodeIShortUIViews[idx].SetAsSemiLastSibling())
                                    Debug.LogError("SetSiblingIndexでparent配下の子オブジェクト数-1へ配置呼び出しの失敗");
                                if (!GameManager.Instance.SoftwareCursorPositionAdjusterView.SetCursorEnabled(false))
                                    Debug.LogError("カーソルのステータスを変更呼び出しの失敗");
                                if (!pivotAndCodeIShortUIViews[idx].SetRenderDisableBackgroundFrame())
                                    Debug.LogError("選択中フレームを非表示呼び出しの失敗");
                                Observable.FromCoroutine<bool>(observer => pivotAndCodeIShortUIViews[idx].PlaySpinAnimationAndUpdateTurnValue(observer))
                                    .Subscribe(_ =>
                                    {
                                        if (!playerView.SetSkipMode(true))
                                            Debug.LogError("透明度をセット呼び出しの失敗");
                                        pivotAndCodeIShortUIModels[idx].Selected();

                                        var result = SelectGameManager.Instance.AlgorithmOwner.CheckIT();
                                        if (0 < result.areaIDToUpdated)
                                        {
                                            if (result.areaIDToUpdated == (int)EnumUnitID.Head)
                                            {
                                                if (!result.isAssigned)
                                                {
                                                    // UI操作を許可しない
                                                    foreach (var child in pivotAndCodeIShortUIModels)
                                                        if (child != null)
                                                        {
                                                            child.SetButtonEnabled(false);
                                                            child.SetEventTriggerEnabled(false);
                                                        }
                                                    // シナリオのレシーバーへ送信
                                                    foreach (var receiver in receivers)
                                                    {
                                                        var n = flowchartModel.GetBlockName(0);
                                                        if (!string.IsNullOrEmpty(n))
                                                            receiver.OnSendFungusMessage(n);
                                                        else
                                                            Debug.LogWarning("取得ブロック名無し");
                                                    }
                                                }
                                                else
                                                {
                                                    // 各エリア情報の更新
                                                    areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == result.areaIDToUpdated)
                                                        .Select(q => q)
                                                        .ToArray()[0][EnumAreaOpenedAndITState.State] = $"{(int)EnumAreaOpenedAndITStateState.ITFixed}";
                                                    // ライトアーム／レフトアームを選択可能状態にする
                                                    foreach (var item in areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == (int)EnumUnitID.RightArm ||
                                                        int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == (int)EnumUnitID.LeftArm)
                                                        .Select(q => q))
                                                        item[EnumAreaOpenedAndITState.State] = $"{(int)EnumAreaOpenedAndITStateState.Select}";
                                                    if (!SelectGameManager.Instance.SceneOwner.SetAreaOpenedAndITState(areaOpenedAndITState))
                                                        Debug.LogError("エリア解放・結合テスト済みデータを更新呼び出しの失敗");
                                                    // T.B.D IT演出（フェードアウトしてエリアセレクトシーンへ遷移する？）
                                                    if (!GameManager.Instance.SoftwareCursorPositionAdjusterView.SetCursorEnabled(true))
                                                        Debug.LogError("カーソルのステータスを変更呼び出しの失敗");
                                                }
                                            }
                                            else
                                            {
                                                // 各エリア情報の更新
                                                areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == result.areaIDToUpdated)
                                                    .Select(q => q)
                                                    .ToArray()[0][EnumAreaOpenedAndITState.State] = $"{(int)EnumAreaOpenedAndITStateState.ITFixed}";
                                                if (!SelectGameManager.Instance.SceneOwner.SetAreaOpenedAndITState(areaOpenedAndITState))
                                                    Debug.LogError("エリア解放・結合テスト済みデータを更新呼び出しの失敗");
                                                // T.B.D IT演出（フェードアウトしてエリアセレクトシーンへ遷移する？）
                                                if (!GameManager.Instance.SoftwareCursorPositionAdjusterView.SetCursorEnabled(true))
                                                    Debug.LogError("カーソルのステータスを変更呼び出しの失敗");
                                            }
                                        }
                                        else if (-1 < result.areaIDToUpdated)
                                        {
                                            // 更新済み
                                            if (!GameManager.Instance.SoftwareCursorPositionAdjusterView.SetCursorEnabled(true))
                                                Debug.LogError("カーソルのステータスを変更呼び出しの失敗");
                                        }
                                        else
                                            Debug.LogError("IT実施呼び出しの失敗");
                                    })
                                    .AddTo(gameObject);

                                break;
                            default:
                                Debug.LogWarning("例外ケース");
                                break;
                        }
                    });
            }
            // ロボットのコア
            foreach (var item in robotHeartModels.Where(q => q != null)
                .Select((p, i) => new { Content = p, Index = i }))
            {
                item.Content.EventState.ObserveEveryValueChanged(x => x.Value)
                    .Subscribe(x =>
                    {
                        switch ((EnumEventCommand)x)
                        {
                            case EnumEventCommand.Default:
                                break;
                            case EnumEventCommand.Selected:
                                // 選択SEを再生
                                if (!playerView.IsSkipMode)
                                    SelectGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_select);
                                // セレクトシーンへの再読み込み
                                if (!common.SetSystemCommonCashAndDefaultStageIndex(item.Content.EnumUnitID))
                                    Debug.LogError("キャッシュをセット呼び出しの失敗");
                                // UI操作を許可しない
                                foreach (var child in robotHeartModels)
                                    if (child != null)
                                    {
                                        child.SetButtonEnabled(false);
                                        child.SetEventTriggerEnabled(false);
                                    }
                                if (!GameManager.Instance.SoftwareCursorPositionAdjusterView.SetCursorEnabled(false))
                                    Debug.LogError("カーソルのステータスを変更呼び出しの失敗");
                                if (!SelectGameManager.Instance.InputSystemsOwner.Exit())
                                    Debug.LogError("InputSystem終了呼び出しの失敗");
                                // シーン読み込み時のアニメーション
                                Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Close))
                                    .Subscribe(_ =>
                                    {
                                        // メインシーンを実装
                                        SelectGameManager.Instance.SceneOwner.ReLoadScene();
                                    })
                                    .AddTo(gameObject);

                                break;
                            case EnumEventCommand.DeSelected:
                                break;
                            case EnumEventCommand.Submited:
                                break;
                            case EnumEventCommand.Canceled:
                                break;
                            case EnumEventCommand.AnyKeysPushed:
                                break;
                            default:
                                break;
                        }
                    });
            }
            // シナリオ管理
            flowchartModel.ReadedScenarioNo.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    if (0 < x)
                    {
                        switch (x)
                        {
                            case 0:
                                // 処理無し
                                break;
                            case 1:
                                // UI操作を許可する
                                foreach (var child in pivotAndCodeIShortUIModels)
                                    if (child != null)
                                    {
                                        child.SetButtonEnabled(true);
                                        child.SetEventTriggerEnabled(true);
                                    }
                                if (!GameManager.Instance.SoftwareCursorPositionAdjusterView.SetCursorEnabled(true))
                                    Debug.LogError("カーソルのステータスを変更呼び出しの失敗");
                                break;
                            default:
                                break;
                        }
                    }
                });
            // キャンバスサイズ補正
            canvasModel.ScaleFactor.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    if (!GameManager.Instance.SoftwareCursorPositionAdjusterView.ChangeCursorScaleProcessor(x))
                        Debug.LogError("VirtualMouseInputのカーソルのスケールを変更するProcessorを適用呼び出しの失敗");
                });
            // プレイヤー向き制御
            playerModel.BodyImage.Direction.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    switch ((EnumDirectionMode2D)x)
                    {
                        case EnumDirectionMode2D.Back:
                            if (!playerView.SetLocalScaleX((float)EnumDirectionMode2D.Back))
                                Debug.LogError("ローカルスケールをセット呼び出しの失敗");
                            break;
                        case EnumDirectionMode2D.Default:
                            if (!playerView.SetLocalScaleX((float)EnumDirectionMode2D.Forward))
                                Debug.LogError("ローカルスケールをセット呼び出しの失敗");
                            break;
                        case EnumDirectionMode2D.Forward:
                            if (!playerView.SetLocalScaleX((float)EnumDirectionMode2D.Forward))
                                Debug.LogError("ローカルスケールをセット呼び出しの失敗");
                            break;
                        default:
                            Debug.LogError("例外エラー");
                            break;
                    }
                });
            var isOnCanceled = new BoolReactiveProperty();
            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    if (isEnabledCancel.Value &&
                        !isOnCanceled.Value &&
                        SelectGameManager.Instance.InputSystemsOwner.InputUI.Canceled)
                    {
                        isOnCanceled.Value = true;
                        // キャンセルSEを再生
                        SelectGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_cancel);
                        // タイトルシーンへの遷移
                        // UI操作を許可しない
                        foreach (var child in logoStageModels)
                            if (child != null)
                            {
                                child.SetButtonEnabled(false);
                                child.SetEventTriggerEnabled(false);
                            }
                        // UI操作を許可しない
                        foreach (var child in pivotAndCodeIShortUIModels)
                            if (child != null)
                            {
                                child.SetButtonEnabled(false);
                                child.SetEventTriggerEnabled(false);
                            }
                        // UI操作を許可しない
                        foreach (var child in robotHeartModels)
                            if (child != null)
                            {
                                child.SetButtonEnabled(false);
                                child.SetEventTriggerEnabled(false);
                            }
                        if (!GameManager.Instance.SoftwareCursorPositionAdjusterView.SetCursorEnabled(false))
                            Debug.LogError("カーソルのステータスを変更呼び出しの失敗");
                        if (!SelectGameManager.Instance.InputSystemsOwner.Exit())
                            Debug.LogError("InputSystem終了呼び出しの失敗");
                        // シーン読み込み時のアニメーション
                        Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Close))
                            .Subscribe(_ =>
                            {
                                SelectGameManager.Instance.SceneOwner.LoadTitleScene();
                            })
                            .AddTo(gameObject);
                    }
                });
        }
    }
}
