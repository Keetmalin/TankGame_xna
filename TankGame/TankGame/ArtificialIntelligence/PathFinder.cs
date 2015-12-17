using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TankGame.ArtificialIntelligence
{
    class PathFinder
    {
        public static int playerLocatoin=0;
        public static List<int> coinLocations;
        private static string[,] map;

        public static List<int> getPath(int from, int to)
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

                if (map[j, i] == "W" || map[j, i] == "S" || map[j, i] == "B")
                {
                    parents[block] = -2;
                    continue;//do no add childs if it is a blocked cell
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

        public static String getMove()
        {

            coinLocations.Add(11);
            //Find the closest coin
            int currentLoc = playerLocatoin;
            int minLoc = currentLoc;
            int minsDist = 100;
            foreach (int coinLoc in coinLocations)
            {
                int dist = getPath(currentLoc, coinLoc).Count;
                if (dist < minsDist)
                {
                    minLoc = coinLoc;
                    minsDist = dist;
                }
            }


            int from = playerLocatoin;
            int to = minLoc;

            List<int> path = getPath(from, to);

            Console.WriteLine(path[0]);
            //choosing the side to turn
            if (path[0] - from == 10) return "RIGHT#";
            if (path[0] - from == -10) return "LEFT#";
            if (path[0] - from == 1) return "DOWN#";
            else return "UP#";
        }
    }
}
