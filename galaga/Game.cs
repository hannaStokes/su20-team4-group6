using System;
using System.IO;
using DIKUArcade;
using DIKUArcade.Timers;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Entities;
using System.Collections.Generic;
using DIKUArcade.Physics;

namespace galaga {
    public class Game : IGameEventProcessor<object> {

        private GameEventBus<object> eventBus;
        private Window win;
        private GameTimer gameTimer;
        private List<Image> enemyStrides;
        private List<Enemy> enemies;
        public List<PlayerShot> playerShots {get; private set;}
        private Player player;
        public Game() {
            eventBus = new GameEventBus<object>();
                eventBus.InitializeEventBus(new List<GameEventType>() {
                    GameEventType.InputEvent, // key press / key release
                    GameEventType.WindowEvent, // messages to the window 
                });
        win = new Window("Galaga" , 500 , 500);
        win.RegisterEventBus(eventBus);
        eventBus.Subscribe(GameEventType.InputEvent, this);
        eventBus.Subscribe(GameEventType.WindowEvent, this);
        gameTimer = new  GameTimer(60, 60);
        player = new Player(
            new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)), 
            new Image(Path.Combine("Assets", "Images", "Player.png")));
        enemyStrides = ImageStride.CreateStrides(4, Path.Combine("Assets", "Images", "BlueMonster.png"));
        enemies = new List<Enemy>();
        AddEnemies();
        }
        public void GameLoop() {
            while(win.IsRunning()) {
                gameTimer.MeasureTime();
                while (gameTimer.ShouldUpdate()) {
                    win.PollEvents();
                    eventBus.ProcessEvents();
                    player.Move();
                    UpdateEnemyList();
                    UpdateShotsList();
                    // Update game logic here
                }
                if (gameTimer.ShouldRender()) {
                    win.Clear();
                    player.Entity.RenderEntity();
                    foreach (Enemy element in enemies) {
                        element.RenderEntity();}
                    win.SwapBuffers();
                }
 
                if (gameTimer.ShouldReset()) {
                    // 1 second has passed - display last captured ups and fps 
                    win.Title = "Galaga | UPS: " + gameTimer.CapturedUpdates +
                    ", FPS: " + gameTimer.CapturedFrames; 
                }
            }
        }
        public void AddEnemies() {
            for (int i = 0; i < 4; i++) {
                enemies.Add(new Enemy(new DynamicShape(new Vec2F(((i+1.0f)/4.0f)-0.175f, 0.8f), new Vec2F(0.1f, 0.1f)), 
            new ImageStride(80, enemyStrides)));
            }
        }
        public void UpdateEnemyList(){
            List<Enemy> newEnemies = new List<Enemy>();
                foreach (Enemy enemy in enemies) {
                    if (!enemy.IsDeleted()) { 
                        newEnemies.Add(enemy);
                    }
                }
            enemies = newEnemies;
        }
        public void UpdateShotsList(){
            List<PlayerShot> newShots = new List<PlayerShot>();
                foreach (PlayerShot shot in playerShots) {
                    if (!shot.IsDeleted()) { 
                        newShots.Add(shot);
                    }
                }
            playerShots = newShots;
        }
        
        public void IterateShots() {
            foreach (var shot in playerShots) {
                shot.Shape.Move();
                if (shot.Shape.Position.Y > 1.0f) {
                    shot.DeleteEntity(); } 
                else {
                    foreach (var enemy in enemies) {
                        if ((CollisionDetection.Aabb(enemy.Shape.AsDynamicShape(),shot.Shape)).Collision) 
                        {
                            shot.DeleteEntity();
                            enemy.DeleteEntity();
                        }
                    }
                }
            } 
        }
        public void KeyPress(string key) {
            switch(key) {
                case "KEY_ESCAPE":
                    eventBus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.WindowEvent, this, "CLOSE_WINDOW", "", ""));
                    break;
                case "KEY_RIGHT":
                    player.Direction(new Vec2F((float)0.01, (float)0.0));
                    break;
                case "KEY_LEFT":
                    player.Direction(new Vec2F(-((float)0.01), (float)0.0));
                    break;
                case "KEY_SPACE":
                    player.AddShots(this);
                    break;
            }
        }
            
        public void KeyRelease(string key) {
            switch(key) {
                case "KEY_RIGHT":
                    player.Direction(new Vec2F((float)0.0, (float)0.0));;
                    break;
                case "KEY_LEFT":
                    player.Direction(new Vec2F((float)0.0, (float)0.0));;
                    break;
            }
        }

    public void ProcessEvent(GameEventType eventType,
        GameEvent<object> gameEvent) {
        if (eventType == GameEventType.WindowEvent) {
            switch (gameEvent.Message) {
                case "CLOSE_WINDOW":
                    win.CloseWindow();
                    break;
                default:
                    break;
            }
        } else if (eventType == GameEventType.InputEvent) {
            switch (gameEvent.Parameter1) {
                case "KEY_PRESS":
                    KeyPress(gameEvent.Message);
                    break;
                case "KEY_RELEASE":
                    KeyRelease(gameEvent.Message);
                    break;
                }
            }
        }
    }
}



