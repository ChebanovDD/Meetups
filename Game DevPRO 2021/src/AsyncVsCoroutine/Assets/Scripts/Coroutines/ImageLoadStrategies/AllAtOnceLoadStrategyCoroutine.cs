﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Enums;
using Interfaces;

namespace Coroutines.ImageLoadStrategies
{
    public class AllAtOnceLoadStrategyCoroutine : ImageLoadStrategyCoroutine
    {
        public AllAtOnceLoadStrategyCoroutine(ImageDownloaderCoroutine imageDownloader,
            SimpleCardFlipperCoroutine cardFlipper) : base(imageDownloader, cardFlipper)
        {
        }

        public override string Name => "All At Once";

        protected override IEnumerator OnLoadImages(ICard[] cards, string uri, CancellationToken cancellationToken)
        {
            var routines = new List<IEnumerator>();
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