using HalfbyteMedia.OnARoll.MakioClient.Config;
using HalfbyteMedia.OnARoll.MakioClient.Mod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalfbyteMedia.OnARoll.MakioClient.Mods.Input
{
    public class Button4 : InputHandler
    {

        public Button4(ButtonID id, BladeController bladeController) : base(id, bladeController) { }


        public override void OnDown()
        {
            ModGlobals.InSlowMo = !ModGlobals.InSlowMo;
        }
    }
}
