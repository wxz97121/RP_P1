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
            AbilityList.Add(new List<string>() { "Up", "Down", "Pull", "Multi" });

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
            AbilityList.Add(new List<string>() { "Up", "Down", "Pull", "Multi", "Left", "Right" });


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
            AbilityList.Add(new List<string>() { "Up", "Down", "Pull", "Multi", "Left", "Right", "Boom" });


        }
    }
}
