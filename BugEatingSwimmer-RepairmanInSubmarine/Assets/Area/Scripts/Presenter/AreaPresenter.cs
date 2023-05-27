using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Area.Common;
using Area.View;
using Area.Model;
using UniRx;
using UniRx.Triggers;
using Area.Audio;
using System.Linq;
using Fungus;

namespace Area.Presenter
{
    /// <summary>
    /// プレゼンタ
    /// エリアシーン
    /// </summary>
    public class AreaPresenter : MonoBehaviour, IAreaGameManager
    {
        ///// <summary>ページのビュー</summary>
        //[SerializeField] private PageView[] pageViews;
        ///// <summary>ページのモデル</summary>
        //[SerializeField] private PageModel[] pageModels;
        ///// <summary>ロゴステージのビュー</summary>
        //[SerializeField] private LogoStageView[] logoStageViews;
        ///// <summary>ロゴステージのモデル</summary>
        //[SerializeField] private LogoStageModel[] logoStageModels;
        ///// <summary>支点とコードのビュー</summary>
        //[SerializeField] private PivotAndCodeIShortUIView[] pivotAndCodeIShortUIViews;
        ///// <summary>支点とコードのモデル</summary>
        //[SerializeField] private PivotAndCodeIShortUIModel[] pivotAndCodeIShortUIModels;
        ///// <summary>ステージキャプションのビュー</summary>
        //[SerializeField] private CaptionStageView[] captionStageViews;
        ///// <summary>ステージキャプションのモデル</summary>
        //[SerializeField] private CaptionStageModel[] captionStageModels;
        /// <summary>FadeImageのビュー</summary>
        [SerializeField] private FadeImageView fadeImageView;
        /// <summary>Fadeimageのモデル</summary>
        [SerializeField] private FadeImageModel fadeImageModel;
        ///// <summary>1ページあたりのコンテンツ数</summary>
        //[SerializeField] private int contentsCountInPage = 5;
        /// <summary>プレイヤーのフレームのビュー</summary>
        [SerializeField] private PlayerView playerView;
        ///// <summary>ロゴステージの統括パネルのビュー</summary>
        //[SerializeField] private LogoStagesView logoStagesView;
        /// <summary>準委任帳票のビュー</summary>
        [SerializeField] private AssignedSeastarCountView assignedSeastarCountView;
        ///// <summary>ヒトデゲージのビュー</summary>
        //[SerializeField] private SeastarGageView[] seastarGageViews;
        /// <summary>全てのユニットの制御のビュー</summary>
        [SerializeField] private RobotPanelView robotPanelView;
        /// <summary>全てのユニットの制御のモデル</summary>
        [SerializeField] private RobotPanelModel robotPanelModel;
        /// <summary>Fungusのレシーバー</summary>
        [SerializeField] private MessageReceived[] receivers;
        /// <summary>Fungusのフローチャートモデル</summary>
        [SerializeField] private FlowchartModel flowchartModel;
        /// <summary>テンプレートパネルのビュー</summary>
        [SerializeField] private TempletePanelView templetePanelView;
        /// <summary>シーンカットのビュー</summary>
        [SerializeField] private CutSceneView cutSceneView;
        /// <summary>エンディングのビュー</summary>
        [SerializeField] private EndingView endingView;

        private void Reset()
        {
            //    var logoStages = GameObject.Find("LogoStages").transform;

            //    // ページのビューとモデルをセット
            //    List<PageView> pageViewList = new List<PageView>();
            //    List<PageModel> pageModelList = new List<PageModel>();
            //    foreach (Transform child in logoStages)
            //    {
            //        pageViewList.Add(child.GetComponent<PageView>());
            //        pageModelList.Add(child.GetComponent<PageModel>());
            //    }
            //    pageViews = pageViewList.ToArray();
            //    pageModels = pageModelList.ToArray();

            //    // ロゴステージのビューとモデルをセット
            //    List<LogoStageView> logoStageViewList = new List<LogoStageView>();
            //    List<LogoStageModel> logoStageModelList = new List<LogoStageModel>();
            //    // 支点とコードのビューとモデルをセット
            //    List<PivotAndCodeIShortUIView> pivotAndCodeIShortUIViewList = new List<PivotAndCodeIShortUIView>();
            //    List<PivotAndCodeIShortUIModel> pivotAndCodeIShortUIModelList = new List<PivotAndCodeIShortUIModel>();
            //    foreach (Transform pages in logoStages)
            //    {
            //        foreach (Transform child in pages)
            //        {
            //            if (-1 < child.name.IndexOf("LogoStage"))
            //            {
            //                logoStageViewList.Add(child.GetComponent<LogoStageView>());
            //                logoStageModelList.Add(child.GetComponent<LogoStageModel>());
            //            }
            //            else if (-1 < child.name.IndexOf("PivotAndCodeIShortUI"))
            //            {
            //                pivotAndCodeIShortUIViewList.Add(child.GetComponent<PivotAndCodeIShortUIView>());
            //                pivotAndCodeIShortUIModelList.Add(child.GetComponent<PivotAndCodeIShortUIModel>());
            //            }
            //        }
            //    }
            //    logoStageViews = logoStageViewList.ToArray();
            //    logoStageModels = logoStageModelList.ToArray();
            //    pivotAndCodeIShortUIViews = pivotAndCodeIShortUIViewList.ToArray();
            //    pivotAndCodeIShortUIModels = pivotAndCodeIShortUIModelList.ToArray();

            //    // ステージキャプションのビューとモデルをセット
            //    List<CaptionStageView> captionStageViewList = new List<CaptionStageView>();
            //    List<CaptionStageModel> captionStageModelList = new List<CaptionStageModel>();
            //    var caption = GameObject.Find("Caption").transform;
            //    foreach (Transform child in caption)
            //    {
            //        captionStageViewList.Add(child.GetComponent<CaptionStageView>());
            //        captionStageModelList.Add(child.GetComponent<CaptionStageModel>());
            //    }
            //    captionStageViews = captionStageViewList.ToArray();
            //    captionStageModels = captionStageModelList.ToArray();

            // フェードのビューとモデルをセット
            fadeImageView = GameObject.Find("FadeImage").GetComponent<FadeImageView>();
            fadeImageModel = GameObject.Find("FadeImage").GetComponent<FadeImageModel>();

            //    playerView = GameObject.Find("Player").GetComponent<PlayerView>();
            playerView = GameObject.Find("Player").GetComponent<PlayerView>();
            //    logoStagesView = GameObject.Find("LogoStages").GetComponent<LogoStagesView>();
            //    List<SeastarGageView> seastarGageViewList = new List<SeastarGageView>();
            //    foreach (Transform item in logoStagesView.transform)
            //    {
            //        seastarGageViewList.Add(item.GetComponent<PageView>() != null ? item.GetComponentInChildren<SeastarGageView>() : null);
            //    }
            //    seastarGageViews = seastarGageViewList.ToArray();
            assignedSeastarCountView = GameObject.Find("AssignedSeastarCount").GetComponent<AssignedSeastarCountView>();
            robotPanelView = GameObject.Find("RobotPanel").GetComponent<RobotPanelView>();
            robotPanelModel = GameObject.Find("RobotPanel").GetComponent<RobotPanelModel>();
            receivers = GameObject.FindObjectsOfType<Fungus.MessageReceived>();
            flowchartModel = GameObject.Find("Flowchart").GetComponent<FlowchartModel>();
            templetePanelView = GameObject.Find("TempletePanel").GetComponent<TempletePanelView>();
            cutSceneView = GameObject.Find("CutScene").GetComponent<CutSceneView>();
            endingView = GameObject.Find("Ending").GetComponent<EndingView>();
        }

        public void OnStart()
        {
            var common = new AreaPresenterCommon();
            // 初期設定
            foreach (var child in robotPanelModel.RobotUnitImageModels)
                if (child != null)
                {
                    child.SetButtonEnabled(false);
                    child.SetEventTriggerEnabled(false);
                    if (!child.LoadStateAndUpdateNavigation())
                        Debug.LogError("ステージ状態のロード及びナビゲーション更新呼び出しの失敗");
                }
            foreach (var unitID in robotPanelModel.RobotUnitImageModels.Select(q => q.RobotUnitImageConfig.EnumUnitID))
                if (!robotPanelView.RendererDisableMode(unitID))
                    Debug.LogError("対象ユニットを非選択状態にする呼び出しの失敗");
            if (!playerView.RedererCursorDirectionAndDistance(new UnityEngine.UI.Navigation(), EnumCursorDistance.Long))
                Debug.LogError("ナビゲーションの状態によってカーソル表示を変更呼び出しの失敗");
            //    foreach (var child in pageViews)
            //        if (child != null)
            //            child.SetVisible(false);
            //    // シーンIDに紐づくキャプションかつ、そのキャプション内にもつSeastarIDをもつオブジェクトを抽出
            //    if (!common.SetIsAssignedAllCaption(captionStageModels))
            //        Debug.LogError("全ステージキャプションのアサイン情報をセット呼び出しの失敗");
            //    foreach (var item in captionStageModels.Where(q => q != null).Select(q => q))
            //        item.gameObject.SetActive(false);
            if (!assignedSeastarCountView.SetCounterText(AreaGameManager.Instance.GimmickOwner.GetAssinedCounter()))
                Debug.LogError("ヒトデ総配属人数をセット呼び出しの失敗");
            if (!common.CheckMissionAndSaveDatasCSVOfMission())
                Debug.LogError("ミッションの更新チェック呼び出しの失敗");
            cutSceneView.gameObject.SetActive(false);
            endingView.gameObject.SetActive(false);

            AreaGameManager.Instance.AudioOwner.PlayBGM(ClipToPlayBGM.bgm_select);
            var enumRobotpanel = common.GetStateOfRobotUnitConnect();
            if (common.IsConnectedAnimation())
            {
                // 実績一覧管理の履歴を取得
                var missions = common.GetMissions();
                if (0 < missions.Length)
                {
                    // 最後に解除したミッション情報を取得する
                    if (!robotPanelView.SetPositionAndEulerAngleOfAllUnit(missions[missions.Length - 1].enumRobotPanel))
                        Debug.LogError("各ユニットに対してアニメーション再生呼び出しの失敗");
                    foreach (var unitID in missions.Select(q => q.enumUnitID)
                        .Distinct())
                        if (!robotPanelView.RendererEnableMode(unitID))
                            Debug.LogError("対象ユニットを非選択状態にする呼び出しの失敗");
                }
            }
            else
            {
                if (!robotPanelView.SetPositionAndEulerAngleOfAllUnit(enumRobotpanel))
                    Debug.LogError("各ユニットに対してアニメーション再生呼び出しの失敗");
            }
            // シナリオ管理
            flowchartModel.ReadedScenarioNo.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    switch (x)
                    {
                        case 0:
                            // 初期値のため処理無し
                            break;
                        case 1:
                            // 実績履歴を更新
                            if (common.AddMissionHistory() < 1)
                                Debug.LogError("実績履歴を更新呼び出しの失敗");
                            Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Close))
                                .Subscribe(_ =>
                                {
                                    // イベント完了後の処理
                                    AreaGameManager.Instance.SceneOwner.ReLoadScene();
                                })
                                .AddTo(gameObject);
                            break;
                        case 2:
                            // ボディなどの次エリア選択ができるようになればOK。
                            Observable.FromCoroutine<bool>(observer => robotPanelView.PlayRenderEnable(common.GetPlayRenderEnables()[0], observer))
                                .Subscribe(_ => { })
                                .AddTo(gameObject);

                            break;
                        case 3:
                            // 回想終了
                            Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Close))
                                .Subscribe(_ =>
                                {
                                    AreaGameManager.Instance.AudioOwner.PlayBGM(ClipToPlayBGM.bgm_select);
                                    cutSceneView.gameObject.SetActive(false);
                                    Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Open))
                                        .Subscribe(_ => { })
                                        .AddTo(gameObject);
                                })
                                .AddTo(gameObject);

                            break;
                        case 4:
                            // ロボ、信号を飛ばす。右腕、左腕の電源が入る
                            // 実績一覧管理を参照して引数を切り替える
                            // ※ヘッドITの場合のみライトアームとレフトアームの解放演出が入るためここのみ配列を渡す
                            Observable.FromCoroutine<bool>(observer => robotPanelView.PlayRenderEnable(common.GetPlayRenderEnables(), observer))
                                .Subscribe(_ => { })
                                .AddTo(gameObject);

                            break;
                        case 5:
                            // 画面転換 一枚絵 夜、湖の畔の工事現場が燃えている 鳴り響くサイレンと爆発音
                            Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Close))
                                .Subscribe(_ =>
                                {
                                    if (!cutSceneView.SetSprite(EnumRecollectionPicture.RecollectionEndingPictureA))
                                        Debug.LogError("スプライトをセット呼び出しの失敗");
                                    cutSceneView.gameObject.SetActive(true);
                                    Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Open))
                                        .Subscribe(_ => { })
                                        .AddTo(gameObject);
                                })
                                .AddTo(gameObject);

                            break;
                        case 6:
                            // 画面転換 フェードアウト（真っ暗） 泡の音
                            AreaGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_swim);
                            Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Close))
                                .Subscribe(_ =>
                                {
                                    AreaGameManager.Instance.AudioOwner.StopBGM();
                                    // 静かになり暫く無音
                                    Observable.FromCoroutine<bool>(observer => flowchartModel.WaitForSeconds(observer))
                                        .Subscribe(_ =>
                                        {
                                            // 唐突に電源が入る 一枚絵（タイトルの拡大で流用した、海の画像）
                                            if (!cutSceneView.SetSprite(EnumRecollectionPicture.Mainbg))
                                                Debug.LogError("スプライトをセット呼び出しの失敗");
                                            if (!fadeImageView.SetAlpha(EnumFadeState.Close))
                                                Debug.LogError("アルファ値をセット呼び出しの失敗");
                                        })
                                        .AddTo(gameObject);
                                })
                                .AddTo(gameObject);

                            break;
                        case 7:
                            // 画面転換 一枚絵 どこかの家のリビング
                            Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Close))
                                .Subscribe(_ =>
                                {
                                    if (!cutSceneView.SetSprite(EnumRecollectionPicture.RecollectionEndingPictureB))
                                        Debug.LogError("スプライトをセット呼び出しの失敗");
                                    cutSceneView.gameObject.SetActive(true);
                                    Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Open))
                                        .Subscribe(_ => { })
                                        .AddTo(gameObject);
                                })
                                .AddTo(gameObject);

                            break;
                        case 8:
                            // タイトルがバーンと出てエンディング
                            AreaGameManager.Instance.AudioOwner.PlayBGM(ClipToPlayBGM.bgm_ending);
                            endingView.gameObject.SetActive(true);
                            Observable.FromCoroutine<int>(observer => endingView.PlayEndingCut(observer))
                                .Subscribe(x =>
                                {
                                    if (0 < x)
                                    {
                                        Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Close))
                                            .Subscribe(_ =>
                                            {
                                                endingView.gameObject.SetActive(false);
                                            })
                                            .AddTo(gameObject);
                                    }
                                })
                                .AddTo(gameObject);

                            break;
                        case 9:
                            // 回想シーン１シーン切り替え②
                            if (!cutSceneView.SetSprite(EnumRecollectionPicture.RecollectionPicture1_2))
                                Debug.LogError("スプライトをセット呼び出しの失敗");

                            break;
                        case 10:
                            // 回想シーン１シーン切り替え③
                            if (!cutSceneView.SetSprite(EnumRecollectionPicture.RecollectionPicture1_3))
                                Debug.LogError("スプライトをセット呼び出しの失敗");

                            break;
                        case 11:
                            // 回想シーン２シーン切り替え②
                            if (!cutSceneView.SetSprite(EnumRecollectionPicture.RecollectionPicture2_2))
                                Debug.LogError("スプライトをセット呼び出しの失敗");

                            break;
                        case 12:
                            // 回想シーン３シーン切り替え②
                            if (!cutSceneView.SetSprite(EnumRecollectionPicture.RecollectionPicture3_2))
                                Debug.LogError("スプライトをセット呼び出しの失敗");

                            break;
                        case 13:
                            // 回想シーン３シーン切り替え③
                            if (!cutSceneView.SetSprite(EnumRecollectionPicture.RecollectionPicture3_3))
                                Debug.LogError("スプライトをセット呼び出しの失敗");

                            break;
                        case 14:
                            // 回想シーン３シーン切り替え④
                            if (!cutSceneView.SetSprite(EnumRecollectionPicture.RecollectionPicture3_4))
                                Debug.LogError("スプライトをセット呼び出しの失敗");

                            break;
                        case 15:
                            // 回想シーン４シーン切り替え②
                            if (!cutSceneView.SetSprite(EnumRecollectionPicture.RecollectionPicture4_2))
                                Debug.LogError("スプライトをセット呼び出しの失敗");

                            break;
                        case 16:
                            // 回想シーン４シーン切り替え③～最後
                            Observable.FromCoroutine<int>(observer => cutSceneView.PlayBetweenFrameAdvanceAtFadeMode(observer, EnumRecollectionPicture.RecollectionPicture4_3, EnumRecollectionPicture.RecollectionPicture4_13))
                                .Subscribe(x =>
                                {
                                    if (x < ((int)EnumRecollectionPicture.RecollectionPicture4_13 - (int)EnumRecollectionPicture.RecollectionPicture4_3))
                                        Debug.LogError("コマ送りでカットを切り替える呼び出しの失敗");
                                })
                                .AddTo(gameObject);

                            break;
                        case 17:
                            // オマケ
                            if (!cutSceneView.SetSprite(EnumRecollectionPicture.SquareBlack))
                                Debug.LogError("スプライトをセット呼び出しの失敗");

                            break;
                        default:
                            break;
                    }
                });
            var sysCommonCash = AreaGameManager.Instance.SceneOwner.GetSystemCommonCash();
            var stageIndex = sysCommonCash[EnumSystemCommonCash.SceneId];
            var areaUnits = common.LoadSaveDatasCSVAndGetAreaUnits();
            var currentUnitID = new IntReactiveProperty(common.GetUnitIDsButIgnoreVoidInCore(areaUnits, stageIndex));
            // シーン読み込み時のアニメーション
            if (!templetePanelView.SetPositionAndScaleAfterPrevSaveTransform((EnumUnitID)currentUnitID.Value))
                Debug.LogError("位置とスケールをセットして一つ前の状態を保存する呼び出しの失敗");
            Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Open))
                .Subscribe(_ =>
                {
                    Observable.FromCoroutine<bool>(observer => templetePanelView.PlayAnimationZoomOutUnit((EnumUnitID)currentUnitID.Value, observer))
                        .Subscribe(_ =>
                        {
                            //    // UI操作を許可
                            //    foreach (var child in logoStageModels)
                            //    if (child != null)
                            //    {
                            //        child.SetButtonEnabled(true);
                            //        child.SetEventTriggerEnabled(true);
                            //    }
                            //if (!common.SetCounterBetweenAndFillAmountAllGage(seastarGageViews))
                            //    Debug.LogError("全ヒトデゲージのカウンターとフィルターをセット呼び出しの失敗");

                            // エリア解放・結合テストのセーブファイル、イベント管理を参照して引数を切り替える
                            if (common.IsConnectedAnimation())
                            {
                                Observable.FromCoroutine<bool>(observer => robotPanelView.PlayRepairEffect(enumRobotpanel, observer))
                                    .Subscribe(_ =>
                                    {
                                        Observable.FromCoroutine<bool>(observer => robotPanelView.PlayAnimationOfAllUnit(enumRobotpanel, observer))
                                            .Subscribe(x =>
                                            {
                                                if (x)
                                                {
                                                    if (enumRobotpanel.Equals(EnumRobotPanel.ConnectedHead))
                                                    {
                                                        Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Close))
                                                            .Subscribe(_ =>
                                                            {
                                                                if (!cutSceneView.SetSprite(EnumRecollectionPicture.RecollectionPicture1_1))
                                                                    Debug.LogError("スプライトをセット呼び出しの失敗");
                                                                AreaGameManager.Instance.AudioOwner.PlayBGM(ClipToPlayBGM.bgm_kaiso);
                                                                cutSceneView.gameObject.SetActive(true);
                                                                Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Open))
                                                                    .Subscribe(_ =>
                                                                    {
                                                                        // シナリオのレシーバーへ送信
                                                                        foreach (var receiver in receivers)
                                                                        {
                                                                            var n = flowchartModel.GetBlockName();
                                                                            if (!string.IsNullOrEmpty(n))
                                                                                receiver.OnSendFungusMessage(n);
                                                                            else
                                                                                Debug.LogWarning("取得ブロック名無し");
                                                                        }
                                                                    })
                                                                    .AddTo(gameObject);
                                                            })
                                                            .AddTo(gameObject);
                                                    }
                                                    else if (enumRobotpanel.Equals(EnumRobotPanel.ConnectedFailureHead))
                                                    {
                                                        // 実績一覧管理を参照して引数を切り替える
                                                        Observable.FromCoroutine<bool>(observer => robotPanelView.PlayRenderEnable(common.GetPlayRenderEnables()[0], observer))
                                                            .Subscribe(_ =>
                                                            {
                                                                // シナリオのレシーバーへ送信
                                                                foreach (var receiver in receivers)
                                                                {
                                                                    var n = flowchartModel.GetBlockName();
                                                                    if (!string.IsNullOrEmpty(n))
                                                                        receiver.OnSendFungusMessage(n);
                                                                    else
                                                                    {
                                                                        Debug.LogWarning("取得ブロック名無し");
                                                                        // 実績履歴を更新
                                                                        if (common.AddMissionHistory() < 1)
                                                                            Debug.LogError("実績履歴を更新呼び出しの失敗");
                                                                        Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Close))
                                                                            .Subscribe(_ =>
                                                                            {
                                                                                // イベント完了後の処理
                                                                                AreaGameManager.Instance.SceneOwner.ReLoadScene();
                                                                            })
                                                                            .AddTo(gameObject);
                                                                        // ブロック名を取得できない場合はブレイクする
                                                                        break;
                                                                    }
                                                                }
                                                            })
                                                            .AddTo(gameObject);
                                                    }
                                                    else if (enumRobotpanel.Equals(EnumRobotPanel.Full) &&
                                                        ((!common.CheckUnlockMissionAndFindHistroy(EnumMissionID.MI0008) &&
                                                        common.CheckUnlockMissionAndUndefinedHistroy(EnumMissionID.MI0008)) ||
                                                        (!common.CheckUnlockMissionAndFindHistroy(EnumMissionID.MI0009) &&
                                                        common.CheckUnlockMissionAndUndefinedHistroy(EnumMissionID.MI0009))))
                                                    {
                                                        // シナリオのレシーバーへ送信
                                                        foreach (var receiver in receivers)
                                                        {
                                                            var n = flowchartModel.GetBlockName();
                                                            if (!string.IsNullOrEmpty(n))
                                                                receiver.OnSendFungusMessage(n);
                                                            else
                                                                Debug.LogWarning("取得ブロック名無し");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Close))
                                                            .Subscribe(_ =>
                                                            {
                                                                // 両腕が繋がった場合はミッションオーナーから次に呼び出すカットを判断できないため
                                                                // 個別でロジックを用意する
                                                                if (enumRobotpanel.Equals(EnumRobotPanel.ConnectedDoublearm))
                                                                {
                                                                    if (common.CheckUnlockMissionAndFindHistroy(EnumMissionID.MI0004))
                                                                    {
                                                                        // 「既に」ライトアームが繋がり、実績アンロック状態かつ、履歴にも存在する
                                                                        // レフトアーム接続時の回想シーンカット演出
                                                                        if (!cutSceneView.SetSprite(EnumRecollectionPicture.RecollectionPicture3_1))
                                                                            Debug.LogError("スプライトをセット呼び出しの失敗");
                                                                    }
                                                                    else if (common.CheckUnlockMissionAndFindHistroy(EnumMissionID.MI0005))
                                                                    {
                                                                        // 「既に」レフトアームが繋がり、実績アンロック状態かつ、履歴にも存在する
                                                                        // ライトアーム接続時の回想シーンカット演出
                                                                        if (!cutSceneView.SetSprite(EnumRecollectionPicture.RecollectionPicture2_1))
                                                                            Debug.LogError("スプライトをセット呼び出しの失敗");
                                                                    }
                                                                    else
                                                                    {
                                                                        // 処理無し
                                                                    }
                                                                }
                                                                else if (enumRobotpanel.Equals(EnumRobotPanel.Full))
                                                                {
                                                                    // ５－３クリアの場合はミッションオーナーから次に呼び出すカットを判断できないため
                                                                    // 個別でロジックを用意する
                                                                    if (!common.CheckUnlockMissionAndFindHistroy(EnumMissionID.MI0007) &&
                                                                        common.CheckUnlockMissionAndUndefinedHistroy(EnumMissionID.MI0007))
                                                                    {
                                                                        // ５－３クリア後の回想シーンカット演出
                                                                        if (!cutSceneView.SetSprite(EnumRecollectionPicture.RecollectionPicture4_1))
                                                                            Debug.LogError("スプライトをセット呼び出しの失敗");
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (!cutSceneView.SetSprite(AreaGameManager.Instance.MissionOwner.Missions.Where(q => q.enumRobotPanel.Equals(enumRobotpanel))
                                                                        .Select(q => q.enumRecollectionPicture)
                                                                        .Distinct()
                                                                        .ToArray()[0]))
                                                                        Debug.LogError("スプライトをセット呼び出しの失敗");
                                                                }
                                                                AreaGameManager.Instance.AudioOwner.PlayBGM(ClipToPlayBGM.bgm_kaiso);
                                                                cutSceneView.gameObject.SetActive(true);
                                                                Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Open))
                                                                    .Subscribe(_ =>
                                                                    {
                                                                        // シナリオのレシーバーへ送信
                                                                        foreach (var receiver in receivers)
                                                                        {
                                                                            var n = flowchartModel.GetBlockName();
                                                                            if (!string.IsNullOrEmpty(n))
                                                                                receiver.OnSendFungusMessage(n);
                                                                            else
                                                                            {
                                                                                Debug.LogWarning("取得ブロック名無し");
                                                                                // 実績履歴を更新
                                                                                if (common.AddMissionHistory() < 1)
                                                                                    Debug.LogError("実績履歴を更新呼び出しの失敗");
                                                                                Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Close))
                                                                                    .Subscribe(_ =>
                                                                                    {
                                                                                        // イベント完了後の処理
                                                                                        AreaGameManager.Instance.SceneOwner.ReLoadScene();
                                                                                    })
                                                                                    .AddTo(gameObject);
                                                                                // ブロック名を取得できない場合はブレイクする
                                                                                break;
                                                                            }
                                                                        }
                                                                    });
                                                            })
                                                            .AddTo(gameObject);
                                                    }
                                                }
                                                else
                                                {
                                                    // シナリオのレシーバーへ送信
                                                    foreach (var receiver in receivers)
                                                    {
                                                        var n = flowchartModel.GetBlockName();
                                                        if (!string.IsNullOrEmpty(n))
                                                            receiver.OnSendFungusMessage(n);
                                                        else
                                                            Debug.LogWarning("取得ブロック名無し");
                                                    }
                                                }
                                            })
                                            .AddTo(gameObject);
                                    })
                                    .AddTo(gameObject);
                            }
                        });
                })
                .AddTo(gameObject);

            if (common.IsConnectedAnimation())
                // 演出がある場合は一度シーンをリロードする想定のため、後続処理を実行しない
                return;

            // ステージ番号をエリア番号へ変換する
            robotPanelModel.RobotUnitImageModels.Where(q => (int)q.RobotUnitImageConfig.EnumUnitID == currentUnitID.Value)
                .Select(q => q)
                .ToArray()[0].SetSelectedGameObject();
            if (!playerView.SelectPlayer(robotPanelModel.RobotUnitImageModels.Where(q => (int)q.RobotUnitImageConfig.EnumUnitID == currentUnitID.Value)
                .Select(q => q)
                .ToArray()[0].transform.position, robotPanelModel.RobotUnitImageModels.Where(q => (int)q.RobotUnitImageConfig.EnumUnitID == currentUnitID.Value)
                .Select(q => q)
                .ToArray()[0].transform))
                Debug.LogError("ステージ選択のプレイヤーを移動して選択させる呼び出しの失敗");
            if (!playerView.RedererCursorDirectionAndDistance(robotPanelModel.RobotUnitImageModels.Where(q => (int)q.RobotUnitImageConfig.EnumUnitID == currentUnitID.Value)
                .Select(q => q)
                .ToArray()[0].Button.navigation, EnumCursorDistance.Long))
                Debug.LogError("ナビゲーションの状態によってカーソル表示を変更呼び出しの失敗");

            // エリア選択状態の表示
            foreach (var item in robotPanelModel.RobotUnitImageModels.Select((p, i) => new { Content = p, Index = i }))
            {
                item.Content.StageState.ObserveEveryValueChanged(x => x.Value)
                    .Subscribe(x =>
                    {
                        if ((int)EnumAreaOpenedAndITStateState.Select <= x)
                        {
                            var unitID = robotPanelModel.RobotUnitImageModels[item.Index].RobotUnitImageConfig.EnumUnitID;
                            if (!robotPanelView.RendererEnableMode(unitID))
                                Debug.LogError("対象ユニットを非選択状態にする呼び出しの失敗");
                        }
                    });
            }
            //    // 選択ステージ番号の更新
            //    stageIndex.ObserveEveryValueChanged(x => x.Value)
            //        .Subscribe(x =>
            //        {
            //            // ページ表示切り替え
            //            var pageIdx = ((x - 1) / contentsCountInPage) + 1;
            //            for (var i = 0; i < pageViews.Length; i++)
            //            {
            //                if (i == 0)
            //                    // 0ページは存在しないためスキップ
            //                    continue;
            //                // 該当ページのみ表示させる
            //                if (!pageViews[i].SetVisible(i == pageIdx))
            //                    Debug.LogError("アルファ値切り替え処理の失敗");
            //            }
            //        });

            playerView.IsPlaying.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    if (x)
                    {
                        foreach (var item in robotPanelModel.RobotUnitImageModels.Where(q => q != null))
                        {
                            if (!item.SetButtonEnabled(false))
                                Debug.LogError("ボタンのステータスを変更呼び出しの失敗");
                            if (!item.SetEventTriggerEnabled(false))
                                Debug.LogError("イベントトリガーのステータスを変更呼び出しの失敗");
                        }
                    }
                    else
                    {
                        foreach (var item in robotPanelModel.RobotUnitImageModels.Where(q => q != null))
                        {
                            if (!item.SetButtonEnabled(true))
                                Debug.LogError("ボタンのステータスを変更呼び出しの失敗");
                            if (!item.SetEventTriggerEnabled(true))
                                Debug.LogError("イベントトリガーのステータスを変更呼び出しの失敗");
                        }
                    }
                });

            // エリア選択の操作
            foreach (var item in robotPanelModel.RobotUnitImageModels)
            {
                item.EventState.ObserveEveryValueChanged(x => x.Value)
                    .Subscribe(x =>
                    {
                        switch ((EnumEventCommand)x)
                        {
                            case EnumEventCommand.Default:
                                // 処理無し
                                break;
                            case EnumEventCommand.Selected:
                                // 選択SEを再生
                                AreaGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_select);
                                // ステージ選択のプレイヤーを移動して選択させる
                                if (!playerView.SetImageEnabled(false))
                                    Debug.LogError("イメージのステータスを変更呼び出しの失敗");
                                Observable.FromCoroutine<bool>(observer => playerView.MoveSelectPlayer(item.transform.position, item.transform, observer))
                                    .Subscribe(_ =>
                                    {
                                        if (!playerView.SetImageEnabled(true))
                                            Debug.LogError("イメージのステータスを変更呼び出しの失敗");
                                        if (!playerView.RedererCursorDirectionAndDistance(item.Button.navigation, EnumCursorDistance.Long))
                                            Debug.LogError("ナビゲーションの状態によってカーソル表示を変更呼び出しの失敗");
                                        if (playerView.IsSkipMode)
                                            if (!playerView.SetSkipMode(false))
                                                Debug.LogError("スキップモードのセット呼び出しの失敗");
                                    })
                                    .AddTo(gameObject);
                                currentUnitID.Value = (int)item.RobotUnitImageConfig.EnumUnitID;
                                break;
                            case EnumEventCommand.DeSelected:
                                // 処理無し
                                break;
                            case EnumEventCommand.Canceled:
                                // キャンセルSEを再生
                                AreaGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_cancel);
                                // タイトルシーンへの遷移
                                // UI操作を許可しない
                                foreach (var child in robotPanelModel.RobotUnitImageModels)
                                    if (child != null)
                                    {
                                        child.SetButtonEnabled(false);
                                        child.SetEventTriggerEnabled(false);
                                    }
                                // シーン読み込み時のアニメーション
                                Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Close))
                                    .Subscribe(_ =>
                                    {
                                        AreaGameManager.Instance.SceneOwner.LoadBackScene();
                                    })
                                    .AddTo(gameObject);

                                break;
                            case EnumEventCommand.Submited:
                                // 決定SEを再生
                                AreaGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_decided);
                                foreach (var child in robotPanelModel.RobotUnitImageModels)
                                    if (child != null)
                                    {
                                        child.SetButtonEnabled(false);
                                        child.SetEventTriggerEnabled(false);
                                    }
                                if (!playerView.RedererCursorDirectionAndDistance(new UnityEngine.UI.Navigation(), EnumCursorDistance.Long))
                                    Debug.LogError("ナビゲーションの状態によってカーソル表示を変更呼び出しの失敗");
                                Observable.FromCoroutine<bool>(observer => templetePanelView.PlayAnimationZoomInUnit(item.RobotUnitImageConfig.EnumUnitID, observer))
                                    .Subscribe(_ =>
                                    {
                                        if (!common.SetSystemCommonCashAndDefaultStageIndex((EnumUnitID)currentUnitID.Value))
                                            Debug.LogError("キャッシュをセット呼び出しの失敗");
                                        // シーン読み込み時のアニメーション
                                        Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Close))
                                            .Subscribe(_ =>
                                            {
                                                AreaGameManager.Instance.SceneOwner.LoadNextScene();
                                            })
                                            .AddTo(gameObject);
                                    })
                                    .AddTo(gameObject);

                                break;
                            default:
                                Debug.LogWarning("例外ケース");
                                break;
                        }

                    });
            }

            //    // ステージ選択の操作
            //    foreach (var child in logoStageModels.Where(q => q != null).Select(q => q))
            //    {
            //        child.EventState.ObserveEveryValueChanged(x => x.Value)
            //            .Subscribe(x =>
            //            {
            //                switch ((EnumEventCommand)x)
            //                {
            //                    case EnumEventCommand.Default:
            //                        // 処理無し
            //                        break;
            //                    case EnumEventCommand.Selected:
            //                        // 選択SEを再生
            //                        if (!playerView.IsSkipMode)
            //                            SelectGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_select);
            //                        // ステージ選択のプレイヤーを移動して選択させる
            //                        if (!playerView.SetImageEnabled(false))
            //                            Debug.LogError("イメージのステータスを変更呼び出しの失敗");
            //                        Observable.FromCoroutine<bool>(observer => playerView.MoveSelectPlayer(logoStageViews[child.Index].transform.position, logoStageViews[child.Index].transform, observer))
            //                            .Subscribe(_ =>
            //                            {
            //                                if (!playerView.SetImageEnabled(true))
            //                                    Debug.LogError("イメージのステータスを変更呼び出しの失敗");
            //                                if (!playerView.RedererCursorDirectionAndDistance(child.Button.navigation, EnumCursorDistance.Long))
            //                                    Debug.LogError("ナビゲーションの状態によってカーソル表示を変更呼び出しの失敗");
            //                                if (playerView.IsSkipMode)
            //                                    if (!playerView.SetSkipMode(false))
            //                                        Debug.LogError("スキップモードのセット呼び出しの失敗");
            //                            })
            //                            .AddTo(gameObject);
            //                        stageIndex.Value = child.Index;
            //                        break;
            //                    case EnumEventCommand.DeSelected:
            //                        // 処理無し
            //                        break;
            //                    case EnumEventCommand.Canceled:
            //                        // キャンセルSEを再生
            //                        SelectGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_cancel);
            //                        // タイトルシーンへの遷移
            //                        // UI操作を許可しない
            //                        foreach (var child in logoStageModels)
            //                            if (child != null)
            //                            {
            //                                child.SetButtonEnabled(false);
            //                                child.SetEventTriggerEnabled(false);
            //                            }
            //                        // シーン読み込み時のアニメーション
            //                        Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Close))
            //                            .Subscribe(_ =>
            //                            {
            //                                SelectGameManager.Instance.SceneOwner.LoadTitleScene();
            //                            })
            //                            .AddTo(gameObject);

            //                        break;
            //                    case EnumEventCommand.Submited:
            //                        // 決定SEを再生
            //                        SelectGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_decided);
            //                        if (!logoStageModels[child.Index].SetButtonEnabled(false))
            //                            Debug.LogError("ボタンのステータスを変更呼び出しの失敗");
            //                        if (!logoStageModels[child.Index].SetEventTriggerEnabled(false))
            //                            Debug.LogError("イベントトリガーのステータスを変更呼び出しの失敗");
            //                        if (!logoStagesView.ZoomInOutPanel(child.Index))
            //                            Debug.LogError("該当ステージを拡大させる呼び出しの失敗");

            //                        break;
            //                    default:
            //                        Debug.LogWarning("例外ケース");
            //                        break;
            //                }
            //            });
            //    }

            //    // ステージが選択された状態なら、キャプションを表示にする
            //    logoStagesView.IsZoomed.ObserveEveryValueChanged(x => x.Value)
            //        .Subscribe(x =>
            //        {
            //            if (x)
            //            {
            //                if (!captionStageViews[stageIndex.Value].isActiveAndEnabled)
            //                    captionStageViews[stageIndex.Value].gameObject.SetActive(true);
            //                if (!captionStageViews[stageIndex.Value].ZoomInOutPanel(stageIndex.Value))
            //                    Debug.LogError("該当ステージを拡大させる呼び出しの失敗");
            //                if (!captionStageModels[stageIndex.Value].SetButtonEnabled(true))
            //                    Debug.LogError("ボタンのステータスを変更呼び出しの失敗");
            //                if (!captionStageModels[stageIndex.Value].SetEventTriggerEnabled(true))
            //                    Debug.LogError("イベントトリガーのステータスを変更呼び出しの失敗");
            //                // デフォルト選択
            //                captionStageModels[stageIndex.Value].SetSelectedGameObject();
            //                if (!playerView.SetColorAlpha(0f))
            //                    Debug.LogError("透明度をセット呼び出しの失敗");
            //                if (!playerView.SetImageEnabled(false))
            //                    Debug.LogError("透明度をセット呼び出しの失敗");
            //                if (!playerView.SetSkipMode(true))
            //                    Debug.LogError("透明度をセット呼び出しの失敗");
            //            }
            //            else
            //            {
            //                if (captionStageViews[stageIndex.Value].isActiveAndEnabled)
            //                {
            //                    // ズームアウトされた状態
            //                    Observable.FromCoroutine<bool>(observer => captionStageViews[stageIndex.Value].ZoomInOutPanel(stageIndex.Value, observer))
            //                        .Subscribe(_ =>
            //                        {
            //                            captionStageViews[stageIndex.Value].gameObject.SetActive(false);
            //                            if (!logoStageModels[stageIndex.Value].SetButtonEnabled(true))
            //                                Debug.LogError("ボタンのステータスを変更呼び出しの失敗");
            //                            if (!logoStageModels[stageIndex.Value].SetEventTriggerEnabled(true))
            //                                Debug.LogError("イベントトリガーのステータスを変更呼び出しの失敗");
            //                            // デフォルト選択
            //                            logoStageModels[stageIndex.Value].SetSelectedGameObject();
            //                            if (!playerView.SetColorAlpha(255f))
            //                                Debug.LogError("透明度をセット呼び出しの失敗");
            //                            if (!playerView.SetImageEnabled(true))
            //                                Debug.LogError("透明度をセット呼び出しの失敗");
            //                        })
            //                        .AddTo(gameObject);
            //                }
            //            }
            //        });

            //    // キャプションの状態管理
            //    foreach (var item in captionStageModels.Select((p, i) => new { Content = p, Index = i })
            //        .Where(q => 0 < q.Index))
            //    {
            //        foreach (var itemChild in item.Content.IsAssigned.Where(q => q != null)
            //            .Select((p, i) => new { Content = p, Index = i }))
            //        {
            //            itemChild.Content.ObserveEveryValueChanged(x => x.Value)
            //                .Subscribe(x =>
            //                {
            //                    if (x)
            //                    {
            //                        if (!captionStageViews[item.Index].SetColorAssigned(itemChild.Index))
            //                            Debug.LogError("アサイン済みのカラー設定呼び出しの失敗");
            //                    }
            //                    else
            //                    {
            //                        if (!captionStageViews[item.Index].SetColorUnAssign(itemChild.Index))
            //                            Debug.LogError("アサイン済みのカラー設定呼び出しの失敗");
            //                    }
            //                });
            //        }
            //        item.Content.EventState.ObserveEveryValueChanged(x => x.Value)
            //            .Subscribe(x =>
            //            {
            //                switch ((EnumEventCommand)x)
            //                {
            //                    case EnumEventCommand.Default:
            //                        // 処理無し
            //                        break;
            //                    case EnumEventCommand.Selected:
            //                        // 処理無し
            //                        break;
            //                    case EnumEventCommand.DeSelected:
            //                        // 処理無し
            //                        break;
            //                    case EnumEventCommand.Canceled:
            //                        // キャンセルSEを再生
            //                        SelectGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_cancel);
            //                        // UI操作を許可しない
            //                        foreach (var child in captionStageModels)
            //                            if (child != null)
            //                            {
            //                                child.SetButtonEnabled(false);
            //                                child.SetEventTriggerEnabled(false);
            //                            }
            //                        if (!logoStagesView.ZoomInOutPanel(stageIndex.Value, true))
            //                            Debug.LogError("該当ステージを拡大させる呼び出しの失敗");

            //                        break;
            //                    case EnumEventCommand.Submited:
            //                        // 決定SEを再生
            //                        SelectGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_decided);
            //                        // メインシーンへの遷移
            //                        sysCommonCash[EnumSystemCommonCash.SceneId] = stageIndex.Value;
            //                        if (!SelectGameManager.Instance.SceneOwner.SetSystemCommonCash(sysCommonCash))
            //                            Debug.LogError("シーンID更新処理呼び出しの失敗");
            //                        // UI操作を許可しない
            //                        foreach (var child in captionStageModels)
            //                            if (child != null)
            //                            {
            //                                child.SetButtonEnabled(false);
            //                                child.SetEventTriggerEnabled(false);
            //                            }
            //                        // シーン読み込み時のアニメーション
            //                        Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Close))
            //                            .Subscribe(_ =>
            //                            {
            //                                // メインシーンを実装
            //                                SelectGameManager.Instance.SceneOwner.LoadMainScene();
            //                            })
            //                            .AddTo(gameObject);

            //                        break;
            //                    default:
            //                        Debug.LogWarning("例外ケース");
            //                        break;
            //                }
            //            });
            //    }

            //    // 支点とコード選択の操作
            //    for (var i = 0; i < pivotAndCodeIShortUIModels.Length; i++)
            //    {
            //        var idx = i;
            //        pivotAndCodeIShortUIModels[idx].EventState.ObserveEveryValueChanged(x => x.Value)
            //            .Subscribe(x =>
            //            {
            //                switch ((EnumEventCommand)x)
            //                {
            //                    case EnumEventCommand.Default:
            //                        // 処理無し
            //                        break;
            //                    case EnumEventCommand.Selected:
            //                        // 選択SEを再生
            //                        if (!playerView.IsSkipMode)
            //                            SelectGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_select);
            //                        if (!playerView.SetImageEnabled(false))
            //                            Debug.LogError("イメージのステータスを変更呼び出しの失敗");
            //                        Observable.FromCoroutine<bool>(observer => playerView.MoveSelectPlayer(pivotAndCodeIShortUIViews[idx].transform.position, pivotAndCodeIShortUIViews[idx].transform, observer))
            //                            .Subscribe(_ =>
            //                            {
            //                                if (!playerView.SetImageEnabled(true))
            //                                    Debug.LogError("イメージのステータスを変更呼び出しの失敗");
            //                                if (!playerView.RedererCursorDirectionAndDistance(pivotAndCodeIShortUIModels[idx].Button.navigation, EnumCursorDistance.Short))
            //                                    Debug.LogError("ナビゲーションの状態によってカーソル表示を変更呼び出しの失敗");
            //                                if (playerView.IsSkipMode)
            //                                    if (!playerView.SetSkipMode(false))
            //                                        Debug.LogError("スキップモードのセット呼び出しの失敗");
            //                            })
            //                            .AddTo(gameObject);
            //                        break;
            //                    case EnumEventCommand.DeSelected:
            //                        // 処理無し
            //                        break;
            //                    case EnumEventCommand.Canceled:
            //                        // キャンセルSEを再生
            //                        SelectGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_cancel);
            //                        // タイトルシーンへの遷移
            //                        // UI操作を許可しない
            //                        foreach (var child in pivotAndCodeIShortUIModels)
            //                            if (child != null)
            //                            {
            //                                child.SetButtonEnabled(false);
            //                                child.SetEventTriggerEnabled(false);
            //                            }
            //                        // シーン読み込み時のアニメーション
            //                        Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Close))
            //                            .Subscribe(_ =>
            //                            {
            //                                SelectGameManager.Instance.SceneOwner.LoadTitleScene();
            //                            })
            //                            .AddTo(gameObject);

            //                        break;
            //                    case EnumEventCommand.Submited:
            //                        // 決定SEを再生
            //                        SelectGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_decided);
            //                        if (!pivotAndCodeIShortUIViews[idx].SetAsSemiLastSibling())
            //                            Debug.LogError("SetSiblingIndexでparent配下の子オブジェクト数-1へ配置呼び出しの失敗");
            //                        Observable.FromCoroutine<bool>(observer => pivotAndCodeIShortUIViews[idx].PlaySpinAnimationAndUpdateTurnValue(observer))
            //                            .Subscribe(_ =>
            //                            {
            //                                if (!playerView.SetSkipMode(true))
            //                                    Debug.LogError("透明度をセット呼び出しの失敗");
            //                                pivotAndCodeIShortUIModels[idx].Selected();

            //                                var result = SelectGameManager.Instance.AlgorithmOwner.CheckIT();
            //                                if (0 < result)
            //                                {
            //                                    // 各エリア情報の更新
            //                                    areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == result)
            //                                        .Select(q => q)
            //                                        .ToArray()[0][EnumAreaOpenedAndITState.State] = $"{(int)EnumAreaOpenedAndITStateState.ITFixed}";
            //                                    if (!SelectGameManager.Instance.SceneOwner.SetAreaOpenedAndITState(areaOpenedAndITState))
            //                                        Debug.LogError("エリア解放・結合テスト済みデータを更新呼び出しの失敗");
            //                                    // T.B.D IT演出（フェードアウトしてエリアエリアシーンへ遷移する？）
            //                                }
            //                                else if (-1 < result)
            //                                {
            //                                    // 更新済み
            //                                }
            //                                else
            //                                    Debug.LogError("IT実施呼び出しの失敗");
            //                            })
            //                            .AddTo(gameObject);

            //                        break;
            //                    default:
            //                        Debug.LogWarning("例外ケース");
            //                        break;
            //                }
            //            });
            //    }
        }
    }
}
