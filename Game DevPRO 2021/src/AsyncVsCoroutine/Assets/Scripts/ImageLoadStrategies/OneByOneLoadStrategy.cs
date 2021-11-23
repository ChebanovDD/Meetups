using System.Threading;
using Cysharp.Threading.Tasks;
using Enums;
using Interfaces;

namespace ImageLoadStrategies
{
    public class OneByOneLoadStrategy : IImageLoadStrategy
    {
        private readonly IImageDownloader _httpClient;
        private readonly ICardFlipper _cardFlipper;

        public OneByOneLoadStrategy(IImageDownloader httpClient, ICardFlipper cardFlipper)
        {
            _httpClient = httpClient;
            _cardFlipper = cardFlipper;
        }

        public string Name => "One By One";

        public async UniTask LoadImagesAsync(ICard[] cards, string uri, CancellationToken cancellationToken = default)
        {
            await UniTask.WhenAll(cards.Select(async card =>
            {
                var downloadImageTask = _httpClient.DownloadImageAsync(uri, cancellationToken);
                
                await _cardFlipper.FlipCardAsync(card, CardSide.Back);
                card.SetArt(await downloadImageTask);
            }));
        
            foreach (var card in cards)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await _cardFlipper.FlipCardAsync(card, CardSide.Front);
            }
        }
    }
}