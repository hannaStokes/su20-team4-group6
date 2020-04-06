using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Math;
using System;
using galaga.GalagaStates;

namespace galaga.MovementStrategy {
    public class ZigZagDown : IMovementStrategy {
        private static float s = 0.0003f;
        private static float p = 0.045f;
        private static float a = 0.05f;

        private float speedMultiplier;

        GameRunning game;

        public void MoveEnemy(Enemy enemy) {
            DynamicShape dynamicShape = enemy.Shape.AsDynamicShape();

            float y = dynamicShape.Position.Y - (s * speedMultiplier);
            float x = (float) (enemy.startPositionX + a * Math.Sin(
                (2 * Math.PI * (enemy.startPositionY - y)) / p));

            if (y > 0.0f) {
                dynamicShape.SetPosition(new Vec2F(x, y));
            } else {
                GalagaBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.GameStateEvent, this, "GAME_OVER", "", ""));
            }
        }

        public void MoveEnemies(EntityContainer<Enemy> enemies) {
            foreach (Enemy enemy in enemies) {
                MoveEnemy(enemy);
            }
        }

        public ZigZagDown(GameRunning game, float speedMultiplier) {
            this.speedMultiplier = speedMultiplier;
            this.game = game;
        }
    }
}