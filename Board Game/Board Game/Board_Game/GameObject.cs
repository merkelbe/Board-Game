using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BoardGame
{
    class GameObject
    {
        #region Fields

        protected int WINDOW_WIDTH;
        protected int WINDOW_HEIGHT;

        protected const int REFRESH_RATE = 16;
        protected const int ELAPSED_TIME = 0;
        
        protected Sprite sprite;
        protected Rectangle destinationRectangle;

        protected bool active;

        #endregion

        #region Properties

        internal int X { get { return destinationRectangle.X; } set { destinationRectangle.X = value; } }
        internal int Y { get { return destinationRectangle.Y; } set { destinationRectangle.Y = value; } }
        internal Rectangle CollisionRectangle { get { return destinationRectangle; } }

        #endregion

        #region Constructors

        internal GameObject(int _x, int _y, Sprite _sprite, int _windowWidth, int _windowHeight)
        {
            WINDOW_WIDTH = _windowWidth;
            WINDOW_HEIGHT = _windowHeight;
            sprite = _sprite;
            destinationRectangle = new Rectangle(_x, _y, _sprite.Width, _sprite.Height);
        }

        #endregion

        #region Methods

        internal virtual void Update()
        {
        }

        internal virtual void Draw()
        {
        }

        #endregion
    }
}
