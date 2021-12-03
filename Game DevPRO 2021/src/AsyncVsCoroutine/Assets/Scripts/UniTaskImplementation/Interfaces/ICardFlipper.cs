using Common.Enums;
using Common.Interfaces;
using Cysharp.Threading.Tasks;

namespace UniTaskImplementation.Interfaces
{
    public interface ICardFlipper
    {
        UniTask FlipCardAsync(ICard card, CardSide cardSide);
    }
}