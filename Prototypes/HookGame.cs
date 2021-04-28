using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;
using System;

namespace Src
{
    class HookGame
    {
        HookPlayer player = new HookPlayer();
        public void Update()
        {
            player.Update();
        }
    }

    class HookPlayer
    {
        Vector2 position = Vector2.One * 300;
        float velocityY = 0;
        float speed = 600;
        float gravity = 1000;

        public Vector2 dir = Vector2.Zero;

        public void Update()
        {
            var delta = Raylib.GetFrameTime();
            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON))
                dir = Vector2.Normalize(Raylib.GetMousePosition() - position);
            if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_LEFT_BUTTON))
            {
                velocityY = 0;

                var lineEnd = position + dir * 10000;
                Raylib.DrawLine((int)position.X, (int)position.Y, (int)lineEnd.X, (int)lineEnd.Y, Color.RED);
            }
            else
            {
                velocityY += gravity * delta;
                position.Y += velocityY * delta;
            }

            position += dir * speed * delta;

            Raylib.DrawCircle((int)position.X, (int)position.Y, 30, Color.WHITE);
        }
    }

}
