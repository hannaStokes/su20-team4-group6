using DIKUArcade.Entities;
using DIKUArcade.Math;

namespace galaga.MovementStrategy {
    public class Down {
        public void MoveEnemy(Enemy enemy) {
            DynamicShape dynamicShape = enemy.Shape.AsDynamicShape();

            dynamicShape.ChangeDirection(new Vec2F(0.0f, -0.001f));

            if ((dynamicShape.Position.Y + dynamicShape.Direction.Y) > 0.0f) {
                dynamicShape.Move();
            } else {
                enemy.DeleteEntity();
            }
        }

        public void MoveEnemies(EntityContainer<Enemy> enemies) {
            foreach (Enemy enemy in enemies) {
                MoveEnemy(enemy);
            }
        }

        public Down() {
        }
    }
}