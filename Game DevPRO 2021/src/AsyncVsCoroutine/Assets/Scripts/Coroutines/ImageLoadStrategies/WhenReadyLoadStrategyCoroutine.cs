using System.Collections;
using System.Linq;
using System.Threading;
using Enums;
using Interfaces;

namespace Coroutines.ImageLoadStrategies
{
    public class WhenReadyLoadStrategyCoroutine : ImageLoadStrategyCoroutine
    {
        public WhenReadyLoadStrategyCoroutine(ImageDownloaderCoroutine imageDownloader,
            SimpleCardFlipperCoroutine cardFlipper) : base(imageDownloader, cardFlipper)
        {
        }

        public override string Name => "When Image Ready";

        protected override IEnumerator OnLoadImages(ICard[] cards, string uri, CancellationToken cancellationToken)
        {
            yield return WhenAll(cards.Select(card => LoadImage(card, uri, cancellationToken)), cancellationToken);
        }

        private IEnumerator LoadImage(ICard card, string uri, CancellationToken cancellationToken)
        {
            var routines = new[]
            {
                ImageDownloader.DownloadImageCoroutine(uri, card.SetArt, cancellationToken),
                CardFlipper.FlipCardCoroutine(card, CardSide.Back)
            };

            yield return WhenAll(routines, cancellationToken);

            if (cancellationToken.IsCancellationRequested)
            {
                yield break;
            }
            
            yield return CardFlipper.FlipCardCoroutine(card, CardSide.Front);
        }
    }
}