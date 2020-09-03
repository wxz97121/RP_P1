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

        int height_button = 64;
        int width_button = 64;
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

        public Button(string name, int buttonX, int buttonY, int _height_button= 64, int _width_button= 64)
        {
            Name = name;
            height_button = _height_button;
            width_button = _width_button;
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

            if (Mouse.GetState().X < buttonX + width_button &&
                    Mouse.GetState().X > buttonX &&
                    Mouse.GetState().Y < buttonY + height_button &&
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
        Color CalcColor()
        {
            if (Name.StartsWith("button"))
            {
                return Color.White;
            }
            return isChecked ? Color.Blue : Game1.HasBegun ? Color.Gray : Color.White;
        }
        public void Draw()
        {
            Game1.m_SpriteBatch.Draw(m_Texture, new Rectangle((int)ButtonX, (int)ButtonY, width_button, height_button), CalcColor());
        }
    }
}
