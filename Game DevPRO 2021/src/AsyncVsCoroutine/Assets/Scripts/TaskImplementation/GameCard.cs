using Common.Enums;
using Common.Extensions;
using Common.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace TaskImplementation
{
    public class GameCard : MonoBehaviour, ICard
    {
        [SerializeField] private Image _background;
        [SerializeField] private RawImage _art;
        [SerializeField] private GameObject _content;

        [Header("Skin")]
        [SerializeField] private Sprite _face;
        [SerializeField] private Sprite _back;
    
        private Vector3 _defaultScale;
        private CardSide _currentCardSide;

        public Transform Transform => transform;
        public Vector3 DefaultScale => _defaultScale;
        public CardSide CurrentCardSide => _currentCardSide;

        private void Awake()
        {
            _defaultScale = transform.localScale;
        }

        private void OnDestroy()
        {
            _art.texture.Release();
        }

        public void SetArt(Texture2D texture)
        {
            _art.texture.Release();
            _art.texture = texture;
        }

        public void SetCardSide(CardSide cardSide)
        {
            _currentCardSide = cardSide;
            _content.SetActive(cardSide == CardSide.Front);
            _background.sprite = cardSide == CardSide.Back ? _back : _face;
        }
    }
}