using System.Threading;
using Cysharp.Threading.Tasks;
using Enums;
using Interfaces;

namespace ImageLoadStrategies
{
    public class OneByOneLoadStrategy : IImageLoadStrategy
    {
        private readonly IImageDownloader _imageDownloader;
        private readonly ICardFlipper _cardFlipper;

        public OneByOneLoadStrategy(IImageDownloader imageDownloader, ICardFlipper cardFlipper)
        {
            _imageDownloader = imageDownloader;
            _cardFlipper = cardFlipper;
        }

        public string Name => "One By One";

        public UniTask LoadImagesAsync(ICard[] cards, string uri)
        {
            throw new System.NotImplementedException();
        }
    }
}