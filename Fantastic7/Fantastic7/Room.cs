using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Fantastic7
{
    class Room
    {
        //protected GObject leftDoor = new GObject(new Vector2(0, 0), new NSprite(new Rectangle(0, 0, 10, 100), Color.Aqua));
        public Room left;
        public Room right;
        public Room up;
        public Room down;
        protected List<GObject> _go;
        protected List<GSprite> _gs;
        public Rectangle floor;

        public bool doorLock; 

        protected GObject[] _doors = { new GObject(new NSprite(new Rectangle(1280 - 110, 720 / 2 - 50, 110, 100), Color.DarkGray)),
            new GObject(new NSprite( new Rectangle(1280/2-50,0,100,110), Color.DarkGray)),
            new GObject(new NSprite( new Rectangle(0,720/2-50,110,100), Color.DarkGray)),
            new GObject(new NSprite( new Rectangle(1280/2-50,720-110,100,110), Color.DarkGray))};

        //Creates a random room
        public Room()
        {
            Color randomColor;
            _gs = new List<GSprite>();
            randomColor = Color.Lerp(Color.White, new Color(EventHandler.rand.Next(0, 255), EventHandler.rand.Next(0, 255), EventHandler.rand.Next(0, 255)), 0.3f);
            _gs.Add(new TSprite(SpriteLoader.images["wall01"], new Rectangle(0, 0, 1280, 720), randomColor));
            randomColor = Color.Lerp(Color.White, new Color(EventHandler.rand.Next(0, 255), EventHandler.rand.Next(0, 255), EventHandler.rand.Next(0, 255)), 0.3f);
            _gs.Add(new TSprite(SpriteLoader.images["floor"], new Rectangle(100, 100, 1280 - 200, 720 - 200), randomColor));
            //_gs.Add(new NSprite(new Rectangle(0, 0, 1280, 720), new Color(EventHandler.rand.Next(0, 255), EventHandler.rand.Next(0, 255), EventHandler.rand.Next(0, 255))));
            //_gs.Add(new NSprite(new Rectangle(100, 100, 1280 - 200, 720 - 200), new Color(EventHandler.rand.Next(0, 255), EventHandler.rand.Next(0, 255), EventHandler.rand.Next(0, 255))));

            unlockAll();

            _go = new List<GObject>();
            
            floor = new Rectangle(100, 100, 1280 - 200, 720 - 200);
        }

        public bool getLockStatus(GObject door)
        {
            return doorLock;
        }

        public void lockAll()
        {
            doorLock = true;
        }

        public void unlockAll()
        {
            doorLock = false;
        }

        protected void addRanger(int lower, int upper)
        {
            for (int i = 0; i < EventHandler.rand.Next(lower, upper); i++)
            {
                Ranger r = new Ranger(new NSprite(new Rectangle(EventHandler.rand.Next(100, 1280 - 230), EventHandler.rand.Next(100, 720 - 230), 50, 50), new Color(EventHandler.rand.Next(0, 255), EventHandler.rand.Next(0, 255), EventHandler.rand.Next(0, 255))), 80, 3, 80, GObject.CollisionNature.KnockBack, new Gun(7, 20, 600));
                _go.Add(r);
            }
        }

        protected void addCharger(int lower, int upper)
        {
            for(int i = 0; i < EventHandler.rand.Next(lower, upper); i++)
            {
                Charger c = new Charger(new NSprite(new Rectangle(EventHandler.rand.Next(100, 1280 - 230), EventHandler.rand.Next(100, 720 - 230), 50, 50), new Color(EventHandler.rand.Next(0, 255), EventHandler.rand.Next(0, 255), EventHandler.rand.Next(0, 255))), 120, 6, 130, GObject.CollisionNature.KnockBack);
                _go.Add(c);
            }
        }

        protected void addBlocks(int lower, int upper)
        {
            for (int i = 0; i < EventHandler.rand.Next(lower, upper); i++)
            {
                _go.Add(new GObject(new NSprite(new Rectangle(EventHandler.rand.Next(100, 1280 - 230), EventHandler.rand.Next(100, 720 - 230), 70, 70), new Color(EventHandler.rand.Next(0, 255), EventHandler.rand.Next(0, 255), EventHandler.rand.Next(0, 255)))));
            }
        }

        protected void addRandomEnemies()
        {
            for (int i = 0; i < EventHandler.rand.Next(2, 4); i++)
            {
                _go.Add(new GObject(new NSprite(new Rectangle(EventHandler.rand.Next(100, 1280 - 230), EventHandler.rand.Next(100, 720 - 230), 70, 70), new Color(EventHandler.rand.Next(0, 255), EventHandler.rand.Next(0, 255), EventHandler.rand.Next(0, 255)))));
            }
            for (int i = 0; i < EventHandler.rand.Next(2, 4); i++)
            {
                _go.Add(new GObject(new NSprite(new Rectangle(EventHandler.rand.Next(100, 1280 - 230), EventHandler.rand.Next(100, 720 - 230), 30, 30), new Color(EventHandler.rand.Next(0, 255), EventHandler.rand.Next(0, 255), EventHandler.rand.Next(0, 255))), GObject.CollisionNature.Free));
            }
            for (int i = 0; i < EventHandler.rand.Next(1, 4); i++)
            {
                Ranger r=new Ranger(new NSprite(new Rectangle(EventHandler.rand.Next(100, 1280 - 230), EventHandler.rand.Next(100, 720 - 230), 50, 50), new Color(EventHandler.rand.Next(0, 255), EventHandler.rand.Next(0, 255), EventHandler.rand.Next(0, 255))), 80, 2, 80, GObject.CollisionNature.KnockBack, new Gun(6, 20, 600));

                _go.Add(r);
            }
            for (int i = 0; i < EventHandler.rand.Next(1, 4); i++)
            {
                Charger c = new Charger(new NSprite(new Rectangle(EventHandler.rand.Next(100, 1280 - 230), EventHandler.rand.Next(100, 720 - 230), 50, 50), new Color(EventHandler.rand.Next(0, 255), EventHandler.rand.Next(0, 255), EventHandler.rand.Next(0, 255))), 120, 8, 130, GObject.CollisionNature.KnockBack);
                _go.Add(c);
            }
        }

        //Normal way to create a specific room, including gameObjects and the base room sprites.
        //Optionally doors to others rooms can be included
        public Room(GSprite[] roomSprites, GObject[] gameObjects = null, Room r = null, Room u = null, Room l = null, Room d = null)
        {
            right = r;
            up = u;
            left = l;
            down = d;

            _gs = new List<GSprite>();
            _gs.AddRange(roomSprites);

            _go = new List<GObject>();
            if (gameObjects != null) _go.AddRange(gameObjects);

            floor = new Rectangle(100, 100, 1280 - 200, 720 - 200);
        }

        public void addObject(GObject go)
        {
            _go.Add(go);
        }

        public void update(GameTime gt)
        {
            
        }

        public void draw(SpriteBatchPlus sb, float scale)
        {
            foreach (GSprite gs in _gs)
            {
                gs.draw(sb, scale);
            }

            foreach (GObject go in _go)
            {
                go.draw(sb, scale);
            }

            if (right != null) _doors[0].draw(sb, scale);
            if (up != null) _doors[1].draw(sb, scale);
            if (left != null) _doors[2].draw(sb, scale);
            if (down != null) _doors[3].draw(sb, scale);
        }
        public GObject[] getDoors()
        {
            return _doors;
        }
        public List<GObject> getGObjects()
        {
            return _go;
        }
        public List<GSprite> GetGSprites()
        {
            return _gs;
        }
        public GObject removeObject(GObject go)
        {
            GObject found;
            foreach (GObject g in _go)
            {
                if (g.Equals(go))
                {
                    found = g;
                    _go.Remove(go);
                    return found;
                }
            }
            return null;
        }
    }

    class MonsterRoom : Room
    {
        public MonsterRoom() : base()
        {
            switch (EventHandler.rand.Next(0, 4)){
                case 0:
                    addCharger(2, 3);
                    addRanger(2, 3);
                    addBlocks(1, 4);
                    break;
                case 1:
                    addCharger(5, 8);
                    addBlocks(4, 5);
                    break;
                case 2:
                    addBlocks(8, 9);
                    break;
                case 3:
                    addRanger(3, 6);
                    addBlocks(3, 6);
                    break;
            }
        }
    }

    class ShopRoom : Room
    {
        public ShopRoom() : base()
        {
            addObject(new Shop(new NSprite(new Rectangle(1280 / 2 - 25, 720 / 2 - 25, 50, 50), Color.Brown)));
        }
    }

    class EndRoom : Room
    {
        bool mapComplete;
        public EndRoom() : base()
        {
            addObject(new EndObject(new NSprite(new Rectangle(1280 / 2 - 75, 720 / 2 - 75, 150, 150), Color.Red)));
            mapComplete = false;
        }


    }

    class TrapRoom : Room
    {
        private int nonEnemyGO;
        public TrapRoom() : base()
        {
            lockAll();
            switch (EventHandler.rand.Next(0, 3))
            {
                case 0:
                    addBlocks(1, 4);
                    nonEnemyGO = _go.Count + 1;
                    addCharger(2, 3);
                    addRanger(2, 3);
                    break;
                case 1:
                    addBlocks(4, 5);
                    nonEnemyGO = _go.Count + 1;
                    addCharger(5, 8);
                    break;
                case 2:
                    addBlocks(3, 6);
                    nonEnemyGO = _go.Count + 1;
                    addRanger(3, 6);
                    break;
            }
        }

        public new void update(GameTime gt)
        {
            if(_go.Count < nonEnemyGO) { unlockAll(); }
        }
    }
}
