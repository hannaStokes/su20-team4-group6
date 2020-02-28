using System;
using System.IO;
using DIKUArcade;
using DIKUArcade.Timers;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Entities;
using System.Collections.Generic;

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
            if ( (dShape.Position.X + dShape.Direction.X) > 0.0f && (dShape.Position.X + dShape.Direction.X) < 0.9f ) {
                dShape.Move();
            }
        }
        public void AddShots(Game game) {
            IBaseImage bulletPicture = new Image(Path.Combine("Assets", "Images", "BulletRed2.png"));
            PlayerShot pShot = new PlayerShot(new DynamicShape(Entity.Shape.Position , new Vec2F(0.008f,0.027f), new Vec2F(0.0f, 0.01f)), bulletPicture);
            game.playerShots.Add(pShot);
        }
    }
 }
