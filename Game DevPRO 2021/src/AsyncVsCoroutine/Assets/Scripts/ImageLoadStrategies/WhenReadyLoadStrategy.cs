using System.Threading;
using Cysharp.Threading.Tasks;
using Enums;
using Interfaces;

namespace ImageLoadStrategies
{
    public class WhenReadyLoadStrategy : IImageLoadStrategy
    {
        private readonly IImageDownloader _imageDownloader;
        private readonly ICardFlipper _cardFlipper;

        public WhenReadyLoadStrategy(IImageDownloader imageDownloader, ICardFlipper cardFlipper)
        {
            _imageDownloader = imageDownloader;
            _cardFlipper = cardFlipper;
        }

        public string Name => "When Image Ready";

        public UniTask LoadImagesAsync(ICard[] cards, string uri)
        {
            throw new System.NotImplementedException();
        }
    }
}