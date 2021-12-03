using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TaskImplementation.Interfaces
{
    public interface IImageDownloader
    {
        UniTask<Texture2D> DownloadImageAsync(string uri, CancellationToken cancellationToken = default);
    }
}