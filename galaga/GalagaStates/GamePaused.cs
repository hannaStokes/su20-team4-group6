using System.IO;
using DIKUArcade.State;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.EventBus;
using DIKUArcade.Math;

namespace galaga.GalagaStates {
    public class GamePaused : IGameState {
        private static GamePaused instance = null;

        private Entity backGroundImage;

        private Text[] menuButtons;

        private int activeMenuButton;

        private int maxMenuButtons;

        public void GameLoop() {

        }

        public void InitializeGameState() {
            backGroundImage = new Entity(new StationaryShape(0, 0, 500, 500),
                new Image(Path.Combine("Assets", "Images", "TitleImage.png")));

            Text paused = new Text("PAUSED",
                new Vec2F(0.5f, 0.5f), new Vec2F(0.5f, 0.5f));

            menuButtons = new Text[] {paused};
        }

        public void UpdateGameLogic() {

        }

        public void RenderState() {
            backGroundImage.RenderEntity();

            foreach (Text button in menuButtons) {
                button.RenderText();
            }
        }

        public void HandleKeyEvent(string keyValue, string keyAction) {
            switch (keyAction) {
                case "KEY_PRESS":
                    switch (keyValue) {
                        case "KEY_P":
                            GalagaBus.GetBus().RegisterEvent(
                                GameEventFactory<object>.CreateGameEventForAllProcessors(
                                    GameEventType.GameStateEvent,
                                    this,
                                    "CHANGE_STATE",
                                    "GAME_RUNNING", ""));
                            break;
                    }
                    break;
            }
        }
        
        public static GamePaused GetInstance() {
            return GamePaused.instance ?? (GamePaused.instance = new GamePaused());
        }
    }
}