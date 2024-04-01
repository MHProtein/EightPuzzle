using System;
using System.Collections.Generic;

namespace EightPuzzle
{
    public enum SolveMethod
    {
        Greedy,
        AStar
    }

    public enum Direction
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

        public Solution Greedy()
        {
            Node currentNode = new Node(currentState);
            currentNode.cost = 0;
            currentNode.parent = null;
            currentNode.mhtDistance = ManhattanDistance(currentState);
            currentNode.heuristicValue = currentNode.cost + currentNode.mhtDistance;
            List<Node> explored = new List<Node>();
            PriorityQueue<Node, int> queue = new PriorityQueue<Node, int>();
            queue.Enqueue(currentNode, currentNode.mhtDistance);

            while (queue.Count != 0)
            {
                currentNode = queue.Dequeue();
                if (InExplored(ref currentNode, ref explored))
                    continue;
                explored.Add(currentNode);
                if (CheckEquality(ref currentNode.check, ref goalState))
                {
                    var solution = new Solution(true);
                    Console.WriteLine("Goal Reached!");
                    solution.steps = GetPath(currentNode);
                    return solution;
                }
                var actions = GetActions(currentNode, SolveMethod.Greedy);

                while (actions.Count != 0)
                {
                    var action = actions.Dequeue();
                    if (!explored.Contains(action))
                    {
                        queue.Enqueue(action, action.mhtDistance);
                    }
                }
            }

            return new Solution(false);
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
        
        public Solution IDAStar(Int64 depth)
        {
            Node currentNode = new Node(currentState);
            Node tempNode = new Node(currentState);
            currentNode.cost = 0;
            currentNode.parent = null;
            currentNode.mhtDistance = ManhattanDistance(currentState);
            currentNode.heuristicValue = currentNode.cost + currentNode.mhtDistance;
            List<Node> explored = new List<Node>();

            Solution solution = new Solution(false);

            for (int i = 0; i < depth; i++)
            {
                solution = AStar_Recursion(currentNode, ref explored, i);

                if (!solution.cutoff)
                {
                    return solution;
                }
                else
                {
                    currentNode = tempNode;
                    explored.Clear();
                }

            }

            return solution;
        }

        Solution AStar_Recursion(Node node, ref List<Node> explored, Int64 depth)
        {
            if (depth == 1)
            {
                return new Solution(false, true);
            }
            depth--;
            if (InExplored(ref node, ref explored))
                return new Solution(false);
            explored.Add(node);

            var actions = GetActions(node, SolveMethod.AStar);

            while (actions.Count != 0)
            {
                var temp = actions.Dequeue();
                node.children.Enqueue(temp, temp.heuristicValue);
            }

            while (node.children.Count != 0)
            {
                var child = node.children.Dequeue();

                if (CheckEquality(ref child.check, ref goalState))
                {
                    Console.WriteLine("Goal Reached!");
                    var solution = new Solution(true);
                    solution.steps = GetPath(child);
                    return solution;
                }

                var sol = AStar_Recursion(child, ref explored, depth);
                if (sol.solved)
                    return sol;
            }

            return new Solution(false);
        }

        PriorityQueue<Node, int> GetActions(Node state, SolveMethod method)
        {
            PriorityQueue<Node, int> actions = new PriorityQueue<Node, int>();
            Grid temp;
            var moves = GetMoves(state.check);
            Node newState;
            var pos = state.check.UnPossessedPos;
            var offset = new Vector2(0, 0);
            foreach (var move in moves)
            {
                offset = new Vector2(0, 0);
                newState = new Node(node: state);
                switch (move)
                {
                    case Direction.UP:
                        offset.y = -1;
                        break;
                    case Direction.DOWN:
                        offset.y = 1;
                        break;
                    case Direction.LEFT:
                        offset.x = -1;
                        break;
                    case Direction.RIGHT:
                        offset.x = 1;
                        break;
                }
                temp = newState.check.data[pos.y + offset.y, pos.x + offset.x];
                temp.position.x -= offset.x;
                temp.position.y -= offset.y;
                newState.check.data[pos.y, pos.x].position.x += offset.x;
                newState.check.data[pos.y, pos.x].position.y += offset.y;
                newState.check.data[pos.y + offset.y, pos.x + offset.x] = newState.check.data[pos.y, pos.x];
                newState.check.data[pos.y, pos.x] = temp;
                newState.check.UnPossessedPos.x += offset.x;
                newState.check.UnPossessedPos.y += offset.y;

                newState.parent = state;
                newState.mhtDistance = ManhattanDistance(newState.check);
                newState.move = move;
                state.AddChild(newState);
                if (method == SolveMethod.Greedy)
                {
                    actions.Enqueue(newState, newState.mhtDistance);
                }
                else
                {
                    newState.cost = state.cost + 1;
                    newState.heuristicValue = newState.cost + newState.mhtDistance;
                    actions.Enqueue(newState, newState.heuristicValue);
                }
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

        LinkedList<Direction> GetPath(Node node)
        {
            var path = new LinkedList<Direction>();
            GetPathRecursion(node, ref path);
            return path;
        }

        void GetPathRecursion(Node node, ref LinkedList<Direction> path)
        {
            if (node.parent != null)
            {
                path.AddFirst(node.move);
                GetPathRecursion(node.parent, ref path);
            }
        }

    }
}