using System;
using Raylib_cs;
using System.Numerics;
using System.Collections.Generic;

namespace Src
{
    class Program
    {
        static void Main(string[] args)
        {
            Raylib.InitWindow(1280, 720, "prototype");
            Raylib.SetTargetFPS(60);
            var thing = new Raycasting();

            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.BLACK);
                thing.Update();
                Raylib.EndDrawing();
            }
        }
    }
}