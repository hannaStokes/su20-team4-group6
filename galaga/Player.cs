using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.EventBus;

public class Player : IGameEventProcessor<object> {
    public Entity Entity {get; private set;}
    public Player(DynamicShape shape, IBaseImage image) {
        Entity = new Entity(shape, image);
    } 
}