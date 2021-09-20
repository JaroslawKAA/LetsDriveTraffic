using Scripts;

namespace System
{
    public class GameEvents : Singleton<GameEvents>
    {
        public event Action OnGameModeChange;
        public event Action OnPlacingSignStart;
        public event Action OnPlacingSignEnd;
        public event Action OnDraggingSignStart;
        public event Action OnDraggingSignEnd;
        
        public void OnGameModeChange_Invoke()
        {
            OnGameModeChange?.Invoke();
        }
        public void OnPlacingSignStart_Invoke()
        {
            OnPlacingSignStart?.Invoke();
        }
        public void OnPlacingSignEnd_Invoke()
        {
            OnPlacingSignEnd?.Invoke();
        }
        
        public void OnDraggingSignStart_Invoke()
        {
            OnDraggingSignStart?.Invoke();
        }
        public void OnDraggingSignEnd_Invoke()
        {
            OnDraggingSignEnd?.Invoke();
        }
    }
}