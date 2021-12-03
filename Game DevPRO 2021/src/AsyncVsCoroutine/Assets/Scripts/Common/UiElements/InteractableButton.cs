using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Common.UiElements
{
    public class InteractableButton : InteractableElement
    {
        [SerializeField] private Button _button;
        
        public event UnityAction Click
        {
            add => _button.onClick.AddListener(value);
            remove => _button.onClick.RemoveListener(value);
        }
        
        protected override void OnSetInteractable(bool value)
        {
            _button.interactable = value;
        }
    }
}
