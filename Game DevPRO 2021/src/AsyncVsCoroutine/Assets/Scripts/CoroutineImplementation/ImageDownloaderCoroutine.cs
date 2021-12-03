using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

namespace CoroutineImplementation
{
    public class ImageDownloaderCoroutine
    {
        public IEnumerator DownloadImageCoroutine(string url, Action<Texture2D> callback,
            CancellationToken cancellationToken)
        {
            using var www = UnityWebRequestTexture.GetTexture(url);

            var request = www.SendWebRequest();
            while (request.isDone == false)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    yield break;
                }

                yield return null;
            }

            callback?.Invoke(www.result == UnityWebRequest.Result.Success
                ? DownloadHandlerTexture.GetContent(www)
                : null);
        }
    }
}