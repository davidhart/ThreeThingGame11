using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TTG
{
    public class WorldMap : GameState
    {
        Vector2[] playerPos = new Vector2[8];
        Texture2D worlMapTex, playerIconTex;
        SpriteBatch _spriteBatch;
        int levelNumber = 0;

        public WorldMap(Game1 parent)
            :base(parent)
        {
            playerPos[0] = new Vector2(338, 542);
            playerPos[1] = new Vector2(357, 472);
            playerPos[2] = new Vector2(271, 454);
            playerPos[3] = new Vector2(117, 408);
            playerPos[4] = new Vector2(109, 256);
            playerPos[5] = new Vector2(240, 251);
            playerPos[6] = new Vector2(212, 182);
            playerPos[7] = new Vector2(113, 382);
        }

        public override void Load(ContentManager content, GraphicsDevice graphics)
        {
            base.Load(content, graphics);
            worlMapTex = content.Load<Texture2D>("volcanis map");
            playerIconTex = content.Load<Texture2D>("player icon");
            _spriteBatch = new SpriteBatch(graphics);
        }
        public override void Draw()
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(worlMapTex, new Vector2(0, 0), Color.White);
            _spriteBatch.Draw(playerIconTex, playerPos[levelNumber - 1], Color.White);
            _spriteBatch.End();
        }
        public override void Update(GameTime gameTime, MouseState newMouse, MouseState oldMouse)
        {
            base.Update(gameTime, newMouse, oldMouse);
        }
    }
}
