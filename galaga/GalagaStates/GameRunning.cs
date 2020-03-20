using DIKUArcade.State;
namespace galaga.GalagaStates {
    public class GameRunning : IGameState {
        private static GameRunning instance = null;
        public static GameRunning GetInstance() {
            return GameRunning.instance ?? (GameRunning.instance = new GameRunning());
        }
        private static int explosionLength = 500;
        private static float speedUp = 0.05f;
        private List<Image> enemyStrides = ImageStride.CreateStrides(4, Path.Combine("Assets", "Images", "BlueMonster.png"));
        private List<Image> explosionStrides = explosionStrides = ImageStride.CreateStrides(8, Path.Combine("Assets", "Images", "Explosion.png"));
        private AnimationContainer explosions = new AnimationContainer(100);
        private EntityContainer<Enemy> enemies = new EntityContainer<Enemy>();
        private Player player = new Player(
                new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)), 
                new Image(Path.Combine("Assets", "Images", "Player.png")), this);
        private Score score = new Score(new Vec2F(0.8f, 0.8f), new Vec2F(0.2f, 0.2f));
        private Random random = new Random();
        private int movementSeed;
        private float speedMultiplier = 1;
        private bool gameOver;
        public List<PlayerShot> playerShots = new List<PlayerShot>();
        public void UpdateGameLogic()
        {
            if (!GameOver)
            {
                player.Move();
                EnemyMove();
                if (CheckEnemiesDefeated()) 
                {
                    AddSquadron();
                }
                UpdateEnemyList();
                UpdateShotsList();
                IterateShots();
            }
        }
        public void RenderState()
        {
            if (!GameOver)
            {
                player.Entity.RenderEntity();
                enemies.RenderEntities();
                foreach (PlayerShot shot in playerShots) 
                {
                shot.RenderEntity();
                }
                explosions.RenderAnimations();
                
            }
            score.RenderScore();
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
    }
}