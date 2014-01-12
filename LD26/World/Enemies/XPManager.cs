using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;


namespace LD26.World.Enemies
{
    class XPManager
    {
        public static int PlayerScore = 0;
        
        static int combos = 0;
        static float combotimer;
        static float textsize = 0.5f;
        static float combotextsize = 0.75f;
        public static void Update()
        {
            if (combotimer > 0)
                combotimer -= GameInfo.CoreGameInfo.DeltaTime;
            if (combotimer <= 0)
            {
                combotimer = 0;
                combos = 0;
                FontManager.RemoveText("Combo");
            }
            Random rand = new Random();
            EnemyHandler.EnemyList.ForEach(x =>
            {
                if (x.Remove && x.Shot)
                {
                    PlayerScore += rand.Next(10, 25);
                    textsize = 1.15f;
                    combos++;
                    if ( combos > 2 )
                        PlayerScore += rand.Next(10 * (combos - 2), 25 * (combos - 2));
                    combotextsize = 1.25f;
                    combotimer = 1500f;
                }
            });
            if (combos > 2)
            {
                FontManager.AddText(new Text("Combo", "COMBO!! x" + combos, new Vector2(MyGameWindow.Instance.Width - 300, 25), combotextsize, true, new Color4(combos * 10, 0, 0, 255)));
            }
            if (GameInfo.GameStateHandler.State == GameInfo.GameStateHandler.GameState.Intro || GameInfo.GameStateHandler.State == GameInfo.GameStateHandler.GameState.Gameplay)
                FontManager.AddText(new Text("Score", "Score: " + PlayerScore, new OpenTK.Vector2(8, 4), textsize, true, Color4.White));
            if (textsize > 0.5f)
                textsize -= 0.035f * GameInfo.CoreGameInfo.DeltaTime * 0.1f;
            if (combotextsize < 0.5f)
                combotextsize = 0.5f;
            if (combotextsize > 0.75f)
                combotextsize -= 0.035f * GameInfo.CoreGameInfo.DeltaTime * 0.1f;
            if (combotextsize < 0.75f)
                combotextsize = 0.75f;

        }
    }
}
