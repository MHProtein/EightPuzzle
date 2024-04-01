using System.Collections.Generic;

namespace EightPuzzle
{
    public class Node
    {
        public PriorityQueue<Node, int> children;
        public Check check;
        public Node parent = null;
        public int cost = 0;
        public int mhtDistance = 0;
        public int heuristicValue = 0;
        public Direction move;

        public Node(Check check)
        {
            this.check = check;
            children = new PriorityQueue<Node, int>();
            cost = 0;
            mhtDistance = 0;
            heuristicValue = 0;
        }

        public Node(Node node)
        {
            check = new Check(node.check);
            children = new PriorityQueue<Node, int>();
            cost = 0;
            mhtDistance = 0;
            heuristicValue = 0;
        }

        public void AddChild(Node child)
        {
            child.cost = cost + 1;
            child.heuristicValue += cost + mhtDistance;
            child.parent = this;
            children.Enqueue(child, child.heuristicValue);
        }
    }
}