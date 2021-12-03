using System.Collections;
using System.Linq;
using System.Threading;
using Common.Enums;
using Common.Interfaces;

namespace CoroutineImplementation.ImageLoadStrategies
{
    public class WhenReadyLoadStrategy : ImageLoadStrategy
    {
        public WhenReadyLoadStrategy(ImageDownloader imageDownloader, SimpleCardFlipper cardFlipper) 
            : base(imageDownloader, cardFlipper)
        {
        }

        public override string Name => "When Image Ready";

        public override IEnumerator LoadImagesCoroutine(ICard[] cards, string uri,
            CancellationToken cancellationToken = default)
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
            
            yield return CardFlipper.FlipCardCoroutine(card, CardSide.Face);
        }
    }
}