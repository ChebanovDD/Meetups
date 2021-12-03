using Common.Interfaces;
using Common.UiElements;
using UnityEngine;

namespace Common
{
    public class GameCanvas : MonoBehaviour
    {
        [SerializeField] private Transform _cardsContainer;
        [SerializeField] private InteractableDropdown _dropdown;
        [SerializeField] private InteractableButton _loadButton;
        [SerializeField] private InteractableButton _cancelButton;

        public InteractableDropdown Dropdown => _dropdown;
        public InteractableButton LoadButton => _loadButton;
        public InteractableButton CancelButton => _cancelButton;
        
        public ICard[] GetCards()
        {
            return _cardsContainer.GetComponentsInChildren<ICard>();
        }
    }
}
