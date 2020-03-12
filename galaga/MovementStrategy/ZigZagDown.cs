using DIKUArcade.Entities;
using DIKUArcade.Math;
using System;

namespace galaga.MovementStrategy {
    public class ZigZagDown {
        private static float s = 0.0003f;
        private static float p = 0.045f;
        private static float a = 0.05f;

        public void MoveEnemy(Enemy enemy) {
            DynamicShape dynamicShape = enemy.Shape.AsDynamicShape();

            float y = dynamicShape.Position.Y - s;
            float x = (float) (enemy.startPositionX + a * Math.Sin(
                (2 * Math.PI * (enemy.startPositionY - y)) / p));

            if (y > 0.0f) {
                dynamicShape.SetPosition(new Vec2F(x, y));
            } else {
                enemy.DeleteEntity();
            }
        }

        public void MoveEnemies(EntityContainer<Enemy> enemies) {
            foreach (Enemy enemy in enemies) {
                MoveEnemy(enemy);
            }
        }

        public ZigZagDown() {
        }
    }
}