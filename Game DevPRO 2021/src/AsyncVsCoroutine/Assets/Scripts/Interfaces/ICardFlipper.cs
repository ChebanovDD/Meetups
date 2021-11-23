using Cysharp.Threading.Tasks;
using Enums;

namespace Interfaces
{
    public interface ICardFlipper
    {
        UniTask FlipCardAsync(ICard card, CardSide cardSide);
    }
}