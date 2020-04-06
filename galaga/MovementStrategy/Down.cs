using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Math;
using galaga.GalagaStates;

namespace galaga.MovementStrategy {
    public class Down : IMovementStrategy {
        private float speedMultiplier;
        GameRunning game;

        public void MoveEnemy(Enemy enemy) {
            DynamicShape dynamicShape = enemy.Shape.AsDynamicShape();

            dynamicShape.ChangeDirection(new Vec2F(0.0f, -0.001f * speedMultiplier));

            if ((dynamicShape.Position.Y + dynamicShape.Direction.Y) > 0.0f) {
                dynamicShape.Move();
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

        public Down(GameRunning game, float speedMultiplier) {
            this.speedMultiplier = speedMultiplier;
            this.game = game;
        }
    }
}