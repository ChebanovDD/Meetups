using System.Threading;
using Common.Enums;
using Common.Interfaces;
using Cysharp.Threading.Tasks;
using UniTaskImplementation.Interfaces;

namespace UniTaskImplementation.ImageLoadStrategies
{
    public class WhenReadyLoadStrategy : IImageLoadStrategy
    {
        private readonly IImageDownloader _imageDownloader;
        private readonly ICardFlipper _cardFlipper;

        public WhenReadyLoadStrategy(IImageDownloader imageDownloader, ICardFlipper cardFlipper)
        {
            _imageDownloader = imageDownloader;
            _cardFlipper = cardFlipper;
        }

        public string Name => "When Image Ready";

        public async UniTask LoadImagesAsync(ICard[] cards, string uri, CancellationToken cancellationToken = default)
        {
            await UniTask.WhenAll(cards.Select(async card =>
            {
                var downloadImageTask = _imageDownloader.DownloadImageAsync(uri, cancellationToken);

                await _cardFlipper.FlipCardAsync(card, CardSide.Back);
                card.SetArt(await downloadImageTask);
                await _cardFlipper.FlipCardAsync(card, CardSide.Face);
            }));
        }
    }
}