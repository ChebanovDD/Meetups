using System;
using System.Collections;
using System.Linq;
using System.Threading;
using Common;
using Common.Extensions;
using Common.Interfaces;
using CoroutineImplementation.ImageLoadStrategies;
using UnityEngine;

namespace CoroutineImplementation
{
    public class AppCoroutine : MonoBehaviour
    {
        private const string ImageUrl = "https://picsum.photos/256";

        [SerializeField] private GameCanvas _gameCanvas;
        [SerializeField] private SimpleCardFlipper _cardFlipper;

        private ICard[] _cards;
        private ImageDownloader _imageDownloader;
        private ImageLoadStrategy[] _imageLoadStrategies;
        private CancellationTokenSource _cancellationTokenSource;

        private void Awake()
        {
            _cards = _gameCanvas.GetCards();
            _imageDownloader = GetImageDownloader();
            _imageLoadStrategies = GetLoadStrategies(_imageDownloader, _cardFlipper);
        }

        private void Start()
        {
            _gameCanvas.LoadButton.Click += OnLoadButtonClick;
            _gameCanvas.CancelButton.Click += OnCancelButtonClick;

            _gameCanvas.CancelButton.SetInteractable(false);
            _gameCanvas.Dropdown.AddItems(_imageLoadStrategies.Select(strategy => strategy.Name));
        }

        private void OnDestroy()
        {
            _cards.Clear();
            _cancellationTokenSource?.Dispose();
            
            _gameCanvas.LoadButton.Click -= OnLoadButtonClick;
            _gameCanvas.CancelButton.Click -= OnCancelButtonClick;
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

        private ImageDownloader GetImageDownloader()
        {
            return new ImageDownloader();
        }

        private ImageLoadStrategy[] GetLoadStrategies(ImageDownloader imageDownloader,
            SimpleCardFlipper cardFlipper)
        {
            return new ImageLoadStrategy[]
            {
                new AllAtOnceLoadStrategy(imageDownloader, cardFlipper),
                new OneByOneLoadStrategy(imageDownloader, cardFlipper),
                new WhenReadyLoadStrategy(imageDownloader, cardFlipper)
            };
        }

        private IEnumerator LoadImagesCoroutine(ImageLoadStrategy loadStrategy, Action callback)
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
            _gameCanvas.Dropdown.SetInteractable(value);
            _gameCanvas.LoadButton.SetInteractable(value);
            _gameCanvas.CancelButton.SetInteractable(!value);
        }

        private ImageLoadStrategy GetSelectedLoadStrategy()
        {
            return _imageLoadStrategies[_gameCanvas.Dropdown.SelectedIndex];
        }
    }
}