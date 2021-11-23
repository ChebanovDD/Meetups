using System.Threading;
using Cysharp.Threading.Tasks;
using Interfaces;
using UnityEngine;
using UnityEngine.Networking;

public class ImageDownloader : IImageDownloader
{
    public async UniTask<Texture2D> DownloadImageAsync(string uri, CancellationToken cancellationToken = default)
    {
        using var www = UnityWebRequestTexture.GetTexture(uri);
        
        await www.SendWebRequest().WithCancellation(cancellationToken);
        
        return www.result == UnityWebRequest.Result.Success ? DownloadHandlerTexture.GetContent(www) : null;
    }
}