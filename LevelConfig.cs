using System;
using System.Collections.Generic;
using System.Text;

namespace Sokoban
{
    class LevelConfig
    {
        public readonly static List<int[,]> MapList = new List<int[,]>();
        //        public readonly static List<int> ColumnList = new List<int>(), RowList = new List<int>();
        public readonly static List<int> TargetXList = new List<int>(), TargetYList = new List<int>();
        public readonly static List<int> AbilitySlotList = new List<int>();
        public readonly static List<List<string>> AbilityList = new List<List<string>>();
        public LevelConfig()
        {
            // 0 empty
            // 1 box
            // 2 player
            // 3 wall
            // 4 boom tile

            //Level 01
            MapList.Add(new int[,]
            {
                {3,3,0,3,3 },
                {3,3,0,3,3 },
                {3,3,0,3,3 },
                {3,3,0,3,3 },
                {3,3,2,3,3 }
            });
            //            ColumnList.Add(5);
            //            RowList.Add(5);
            TargetXList.Add(0);
            TargetYList.Add(2);
            AbilitySlotList.Add(1);
            AbilityList.Add(new List<string>() { "UP", "DOWN", "pull", "push" });

            //Level 02
            MapList.Add(new int[,]
{
                {1,0,0,0 },
                {0,1,1,0 },
                {0,1,1,0 },
                {2,0,0,1 }
});
            //            ColumnList.Add(4);
            //            RowList.Add(4);
            TargetXList.Add(0);
            TargetYList.Add(3);
            AbilitySlotList.Add(3);
            AbilityList.Add(new List<string>() { "UP", "DOWN", "pull", "push", "L", "R" });


            //Level 03
            MapList.Add(new int[,]
{
                {1,0,1,0,3 },
                {0,1,0,1,3 },
                {3,0,1,4,1 },
                {0,1,0,0,0 },
                {4,1,3,1,0 },
                {0,1,0,0,3 },
                {0,2,0,0,3 }
});
            //            ColumnList.Add(5);
            //            RowList.Add(7);
            TargetXList.Add(0);
            TargetYList.Add(1);
            AbilitySlotList.Add(5);
            AbilityList.Add(new List<string>() { "UP", "DOWN", "pull", "push", "L", "R", "explosion" });

            //Level 04
            MapList.Add(new int[,]
            {
                { 5,0,0,1,0 },
                { 0,0,0,1,0 },
                { 2,0,0,1,0 },
                { 0,0,0,1,0 },
                { 6,0,0,1,0 },
            });
            TargetXList.Add(2);
            TargetYList.Add(4);
            AbilitySlotList.Add(4);
            AbilityList.Add(new List<string>() { "UP", "DOWN", "L", "R", "pull" });

            MapList.Add(new int[,]
{
                { 0,2,1,0,0 },
                { 0,0,1,0,0 },
                { 1,1,5,1,1 },
                { 3,0,1,0,0 },
                { 3,3,1,0,6 },
});
            TargetXList.Add(3);
            TargetYList.Add(1);
            AbilitySlotList.Add(4);
            AbilityList.Add(new List<string>() { "UP", "DOWN", "L", "R", "pull" });
        }
    }
}
