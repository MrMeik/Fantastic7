﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Fantastic7
{
    class Map
    {
        private bool muteConsole = true;
        private Room[] _rooms;
        public Room _currRoom;
        private int size = 6;
        public Entity player; 
        private int[] dir = { 0, 1, 2, 3 };
        private const int doorChance = 10;
        private const int popChance = 20;
        private const int directionChance = 50;
        private const int roomRejectionSize = 10;
        private const int roomUpperBound = 30;
        private Random r;

        private GGUI miniMap;
        public GHUD hud;

        public Map()
        {
            _rooms = new Room[size * size];
        }

        public Map(SpriteFont font)
        {
            _rooms = new Room[size * size];
            //player = new Entity(new NSprite(new Rectangle(500, 500, 50, 50), Color.Wheat), 200, 35, 400, GObject.CollisionNature.KnockBack, new Gun());
            player = new Entity(new TSprite(SpriteLoader.images["player"][0], new Rectangle(300, 300, 50, 50), Color.White), 200, 35, 400, GObject.CollisionNature.KnockBack, new Gun());
            hud = new GHUD(player, font);
        }

        public void GenerateMap()
        {
            for (int i = 0; i < size * size; i++) _rooms[i] = null;
            r = new Random();
            int count;
            int x, y;
            do
            {

                //Generates first room in a random place on map
                x = r.Next(0, size);
                y = r.Next(0, size);
                _rooms[x + size * y] = new ShopRoom();
                _currRoom = _rooms[x + size * y];

                //Creates stack used for recursize alg
                Stack<Room> _stack = new Stack<Room>();
                _stack.Push(_rooms[x + size * y]);
                _roomRec(_stack, x, y);// Generates map layout recursively 

                count = 0;
                for (int i = 0; i < size * size; i++) if (_rooms[i] != null) count++;
                if (count < roomRejectionSize || count > roomUpperBound)
                {
                    if(!muteConsole) Console.Out.WriteLine("Retry Generation /////////////////");
                    for (int i = 0; i < size * size; i++) _rooms[i] = null;
                }
            } while (count < roomRejectionSize || count > roomUpperBound);

            _currRoom.addObject(player);//Puts player in first room

            Room crawler = _currRoom;
            do
            {
                for (int i = 0; i < 100; i++)
                {
                    List<Room> direction = new List<Room>();
                    if (crawler.left != null) direction.Add(crawler.left);
                    if (crawler.up != null) direction.Add(crawler.up);
                    if (crawler.right != null) direction.Add(crawler.right);
                    if (crawler.down != null) direction.Add(crawler.down);

                    crawler = direction.ElementAt(r.Next(direction.Count));
                }
            } while (crawler == _currRoom);

            Room temp = new EndRoom();
            if (crawler.left != null)
            {
                crawler.left.right = temp;
                temp.left = crawler.left;
            }
            if (crawler.up != null)
            {
                crawler.up.down = temp;
                temp.up = crawler.up;
            }
            if (crawler.right != null)
            {
                crawler.right.left = temp;
                temp.right = crawler.right;
            }
            if (crawler.down != null)
            {
                crawler.down.up = temp;
                temp.down = crawler.down;
            }

            for(int i = 0; i < _rooms.Length; i++) if(_rooms[i] == crawler) _rooms[i] = temp;


                //
                //Used for constructing minimap, can be ignorned 
                List < GSprite > gs = new List<GSprite>();

            gs.Add(new NSprite(new Rectangle(0, 0, 10 + 100 * size, 10 + 100 * size), Color.Black));

            for(int i = 0; i < size; i++)
            {
                for(int j = 0; j < size; j++)
                {
                    if (_rooms[i + size * j] != null)
                    {
                        if (i == x && j == y) gs.Add(new NSprite(new Rectangle(10 + (100) * i, 10 + 100 * j, 90, 90), Color.Red));
                        else if (_rooms[i + size * j] is EndRoom) gs.Add(new NSprite(new Rectangle(10 + (100) * i, 10 + 100 * j, 90, 90), Color.Purple));
                        else gs.Add(new NSprite(new Rectangle(10 + (100) * i, 10 + 100 * j, 90, 90), Color.Azure));
                    }
                }
            }

            for(int i = 0; i < size; i++)
            {
                for(int j = 0; j < size; j++)
                {
                    if (_rooms[i + size * j] != null)
                    {
                        if (_rooms[i + size * j].right != null) gs.Add(new NSprite(new Rectangle(100 + (100) * i, 50 + 100 * j, 10, 10), Color.Gray));
                        if (_rooms[i + size * j].down != null) gs.Add(new NSprite(new Rectangle(50 + (100) * i, 100 + 100 * j, 10, 10), Color.Gray));
                    }
                }
            }
            miniMap = new GGUI(gs.ToArray(), null, Color.Beige);
            if (!muteConsole)  Console.Out.WriteLine("Complete");
            //End minimap section
            //


        }


        //The recursive alg to generate the map
        protected void _roomRec(Stack<Room> stack, int x, int y)
        {


            int var = r.Next(100 - (stack.Count - 2) * 5);
            if (!muteConsole) Console.Out.WriteLine("Stack #:" + stack.Count + "\tRNG:" + var);

            //Used to break up long chains of rooms. Will randomly stop a chain an go back a room
            if (stack.Count > 1 && popChance > var)
            {
                stack.Pop();
                return;
            }


            List<int> a = new List<int>();
            a.AddRange(dir);
            int sel;

            //Checks to see if the room in on the edge of the map and prevents it from generating outside the map
            if (x == size - 1) a.Remove(0);
            if (y == 0) a.Remove(1);
            if (x == 0) a.Remove(2);
            if (y == size - 1) a.Remove(3);

            //Checks to see which direction it came from or if it has any doors attached to it. Removes those to prevent doubling up of rooms
            if (stack.Peek().right != null) a.Remove(0);
            if (stack.Peek().up != null) a.Remove(1);
            if (stack.Peek().left != null) a.Remove(2);
            if (stack.Peek().down != null) a.Remove(3);

            //Will loop until all directional options are exhausted 
            while (a.Count > 0)
            {
                if (!muteConsole) Console.Out.WriteLine("Stack #:" + stack.Count);

                //Random direction pop
                if (directionChance > r.Next(100))
                {
                    if (!muteConsole) Console.Out.WriteLine("Direction Pop");
                    a.Remove(a.ElementAt(r.Next(a.Count())));
                }

                if (a.Count > 0)
                {
                    sel = r.Next(a.Count); //Picks a random direction that is still unattempted
                    switch (a.ElementAt(sel))
                    {
                        case 0:
                            if (_rooms[x + 1 + size * y] == null)//Checks if the direction does not have a room and creates one
                            {
                                _rooms[x + 1 + size * y] = newRoom();
                                _rooms[x + 1 + size * y].left = _rooms[x + size * y];
                                _rooms[x + size * y].right = _rooms[x + 1 + size * y];
                                stack.Push(_rooms[x + 1 + size * y]);
                                _roomRec(stack, x + 1, y); //Calls back to itself restarting process
                            }
                            else
                            {
                                if (doorChance > r.Next(100)) //If the position has a room, randomly decides to put a door
                                {
                                    _rooms[x + 1 + size * y].left = _rooms[x + size * y];
                                    _rooms[x + size * y].right = _rooms[x + 1 + size * y];
                                }
                            }
                            a.Remove(0);//Prevents option from coming up again
                            break;

                        case 1:
                            if (_rooms[x + size * (y - 1)] == null)
                            {
                                _rooms[x + size * (y - 1)] = newRoom();
                                _rooms[x + size * (y - 1)].down = _rooms[x + size * y];
                                _rooms[x + size * y].up = _rooms[x + size * (y - 1)];
                                stack.Push(_rooms[x + size * (y - 1)]);
                                _roomRec(stack, x, y - 1);
                            }
                            else
                            {
                                if (doorChance > r.Next(100))
                                {
                                    _rooms[x + size * (y - 1)].down = _rooms[x + size * y];
                                    _rooms[x + size * y].up = _rooms[x + size * (y - 1)];
                                }
                            }
                            a.Remove(1);
                            break;

                        case 2:
                            if (_rooms[x - 1 + size * y] == null)
                            {
                                _rooms[x - 1 + size * y] = newRoom();
                                _rooms[x - 1 + size * y].right = _rooms[x + size * y];
                                _rooms[x + size * y].left = _rooms[x - 1 + size * y];
                                stack.Push(_rooms[x - 1 + size * y]);
                                _roomRec(stack, x - 1, y);
                            }
                            else
                            {
                                if (doorChance > r.Next(100))
                                {
                                    _rooms[x - 1 + size * y].right = _rooms[x + size * y];
                                    _rooms[x + size * y].left = _rooms[x - 1 + size * y];
                                }
                            }
                            a.Remove(2);
                            break;

                        case 3:
                            if (_rooms[x + size * (y + 1)] == null)
                            {
                                _rooms[x + size * (y + 1)] = newRoom();
                                _rooms[x + size * (y + 1)].up = _rooms[x + size * y];
                                _rooms[x + size * y].down = _rooms[x + size * (y + 1)];
                                stack.Push(_rooms[x + size * (y + 1)]);
                                _roomRec(stack, x, y + 1);
                            }
                            else
                            {
                                if (doorChance > r.Next(100))
                                {
                                    _rooms[x + size * (y + 1)].up = _rooms[x + size * y];
                                    _rooms[x + size * y].down = _rooms[x + size * (y + 1)];
                                }
                            }
                            a.Remove(3);
                            break;
                        default:
                            break;

                    }
                }
            }

            stack.Pop(); //Base case occures when all four directions are tried

        }

        private Room newRoom()
        {
            int room = r.Next(10);
            if (room < 7) return new MonsterRoom();
            else return new TrapRoom();
        }

        public void update(GameTime gt)
        {
            if(_currRoom is TrapRoom) ((TrapRoom)_currRoom).checkLock();
            
            hud.update(gt);
        }

        public void changeRoom(int i)
        {
            if (!(i >= size * size || i < 0)) _currRoom = _rooms[i];
        }
        public void changeRoomByInstance(Room room)
        {
            if (room != null){
                _currRoom = room;
            }  
        }

        public void draw(SpriteBatchPlus sb, float scale)
        {
            _currRoom.draw(sb,scale);
            hud.draw(sb, scale);

            //This draws the map over top off the room
            //Used for testing purposes when checking the map generation
            //miniMap.draw(sb, scale); 
        }
    }
}
