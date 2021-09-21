using System;
using Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace UI
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private TrashBin _trashBin;
        [SerializeField] private TextMeshProUGUI _timeText;
        [SerializeField] private TextMeshProUGUI _accidentsText;
        [SerializeField] private TextMeshProUGUI _accidentsInLastMinText;

        public bool IsCursorOverBin => _trashBin.IsCursorOver;

        protected override void OnAwake()
        {
            base.OnAwake();
            Assert.IsNotNull(_trashBin);
            Assert.IsNotNull(_timeText);
            Assert.IsNotNull(_accidentsText);
            Assert.IsNotNull(_accidentsInLastMinText);
        }

        public void SetAccidents(string value)
        {
            _accidentsText.text = value;
        }

        public void SetTime(string value)
        {
            _timeText.text = value;
        }

        public void SetAccidentsInLastMin(string value)
        {
            _accidentsInLastMinText.text = value;
        }
    }
}