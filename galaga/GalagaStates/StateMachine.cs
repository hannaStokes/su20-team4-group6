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

        public void HandleKeyEvent(string keyVal, string keyAct) {
            ActiveState.activeMenuButton = 
            switch(keyAct) {
                case "KEY_UP":
                    if (ActiveState.activeMenuButton == 0){ActiveState.activeMenuButton = 1;}
                    else (ActiveState.activeMenuButton = 0);
                    break;
                case "KEY_DOWN":
                    if (ActiveState.activeMenuButton == 0){ActiveState.activeMenuButton = 1;}
                    else (ActiveState.activeMenuButton = 0);
                    break;
                case "KEY_ENTER":
                    if (ActiveState.activeMenuButton == 0){GalagaBus.GetBus().RegisterEvent(
                                GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.GameStateEvent,
                                this,
                                "CHANGE_STATE",
                                "GAME_RUNNING", ""));}
                    else { GalagaBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.WindowEvent,
                            this, 
                            "CLOSE_WINDOW",
                            "",
                            ""));}
                    break;
            }
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
                    KeyPress("KEY_M");
                    break;
                case GameStateType.GameRunning :
                    ActiveState = GameRunning.GetInstance();
                    KeyPress("KEY_R");
                    break;
                case GameStateType.GamePaused :
                    ActiveState = GamePaused.GetInstance();
                    KeyPress("KEY_P");
                    break;
            }
        }
    }
}