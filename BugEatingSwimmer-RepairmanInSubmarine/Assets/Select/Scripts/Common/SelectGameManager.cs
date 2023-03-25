using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Select.Presenter;
using Select.Audio;

namespace Select.Common
{
    /// <summary>
    /// セレクトのゲームマネージャー
    /// </summary>
    public class SelectGameManager : MonoBehaviour
    {
        /// <summary>セレクトのゲームマネージャー</summary>
        private static SelectGameManager _instance;
        /// <summary>セレクトのゲームマネージャー</summary>
        public static SelectGameManager Instance => _instance;

        /// <summary>プレゼンタ</summary>
        [SerializeField] private SelectPresenter presenter;
        /// <summary>プレゼンタ</summary>
        public SelectPresenter Presenter => presenter;
        /// <summary>オーディオのオーナー</summary>
        [SerializeField] private AudioOwner audioOwner;
        /// <summary>オーディオのオーナー</summary>
        public AudioOwner AudioOwner => audioOwner;
        /// <summary>シーンオーナー</summary>
        [SerializeField] private SceneOwner sceneOwner;
        /// <summary>シーンオーナー</summary>
        public SceneOwner SceneOwner => sceneOwner;
        /// <summary>カーソル表示</summary>
        [SerializeField] private CursorVisible cursorVisible;
        /// <summary>カーソル表示</summary>
        public CursorVisible CursorVisible => cursorVisible;

        private void Reset()
        {
            presenter = GameObject.Find("Presenter").GetComponent<SelectPresenter>();
            audioOwner = GameObject.Find("AudioOwner").GetComponent<AudioOwner>();
            sceneOwner = GameObject.Find("SceneOwner").GetComponent<SceneOwner>();
            cursorVisible = GameObject.Find("CursorVisible").GetComponent<CursorVisible>();
        }

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
        }

        private void Start()
        {
            audioOwner.OnStart();
            presenter.OnStart();
            cursorVisible.OnStart();
            sceneOwner.OnStart();
        }
    }

    /// <summary>
    /// ゲームマネージャーとのインターフェース
    /// </summary>
    public interface ISelectGameManager
    {
        /// <summary>
        /// オンスタート
        /// </summary>
        void OnStart();
    }
}
