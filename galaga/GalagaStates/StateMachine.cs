using DIKUArcade.EventBus;
using DIKUArcade.State;
using System;

namespace galaga.GalagaStates {
    public class StateMachine : IGameEventProcessor<object> {
        public IGameState ActiveState { get; private set; }

        private GamePaused gamePaused;
        private MainMenu mainMenu;
        private GameRunning gameRunning;

        private GameStateType previousState;

        public StateMachine() {
            GalagaBus.GetBus().Subscribe(GameEventType.GameStateEvent, this);
            GalagaBus.GetBus().Subscribe(GameEventType.InputEvent, this);

            gameRunning = GameRunning.GetInstance();
            gameRunning.InitializeGameState();
            gamePaused = GamePaused.GetInstance();
            gamePaused.InitializeGameState();
            mainMenu = MainMenu.GetInstance();
            mainMenu.InitializeGameState();

            ActiveState = mainMenu;
        }

        private void SwitchState(GameStateType stateType) {
            switch (stateType) {
                case GameStateType.GamePaused:
                    ActiveState = gamePaused;
                    break;
                case GameStateType.MainMenu:
                    ActiveState = mainMenu;
                    break;
                case GameStateType.GameRunning:
                    ActiveState = gameRunning;
                    break;
            }
        }

        private void KeyPress(string key) {
        }

        private void KeyRelease(string key) {
        }

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            switch(gameEvent.Message) {
                case "CHANGE_STATE":
                    SwitchState(StateTransformer.TransformStringToState(gameEvent.Parameter1));
                    break;
                default:
                    switch (gameEvent.Parameter1) {
                        case "KEY_PRESS":
                            ActiveState.HandleKeyEvent(gameEvent.Message, gameEvent.Parameter1);
                            break;
                    }
                    break;
            }
        }
    }
}