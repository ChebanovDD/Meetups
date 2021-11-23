using Enums;
using UnityEngine;

namespace Interfaces
{
    public interface ICard
    {
        Transform Transform { get; }
        Vector3 DefaultScale { get; }
        CardSide CurrentCardSide { get; }

        void SetArt(Texture2D texture);
        void SetCardSide(CardSide cardSide);
    }
}