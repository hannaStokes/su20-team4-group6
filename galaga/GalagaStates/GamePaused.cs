using DIKUArcade.State;
namespace galaga.GalagaStates {
    public class GamePaused : IGameState {
        private static GamePaused instance = null;
        private Text Paused = new Text("Game Paused", new Vec2F(0.3f, 0.5f), new Vec2F(0.4f, 0.07f));
        private Text Continue = new Text("(To continue the game press R)", new Vec2F(0.3f, 0.35f), new Vec2F(0.4f, 0.04f));
        private Text MainMenu = new Text("(To exit to main menu press M)", new Vec2F(0.3f, 0.30f), new Vec2F(0.4f, 0.04f));
        public static GamePaused GetInstance() {
            return GamePaused.instance ?? (GamePaused.instance = new GamePaused());
        }
        public void RenderState()
        {
            Paused.SetColor(30,144,255);
            Paused.RenderText();
            Continue.SetColor(30,144,255);
            Continue.RenderText();
        }

    }
}