using System.Threading;
using Cysharp.Threading.Tasks;

namespace Interfaces
{
    public interface IImageLoadStrategy
    {
        string Name { get; }
        UniTask LoadImagesAsync(ICard[] cards, string uri);
    }
}