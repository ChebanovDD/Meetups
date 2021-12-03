using Common.Enums;
using UnityEngine;

namespace Common.Interfaces
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