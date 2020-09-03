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
        public readonly static List<int> MinStepsList = new List<int>();
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
                {3,3,3,3,3 },
                {3,3,0,3,3 },
                {3,3,0,3,3 },
                {3,3,0,3,3 },
                {3,3,0,3,3 },
                {3,3,0,3,3 },
                {3,3,2,3,3 },
                {3,3,3,3,3 },
            });
            TargetXList.Add(1);
            TargetYList.Add(2);
            AbilitySlotList.Add(1);
            MinStepsList.Add(5);
            AbilityList.Add(new List<string>() { "UP", "DOWN", "pull", "push" });

            //Level 02
            MapList.Add(new int[,]
{               {3,3,3,3,3,3 },
                {3,1,0,0,0,3 },
                {3,0,1,1,0,3 },
                {3,0,1,1,0,3 },
                {3,2,0,0,1,3 },
                {3,3,3,3,3,3 },
});
            TargetXList.Add(1);
            TargetYList.Add(4);
            AbilitySlotList.Add(3);
            MinStepsList.Add(6);
            AbilityList.Add(new List<string>() { "UP", "DOWN", "L", "R", "pull", "push" });

            //Level 03
            MapList.Add(new int[,]
            {
                {3,3,3,3,3,3,3 },
                {3,5,0,0,1,0,3 },
                {3,0,0,0,1,0,3 },
                {3,2,0,0,1,0,3 },
                {3,0,0,0,1,0,3 },
                {3,6,0,0,1,0,3 },
                {3,3,3,3,3,3,3 },
            });
            TargetXList.Add(3);
            TargetYList.Add(5);
            AbilitySlotList.Add(4);
            MinStepsList.Add(11);
            AbilityList.Add(new List<string>() { "UP", "DOWN", "L", "R", "pull", "push" });

            //Level 04
            MapList.Add(new int[,]
{               { 3,3,3,3,3,3,3 },
                { 3,0,2,1,0,0,3 },
                { 3,0,0,1,0,0,3 },
                { 3,1,1,5,1,1,3 },
                { 3,3,0,1,0,0,3 },
                { 3,3,3,1,0,6,3 },
                { 3,3,3,3,3,3,3 },
});
            TargetXList.Add(4);
            TargetYList.Add(2);
            AbilitySlotList.Add(4);
            MinStepsList.Add(8);
            AbilityList.Add(new List<string>() { "UP", "DOWN", "L", "R", "pull" });
            //Level 05
            MapList.Add(new int[,]

{               {3,3,3,3,3,3,3,3,3 },
                {3,7,0,0,1,0,0,6,3 },
                {3,0,0,0,0,0,0,0,3 },
                {3,0,0,1,1,1,0,0,3 },
                {3,0,0,1,2,1,0,0,3 },
                {3,0,0,1,1,1,0,0,3 },
                {3,0,0,1,1,1,0,0,3 },
                {3,3,5,1,0,1,8,3,3 },
                {3,3,3,3,3,3,3,3,3 }
});
            TargetXList.Add(7);
            TargetYList.Add(4);
            AbilitySlotList.Add(3);
            MinStepsList.Add(13);
            AbilityList.Add(new List<string>() { "UP", "DOWN", "L", "R", "pull", "push", "explosion" });

            //Level 06
            MapList.Add(new int[,]
{               {3,3,3,3,3,3,3 },
                {3,1,0,1,0,3,3 },
                {3,0,1,0,1,3,3 },
                {3,3,0,1,4,1,3 },
                {3,0,1,0,0,0,3 },
                {3,4,1,3,1,0,3 },
                {3,0,1,0,0,3,3 },
                {3,0,2,0,0,3,3 },
                {3,3,3,3,3,3,3 }
});
            TargetXList.Add(1);
            TargetYList.Add(2);
            AbilitySlotList.Add(4);
            MinStepsList.Add(12);
            AbilityList.Add(new List<string>() { "UP", "DOWN", "L", "R", "pull", "push", "explosion" });


            //Level 07
            MapList.Add(new int[,]
{               {3,3,3,3,3,3,3 },
                {3,1,0,1,0,3,3 },
                {3,0,1,0,1,3,3 },
                {3,3,0,1,4,1,3 },
                {3,0,1,0,0,0,3 },
                {3,4,1,3,1,0,3 },
                {3,0,1,0,0,3,3 },
                {3,0,2,0,0,3,3 },
                {3,3,3,3,3,3,3 },
});
            TargetXList.Add(1);
            TargetYList.Add(2);
            AbilitySlotList.Add(5);
            MinStepsList.Add(32);
            AbilityList.Add(new List<string>() { "UP", "DOWN", "L", "R", "pull", "push" });

        }
    }
}
