using System;
using System.Collections.Generic;

namespace EightPuzzle
{
    enum SolveMethod
    {
        Greedy,
        AStar
    }

    enum Direction
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }
    public class Puzzle
    {
        public Check currentState;
        public Check goalState;
        private string initState;
        private SolveMethod method;

        public Puzzle(string initState, string goalState)
        {
            initState = initState;
            this.goalState = new Check(goalState);
            this.currentState = new Check(initState);
        }

        void Solve(SolveMethod method)
        {
            this.method = method;
            switch (method)
            {
                case SolveMethod.Greedy:
                    break;
                case SolveMethod.AStar:
                    break;
            }
        }

        void PrintPath(Node node)
        {
            if (node.parent != null)
            {
                PrintPath(node.parent);
            }
            node.check.Print();
        }

        public bool Greedy_Recursion(Node node, ref List<Node> explored)
        {
            if (InExplored(ref node, ref explored))
                return false;
            explored.Add(node);

            var actions = GetActions(node, SolveMethod.Greedy);

            while (actions.Count != 0)
            {
                var temp = actions.Dequeue();
                node.children.Enqueue(temp, temp.mhtDistance);
            }

            while (node.children.Count != 0)
            {
                var child = node.children.Dequeue();

                if (CheckEquality(ref child.check, ref goalState))
                {
                    Console.WriteLine("Goal Reached!");
                    PrintPath(child);
                    return true;
                }

                if (Greedy_Recursion(child, ref explored))
                    return true;
            }

            return false;
        }

        public void Greedy()
        {
            Node currentNode = new Node(currentState);
            currentNode.cost = 0;
            currentNode.parent = null;
            currentNode.mhtDistance = ManhattanDistance(currentState);
            currentNode.heuristicValue = currentNode.cost + currentNode.mhtDistance;
            List<Node> explored = new List<Node>();
            PriorityQueue<Node, int> queue = new PriorityQueue<Node, int>();
            queue.Enqueue(currentNode, currentNode.mhtDistance);
            Node temp;
            Greedy_Recursion(currentNode, ref explored);
        }

        bool InExplored(ref Node node, ref List<Node> explored)
        {
            foreach (Node nd in explored)
            {
                if (CheckEquality(ref node.check, ref nd.check))
                {
                    return true;
                }
            }
            return false;
        }

        bool CheckEquality(ref Check check1, ref Check check2)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (check1.data[i, j].Number != check2.data[i, j].Number)
                        return false;
                }
            }
            return true;
        }
        
        public void AStar()
        {
            Node currentNode = new Node(currentState);
            currentNode.cost = 0;
            currentNode.parent = null;
            currentNode.mhtDistance = ManhattanDistance(currentState);
            currentNode.heuristicValue = currentNode.cost + currentNode.mhtDistance;
            List<Node> explored = new List<Node>();
            PriorityQueue<Node, int> queue = new PriorityQueue<Node, int>();
            queue.Enqueue(currentNode, currentNode.heuristicValue);
            Node temp;

            while (queue.Count != 0)
            {
                currentNode = queue.Dequeue();
                if (InExplored(ref currentNode, ref explored))
                    continue;
                explored.Add(currentNode);
                if (CheckEquality(ref currentNode.check, ref goalState))
                {
                    Console.WriteLine("Goal Reached!");
                    PrintPath(currentNode);
                    return;
                }
                var actions = GetActions(currentNode, SolveMethod.AStar);

                while (actions.Count != 0)
                {
                    var action = actions.Dequeue();
                    if (!explored.Contains(action))
                    {
                        queue.Enqueue(action, action.heuristicValue);
                    }
                }
            }

            Console.WriteLine("Failed to reach the goal!");
        }

        PriorityQueue<Node, int> GetActions(Node state, SolveMethod method)
        {
            PriorityQueue<Node, int> actions = new PriorityQueue<Node, int>();
            Grid temp;
            var moves = GetMoves(state.check);
            Node newState;
            var pos = state.check.UnPossessedPos;
            foreach (var move in moves)
            {
                newState = new Node(node: state);
                switch (move)
                {
                    case Direction.UP:
                    {
                        temp = newState.check.data[pos.y - 1, pos.x];
                        newState.check.data[pos.y, pos.x].position.y -= 1;
                        temp.position.y += 1;
                        newState.check.data[pos.y - 1, pos.x] = newState.check.data[pos.y, pos.x];
                        newState.check.data[pos.y, pos.x] = temp;
                        newState.check.UnPossessedPos.y -= 1;
                        
                        break;
                    }
                    case Direction.DOWN:
                    {
                        temp = newState.check.data[pos.y + 1, pos.x];
                        newState.check.data[pos.y, pos.x].position.y += 1;
                        temp.position.y -= 1;
                        newState.check.data[pos.y + 1, pos.x] = newState.check.data[pos.y, pos.x];
                        newState.check.data[pos.y, pos.x] = temp;
                        newState.check.UnPossessedPos.y += 1;
                        break;
                    }
                    case Direction.LEFT:
                    {
                        temp = newState.check.data[pos.y, pos.x - 1];
                        newState.check.data[pos.y, pos.x].position.x -= 1;
                        temp.position.x += 1;
                        newState.check.data[pos.y, pos.x - 1] = newState.check.data[pos.y, pos.x];
                        newState.check.data[pos.y, pos.x] = temp;
                        newState.check.UnPossessedPos.x -= 1;
                 
                        break;
                    }
                    case Direction.RIGHT:
                    {
                        temp = newState.check.data[pos.y, pos.x + 1];
                        temp.position.x -= 1;
                        newState.check.data[pos.y, pos.x].position.x += 1;
                        newState.check.data[pos.y, pos.x + 1] = newState.check.data[pos.y, pos.x];
                        newState.check.data[pos.y, pos.x] = temp;
                        newState.check.UnPossessedPos.x += 1;
                            break;
                    }
                }
                newState.parent = state;
                newState.mhtDistance = ManhattanDistance(newState.check);
                newState.cost = state.cost + 1;
                newState.heuristicValue = newState.cost + newState.mhtDistance;
                state.AddChild(newState);
                if(method == SolveMethod.Greedy)
                    actions.Enqueue(newState, newState.mhtDistance);
                else
                    actions.Enqueue(newState, newState.heuristicValue);
            }
            return actions;
        }

        List<Direction> GetMoves(Check state)
        {
            List<Direction> moves = new List<Direction>() { Direction.UP, Direction.DOWN, Direction.LEFT, Direction.RIGHT };
            var pos = state.UnPossessedPos;
            if (pos.x == 0)
                moves.Remove(Direction.LEFT);
            else if (pos.x == 2)
                moves.Remove(Direction.RIGHT);
            if (pos.y == 0)
                moves.Remove(Direction.UP);
            else if(pos.y == 2)
                moves.Remove(Direction.DOWN);
            return moves;
        }
        
        public int ManhattanDistance(Check state)
        {
            int distance = 0;
            foreach (var grid in state.data)
            {
                var targetPosition = goalState.GetPosition(grid.Number);
                distance += Math.Abs(grid.position.x - targetPosition.x) 
                       + Math.Abs(grid.position.y - targetPosition.y);
            }
            return distance;
        }
    }
}