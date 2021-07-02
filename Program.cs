using System;
using Raylib_cs;
using System.Numerics;
using System.Collections.Generic;

namespace Src
{
    class Program
    {
        public static Vector2 cameraPos = Vector2.Zero;
        static void Main(string[] args)
        {
            Raylib.InitWindow(1280, 720, "prototype");
            Raylib.SetWindowState(ConfigFlag.FLAG_WINDOW_RESIZABLE);
            var camera = new Camera2D(Vector2.Zero, Vector2.Zero, 0, 1);
     
            var thing = new PlanetSim();
            var cameraFollowMouse = false;
            var clickMousePos = Vector2.Zero;
            var clickCameraPos = Vector2.Zero;

            while (!Raylib.WindowShouldClose())
            {
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_UP))
                {
                    camera.zoom += 0.5f;
                    var m = Raylib.GetMousePosition();
                    camera.target = m;
                    camera.offset = new Vector2(Raylib.GetScreenWidth(), Raylib.GetScreenHeight()) / 2;
                    cameraPos = camera.target - camera.offset;

                }
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_DOWN) && camera.zoom > 0.5f)
                    camera.zoom -= 0.5f;

                if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_MIDDLE_BUTTON))
                {
                    cameraFollowMouse = true;
                    clickMousePos = Raylib.GetMousePosition();
                    clickCameraPos = camera.target;
                }
                if (Raylib.IsMouseButtonReleased(MouseButton.MOUSE_MIDDLE_BUTTON))
                    cameraFollowMouse = false;

                if (cameraFollowMouse)
                {
                    camera.target = clickCameraPos + (clickMousePos - Raylib.GetMousePosition()) / camera.zoom;
                }

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