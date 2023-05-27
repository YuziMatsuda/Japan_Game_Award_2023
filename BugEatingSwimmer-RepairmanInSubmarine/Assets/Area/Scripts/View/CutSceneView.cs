using Area.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Area.View
{
    /// <summary>
    /// シーンカット
    /// ビュー
    /// </summary>
    public class CutSceneView : MonoBehaviour, IFadeImageView, IRecollectionPictures, ICutSceneView
    {
        /// <summary>回想シーン</summary>
        [SerializeField] private RecollectionPictures recollectionPictures;
        /// <summary>フェード</summary>
        [SerializeField] private FadeImageView fadeImageView;

        public IEnumerator PlayBetweenFrameAdvanceAtFadeMode(System.IObserver<int> observer, EnumRecollectionPicture from, EnumRecollectionPicture to)
        {
            var index = new IntReactiveProperty();
            index.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    if (x <= (int)to - (int)from)
                    {
                        Observable.FromCoroutine<bool>(observer => PlayFadeAnimation(observer, EnumFadeState.Close))
                            .Subscribe(_ =>
                            {
                                if (!recollectionPictures.SetSprite((EnumRecollectionPicture)((int)from + x)))
                                    observer.OnNext(x);
                                Observable.FromCoroutine<bool>(observer => PlayFadeAnimation(observer, EnumFadeState.Open))
                                    .Subscribe(_ => index.Value++)
                                    .AddTo(gameObject);
                            })
                            .AddTo(gameObject);
                    }
                    else
                    {
                        observer.OnNext(x);
                    }
                });
            yield return null;
        }

        public IEnumerator PlayFadeAnimation(System.IObserver<bool> observer, EnumFadeState state)
        {
            Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimationOfRecollection(observer, state))
                .Subscribe(_ => observer.OnNext(true))
                .AddTo(gameObject);

            yield return null;
        }

        public IEnumerator PlayFadeAnimationOfRecollection(System.IObserver<bool> observer, EnumFadeState state)
        {
            throw new System.NotImplementedException();
        }

        public bool SetAlpha(EnumFadeState state)
        {
            throw new System.NotImplementedException();
        }

        public bool SetSprite(EnumRecollectionPicture index)
        {
            return ((IRecollectionPictures)recollectionPictures).SetSprite(index);
        }

        private void Reset()
        {
            recollectionPictures = GetComponentInChildren<RecollectionPictures>();
            fadeImageView = GetComponentInChildren<FadeImageView>();
        }
    }

    /// <summary>
    /// シーンカット
    /// ビュー
    /// インターフェース
    /// </summary>
    public interface ICutSceneView
    {
        /// <summary>
        /// コマ送りでカットを切り替える
        /// フェードモード
        /// </summary>
        /// <param name="from">このカットから</param>
        /// <param name="to">このカットまで</param>
        /// <returns>カット切り替え回数</returns>
        public IEnumerator PlayBetweenFrameAdvanceAtFadeMode(System.IObserver<int> observer, EnumRecollectionPicture from, EnumRecollectionPicture to);
    }
}
