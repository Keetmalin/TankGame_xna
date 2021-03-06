﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankGame
{
    /// <summary>
    /// Defined Constants of the Game
    /// </summary>
    public class Constant
    {
        #region "C2S - Client To Server"
        public const string C2S_INITIALREQUEST = "JOIN#";
        public const string UP = "UP#";
        public const string DOWN = "DOWN#";
        public const string LEFT = "LEFT#";
        public const string RIGHT = "RIGHT#";
        public const string SHOOT = "SHOOT#";
        #endregion

        #region "S2C - Server To Client"
        public const string S2C_DEL = "#";

        public const string S2C_GAMESTARTED = "GAME_ALREADY_STARTED#";
        public const string S2C_NOTSTARTED = "GAME_NOT_STARTED_YET#";
        public const string S2C_GAMEOVER = "GAME_HAS_FINISHED#";
        public const string S2C_GAMEJUSTFINISHED = "GAME_FINISHED#";

        public const string S2C_CONTESTANTSFULL = "PLAYERS_FULL#";
        public const string S2C_ALREADYADDED = "ALREADY_ADDED#";

        public const string S2C_INVALIDCELL = "INVALID_CELL#";
        public const string S2C_NOTACONTESTANT = "NOT_A_VALID_CONTESTANT#";
        public const string S2C_TOOEARLY = "TOO_QUICK#";
        public const string S2C_CELLOCCUPIED = "CELL_OCCUPIED#";
        public const string S2C_HITONOBSTACLE = "OBSTACLE#";//Penalty should be added.
        public const string S2C_FALLENTOPIT = "PITFALL";

        public const string S2C_NOTALIVE = "DEAD#";

        public const string S2C_REQUESTERROR = "REQUEST_ERROR";
        public const string S2C_SERVERERROR = "SERVER_ERROR";
        #endregion

        #region  "Map Cells"
        public const string EMPTY = "E";
        public const string WATER = "W";
        public const string BRICK = "B";
        public const string STONE = "S";
        public const string COIN = "C";
        public const string LIFE = "L";
        public const string PLAYER_0 = "P0";
        public const string PLAYER_1 = "P1";
        public const string PLAYER_2 = "P2";
        public const string PLAYER_3 = "P3";
        public const string PLAYER_4 = "P4";

        #endregion

        #region "Directions"
        public const string NORTH = "0";
        public const string EAST = "1";
        public const string SOUTH = "2";
        public const string WEST = "3";
        #endregion

        #region "Server Configurations"
        //        public static string SERVER_IP = ConfigurationManager.AppSettings.Get("ServerIP");
        //        public static int SERVER_PORT = int.Parse(ConfigurationManager.AppSettings.Get("ServerPort"));
        //        public static int CLIENT_PORT = int.Parse(ConfigurationManager.AppSettings.Get("ClientPort"));
        //        public static int LIFE_TIME = int.Parse(ConfigurationManager.AppSettings.Get("LifeTime"));
        //        public static double PLAYER_DELAY = double.Parse(ConfigurationManager.AppSettings.Get("PlayerDelay"));
        //        public static int BULLET_MULTI = int.Parse(ConfigurationManager.AppSettings.Get("BulletSpeedMulti"));
        //        public static int UPDATE_PERIOD = (int)(PLAYER_DELAY / BULLET_MULTI);

        //        public static int PLAYER_HEALTH = int.Parse(ConfigurationManager.AppSettings.Get("PlayerHealth"));
        //        public static int BRICK_POINTS = int.Parse(ConfigurationManager.AppSettings.Get("BrickPoints"));
        //        public static int PLUNDER_TREASUR_LIFE = int.Parse(ConfigurationManager.AppSettings.Get("PlunderCoinPileLife"));








        public static int MAP_SIZE = 10;
        //        public static int MAX_BRICKS = int.Parse(ConfigurationManager.AppSettings.Get("MaxBricks"));
        //        public static int MAX_OBSTACLES = int.Parse(ConfigurationManager.AppSettings.Get("MaxObs"));
        //        public static int CoinPile_RATE = int.Parse(ConfigurationManager.AppSettings.Get("CoinPileRate"));
        //        public static int LIFEPACK_RATE = int.Parse(ConfigurationManager.AppSettings.Get("LifePackRate"));
        //        public static int AI_FACTOR = int.Parse(ConfigurationManager.AppSettings.Get("AI"));

        #endregion
    }

    public enum DirectionConstants
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3,

    }

    enum CellType
    {
        Tank,
        Brick,
        Stone,
        Water,
        Bullet,
        CoinPile,
        Lifepack,
        Empty
    }
}
