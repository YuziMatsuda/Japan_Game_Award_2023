using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Area.Presenter;
using Area.Audio;

namespace Area.Common
{
    /// <summary>
    /// セレクトのゲームマネージャー
    /// </summary>
    public class AreaGameManager : MonoBehaviour
    {
        /// <summary>セレクトのゲームマネージャー</summary>
        private static AreaGameManager _instance;
        /// <summary>セレクトのゲームマネージャー</summary>
        public static AreaGameManager Instance => _instance;

        /// <summary>プレゼンタ</summary>
        [SerializeField] private AreaPresenter presenter;
        /// <summary>プレゼンタ</summary>
        public AreaPresenter Presenter => presenter;
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
        /// <summary>ギミックのオーナー</summary>
        [SerializeField] private GimmickOwner gimmickOwner;
        /// <summary>ギミックのオーナー</summary>
        public GimmickOwner GimmickOwner => gimmickOwner;
        /// <summary>実績一覧管理のオーナー</summary>
        [SerializeField] private MissionOwner missionOwner;
        /// <summary>実績一覧管理のオーナー</summary>
        public MissionOwner MissionOwner => missionOwner;
        /// <summary>パーティクルのオーナー</summary>
        [SerializeField] private ParticleSystemsOwner particleSystemsOwner;
        /// <summary>パーティクルのオーナー</summary>
        public ParticleSystemsOwner ParticleSystemsOwner => particleSystemsOwner;

        private void Reset()
        {
            presenter = GameObject.Find("Presenter").GetComponent<AreaPresenter>();
            audioOwner = GameObject.Find("AudioOwner").GetComponent<AudioOwner>();
            sceneOwner = GameObject.Find("SceneOwner").GetComponent<SceneOwner>();
            cursorVisible = GameObject.Find("CursorVisible").GetComponent<CursorVisible>();
            particleSystemsOwner = GameObject.Find("ParticleSystemsOwner").GetComponent<ParticleSystemsOwner>();
            gimmickOwner = GameObject.Find("GimmickOwner").GetComponent<GimmickOwner>();
            missionOwner = GameObject.Find("MissionOwner").GetComponent<MissionOwner>();
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
            particleSystemsOwner.OnStart();
            gimmickOwner.OnStart();
        }
    }

    /// <summary>
    /// ゲームマネージャーとのインターフェース
    /// </summary>
    public interface IAreaGameManager
    {
        /// <summary>
        /// オンスタート
        /// </summary>
        void OnStart();
    }
}
