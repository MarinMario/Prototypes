using System;
using Raylib_cs;
using System.Numerics;
using System.Collections.Generic;


namespace Src
{
    class Bow
    {
        float rotation = 0;
        List<Arrow> arrows = new List<Arrow>();
        float speedTimer = 0;

        public void Update()
        {
            var vec = Util.AngleToVec(rotation, 100);
            var center = new Vector2(Raylib.GetScreenWidth(), Raylib.GetScreenHeight()) / 2;
            Raylib.DrawLineEx(center, center + vec, 3, Color.RED);

            rotation = MathF.PI / 2 - Util.VecToAngle(Vector2.Normalize(Raylib.GetMousePosition() - center));

            foreach (var arrow in arrows)
                arrow.Update();

            if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_LEFT_BUTTON))
            {
                speedTimer += Raylib.GetFrameTime();
                var text = (int)(speedTimer * 100);
                var textSize = Raylib.MeasureText(text.ToString(), 30);
                Raylib.DrawText(text.ToString(), Raylib.GetScreenWidth() / 2 - textSize / 2, 20, 30, Color.RED);
            }
            else
            {
                if (speedTimer > 0.1)
                    arrows.Add(new Arrow(center, rotation, speedTimer * 1000));
                speedTimer = 0;
            }

        }
    }

    class Arrow
    {
        Vector2 position;
        float rotation;
        Vector2 velocity = Vector2.Zero;
        
        public Arrow(Vector2 position, float rotation, float speed)
        {
            this.position = position;
            this.rotation = rotation;
            this.velocity = Util.AngleToVec(rotation, 1) * speed;
        }

        public void Update()
        {
            var delta = Raylib.GetFrameTime();

            var lastPos = position;

            velocity.X += -Math.Sign(velocity.X) * 100 * delta; 
            velocity.Y += 1000 * delta; 
            position += velocity * delta;
            Raylib.DrawCircleV(position, 10, Color.GREEN);
            Raylib.DrawLineV(position + Vector2.Normalize(position - lastPos) * 100, position, Color.BLUE);
        }
    }
}
