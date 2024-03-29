﻿using System.Threading;
using Common.Enums;
using Common.Interfaces;
using Cysharp.Threading.Tasks;
using UniTaskImplementation.Interfaces;

namespace UniTaskImplementation.ImageLoadStrategies
{
    public class AllAtOnceLoadStrategy : IImageLoadStrategy
    {
        private readonly IImageDownloader _imageDownloader;
        private readonly ICardFlipper _cardFlipper;

        public AllAtOnceLoadStrategy(IImageDownloader imageDownloader, ICardFlipper cardFlipper)
        {
            _imageDownloader = imageDownloader;
            _cardFlipper = cardFlipper;
        }

        public string Name => "All At Once";

        public async UniTask LoadImagesAsync(ICard[] cards, string uri, CancellationToken cancellationToken = default)
        {
            var downloadAndFlipBack = cards.Select(async card =>
            {
                var downloadImageTask = _imageDownloader.DownloadImageAsync(uri, cancellationToken);

                await _cardFlipper.FlipCardAsync(card, CardSide.Back);
                card.SetArt(await downloadImageTask);
            });

            await UniTask.WhenAll(downloadAndFlipBack);
            await UniTask.WhenAll(cards.Select(card => _cardFlipper.FlipCardAsync(card, CardSide.Face)));
        }
    }
}