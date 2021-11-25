using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Extensions;
using ImageLoadStrategies;
using Interfaces;
using UiElements;
using UnityEngine;

public class App : MonoBehaviour
{
    private const string ImageUrl = "https://picsum.photos/256";

    [SerializeField] private Transform _cardsContainer;
    [SerializeField] private InteractableButton _loadButton;
    [SerializeField] private InteractableButton _cancelButton;
    [SerializeField] private InteractableDropdown _imageLoadersDropDown;

    private bool _isDestroyed;

    private ICard[] _cards;
    private IImageDownloader _imageDownloader;
    private IImageLoadStrategy[] _imageLoadStrategies;
    private CancellationTokenSource _cancellationTokenSource;

    private void Awake()
    {
        _cards = GetCards();
        _imageDownloader = GetImageDownloader();
        _imageLoadStrategies = GetLoadStrategies(_imageDownloader, GetComponent<ICardFlipper>());
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
        _isDestroyed = true;
        _cancellationTokenSource?.Cancel();
        
        _cards.Clear();
        _loadButton.Click.RemoveListener(OnLoadButtonClick);
        _cancelButton.Click.RemoveListener(OnCancelButtonClick);
    }
    
    private async void OnLoadButtonClick()
    {
        SetUiInteractable(false);
        await LoadImagesAsync(GetSelectedLoadStrategy());
        SetUiInteractable(true);
    }

    private void OnCancelButtonClick()
    {
        CancelLoading();
    }
    
    private ICard[] GetCards()
    {
        return _cardsContainer.GetComponentsInChildren<ICard>();
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
        
        _cancelButton.SetInteractable(!value);
        
        _loadButton.SetInteractable(value);
        _imageLoadersDropDown.SetInteractable(value);
    }

    private IImageLoadStrategy GetSelectedLoadStrategy()
    {
        return _imageLoadStrategies[_imageLoadersDropDown.SelectedItem];
    }
}