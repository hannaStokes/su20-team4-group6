using DIKUArcade;


namespace galaga {
    public enum GameStateType {
            
        GameRunning,

        GamePaused,

        MainMenu
    }
    
    public class StateTransformer {

        public static GameStateType TransformStringToState(string state) {
            GameStateType gameState = GameStateType.GameRunning;
            switch (state) {
                case "GAME_RUNNING" :
                    break;
                case "GAME_PAUSED" :
                    gameState = GameStateType.GamePaused;
                    break;
                case "GAME_MAIN":
                    gameState = GameStateType.MainMenu;
                    break;
                default :
                    break;
                    
            }
            return gameState;
        }

        public static string TransformStateToString(GameStateType state) {
            string gameState = "GAME_RUNNING";
            switch (state) {
                case GameStateType.GameRunning :
                    break;
                case GameStateType.GamePaused :
                    gameState = "GAME_PAUSED";
                    break;
                case GameStateType.MainMenu :
                    gameState = "GAME_MAIN";
                    break;
                default :
                    break;
                    
            }
            return gameState;
        }
    }
}
