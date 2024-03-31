using System;

namespace EightPuzzle
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            string initState = "724506831";
            string goalState = "012345678";
            Puzzle puzzle = new Puzzle(initState, goalState);
            puzzle.AStar();
        }
    }
}