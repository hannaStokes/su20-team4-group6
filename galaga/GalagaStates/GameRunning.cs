using System.IO;
using System.Collections.Generic;
using System;
using DIKUArcade.State;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using galaga.MovementStrategy;
using galaga.Squadron;

namespace galaga.GalagaStates {
    public class GameRunning : IGameState {
        private static GameRunning instance = null;
        private static int explosionLength = 500;
        private static float speedUp = 0.05f;

        private Entity backGroundImage;

        private Text[] menuButtons;
        private List<Image> explosionStrides;
        private List<Image> enemyStrides;
        private EntityContainer<Enemy> enemies;
        public List<PlayerShot> playerShots;

        private AnimationContainer explosions;
        private Player player;
        private Score score;
        private Random random;

        private int activeMenuButton;
        private int maxMenuButtons;
        private int movementSeed;

        private float speedMultiplier;

        private bool gameOver;

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

        public void AddExplosion(float posX, float posY, float extentX, float extentY) {
            explosions.AddAnimation(
                new StationaryShape(posX, posY, extentX, extentY), explosionLength,
                new ImageStride(explosionLength / 8, explosionStrides));
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

        public void UpdateGameLogic() {
            gameOver = true;
        }

        public void GameLoop() {
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

        public void InitializeGameState() {
            player = new Player(
                new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)), 
                new Image(Path.Combine("Assets", "Images", "Player.png")), this);

            GalagaBus.GetBus().Subscribe(GameEventType.InputEvent, player);

            enemyStrides = ImageStride.CreateStrides(4, Path.Combine("Assets", "Images", "BlueMonster.png"));
            enemies = new EntityContainer<Enemy>();

            explosionStrides = ImageStride.CreateStrides(8, Path.Combine("Assets", "Images", "Explosion.png"));
            explosions = new AnimationContainer(100);

            playerShots = new List<PlayerShot>();

            score = new Score(new Vec2F(0.8f, 0.8f), new Vec2F(0.2f, 0.2f));

            speedMultiplier = 1;

            random = new Random();
        }

        public void RenderState() {
            if (!gameOver) {
                player.Entity.RenderEntity();
                
                enemies.RenderEntities();

                foreach (PlayerShot shot in playerShots) {
                    shot.RenderEntity();
                }

                explosions.RenderAnimations();
            }

            score.RenderScore();
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
                                    "GAME_PAUSED", ""));
                            break;
                    }
                    break;
            }
        }
        
        public static GameRunning GetInstance() {
            return GameRunning.instance ?? (GameRunning.instance = new GameRunning());
        }
    }
}