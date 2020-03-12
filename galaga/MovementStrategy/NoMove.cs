using DIKUArcade.Entities;

namespace galaga.MovementStrategy {
    public class NoMove : IMovementStrategy {
        public void MoveEnemy(Enemy enemy) {
        }

        public void MoveEnemies(EntityContainer<Enemy> enemies) {
        }

        public NoMove() {
        }
    }
}