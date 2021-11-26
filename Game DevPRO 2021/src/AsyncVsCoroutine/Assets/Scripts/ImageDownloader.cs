using System.Threading;
using Cysharp.Threading.Tasks;
using Interfaces;
using UnityEngine;
using UnityEngine.Networking;

public class ImageDownloader : IImageDownloader
{
    public UniTask<Texture2D> DownloadImageAsync(string uri)
    {
        throw new System.NotImplementedException();
    }
}