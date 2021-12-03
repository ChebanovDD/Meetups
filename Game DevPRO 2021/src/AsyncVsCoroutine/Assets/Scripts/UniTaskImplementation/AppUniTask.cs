using System;
using System.Linq;
using System.Threading;
using Common;
using Common.Extensions;
using Common.Interfaces;
using Cysharp.Threading.Tasks;
using UniTaskImplementation.ImageLoadStrategies;
using UniTaskImplementation.Interfaces;
using UnityEngine;

namespace UniTaskImplementation
{
    public class AppUniTask : MonoBehaviour
    {
        private const string ImageUrl = "https://picsum.photos/256";

        [SerializeField] private GameCanvas _gameCanvas;

        private bool _isDestroyed;

        private ICard[] _cards;
        private IImageDownloader _imageDownloader;
        private IImageLoadStrategy[] _imageLoadStrategies;
        private CancellationTokenSource _cancellationTokenSource;

        private void Awake()
        {
            _cards = _gameCanvas.GetCards();
            _imageDownloader = GetImageDownloader();
            _imageLoadStrategies = GetLoadStrategies(_imageDownloader, GetComponent<ICardFlipper>());
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
            _isDestroyed = true;
            _cancellationTokenSource?.Cancel();
        
            _cards.Clear();
            _gameCanvas.LoadButton.Click -= OnLoadButtonClick;
            _gameCanvas.CancelButton.Click -= OnCancelButtonClick;
        }
    
        private void OnLoadButtonClick()
        {
            LoadImages().Forget();
        }

        private void OnCancelButtonClick()
        {
            CancelLoading();
        }

        private IImageDownloader GetImageDownloader()
        {
            return new ImageDownloader();
        }

        private IImageLoadStrategy[] GetLoadStrategies(IImageDownloader imageDownloader, ICardFlipper cardFlipper)
        {
            return new IImageLoadStrategy[]
            {
                new AllAtOnceLoadStrategy(imageDownloader, cardFlipper),
                new OneByOneLoadStrategy(imageDownloader, cardFlipper),
                new WhenReadyLoadStrategy(imageDownloader, cardFlipper)
            };
        }

        private async UniTaskVoid LoadImages()
        {
            SetUiInteractable(false);
            await LoadImagesAsync(GetSelectedLoadStrategy());
            SetUiInteractable(true);
        }

        private async UniTask LoadImagesAsync(IImageLoadStrategy loadStrategy)
        {
            try
            {
                _cancellationTokenSource = new CancellationTokenSource();
                await loadStrategy.LoadImagesAsync(_cards, ImageUrl, _cancellationTokenSource.Token);
            }
            catch (OperationCanceledException e)
            {
                Debug.Log(e.Message);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
            finally
            {
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null;
            }
        }

        private void CancelLoading()
        {
            _cancellationTokenSource?.Cancel();
        }

        private void SetUiInteractable(bool value)
        {
            if (_isDestroyed)
            {
                return;
            }
            
            _gameCanvas.Dropdown.SetInteractable(value);
            _gameCanvas.LoadButton.SetInteractable(value);
            _gameCanvas.CancelButton.SetInteractable(!value);
        }

        private IImageLoadStrategy GetSelectedLoadStrategy()
        {
            return _imageLoadStrategies[_gameCanvas.Dropdown.SelectedItem];
        }
    }
}