using Scripts;
using UnityEngine;
using UnityEngine.Assertions;

namespace UI
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private TrashBin _trashBin;

        public bool IsCursorOverBin => _trashBin.IsCursorOver;

        protected override void OnAwake()
        {
            base.OnAwake();
            Assert.IsNotNull(_trashBin);
        }
    }
}