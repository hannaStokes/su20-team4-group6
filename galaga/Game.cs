using System.IO;
using DIKUArcade;
using DIKUArcade.Timers;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Entities;
using System.Collections.Generic;
using DIKUArcade.Physics;
using galaga.Squadron;
using galaga.MovementStrategy;
using System;

namespace galaga {
    public class Game : IGameEventProcessor<object> {
        private Window win;
        public GameEventBus<object> eventBus;
        public StateMachine stateMachine;
        private GameTimer gameTimer;
        public Game() {
            eventBus = GalagaBus.GetBus();
            stateMachine = new StateMachine();
            eventBus.InitializeEventBus(new List<GameEventType>() {
                GameEventType.InputEvent, // key press / key release
                GameEventType.WindowEvent, // messages to the window 
                GameEventType.GameStateEvent // changes to the game state
            });
            win = new Window("Galaga", 500, 500);

            win.RegisterEventBus(eventBus);
            eventBus.Subscribe(GameEventType.InputEvent, player);
            eventBus.Subscribe(GameEventType.WindowEvent, this);
            eventBus.Subscribe(GameEventType.GameStateEvent, this);

            gameTimer = new GameTimer(60, 60);
        }

        public void GameLoop() {
            while(win.IsRunning()) {
                gameTimer.MeasureTime();

                while (gameTimer.ShouldUpdate()) {
                    win.PollEvents();
                    eventBus.ProcessEvents();
                    stateMachine.ActiveState.UpdateGameLogic();
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
                    gameOver = true;
                    break;
                default:
                    break;
            }
        }
    }
}