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
        /// <summary>1ページあたりのコンテンツ数</summary>
        [SerializeField] private int contentsCountInPage = 5;
        /// <summary>ステージ選択のフレームのビュー</summary>
        [SerializeField] private SelectStageFrameView selectStageFrameView;
        /// <summary>プレイヤーのフレームのビュー</summary>
        [SerializeField] private PlayerView playerView;
        /// <summary>ロゴステージの統括パネルのビュー</summary>
        [SerializeField] private LogoStagesView logoStagesView;

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
                }
            }
            logoStageViews = logoStageViewList.ToArray();
            logoStageModels = logoStageModelList.ToArray();
            pivotAndCodeIShortUIViews = pivotAndCodeIShortUIViewList.ToArray();
            pivotAndCodeIShortUIModels = pivotAndCodeIShortUIModelList.ToArray();

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

            selectStageFrameView = GameObject.Find("SelectStageFrame").GetComponent<SelectStageFrameView>();
            playerView = GameObject.Find("Player").GetComponent<PlayerView>();
            logoStagesView = GameObject.Find("LogoStages").GetComponent<LogoStagesView>();
        }

        public void OnStart()
        {
            // 初期設定
            foreach (var child in logoStageModels)
                if (child != null)
                {
                    child.SetButtonEnabled(false);
                    child.SetEventTriggerEnabled(false);
                    if (!child.LoadStateAndUpdateNavigation())
                        Debug.LogError("ステージ状態のロード及びナビゲーション更新呼び出しの失敗");
                }
            foreach (var child in pageViews)
                if (child != null)
                    child.SetVisible(false);
            foreach (var item in captionStageModels.Where(q => q != null).Select(q => q))
                item.gameObject.SetActive(false);

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
                })
                .AddTo(gameObject);

            // ステージ番号を取得する処理を追加する
            var sysCommonCash = SelectGameManager.Instance.SceneOwner.GetSystemCommonCash();
            var stageIndex = new IntReactiveProperty(sysCommonCash[EnumSystemCommonCash.SceneId]);
            logoStageModels[stageIndex.Value].SetSelectedGameObject();
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
                    var pageIdx = ((x - 1) / contentsCountInPage) + 1;
                    for (var i = 0; i < pageViews.Length; i++)
                    {
                        if (i == 0)
                            // 0ページは存在しないためスキップ
                            continue;
                        // 該当ページのみ表示させる
                        if (!pageViews[i].SetVisible(i == pageIdx))
                            Debug.LogError("アルファ値切り替え処理の失敗");
                    }
                    if (!selectStageFrameView.MoveSelectStageFrame(logoStageViews[x].transform.position, (logoStageViews[x].transform as RectTransform).sizeDelta))
                        Debug.LogError("ステージ選択のフレームを移動して選択させる呼び出しの失敗");
                    if (!playerView.MoveSelectPlayer(logoStageViews[x].transform.position, logoStageViews[x].transform))
                        Debug.LogError("ステージ選択のプレイヤーを移動して選択させる呼び出しの失敗");
                });

            playerView.IsPlaying.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    if (x && playerView.HookContent != null)
                    {
                        var l = playerView.HookContent.GetComponent<LogoStageModel>();
                        if (l != null)
                        {
                            if (!l.SetButtonEnabled(false))
                                Debug.LogError("ボタンのステータスを変更呼び出しの失敗");
                            if (!l.SetEventTriggerEnabled(false))
                                Debug.LogError("イベントトリガーのステータスを変更呼び出しの失敗");

                        }
                        var p = playerView.HookContent.GetComponent<PivotAndCodeIShortUIModel>();
                        if (p != null)
                        {
                            if (!p.SetButtonEnabled(false))
                                Debug.LogError("ボタンのステータスを変更呼び出しの失敗");
                            if (!p.SetEventTriggerEnabled(false))
                                Debug.LogError("イベントトリガーのステータスを変更呼び出しの失敗");
                        }
                    }
                    else if (!x && playerView.HookContent != null)
                    {
                        var l = playerView.HookContent.GetComponent<LogoStageModel>();
                        if (l != null)
                        {
                            if (!l.SetButtonEnabled(true))
                                Debug.LogError("ボタンのステータスを変更呼び出しの失敗");
                            if (!l.SetEventTriggerEnabled(true))
                                Debug.LogError("イベントトリガーのステータスを変更呼び出しの失敗");

                        }
                        var p = playerView.HookContent.GetComponent<PivotAndCodeIShortUIModel>();
                        if (p != null)
                        {
                            if (!p.SetButtonEnabled(true))
                                Debug.LogError("ボタンのステータスを変更呼び出しの失敗");
                            if (!p.SetEventTriggerEnabled(true))
                                Debug.LogError("イベントトリガーのステータスを変更呼び出しの失敗");
                        }
                    }
                    else
                    {
                        // 例外ケース
                    }
                });

            // ステージ選択の操作
            foreach (var child in logoStageModels.Where(q => q != null).Select(q => q))
            {
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
                                SelectGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_select);
                                stageIndex.Value = child.Index;
                                break;
                            case EnumEventCommand.DeSelected:
                                // 処理無し
                                break;
                            case EnumEventCommand.Canceled:
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
                                // シーン読み込み時のアニメーション
                                Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Close))
                                    .Subscribe(_ =>
                                    {
                                        SelectGameManager.Instance.SceneOwner.LoadTitleScene();
                                    })
                                    .AddTo(gameObject);

                                break;
                            case EnumEventCommand.Submited:
                                // 決定SEを再生
                                SelectGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_decided);
                                if (!logoStageModels[child.Index].SetButtonEnabled(false))
                                    Debug.LogError("ボタンのステータスを変更呼び出しの失敗");
                                if (!logoStageModels[child.Index].SetEventTriggerEnabled(false))
                                    Debug.LogError("イベントトリガーのステータスを変更呼び出しの失敗");
                                if (!logoStagesView.ZoomInOutPanel(child.Index))
                                    Debug.LogError("該当ステージを拡大させる呼び出しの失敗");

                                break;
                            default:
                                Debug.LogWarning("例外ケース");
                                break;
                        }
                    });
            }

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
                        // デフォルト選択
                        captionStageModels[stageIndex.Value].SetSelectedGameObject();
                        if (!selectStageFrameView.SetColorAlpha(0f))
                            Debug.LogError("透明度をセット呼び出しの失敗");
                        if (!playerView.SetColorAlpha(0f))
                            Debug.LogError("透明度をセット呼び出しの失敗");
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
                                    // デフォルト選択
                                    logoStageModels[stageIndex.Value].SetSelectedGameObject();
                                    if (!selectStageFrameView.SetColorAlpha(255f))
                                        Debug.LogError("透明度をセット呼び出しの失敗");
                                    if (!playerView.SetColorAlpha(255f))
                                        Debug.LogError("透明度をセット呼び出しの失敗");
                                })
                                .AddTo(gameObject);
                        }
                    }
                });

            // キャプションの状態管理
            foreach (var item in captionStageModels.Where(q => q != null).Select(q => q))
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
                                SelectGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_select);
                                if (!selectStageFrameView.MoveSelectStageFrame(pivotAndCodeIShortUIViews[idx].transform.position, (pivotAndCodeIShortUIViews[idx].transform as RectTransform).sizeDelta))
                                    Debug.LogError("ステージ選択のフレームを移動して選択させる呼び出しの失敗");
                                if (!playerView.MoveSelectPlayer(pivotAndCodeIShortUIViews[idx].transform.position, pivotAndCodeIShortUIViews[idx].transform))
                                    Debug.LogError("ステージ選択のプレイヤーを移動して選択させる呼び出しの失敗");
                                break;
                            case EnumEventCommand.DeSelected:
                                // 処理無し
                                break;
                            case EnumEventCommand.Canceled:
                                // キャンセルSEを再生
                                SelectGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_cancel);
                                // タイトルシーンへの遷移
                                // UI操作を許可しない
                                foreach (var child in pivotAndCodeIShortUIModels)
                                    if (child != null)
                                    {
                                        child.SetButtonEnabled(false);
                                        child.SetEventTriggerEnabled(false);
                                    }
                                // シーン読み込み時のアニメーション
                                Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Close))
                                    .Subscribe(_ =>
                                    {
                                        SelectGameManager.Instance.SceneOwner.LoadTitleScene();
                                    })
                                    .AddTo(gameObject);

                                break;
                            case EnumEventCommand.Submited:
                                // 決定SEを再生
                                SelectGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_decided);

                                break;
                            default:
                                Debug.LogWarning("例外ケース");
                                break;
                        }
                    });
            }
        }
    }
}