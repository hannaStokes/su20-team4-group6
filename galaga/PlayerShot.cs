using System;
using System.IO;
using DIKUArcade;
using DIKUArcade.Timers;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Entities;
public class PlayerShot : Entity {
    public PlayerShot(DynamicShape shape, IBaseImage image): base(shape, image) {
        shape.Direction = new Vec2F(0.0f , 0.01f);
    } 


} 