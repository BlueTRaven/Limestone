using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;

using System.Net;
using System.Net.Sockets;

using Newtonsoft.Json;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

using Limestone.Utility;
using Limestone.Buffs;
using Limestone.Tiles;
using Limestone.Items;
using Limestone.Inp;

using Superbest_random;

namespace Limestone.Entities
{
    public enum Class
    {
        Archer,
        Huntress,
        IceMage,
        TimeKeeper,
        NONE
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class Player : EntityLiving
    {
        [JsonProperty]
        public int level, xp, xpToNextLevel;
        private Rectangle sourceRect { get { return Assets.GetSourceRect(textureCoord, textureSize); } set { } }
        public override Vector2 center { get { return new Vector2(position.X + ((sourceRect.Width * 4) / 2), position.Y + ((sourceRect.Height * 4) / 2)); } set { } }

        public float maxSpeed;

        public Vector2 cameraOffsetPos;

        float healthWidth = 128;
        float manaWidth = 128;
        Vector2 healthTextSize;
        Vector2 manaTextSize;
        public float speedMult = 1, effectiveSpeed;

        public int mana, maxMana;

        public float damageMult = 1;
        [JsonProperty]
        public int baseAtt, baseDex, baseWis, baseVit, baseDef, baseMana, baseHP;
        public float baseSpd;
        public int attack, wisdom, vitality, equiSpeed;
        public float dexterity;
        private int maxBaseAtt, maxBaseDex, maxBaseWis, maxBaseVit, maxBaseSpd, maxBaseDef, maxBaseMana, maxBaseHP;
        private Vector2 attPL, dexPL, wisPL, vitPL, spdPL, manaPL, hpPL;

        public bool moving = false;
        private int moveCounter = 0;
        public Vector2 predictVec;

        private float maxShoot;

        private Rectangle bgBar { get { return new Rectangle(Vector2.Zero.ToPoint(), new Vector2(128, 32).ToPoint()); } set { } }
        private Rectangle bgBar2 { get { return new Rectangle(new Point(0, 32), new Vector2(128, 32).ToPoint()); } set { } }

        public Enemy quest;

        public Rectangle inventoryRect; public bool movingRect;
        [JsonProperty(ReferenceLoopHandling = ReferenceLoopHandling.Serialize)]
        private ItemSlot[] equipment = new ItemSlot[4]; //IN ORDER: Weapon, Ability, Armor, Accessory
        [JsonProperty(ReferenceLoopHandling = ReferenceLoopHandling.Serialize)]
        private ItemSlot[] inventory = new ItemSlot[8];
        public bool drawInventory = false;
        [JsonProperty]
        public Class cClass;

        public bool cameraIsOffset = false;

        public SoundEffect shootSound;

        public bool DEBUGINVENCIBLE = false;
        public Player(Vector2 position, Class cClass)
        {
            this.tType = EntityType.Player;
            player = true;
            active = true;
            this.position = position;
            this.cClass = cClass;

            //baseSpd = 4; //min = 4 max = 10
            maxSpeed = 4;

            //texture = Assets.GetTexture("char3");
            textureSize = new Vector2(8);
            textureCoord = new Vector2(0);

            hitbox = new RotateableRectangle(new Rectangle(position.ToPoint(), new Point(20)));

            health = 100;
            maxHealth = health;
            baseHP = maxHealth;

            mana = 200;
            maxMana = mana;
            baseMana = maxMana;

            level = 1;
            xpToNextLevel = 50;

            scale = 4f;

            hitSound = Assets.GetSoundEffect("playerImpact1");

            healthTextSize = Assets.GetFont("munro12").MeasureString(String.Format("{0,4}/{1,4}", health, maxHealth));
            manaTextSize = Assets.GetFont("munro12").MeasureString(String.Format("{0,4}/{1,4}", mana, maxMana));

            counter.Add("shoot", 0);

            inventoryRect = new Rectangle(0, 128, 240, 256);
            for (int i = 0; i < 8; i++)
            {
                if (i < 4)
                    inventory[i] = new ItemSlot(null, new Rectangle(64 * i, 16, 48, 48));
                else inventory[i] = new ItemSlot(null, new Rectangle(64 * (i - 4), 80, 48, 48));
            }
            equipment[0] = new ItemSlotLimited(null, new Rectangle(64 * 0, 148, 48, 48), LimitType.Weapon, cClass);
            equipment[1] = new ItemSlotLimited(new ItemAbility(0, AbilityType.Quiver), new Rectangle(64 * 1, 148, 48, 48), LimitType.Ability, cClass);
            equipment[2] = new ItemSlotLimited(null, new Rectangle(64 * 2, 148, 48, 48), LimitType.Armor, Class.NONE);
            equipment[3] = new ItemSlotLimited(null, new Rectangle(64 * 3, 148, 48, 48), LimitType.Accessory, Class.NONE);

            cClass = Class.Archer;
            SetDefaults();

            //baseDef = 15;
        }

        #region defaults
        private void SetDefaults()
        {
            if (cClass == Class.Archer)
            {
                equipment[0].item = new ItemWeapon(11, WeaponType.Bow);
                equipment[1].item = new ItemAbility(4, AbilityType.Quiver);
                equipment[2].item = new ItemArmor(12, ArmorType.Heavy);
                equipment[3].item = new ItemRing(0);
                texture = Assets.GetTexture("char1");
                maxBaseAtt = 75;
                maxBaseDex = 50;
                maxBaseDef = 30;
                maxBaseWis = 50;
                maxBaseVit = 50;

                maxBaseHP = 650;
                maxBaseMana = 300;

                hpPL = new Vector2(20, 30);
                manaPL = new Vector2(2, 8);
                attPL = new Vector2(1, 2);
                spdPL = new Vector2(0, 2);
                dexPL = new Vector2(0, 1);
                vitPL = new Vector2(0, 1);
                wisPL = new Vector2(0, 2);

                shootSound = Assets.GetSoundEffect("bowfire1");
            }
            else if (cClass == Class.IceMage)
            {
                equipment[0].item = new ItemWeapon(11, WeaponType.EleOrb);
                equipment[1].item = new ItemAbility(0, AbilityType.IcecSpell);
                texture = Assets.GetTexture("char2");
                maxBaseAtt = 50;
                maxBaseDex = 75;
                maxBaseDef = 25;
                maxBaseWis = 75;
                maxBaseVit = 50;

                maxBaseHP = 600;
                maxBaseMana = 350;

                hpPL = new Vector2(20, 30);
                manaPL = new Vector2(2, 8);
                attPL = new Vector2(1, 2);
                spdPL = new Vector2(0, 2);
                dexPL = new Vector2(0, 1);
                vitPL = new Vector2(0, 1);
                wisPL = new Vector2(0, 2);

                shootSound = Assets.GetSoundEffect("eleorbfire1");
            }
            else if (cClass == Class.TimeKeeper)
            {
                equipment[0].item = new ItemWeapon(11, WeaponType.Sword);
                equipment[1].item = new ItemAbility(0, AbilityType.Pendant);
                equipment[2].item = new ItemArmor(12, ArmorType.Light);
                texture = Assets.GetTexture("char3");
                maxBaseAtt = 45;
                maxBaseDex = 75;
                maxBaseDef = 30;
                maxBaseWis = 75;
                maxBaseVit = 55;

                maxBaseHP = 650;
                maxBaseMana = 350;

                hpPL = new Vector2(20, 30);
                manaPL = new Vector2(2, 8);
                attPL = new Vector2(0, 1);
                spdPL = new Vector2(1, 2);
                dexPL = new Vector2(0, 2);
                vitPL = new Vector2(0, 1);
                wisPL = new Vector2(0, 2);
            }
        }
        #endregion

        int alive;
        public override void Update(World world)
        {
            damageMult = 1;
            SetEquipStats();

            textureCoord.X = 0;
            textureSize = new Vector2(8);
            bool leftPressed = Main.mouse.MouseKeyPressContinuous(MouseButton.Left);
            Vector2 clickpoint = Main.mouse.position;
            if (new Rectangle(inventoryRect.X, inventoryRect.Y, inventoryRect.Width, 16).Contains(clickpoint) && leftPressed)
                movingRect = true;

            if (movingRect && drawInventory)
                inventoryRect = new Rectangle(MathHelper.Clamp((int)(Main.mouse.position.X - (inventoryRect.Width / 2)), 0, Main.WIDTH - inventoryRect.Width), MathHelper.Clamp((int)(Main.mouse.position.Y - 4), 0, Main.HEIGHT - 136), 240, 468);
            if (!leftPressed)
                movingRect = false;

            alive++;
            predictVec = Vector2.Zero;
            moving = false;
            if (health > maxHealth)
                health = maxHealth;

            bool keyW = Main.keyboard.KeyPressedContinuous(Keys.W), keyA = Main.keyboard.KeyPressedContinuous(Keys.A), keyS = Main.keyboard.KeyPressedContinuous(Keys.S), keyD = Main.keyboard.KeyPressedContinuous(Keys.D);
            
            for (int i = buffs.Count - 1; i >= 0; i--)
            {
                buffs[i].RunEffect(world);

                if (!buffs[i].active)
                    buffs.RemoveAt(i--);
            }

            effectiveSpeed = speed;
            speed = (maxSpeed + speed * (6f / 75f)) * speedMult;
            if (Main.isActive)
            {
                if (Main.keyboard.KeysPressedTogether(new Keys[] { Keys.W, Keys.A, Keys.S, Keys.D }))
                    speed *= 0.75f;

                if (Main.keyboard.KeyPressed(Keys.X))
                    cameraIsOffset = !cameraIsOffset;

                if (keyD)
                {
                    predictVec += Main.camera.right * speed;
                    Move(Main.camera.right * speed);
                    textureCoord.Y = 0;// Vector2.Zero;
                    flip = false;
                    moving = true;
                }

                if (keyA)
                {
                    predictVec += Main.camera.left * speed;
                    Move(Main.camera.left * speed);
                    textureCoord.Y = 0;// Vector2.Zero;
                    flip = true;
                    moving = true;
                }

                if (keyW)
                {
                    predictVec += Main.camera.up * speed;
                    Move(Main.camera.up * speed);
                    textureCoord.Y = 2;// new Vector2(0, 2);
                    flip = false;
                    moving = true;
                }

                if (keyS)
                {
                    predictVec += Main.camera.down * speed;
                    Move(Main.camera.down * speed);
                    textureCoord.Y = 1;// new Vector2(0, 1f);
                    flip = false;
                    moving = true;
                }


                if (moving)
                {
                    if (counter["shoot"] <= 0)
                    {
                        moveCounter++;

                        if (moveCounter < 15)
                            textureCoord.X += 1;
                        else if (moveCounter >= 15 && moveCounter < 30 && ((keyA || keyD) && !(keyW || keyS)))
                            textureCoord.X = 0;
                        else if (moveCounter >= 15 && moveCounter < 30)
                            textureCoord.X += 2;
                        else
                            moveCounter = 0;
                    }
                }

                if (Main.keyboard.KeyPressed(Keys.Space))
                {
                    if (equipment[1].item != null)
                    {
                        ItemAbility ability = (ItemAbility)equipment[1].item;
                        ability.function(world, this);
                    }
                    manaTextSize = Assets.GetFont("munro12").MeasureString(String.Format("{0,4}/{1,4}", mana, maxMana));
                    float manaP = (float)mana / (float)maxMana;
                    manaWidth = 128 * manaP;
                }

                if (Main.mouse.MouseKeyPressContinuous(Inp.MouseButton.Left) && !movingRect)
                {
                    float angle = VectorHelper.FindAngleBetweenTwoPoints(center, VectorHelper.ConvertScreenToWorldCoords(Main.mouse.position));
                    float angleRel = angle - MathHelper.ToDegrees(-Main.camera.Rotation);

                    if (angleRel >= 360)
                        angleRel -= 360;
                    if (angleRel < 0)
                        angleRel += 360;
                    if (angleRel >= 45 && angleRel <= 135)
                    {   //up
                        textureCoord.Y = 2;
                        flip = false;
                    }
                    else if (angleRel >= 135 && angleRel <= 225)
                    {   //left
                        textureCoord.Y = 0;
                        flip = false;
                    }
                    else if (angleRel >= 225 && angleRel <= 315)
                    {   //down
                        textureCoord.Y = 1;
                        flip = false;
                    }
                    else if ((angleRel >= 315 && angleRel <= 360) || (angleRel <= 45 && angleRel >= 0))
                    {   //right
                        textureCoord.Y = 0;
                        flip = true;
                    }
                    if (counter["shoot"] <= 0 && !stunned)
                    {
                        ItemWeapon wep = (ItemWeapon)equipment[0].item;
                        if (wep != null)
                        {
                            wep.function(world, this);
                            if (shootSound != null) shootSound.Play();
                        }
                        else
                        {
                            //Projectile p = world.CreateProjectile(Projectile.Create(1, 20, center, angle, 12, 512, 4)); ;
                            //p.friendly = true;
                        }

                        float calcDex = 60 / (1.5f + 6.5f * ((float)dexterity / 75));//60 / dexterity;
                        maxShoot = calcDex;
                        counter["shoot"] = calcDex;
                    }
                }

                if (counter["shoot"] > 0 && counter["shoot"] <= maxShoot / 1.5f)
                    textureCoord.X = 4;
                else if (counter["shoot"] > 0)
                    textureCoord.X = 5;

                if (Main.keyboard.KeyPressedContinuous(Keys.Q))
                {
                    Main.camera.AddRot(2.2f);
                    //Main.camera.Rotation += MathHelper.ToRadians(2.2f);
                }

                if (Main.keyboard.KeyPressedContinuous(Keys.E))
                {
                    Main.camera.AddRot(-2.2f);
                    //Main.camera.Rotation -= MathHelper.ToRadians(2.2f);
                }

                if (Main.keyboard.KeyPressed(Keys.R))
                {
                    Main.camera.Rotation = 0;
                }

                if (Main.keyboard.KeyPressed(Keys.T))
                {
                    drawInventory = !drawInventory;
                }

                if (Main.keyboard.KeyPressed(Keys.I))
                    DEBUGINVENCIBLE = !DEBUGINVENCIBLE;
            }
            counter["shoot"]--;

            for (int i = texts.Count - 1; i >= 0; i--)
            {
                texts[i].center = center;
                texts[i].Update();
                if (texts[i].dead)
                    texts.RemoveAt(i--);
            }

            float shortest = 99999;
            Enemy shortestDistEnemy = null;
            foreach (Enemy e in world.enemies)
            {
                if (e.quest)
                {
                    float distance = (center - e.center).Length();
                    if (distance < shortest)
                    {
                        shortest = distance;
                        shortestDistEnemy = e;
                    }
                }
            }
            quest = shortestDistEnemy;

            int vitcalc = (int)Math.Floor((double)(60 - vitality / 1.1f));
            if (alive % vitcalc == 0)
            {
                if (health + 1 <= maxHealth)
                {
                    healthTextSize = Assets.GetFont("munro12").MeasureString(String.Format("{0,4}/{1,4}", health, maxHealth));
                    float healthP = (float)health / (float)maxHealth;
                    healthWidth = 128 * healthP;
                    health++;
                }
            }

            int wiscalc = (int)Math.Floor((double)(60 - wisdom / 1.2));
            if (alive % wiscalc == 0)
            {
                if (mana + 1 <= maxMana)
                {
                    manaTextSize = Assets.GetFont("munro12").MeasureString(String.Format("{0,4}/{1,4}", mana, maxMana));
                    float manaP = (float)mana / (float)maxMana;
                    manaWidth = 128 * manaP;
                    mana++;
                }
            }
        }

        public void SetEquipStats()
        {
            speedMult = 1;
            attack = defense = wisdom = vitality = maxHealth = maxMana = 0;
            dexterity = 0;
            speed = 0;

            attack += baseAtt;
            defense += baseDef;
            dexterity += baseDex;
            wisdom += baseWis;
            vitality += baseVit;
            speed += baseSpd;

            maxHealth += baseHP;
            maxMana += baseMana;
            foreach (ItemSlot slot in equipment)
            {
                if (slot != null && slot.item != null)
                {
                    attack += slot.item.attack;
                    defense += slot.item.defense;
                    dexterity += slot.item.dexterity;
                    wisdom += slot.item.wisdom;
                    vitality += slot.item.vitality;
                    speed += slot.item.speed;

                    maxHealth += slot.item.health;
                    maxMana += slot.item.mana;
                }
            }
        }

        public void GiveXp(int amt)
        {   //say xp = 0, xpToNextLevel = 300, amt = 350
            if (level < 20)
            {
                int leftover = 0;

                if (xp + amt > xpToNextLevel)
                {
                    leftover = amt - xpToNextLevel;
                    LevelUp();
                    xp = 0;
                    if (leftover > 0)
                        GiveXp(leftover);
                }
                else
                {
                    texts.Add(new DamageText(amt.ToString(), center, Color.Green));
                    xp += amt;
                }
            }
        }

        public void LevelUp()
        {
            level++;
            texts.Add(new DamageText(level.ToString(), center, Color.Green));
            xpToNextLevel = (int)(xpToNextLevel * 1.4f);
            baseAtt += Main.rand.Next(attPL.X, attPL.Y + 1);
            baseDex += Main.rand.Next(dexPL.X, dexPL.Y + 1);
            baseWis += Main.rand.Next(wisPL.X, wisPL.Y + 1);
            baseVit += Main.rand.Next(vitPL.X, vitPL.Y + 1);
            baseSpd += Main.rand.Next(spdPL.X, spdPL.Y + 1);

            baseHP += Main.rand.Next(hpPL.X, hpPL.Y + 1);
            baseMana += Main.rand.Next(manaPL.X, manaPL.Y + 1);
        }

        public override void TakeDamage(int amt, Projectile2 source, World world)
        {
            if (!DEBUGINVENCIBLE)
            {
                if (world.particles.Count + 3 < 32)
                {
                    for (int i = 0; i <= 3; i++)
                    {
                        Color c = texture.GetPixels().GetPixel(Main.rand.Next(0, texture.Width), Main.rand.Next(0, texture.Height), 1);
                        while (c == Color.Transparent) //So we don't end up with a weird transparent pixel
                            c = texture.GetPixels().GetPixel(Main.rand.Next(0, texture.Width), Main.rand.Next(0, texture.Height), 1);

                        float randVelX = (float)Main.rand.NextDouble(-2, 2);

                        float randVelY = (float)Main.rand.NextDouble(-2, 2);

                        world.CreateParticle(new Particle(hitbox.center + (Main.camera.down * 4), new Vector2(randVelX, randVelY), c, 4, Main.rand.Next(12, 21)));
                    }
                }

                int damagePost = 0;
                if (source != null)
                {
                    if (source.giveBuffs != null)
                    {
                        bool foundBuff = false;
                        foreach (Buff gb in source.giveBuffs)
                        {
                            foreach (Buff b in buffs)
                            {
                                if (b.name == gb.name)
                                {
                                    if (b.active)
                                    {
                                        foundBuff = true;
                                        if (gb.duration > b.duration)
                                            b.duration = gb.duration;  //Refresh the buff's duration
                                        break;
                                    }
                                }
                            }
                            if (!foundBuff)
                            {
                                texts.Add(new DamageText(gb.name, center + new Vector2(0, -texture.Height), Color.Red));
                                gb.entity = this;
                                buffs.Add(gb);
                            }
                        }
                    }
                    damagePost = CalculateDefenseResist(amt, source.armorPiercing);
                }
                else
                    damagePost = CalculateDefenseResist(amt, true);

                healthTextSize = Assets.GetFont("munro12").MeasureString(health + "/" + maxHealth);
                if (health - damagePost > 0)
                {
                    hitSound.Play();

                    if (source == null ? true : source.armorPiercing || defense == 0)
                        texts.Add(new DamageText(damagePost.ToString(), center + new Vector2(0, -texture.Height), Color.Purple));
                    else
                        texts.Add(new DamageText(damagePost.ToString(), center + new Vector2(0, -texture.Height), Color.Red));

                    health -= damagePost;
                    float healthP = (float)health / (float)maxHealth;
                    healthWidth = 128 * healthP;
                }
                else
                {
                    healthWidth = 0;
                    Logger.Log("Player has died", false);
                    Die(world);
                }
            }
        }

        public int CalculateDefenseResist(int amt, bool armorpeirce)
        {
            return armorpeirce ? amt : (int)MathHelper.Clamp((amt - ((defense * .65f) * 1.6f)), 0, 999);
        }

        public override void Die(World world)
        {
            for (int i = texts.Count - 1; i >= 0; i--)
            {
                texts.RemoveAt(i--);
            }
            dead = true;
        }

        public void SetSpeed(float value)
        {
            maxSpeed = value;
            speed = maxSpeed;
        }

        #region drawing

        /// <summary>
        /// Draws inventory and equipment contents.
        /// NOTE: Do DrawHelper.StartDrawCameraSpace before, and DrawHelper.StartDrawWorldSpace after!
        /// </summary>
        /// <param name="batch"></param>
        public void DrawItemsContents(SpriteBatch batch)
        {
            DrawGeometry.DrawRectangle(batch, new Rectangle(inventoryRect.X, inventoryRect.Y, inventoryRect.Width, 468 - 120), new Color(100, 100, 100, 128));
            DrawGeometry.DrawRectangle(batch, new Rectangle(inventoryRect.X, inventoryRect.Y, inventoryRect.Width, 16), Color.Gray);

            for (int i = 0; i < 8; i++)
            {
                if (i < 4)
                    inventory[i].bounds = new Rectangle(64 * i, 96, 48, 48);
                else inventory[i].bounds = new Rectangle(64 * (i - 4), 160, 48, 48);
            }
            for (int i = 0; i < 4; i++)
            {
                equipment[i].bounds = new Rectangle(64 * i, 24, 48, 48);
            }

            Vector2 mousepos = Main.mouse.position;
            ItemSlot hovered = null;
            for (int i = 0; i < 8; i++)
            {
                if (inventory[i] != null)
                {
                    Rectangle bounds = inventory[i].bounds;
                    inventory[i].bounds = new Rectangle(bounds.X + inventoryRect.X, bounds.Y + inventoryRect.Y, 48, 48);

                    batch.Draw(Assets.GetTexture("guiItemslot"), inventory[i].bounds, Color.White);
                    //DrawGeometry.DrawRectangle(batch, inventory[i].bounds, Color.Gray);
                    if (inventory[i].item != null)
                    {
                        DrawHelper.DrawOutline(batch, inventory[i].item.texture, inventory[i].drawRect.Location.ToVector2() - new Vector2(16), Vector2.Zero, 1, 0, 4);

                        if (inventory[i].item.itemType == 3)
                        {
                            ItemRing ring = (ItemRing)inventory[i].item;
                            DrawHelper.DrawOutline(batch, ring.ringTex, inventory[i].drawRect.Location.ToVector2() - new Vector2(16), Vector2.Zero, 1, 0, 4);

                            batch.Draw(ring.texture, inventory[i].drawRect, Color.White);
                            batch.Draw(ring.ringTex, inventory[i].drawRect, ring.ringColor);
                        }
                        else
                            batch.Draw(inventory[i].item.texture, inventory[i].drawRect, Color.White);
                    }

                    if (inventory[i].bounds.Contains(mousepos))
                    {
                        hovered = inventory[i];
                        Main.mouse.hoveredSlot = inventory[i];
                    }
                }
            }
            for (int i = 0; i < 4; i++)
            {
                if (equipment[i] != null)
                {
                    Rectangle bounds = equipment[i].bounds;
                    equipment[i].bounds = new Rectangle(bounds.X + inventoryRect.X, bounds.Y + inventoryRect.Y, 48, 48);

                    batch.Draw(Assets.GetTexture("guiItemslot"), equipment[i].bounds, Color.White);
                    //DrawGeometry.DrawRectangle(batch, equipment[i].bounds, Color.Gray);
                    if (equipment[i].item != null)
                    {
                        DrawHelper.DrawOutline(batch, equipment[i].item.texture, equipment[i].drawRect.Location.ToVector2() - new Vector2(16), Vector2.Zero, 1, 0, 4);

                        if (equipment[i].item.itemType == 3)
                        {
                            ItemRing ring = (ItemRing)equipment[i].item;
                            DrawHelper.DrawOutline(batch, ring.ringTex, equipment[i].drawRect.Location.ToVector2() - new Vector2(16), Vector2.Zero, 1, 0, 4);

                            batch.Draw(ring.texture, equipment[i].drawRect, Color.White);
                            batch.Draw(ring.ringTex, equipment[i].drawRect, ring.ringColor);
                        }
                        else
                            batch.Draw(equipment[i].item.texture, equipment[i].drawRect, Color.White);
                    }

                    if (equipment[i].bounds.Contains(mousepos))
                    {
                        hovered = equipment[i];
                        Main.mouse.hoveredSlot = equipment[i];
                    }
                }
            }
        }

        public void DrawHealthBar(SpriteBatch batch, Texture2D minimap = null)
        {
            if (quest != null)
            {
                Vector2 point = Vector2.Normalize(quest.center - center);
                if ((center - quest.center).Length() > 1024)
                    DrawGeometry.DrawLine(batch, center + (point * 224), quest.center, Color.Red);
            }

            Vector2 test1 = new Vector2(-1, 0);
            Vector2 test2 = new Vector2(-1, 0);
            test1 = Vector2.Transform(test1, Matrix.CreateRotationZ(MathHelper.ToRadians(315) - Main.camera.Rotation));
            test2 = Vector2.Transform(test2, Matrix.CreateRotationZ(MathHelper.ToRadians(45) - Main.camera.Rotation));

            //DrawGeometry.DrawLine(batch, center, center + test1 * 64, Color.White);
            //DrawGeometry.DrawLine(batch, center, center + test2 * 64, Color.White);

            //DrawHelper.StartDrawCameraSpace(batch);
            DrawGeometry.DrawRectangle(batch, bgBar, Color.Gray);
            DrawGeometry.DrawRectangle(batch, new Rectangle(Vector2.Zero.ToPoint(), new Vector2(healthWidth, 32).ToPoint()), Color.Red);

            batch.DrawString(Assets.GetFont("bitfontMunro12"), String.Format("{0,4}/{1,4}", health, maxHealth), new Vector2(bgBar.Center.X - (healthTextSize.X / 2), bgBar.Center.Y - (healthTextSize.Y / 2)), Color.White);

            DrawGeometry.DrawRectangle(batch, bgBar2, Color.Gray);
            DrawGeometry.DrawRectangle(batch, new Rectangle(bgBar2.Location, new Vector2(manaWidth, 32).ToPoint()), Color.CornflowerBlue);

            batch.DrawString(Assets.GetFont("bitfontMunro12"), String.Format("{0,4}/{1,4}", mana, maxMana), new Vector2(bgBar2.Center.X - (manaTextSize.X / 2), bgBar2.Center.Y - (manaTextSize.Y / 2)), Color.White);

            if (minimap != null)
                batch.Draw(minimap, new Rectangle(640, 0, 128, 128), Color.White);

            batch.DrawString(Assets.GetFont("bitfontMunro12"), level + "\n" +
                "Att: " + (baseAtt + attack) + " +" + (attack - baseAtt) + "\n" +
                "Dex: " + (baseDex + dexterity) + " +" + (dexterity - baseDex) + "\n" +
                "Def: " + (baseDef + defense) + " +" + (defense - baseDef) + "\n" +
                "Spd: " + (baseSpd + effectiveSpeed) + " +" + (effectiveSpeed - baseSpd) + "\n" +
                "Vit: " + (baseVit + vitality) + " +" + (vitality - baseVit) + "\n" +
                "Wis: " + (baseWis + wisdom) + " +" + (wisdom - baseWis) + "\n", new Vector2(1, 96), Color.White);

            batch.DrawString(Assets.GetFont("munro12"), Main.cbox.ShowText(), new Vector2(1, 224), Color.White);
            //DrawHelper.StartDrawWorldSpace(batch);
            //batch.DrawString(Assets.GetFont("munro12"), position.X + ", " + position.Y + " | " + position.X / 32 + ", " + position.Y / 32, new Vector2(1, 96), Color.White);
        }

        public override void DrawOutline(SpriteBatch batch)
        {
            Vector2 offset = Main.camera.up * 8;
            Vector2 offset2 = Main.camera.right * 32;

            if (textureCoord.X != 5)
                DrawHelper.DrawOutline(batch, texture, center - texture.Bounds.Center.ToVector2() + offset, sourceRect, DrawHelper.GetTextureOffset(texture.Bounds.Location.ToVector2(), textureSize), 1, flip);
            else
                DrawHelper.DrawOutline(batch, texture, center - texture.Bounds.Center.ToVector2() + offset - (!flip ? Vector2.Zero : offset2), new Rectangle(40, (int)textureCoord.Y * 8, 16, 8), DrawHelper.GetTextureOffset(texture.Bounds.Location.ToVector2(), textureSize), 1, flip);

        }

        public override void Draw(SpriteBatch batch)
        {
            Vector2 offset = Main.camera.up * 8;
            Vector2 offset2 = Main.camera.right * 32;
            batch.Draw(Assets.GetTexture("shadow"), center, null,
                new Color(0, 0, 0, 127), -Main.camera.Rotation, DrawHelper.GetTextureOffset(Vector2.Zero, new Vector2(64, 64)), scale / 8, 0, 0);

            if (textureCoord.X != 5)
                batch.Draw(texture, center + offset, sourceRect,
                Color.White, -Main.camera.Rotation, DrawHelper.GetTextureOffset(texture.Bounds.Location.ToVector2(), textureSize), 4f, flip ? SpriteEffects.FlipHorizontally : 0, 0);
            else //if (textureCoord.X == 5)
                batch.Draw(texture, center + offset - (!flip ? Vector2.Zero : offset2), new Rectangle(40, (int)textureCoord.Y * 8, 16, 8),
                Color.White, -Main.camera.Rotation, DrawHelper.GetTextureOffset(texture.Bounds.Location.ToVector2(), textureSize), 4f, flip ? SpriteEffects.FlipHorizontally : 0, 0);

            foreach (DamageText dt in texts)
                dt.Draw(batch);
        }
        #endregion 
    }
}
