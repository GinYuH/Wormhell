using Microsoft.Xna.Framework;
using System;
using System.ComponentModel;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader.Config;

namespace Wormhell
{
    [BackgroundColor(49, 32, 36, 216)]
    public class WormConfig : ModConfig
    {
        public static WormConfig Instance;
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Header("$Mods.Wormhell.Config.Mainname")]
        [Label("$Mods.Wormhell.Config.AllowTown.Label")]
        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(false)]
        [Tooltip("Allows Town NPCs to become worms")]
        public bool ConfigTown { get; set; }

        [Label("$Mods.Wormhell.Config.AllowCritter.Label")]
        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(false)]
        [Tooltip("Allows Critters to become worms")]
        public bool ConfigCritter { get; set; }

        [Label("$Mods.Wormhell.Config.AllowImmortal.Label")]
        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(false)]
        [Tooltip("Allows Invincible NPCs to become worms")]
        public bool ConfigInvincible { get; set; }

        [Label("$Mods.Wormhell.Config.Count.Label")]
        [SliderColor(224, 165, 56, 128)]
        [Range(0, 199)]
        [DefaultValue(30)]
        [Tooltip("The amount of segments spawned per NPC. Do note that the NPC limit is 200")]
        public int Segments { get; set; }

        [Label("$Mods.Wormhell.Config.Defense.Label")]
        [SliderColor(224, 165, 56, 128)]
        [Range(0, 500)]
        [DefaultValue(0)]
        [Tooltip("The amount of increased defense that segments get")]
        public int SDef { get; set; }

        [Label("$Mods.Wormhell.Config.Immortal.Label")]
        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(false)]
        [Tooltip("Makes worm segments invincible")]
        public bool Hard { get; set; }

        [Label("$Mods.Wormhell.Config.Harmless.Label")]
        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(false)]
        [Tooltip("Makes worm segments not deal damage")]
        public bool DaM { get; set; }

    }
}
