using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TankGame.ArtificialIntelligence
{
    class AI
    {

        public AI()
        {

        }

        public string nextCommand()
        //method to generate the next command to be sent to the server
        {
            string command = "";
            command = nextMoveToGetCoins();
            return command;
        }

        private string nextMoveToGetCoins()
        //method to generate the next command to go to the nearest coin pile
        {
            string move = "";

            int currentLoc = PathFinder.myLocation;
            int destination = currentLoc;
            int distanceToTravel = 99;

            foreach (int coinLoc in PathFinder.coinLocations)
            {
                int distance = PathFinder.findPath(currentLoc, coinLoc).Count;
                if (distance < distanceToTravel)
                {
                    destination = coinLoc;
                    distanceToTravel = distance;
                }
            }


            int from = PathFinder.myLocation;
            int to = distanceToTravel;

            List<int> path = PathFinder.findPath(from, to);

            try
            {
                if (path[0] - from == 10)
                    move = "RIGHT#";
                if (path[0] - from == -10)
                    move = "LEFT#";
                if (path[0] - from == 1)
                    move = "DOWN#";
                else
                    move = "UP#";

            }
            catch (ArgumentOutOfRangeException e) { }
            return move;
        }
    }
}
