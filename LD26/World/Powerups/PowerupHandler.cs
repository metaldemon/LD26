using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD26.World.Powerups
{
    class PowerupHandler
    {
        public static List<Powerup> PowerupList = new List<Powerup>();

        public static void Update()
        {
            PowerupList = PowerupList.Where(x => !x.Remove).ToList();
            PowerupList.ForEach(x => x.Update());
        }

        public static void Draw()
        {
            PowerupList.ForEach(x => x.Draw());
        }
    }
}
