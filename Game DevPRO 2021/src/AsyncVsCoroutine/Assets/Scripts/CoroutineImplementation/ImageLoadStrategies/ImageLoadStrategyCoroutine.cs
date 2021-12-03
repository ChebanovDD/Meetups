using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Common.Interfaces;
using UnityEngine;

namespace CoroutineImplementation.ImageLoadStrategies
{
    public abstract class ImageLoadStrategyCoroutine
    {
        private readonly MonoBehaviour _monoBehaviour;
        
        protected ImageLoadStrategyCoroutine(ImageDownloader imageDownloader,
            SimpleCardFlipper cardFlipper)
        {
            CardFlipper = cardFlipper;
            ImageDownloader = imageDownloader;
            
            _monoBehaviour = cardFlipper; // Note: Uses only MonoBehaviour to control coroutines.
        }

        public abstract string Name { get; }

        public abstract IEnumerator LoadImagesCoroutine(ICard[] cards, string uri,
            CancellationToken cancellationToken = default);

        protected SimpleCardFlipper CardFlipper { get; }
        protected ImageDownloader ImageDownloader { get; }
        
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
                    StopCoroutine(startedCoroutine);
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

                startedCoroutines.Add(StartCoroutine(routine));
            }

            return startedCoroutines;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Coroutine StartCoroutine(IEnumerator routine)
        {
            return _monoBehaviour.StartCoroutine(routine);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void StopCoroutine(Coroutine coroutine)
        {
            _monoBehaviour.StopCoroutine(coroutine);
        }
    }
}