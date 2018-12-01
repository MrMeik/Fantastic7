using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


namespace Fantastic7
{
    class SpriteLoader : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public static Dictionary<String, Texture2D> images;

        public SpriteLoader(Game game) : base(game)
        {
            images = new Dictionary<string, Texture2D>();
        }

        public override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // loading textures
            images.Add("wall01", Game.Content.Load<Texture2D>("sprites/wall_01"));
            images.Add("floor", Game.Content.Load<Texture2D>("sprites/floor"));
            //images.Add("door_north", Game.Content.Load<Texture2D>("sprites/door_closed_north"));
            images.Add("player", Game.Content.Load<Texture2D>("sprites/player"));

            base.LoadContent();
        }
    }
}
