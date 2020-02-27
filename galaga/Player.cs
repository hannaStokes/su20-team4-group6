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
    }
}