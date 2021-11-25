using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Interfaces;
using UnityEngine;

namespace Coroutines.ImageLoadStrategies
{
    public abstract class ImageLoadStrategyCoroutine
    {
        protected readonly ImageDownloaderCoroutine ImageDownloader;
        protected readonly SimpleCardFlipperCoroutine CardFlipper;

        protected ImageLoadStrategyCoroutine(ImageDownloaderCoroutine imageDownloader,
            SimpleCardFlipperCoroutine cardFlipper)
        {
            ImageDownloader = imageDownloader;
            CardFlipper = cardFlipper;
        }

        public abstract string Name { get; }

        public abstract IEnumerator LoadImagesCoroutine(ICard[] cards, string uri,
            CancellationToken cancellationToken = default);

        protected IEnumerator WhenAll(IEnumerable<IEnumerator> routines, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                yield break;
            }
            
            var coroutines = new List<Coroutine>();
            foreach (var coroutine in routines)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    yield break;
                }
                
                coroutines.Add(CardFlipper.StartCoroutine(coroutine));
            }

            foreach (var coroutine in coroutines)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    yield break;
                }
                
                yield return coroutine;
            }
        }

        protected IEnumerator ForEach(IEnumerable<ICard> cards, Func<ICard, IEnumerator> routine,
            CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                yield break;
            }

            foreach (var card in cards)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    yield break;
                }

                yield return routine(card);
            }
        }
    }
}