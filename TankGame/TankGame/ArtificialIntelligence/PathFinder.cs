using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TankGame.ArtificialIntelligence
{
    class PathFinder
    {
        //Map ActorMap;
        const int width = Constant.MAP_SIZE;
        const int height = Constant.MAP_SIZE;
        AINode[,] map = new AINode[width, height];
        int startX, startY, endX, endY;
        List<AINode> openList, closedList;
        bool finishedCalculation;
        //AIManager.AIMode mode;
        Stack<Point> path;
        int pathLength;
        DirectionConstants currentDirection;

        internal Stack<Point> Path
        {
            get
            {
                return path;
            }
        }

        public PathFinder(Map map)
        {
            this.ActorMap = map;
            openList = new List<AINode>();
            closedList = new List<AINode>();
            mode = AIManager.AIMode.Greedy;
        }
        /// <summary>
        /// Generate a map containing the nodes using the actors map
        /// </summary>
        private void generateMap()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Actor actor = ActorMap.getActor(i, j);
                    AINode node = new AINode();
                    node.X = i;
                    node.Y = j;
                    if (actor != null)
                    {
                        if (actor.GetType() == typeof(Brick))
                        {
                            node.Type = NodeType.Brick;
                            node.Walkable = true;
                        }
                        else if (actor.GetType() == typeof(Tank))
                        {
                            node.Type = NodeType.Tank;
                            node.Walkable = true;
                        }
                        else if (actor.GetType() == typeof(CoinPile))
                        {
                            node.Type = NodeType.Coins;
                            node.Walkable = true;
                        }
                        else if (actor.GetType() == typeof(Stone))
                        {
                            node.Type = NodeType.Stone;
                            node.Walkable = false;
                        }
                        else if (actor.GetType() == typeof(LifePack))
                        {
                            node.Type = NodeType.Lifepack;
                            node.Walkable = true;
                        }
                        else if (actor.GetType() == typeof(Water))
                        {
                            node.Type = NodeType.Water;
                            node.Walkable = false;
                        }
                    }
                    else
                    {
                        node.Type = NodeType.Empty;
                        node.Walkable = true;
                    }
                    map[i, j] = node;
                }
            }
        }

        /// <summary>
        /// Generates shortest path
        /// </summary>
        /// <param name="x">x coordinate of the destination</param>
        /// <param name="y">y coordinate of the destination</param>
        public Stack<Point> generateShortestPath(int x, int y)
        {
            openList.Clear();
            closedList.Clear();

            Stack<Point> path;

            endX = x;
            endY = y;
            currentDirection = ActorMap.ClientDirection;
            finishedCalculation = false;

            switch (Mode)// adjust for the AI mode
            {
                case AIManager.AIMode.Defensive:
                    NodeType.Lifepack = 2;
                    NodeType.Coins = 5;
                    NodeType.Tank = 5;
                    break;
                case AIManager.AIMode.Greedy:
                    NodeType.Coins = 2;
                    NodeType.Lifepack = 5;
                    NodeType.Tank = 5;
                    break;
                case AIManager.AIMode.Offensive:
                    NodeType.Lifepack = 4;
                    NodeType.Coins = 5;
                    NodeType.Tank = 2;
                    break;
            }

            //generates the map needed for the calculation
            generateMap();

            //setting the start point for the calculation
            setStartNode();

            openList.Add(map[startX, startY]);

            while (!finishedCalculation)
            {
                analyzeNodeNeighbours();
            }
            path = findPath();
            this.path = path;
            return path;
        }

        /// <summary>
        /// back-trace the path from the target to the source
        /// </summary>
        /// <returns>A List of points containing the path</returns>
        private Stack<Point> findPath()
        {
            Stack<Point> path = new Stack<Point>();
            AINode current = map[endX, endY];// gets the target node

            while (current != null && (current.X != startX || current.Y != startY))// if current node is null or the current node where the client is at terminate the loop
            {
                Point point = new Point(current.X, current.Y);
                point.FirstDirection = current.DirectionAtNode;
                point.LastDirection = current.LastDirectionAtNode;
                path.Push(point);// adds the coordinates of the current node
                current = current.Parent;// set the next check to its parent
            }
            this.path = path;
            System.Console.WriteLine("Target : " + endX + "," + endY + " ");
            foreach (Point point in path.ToArray())
            {
                System.Console.Write("<" + point.X + " " + point.Y + ">");
            }
            System.Console.WriteLine();
            return path;
        }

        /// <summary>
        /// Sorts the given list by F value
        /// </summary>
        /// <param name="nodes"> list of nodes to be sorted</param>
        private void sortList(List<AINode> nodes)
        {
            nodes.Sort(sortListByFValue);
        }
        private void analyzeNodeNeighbours()
        {

            if (openList.Count == 0) // no path exists and the calculation will stop
            {
                finishedCalculation = true;
            }
            AINode currentNode = openList.ElementAt(0);
            if (!openList.Remove(currentNode))
            {
                throw new InvalidOperationException("Node is not found in the list");
            }
            closedList.Add(currentNode);
            if (currentNode.X == endX && currentNode.Y == endY) // shortest path has been found
            {
                finishedCalculation = true;
            }
            AINode tempNode;
            if (currentNode.X - 1 >= 0)
            {
                tempNode = map[currentNode.X - 1, currentNode.Y];
                analyzeNode(currentNode, tempNode);
            }
            if (currentNode.Y - 1 >= 0)
            {
                tempNode = map[currentNode.X, currentNode.Y - 1];
                analyzeNode(currentNode, tempNode);
            }
            if (currentNode.Y + 1 < height)
            {
                tempNode = map[currentNode.X, currentNode.Y + 1];
                analyzeNode(currentNode, tempNode);
            }
            if (currentNode.X + 1 < width)
            {
                tempNode = map[currentNode.X + 1, currentNode.Y];
                analyzeNode(currentNode, tempNode);
            }
            sortList(openList);
        }

        /// <summary>
        /// Analyzes a single node and calculate G and H values if neccessary
        /// </summary>
        /// <param name="currentNode"></param>
        /// <param name="tempNode"></param>
        private void analyzeNode(AINode currentNode, AINode tempNode)
        {
            if (tempNode.Walkable && !closedList.Contains(tempNode))// check if the node is walkable and not in the closed list
            {
                int g = currentNode.G + tempNode.Type;
                g += CalculateRotationDelay(currentNode, tempNode);

                int h = calculateHValueFor(tempNode.X, tempNode.Y);
                if (!openList.Contains(tempNode))
                {
                    tempNode.Parent = currentNode;
                    setDirectionAtNode(tempNode);
                    tempNode.G = g;
                    tempNode.H = h;
                    openList.Add(tempNode);
                }
                else
                {
                    if (g < tempNode.G)
                    {
                        tempNode.Parent = currentNode;
                        setDirectionAtNode(tempNode);
                        tempNode.G = g;
                        tempNode.H = h;
                    }
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentNode"></param>
        /// <param name="tempNode"></param>
        /// <returns></returns>
        private int CalculateRotationDelay(AINode currentNode, AINode tempNode)
        {
            int g = 0;
            if (currentNode.DirectionAtNode == DirectionConstants.Down)
            {
                if (currentNode.X != tempNode.X)
                {
                    g += NodeType.Empty;

                }
                else if (currentNode.Y > tempNode.Y)
                {
                    g += NodeType.Empty;
                }
            }
            else if (currentNode.DirectionAtNode == DirectionConstants.Up)
            {
                if (currentNode.X != currentNode.X)
                {
                    g += NodeType.Empty;
                }
                else if (currentNode.Y < tempNode.Y)
                {
                    g += NodeType.Empty;
                }
            }
            else if (currentNode.DirectionAtNode == DirectionConstants.Left)
            {
                if (currentNode.Y != currentNode.Y)
                {
                    g += NodeType.Empty;
                }
                else if (currentNode.X < tempNode.X)
                {
                    g += NodeType.Empty;
                }
            }
            else if (currentNode.DirectionAtNode == DirectionConstants.Right)
            {
                if (currentNode.Y != currentNode.Y)
                {
                    g += NodeType.Empty;
                }
                else if (currentNode.X > tempNode.X)
                {
                    g += NodeType.Empty;
                }
            }
            return g;
        }
        private void setDirectionAtNode(AINode tempNode)
        {
            AINode currentNode = tempNode.Parent;
            if (currentNode == null)
            {
                tempNode.DirectionAtNode = currentDirection;
                return;
            }
            if (tempNode.X > currentNode.X)
            {
                tempNode.DirectionAtNode = DirectionConstants.Right;
                currentNode.LastDirectionAtNode = DirectionConstants.Right;
            }
            else if (tempNode.X < currentNode.X)
            {
                tempNode.DirectionAtNode = DirectionConstants.Left;
                currentNode.DirectionAtNode = DirectionConstants.Left;
            }
            else if (tempNode.Y > currentNode.Y)
            {
                tempNode.DirectionAtNode = DirectionConstants.Down;
                currentNode.DirectionAtNode = DirectionConstants.Down;
            }
            else
            {
                tempNode.DirectionAtNode = DirectionConstants.Up;
                currentNode.DirectionAtNode = DirectionConstants.Up;
            }
        }
        private int calculateHValueFor(int x, int y)
        {
            int xLength = endX - x;
            int yLength = endY - y;

            //gets the absolute values in case they are negative
            if (xLength < 0)
                xLength = 0 - xLength;
            if (yLength < 0)
                yLength = 0 - yLength;

            return xLength + yLength;
        }
        private void setStartNode()
        {
            startX = ActorMap.ClientX;
            startY = ActorMap.ClientY;
        }

        private int sortListByFValue(AINode first, AINode second)
        {
            int returnValue = 0;

            returnValue = first.F - second.F;
            if (returnValue == 0)
            {
                returnValue = first.G - second.G;
            }
            if (returnValue == 0)
            {
                returnValue = first.H - second.H;
            }
            return returnValue;
        }
        public AIManager.AIMode Mode
        {
            get
            {
                return mode;
            }
            set
            {
                mode = value;
            }
        }
        public void setActorMap(Map map)
        {
            this.ActorMap = map;
        }

        public int calculatePathLength()
        {
            pathLength = 0;
            Point[] points = path.ToArray();
            foreach (Point point in points)
            {
                pathLength += map[point.X, point.Y].F;
            }
            return pathLength;
        }

        internal Stack<AICell> CalculateShortestPath(AICell target, AICell current)
        {

            this.endX = target.X;
            this.endY = target.Y;
            this.startX = current.X;
            this.startY = current.Y;

            Stack<Point> pathPoints = generateShortestPath(endX, endY);
            Stack<AICell> pathCells = new Stack<AICell>();
            foreach (Point point in pathPoints)
            {
                AICell cell = new AICell();
                cell.X = point.X;
                cell.Y = point.Y;
                pathCells.Push(cell);
            }
            Stack<AICell> temp = new Stack<AICell>();
            foreach (AICell cell in pathCells)
            {
                temp.Push(cell);
            }
            //pathCells.Reverse();
            return temp;
            // return pathCells;
        }
    }
    class AINode
    {

        #region Properties
        DirectionConstants firstDirectionAtNode, lastDirectionAtNode;
        int gValue, hValue;// values from Dijkstra's,Heuristics and the sum of them
        int x, y;
        AINode parent;
        int nodeType;
        bool walkable;

        /// <summary>
        /// determine whether the node is walkable
        /// </summary>
        public bool Walkable
        {
            get
            {
                return walkable;
            }
            set
            {
                walkable = value;
            }
        }

        public DirectionConstants LastDirectionAtNode
        {
            get
            {
                return lastDirectionAtNode;
            }
            set
            {
                lastDirectionAtNode = value;
            }
        }
        public DirectionConstants DirectionAtNode
        {
            get
            {
                return firstDirectionAtNode;
            }
            set
            {
                firstDirectionAtNode = value;
            }
        }
        /// <summary>
        /// Parent of the node
        /// </summary>
        public AINode Parent
        {
            get
            {
                return parent;
            }
            set
            {
                parent = value;
            }
        }

        /// <summary>
        /// Type of the node
        /// </summary>
        public int Type
        {
            get
            {
                return nodeType;
            }
            set
            {
                nodeType = value;
            }
        }
        /// <summary>
        /// Y value of the node
        /// </summary>
        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

        /// <summary>
        /// Value from Dijkstra's algorithm applied
        /// </summary>
        public int G
        {
            get
            {
                return gValue;
            }
            set
            {
                gValue = value;
            }
        }
        /// <summary>
        /// Value from the heuristics
        /// </summary>
        public int H
        {
            get
            {
                return hValue;
            }
            set
            {
                hValue = value;
            }
        }
        /// <summary>
        /// Value from the sum of G and H
        /// </summary>
        public int F
        {
            get
            {
                return G + H;
            }
        }
        /// <summary>
        /// X value of the node
        /// </summary>
        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        #endregion
    }
    class NodeType
    {
        public static int Brick = 250,//has to shoot 4 times to destroy plus the movement to the new empty cell
        Empty = 50,
        Coins = 1,
        Lifepack = 10,
        Tank = 100,
        Stone = 100000,
        Water = 100000,
        Bullet = 10000;
    }

}
