using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UniRx;

namespace Main.Model
{
    /// <summary>
    /// コシギンチャク
    /// モデル
    /// </summary>
    [RequireComponent(typeof(LoinclothConfig))]
    public class LoinclothModel : MonoBehaviour
    {
        /// <summary>設定</summary>
        [SerializeField] private LoinclothConfig loinclothConfig;
        /// <summary>設定</summary>
        public LoinclothConfig LoinclothConfig => loinclothConfig;
        /// <summary>ステージ番号</summary>
        private int _index = -1;
        /// <summary>番号置換の正規表現</summary>
        private readonly Regex _regex = new Regex("^.*_");
        /// <summary>ステージ番号</summary>
        public int Index
        {
            get
            {
                // ステージ番号セットの初期処理
                if (_index < 0)
                    _index = int.Parse(_regex.Replace(name, ""));
                if (_index < 0)
                    throw new System.Exception("置換失敗");
                return _index;
            }
        }
        /// <summary>生成された</summary>
        private readonly BoolReactiveProperty _instanced = new BoolReactiveProperty();
        /// <summary>生成された</summary>
        public IReactiveProperty<bool> Instanced => _instanced;

        private void Reset()
        {
            loinclothConfig = GetComponent<LoinclothConfig>();
        }

        private void Start()
        {
            _instanced.Value = true;
        }
    }
}
