using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using System.Collections.Generic;

namespace galaga.Squadron {
    public class LineSquadron : ISquadron {
        public EntityContainer<Enemy> Enemies { get; }

        public int MaxEnemies { get; }

        public void CreateEnemies(List<Image> enemyStrides)
        {
            Vec2F sizeVec = new Vec2F(0.1f, 0.1f);
            ImageStride imageStride = new ImageStride(80, enemyStrides);

            for (int i = 0; i < MaxEnemies; i++)
            {
                Enemies.AddDynamicEntity(new Enemy(
                    new DynamicShape(new Vec2F(((i + 1.0f) / 4.0f) - 0.175f, 0.8f), sizeVec), 
                    imageStride));
            }
        }

        public LineSquadron() {
            MaxEnemies = 4;
            Enemies = new EntityContainer<Enemy>();
        }
    }
}