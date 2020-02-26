using System;
using DIKUArcade;
using DIKUArcade.Timers;
using DIKUArcade.EventBus;
namespace galaga
{
    public class Game : IGameEventProcessor<object> {
        private Window win;
        private GameTimer gameTimer;
        public Game() {
        // TODO: Choose some reasonable values for the window and timer constructor. 
        // For the window, we recommend a 500x500 resolution (a 1:1 aspect ratio). win = new Window(... , ... , ...);
        gameTimer = new  DIKUArcade.Timers.GameTimer(60, 60);
        win = new Window("Galaga" , 500 , 500);
        }
        public void GameLoop() {
            while(win.IsRunning()) {
                gameTimer.MeasureTime();
                while (gameTimer.ShouldUpdate()) {
                    win.PollEvents();
                    // Update game logic here
                }
                if (gameTimer.ShouldRender()) {
                    win.Clear();
                    // Render gameplay entities here
                    win.SwapBuffers();
                }
 
                if (gameTimer.ShouldReset()) {
                    // 1 second has passed - display last captured ups and fps 
                    win.Title = "Galaga | UPS: " + gameTimer.CapturedUpdates +
                    ", FPS: " + gameTimer.CapturedFrames; 
                }
            }
        }
    public void KeyPress(string key) {
        throw new NotImplementedException();
    }
    public void KeyRelease(string key) {
        throw new NotImplementedException();
    }
    public void ProcessEvent(GameEventType eventType,
        GameEvent<object> gameEvent) {
        throw new NotImplementedException();
        }
    }
}


