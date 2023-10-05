using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Area.View
{
    /// <summary>
    /// マーチ移動制御UI
    /// ビュー
    /// </summary>
    public class MarchMovementUI : MarchMovementParent
    {
        private void Reset()
        {
            marchDistance = -1109f;
        }

        protected override void PlayMarching()
        {
            if (_rectTransform == null)
                _rectTransform = transform as RectTransform;
            _rectTransform.DOAnchorPosX(marchDistance, durations[0])
                .SetEase(Ease.Linear);
        }
    }
}
