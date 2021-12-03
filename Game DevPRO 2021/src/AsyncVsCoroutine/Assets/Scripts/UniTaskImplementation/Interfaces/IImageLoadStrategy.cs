using System.Threading;
using Common.Interfaces;
using Cysharp.Threading.Tasks;

namespace UniTaskImplementation.Interfaces
{
    public interface IImageLoadStrategy
    {
        string Name { get; }
        UniTask LoadImagesAsync(ICard[] cards, string uri, CancellationToken cancellationToken = default);
    }
}