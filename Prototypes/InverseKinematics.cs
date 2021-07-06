using System;
using Raylib_cs;
using System.Numerics;
using System.Collections.Generic;


namespace Src
{
    class InverseKinematics
    {

        SegmentSet set;

        public InverseKinematics()
        {
            var s = new Segment[10];
            for (var i = 0; i < 10; i++)
                s[i] = new Segment(i * 2, 100);
            set = new SegmentSet(new Vector2(200), s);
        }

        public void Update()
        {
            set.Update();
            foreach (var thing in set.set)
                Raylib.DrawLineEx(thing.p1, thing.p2, 10, Color.RED);
        }
    }

    class Segment
    {
        public Vector2 p1;
        public Vector2 p2;
        public float angle;
        public float length;

        public Segment(float angle, float length)
        {
            this.angle = angle;
            this.length = length;
        }

        public Vector2 CalcP2()
        {
            var r = Util.AngleToVec(angle, length) + p1;
            p2 = r;
            return r;
        }

        public Vector2 CalcP1()
        {
            var r = p2 - Util.AngleToVec(angle, length);
            p1 = r;
            return r;
        }
    }

    class SegmentSet
    {
        public Segment[] set;

        public SegmentSet(Vector2 position, Segment[] set)
        {
            set[0].p1 = position;
            set[0].CalcP2();
            for (var i = 1; i < set.Length; i++)
            {
                set[i].p1 = set[i - 1].p2;
                set[i].CalcP2();
            }
            this.set = set;
        }

        public void Update()
        {
            if(Raylib.IsMouseButtonDown(MouseButton.MOUSE_LEFT_BUTTON))
            {
                var mouse = Raylib.GetMousePosition();
                set[^1].angle = 90 - Util.VecToAngle(mouse - set[^1].p1);
                set[^1].p2 = mouse;
                set[^1].CalcP1();

                for(var i = set.Length - 2; i >= 0; i--)
                {
                    set[i].angle = 90 - Util.VecToAngle(set[i + 1].p1 - set[i].p1);
                    set[i].p2 = set[i + 1].p1;
                    set[i].CalcP1();
                }
            }
        }
    }
}
