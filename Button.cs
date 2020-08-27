using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
namespace Sokoban
{

    public class Button
    {
        int buttonX, buttonY; //button position
        Texture2D m_Texture;
        public bool isChecked = false;
        public string Name;
        public int ButtonX
        {
            get
            {
                return buttonX;
            }
        }

        public int ButtonY
        {
            get
            {
                return buttonY;
            }
        }

        public Button(string name, int buttonX, int buttonY)
        {
            Name = name;

            this.m_Texture = Game1.m_Content.Load<Texture2D>("Sprite/" + name);
            //TODO: Maybe we should catch some exception here?

            //this.m_Texture = texture;
            this.buttonX = buttonX;
            this.buttonY = buttonY;
            isChecked = false;

        }

        /**
         * @return true: If a player enters the button with mouse
        **/
        public bool enterButton()
        {

            if (Mouse.GetState().X < buttonX + m_Texture.Width / 4 &&
                    Mouse.GetState().X > buttonX &&
                    Mouse.GetState().Y < buttonY + m_Texture.Height / 4 &&
                    Mouse.GetState().Y > buttonY)
            {
                return true;
            }
            return false;
        }

        public bool ToogleChecked()
        {
            isChecked = !isChecked;
            return isChecked;
        }

        public void Draw()
        {
            Game1.m_SpriteBatch.Draw(m_Texture, new Rectangle((int)ButtonX, (int)ButtonY, m_Texture.Width / 4, m_Texture.Height / 4), isChecked ? Color.Blue : Color.White);
        }
    }
}
