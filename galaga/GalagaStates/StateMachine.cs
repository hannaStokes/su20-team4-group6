using DIKUArcade.EventBus;
using DIKUArcade.State;

namespace galaga.GalagaStates {
    public class StateMachine : IGameEventProcessor<object> {
        public IGameState ActiveState { get; private set; }
        public StateMachine() {
            GalagaBus.GetBus().Subscribe(GameEventType.GameStateEvent, this);
            GalagaBus.GetBus().Subscribe(GameEventType.InputEvent, this);
            ActiveState = GameStateType.MainMenu.GetInstance();
        }

        void HandleKeyEvent(string keyValue, string keyAction) {
            
        }
        private void SwitchState(GameStateType stateType) {
            ActiveState = GameEventType.GameStateEvent;
            switch (stateType) {
                case GameStateType.MainMenu :

                    IGameState.HandleKeyEvent("KEY_M", "MAIN_MENU");
                    event = GameEventFactory<object>.CreateGameEventForAllProcessors(GameEventType.GameStateEvent, this, "MAIN_MENU", "", "");
                    GalagaBus.GetBus().RegisterEvent(event);
                    TransformStringToState(event.Message);
                    break;
                case GameStateType.GameRunning :
                    event = GameEventFactory<object>.CreateGameEventForAllProcessors(GameEventType.GameStateEvent, this, "GAME_RUNNING", "", "");
                    GalagaBus.GetBus().RegisterEvent(event);
                    TransformStringToState(event.Message);
                    break;
                case GameStateType.GamePaused :
                    event = GameEventFactory<object>.CreateGameEventForAllProcessors(GameEventType.GameStateEvent, this, "GAME_PAUSED", "", "");
                    GalagaBus.GetBus().RegisterEvent(event);
                    TransformStringToState(event.Message);
                    break;
            }
        }
    }
}