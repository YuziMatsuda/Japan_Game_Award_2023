using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Presenter;
using Main.Audio;
using Main.InputSystem;

namespace Main.Common
{
    /// <summary>
    /// メインのゲームマネージャー
    /// </summary>
    public class MainGameManager : MonoBehaviour
    {
        /// <summary>メインのゲームマネージャー</summary>
        private static MainGameManager _instance;
        /// <summary>メインのゲームマネージャー</summary>
        public static MainGameManager Instance => _instance;

        /// <summary>プレゼンタ</summary>
        [SerializeField] private MainPresenter presenter;
        /// <summary>プレゼンタ</summary>
        public MainPresenter Presenter => presenter;
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
        /// <summary>InputSystemのオーナー</summary>
        [SerializeField] private InputSystemsOwner inputSystemsOwner;
        /// <summary>InputSystemのオーナー</summary>
        public InputSystemsOwner InputSystemsOwner => inputSystemsOwner;
        /// <summary>SkyBoxのオーナー</summary>
        [SerializeField] private SkyBoxOwner skyBoxOwner;
        /// <summary>SkyBoxのオーナー</summary>
        public SkyBoxOwner SkyBoxOwner => skyBoxOwner;
        /// <summary>Levelのオーナー</summary>
        [SerializeField] private LevelOwner levelOwner;
        /// <summary>Levelのオーナー</summary>
        public LevelOwner LevelOwner => levelOwner;
        /// <summary>アルゴリズムのオーナー</summary>
        [SerializeField] private AlgorithmOwner algorithmOwner;
        /// <summary>アルゴリズムのオーナー</summary>
        public AlgorithmOwner AlgorithmOwner => algorithmOwner;
        /// <summary>パーティクルのオーナー</summary>
        [SerializeField] private ParticleSystemsOwner particleSystemsOwner;
        /// <summary>パーティクルのオーナー</summary>
        public ParticleSystemsOwner ParticleSystemsOwner => particleSystemsOwner;
        /// <summary>ギミックのオーナー</summary>
        [SerializeField] private GimmickOwner gimmickOwner;
        /// <summary>ギミックのオーナー</summary>
        public GimmickOwner GimmickOwner => gimmickOwner;

        private void Reset()
        {
            presenter = GameObject.Find("Presenter").GetComponent<MainPresenter>();
            audioOwner = GameObject.Find("AudioOwner").GetComponent<AudioOwner>();
            sceneOwner = GameObject.Find("SceneOwner").GetComponent<SceneOwner>();
            cursorVisible = GameObject.Find("CursorVisible").GetComponent<CursorVisible>();
            inputSystemsOwner = GameObject.Find("InputSystemsOwner").GetComponent<InputSystemsOwner>();
            skyBoxOwner = GameObject.Find("SkyBoxOwner").GetComponent<SkyBoxOwner>();
            levelOwner = GameObject.Find("LevelOwner").GetComponent<LevelOwner>();
            algorithmOwner = GameObject.Find("AlgorithmOwner").GetComponent<AlgorithmOwner>();
            particleSystemsOwner = GameObject.Find("ParticleSystemsOwner").GetComponent<ParticleSystemsOwner>();
            gimmickOwner = GameObject.Find("GimmickOwner").GetComponent<GimmickOwner>();
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
            inputSystemsOwner.OnStart();
            skyBoxOwner.OnStart();
            levelOwner.OnStart();
            algorithmOwner.OnStart();
            particleSystemsOwner.OnStart();
            gimmickOwner.OnStart();
        }
    }

    /// <summary>
    /// ゲームマネージャーとのインターフェース
    /// </summary>
    public interface IMainGameManager
    {
        /// <summary>
        /// オンスタート
        /// </summary>
        void OnStart();
    }
}
