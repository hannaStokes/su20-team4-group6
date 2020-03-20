using DIKUArcade.State;
namespace galaga.GalagaStates {
    public class MainMenu : IGameState {
        private static MainMenu instance = null;
        private Entity backGroundImage = new Entity (new DynamicShape(new Vec2F(0.0f, 0.0f), new Vec2F(0.0f, 0.0f)),new Image(Path.Combine("Assets", "Images", "TitleImage.png")));
        private Text[] menuButtons = {new Text("New Game", new Vec2F(0.5f, 0.7f), new Vec2F(0.1f, 0.1f)), new Text("Quit", new Vec2F(0.5f, 0.5f), new Vec2F(0.1f, 0.1f))};
        private int activeMenuButton = 0;
        private int maxMenuButtons = 2;
        public static MainMenu GetInstance() {
            return MainMenu.instance ?? (MainMenu.instance = new MainMenu());
        }
        public void RenderState()
        {
            backGroundImage.RenderEntity();
            foreach (Text menubottom in menuButtons)
            {
                menubottom.RenderText();
            }
        }
    }
}