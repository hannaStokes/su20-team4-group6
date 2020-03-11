using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using System.Collections.Generic;

namespace galaga.Squadron {
    public class DiamondSquadron : ISquadron {
        public EntityContainer<Enemy> Enemies { get; }

        public int MaxEnemies { get; }

        public void CreateEnemies(List<Image> enemyStrides)
        {
            Vec2F sizeVec = new Vec2F(0.1f, 0.1f);
            ImageStride imageStride = new ImageStride(80, enemyStrides);

            Enemies.AddDynamicEntity(new Enemy(
                new DynamicShape(new Vec2F((0.5f) - 0.05f, 0.8f), sizeVec), 
                imageStride));

            Enemies.AddDynamicEntity(new Enemy(
                new DynamicShape(new Vec2F((0.45f) - 0.05f, 0.7f), sizeVec), 
                imageStride));

            Enemies.AddDynamicEntity(new Enemy(
                new DynamicShape(new Vec2F((0.55f) - 0.05f, 0.7f), sizeVec), 
                imageStride));

            Enemies.AddDynamicEntity(new Enemy(
                new DynamicShape(new Vec2F((0.5f) - 0.05f, 0.6f), sizeVec), 
                imageStride));
        }

        public DiamondSquadron() {
            MaxEnemies = 4;
            Enemies = new EntityContainer<Enemy>();
        }
    }
}