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
            Raylib.SetWindowState(ConfigFlag.FLAG_WINDOW_RESIZABLE);
     
            var thing = new HookGame();

            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.BLACK);
                if (Raylib.GetFrameTime() < 0.25)
                    thing.Update();
                Raylib.EndDrawing();
            }
        }
    }
}