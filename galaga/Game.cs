using DIKUArcade;
using DIKUArcade.Timers;
using DIKUArcade.EventBus;
using DIKUArcade.Math;
using System.Collections.Generic;
using galaga.GalagaStates;

namespace galaga {
    public class Game : IGameEventProcessor<object> {
        private Window win;
        private GameTimer gameTimer;
        public StateMachine stateMachine;

        public Game() {
            GalagaBus.GetBus().InitializeEventBus(new List<GameEventType>() {
                GameEventType.InputEvent, // key press / key release
                GameEventType.WindowEvent, // messages to the window 
                GameEventType.GameStateEvent // changes to the game state
            });
            win = new Window("Galaga", 500, 500);

            win.RegisterEventBus(GalagaBus.GetBus());
            GalagaBus.GetBus().Subscribe(GameEventType.WindowEvent, this);
            GalagaBus.GetBus().Subscribe(GameEventType.GameStateEvent, this);

            gameTimer = new GameTimer(60, 60);           

            stateMachine = new StateMachine();
        }

        public void GameLoop() {
            while(win.IsRunning()) {
                gameTimer.MeasureTime();

                while (gameTimer.ShouldUpdate()) {
                    win.PollEvents();
                    GalagaBus.GetBus().ProcessEvents();
                    stateMachine.ActiveState.GameLoop();
                }

                if (gameTimer.ShouldRender()) {
                    win.Clear();
                    stateMachine.ActiveState.RenderState();
                    win.SwapBuffers();
                }
 
                if (gameTimer.ShouldReset()) {
                    // 1 second has passed - display last captured ups and fps 
                    win.Title = "Galaga | UPS: " + gameTimer.CapturedUpdates +
                    ", FPS: " + gameTimer.CapturedFrames; 
                }
            }
        }
        
        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            switch (gameEvent.Message) {
                case "CLOSE_WINDOW":
                    win.CloseWindow();
                    break;
                case "GAME_OVER":
                    stateMachine.ActiveState.UpdateGameLogic();
                    break;
                default:
                    break;
            }
        }
    }
}