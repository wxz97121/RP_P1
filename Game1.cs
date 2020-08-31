using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sokoban
{
    public class Game1 : Game
    {
        public static SpriteBatch m_SpriteBatch;
        public static ContentManager m_Content;
        //public static int BtnWidth = 200, BtnHeight = 200;

        // private Stack<int[,]> history = new Stack<int[,]>();

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        List<Texture2D> GameSprite;
        List<Texture2D> PlayerSprite;
        Texture2D TargetSprite;
        int PlayerX, PlayerY; // where the player is.
        int TargetX, TargetY;
        int[,] NowMap;
        public List<int[,]> History;
        int Row, Column, NowLevelIndex;
        int MaxAblitiesNum = 1;
        int NowDir = 0;
        LevelConfig _LevelData;

        int NowAblitiesChosen = 0;
        public List<Button> AbilityBtn;

        public Button restartBtn;
        /*
        public Button UpBtn, DownBtn, LeftBtn, RightBtn, PullBtn, MultiBtn;
        public Texture2D UpSprite, DownSprite, LeftSprite, RightSprite, PullSprite, MultiSprite;
        */

        bool CanUp = false, CanDown = false, CanLeft = false, CanRight = false, CanPull = false, CanPushMulti = false, CanDestroy = false;

        private readonly int[] dx = { -1, 1, 0, 0 };
        private readonly int[] dy = { 0, 0, -1, 1 };
        //0 up 1 down 2 left 3 right

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            m_Content = Content;
            IsMouseVisible = true;
        }
        bool CheckForWin()
        {
            //return false;
            if (PlayerX == TargetX && PlayerY == TargetY)
                return true;
            return false;
        }
        void Undo()
        {
            if (History.Count <= 1)
                return;
            History.RemoveAt(History.Count - 1);
            NowMap = History[History.Count - 1].Clone() as int[,];

            UpdatePlayerPos();
            return;
            //TODO: Undo feature
        }
        void ProcessPull(int nowx, int nowy, int Dir)
        {
            if (!CheckIfInRange(nowx - dx[Dir], nowy - dy[Dir])) return;
            if (NowMap[nowx - dx[Dir], nowy - dy[Dir]] == 1)
            {
                NowMap[nowx, nowy] = 1;
                NowMap[nowx - dx[Dir], nowy - dy[Dir]] = 0;
            }
        }
        bool CheckIfInRange(int x, int y)
        {
            if (x < 0 || x >= Row || y < 0 || y >= Column) return false;
            return true;
        }
        bool Move(int nowx, int nowy, int Dir)
        {
            int tarx = nowx + dx[Dir];
            int tary = nowy + dy[Dir];
            if (!CheckIfInRange(tarx, tary)) return false;
            //Debug.WriteLine(tarx.ToString()+" "+tary.ToString());

            // 0 empty
            // 1 box
            // 2 player
            // 3 wall
            // 4 rocks
            if (NowMap[tarx, tary] == 0)
            {
                NowMap[tarx, tary] = NowMap[nowx, nowy];
                NowMap[nowx, nowy] = 0;
                if (CanPull) ProcessPull(nowx, nowy, Dir);
                return true;


            }
            else if (NowMap[tarx, tary] == 1)
            {
                if (!CanPushMulti)
                {

                    if (CheckIfInRange(tarx + dx[Dir], tary + dy[Dir]) && NowMap[tarx + dx[Dir], tary + dy[Dir]] == 0)
                    {
                        NowMap[tarx + dx[Dir], tary + dy[Dir]] = NowMap[tarx, tary];
                        NowMap[tarx, tary] = NowMap[nowx, nowy];
                        NowMap[nowx, nowy] = 0;
                        if (CanPull) ProcessPull(nowx, nowy, Dir);
                        return true;
                    }
                }
                else
                {
                    if (Move(tarx, tary, Dir))
                    {
                        NowMap[tarx, tary] = NowMap[nowx, nowy];
                        NowMap[nowx, nowy] = 0;
                        if (CanPull) ProcessPull(nowx, nowy, Dir);
                        return true;
                    }
                }

            }
            else if (NowMap[tarx, tary] == 4)
            {
                if (CanDestroy)
                {
                    NowMap[tarx, tary] = NowMap[nowx, nowy];
                    NowMap[nowx, nowy] = 0;
                    return true;

                }

            }
            return false;

        }
        void LoadLevel(int num)
        {
            if (num >= LevelConfig.MapList.Count || num < 0) return;
            NowMap = (int[,])LevelConfig.MapList[num].Clone();
            History = new List<int[,]>();
            History.Add(NowMap.Clone() as int[,]);
            Row = NowMap.GetLength(0);
            Column = NowMap.GetLength(1);
            /*
            Row = LevelConfig.RowList[num];
            Column = LevelConfig.ColumnList[num];
            */
            TargetX = LevelConfig.TargetXList[num];
            TargetY = LevelConfig.TargetYList[num];
            NowAblitiesChosen = 0;
            MaxAblitiesNum = LevelConfig.AbilitySlotList[num];
            AbilityBtn = new List<Button>();
            restartBtn = new Button("restart", 400, 100);

            CanUp = CanDown = CanLeft = CanRight = CanPull = CanPushMulti = CanDestroy = false;

            NowDir = 0;
            for (int i = 0; i < LevelConfig.AbilityList[num].Count; i++)
            {
                AbilityBtn.Add(new Button(LevelConfig.AbilityList[num][i], 550, 64 * i));
            }


            UpdatePlayerPos();
            NowLevelIndex = num;

        }
        void UpdatePlayerPos()
        {
            for (int i = 0; i < Row; i++)
                for (int j = 0; j < Column; j++)
                    if (NowMap[i, j] == 2)
                    {
                        PlayerX = i;
                        PlayerY = j;
                        return;
                    }
        }

        protected override void Initialize()
        {
            _LevelData = new LevelConfig();
            GameSprite = new List<Texture2D>();
            NowLevelIndex = 0;
            PlayerSprite = new List<Texture2D>();
            LoadLevel(0);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            m_SpriteBatch = _spriteBatch;
            GameSprite.Add(Content.Load<Texture2D>("Sprite/white_space"));//space1
            GameSprite.Add(Content.Load<Texture2D>("Sprite/box 2x"));//box
            GameSprite.Add(Content.Load<Texture2D>("Sprite/body 2x"));
            GameSprite.Add(Content.Load<Texture2D>("Sprite/black_space"));//blackspace
            GameSprite.Add(Content.Load<Texture2D>("Sprite/rock 2x"));//rock

            TargetSprite = Content.Load<Texture2D>("Sprite/flag 2x");//flag

            PlayerSprite.Add(Content.Load<Texture2D>("Sprite/body back 2x"));
            PlayerSprite.Add(Content.Load<Texture2D>("Sprite/body 2x"));
            PlayerSprite.Add(Content.Load<Texture2D>("Sprite/body Lside 2x"));
            PlayerSprite.Add(Content.Load<Texture2D>("Sprite/body side 2x"));

        }
        private KeyboardState PreviousState;
        private MouseState PreviousMouseState;
        void UpdateState()
        {
            PreviousState = Keyboard.GetState();
            PreviousMouseState = Mouse.GetState();
        }
        bool CheckPressed(Keys keys)
        {
            if (Keyboard.GetState().IsKeyDown(keys) && PreviousState.IsKeyUp(keys)) return true;
            return false;
        }
        bool CheckLMBClicked()
        {
            if (Mouse.GetState().LeftButton.Equals(ButtonState.Pressed) && PreviousMouseState.LeftButton.Equals(ButtonState.Released)) return true;
            return false;
        }
        void OnButtonClicked(string name)
        {
            switch (name)
            {
                case "UP":
                    CanUp = !CanUp;
                    break;
                case "DOWN":
                    CanDown = !CanDown;
                    break;
                case "L":
                    CanLeft = !CanLeft;
                    break;
                case "R":
                    CanRight = !CanRight;
                    break;
                case "pull":
                    CanPull = !CanPull;
                    break;
                case "push":
                    CanPushMulti = !CanPushMulti;
                    break;
                case "explosion":
                    CanDestroy = !CanDestroy;
                    break;
                default:
                    break;

            }
        }
        bool DestroyBox(int nowx, int nowy, int Dir)
        {
            int tarx = nowx + dx[Dir];
            int tary = nowy + dy[Dir];
            Debug.WriteLine(tarx.ToString() + " " + tary.ToString() + " " + NowMap[tarx, tary].ToString());
            if (!CheckIfInRange(tarx, tary)) return false;
            if (NowMap[tarx, tary] == 4)
            {
                NowMap[tarx, tary] = 0;
                return true;
            }
            return false;
        }
        void UpdateKeyboardInput()
        {

            if (CheckPressed(Keys.Up) && CanUp)
                if (Move(PlayerX, PlayerY, 0))
                {
                    NowDir = 0;
                    History.Add(NowMap.Clone() as int[,]);
                }
            if (CheckPressed(Keys.Down) && CanDown)
                if (Move(PlayerX, PlayerY, 1))
                {
                    NowDir = 1;
                    History.Add(NowMap.Clone() as int[,]);
                }
            if (CheckPressed(Keys.Left) && CanLeft)

                if (Move(PlayerX, PlayerY, 2))
                {
                    NowDir = 2;
                    History.Add(NowMap.Clone() as int[,]);
                }
            if (CheckPressed(Keys.Right) && CanRight)
                if (Move(PlayerX, PlayerY, 3))
                {
                    NowDir = 3;
                    History.Add(NowMap.Clone() as int[,]);
                }
            if (CheckPressed(Keys.E) && CanDestroy)
                DestroyBox(PlayerX, PlayerY, NowDir);

            if (CheckPressed(Keys.R)) LoadLevel(NowLevelIndex);
            if (CheckPressed(Keys.Z)) Undo();
            if (CheckPressed(Keys.P)) LoadLevel(NowLevelIndex - 1);
            if (CheckPressed(Keys.N)) LoadLevel(NowLevelIndex + 1);
        }
        void UpdateMouseInput()
        {
            if (CheckLMBClicked())
            {
                for (int i = 0; i < AbilityBtn.Count; i++)
                    if (AbilityBtn[i].enterButton())
                    {
                        if (AbilityBtn[i].isChecked)
                        {
                            NowAblitiesChosen--;
                            AbilityBtn[i].ToogleChecked();
                            OnButtonClicked(AbilityBtn[i].Name);
                        }
                        else if (NowAblitiesChosen < MaxAblitiesNum)
                        {
                            NowAblitiesChosen++;
                            AbilityBtn[i].ToogleChecked();
                            OnButtonClicked(AbilityBtn[i].Name);
                        }
                    }
                if (restartBtn.enterButton())
                {

                    LoadLevel(NowLevelIndex);
                }

            }
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            UpdateKeyboardInput();
            UpdateMouseInput();


            //TODO: Lock the abilities after player start moving.
            UpdateState();
            UpdatePlayerPos();
            if (CheckForWin())
                LoadLevel(NowLevelIndex + 1);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            for (int i = 0; i < Row; i++)
                for (int j = 0; j < Column; j++)
                {
                    Texture2D TarTexture = GameSprite[NowMap[i, j]];
                    if (NowMap[i, j] == 2) TarTexture = PlayerSprite[NowDir];
                    _spriteBatch.Draw(TarTexture, new Vector2(j * 64, i * 64), Color.White);
                }
            if (NowMap[TargetX, TargetY] == 0)
                _spriteBatch.Draw(TargetSprite, new Vector2(TargetY * 64, TargetX * 64), Color.White);

            for (int i = 0; i < AbilityBtn.Count; i++)
            {
                AbilityBtn[i].Draw();
            }
            restartBtn.Draw();
            //TODO: Show how many abilities are allowed to choose for this level.
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
