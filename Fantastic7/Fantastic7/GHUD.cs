﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Fantastic7
{
    /// <summary>
    /// Class
    /// </summary>
    class GHUD
    {
        private float _currentHP;
        private int _currency;
        public float CurrentHP {
            get
            {
                return _currentHP;
            }
            set
            {
                _currentHP = value;
                if (_currentHP > MaxHP)
                    _currentHP = MaxHP;
                if (_currentHP < 0)
                    _currentHP = 0;
            }
        }
        public int Currency
        {
            get { return _currency; }
            set
            {
                _currency = value;
                if (_currency < 0)
                    _currency = 0;
            }
        }
        public float MaxHP { get; set; }
        public int Score { get; set; }
        public int Level { get; set; }

        private SpriteFont font;
        private NSprite _hudBG;
        private NSprite _healthBorder;
        private int _fheight;
        private int _fwidth;
        private int xOffset = 10;
        private int yOffset = 10;
        private Entity player;
        private int hpBarLength = 100;

        public GHUD(Entity player, SpriteFont font)
        {
            this.player = player;
            MaxHP = player._maxHealth;
            CurrentHP = player._curHealth;
            Score = 0;
            Level = 1;
            this.font = font;
            _fheight = (int)font.MeasureString("H").Y;
            _fwidth = (int)font.MeasureString("Health: ").X;
            _hudBG = new NSprite(new Rectangle(0, 0, _fwidth + hpBarLength + xOffset * 2, 120), new Color(Color.Black, 0.5f));
            _healthBorder = new NSprite(new Rectangle((int)_hudBG.getPosition().X + 78, (int)_hudBG.getPosition().Y + 14, hpBarLength + 4, 15), new Color(Color.Gray, 0.8f));
    }

        public void update(GameTime gt)
        {
            if (CurrentHP != player._curHealth)
                CurrentHP = player._curHealth;
            if (MaxHP != player._maxHealth)
                MaxHP = player._maxHealth;
        }

        public void draw(SpriteBatchPlus sb, float scale)
        {
            _hudBG.draw(sb, scale);

            // Draws health bar
            _healthBorder.draw(sb, scale);
            sb.Draw(sb.defaultTexture(), 
                new Rectangle((int)_hudBG.getPosition().X + 80, (int)_hudBG.getPosition().Y + 16, (int)(CurrentHP * ((hpBarLength / MaxHP))), 11), 
                Color.LawnGreen);

            sb.DrawString(font, "Health: ", 
                new Vector2(_hudBG.getPosition().X + xOffset, _hudBG.getPosition().Y + yOffset), 
                Color.White);
            String health = CurrentHP + " / " + MaxHP;
            sb.DrawString(font, health, 
                new Vector2(_healthBorder.getPosition().X + _healthBorder.getRect().Width / 2 - font.MeasureString(health).X / 2, 
                    _healthBorder.getPosition().Y + _healthBorder.getRect().Height / 2 - font.MeasureString(health).Y / 2), 
                new Color(Color.Black, 0.4f));
            sb.DrawString(font, "Score: " + Score, 
                new Vector2(_hudBG.getPosition().X + xOffset, _hudBG.getPosition().Y + yOffset + _fheight), 
                Color.White);
            sb.DrawString(font, "Currency: " + Currency, 
                new Vector2(_hudBG.getPosition().X + xOffset, _hudBG.getPosition().Y + yOffset + _fheight * 2), 
                Color.White);
            sb.DrawString(font, "Level: " + Level, 
                new Vector2(_hudBG.getPosition().X + xOffset, _hudBG.getPosition().Y + yOffset + _fheight * 3), 
                Color.White);
        }
    }
}
