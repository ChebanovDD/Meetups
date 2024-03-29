﻿using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Common.Enums;
using Common.Interfaces;

namespace CoroutineImplementation.ImageLoadStrategies
{
    public class OneByOneLoadStrategy : ImageLoadStrategy
    {
        public OneByOneLoadStrategy(ImageDownloader imageDownloader, SimpleCardFlipper cardFlipper) 
            : base(imageDownloader, cardFlipper)
        {
        }

        public override string Name => "One By One";

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
            yield return ForEach(cards, card => CardFlipper.FlipCardCoroutine(card, CardSide.Face), cancellationToken);
        }
    }
}