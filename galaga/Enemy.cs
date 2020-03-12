using DIKUArcade.Graphics;
using DIKUArcade.Entities;

public class Enemy : Entity {
    public float startPositionX {get; }
    public float startPositionY {get; }

    public Enemy(DynamicShape shape, IBaseImage image): base(shape, image) {
        startPositionX = shape.Position.X;
        startPositionY = shape.Position.Y;
    } 
}