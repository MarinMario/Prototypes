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
            var camera = new Camera2D(Vector2.Zero, Vector2.Zero, 0, 1);
     
            var thing = new Bow();

            while (!Raylib.WindowShouldClose())
            {
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_UP))
                {
                    camera.zoom += 0.5f;
                    var m = Raylib.GetMousePosition();
                    camera.target = m;
                    camera.offset = new Vector2(Raylib.GetScreenWidth(), Raylib.GetScreenHeight()) / 2;

                }
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_DOWN) && camera.zoom > 0.5f)
                    camera.zoom -= 0.5f;

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.BLACK);
                
                Raylib.BeginMode2D(camera);
                if (Raylib.GetFrameTime() < 0.25)
                    thing.Update();
                Raylib.EndMode2D();

                Raylib.EndDrawing();
            }
        }
    }
}