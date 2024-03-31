using System;
using System.Diagnostics;
using Microsoft.VisualBasic.CompilerServices;

namespace EightPuzzle 
{
    public class Check : IEquatable<Check>
    {
    public Grid[,] data;
    public Vector2 UnPossessedPos;

    public Check(string initState)
    {
        int k = 0;
        data = new Grid[3, 3];
        for (int i = 0; i != 3; ++i)
        {
            for (int j = 0; j != 3; ++j)
            {
                if (initState[k] == '0')
                {
                    UnPossessedPos = new Vector2(j, i);
                        data[i, j] = new Grid(0, UnPossessedPos);
                }
                else
                    data[i, j] = new Grid(initState[k] - '0',
                        new Vector2(j, i));

                k++;
            }

        }
    }

    public Check(Check check)
    {
        int k = 0;
        data = new Grid[3, 3];
        for (int i = 0; i != 3; ++i)
        {
            for (int j = 0; j != 3; ++j)
            {
                data[i, j] = new Grid(check.data[i, j].Number, new Vector2(j, i));
                if (!data[i, j].IsPossessed)
                    UnPossessedPos = new Vector2(j, i);
            }
        }
    }

    public Vector2 GetPosition(int number)
    {
        foreach (var grid in data)
        {
            if (grid.Number == number)
                return grid.position;
        }

        return new Vector2(Int32.MaxValue, Int32.MaxValue);
    }

    public void Print()
    {
        Console.WriteLine("-------------");
        Console.WriteLine("| {0} | {1} | {2} |",
            data[0, 0].Number, data[0, 1].Number, data[0, 2].Number);
        Console.WriteLine("| {0} | {1} | {2} |",
            data[1, 0].Number, data[1, 1].Number, data[1, 2].Number);
        Console.WriteLine("| {0} | {1} | {2} |",
            data[2, 0].Number, data[2, 1].Number, data[2, 2].Number);
        Console.WriteLine("-------------");
    }

    bool IEquatable<Check>.Equals(Check other)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (data[i, j].Number != other.data[i, j].Number)
                    return false;
            }
        }
        return true;
    }


    }
}