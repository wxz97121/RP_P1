using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Diagnostics;
//using System.Drawing;

namespace Sokoban
{
    public class Game1 : Game
    {
        public static SpriteBatch m_SpriteBatch;
        public static ContentManager m_Content;
        public static SpriteFont Arial32;
        //public static int BtnWidth = 200, BtnHeight = 200;

        // private Stack<int[,]> history = new Stack<int[,]>();

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        List<Texture2D> GameSprite;
        List<Texture2D> PlayerSprite;
        Texture2D TargetSprite, FinalTargetSprite;
        Texture2D BGSprite, StarSprite;
        int PlayerX, PlayerY; // where the player is.
        int TargetX, TargetY;
        int[,] NowMap;
        public List<int[,]> History;
        int Row, Column, NowLevelIndex;
        int MaxAblitiesNum = 1;
        int NowDir = 0;
        int stepCount = 0;
        LevelConfig _LevelData;
        bool isMainMenu = true;
        public static bool HasBegun = false;
        int NowAblitiesChosen = 0;
        public List<Button> AbilityBtn;
        private SoundEffect WinAudio;
        private SoundEffect StarAudio;

        public Tutorial_Box first_tutorial;
        public Tutorial_Box second_tutorial;
        public Button restartBtn;
        public Button undoBtn;
        public Button menuBtn;
        public Button challengeBtn;
        public Button freestyleBtn;

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

            //int MinStepsOfThisLevel = LevelConfig.MinStepsList[NowLevelIndex];


            return false;
        }
        void Undo()
        {
            if (History.Count <= 1)
            {
                HasBegun = false;
                return;
            }
            History.RemoveAt(History.Count - 1);
            NowMap = History[History.Count - 1].Clone() as int[,];
            if (History.Count <= 1) HasBegun = false;
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
            HasBegun = true;
            int tarx = nowx + dx[Dir];
            int tary = nowy + dy[Dir];
            int nowtile = NowMap[nowx, nowy];
            if (!CheckIfInRange(tarx, tary)) return false;
            //Debug.WriteLine(tarx.ToString()+" "+tary.ToString());

            // 0 empty
            // 1 box
            // 2 player
            // 3 wall
            // 4 rocks
            // 5~10 Portal
            if (NowMap[tarx, tary] == 0)
            {
                NowMap[tarx, tary] = NowMap[nowx, nowy];
                NowMap[nowx, nowy] = 0;
                if (CanPull && nowtile == 2) ProcessPull(nowx, nowy, Dir);
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
                        if (CanPull && nowtile == 2) ProcessPull(nowx, nowy, Dir);
                        return true;
                    }
                }
                else
                {
                    if (Move(tarx, tary, Dir))
                    {
                        NowMap[tarx, tary] = NowMap[nowx, nowy];
                        NowMap[nowx, nowy] = 0;
                        if (CanPull && nowtile == 2) ProcessPull(nowx, nowy, Dir);
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
            else if (NowMap[tarx, tary] >= 5 && NowMap[nowx, nowy] == 2)
            {
                int PortalIndex = NowMap[tarx, tary];
                int PortalPair = PortalIndex + 2 * (PortalIndex % 2) - 1;
                for (int i = 0; i < Row; i++)
                    for (int j = 0; j < Column; j++)
                        if (NowMap[i, j] == PortalPair)
                        {
                            NowMap[i, j] = 2;
                            NowMap[tarx, tary] = 0;
                            NowMap[nowx, nowy] = 0;
                        }
            }

            return false;

        }
        void LoadLevel(int num)
        {
            if (num >= LevelConfig.MapList.Count || num < 0) return;
            isWin = false;
            NowMap = (int[,])LevelConfig.MapList[num].Clone();
            History = new List<int[,]>();
            History.Add(NowMap.Clone() as int[,]);
            Row = NowMap.GetLength(0);
            Column = NowMap.GetLength(1);
            HasBegun = false;
            /*
            Row = LevelConfig.RowList[num];
            Column = LevelConfig.ColumnList[num];
            */
            TargetX = LevelConfig.TargetXList[num];
            TargetY = LevelConfig.TargetYList[num];
            NowAblitiesChosen = 0;
            MaxAblitiesNum = LevelConfig.AbilitySlotList[num];
            AbilityBtn = new List<Button>();
            restartBtn = new Button("Reset_Button", 700, 700);
            undoBtn = new Button("button_undo", 50, 700);
            menuBtn = new Button("Home_Icon", 800, 700);
            first_tutorial = new Tutorial_Box("first_tutorial", 350, 100);
            second_tutorial = new Tutorial_Box("second_tutorial", 200, 750);

            CanUp = CanDown = CanLeft = CanRight = CanPull = CanPushMulti = CanDestroy = false;

            NowDir = 0;
            AbilityBtn.Add(new Button(LevelConfig.AbilityList[num][0], 725, 100));
            AbilityBtn.Add(new Button(LevelConfig.AbilityList[num][1], 725, 175));
            AbilityBtn.Add(new Button(LevelConfig.AbilityList[num][2], 650, 175));
            AbilityBtn.Add(new Button(LevelConfig.AbilityList[num][3], 800, 175));
            //if (undoBtn.isChecked = false) { }
            for (int i = 4; i < LevelConfig.AbilityList[num].Count; i++)
            {
                AbilityBtn.Add(new Button(LevelConfig.AbilityList[num][i], 750, 75 * (i - 2) + 150));
                //AbilityBtn.Add(new Button(LevelConfig.AbilityList[num][i], 75 * i + 25, 680));
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
            _graphics.PreferredBackBufferWidth = 900;
            _graphics.PreferredBackBufferHeight = 900;
            _graphics.ApplyChanges();
            _LevelData = new LevelConfig();
            GameSprite = new List<Texture2D>();
            NowLevelIndex = 0;
            PlayerSprite = new List<Texture2D>();
            LoadLevel(0);
            Window.AllowUserResizing = true;


            base.Initialize();
        }
        Texture2D TitleSprite;
        protected override void LoadContent()
        {
            WinAudio = Content.Load<SoundEffect>("Audio/Win");
            StarAudio = Content.Load<SoundEffect>("Audio/Star");
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            m_SpriteBatch = _spriteBatch;
            GameSprite.Add(Content.Load<Texture2D>("Sprite/white_space"));//space1 0
            GameSprite.Add(Content.Load<Texture2D>("Sprite/box"));//box 1
            GameSprite.Add(Content.Load<Texture2D>("Sprite/body"));// 2
            GameSprite.Add(Content.Load<Texture2D>("Sprite/black_space"));//blackspace 3
            GameSprite.Add(Content.Load<Texture2D>("Sprite/rock 2x"));//rock 4

            GameSprite.Add(Content.Load<Texture2D>("Sprite/portal_A_2x"));
            GameSprite.Add(GameSprite[5]);

            GameSprite.Add(Content.Load<Texture2D>("Sprite/portal_C_2x"));
            GameSprite.Add(GameSprite[7]);

            GameSprite.Add(Content.Load<Texture2D>("Sprite/portalC"));
            GameSprite.Add(GameSprite[9]);

            TitleSprite = Content.Load<Texture2D>("Sprite/Title");
            TargetSprite = Content.Load<Texture2D>("Sprite/flag 2x");//flag
            FinalTargetSprite = Content.Load<Texture2D>("Sprite/Trophy");
            BGSprite = Content.Load<Texture2D>("Sprite/Background");
            StarSprite = Content.Load<Texture2D>("Sprite/New_Star");
            PlayerSprite.Add(Content.Load<Texture2D>("Sprite/body_back"));
            PlayerSprite.Add(Content.Load<Texture2D>("Sprite/body"));
            PlayerSprite.Add(Content.Load<Texture2D>("Sprite/body Lside 2x"));
            PlayerSprite.Add(Content.Load<Texture2D>("Sprite/body_side"));

            Arial32 = Content.Load<SpriteFont>("Fonts/Arial32");
        }
        private KeyboardState PreviousState;
        private MouseState PreviousMouseState;
        void UpdateState()
        {
            stepCount = History.Count - 1;
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
            challengeBtn = new Button("challenge_button", 365, 350, 128, 128);
            freestyleBtn = new Button("freestyle", 365, 550, 128, 128);
            if (CheckLMBClicked())
            {
                for (int i = 0; i < AbilityBtn.Count; i++)
                    if (AbilityBtn[i].enterButton() && !HasBegun)
                    {
                        if (AbilityBtn[i].isChecked)
                        {
                            NowAblitiesChosen--;
                            AbilityBtn[i].ToogleChecked();
                            OnButtonClicked(AbilityBtn[i].Name);
                            AbilityBtn[i].rePos();

                        }

                        else if (NowAblitiesChosen < MaxAblitiesNum)
                        {
                            NowAblitiesChosen++;
                            AbilityBtn[i].ToogleChecked();
                            OnButtonClicked(AbilityBtn[i].Name);

                            AbilityBtn[i].buttonX = -75 + 100 * NowAblitiesChosen;
                            AbilityBtn[i].buttonY = 800;
                        }
                    }
                if (restartBtn.enterButton()) LoadLevel(NowLevelIndex);
                if (undoBtn.enterButton()) Undo();
                if (challengeBtn.enterButton() && isMainMenu)
                {

                    isMainMenu = false;
                    LoadLevel(0);

                }
                if (menuBtn.enterButton())
                {
                    isMainMenu = true;
                }

            }
        }

        bool isWin = false;
        double SecondsSinceWin = 0;
        float exp(double a, double x, double b)
        {
            return MathF.Pow((float)a, (float)x) - (float)b;
        }
        private int _TMPSTARS;
        void DrawWinUI()
        {
            _spriteBatch.Draw(BGSprite, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);
            if (SecondsSinceWin > 0.5)
            {
                if (_TMPSTARS == 0)
                {
                    StarAudio.CreateInstance().Play();
                    _TMPSTARS++;
                }
                _spriteBatch.Draw(StarSprite, new Vector2(50, 350), Color.White * 15 * ((float)SecondsSinceWin - 0.5f));
            }
            if (SecondsSinceWin > 1)
            {
                if (_TMPSTARS == 1)
                {
                    StarAudio.CreateInstance().Play();
                    _TMPSTARS++;
                }
                _spriteBatch.Draw(StarSprite, new Vector2(312, 350), Color.White * 15 * ((float)SecondsSinceWin - 1f));
            }
            if (SecondsSinceWin > 1.5)
            {
                if (_TMPSTARS == 2)
                {
                    StarAudio.CreateInstance().Play();
                    _TMPSTARS++;
                }
                _spriteBatch.Draw(StarSprite, new Vector2(575, 350), Color.White * 15 * ((float)SecondsSinceWin - 1.5f));
            }
        }
        void ProcessWin(GameTime gameTime)
        {
            if (!isWin)
            {
                _TMPSTARS = 0;
                SecondsSinceWin = 0;
                WinAudio.CreateInstance().Play();
            }
            isWin = true;
            SecondsSinceWin += gameTime.ElapsedGameTime.TotalSeconds;
            if (SecondsSinceWin > 3.25f)
                LoadLevel(NowLevelIndex + 1);
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (!isWin)
            {
                UpdateKeyboardInput();
                UpdateMouseInput();
            }


            //TODO: Lock the abilities after player start moving.
            UpdateState();
            UpdatePlayerPos();
            if (CheckForWin())
                ProcessWin(gameTime);


            base.Update(gameTime);
        }

        void drawButtonLabels()
        {
            if (NowLevelIndex == 0)
            {

                _spriteBatch.DrawString(Arial32, "Move", new Vector2(620, 110), Color.Black);
                //_spriteBatch.DrawString(Arial32, "Walk Down", new Vector2(610, 240), Color.Black);
                _spriteBatch.DrawString(Arial32, "Pull Box", new Vector2(640, 315), Color.Black);
                _spriteBatch.DrawString(Arial32, "Multi Boxes", new Vector2(608, 390), Color.Black);
            }
            if (NowLevelIndex == 1)
            {
                _spriteBatch.DrawString(Arial32, "Move", new Vector2(620, 110), Color.Black);
                //_spriteBatch.DrawString(Arial32, "Walk Up", new Vector2(640, 165), Color.Black);
                //_spriteBatch.DrawString(Arial32, "Walk Down", new Vector2(610, 240), Color.Black);
                //_spriteBatch.DrawString(Arial32, "Walk Left", new Vector2(626, 315), Color.Black);
                //_spriteBatch.DrawString(Arial32, "Walk Right", new Vector2(608, 390), Color.Black);
                _spriteBatch.DrawString(Arial32, "Pull Box", new Vector2(640, 315), Color.Black);
                _spriteBatch.DrawString(Arial32, "Multi Boxes", new Vector2(608, 390), Color.Black);
            }
            if (NowLevelIndex == 2)
            {
                _spriteBatch.DrawString(Arial32, "Move", new Vector2(620, 110), Color.Black);
                //_spriteBatch.DrawString(Arial32, "Walk Up", new Vector2(640, 165), Color.Black);
                //_spriteBatch.DrawString(Arial32, "Walk Down", new Vector2(610, 240), Color.Black);
                //_spriteBatch.DrawString(Arial32, "Walk Left", new Vector2(626, 315), Color.Black);
                //_spriteBatch.DrawString(Arial32, "Walk Right", new Vector2(608, 390), Color.Black);
                _spriteBatch.DrawString(Arial32, "Pull Box", new Vector2(640, 315), Color.Black);
                _spriteBatch.DrawString(Arial32, "Multi Boxes", new Vector2(608, 390), Color.Black);
            }
            if (NowLevelIndex == 3)
            {
                _spriteBatch.DrawString(Arial32, "Move", new Vector2(620, 110), Color.Black);
                //_spriteBatch.DrawString(Arial32, "Walk Up", new Vector2(640, 165), Color.Black);
                //_spriteBatch.DrawString(Arial32, "Walk Down", new Vector2(610, 240), Color.Black);
                //_spriteBatch.DrawString(Arial32, "Walk Left", new Vector2(626, 315), Color.Black);
                //_spriteBatch.DrawString(Arial32, "Walk Right", new Vector2(608, 390), Color.Black);
                _spriteBatch.DrawString(Arial32, "Pull Box", new Vector2(640, 315), Color.Black);
            }
            if (NowLevelIndex == 4)
            {
                _spriteBatch.DrawString(Arial32, "Move", new Vector2(620, 110), Color.Black);
                //_spriteBatch.DrawString(Arial32, "Walk Up", new Vector2(640, 165), Color.Black);
                //_spriteBatch.DrawString(Arial32, "Walk Down", new Vector2(610, 240), Color.Black);
                //_spriteBatch.DrawString(Arial32, "Walk Left", new Vector2(626, 315), Color.Black);
                //_spriteBatch.DrawString(Arial32, "Walk Right", new Vector2(608, 390), Color.Black);
                _spriteBatch.DrawString(Arial32, "Pull Box", new Vector2(640, 315), Color.Black);
                _spriteBatch.DrawString(Arial32, "Multi Boxes", new Vector2(608, 390), Color.Black);
                _spriteBatch.DrawString(Arial32, "Explode Rock", new Vector2(584, 465), Color.Black);
            }
            if (NowLevelIndex == 5)
            {
                _spriteBatch.DrawString(Arial32, "Move", new Vector2(620, 110), Color.Black);
                //_spriteBatch.DrawString(Arial32, "Walk Up", new Vector2(640, 165), Color.Black);
                //_spriteBatch.DrawString(Arial32, "Walk Down", new Vector2(610, 240), Color.Black);
                //_spriteBatch.DrawString(Arial32, "Walk Left", new Vector2(626, 315), Color.Black);
                //_spriteBatch.DrawString(Arial32, "Walk Right", new Vector2(608, 390), Color.Black);
                _spriteBatch.DrawString(Arial32, "Pull Box", new Vector2(640, 315), Color.Black);
                _spriteBatch.DrawString(Arial32, "Multi Boxes", new Vector2(608, 390), Color.Black);
                _spriteBatch.DrawString(Arial32, "Explode Rock", new Vector2(584, 465), Color.Black);
            }
            if (NowLevelIndex == 6)
            {
                _spriteBatch.DrawString(Arial32, "Move", new Vector2(620, 110), Color.Black);
                //_spriteBatch.DrawString(Arial32, "Walk Up", new Vector2(640, 165), Color.Black);
                //_spriteBatch.DrawString(Arial32, "Walk Down", new Vector2(610, 240), Color.Black);
                //_spriteBatch.DrawString(Arial32, "Walk Left", new Vector2(626, 315), Color.Black);
                //_spriteBatch.DrawString(Arial32, "Walk Right", new Vector2(608, 390), Color.Black);
                _spriteBatch.DrawString(Arial32, "Pull Box", new Vector2(640, 315), Color.Black);
                _spriteBatch.DrawString(Arial32, "Multi Boxes", new Vector2(610, 390), Color.Black);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            //isMainMenu = true;
            if (isMainMenu == true)
            {
                GraphicsDevice.Clear(Color.Gray);
                _spriteBatch.Begin();
                _spriteBatch.Draw(TitleSprite, new Vector2(0, 0), Color.White);
                challengeBtn.Draw();
                freestyleBtn.Draw();
                _spriteBatch.End();
            }
            else
            {
                GraphicsDevice.Clear(Color.Gray);
                _spriteBatch.Begin();
                for (int i = 0; i < Row; i++)
                    for (int j = 0; j < Column; j++)
                    {
                        Texture2D TarTexture = GameSprite[NowMap[i, j]];
                        if (NowMap[i, j] == 2) TarTexture = PlayerSprite[NowDir];
                        _spriteBatch.Draw(TarTexture, new Vector2(j * 64, i * 64 + 104), Color.White);
                    }
                if (NowMap[TargetX, TargetY] == 0)
                    _spriteBatch.Draw(NowLevelIndex + 1 < LevelConfig.MapList.Count ? TargetSprite : FinalTargetSprite,
                        new Vector2(TargetY * 64, TargetX * 64 + 104), Color.White);

                for (int i = 0; i < AbilityBtn.Count; i++)
                {
                    AbilityBtn[i].Draw();
                }
                restartBtn.Draw();
                undoBtn.Draw();
                menuBtn.Draw();
                if (NowLevelIndex == 0)
                {
                    if (CanUp == false)
                    {
                        first_tutorial.Draw();
                    }
                    else
                    {
                        second_tutorial.Draw();
                    }
                }

                //draw all the text
                if (MaxAblitiesNum - NowAblitiesChosen == 0)
                {
                    _spriteBatch.DrawString(Arial32, "No Abilities Left", new Vector2(150, 700), Color.Black);
                }
                else
                {
                    _spriteBatch.DrawString(Arial32, "Abilities Left: " + (MaxAblitiesNum - NowAblitiesChosen), new Vector2(150, 700), Color.Black);
                }
                _spriteBatch.DrawString(Arial32, "Steps taken: " + stepCount, new Vector2(10, 10), Color.Black);

                //draw button descriptions based on level
                drawButtonLabels();

                if (isWin)
                {
                    DrawWinUI();
                }
                _spriteBatch.End();
                base.Draw(gameTime);
            }
        }
    }
}
