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

        Game Game;
        public Player(DynamicShape shape, IBaseImage image, Game game) {
            Entity = new Entity(shape, image);
            Game = game;
        }

        private void KeyPress(string key) {
            switch(key) {
                case "KEY_ESCAPE":
                    Game.eventBus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.WindowEvent, this, "CLOSE_WINDOW", "", ""));
                    break;
                case "KEY_RIGHT":
                    Direction(new Vec2F((float)0.01, (float)0.0));
                    break;
                case "KEY_LEFT":
                    Direction(new Vec2F(-((float)0.01), (float)0.0));
                    break;
                case "KEY_SPACE":
                    AddShots(Game);
                    break;
            }
        }
            
        private void KeyRelease(string key) {
            switch(key) {
                case "KEY_RIGHT":
                    Direction(new Vec2F((float)0.0, (float)0.0));;
                    break;
                case "KEY_LEFT":
                    Direction(new Vec2F((float)0.0, (float)0.0));;
                    break;
            }
        }

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            switch (gameEvent.Parameter1) {
                case "KEY_PRESS":
                    KeyPress(gameEvent.Message);
                    break;
                case "KEY_RELEASE":
                    KeyRelease(gameEvent.Message);
                    break;    
            }
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
            PlayerShot pShot = new PlayerShot(new DynamicShape(new Vec2F(Entity.Shape.Position.X + 0.045f,Entity.Shape.Position.Y + 0.1f), new Vec2F(0.008f,0.027f), new Vec2F(0.0f, 0.01f)), bulletPicture);
            game.playerShots.Add(pShot);
        }
    }
 }
