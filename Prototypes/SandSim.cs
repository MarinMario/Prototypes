using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;
using System;

namespace Src
{
    class SandSim
    {
        int cellSize = 15;
        const int gridX = 40;
        const int gridY = 40;
        Cell[] state = new Cell[gridX * gridY];
        Random random = new Random();
        enum Cell { Empty, Sand, Water }

        public SandSim()
        {
        }

        bool updateGrid = false;
        public void Update()
        {
            if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_LEFT_BUTTON))
                changeCellAtMouse(Cell.Water);
            if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_RIGHT_BUTTON))
                changeCellAtMouse(Cell.Empty);

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
            for (var y = 0; y < gridY; y++)
                for (var x = 0; x < gridX; x++)
                {
                    var cell = state[y * gridX + x];
                    var color =
                        cell == Cell.Empty ? Color.GRAY :
                        cell == Cell.Water ? Color.BLUE :
                        cell == Cell.Sand ? Color.YELLOW : Color.BLACK;
                    Raylib.DrawRectangle(x * cellSize, y * cellSize, cellSize, cellSize, color);
                }
            Raylib.DrawFPS(1200, 0);

            var t = Raylib.MeasureText("PAUSED", 50);
            Raylib.DrawText(updateGrid ? "" : "PAUSED", Raylib.GetScreenWidth() / 2 - t / 2, Raylib.GetScreenHeight() / 2, 50, Color.WHITE);
            Raylib.DrawText($"speed: {speed}", 1, 1, 20, Color.WHITE);

            void changeCellAtMouse(Cell cell)
            {
                var m = Raylib.GetMousePosition();
                var x = (int)(m.X / cellSize);
                var y = (int)(m.Y / cellSize);

                if (x > 0 && y > 0 && x < gridX && y < gridY)
                    state[y * gridX + x] = cell;
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

            var next = new Cell[gridX * gridY];
            for (var i = 0; i < state.Length; i++)
                next[i] = state[i];

            for (var y = 0; y < gridY; y++)
            {
                for (var x = 0; x < gridX; x++)
                {
                    var i = y * gridX + x;
                    try
                    {
                        if (state[i] == Cell.Sand)
                        {
                            if (state[i + gridX] == Cell.Empty)
                            {
                                next[i] = Cell.Empty;
                                next[i + gridX] = Cell.Sand;
                            }
                            else if (state[i + gridX - 1] == Cell.Empty)
                            {
                                next[i] = Cell.Empty;
                                next[i + gridX - 1] = Cell.Sand;
                            }
                            else if (state[i + gridX + 1] == Cell.Empty)
                            {
                                next[i] = Cell.Empty;
                                next[i + gridX + 1] = Cell.Sand;
                            }
                        }
                        else if(state[i] == Cell.Water)
                        {
                            if (state[i + gridX] == Cell.Empty)
                            {
                                next[i] = Cell.Empty;
                                next[i + gridX] = Cell.Water;
                            }
                            else if (state[i + gridX - 1] == Cell.Empty)
                            {
                                next[i] = Cell.Empty;
                                next[i + gridX - 1] = Cell.Water;
                            }
                            else if (state[i + gridX + 1] == Cell.Empty)
                            {
                                next[i] = Cell.Empty;
                                next[i + gridX + 1] = Cell.Water;
                            }
                            else if(state[i + 1] == Cell.Empty)
                            {
                                next[i] = Cell.Empty;
                                next[i + 1] = Cell.Water;
                            }

                        }

                    }
                    catch (IndexOutOfRangeException e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
            state = next;
        }
    }
}
