using DIKUArcade.EventBus;
using DIKUArcade.State;

namespace galaga.GalagaStates {
    public class StateMachine : IGameEventProcessor<object> {
        public IGameState ActiveState { get; private set; }
        public StateMachine() {
            GalagaBus.GetBus().Subscribe(GameEventType.GameStateEvent, this);
            GalagaBus.GetBus().Subscribe(GameEventType.InputEvent, this);
            ActiveState = MainMenu.GetInstance();
        }

        private void KeyPress(string key) {
            switch(key) {
                case "KEY_M":
                    event = GameEventFactory<object>.CreateGameEventForAllProcessors(GameEventType.GameStateEvent, this, "MAIN_MENU", "", "");
                    GalagaBus.GetBus().RegisterEvent(event);
                    break;
                case "KEY_R":
                    event = GameEventFactory<object>.CreateGameEventForAllProcessors(GameEventType.GameStateEvent, this, "GAME_RUNNING", "", "");
                    GalagaBus.GetBus().RegisterEvent(event);
                    break;
                case "KEY_P":
                    event = GameEventFactory<object>.CreateGameEventForAllProcessors(GameEventType.GameStateEvent, this, "GAME_PAUSED", "", "");
                    GalagaBus.GetBus().RegisterEvent(event);
                    break;
            }
        }
        private void SwitchState(GameStateType stateType) {
            switch (stateType) {
                case GameStateType.MainMenu :
                    ActiveState = MainMenu.GetInstance();
                    KeyPress("KEY_M")
                    break;
                case GameStateType.GameRunning :
                    ActiveState = MainMenu.GetInstance();
                    KeyPress("KEY_R");
                    break;
                case GameStateType.GamePaused :
                    ActiveState = MainMenu.GetInstance();
                    KeyPress("KEY_P");
                    break;
            }
        }
    }
}