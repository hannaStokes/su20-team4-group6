using DIKUArcade.State;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.EventBus;
using System.IO;
using System;

namespace galaga.GalagaStates {
    public class MainMenu : IGameState {
        private static MainMenu instance = null;

        private Entity backGroundImage;

        private Text[] menuButtons;

        private int activeMenuButton;

        private int maxMenuButtons;

        public void GameLoop() {

        }

        public void InitializeGameState() {
            backGroundImage = new Entity(new StationaryShape(0, 0, 500, 500),
            new Image(Path.Combine("Assets", "Images", "TitleImage.png")));

            Text newGameButton = new Text("New Game",
                new Vec2F(0.4f, 0.5f), new Vec2F(0.3f, 0.3f));

            Text quitButton = new Text("Quit",
                new Vec2F(0.4f, 0.2f), new Vec2F(0.3f, 0.3f));

            menuButtons = new Text[] {newGameButton, quitButton};

            activeMenuButton = 0;

            maxMenuButtons = menuButtons.Length;
        }

        public void UpdateGameLogic() {

        }

        public void RenderState() {
            backGroundImage.RenderEntity();

            for (int i = 0; i < maxMenuButtons; i++)
            {
                Text button = menuButtons[i];

                if (i == activeMenuButton) {
                    button.SetColor(new Vec3I(0, 255, 0));
                } else {
                    button.SetColor(new Vec3I(0, 0, 255));
                }

                button.RenderText();
            }
        }

        public void HandleKeyEvent(string keyValue, string keyAction) {
            switch (keyAction) {
                case "KEY_PRESS":
                    switch (keyValue) {
                        case "KEY_UP":
                            activeMenuButton = Math.Max(0, activeMenuButton - 1);
                            break;
                        case "KEY_DOWN":
                            activeMenuButton = Math.Min(maxMenuButtons, activeMenuButton + 1);
                            break;
                        case "KEY_ENTER":
                            switch (activeMenuButton) {
                                case 0:
                                    GalagaBus.GetBus().RegisterEvent(
                                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                                            GameEventType.GameStateEvent,
                                            this,
                                            "CHANGE_STATE",
                                            "GAME_RUNNING", ""));
                                    
                                    break;
                                case 1:
                                    GalagaBus.GetBus().RegisterEvent(
                                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                                            GameEventType.WindowEvent,
                                            this,
                                            "CLOSE_WINDOW", "", ""));
                                    break;

                            }
                            break;
                    }
                    break;
            }
        }

        public static MainMenu GetInstance() {
            return MainMenu.instance ?? (MainMenu.instance = new MainMenu());
        }
    }
}