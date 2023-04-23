using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Main.Model;
using DG.Tweening;

namespace Main.Common
{
    /// <summary>
    /// アルゴリズムのオーナー
    /// </summary>
    public class AlgorithmOwner : MonoBehaviour, IMainGameManager, IAlgorithmOwner
    {
        /// <summary>接触対象のオブジェクトタグ</summary>
        [SerializeField] private string[] tags = { ConstTagNames.TAG_NAME_STARTNODE, ConstTagNames.TAG_NAME_GOALNODE, ConstTagNames.TAG_NAME_MOLECULES, };

        /// <summary>
        /// モジュール構造体
        /// </summary>
        private struct Module
        {
            /// <summary>ノードコードを二次元配列で管理</summary>
            public Transform[][] moduleTracer;
            /// <summary>バグフィックス</summary>
            public bool isBugFixed;
        }

        /// <summary>モジュール構造体</summary>
        private Module[] _modules;
        /// <summary>信号が送信された履歴</summary>
        public Transform[] HistorySignalsPosted => _history.historySignalsPosted;
        /// <summary>
        /// 送信履歴構造体
        /// </summary>
        private struct History
        {
            /// <summary>信号が送信された履歴</summary>
            public Transform[] historySignalsPosted;
            /// <summary>バグフィックス</summary>
            public bool isBugFixed;
        }
        /// <summary>送信履歴構造体</summary>
        private History _history = new History();
        /// <summary>信号が受信された履歴</summary>
        private Transform[][] _historySignalsGeted;
        /// <summary>信号が受信された履歴</summary>
        public Transform[][] HistorySignalsGeted => _historySignalsGeted;
        /// <summary>アルゴリズムの共通処理</summary>
        private AlgorithmCommon _algorithmCommon = new AlgorithmCommon();
        /// <summary>Historyに入っているルートに電流を走らせる処理の実行中</summary>
        private bool _isPlaingRunLightningSignal;
        /// <summary>電流を走らせる処理の終了時間</summary>
        [SerializeField] private float pathDuration = .15f;

        public void OnStart()
        {
            // Level内でオブジェクトを探索
            var level = MainGameManager.Instance.LevelOwner;
            // T.B.D コードダイブ先を含め全てのモジュール数の構造体リストで初期化
            _modules = new Module[1];
            for (var i = 0; i < _modules.Length; i++)
            {
                var module = new Dictionary<Transform, Vector2>();
                foreach (Transform child in level.InstancedLevel.transform)
                    if (-1 < System.Array.IndexOf(tags, child.tag))
                    {
                        module[child] = child.localPosition;
                        //Debug.Log($"トレース対象オブジェクト:[{child.gameObject}]_座標:[{module[child]}]");
                    }
            }
        }

        public bool SetBugFixed(bool isBugFixed)
        {
            try
            {
                _history.isBugFixed = isBugFixed;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool AddHistorySignalsPosted(Transform nodeCode)
        {
            return AddHistorySignalsPosted(nodeCode, false);
        }

        public bool AddHistorySignalsPosted(Transform nodeCode, bool isReset)
        {
            try
            {
                if (isReset)
                {
                    _history.historySignalsPosted = null;
                }
                var historySignalsPostedList = _history.historySignalsPosted == null || (_history.historySignalsPosted != null && _history.historySignalsPosted.Length == 0) ? new List<Transform>() : _history.historySignalsPosted.ToList();
                historySignalsPostedList.Add(nodeCode);
                _history.historySignalsPosted = historySignalsPostedList.ToArray();

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetHistorySignalsPosted(Transform[] nodeCodes)
        {
            try
            {
                _history.historySignalsPosted = nodeCodes;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetHistorySignalsPosted(List<Transform> nodeCodes)
        {
            throw new System.NotImplementedException();
        }

        public Transform[] GetSignalDestinations(EnumDirectionMode[] enumDirectionModes, Transform transform, EnumCodeState codeState, float direction, LayerMask layerMask)
        {
            // T.B.D コードダイブ先を含め全てのモジュール数の構造体リストで初期化
            return GetSignalDestinations(enumDirectionModes, transform, codeState, direction, layerMask, 0);
        }

        public Transform[] GetSignalDestinations(EnumDirectionMode[] enumDirectionModes, Transform transform, EnumCodeState codeState, float direction, LayerMask layerMask, int moduleIdx)
        {
            var destinationList = new List<Transform>();
            if (transform.CompareTag(ConstTagNames.TAG_NAME_STARTNODE) ||
                transform.CompareTag(ConstTagNames.TAG_NAME_MOLECULES) ||
                transform.CompareTag(ConstTagNames.TAG_NAME_GOALNODE))
            {
                foreach (var mode in enumDirectionModes)
                {
                    var rayDirection = new Vector2();
                    switch (mode)
                    {
                        case EnumDirectionMode.Up:
                            rayDirection = Vector2.up;
                            break;
                        case EnumDirectionMode.Right:
                            rayDirection = Vector2.right;
                            break;
                        case EnumDirectionMode.Down:
                            rayDirection = Vector2.down;
                            break;
                        case EnumDirectionMode.Left:
                            rayDirection = Vector2.left;
                            break;
                        default:
                            throw new System.Exception("例外エラー");
                    }

                    var hits = Physics2D.RaycastAll(transform.position, rayDirection, direction, layerMask);
                    // ヒットしたオブジェクトの内、コライダーを持つかつ、オブジェクト自身は対象に含まないかつ、スタートノードは対象に含まない
                    foreach (var hit in hits.Where(q => q.collider != null &&
                        !q.transform.Equals(transform) &&
                        q.transform.GetComponent<StartNodeModel>() == null)
                        .Select(q => q))
                    {
                        var pivotModel = hit.transform.GetComponent<PivotModel>();
                        var goalNodeModel = hit.transform.GetComponent<GoalNodeModel>();
                        if (pivotModel == null && goalNodeModel == null)
                            throw new System.Exception($"PivotModelまたはGoalNodeModelのコンポーネント無し:[{pivotModel}][{goalNodeModel}]");
                        if (pivotModel != null &&
                            (
                            pivotModel.IsGetting.Value))
                            // 送信済みは対象に含まない（逆流＆ループ対策）
                            continue;
                        destinationList.Add(pivotModel != null ? pivotModel.transform : goalNodeModel.transform);
                    }
                }
            }
            Debug.Log($"[{transform.name}]接続先_[{string.Join(",", destinationList.Select(q => q.name))}]");

            return destinationList.ToArray();
        }

        public Transform[] GetSignalDestinationsReverse(EnumDirectionMode[] enumDirectionModes, Transform transform, EnumCodeState codeState, float direction, LayerMask layerMask)
        {
            // T.B.D コードダイブ先を含め全てのモジュール数の構造体リストで初期化
            return GetSignalDestinationsReverse(enumDirectionModes, transform, codeState, direction, layerMask, 0);
        }

        public Transform[] GetSignalDestinationsReverse(EnumDirectionMode[] enumDirectionModes, Transform transform, EnumCodeState codeState, float direction, LayerMask layerMask, int moduleIdx)
        {
            var destinationList = new List<Transform>();
            if (transform.CompareTag(ConstTagNames.TAG_NAME_MOLECULES) ||
                transform.CompareTag(ConstTagNames.TAG_NAME_ATOMS))
            {
                foreach (var mode in enumDirectionModes)
                {
                    var rayDirection = new Vector2();
                    switch (mode)
                    {
                        case EnumDirectionMode.Up:
                            rayDirection = Vector2.up;
                            break;
                        case EnumDirectionMode.Right:
                            rayDirection = Vector2.right;
                            break;
                        case EnumDirectionMode.Down:
                            rayDirection = Vector2.down;
                            break;
                        case EnumDirectionMode.Left:
                            rayDirection = Vector2.left;
                            break;
                        default:
                            throw new System.Exception("例外エラー");
                    }

                    var hits = Physics2D.RaycastAll(transform.position, rayDirection, direction, layerMask);
                    // ヒットしたオブジェクトの内、コライダーを持つかつ、オブジェクト自身は対象に含まない
                    foreach (var hit in hits.Where(q => q.collider != null && !q.transform.Equals(transform)).Select(q => q))
                    {
                        var pivotModel = hit.transform.GetComponent<PivotModel>();
                        var startNodeModel = hit.transform.GetComponent<StartNodeModel>();
                        if (pivotModel == null && startNodeModel == null)
                            throw new System.Exception($"PivotModelまたはStartNodeModelのコンポーネント無し:[{pivotModel}][{startNodeModel}]");

                        if (pivotModel != null &&
                            ((EnumDirectionMode)pivotModel.EnumDirectionModeReact.Value).Equals(_algorithmCommon.GetAjustedEnumDirectionMode(mode, 2)))
                        {
                            destinationList.Add(pivotModel.transform);
                        }
                        else if (startNodeModel != null &&
                            ((EnumDirectionMode)startNodeModel.EnumDirectionModeReact.Value).Equals(_algorithmCommon.GetAjustedEnumDirectionMode(mode, 2)))
                        {
                            destinationList.Add(startNodeModel.transform);
                        }
                    }
                }
            }
            Debug.Log($"[{transform.name}]接続先_[{string.Join(",", destinationList.Select(q => q.name))}]");

            return destinationList.ToArray();
        }

        public bool AddHistorySignalsGeted(Transform nodeCode)
        {
            // 帰納法のリストは複数に分かれる（リストのリストになる想定）
            return AddHistorySignalsGeted(nodeCode, false);
        }

        public bool AddHistorySignalsGeted(Transform nodeCode, bool isReset)
        {
            // 帰納法のリストは複数に分かれる（リストのリストになる想定）
            try
            {
                if (isReset)
                    _historySignalsGeted = null;
                var historySignalsListParent = new List<Transform[]>();
                if (_historySignalsGeted == null || (_historySignalsGeted != null && _historySignalsGeted.Length == 0))
                {
                    var historySignalsList = new List<Transform>();
                    historySignalsList.Add(nodeCode);
                    historySignalsListParent.Add(historySignalsList.ToArray());
                }
                else
                {
                    foreach (var item in _historySignalsGeted)
                    {
                        var historySignalsList = item.ToList();
                        historySignalsList.Add(nodeCode);
                        // 分岐数に合わせてクローンを生成
                        for (var i = 0; i < 3; i++)
                        {
                            historySignalsListParent.Add(historySignalsList.ToArray());
                        }
                    }
                }
                _historySignalsGeted = historySignalsListParent.ToArray();
                var sb = new System.Text.StringBuilder();
                foreach (var item in _historySignalsGeted)
                {
                    sb.Append(string.Join(",", item.Select(q => q.name).ToArray()))
                        .Append("\r\n");
                }
                Debug.Log(sb.ToString());

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetHistorySignalsGeted(Transform[] nodeCodes)
        {
            throw new System.NotImplementedException();
        }

        public bool SetHistorySignalsGeted(List<Transform> nodeCodes)
        {
            throw new System.NotImplementedException();
        }

        public bool GetGetProcessState(GameObject[] codeObjs)
        {
            // T.B.D 一つのコードのFromリストが0でも、他のコードのFromリストが0でない場合もある
            // モジュール内のFromリストを参照してnullでないかつ、0でないならtrue（実行中）
            // 最後の要素も0ならfalse（停止）
            // 引数は不要？

            if (_historySignalsGeted == null)
                throw new System.Exception("信号が受信された履歴データ無し");
            foreach (var items in _historySignalsGeted)
            {
                if (0 < items.Length)
                {
                    if (items[items.Length - 1].GetComponent<GoalNodeModel>() != null)
                        // ゴールノードのみでは判断不可のためスキップ
                        continue;

                    // コードがGet済みかつ、Fromリストが存在するなら実行中
                    if (items[items.Length - 1].GetComponent<PivotModel>() != null &&
                        items[items.Length - 1].GetComponent<PivotModel>().IsGetting.Value &&
                        0 < items[items.Length - 1].GetComponent<PivotModel>().FromListLength.Value)
                        return true;

                    // スタートノードがGet済みなら実行中
                    if (items[items.Length - 1].GetComponent<StartNodeModel>() != null &&
                        items[items.Length - 1].GetComponent<StartNodeModel>().IsGetting.Value)
                        return true;
                }
            }

            return false;
        }

        public Transform GetHistorySignalsGetedLasted(Transform nodeCode)
        {
            if (_historySignalsGeted == null)
                throw new System.Exception("信号が受信された履歴データ無し");
            foreach (var items in _historySignalsGeted)
            {
                if (0 < items.Length)
                {
                    return items[items.Length - 1];
                }
            }
            throw new System.Exception("信号が受信された履歴データ無し");
        }

        public bool MergeHistorySignalsGetedToPosted()
        {
            if (_history.historySignalsPosted == null)
                throw new System.Exception("信号を送信した履歴データ無し");
            if (_historySignalsGeted == null)
                throw new System.Exception("信号が受信された履歴データ無し");
            // DistinctしてStartNodeが入っている配列のみ取得
            var historySignalsGeted = _historySignalsGeted.Distinct();
            List<Transform> historySignalsGetedChild = new List<Transform>();
            foreach (var item in historySignalsGeted)
            {
                historySignalsGetedChild.AddRange(item.AsEnumerable()
                .Reverse());
                // StartNodeが格納されているルートは一つのみの想定
                break;
            }
            if (historySignalsGetedChild.Count < 1)
                throw new System.Exception("信号受信リスト整形の失敗");

            var historySignalsPosted = new List<Transform>();
            if (historySignalsGetedChild[0].Equals(_history.historySignalsPosted[_history.historySignalsPosted.Length - 1]))
            {
                // POST要素の最後以外を格納
                historySignalsPosted.AddRange(_history.historySignalsPosted.Select((p, i) => new { Content = p, Index = i })
                    .Where(q => q.Index < _history.historySignalsPosted.Length - 1)
                    .Select(q => q.Content)
                    .ToArray());
                // GET要素を格納
                historySignalsPosted.AddRange(historySignalsGetedChild);
                _history.historySignalsPosted = historySignalsPosted.ToArray();

                return true;
            }
            else
                // マージなし
                return false;
        }

        public IEnumerator PlayRunLightningSignal(System.IObserver<bool> observer)
        {
            // 信号送信済みでないかつ、信号送信中でない
            if (!_isPlaingRunLightningSignal &&
                _history.historySignalsPosted != null)
            {
                _isPlaingRunLightningSignal = true;

                var root = _history.historySignalsPosted.Select(q => q.position).ToArray();
                if (!MainGameManager.Instance.ParticleSystemsOwner.PlayParticleSystems(GetInstanceID(), EnumParticleSystemsIndex.DustConnectSignal, root[0]))
                    Debug.LogError("指定されたパーティクルシステムを再生する呼び出しの失敗");
                var signal = MainGameManager.Instance.ParticleSystemsOwner.GetParticleSystemsTransform(GetInstanceID(), EnumParticleSystemsIndex.DustConnectSignal);
                var doPathRoot = root.Select((p, i) => new { Content = p, Index = i })
                    .Where(q => 0 < q.Index)
                    .Select(q => q.Content).ToArray();
                signal.DOPath(doPathRoot, pathDuration * doPathRoot.Length)
                    .OnComplete(() =>
                    {
                        if (!MainGameManager.Instance.ParticleSystemsOwner.StopParticleSystems(GetInstanceID(), EnumParticleSystemsIndex.DustConnectSignal))
                            Debug.LogError("指定されたパーティクルシステムを停止する呼び出しの失敗");
                        _isPlaingRunLightningSignal = false;
                        observer.OnNext(true);
                    });
            }
            else
            {
                // 実行中
            }

            yield return null;
        }

        public Transform GetHistorySignalsPostedLasted(Transform nodeCode)
        {
            if (_history.historySignalsPosted == null)
                throw new System.Exception("信号が送信された履歴データ無し");

            if (0 < _history.historySignalsPosted.Length)
            {
                return _history.historySignalsPosted[_history.historySignalsPosted.Length - 1];
            }
            else
            {
                throw new System.Exception("信号が送信された履歴データ無し");
            }
        }
    }

    /// <summary>
    /// アルゴリズムのオーナーインターフェース
    /// </summary>
    public interface IAlgorithmOwner
    {
        /// <summary>
        /// 信号の送信先リストを取得
        /// </summary>
        /// <param name="enumDirectionModes">方角モード</param>
        /// <param name="transform">トランスフォーム</param>
        /// <param name="codeState">コード状態管理</param>
        /// <param name="direction">レイの距離</param>
        /// <param name="layerMask">レイヤーマスク</param>
        /// <returns>送信先Transform配列</returns>
        public Transform[] GetSignalDestinations(EnumDirectionMode[] enumDirectionModes, Transform transform, EnumCodeState codeState, float direction, LayerMask layerMask);
        /// <summary>
        /// 信号の送信先リストを取得
        /// </summary>
        /// <param name="enumDirectionModes">方角モード</param>
        /// <param name="transform">トランスフォーム</param>
        /// <param name="codeState">コード状態管理</param>
        /// <param name="direction">レイの距離</param>
        /// <param name="layerMask">レイヤーマスク</param>
        /// <param name="moduleIdx">構造体インデックス</param>
        /// <returns>送信先Transform配列</returns>
        public Transform[] GetSignalDestinations(EnumDirectionMode[] enumDirectionModes, Transform transform, EnumCodeState codeState, float direction, LayerMask layerMask, int moduleIdx);
        /// <summary>
        /// 信号の送信元リストを取得
        /// </summary>
        /// <param name="enumDirectionModes">方角モード</param>
        /// <param name="transform">トランスフォーム</param>
        /// <param name="codeState">コード状態管理</param>
        /// <param name="direction">レイの距離</param>
        /// <param name="layerMask">レイヤーマスク</param>
        /// <returns>送信先Transform配列</returns>
        public Transform[] GetSignalDestinationsReverse(EnumDirectionMode[] enumDirectionModes, Transform transform, EnumCodeState codeState, float direction, LayerMask layerMask);
        /// <summary>
        /// 信号の送信元リストを取得
        /// </summary>
        /// <param name="enumDirectionModes">方角モード</param>
        /// <param name="transform">トランスフォーム</param>
        /// <param name="codeState">コード状態管理</param>
        /// <param name="direction">レイの距離</param>
        /// <param name="layerMask">レイヤーマスク</param>
        /// <param name="moduleIdx">構造体インデックス</param>
        /// <returns>送信先Transform配列</returns>
        public Transform[] GetSignalDestinationsReverse(EnumDirectionMode[] enumDirectionModes, Transform transform, EnumCodeState codeState, float direction, LayerMask layerMask, int moduleIdx);
        /// <summary>
        /// バグフィックス状態をセット
        /// </summary>
        /// <param name="isBugFixed">バグフィックス状態</param>
        /// <returns>成功／失敗</returns>
        public bool SetBugFixed(bool isBugFixed);
        /// <summary>
        /// 信号が送信された履歴へ追加
        /// </summary>
        /// <param name="nodeCode">ノードコード</param>
        /// <param name="isReset">リセットフラグ</param>
        /// <returns>成功／失敗</returns>
        public bool AddHistorySignalsPosted(Transform nodeCode, bool isReset);
        /// <summary>
        /// 信号が送信された履歴へ追加
        /// </summary>
        /// <param name="nodeCode">ノードコード</param>
        /// <returns>成功／失敗</returns>
        public bool AddHistorySignalsPosted(Transform nodeCode);
        /// <summary>
        /// 信号が送信された履歴の配列をセット
        /// </summary>
        /// <param name="nodeCode">信号が送信された履歴の配列</param>
        /// <returns>成功／失敗</returns>
        public bool SetHistorySignalsPosted(Transform[] nodeCodes);
        /// <summary>
        /// 信号が送信された履歴の配列をセット
        /// </summary>
        /// <param name="nodeCode">信号が送信された履歴リスト</param>
        /// <returns>成功／失敗</returns>
        public bool SetHistorySignalsPosted(List<Transform> nodeCodes);
        /// <summary>
        /// 信号が受信された履歴へ追加
        /// </summary>
        /// <param name="nodeCode">ノードコード</param>
        /// <param name="isReset">リセットフラグ</param>
        /// <returns>成功／失敗</returns>
        public bool AddHistorySignalsGeted(Transform nodeCode, bool isReset);
        /// <summary>
        /// 信号が受信された履歴へ追加
        /// </summary>
        /// <param name="nodeCode">ノードコード</param>
        /// <returns>成功／失敗</returns>
        public bool AddHistorySignalsGeted(Transform nodeCode);
        /// <summary>
        /// 信号が受信された履歴の配列をセット
        /// </summary>
        /// <param name="nodeCode">信号が送信された履歴の配列</param>
        /// <returns>成功／失敗</returns>
        public bool SetHistorySignalsGeted(Transform[] nodeCodes);
        /// <summary>
        /// 信号が受信された履歴の配列をセット
        /// </summary>
        /// <param name="nodeCode">信号が送信された履歴リスト</param>
        /// <returns>成功／失敗</returns>
        public bool SetHistorySignalsGeted(List<Transform> nodeCodes);
        /// <summary>
        /// 信号受信プロセスの状態を取得
        /// </summary>
        /// <param name="codeObjs">コード系</param>
        /// <returns>実行中／停止</returns>
        public bool GetGetProcessState(GameObject[] codeObjs);
        /// <summary>
        /// 信号が送信された履歴の配列から対象の一つ前のノードコードを取得
        /// </summary>
        /// <param name="nodeCode">ノードコード</param>
        /// <returns>ノードコード（履歴から一つ前）</returns>
        public Transform GetHistorySignalsPostedLasted(Transform nodeCode);
        /// <summary>
        /// 信号が受信された履歴の配列から対象の一つ前のノードコードを取得
        /// </summary>
        /// <param name="nodeCode">ノードコード</param>
        /// <returns>ノードコード（履歴から一つ前）</returns>
        public Transform GetHistorySignalsGetedLasted(Transform nodeCode);
        /// <summary>
        /// 信号が受信された履歴の配列を
        /// 信号が送信された履歴の配列へマージ
        /// </summary>
        /// <returns>マージあり／マージなし</returns>
        public bool MergeHistorySignalsGetedToPosted();

        /// <summary>
        /// Historyに入っているルートに電流を走らせる
        /// </summary>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayRunLightningSignal(System.IObserver<bool> observer);
    }
}
