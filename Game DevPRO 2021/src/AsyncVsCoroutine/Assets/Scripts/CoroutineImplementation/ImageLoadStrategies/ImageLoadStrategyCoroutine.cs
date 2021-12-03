using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Common.Interfaces;
using UnityEngine;

namespace CoroutineImplementation.ImageLoadStrategies
{
    public abstract class ImageLoadStrategyCoroutine
    {
        protected ImageLoadStrategyCoroutine(ImageDownloaderCoroutine imageDownloader,
            SimpleCardFlipperCoroutine cardFlipper)
        {
            CardFlipper = cardFlipper;
            ImageDownloader = imageDownloader;
        }

        public abstract string Name { get; }

        public abstract IEnumerator LoadImagesCoroutine(ICard[] cards, string uri,
            CancellationToken cancellationToken = default);

        protected SimpleCardFlipperCoroutine CardFlipper { get; }
        protected ImageDownloaderCoroutine ImageDownloader { get; }
        
        protected IEnumerator WhenAll(IEnumerable<IEnumerator> routines, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                yield break;
            }

            var startedCoroutines = StartCoroutines(routines, cancellationToken);
            
            foreach (var startedCoroutine in startedCoroutines)
            {
                if (cancellationToken.IsCancellationRequested == false)
                {
                    yield return startedCoroutine;
                }
                else if (startedCoroutine != null)
                {
                    CardFlipper.StopCoroutine(startedCoroutine);
                }
            }
        }

        protected IEnumerator ForEach(IEnumerable<ICard> cards, Func<ICard, IEnumerator> routine,
            CancellationToken cancellationToken = default)
        {
            foreach (var card in cards)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    yield break;
                }

                yield return routine(card);
            }
        }
        
        private IEnumerable<Coroutine> StartCoroutines(IEnumerable<IEnumerator> routines,
            CancellationToken cancellationToken = default)
        {
            var startedCoroutines = new List<Coroutine>();
            
            foreach (var routine in routines)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                startedCoroutines.Add(CardFlipper.StartCoroutine(routine));
            }

            return startedCoroutines;
        }
    }
}