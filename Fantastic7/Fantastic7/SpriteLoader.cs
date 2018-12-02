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
        public static Dictionary<String, Texture2D[]> images;

        public SpriteLoader(Game game) : base(game)
        {
            images = new Dictionary<string, Texture2D[]>();
        }

        public override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Texture2D[] door_north = { Game.Content.Load<Texture2D>("sprites/door_north_open"), Game.Content.Load<Texture2D>("sprites/door_north_closed") };
            Texture2D[] door_south = { Game.Content.Load<Texture2D>("sprites/door_south_open"), Game.Content.Load<Texture2D>("sprites/door_south_closed") };
            Texture2D[] door_east = { Game.Content.Load<Texture2D>("sprites/door_east_open"), Game.Content.Load<Texture2D>("sprites/door_east_closed") };
            Texture2D[] door_west = { Game.Content.Load<Texture2D>("sprites/door_west_open"), Game.Content.Load<Texture2D>("sprites/door_west_closed") };
            // add textures to dictionary
            images.Add("door_north", door_north);
            images.Add("door_south", door_south);
            images.Add("door_east", door_east);
            images.Add("door_west", door_west);
            images.Add("wall", new Texture2D[] { Game.Content.Load<Texture2D>("sprites/wall_01") });
            images.Add("floor", new Texture2D[] { Game.Content.Load<Texture2D>("sprites/floor") });
            images.Add("player", new Texture2D[] { Game.Content.Load<Texture2D>("sprites/player") });
            images.Add("charger", new Texture2D[] { Game.Content.Load<Texture2D>("sprites/charger") });
            images.Add("ranger", new Texture2D[] { Game.Content.Load<Texture2D>("sprites/ranger") });

            base.LoadContent();
        }
    }
}
