using System;
using System.Collections;
using System.Linq;
using System.Threading;
using Coroutines.ImageLoadStrategies;
using Extensions;
using Interfaces;
using UiElements;
using UnityEngine;

namespace Coroutines
{
    public class AppCoroutine : MonoBehaviour
    {
        private const string ImageUrl = "https://picsum.photos/256";

        [SerializeField] private Transform _cardsContainer;
        [SerializeField] private SimpleCardFlipperCoroutine _cardFlipper;
        
        [Space]
        [SerializeField] private InteractableButton _loadButton;
        [SerializeField] private InteractableButton _cancelButton;
        [SerializeField] private InteractableDropdown _imageLoadersDropDown;

        private ICard[] _cards;
        private ImageDownloaderCoroutine _imageDownloader;
        private ImageLoadStrategyCoroutine[] _imageLoadStrategies;
        private CancellationTokenSource _cancellationTokenSource;

        private void Awake()
        {
            _cards = GetCards();
            _imageDownloader = GetImageDownloader();
            _imageLoadStrategies = GetLoadStrategies(_imageDownloader, _cardFlipper);
        }

        private void Start()
        {
            _cancelButton.SetInteractable(false);
            _cancelButton.Click.AddListener(OnCancelButtonClick);

            _loadButton.Click.AddListener(OnLoadButtonClick);
            _imageLoadersDropDown.AddItems(_imageLoadStrategies.Select(t => t.Name));
        }

        private void OnDestroy()
        {
            _cards.Clear();
            _cancellationTokenSource?.Dispose();
            
            _loadButton.Click.RemoveListener(OnLoadButtonClick);
            _cancelButton.Click.RemoveListener(OnCancelButtonClick);
            
        }
        
        private void OnLoadButtonClick()
        {
            SetUiInteractable(false);
            StartCoroutine(LoadImagesCoroutine(GetSelectedLoadStrategy(), () =>
            {
                SetUiInteractable(true);
            }));
        }

        private void OnCancelButtonClick()
        {
            CancelLoading();
        }

        private ICard[] GetCards()
        {
            return _cardsContainer.GetComponentsInChildren<ICard>();
        }

        private ImageDownloaderCoroutine GetImageDownloader()
        {
            return new ImageDownloaderCoroutine();
        }

        private ImageLoadStrategyCoroutine[] GetLoadStrategies(ImageDownloaderCoroutine imageDownloader,
            SimpleCardFlipperCoroutine cardFlipper)
        {
            return new ImageLoadStrategyCoroutine[]
            {
                new AllAtOnceLoadStrategyCoroutine(imageDownloader, cardFlipper),
                new OneByOneLoadStrategyCoroutine(imageDownloader, cardFlipper),
                new WhenReadyLoadStrategyCoroutine(imageDownloader, cardFlipper)
            };
        }

        private IEnumerator LoadImagesCoroutine(ImageLoadStrategyCoroutine loadStrategy, Action callback)
        {
            _cancellationTokenSource = new CancellationTokenSource();

            yield return loadStrategy.LoadImagesCoroutine(_cards, ImageUrl, _cancellationTokenSource.Token);
            
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;

            callback?.Invoke();
        }

        private void CancelLoading()
        {
            _cancellationTokenSource?.Cancel();
        }
        
        private void SetUiInteractable(bool value)
        {
            _cancelButton.SetInteractable(!value);

            _loadButton.SetInteractable(value);
            _imageLoadersDropDown.SetInteractable(value);
        }

        private ImageLoadStrategyCoroutine GetSelectedLoadStrategy()
        {
            return _imageLoadStrategies[_imageLoadersDropDown.SelectedItem];
        }
    }
}