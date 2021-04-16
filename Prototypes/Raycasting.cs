using System;
using Raylib_cs;
using System.Numerics;
using System.Collections.Generic;

namespace Src
{
    class Raycasting
    {
        Vector2 playerPos = new Vector2(300, 300);
        List<Wall> walls = new List<Wall>{ 
            new Wall(new Vector2(400, 200), new Vector2(500, 600)),
            new Wall(new Vector2(600, 600), new Vector2(800, 500)),
            new Wall(new Vector2(100, 100), new Vector2(400, 150)),
            new Wall(new Vector2(800, 600), new Vector2(1000, 400)),
        };
        List<Raycast> rays = new List<Raycast>();
        public Raycasting()
        {
            for(var i = 0; i < 100; i++)
                rays.Add(new Raycast(Vector2.Zero, MathF.PI / 2, 10000));
        }
        public void Update()
        {
            //player movement
            var dir = Vector2.Zero;
            if(Raylib.IsKeyDown(KeyboardKey.KEY_D))
                dir.X = 1;
            if(Raylib.IsKeyDown(KeyboardKey.KEY_A))
                dir.X = -1;
            if(Raylib.IsKeyDown(KeyboardKey.KEY_S))
                dir.Y = 1;
            if(Raylib.IsKeyDown(KeyboardKey.KEY_W))
                dir.Y = -1;
            if(dir != Vector2.Zero)
                dir = Vector2.Normalize(dir);
            playerPos += dir * Raylib.GetFrameTime() * 300;
            Raylib.DrawCircleV(playerPos, 10, Color.RED);

            for(var i = 0; i < rays.Count; i++)
            {
                rays[i].origin = playerPos;
                rays[i].angle = MathF.PI / 4 + MathF.PI / rays.Count / 2 * i - Util.VecToAngle(Raylib.GetMousePosition() - playerPos);
                rays[i].Update(walls);
            }
            foreach(var wall in walls)
                Raylib.DrawLineV(wall.p1, wall.p2, Color.PURPLE);

        }

    }

    class Raycast
    {
        public Vector2 origin;
        public float angle; 
        float target;
        public Raycast(Vector2 origin, float angle, float target)
        {
            this.origin = origin; this.angle = angle; this.target = target;
        }

        public void Update(List<Wall> walls)
        {
            var tpos = Util.AngleToVec(angle, target);
            var rayCollision = RayCollision(walls);
            if(rayCollision != Vector2.Zero)
                Raylib.DrawLineV(origin, rayCollision, Color.WHITE);
            else
                Raylib.DrawLineV(origin, origin + tpos, Color.WHITE);
            
            // Raylib.DrawCircleV(rayCollision, 5, Color.BLUE);
        }

        Vector2 RayCollision(List<Wall> walls)
        {
            var position = Vector2.Zero;
            var dir = Util.AngleToVec(angle, 1);
            while(position.Length() < target)
            {
                position += dir;
                foreach(var wall in walls)
                {
                    var t = position + origin;
                    var a = Vector2.Distance(wall.p1, t) + Vector2.Distance(t, wall.p2);
                    var b = Vector2.Distance(wall.p1, wall.p2);
                    if(a - b < 0.1)
                        return t;
                }
            }

            return Vector2.Zero;

        }
    }

    class Wall
    {
        public Vector2 p1;
        public Vector2 p2;
        public Wall(Vector2 p1, Vector2 p2)
        {
            this.p1 = p1; this.p2 = p2;
        }
    }
}