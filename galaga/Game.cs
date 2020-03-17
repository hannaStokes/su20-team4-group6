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
        private static int explosionLength = 500;
        private static float speedUp = 0.05f;

        private Window win;
        private GameTimer gameTimer;
        private List<Image> enemyStrides;
        private List<Image> explosionStrides;
        private AnimationContainer explosions;
        private EntityContainer<Enemy> enemies;
        private Player player;
        private Score score;
        private Random random;
        private int movementSeed;
        private float speedMultiplier;
        private bool gameOver;

        public List<PlayerShot> playerShots;
        public GameEventBus<object> eventBus;

        public Game() {
            eventBus = GalagaBus.GetBus();
            eventBus.InitializeEventBus(new List<GameEventType>() {
                GameEventType.InputEvent, // key press / key release
                GameEventType.WindowEvent, // messages to the window 
                GameEventType.GameStateEvent // changes to the game state
            });
            win = new Window("Galaga", 500, 500);

            player = new Player(
                new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)), 
                new Image(Path.Combine("Assets", "Images", "Player.png")), this);
            playerShots = new List<PlayerShot>();

            win.RegisterEventBus(eventBus);
            eventBus.Subscribe(GameEventType.InputEvent, player);
            eventBus.Subscribe(GameEventType.WindowEvent, this);
            eventBus.Subscribe(GameEventType.GameStateEvent, this);

            gameTimer = new GameTimer(60, 60);

            enemyStrides = ImageStride.CreateStrides(4, Path.Combine("Assets", "Images", "BlueMonster.png"));
            enemies = new EntityContainer<Enemy>();

            explosionStrides = ImageStride.CreateStrides(8, Path.Combine("Assets", "Images", "Explosion.png"));
            explosions = new AnimationContainer(100);

            score = new Score(new Vec2F(0.8f, 0.8f), new Vec2F(0.2f, 0.2f));

            speedMultiplier = 1;

            random = new Random();
        }

        public void GameLoop() {
            while(win.IsRunning()) {
                gameTimer.MeasureTime();

                while (gameTimer.ShouldUpdate()) {
                    win.PollEvents();
                    eventBus.ProcessEvents();

                    if (!gameOver) {
                        player.Move();

                        EnemyMove();

                        if (CheckEnemiesDefeated()) {
                            AddSquadron();
                        }

                        UpdateEnemyList();
                        UpdateShotsList();
                        IterateShots();
                    }
                }

                if (gameTimer.ShouldRender()) {
                    win.Clear();

                    if (!gameOver) {
                        player.Entity.RenderEntity();
                        
                        enemies.RenderEntities();

                        foreach (PlayerShot shot in playerShots) {
                            shot.RenderEntity();
                        }

                        explosions.RenderAnimations();
                    }

                    score.RenderScore();
                    win.SwapBuffers();
                }
 
                if (gameTimer.ShouldReset()) {
                    // 1 second has passed - display last captured ups and fps 
                    win.Title = "Galaga | UPS: " + gameTimer.CapturedUpdates +
                    ", FPS: " + gameTimer.CapturedFrames; 
                }
            }
        }

        public void UpdateEnemyList(){
            EntityContainer<Enemy> newEnemies = new EntityContainer<Enemy>();

            foreach (Enemy enemy in enemies) {
                if (!enemy.IsDeleted()) { 
                    newEnemies.AddDynamicEntity(enemy);
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

        public void AddExplosion(float posX, float posY, float extentX, float extentY) {
            explosions.AddAnimation(
                new StationaryShape(posX, posY, extentX, extentY), explosionLength,
                new ImageStride(explosionLength / 8, explosionStrides));
        }
        
        public void IterateShots() {
            foreach (PlayerShot shot in playerShots) {
                shot.Shape.Move();

                if (shot.Shape.Position.Y > 1.0f) {
                    shot.DeleteEntity();
                } else {
                    foreach (Enemy enemy in enemies) {
                        if ((CollisionDetection.Aabb(shot.Shape.AsDynamicShape(),enemy.Shape)).Collision) 
                        {
                            shot.DeleteEntity();
                            enemy.DeleteEntity();
                            AddExplosion(enemy.Shape.Position.X,enemy.Shape.Position.Y,enemy.Shape.Extent.X,enemy.Shape.Extent.Y);
                            score.AddPoint();
                        }
                    }
                }
            } 
        }

        private void EnemyMove() {
            switch (movementSeed) {
                case 1:
                    ZigZagDown zigZagDown = new ZigZagDown(this, speedMultiplier);
                    zigZagDown.MoveEnemies(enemies);
                    break;
                case 2:
                    Down down = new Down(this, speedMultiplier);
                    down.MoveEnemies(enemies);
                    break;
                case 3:
                    break;
            }
        }

        private bool CheckEnemiesDefeated() {
            return enemies.CountEntities() == 0;
        }

        private void AddSquadron() {
            movementSeed = random.Next(1, 4);

            speedMultiplier += speedUp;

            switch (movementSeed) {
                case 1:
                    DiamondSquadron diamondSquadron = new DiamondSquadron();
                    diamondSquadron.CreateEnemies(enemyStrides);
                    enemies = diamondSquadron.Enemies;
                    break;
                case 2:
                    LineSquadron lineSquadron = new LineSquadron();
                    lineSquadron.CreateEnemies(enemyStrides);
                    enemies = lineSquadron.Enemies;
                    break;
                case 3:
                    SquareSquadron squareSquadron = new SquareSquadron();
                    squareSquadron.CreateEnemies(enemyStrides);
                    enemies = squareSquadron.Enemies;
                    break;
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