using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UiElements
{
    public class InteractableDropdown : InteractableElement
    {
        [SerializeField] private TMP_Dropdown _dropdown;
        [SerializeField] private Image _arrow;

        private Color _defaultArrowColor;
        
        public int SelectedIndex => _dropdown.value;

        protected override void Awake()
        {
            base.Awake();
            _defaultArrowColor = _arrow.color;
        }

        public void AddItems(IEnumerable<string> items)
        {
            _dropdown.AddOptions(items.ToList());
        }
        
        protected override void OnSetInteractable(bool value)
        {
            _dropdown.interactable = value;
            _arrow.color = value ? _defaultArrowColor : _disabledColor;
        }
    }
}