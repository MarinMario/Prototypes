using System;
using Raylib_cs;
using System.Numerics;
using System.Collections.Generic;

namespace Src
{
    class SoftBodySim
    {

        SoftBody b1 = new SoftBody(new Vector2(200, 200), 5, 10, 0.5f, 1, 20, 20);
        Random random = new Random();
        bool update = false;
        public void Update()
        {
            if(Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE))
                update = !update;
            if(update)
                b1.Update();
            b1.Draw();
            if(Raylib.IsKeyDown(KeyboardKey.KEY_D))
                b1.ApplyForce(new Vector2(50 * Raylib.GetFrameTime(), 0));
            if(Raylib.IsKeyDown(KeyboardKey.KEY_A))
                b1.ApplyForce(new Vector2(-50 * Raylib.GetFrameTime(), 0));
            if(Raylib.IsKeyDown(KeyboardKey.KEY_W))
                b1.ApplyForce(new Vector2(0, -200 * Raylib.GetFrameTime()));
            if(Raylib.IsKeyDown(KeyboardKey.KEY_S))
                b1.ApplyForce(new Vector2(0, 200 * Raylib.GetFrameTime()));
            
            if(Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
            {
                b1 = new SoftBody(
                    new Vector2(300, -100), random.Next(10, 120),
                    random.Next(10, 100), (float)random.Next(1, 100) / 10f, random.Next(1, 10), 
                    random.Next(2, 20), random.Next(2, 20));
            }
        }
    }

    class SoftBody
    {
        public List<MassPoint> points = new List<MassPoint>();
        public List<Spring> springs = new List<Spring>();
        Rectangle ground = new Rectangle(-100, 650, 1400, 100);
        Rectangle platform = new Rectangle(500, 200, 200, 100);
        float springLength;
        float collapseFactor;
        public SoftBody(Vector2 position, float stiffness = 50, float length = 50, float collapseFactor = 3, float damp = 10, int row = 5, int column = 7)
        {
            this.springLength = length;
            this.collapseFactor = collapseFactor;
            for(var y = 0; y < column; y++)
                for(var x = 0; x < row; x++)
                    points.Add(new MassPoint((length - 5) * new Vector2(x, y) + position, 1));
            
            for(var y = 0; y < column; y++)
                for(var x = 0; x < row; x++)
                {
                    var e = y * row + x;
                    if(e + 1 < y * row + row)
                        springs.Add(new Spring(points[e], points[e + 1], stiffness, length, damp));
                    if(y < column - 1)
                        springs.Add(new Spring(points[e], points[e + row], stiffness, length, damp));
                    if(y < column - 1 && e + 1 < y * row + row)
                    {
                        springs.Add(new Spring(points[e], points[e + row + 1], stiffness, length, damp));
                        springs.Add(new Spring(points[e + 1], points[e + row], stiffness, length, damp));
                    }
                }
            
        }
        public void Update()
        {
            foreach(var s in springs)
            {
                s.Update();
            }

            foreach(var p in points)
                if(Raylib.CheckCollisionCircleRec(p.position, 5, ground) && p.velocity.Y > 0)
                {
                    p.position.Y = ground.y;
                    p.velocity.Y = 0;
                }
            
            foreach(var p1 in points)
                foreach(var p2 in points)
                    if(p1 != p2 && Raylib.CheckCollisionCircles(p1.position, springLength / collapseFactor, p2.position, springLength / collapseFactor))
                    {
                        var dir = p2.position - p1.position;
                        p1.velocity -= dir * Raylib.GetFrameTime();
                    }
        }

        public void Draw()
        {
            Raylib.DrawRectangle((int)ground.x, (int)ground.y, (int)ground.width, (int)ground.height, Color.DARKPURPLE);
            foreach(var s in springs)
                s.Draw(Color.BLUE);
            for(var i = 0; i < points.Count; i++)
                points[i].Draw(Color.RED);
        }

        public void ApplyForce(Vector2 force)
        {
            foreach(var p in points)
                p.velocity += force;
        }
    }

    class MassPoint
    {
        public Vector2 position;
        public Vector2 velocity = Vector2.Zero;
        public Vector2 force = Vector2.Zero;
        float gravity = 10f;
        float mass;

        public MassPoint(Vector2 position, float mass)
        {
            this.position = position;
            this.mass = mass;
        }

        public void Update(Vector2 addForce)
        {
            var delta = Raylib.GetFrameTime() < 0.2 ? Raylib.GetFrameTime() : 0;
            force = Vector2.Zero;
            var gForce = new Vector2(0, gravity * mass);
            force += gForce + addForce;
            velocity += (force * delta) / mass;
            position += velocity * delta;
        }

        public void Draw(Color color)
        {
            Raylib.DrawCircle((int)position.X, (int)position.Y, 2, color);
        }
    }

    class Spring
    {
        MassPoint point1, point2;
        float stiffness;
        float restLength;
        float dampingFactor;
        public Spring(MassPoint point1, MassPoint point2, float stiffness, float restLength, float dampingFactor)
        {
            this.point1 = point1;
            this.point2 = point2;
            this.stiffness = stiffness;
            this.restLength = restLength;
            this.dampingFactor = dampingFactor;
        }

        public void Update()
        {
            var currentLength = Vector2.Distance(point1.position, point2.position);
            
            var dir = Vector2.Normalize(point2.position - point1.position);
            var velDiff = point2.velocity - point1.velocity;
            var dampingForce = Vector2.Dot(dir, velDiff) * dampingFactor;

            var force = stiffness * (currentLength - restLength) + dampingForce;
            point1.Update(force * dir);
            point2.Update(-force * dir);
                
        }

        public void Draw(Color color)
        {
            Raylib.DrawLine((int)point1.position.X, (int)point1.position.Y, (int)point2.position.X, (int)point2.position.Y, color);
        }
    }
}
