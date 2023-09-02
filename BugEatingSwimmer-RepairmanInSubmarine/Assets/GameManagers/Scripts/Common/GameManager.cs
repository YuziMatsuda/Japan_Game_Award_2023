using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Select.Common;
using UnityEngine.SceneManagement;

namespace GameManagers.Common
{
    /// <summary>
    /// ゲームマネージャー
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        /// <summary>セレクトのゲームマネージャー</summary>
        private static GameManager _instance;
        /// <summary>セレクトのゲームマネージャー</summary>
        public static GameManager Instance => _instance;
        /// <summary>ソフトウェアのカーソル制御ビュー</summary>
        [SerializeField] private SoftwareCursorPositionAdjusterView softwareCursorPositionAdjusterView;
        /// <summary>ソフトウェアのカーソル制御ビュー</summary>
        public SoftwareCursorPositionAdjusterView SoftwareCursorPositionAdjusterView => softwareCursorPositionAdjusterView;
        /// <summary>次に読み込むシーン名</summary>
        [SerializeField] private string nextSceneName = "TitleScene";

        private void Reset()
        {
            softwareCursorPositionAdjusterView = GetComponentInChildren<SoftwareCursorPositionAdjusterView>();
        }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            NextTitleScene();
        }

        /// <summary>
        /// 次のシーンをロード
        /// </summary>
        private void NextTitleScene()
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
    /// <summary>
    /// ゲームマネージャーとのインターフェース
    /// </summary>
    public interface IGameManager
    {
        /// <summary>
        /// オンスタート
        /// </summary>
        void OnStart();
    }
}
