using DIKUArcade.EventBus;
using DIKUArcade.State;
using System;

namespace galaga.GalagaStates {
    public class StateMachine : IGameEventProcessor<object> {
        public IGameState ActiveState { get; private set; }

        private GameStateType previousState;

        public StateMachine() {
            GalagaBus.GetBus().Subscribe(GameEventType.GameStateEvent, this);
            GalagaBus.GetBus().Subscribe(GameEventType.InputEvent, this);

            ActiveState = MainMenu.GetInstance();
            ActiveState.InitializeGameState();
            previousState = GameStateType.MainMenu;
        }

        private void SwitchState(GameStateType stateType) {
            switch (stateType) {
                case GameStateType.GamePaused:
                    ActiveState = GamePaused.GetInstance();
                    ActiveState.InitializeGameState();
                    break;
                case GameStateType.MainMenu:
                    ActiveState = MainMenu.GetInstance();
                    ActiveState.InitializeGameState();
                    break;
                case GameStateType.GameRunning:
                    ActiveState = GameRunning.GetInstance();
                    if (previousState == GameStateType.MainMenu) {
                        ActiveState.InitializeGameState();
                    }
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