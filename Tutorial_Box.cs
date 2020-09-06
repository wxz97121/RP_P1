using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sokoban
{
    public class Tutorial_Box
    {
        int x_position, y_position;

        public string Name;
        Texture2D m_Texture;
        int boxHeight = 128;
        int boxWidth = 256;
        bool Active = false;


        public int dialogBoxX
        {
            get
            {
                return x_position;
            }
        }

        public int dialogBoxY
        {
            get
            {
                return y_position;
            }
        }

        public Tutorial_Box(string name, int x_position, int y_position, int boxHeight=128, int boxWidth=256) {
            Name = name;
            this.x_position = x_position;
            this.y_position = y_position;
            this.boxHeight = boxHeight;
            this.boxWidth = boxWidth;
            this.m_Texture = Game1.m_Content.Load<Texture2D>("Sprite/" + name);


        }
        public void Draw()
        {
            Game1.m_SpriteBatch.Draw(m_Texture, new Rectangle((int)x_position, (int)y_position, boxWidth, boxHeight), Color.White);
        }

    }
}
