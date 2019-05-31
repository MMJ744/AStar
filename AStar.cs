using System;
using System.Collections.Generic;

namespace AStar
{
    
    public static class AStar
    {
        static void Main(string[] args)
        {
            Console.WriteLine(AStar.Test());

            Console.ReadLine();
        }
        public static List<Vector2> FindShortestPath(List<Vector2> map, Vector2 start, Vector2 goal)
        {
            var path = new List<Vector2>();
            var openList = new List<Node>();
            Node current = null;
            foreach (var v in map.ToArray()) //First create nodes
            {
                var n = new Node { G = int.MaxValue, H = (goal - v).magnitude(), L = v, F = int.MaxValue, Parent = null, Goal = v == goal };

                if (v == start) current = n;
                else openList.Add(n);
            }
            if (current == null) return null;
            while (!current.Goal && openList.Count > 0) //Until goal reached or no path found
            {
                var neighbours = GetNeighbours(current, openList);
                foreach (var neighbour in neighbours) //Calculate F values for neighbours
                {
                    var temp = 1 + current.G + neighbour.H;
                    if (temp >= neighbour.F) continue;
                    neighbour.F = temp;
                    neighbour.Parent = current;
                }
                openList.Sort((x, y) => x.F.CompareTo(y.F));
                current = openList[0]; //Take node with lowest F value from the list
                openList.RemoveAt(0);
            }
            if (!current.Goal) return null; //No path found
            path.Add(current.L);
            while ((current = current.Parent) != null) //Construct the path starting from goal
            {
                path.Add(current.L);
            }
            path.Reverse();
            return path;
        }

        private static int CalcH(Vector2 n, Vector2 g) //Applies the heuristic to calculate H
        {
            return (int)(g - n).magnitude(); //Using the Manhattan Heuristic
        }

        private static List<Node> GetNeighbours(Node n, List<Node> map) //Gets all neighbouring nodes
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            return map.FindAll(x => (x.L - n.L).sqrMagnitue() == 1);
        }

        public static bool Test()
        {
            //Test Neighbour function
            var n1 = new Node { L = new Vector2(1, 0) };
            var n2 = new Node { L = new Vector2(0, 1) };
            var list = new List<Node>
            {
                new Node {L = new Vector2(0, 0)}, n1, n2, new Node {L = new Vector2(1, 1)},
                new Node {L = new Vector2(2, 2)}, new Node {L = new Vector2(3, 3)}
            };
            var n = new Node { L = new Vector2(0, 0) };
            List<Node> x = GetNeighbours(n, list);
            var passed = x.Contains(n1) && x.Contains(n2);
            var map = new List<Vector2>();
            map.Add(new Vector2(0, 0));
            map.Add(new Vector2(0, 1));
            map.Add(new Vector2(0, 2));
            map.Add(new Vector2(0, 3));
            map.Add(new Vector2(0, 4));
            map.Add(new Vector2(1, 2));
            map.Add(new Vector2(2, 2));
            map.Add(new Vector2(2, 3));
            map.Add(new Vector2(2, 4));
            map.Add(new Vector2(2, 0));
            map.Add(new Vector2(2, 1));
            var path = FindShortestPath(map, new Vector2(0, 0), new Vector2(2, 4));
            passed = passed && path != null;
            Console.WriteLine(string.Join<Vector2>(",",path.ToArray()));
            return passed;
        }
    }

    internal class Node
    {
        public Vector2 L;
        public int G;
        public int H;
        public int F;
        public bool Goal;
        public Node Parent;
    }

    public class Vector2
    {
        public int x, y;

        public int magnitude()
        {
            return (int) Math.Sqrt(x * x + y * y);
        }

        public int sqrMagnitue()
        {
            return x * x + y * y;
        }

        public Vector2(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
        public Vector2() { }

        public static Vector2 operator- (Vector2 v)
        {
            return new Vector2(-v.x, -v.y);
        }

        public static Vector2 operator- (Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x - v2.x, v1.y - v2.y);
        }

        public static bool operator== (Vector2 v1, Vector2 v2)
        {
            return v1.x == v2.x && v1.y == v2.y;
        }

        public static bool operator !=(Vector2 v1, Vector2 v2)
        {
            return v1.x != v2.x || v1.y != v2.y;
        }

        public override string ToString()
        {
            return "(" + x + "," + y+")";
        }
    }

}