using System.Threading;
using Cysharp.Threading.Tasks;
using Enums;
using Interfaces;

namespace ImageLoadStrategies
{
    public class WhenReadyLoadStrategy : IImageLoadStrategy
    {
        private readonly IImageDownloader _httpClient;
        private readonly ICardFlipper _cardFlipper;

        public WhenReadyLoadStrategy(IImageDownloader httpClient, ICardFlipper cardFlipper)
        {
            _httpClient = httpClient;
            _cardFlipper = cardFlipper;
        }

        public string Name => "When Image Ready";

        public async UniTask LoadImagesAsync(ICard[] cards, string uri, CancellationToken cancellationToken = default)
        {
            await UniTask.WhenAll(cards.Select(async card =>
            {
                var downloadImageTask = _httpClient.DownloadImageAsync(uri, cancellationToken);
                
                await _cardFlipper.FlipCardAsync(card, CardSide.Back);
                card.SetArt(await downloadImageTask);
                await _cardFlipper.FlipCardAsync(card, CardSide.Front);
            }));
        }
    }
}