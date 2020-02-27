using System;
using DIKUArcade;
using DIKUArcade.Timers;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Entities;

namespace galaga {

    public class Player : IGameEventProcessor<object> {
        public Entity Entity {get; private set;}
        public Player(DynamicShape shape, IBaseImage image) {
            Entity = new Entity(shape, image);
        }

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent)
        {
            throw new NotImplementedException();
        }
        public void Direction(Vec2F vector) {
            var dShape = Entity.Shape.AsDynamicShape();
            dShape.ChangeDirection(vector);
        }
        public void Move() {
            var dShape = Entity.Shape.AsDynamicShape();
            var pos = dShape.Position + dShape.Direction;
            if ( pos.X > 0.0 && pos.X < 0.9 ) {
                dShape.Move();
            }
        }
    }
}