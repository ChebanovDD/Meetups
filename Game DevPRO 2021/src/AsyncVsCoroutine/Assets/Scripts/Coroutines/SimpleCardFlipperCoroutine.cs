using System.Collections;
using DG.Tweening;
using Enums;
using Interfaces;
using UnityEngine;

namespace Coroutines
{
    public class SimpleCardFlipperCoroutine : MonoBehaviour
    {
        [Header("Animation")]
        [SerializeField] private float _maxScaleValue = 1.0f;
        [SerializeField] private float _animationSpeed = 1.0f;
        [SerializeField] private Ease _flipEaseType = Ease.Flash;

        private Vector3 _rotateTo;
        private Vector3 _maxScale;

        private void Awake()
        {
            _rotateTo = new Vector3(0, 90, 0);
            _maxScale = Vector3.one * _maxScaleValue;
        }

        public IEnumerator FlipCardCoroutine(ICard card, CardSide cardSide)
        {
            if (card.CurrentCardSide == cardSide)
            {
                yield break;
            }

            yield return AnimateCardFlipCoroutine(card.Transform, _rotateTo, _maxScale);
            card.SetCardSide(cardSide);
            yield return AnimateCardFlipCoroutine(card.Transform, Vector3.zero, card.DefaultScale);
        }

        private IEnumerator AnimateCardFlipCoroutine(Transform cardTransform, Vector3 rotationEndValue,
            Vector3 scaleEndValue)
        {
            yield return DOTween.Sequence()
                .Join(cardTransform.DORotate(rotationEndValue, _animationSpeed))
                .Join(cardTransform.DOScale(scaleEndValue, _animationSpeed))
                .SetEase(_flipEaseType)
                .WaitForCompletion();
        }
    }
}