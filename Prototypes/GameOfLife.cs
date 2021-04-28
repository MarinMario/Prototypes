using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;
using System;

namespace Src
{
    class GameOfLife
    {
        int cellSize = 20;
        (int X, int Y) gridSize = (100, 100);
        List<List<bool>> state = new List<List<bool>>();

        public GameOfLife()
        {
            for (var y = 0; y < gridSize.Y; y++)
            {
                var row = new List<bool>();
                for (var x = 0; x < gridSize.X; x++)
                    row.Add(false);
                state.Add(row);
            }
        }

        bool updateGrid = false;
        public void Update()
        {
            if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_LEFT_BUTTON))
                changeCellAtMouse(true);
            if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_RIGHT_BUTTON))
                changeCellAtMouse(false);

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE))
                updateGrid = !updateGrid;

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_RIGHT))
                speed += 5;
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_LEFT))
                speed -= 5;
            speed = (int)Math.Clamp(speed, 0, 100);

            if (updateGrid)
                UpdateGrid();


            //rendering
            for (var y = 0; y < gridSize.Y; y++)
                for (var x = 0; x < gridSize.X; x++)
                {
                    var color = state[y][x] ? Color.GREEN : Color.BLUE;
                    Raylib.DrawRectangle(x * cellSize, y * cellSize, cellSize, cellSize, color);
                }

            var t = Raylib.MeasureText("PAUSED", 50);
            Raylib.DrawText(updateGrid ? "" : "PAUSED", Raylib.GetScreenWidth() / 2 - t / 2, Raylib.GetScreenHeight() / 2, 50, Color.WHITE);
            Raylib.DrawText($"speed: {speed}", 1, 1, 20, Color.WHITE);

            void changeCellAtMouse(bool alive)
            {
                var m = Raylib.GetMousePosition();
                var x = (int)(m.X / cellSize);
                var y = (int)(m.Y / cellSize);
                if (x < gridSize.X && y < gridSize.Y)
                    state[y][x] = alive;
            }
        }


        float timer = 0f;
        float speed = 10f;
        public void UpdateGrid()
        {
            timer += Raylib.GetFrameTime();
            if (timer > speed / 100)
                timer = 0;
            else
                return;

            var newState = new List<List<bool>>();
            for (var y = 0; y < gridSize.Y; y++)
            {
                var row = new List<bool>();
                for (var x = 0; x < gridSize.X; x++)
                {
                    var count = countNeighbors(x, y);
                    if (state[y][x] && count == 2 || count == 3)
                        row.Add(true);
                    else if (!state[y][x] && count == 3)
                        row.Add(true);
                    else
                        row.Add(false);

                }
                newState.Add(row);
            }
            state = newState;

            int countNeighbors(int x, int y)
            {
                var count = 0;
                var neis = new (int x, int y)[] { (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1) };
                foreach (var n in neis)
                    try
                    {
                        if (state[y + n.y][x + n.x])
                            count += 1;
                    }
                    catch { };

                return count;
            }
        }
    }
}
