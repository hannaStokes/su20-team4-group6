using DIKUArcade;

namespace galaga.GalagaStates {
    public enum GameStateType {
        GameRunning,
        GamePaused,
        MainMenu
    }

    public class StateTransformer {
        public static GameStateType TransformStringToState(string state) {
            switch (state) {
                case "GAME_RUNNING":
                    return GameStateType.GameRunning;
                case "GAME_PAUSED":
                    return GameStateType.GamePaused;
                case "GAME_MAIN":
                    return GameStateType.MainMenu;
                default:
                    throw new System.ArgumentException("Invalid input");
            }
        }

        public static string TransformStateToString(GameStateType state) {
            switch (state) {
                case GameStateType.GameRunning:
                    return "GAME_RUNNING";
                case GameStateType.GamePaused:
                    return "GAME_PAUSED";
                case GameStateType.MainMenu:
                    return "GAME_MAIN";
                default :
                    throw new System.ArgumentException("Invalid input");
            }
        }
    }
}