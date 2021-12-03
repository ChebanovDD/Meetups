using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Common.Enums;
using Common.Interfaces;

namespace CoroutineImplementation.ImageLoadStrategies
{
    public class AllAtOnceLoadStrategy : ImageLoadStrategyCoroutine
    {
        public AllAtOnceLoadStrategy(ImageDownloader imageDownloader,
            SimpleCardFlipper cardFlipper) : base(imageDownloader, cardFlipper)
        {
        }

        public override string Name => "All At Once";

        public override IEnumerator LoadImagesCoroutine(ICard[] cards, string uri,
            CancellationToken cancellationToken = default)
        {
            var routines = new List<IEnumerator>(cards.Length * 2);
            foreach (var card in cards)
            {
                routines.Add(ImageDownloader.DownloadImageCoroutine(uri, card.SetArt, cancellationToken));
                routines.Add(CardFlipper.FlipCardCoroutine(card, CardSide.Back));
            }

            yield return WhenAll(routines, cancellationToken);
            yield return WhenAll(cards.Select(card => CardFlipper.FlipCardCoroutine(card, CardSide.Front)),
                cancellationToken);
        }
    }
}