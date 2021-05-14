using System;
using Raylib_cs;
using System.Numerics;
using System.Collections.Generic;


namespace Src
{
    class Grass
    {

        List<GrassBlade2> grass = new List<GrassBlade2>();
        Random random = new Random();
        public Grass()
        {
            for(var i = 0; i < 100; i++)
                grass.Add(new GrassBlade2( new Vector2(100 + i * 11, 600), random.Next(50, 100), 5));
        }
        
        public void Update()
        {
            Raylib.DrawCircleV(Raylib.GetMousePosition(), 50, Color.WHITE);
            foreach(var thing in grass)
                thing.Update();
        }
    }
    
    class GrassBlade
    {
        public Vector2 position;
        public float length;
        float stiffness;
        public float currentAngle;
        public float restAngle;
        float velocity = 0;

        public GrassBlade(Vector2 position, float length = 100, float stiffness = 1, float restAngle = MathF.PI, float currentAngle = MathF.PI)
        {
            this.position = position; this.length = length; this.stiffness = stiffness;
            this.currentAngle = currentAngle; this.restAngle = restAngle;
        }
    
        public void Update()
        {
            currentAngle = Math.Clamp(currentAngle, MathF.PI / 2, MathF.PI / 2 + MathF.PI);
            var force = (restAngle - currentAngle) * stiffness - velocity;
            velocity += force * Raylib.GetFrameTime();
            currentAngle += velocity * Raylib.GetFrameTime(); 
        }

        public void Draw()
        {
            var vec = position + Util.AngleToVec(currentAngle, length);
            Raylib.DrawLineEx(position, vec, 10, Color.GREEN);
            // Raylib.DrawCircleV(position, 5, Color.RED);
        }
    }

    class GrassBlade2
    {
        public GrassBlade gb1;
        public GrassBlade gb2 = new GrassBlade(Vector2.Zero);
        public GrassBlade2(Vector2 position, float length, float stiffness)
        {
            gb1 = new GrassBlade(position, length, stiffness);
            gb2 = new GrassBlade(position, length, stiffness);
            Gb2Pos();
        }

        public void Update()
        {
            gb1.Update();
            gb2.Update();
            Gb2Pos();

            var m = Raylib.GetMousePosition();
            var gb1Vec = gb1.position + Util.AngleToVec(gb1.currentAngle, gb1.length);
            var gb2Vec = gb2.position + Util.AngleToVec(gb2.currentAngle, gb2.length);
            var acc = MathF.Atan2(m.Y, m.X) * Raylib.GetFrameTime() * 5;
            if(Raylib.IsMouseButtonDown(MouseButton.MOUSE_LEFT_BUTTON))
            {
                if(Vector2.Distance(m, gb1Vec) < 50)
                {
                    gb1.currentAngle += acc;
                    gb2.currentAngle += acc;
                }
                if(Vector2.Distance(m, gb2Vec) < 50)
                    gb2.currentAngle += acc;
            }

            if(Raylib.IsMouseButtonDown(MouseButton.MOUSE_RIGHT_BUTTON))
            {
                if(Vector2.Distance(m, gb1Vec) < 50)
                {
                    gb1.currentAngle -= acc;
                    gb2.currentAngle -= acc;
                }
                if(Vector2.Distance(m, gb2Vec) < 50)
                    gb2.currentAngle -= acc;
            }

            gb1.Draw();
            gb2.Draw();
        }

        void Gb2Pos()
        {
            gb2.position = gb1.position + Util.AngleToVec(gb1.currentAngle, gb1.length);
        }
    }
}