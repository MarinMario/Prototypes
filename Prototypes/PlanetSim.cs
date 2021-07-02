using System;
using Raylib_cs;
using System.Numerics;
using System.Collections.Generic;

namespace Src
{
    class PlanetSim
    {

        List<Planet> planets = new List<Planet>
        {
            new Planet(100000, new Vector2(600, 200), Vector2.One * -100),
            new Planet(100000, new Vector2(100, 500), Vector2.One * 100),
            new Planet(20000, new Vector2(1000, 1500), new Vector2(-300, 0))
        };

        public void Update()
        {
            foreach (var planet in planets)
                planet.Update(planets);
        }
    }

    class Planet
    {
        public float size;
        public Vector2 position;
        Vector2 velocity = Vector2.Zero;

        public Planet(float size, Vector2 position, Vector2 velocity)
        {
            this.size = size; this.position = position; this.velocity = velocity;
        }

        public void Update(List<Planet> planets)
        {
            var delta = Raylib.GetFrameTime();
            foreach (var planet in planets)
                if (planet != this)
                {
                    var dir = Vector2.Normalize(planet.position - position);
                    var dist = Vector2.Distance(planet.position, position);
                    if (dist != 0)
                        velocity += dir * planet.size / dist * delta;
                    position += velocity * delta;
                }

            //Raylib.DrawCircleLines((int)position.X, (int)position.Y, orbitSize, Color.WHITE);
            Raylib.DrawCircle((int)position.X, (int)position.Y, size / 1000, Color.WHITE);
        }
    }

}