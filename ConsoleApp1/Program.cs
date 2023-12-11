using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

enum Border
{
    MaxRight = 30,
    MaxBottom = 20
}

class Snake
{
    private List<int[]> body;
    private string direction;
    private bool isGameOver;
    private bool isFoodEaten;

    public Snake()
    {
        body = new List<int[]> { new int[] { 5, 5 } };
        direction = "Right";
        isGameOver = false;
        isFoodEaten = false;
    }

    public void StartGame()
    {
        Console.Clear();
        Console.CursorVisible = false;

        DrawBorder();

        Console.WriteLine("игра змейка");
        Console.WriteLine("нажми Enter для старта...");

        ConsoleKeyInfo key = Console.ReadKey();
        if (key.Key == ConsoleKey.Enter)
        {
            Console.Clear();
            DrawBorder();
            DrawSnake();
            DrawFood();

            Thread snakeThread = new Thread(MoveSnake);
            snakeThread.Start();

            while (!isGameOver)
            {
                if (Console.KeyAvailable)
                {
                    key = Console.ReadKey(true);
                    ChangeDirection(key);
                }
            }

            Console.Clear();
            Console.WriteLine("лох!");
        }
    }

    private void MoveSnake()
    {
        while (!isGameOver)
        {
            Move();
            DrawSnake();
            Thread.Sleep(100);
        }
    }

    private void Move()
    {
        int[] head = body.First().ToArray();
        int[] newHead = { head[0], head[1] };

        switch (direction)
        {
            case "Up":
                newHead[1]--;
                break;
            case "Down":
                newHead[1]++;
                break;
            case "Left":
                newHead[0]--;
                break;
            case "Right":
                newHead[0]++;
                break;
        }

        if (newHead[0] < 1 || newHead[0] >= (int)Border.MaxRight || newHead[1] < 1 || newHead[1] >= (int)Border.MaxBottom)
        {
            isGameOver = true;
            return;
        }

        if (body.Any(b => b.SequenceEqual(newHead)))
        {
            isGameOver = true;
            return;
        }

        body.Insert(0, newHead);

        if (newHead.SequenceEqual(foodPosition))
        {
            isFoodEaten = true;
            DrawFood();
        }

        if (!isFoodEaten)
        {
            Console.SetCursorPosition(body.Last()[0], body.Last()[1]);
            Console.Write(" ");
            body.RemoveAt(body.Count - 1);
        }
        else
        {
            isFoodEaten = false;
        }
    }

    private void ChangeDirection(ConsoleKeyInfo key)
    {
        switch (key.Key)
        {
            case ConsoleKey.UpArrow:
                if (direction != "Down")
                    direction = "Up";
                break;
            case ConsoleKey.DownArrow:
                if (direction != "Up")
                    direction = "Down";
                break;
            case ConsoleKey.LeftArrow:
                if (direction != "Right")
                    direction = "Left";
                break;
            case ConsoleKey.RightArrow:
                if (direction != "Left")
                    direction = "Right";
                break;
        }
    }

    private int[] foodPosition;
    private Random random = new Random();

    private void DrawFood()
    {
        int x, y;
        do
        {
            x = random.Next(1, (int)Border.MaxRight);
            y = random.Next(1, (int)Border.MaxBottom);
        } while (body.Any(b => b.SequenceEqual(new int[] { x, y })));

        foodPosition = new int[] { x, y };

        Console.SetCursorPosition(x, y);
        Console.Write("F");
    }

    private void DrawSnake()
    {
        Console.SetCursorPosition(body.First()[0], body.First()[1]);
        Console.Write("O");

        for (int i = 1; i < body.Count; i++)
        {
            Console.SetCursorPosition(body[i][0], body[i][1]);
            Console.Write("o");
        }
    }

    private void DrawBorder()
    {
        for (int i = 0; i <= (int)Border.MaxRight; i++)
        {
            Console.SetCursorPosition(i, 0);
            Console.Write("#");
            Console.SetCursorPosition(i, (int)Border.MaxBottom);
            Console.Write("#");
        }

        for (int i = 0; i <= (int)Border.MaxBottom; i++)
        {
            Console.SetCursorPosition(0, i);
            Console.Write("#");
            Console.SetCursorPosition((int)Border.MaxRight, i);
            Console.Write("#");
        }
    }
}

class Program
{
    static void Main()
    {
        Snake snakeGame = new Snake();
        snakeGame.StartGame();
    }
}
