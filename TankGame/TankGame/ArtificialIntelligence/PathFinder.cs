using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TankGame.ArtificialIntelligence
{
    class PathFinder
    {
        public static int myLocation = MsgParser.myLocation;
        public static List<int> coinLocations = MsgParser.coinLocations;
        private static string[,] map = MsgParser.map;

        //-------Method to find shortest path
        public static List<int> findPath(int from, int to)
        {
            List<int> path = new List<int>();

            //Mark cells visited when we visit them, mark with parent cell's id
            int[] parents = new int[100];
            for (int i = 0; i < 100; i++)
                parents[i] = -1;

            //Queue to add cell while flooding
            Queue<int> queue = new Queue<int>();
            queue.Enqueue(from);

            while (queue.Count > 0)
            {

                int block = queue.Dequeue();
                int i = block / 10, j = block % 10;

                if (map[i,j] == "W" || map[i,j] == "S" || map[i,j] == "B")
                {
                    parents[block] = -2;
                    continue;
                }

                if (i > 0)
                {
                    int tmp = (i - 1) * 10 + j;
                    if (parents[tmp] == -1)
                    {
                        parents[tmp] = block;
                        queue.Enqueue(tmp);
                    }
                }
                if (j > 0)
                {
                    int tmp = (i) * 10 + j - 1;
                    if (parents[tmp] == -1)
                    {
                        parents[tmp] = block;
                        queue.Enqueue(tmp);
                    }
                }
                if (i < 9)
                {
                    int tmp = (i + 1) * 10 + j;
                    if (parents[tmp] == -1)
                    {
                        parents[tmp] = block;
                        queue.Enqueue(tmp);
                    }
                }
                if (j < 9)
                {
                    int tmp = (i) * 10 + j + 1;
                    if (parents[tmp] == -1)
                    {
                        parents[tmp] = block;
                        queue.Enqueue(tmp);
                    }
                }
            }

            //Return null if we can't find a possible path
            if (parents[to] < 0) return path;


            //Find the path
            int movingCell = to;
            while (movingCell != from)
            {
                path.Add(movingCell);
                movingCell = parents[movingCell];
            }
            path.Reverse();
            return path;
        }
    }
}
