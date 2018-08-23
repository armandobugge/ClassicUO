﻿using ClassicUO.Game.GameObjects;
using Microsoft.Xna.Framework;

namespace ClassicUO.Game.Renderer.Views
{
    public class GameTextView : View
    {
        private string _text;

        public GameTextView(in GameText parent) : base(parent)
        {
            _text = parent.Text;

            Texture = TextureManager.GetOrCreateStringTextTexture(GameObject);
            //Bounds = new Rectangle(Texture.Width / 2 - 22, Texture.Height, Texture.Width, Texture.Height);
        }

        public new GameText GameObject => (GameText)base.GameObject;



        public override void Update(in double frameMS)
        {
            base.Update(in frameMS);

            if (GameObject.IsPersistent)
                return;

            GameObject.Timeout -= (int)frameMS;
            if (GameObject.Timeout <= 0)
            {
                GameObject.Dispose();
            }
        }

        public override bool Draw(in SpriteBatch3D spriteBatch, in Vector3 position)
        {
            return DrawInternal(in spriteBatch, in position);
        }

        public override bool DrawInternal(in SpriteBatch3D spriteBatch, in Vector3 position)
        {
            if (!AllowedToDraw)
            {
                return false;
            }

            if (_text != GameObject.Text || Texture == null || Texture.IsDisposed)
            {
                Texture = TextureManager.GetOrCreateStringTextTexture(GameObject);
                //Bounds = new Rectangle(Texture.Width / 2 - 22, Texture.Height, Texture.Width, Texture.Height);

                _text = GameObject.Text;
            }

            Texture.Ticks = World.Ticks;
            //HueVector = RenderExtentions.GetHueVector(0, GameObject.IsPartialHue, false, false);

            Rectangle src = new Rectangle();
            Rectangle dest = new Rectangle((int)position.X, (int)position.Y, GameObject.Width, GameObject.Height);

            src.X = 0; src.Y = 0;

            int maxX = src.X + dest.Width;
            if (maxX <= GameObject.Width)
                src.Width = dest.Width;
            else
            {
                src.Width = GameObject.Width - src.X;
                dest.Width = src.Width;
            }

            int maxY = src.Y + dest.Height;
            if (maxY <= GameObject.Height)
                src.Height = dest.Height;
            else
            {
                src.Height = GameObject.Height - src.Y;
                dest.Height = src.Height;
            }

            return GameObject.Parent == null ?  spriteBatch.Draw2D(Texture, dest, src , Vector3.Zero) : base.Draw(spriteBatch, position);
        }
         

    }
}
