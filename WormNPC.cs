using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.GameContent;

namespace Wormhell
{
    public class WormNPC : GlobalNPC
    {
        public bool TailSpawned = false;

        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        public override bool PreAI(NPC npc)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient && npc.type != ModContent.NPCType<WormBody>() && !npc.dontCountMe && ValidWorm(npc))
            {
                if (!TailSpawned)
                {
                    int num2 = npc.whoAmI;
                    int num3 = WormConfig.Instance.Segments;
                    for (int j = 0; j < num3; j++)
                    {
                        int num4 = ModContent.NPCType<WormBody>();
                        int num5 = NPC.NewNPC(npc.GetSource_FromAI(), (int)(npc.position.X + (float)(npc.width / 2)), (int)(npc.position.Y + (float)npc.height), num4, npc.whoAmI, 0f, 0f, 0f, 0f, 255);
                        Main.npc[num5].ai[3] = (float)npc.whoAmI;
                        Main.npc[num5].realLife = npc.whoAmI;
                        Main.npc[num5].ai[1] = (float)num2;
                        if (num2 != npc.whoAmI)
                        Main.npc[num2].ai[0] = (float)num5;
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num5, 0f, 0f, 0f, 0, 0, 0);
                        num2 = num5;
                    }
                    TailSpawned = true;
                }
            }
            return true;
        }
        public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter)
        {
            binaryWriter.Write(TailSpawned);
        }
        public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader)
        {
            TailSpawned = binaryReader.ReadBoolean();
        }

        bool ValidWorm(NPC npc)
        {
            // if the npc is a X and X are disabled
            bool CritterAndConfigDisabled = WormConfig.Instance.ConfigCritter == false && npc.CountsAsACritter;
            bool townie = WormConfig.Instance.ConfigTown == false && npc.townNPC;
            bool invin = WormConfig.Instance.ConfigInvincible == false && npc.dontTakeDamage;

            // if none of the checks apply, proceed
            return !CritterAndConfigDisabled && !townie && !invin;
        }
    }

    public class WormBody : ModNPC
    {
        int originalowner = 0;
        bool initialized = false;

        public override void SetDefaults()
        {
             NPC.lifeMax = 10000;
             NPC.damage = 1;
             NPC.knockBackResist = 0f;
             NPC.dontCountMe = true;
             NPC.aiStyle = -1;
             AIType = -1;
             NPC.canGhostHeal = false;
             NPC.noGravity = true;

        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }

        public void SetUp()
        {
            NPC host = Main.npc[(int)NPC.ai[3]];

            NPC.lifeMax = host.lifeMax;
            NPC.life = host.life;
            NPC.defense = host.defense;
            NPC.damage = host.damage;
            NPC.alpha = host.alpha;
            NPC.width = host.width;
            NPC.height = host.height;
            NPC.behindTiles = host.behindTiles;
            NPC.HitSound = host.HitSound;
            NPC.DeathSound = host.DeathSound;
            NPC.netAlways = host.netAlways;
            NPC.dontTakeDamage = host.dontTakeDamage;
            NPC.hide = host.hide;
            NPC.lavaImmune = host.lavaImmune;
            NPC.netAlways = host.netAlways;
            NPC.scale = host.scale;
            NPC.Size = host.Size;
            NPC.Opacity = host.Opacity;
            NPC.reflectsProjectiles = host.reflectsProjectiles;
            NPC.SpawnedFromStatue = host.SpawnedFromStatue;
            NPC.friendly = host.friendly;
            NPC.chaseable = host.chaseable;
            originalowner = host.type;
        }

        public override void AI()
        {
            NPC host = Main.npc[(int)NPC.ai[3]];
            NPC.defense = host.defense + WormConfig.Instance.SDef;
            if (host.type != NPC.type)
            {
                if (!initialized)
                {
                    originalowner = host.type;
                    initialized = true;
                }
                SetUp();
            }
            if (WormConfig.Instance.Hard)
            {
                NPC.dontTakeDamage = true;
            }
            if (WormConfig.Instance.DaM)
            {
                NPC.damage = 0;
            }
            if (NPC.ai[3] > 0f && Main.npc[(int)NPC.ai[3]].type == originalowner)
            {
                NPC.realLife = (int)NPC.ai[3];
            }
            if (NPC.target < 0 || NPC.target == Main.maxPlayers || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest(true);
            }
            bool flag = false;
            if (NPC.ai[1] <= 0f)
            {
                flag = true;
            }
            else if (Main.npc[(int)NPC.ai[1]].life <= 0 || !Main.npc[(int)NPC.ai[1]].active)
            {
                flag = true;
            }
            if (flag)
            {
                NPC.life = 0;
                NPC.HitEffect(0, 10.0);
                NPC.checkDead();
            }
            Vector2 vector3 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
            float num20 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2);
            float num21 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2);
            num20 = (float)((int)(num20 / 16f) * 16);
            num21 = (float)((int)(num21 / 16f) * 16);
            vector3.X = (float)((int)(vector3.X / 16f) * 16);
            vector3.Y = (float)((int)(vector3.Y / 16f) * 16);
            num20 -= vector3.X;
            num21 -= vector3.Y;
            float num22 = (float)Math.Sqrt((double)(num20 * num20 + num21 * num21));
            if (NPC.ai[1] > 0f && NPC.ai[1] < (float)Main.npc.Length)
            {
                try
                {
                    vector3 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                    num20 = Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - vector3.X;
                    num21 = Main.npc[(int)NPC.ai[1]].position.Y + (float)(Main.npc[(int)NPC.ai[1]].height / 2) - vector3.Y;
                }
                catch
                {
                }
                NPC.rotation = (float)Math.Atan2((double)num21, (double)num20) + 1.57f;
                num22 = (float)Math.Sqrt((double)(num20 * num20 + num21 * num21));
                int num23 = (int)(44f * NPC.scale);
                num22 = (num22 - (float)num23) / num22;
                num20 *= num22;
                num21 *= num22;
                NPC.velocity = Vector2.Zero;
                NPC.position.X = NPC.position.X + num20;
                NPC.position.Y = NPC.position.Y + num21;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            NPC hypnos = Main.npc[(int)NPC.ai[3]];

            Texture2D texture = TextureAssets.Npc[hypnos.type].Value;
            Vector2 origin = new Vector2((float)(texture.Width / 2), (float)(texture.Height / Main.npcFrameCount[hypnos.type] / 2));

            /*Mod Cal;
            ModLoader.TryGetMod("CalamityMod", out Cal);
            if (Cal != null)
            {
                if (hypnos.type == Cal.Find<ModNPC>("Leviathan").NPC.type || hypnos.type == Cal.Find<ModNPC>("PlaguebringerGoliath").NPC.type || hypnos.type == Cal.Find<ModNPC>("HiveMind").NPC.type || hypnos.type == Cal.Find<ModNPC>("SupremeCalamitas").NPC.type)
                {
                    divison = 2;
                }
                if (hypnos.type == Cal.Find<ModNPC>("Artemis").NPC.type || hypnos.type == Cal.Find<ModNPC>("Apollo").NPC.type)
                {
                    divison = 10;
                }
                if (hypnos.type == Cal.Find<ModNPC>("AresBody").NPC.type || hypnos.type == Cal.Find<ModNPC>("AresLaserCannon").NPC.type
                    || hypnos.type == Cal.Find<ModNPC>("AresTeslaCannon").NPC.type || hypnos.type == Cal.Find<ModNPC>("AresPlasmaFlamethrower").NPC.type)
                {
                    divison = 6;
                }
                if (hypnos.type == Cal.Find<ModNPC>("AresGaussNuke").NPC.type)
                {
                    divison = 9;
                }
            }
            if (hypnos.type == NPCID.DD2OgreT2 || hypnos.type == NPCID.DD2OgreT3)
            {
                divison = 3;
            }*/
            Vector2 npcOffset = NPC.Center - screenPos;
            npcOffset -= new Vector2((float)texture.Width, (float)(texture.Height / Main.npcFrameCount[hypnos.type])) * hypnos.scale / 2f;
            npcOffset += origin * (NPC.scale) + new Vector2(0f, NPC.gfxOffY);

            Rectangle area = new Rectangle(hypnos.frame.X, hypnos.frame.Y, (int)(hypnos.frame.Width), hypnos.frame.Height);
            spriteBatch.Draw(texture, npcOffset, area, NPC.GetAlpha(drawColor), NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0f);
            

            return false;
        }

        public override void ModifyTypeName(ref string typeName)
        {
            typeName = Main.npc[(int)NPC.ai[3]].TypeName;
        }

        public override bool CheckActive()
        {
            return false;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(NPC.localAI[0]);
            writer.Write(initialized);
            writer.Write(originalowner);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.localAI[0] = reader.ReadSingle();
            initialized = reader.ReadBoolean();
            originalowner = reader.ReadInt32();
        }
    }
}
