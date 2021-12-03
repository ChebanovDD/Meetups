using System.Threading;
using Common.Enums;
using Common.Interfaces;
using Cysharp.Threading.Tasks;
using TaskImplementation.Interfaces;

namespace TaskImplementation.ImageLoadStrategies
{
    public class OneByOneLoadStrategy : IImageLoadStrategy
    {
        private readonly IImageDownloader _imageDownloader;
        private readonly ICardFlipper _cardFlipper;

        public OneByOneLoadStrategy(IImageDownloader imageDownloader, ICardFlipper cardFlipper)
        {
            _imageDownloader = imageDownloader;
            _cardFlipper = cardFlipper;
        }

        public string Name => "One By One";

        public async UniTask LoadImagesAsync(ICard[] cards, string uri, CancellationToken cancellationToken = default)
        {
            await UniTask.WhenAll(cards.Select(async card =>
            {
                var downloadImageTask = _imageDownloader.DownloadImageAsync(uri, cancellationToken);

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