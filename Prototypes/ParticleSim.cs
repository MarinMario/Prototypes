using System;
using Raylib_cs;
using System.Numerics;
using System.Collections.Generic;

namespace Src
{
    class ParticleSim
    {

        List<Particle> state = new List<Particle>();
        Random random = new Random();

        public ParticleSim()
        {
            for(var x = 0; x < 30; x++)
                for(var y = 0; y < 30; y++)
                {
                    var t = random.Next(0, 3);
                    var pos = new Vector2(x, y) * 20 + new Vector2(300, 30);
                    var dir = (float)random.NextDouble() * 4;
                    if (t == 0)
                        state.Add(Particle.GreenParticle(pos, dir));
                    else if (t == 1)
                        state.Add(Particle.BlueParticle(pos, dir));
                    else
                        state.Add(Particle.RedParticle(pos, dir));
                }
        }

        float timer = 0;

        bool updateState = false;
        public void Update()
        {
            var delta = Raylib.GetFrameTime();
            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON))
            {
                var dir = (float)random.NextDouble() * 4;
                if (Raylib.IsKeyDown(KeyboardKey.KEY_G))
                    state.Add(Particle.GreenParticle(Raylib.GetMousePosition(), dir));
                if (Raylib.IsKeyDown(KeyboardKey.KEY_R))
                    state.Add(Particle.RedParticle(Raylib.GetMousePosition(), dir));
                if (Raylib.IsKeyDown(KeyboardKey.KEY_B))
                    state.Add(Particle.BlueParticle(Raylib.GetMousePosition(), dir));
            }

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_BACKSPACE))
                state.Clear();
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE))
                updateState = !updateState;

            foreach (var p1 in state)
            {
                p1.Draw();
                if (!updateState)
                    continue;
                p1.Update(state);
            }

            timer += delta;
            if(timer > 0.1f)
            {
                Console.WriteLine(state.Count);
                timer = 0;
            }
        }
    }

    class Particle
    {
        public Vector2 position;
        public float radius;
        public float direction;
        public float rotationSpeed;
        public float walkSpeed;
        public float effectRadius;
        public Color color;
        public enum Type { Green, Red, Blue }
        public Type type;
        Random random = new Random();

        public Particle(Vector2 position, float radius, float direction, float rotationSpeed, float walkSpeed, float effectRadius, Color color, Type type)
        {
            this.position = position; this.radius = radius; this.direction = direction; this.rotationSpeed = rotationSpeed;
            this.walkSpeed = walkSpeed; this.effectRadius = effectRadius; this.color = color; this.type = type;
        }

        public static Particle GreenParticle(Vector2 position, float direction)
        {
            return new Particle(position, 5, direction, 1, 10, 100, Color.GREEN, Type.Green);
        }

        public static Particle RedParticle(Vector2 position, float direction)
        {
            return new Particle(position, 3, direction, 2, 25, 30, Color.RED, Type.Red);
        }

        public static Particle BlueParticle(Vector2 position, float direction)
        {
            return new Particle(position, 7, direction, 3, 60, 100, Color.BLUE, Type.Blue);
        }

        public void Update(List<Particle> state)
        {
            var delta = Raylib.GetFrameTime();

            var collision = false;
            var effects = new List<Particle>();
            foreach (var p in state)
            {
                if (p != this)
                {
                    var distance = Vector2.Distance(position, p.position);
                    if (distance < radius + p.radius)
                    {
                        collision = true;
                        position += Vector2.Normalize(position - p.position) * walkSpeed * 2 * delta;
                    }
                    else if(distance < effectRadius + p.radius)
                    {
                        effects.Add(p);
                    }
                }
            }


            foreach (var p in effects)
            {
                var v = position - p.position;
                var vel = v != Vector2.Zero ? Vector2.Normalize(position - p.position) * delta : new Vector2((float)random.NextDouble()) * 10;
                if (type == Type.Green && p.type == Type.Red)
                    p.position += vel * p.walkSpeed;
                if (type == Type.Blue && p.type == Type.Green)
                    p.position += vel * p.walkSpeed;
                if (type == Type.Red && p.type == Type.Blue)
                    p.position += vel * p.walkSpeed;

            }

            position.X = Math.Clamp(position.X, 0, 1280);
            position.Y = Math.Clamp(position.Y, 0, 720);
        }

        public void Draw()
        {
            var vecdir = Util.AngleToVec(direction, 1);
            Raylib.DrawCircle((int)position.X, (int)position.Y, radius, color);
            //Raylib.DrawLineEx(position, position + vecdir * radius, 1, Color.BLACK);
            //Raylib.DrawCircleLines((int)position.X, (int)position.Y, effectRadius, color);
        }
    }
}
