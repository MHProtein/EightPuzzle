namespace EightPuzzle;

public class Solution
{
    public bool solved = false;
    public bool cutoff;
    public LinkedList<Direction> steps;

    public Solution(bool solved)
    {
        this.solved = solved;
        steps = new LinkedList<Direction>();
    }

    public Solution(bool solved, bool cutoff)
    {
        this.solved = solved;
        this.cutoff = cutoff;
        steps = new LinkedList<Direction>();
    }

    public void PrintSteps()
    {
        if (!solved)
        {
            if (cutoff)
            {
                Console.WriteLine("Cutoff");
            }
            else
            {
                Console.WriteLine("No solution");
            }
            return;
        }
        Console.WriteLine("Steps : ");
        int i = 0;
        foreach (var step in steps)
        {
            if (i != steps.Count - 1)
            {
                Console.Write("{0} -> ", step);
            }
            else
            {
                Console.Write(step);
            }

            i++;
        }
        Console.WriteLine();
    }

}