using UnityEngine;
using UnityEngine.UI;

namespace UiElements
{
    public class InteractableButton : InteractableElement
    {
        [SerializeField] private Button _button;
        
        public Button.ButtonClickedEvent Click => _button.onClick;
        
        protected override void OnSetInteractable(bool value)
        {
            _button.interactable = value;
        }
    }
}
