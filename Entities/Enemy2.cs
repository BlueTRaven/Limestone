/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;
using Limestone.Buffs;
using Limestone.Items;
using Limestone.Tiles;

namespace Limestone.Entities
{
    delegate void ShootFunc(params Projectile[] projectiles);
    public class Enemy2 : EntityLiving
    {
        public int type;
        public bool boss;
        public bool quest;

        public int xpGive;
        private Rectangle sourceRect { get { return Assets.GetSourceRect(textureCoord, textureSize); } set { } }
        public override Vector2 center { get { return new Vector2(position.X + ((sourceRect.Width * scale) / 2), position.Y + ((sourceRect.Height * scale) / 2)); } set { } }

        private Vector2 offset { get { return new Vector2(hitbox.width / 4, hitbox.height / 4); } }

        Vector2 healthBarDefault;
        Vector2 healthBarPos = Vector2.Zero;
        float healthBarWidth = 48;

        public bool invulnerable = false;
        public bool invulnOverride = false;
        public bool untargetable = false;
        public bool untOverride = false;
        private int _invulnTicks;
        public int invulnTicks { get { return _invulnTicks; } set { _invulnTicks = value; invulnerable = true; } }
        private int _untargetTicks;
        public int untargetTicks { get { return _untargetTicks; } set { _untargetTicks = value; untargetable = true; } }

        private bool flips = true, shooting = false;

        private int currentWalkFrame;
        private int walkFrameTimer;
        private List<int> walkFrameTimers = new List<int>();

        private int shootFrameLoc;
        private int currentShootFrame;
        private int shootFrameTimer;
        private List<int> shootFrameTimers = new List<int>();

        private Dictionary<int, int> frameWidths = new Dictionary<int, int>(); //how long each "x" coordinate is in size. corresponds to x * scale.

        public int questPriority;

        public string name = "NAME";
        float offsetDist = 2f;
        float shadowscale = 1;

        public LootItem lootItem;
        private Color baseColor;
        private Color flashColor;
        private float flashDuration, flashDurationMax;
        private int flashTotalDuration = -1;
        private Enemy2 parent;
        public List<Enemy2> children = new List<Enemy2>();
        private List<Enemy2> relatives = new List<Enemy2>();
        private Vector2 startLocation;

        private bool idleMove = false;
        private Vector2 idleDirection;
        private int idleTimer, idleTimerMax, idleSpeed;

        public Enemy2(int type, Vector2 position, bool boss = false) : base()
        {
            this.tType = EntityType.Enemy2;
            this.position = position;
            this.startLocation = position;
            this.type = type;
            this.boss = boss;

            SetDefaults();
        }

        private void SetDefaults()
        {
            texture = Assets.GetTexture("notex");

            hitbox = new RotateableRectangle(new Rectangle(position.ToPoint(), new Point(64)));

            textureSize = new Vector2(8);
            textureCoord = new Vector2(0);
            if (!boss)
            {
                #region SetDefaults Enemies
                if (type == 0)
                {
                    name = "Anti-Spectator";
                    texture = Assets.GetTexture("antispectator");
                    counter.Add("shot0", 0);
                    hitSound = Assets.GetSoundEffect("monsterImpact2");
                    dieSound = Assets.GetSoundEffect("deathMonster1");
                    lootItem = new LootItem(0);

                    health = 50;
                    scale = 4;
                    hitbox = new RotateableRectangle(new Rectangle(this.position.ToPoint(), new Point(32)));
                    activeDistance = 512;

                    flips = false;

                    xpGive = 90000;

                    walkFrameTimers.Add(1);
                    frameWidths.Add(0, 8);
                }
                else if (type == 1)
                {
                    name = "Mummy";
                    texture = Assets.GetTexture("mummy1");
                    hitSound = Assets.GetSoundEffect("monsterImpact1");
                    dieSound = Assets.GetSoundEffect("deathMonster1");

                    xpGive = 750;
                    health = 1500;
                    scale = 4;
                    shadowscale = 4;
                    hitbox = new RotateableRectangle(new Rectangle(this.position.ToPoint(), new Point(32)));
                    activeDistance = 512;

                    counter.Add("shot0", 0);

                    lootItem = new LootItem(0);

                    walkFrameTimers.Add(15);
                    walkFrameTimers.Add(15);
                    walkFrameTimers.Add(15);
                    shootFrameLoc = 4;
                    shootFrameTimers.Add(15);
                    shootFrameTimers.Add(15);

                    for (int i = 0; i < 5; i++)
                        frameWidths.Add(i, 8);
                    frameWidths.Add(5, 16);
                }
                else if (type == 2)
                {
                    name = "Pharoh mummy";
                    texture = Assets.GetTexture("mummy2");
                    hitSound = Assets.GetSoundEffect("monsterImpact2");
                    dieSound = Assets.GetSoundEffect("deathMonster1");

                    xpGive = 600;
                    health = 3000;
                    scale = 5;
                    shadowscale = 4;
                    hitbox = new RotateableRectangle(new Rectangle(this.position.ToPoint(), new Point(32)));
                    activeDistance = 512;

                    counter.Add("shot0", 0);
                    counter.Add("shotcount", 0);
                    counter.Add("wait", 0);

                    lootItem = new LootItem(0);

                    walkFrameTimers.Add(15);
                    walkFrameTimers.Add(15);
                    walkFrameTimers.Add(15);
                    shootFrameLoc = 4;
                    shootFrameTimers.Add(15);
                    shootFrameTimers.Add(15);

                    for (int i = 0; i <= 5; i++)
                        frameWidths.Add(i, 8);
                    //frameWidths.Add(5, 16);
                }
                else if (type == 3)
                {
                    name = "Pile of wrappings";
                    texture = Assets.GetTexture("pilewrapping");
                    hitSound = Assets.GetSoundEffect("woodImpact1");
                    dieSound = Assets.GetSoundEffect("woodImpact2");

                    xpGive = 500;
                    health = 1000;
                    scale = 4;
                    shadowscale = 4;
                    hitbox = new RotateableRectangle(new Rectangle(this.position.ToPoint(), new Point(32)));
                    activeDistance = 512;
                    counter.Add("shot0", 0);
                    counter.Add("angle", 0);
                    counter.Add("shotcount", 0);

                    lootItem = new LootItem(0);

                    walkFrameTimers.Add(1);
                    frameWidths.Add(0, 8);
                }
                else if (type == 4)
                {
                    name = "Tomb Snake";
                    texture = Assets.GetTexture("snake");
                    hitSound = Assets.GetSoundEffect("monsterImpact2");
                    dieSound = Assets.GetSoundEffect("deathMonster1");

                    xpGive = 650;
                    health = 1500;
                    scale = 4;
                    shadowscale = 8;
                    activeDistance = 512;
                    offsetDist = 5;

                    hitbox = new RotateableRectangle(new Rectangle(this.position.ToPoint(), new Point(64)));
                    textureSize = new Vector2(16, 16);
                    activeDistance = 512;

                    counter.Add("shot0", 0);
                    counter.Add("chargeX", 0);
                    counter.Add("chargeY", 0);
                    counter.Add("chargeTimer", 60);

                    lootItem = new LootItem(0);

                    walkFrameTimers.Add(1);
                    frameWidths.Add(0, 16);
                }
                else if (type == 5)
                {
                }
                else if (type == 6)
                {
                    name = "Artifact of Shu";
                    texture = Assets.GetTexture("shuArtifact");
                    hitSound = Assets.GetSoundEffect("woodImpact2");
                    dieSound = Assets.GetSoundEffect("woodImpact1");

                    speed = 2;
                    xpGive = 50;
                    defense = 45;
                    health = 1500;
                    scale = 4;
                    shadowscale = 4;
                    flip = false;

                    hitbox = new RotateableRectangle(new Rectangle(this.position.ToPoint(), new Point(48)));
                    activeDistance = 5120;
                    lootItem = new LootItem(0);

                    counter.Add("shot0", 0);
                    counter.Add("numshots", 0);
                    counter.Add("wait", 0);
                    counter.Add("angle", 0);
                    walkFrameTimers.Add(1);
                    frameWidths.Add(0, 8);
                }
                else if (type == 7)
                {
                    name = "Artifact Tower of Shu";
                    texture = Assets.GetTexture("shuArtifactTower");
                    hitSound = Assets.GetSoundEffect("woodImpact2");
                    dieSound = Assets.GetSoundEffect("woodImpact1");

                    counter.Add("shot0", 0);
                    counter.Add("distance", 256);
                    counter.Add("angle", 0);
                    counter.Add("dying", 0);
                    counter.Add("type", 0);

                    speed = 2;
                    xpGive = 50;
                    health = 1500;
                    defense = 99999;
                    scale = 4;
                    shadowscale = 4;
                    flip = false;

                    hitbox = new RotateableRectangle(new Rectangle(this.position.ToPoint(), new Point(48)));
                    activeDistance = 5120;
                    lootItem = new LootItem(0);

                    walkFrameTimers.Add(1);
                    frameWidths.Add(0, 8);
                }
                else if (type == 8)
                {
                    name = "Artifact of Tefnut";
                    texture = Assets.GetTexture("tefnutArtifact");
                    hitSound = Assets.GetSoundEffect("woodImpact2");
                    dieSound = Assets.GetSoundEffect("woodImpact1");

                    counter.Add("shot0", 0);
                    counter.Add("angle", 0);
                    counter.Add("dying", 0);
                    counter.Add("type", 1);

                    counter.Add("dieX", 0);
                    counter.Add("dieY", 0);

                    speed = 2;
                    xpGive = 50;
                    defense = 45;
                    health = 2500;
                    scale = 4;
                    shadowscale = 4;
                    activeDistance = 5120;
                    flip = false;

                    hitbox = new RotateableRectangle(new Rectangle(this.position.ToPoint(), new Point(48)));
                    activeDistance = 5120;
                    lootItem = new LootItem(0);

                    walkFrameTimers.Add(1);
                    frameWidths.Add(0, 8);
                }
                else if (type == 9)
                {
                    texture = Assets.GetTexture("rocksLowlands1");

                    hitSound = Assets.GetSoundEffect("woodImpact2");
                    dieSound = Assets.GetSoundEffect("woodImpact1");

                    counter.Add("type", 2);

                    speed = 0;
                    xpGive = 9000;
                    defense = 45;
                    health = 5000;
                    scale = 4;
                    shadowscale = 4;
                    activeDistance = 256;
                    flips = false;

                    hitbox = new RotateableRectangle(new Rectangle(this.position.ToPoint(), new Point(48)));

                    lootItem = new LootItem(0);

                    walkFrameTimers.Add(1);
                    frameWidths.Add(0, 8);
                }
                healthBarDefault = new Vector2(-24, 20);
                #endregion
            }
            else
            {
                #region SetDefaults Bosses

                textureSize = new Vector2(16);
                if (type == 0)
                {
                    name = "Shu";
                    texture = Assets.GetTexture("shu");
                    hitSound = Assets.GetSoundEffect("woodImpact1");
                    dieSound = Assets.GetSoundEffect("deathMonster1");
                    flips = false;

                    scale = 7;
                    shadowscale = 10;
                    defense = 55;
                    xpGive = 5000;
                    health = 80000;
                    hitbox = new RotateableRectangle(new Rectangle(center.ToPoint() - new Point(36), new Point(72)));
                    offsetDist = 5;
                    activeDistance = 5120;

                    counter.Add("shot0", 0);
                    counter.Add("shot1", 0);
                    counter.Add("shot2", 0);
                    counter.Add("phase", 0);
                    counter.Add("dist", 90);
                    counter.Add("angle", 0);
                    counter.Add("child", 0);

                    counter.Add("distance", 384);
                    counter.Add("changedistance", 1);
                    lootItem = new LootItem(2);

                    walkFrameTimers.Add(1);
                    frameWidths.Add(0, 16);
                }
                if (type == 1)
                {
                    name = "Tefnut";
                    texture = Assets.GetTexture("tefnut");
                    hitSound = Assets.GetSoundEffect("woodImpact1");
                    dieSound = Assets.GetSoundEffect("deathMonster1");
                    flips = false;

                    scale = 7;
                    shadowscale = 10;
                    defense = 35;
                    xpGive = 5000;
                    health = 65000;
                    hitbox = new RotateableRectangle(new Rectangle(center.ToPoint() - new Point(36), new Point(72)));
                    offsetDist = 5;
                    activeDistance = 5120;

                    counter.Add("phase", 0);
                    counter.Add("wait1", 0);
                    counter.Add("shot1", 0);
                    counter.Add("shot1count", 0);
                    counter.Add("shot1angle", 0);
                    counter.Add("shot2", 0);
                    counter.Add("shot3", 0);
                    counter.Add("shot3angle", 0);
                    counter.Add("angle", 180);
                    counter.Add("shot4", 0);
                    counter.Add("child", 0);

                    counter.Add("distance", 384);
                    counter.Add("changedistance", 1);
                    lootItem = new LootItem(1);

                    walkFrameTimers.Add(1);
                    frameWidths.Add(0, 16);
                }
                else if (type == 2)
                {
                    name = "Ra";
                    texture = Assets.GetTexture("notex");
                    hitSound = Assets.GetSoundEffect("woodImpact1");
                    dieSound = Assets.GetSoundEffect("deathMonster1");
                    flips = false;

                    scale = 7;
                    shadowscale = 10;
                    defense = 60;
                    xpGive = 5000;
                    health = 85000;
                    hitbox = new RotateableRectangle(new Rectangle(center.ToPoint() - new Point(36), new Point(72)));
                    offsetDist = 5;
                    activeDistance = 5120;

                    counter.Add("shot0", 0);
                    counter.Add("angle0", 0);
                    counter.Add("shot1", 0);
                    counter.Add("shot2", 0);
                    counter.Add("phase", 0);

                    lootItem = new LootItem(0);

                    walkFrameTimers.Add(1);
                    frameWidths.Add(0, 16);
                }
                #endregion
                healthBarDefault = new Vector2(-24, 64);
            }
            healthBarPos = healthBarDefault;

            maxHealth = health;

            currentWalkFrame = 0;
            if (walkFrameTimers.Count > 0)
                walkFrameTimer = walkFrameTimers[0];
            currentShootFrame = 0;
            if (shootFrameTimers.Count > 0)
                shootFrameTimer = shootFrameTimers[0];

            baseColor = color;
        }

        private int alive;
        public override void Update(World world)
        {
            #region Set
            speed = 1;
            alive++;

            for (int i = buffs.Count - 1; i >= 0; i--)
            {
                buffs[i].RunEffect(world);

                if (!buffs[i].active)
                    buffs.RemoveAt(i--);
            }

            float rotToPlayer = PlayerAngle(world.player);
            float distance = (center - world.player.center).Length();

            if (flashTotalDuration == -1)
            {
                if (walkFrameTimers.Count > 0 && !shooting)  //if there's no shoot frame playing, use this
                {
                    walkFrameTimer--;
                    if (walkFrameTimer <= 0)
                    {
                        //currentWalkFrame++;
                        if (currentWalkFrame >= walkFrameTimers.Count - 1)
                            currentWalkFrame = 0;
                        walkFrameTimer = walkFrameTimers[currentWalkFrame++];
                    }
                }
                else if (shootFrameTimers.Count > 0 && shooting)
                {
                    if (shootFrameTimer-- <= 0)
                    {
                        if (shooting)
                        {
                            shootFrameTimer = shootFrameTimers[currentShootFrame];
                            currentShootFrame++;
                        }
                        if (currentShootFrame > shootFrameTimers.Count - 1)
                        {
                            shooting = false;
                            currentShootFrame = 0;
                        }
                    }
                }
            }
            else
            {
                flashTotalDuration--;
                flashDuration--;

                if (flashDuration <= 0)
                    flashDuration = flashDurationMax;

                if (flashDuration <= flashDurationMax / 2)
                    color = Color.Lerp(baseColor, flashColor, flashDuration / flashDurationMax);
                else
                    color = Color.Lerp(flashColor, baseColor, flashDuration / flashDurationMax);
            }

            if (idleMove)
            {
                idleTimer--;
                if (idleTimer <= 0)
                {
                    idleDirection = Vector2.Normalize(new Vector2((float)Main.rand.NextDouble(-2, 2), (float)Main.rand.NextDouble(-2, 2)));
                    idleTimer = idleTimerMax;
                    idleMove = false;
                }

                position += (idleDirection * idleSpeed) * speed;
                hitbox.MoveTo(position);
            }
            #endregion

            if (!boss)
            {
                #region AI Enemies
                if (type == 0)
                {
                    if (!stunned)
                        counter["shot0"]--;
                    //counter["shot1"]--;

                    if (counter["shot0"] <= 0)
                    {
                        counter["shot0"] = 45;
                        for (int i = 0; i <= 360; i += 360 / 12)
                        {
                            //world.CreateProjectile(Projectile.Create(6, 120, center, i, 2.5f, 128, 6, false));
                        }
                    }
                }
                else if (type == 1)
                {
                    if (!stunned)
                        counter["shot0"]--;

                    bool walking = false;
                    if (distance > 192)
                    {
                        if (!idleMove)
                        {
                            walking = true;
                            Vector2 moveDirec = Vector2.Normalize(world.player.center - center);
                            Move(moveDirec, 2.8f);
                        }
                    }
                    else
                    {
                        SetIdle(30, 1);
                    }
                    if (distance < 224)
                    {
                        if (counter["shot0"] <= 0)
                        {
                            for (float i = -3.75f; i <= 3.75f; i += 7.5f)
                            {
                                Projectile p = new Projectile(Assets.GetTexture("wrapper"), Color.White, hitbox.center, new Vector2(8, 4), true, 4, rotToPlayer + i, 6, 0, 512, 60);
                                Shoot(world, p);
                            }

                            for (int i = 0; i < 12; i++)
                            {
                                world.CreateParticle(new Particle(hitbox.center, new Vector2((float)Main.rand.NextDouble(-5, 5), (float)Main.rand.NextDouble(-5, 5)), Color.Brown, 4, 50));
                            }
                            counter["shot0"] = 75;
                        }
                    }

                    if (!shooting && walking)
                        textureCoord.X = currentWalkFrame;
                    else if (shooting)
                        textureCoord.X = currentShootFrame + shootFrameLoc;
                    else textureCoord.X = 0;
                }
                else if (type == 2)
                {
                    if (!stunned)
                    {
                        counter["shot0"]--;
                    }

                    bool walking = false;
                    if (distance > 192)
                    {
                        if (!idleMove)
                        {
                            walking = true;
                            Vector2 moveDirec = Vector2.Normalize(world.player.center - center);
                            Move(moveDirec, 3.5f);
                        }
                    }
                    else
                    {
                        SetIdle(30, 1);
                    }
                    if (distance < 256)
                    {
                        if (counter["wait"] > 0)
                        {
                            counter["wait"]--;

                            if (counter["shot0"] <= 0)
                            {
                                for (float i = -3.75f; i <= 3.75f; i += 7.5f)
                                {
                                    Projectile p = new Projectile(Assets.GetTexture("wrapper"), Color.White, hitbox.center, new Vector2(8, 4), true, 4, rotToPlayer + i, 6, 0, 512, 60);
                                    Shoot(world, p);
                                }

                                for (int i = 0; i < 12; i++)
                                {
                                    world.CreateParticle(new Particle(hitbox.center, new Vector2((float)Main.rand.NextDouble(-5, 5), (float)Main.rand.NextDouble(-5, 5)), Color.Brown, 4, 50));
                                }
                                counter["shot0"] = 75;
                            }
                        }
                        else
                        {
                            if (children.Count < 3)
                            {
                                if (counter["shotcount"] == 0)
                                {
                                    counter["shotcount"] = 1;
                                    Flash(Color.Orange, 30, 120);
                                }
                                if (flashTotalDuration <= -1)
                                {

                                    Enemy2 e = world.CreateEnemy2(Create(3, hitbox.center, false));
                                    children.Add(e);
                                    counter["shotcount"] = 0;
                                    counter["wait"] = 600;
                                }
                            }
                            else
                            {
                                counter["shotcount"] = 0;
                                counter["wait"] = 600;
                            }
                        }
                    }

                    if (!shooting && walking)
                        textureCoord.X = currentWalkFrame;
                    else if (shooting)
                        textureCoord.X = currentShootFrame + shootFrameLoc;
                    else textureCoord.X = 0;
                }
                else if (type == 3)
                {
                    if (!stunned)
                        counter["shot0"]--;

                    if (counter["shot0"] <= 0)
                    {
                        if (flashTotalDuration <= -1)
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                Projectile p = new Projectile(Assets.GetTexture("wrapper"), Color.White, hitbox.position, new Vector2(8, 4), true, 4, counter["angle"] + i * 90, 5, 0, 769, 60);
                                world.CreateProjectile(p);
                            }
                            if (counter["shotcount"] <= 9)
                                counter["angle"] += 5;
                            else if (counter["shotcount"] > 9 && counter["shotcount"] <= 18)
                                counter["angle"] -= 5;
                            else
                            {
                                counter["shotcount"] = 0;
                                Flash(Color.Blue, 60, 180);
                            }
                            counter["shot0"] = 15;
                            counter["shotcount"]++;
                        }
                    }
                }
                else if (type == 4)
                {
                    if (!stunned)
                    {
                        if (counter["chargeTimer"] <= 0)
                            counter["shot0"]--;
                    }

                    if (distance < 396)
                    {
                        if (counter["shot0"] <= 0 && counter["chargeTimer"] <= 0)
                        {
                            counter["chargeX"] = world.player.center.X;
                            counter["chargeY"] = world.player.center.Y;
                            counter["chargeTimer"] = 120;
                            counter["shot0"] = 180;

                            Projectile p = Projectile.Create(Assets.GetTexture("fang"), Color.White, hitbox.center, new Vector2(8, 4), true, 4.5f, rotToPlayer, 10, 0, 512, 100);
                            p.givebuffs.Add(new Buff("Paralyzed", 120, Buff.EffectParalyzed));
                            world.CreateProjectile(p);
                        }
                    }
                    if (counter["chargeTimer"] > 0)
                    {
                        Vector2 moveDirec = Vector2.Normalize(new Vector2(counter["chargeX"], counter["chargeY"]) - center);
                        Move(moveDirec, 8);
                        if (distance < 32 || (center - new Vector2(counter["chargeX"], counter["chargeY"])).Length() < 32)
                        {
                            Projectile p = Projectile.Create(Assets.GetTexture("tearshot"), Color.Green, hitbox.center, new Vector2(8, 4), true, 4.5f, rotToPlayer, 5, 180, 512, 60).SetWavy(32, .09f, false);
                            p.givebuffs.Add(new Buff("Slowed", 120, Buff.EffectSlowed));
                            world.CreateProjectile(p);

                            Projectile p2 = Projectile.Create(Assets.GetTexture("tearshot"), Color.Green, hitbox.center, new Vector2(8, 4), true, 4.5f, rotToPlayer, 5, 180, 512, 60).SetWavy(32, .09f, true);
                            p2.givebuffs.Add(new Buff("Slowed", 120, Buff.EffectSlowed));
                            world.CreateProjectile(p2);
                            counter["chargeTimer"] = 0;
                        }
                        else
                            counter["chargeTimer"]--;
                    }
                    else
                    {
                        SetIdle(15, 2);
                    }

                }
                else if (type == 5)
                {
                    if (!stunned)
                        counter["shot0"]--;

                    bool walking = false;
                    if (distance > 96 && counter["idlecount"] == 0)
                    {
                        if (!idleMove)
                        {
                            walking = true;
                            Vector2 moveDirec = Vector2.Normalize(world.player.center - center);
                            Move(moveDirec, 4);
                        }
                    }
                    else
                    {
                        SetIdle(15, 2);
                        counter["idlecount"]++;

                        if (counter["idlecount"] >= 4 && counter["shot0"] <= 0)
                            counter["idlecount"] = 0;
                    }
                    if (distance < 96)
                    {
                        if (counter["shot0"] <= 0)
                        {
                            int rand = Main.rand.Next(1, 4);
                            while (rand == counter["prevcount"])
                                rand = Main.rand.Next(1, 4);
                            for (float i = -rand; i <= rand; i++)
                            {
                                Projectile p = new Projectile(Assets.GetTexture("spiker"), Color.White, hitbox.center, new Vector2(4, 8), true, 4, rotToPlayer + (i * 5), 6.5f, -90, 256, 40);
                                Shoot(world, p);
                            }
                            counter["prevcount"] = rand;
                            counter["shot0"] = 60;
                        }
                    }

                    if (counter["shot0"] == 30)
                    {
                        for (float i = -counter["prevcount"]; i <= counter["prevcount"]; i++)
                        {
                            Projectile p = new Projectile(Assets.GetTexture("spiker2"), Color.White, hitbox.center, new Vector2(4, 8), true, 4, rotToPlayer + (i * 5), 9, 0, 128, 25);
                            Shoot(world, p);
                        }
                    }

                    if (!shooting && walking)
                        textureCoord.X = currentWalkFrame;
                    else if (shooting)
                        textureCoord.X = currentShootFrame + shootFrameLoc;
                    else textureCoord.X = 0;
                }
                else if (type == 6)
                {
                    counter["wait"]--;
                    if (!stunned)
                        counter["shot0"]--;

                    if (parent == null)
                    {
                        if (distance > 128 && distance <= 480)
                        {
                            Vector2 moveDirec = Vector2.Normalize(world.player.center - center);
                            velocity = moveDirec * 3.5f;
                            Move(velocity);
                        }
                    }
                    else
                    {
                        counter["angle"] += 4;
                        Vector2 pos = new Vector2(-1, 0);
                        pos = Vector2.Transform(pos * 128, Matrix.CreateRotationZ(MathHelper.ToRadians(counter["angle"])));
                        pos += parent.hitbox.center;
                        float distFromLength = (center - pos).Length();

                        position = pos;

                        hitbox.MoveTo(center);
                    }
                    if (distance < 196)
                    {
                        if (counter["wait"] < 0)
                        {
                            if (counter["numshots"] < 8)
                            {
                                if (counter["shot0"] <= 0)
                                {
                                    Projectile p = new Projectile(Assets.GetTexture("bolt"), Color.Gray, position, new Vector2(12, 32), true, 4, PlayerAngle(world.player), 25, -45, 1024, 30);
                                    p.givebuffs.Add(new Buff("Paralyzed", 15, Buff.EffectParalyzed));
                                    Shoot(world, p);
                                    counter["shot0"] = 5;
                                    counter["numshots"]++;
                                }
                            }
                            else
                            {
                                counter["numshots"] = 0;
                                counter["wait"] = 240;
                            }
                        }
                    }
                }
                else if (type == 7)
                {
                    if (parent != null)
                    {
                        invulnerable = true;
                        invulnOverride = true;
                        counter["angle"]++;

                        if (!stunned)
                            counter["shot0"]--;

                        counter["angle"] += 1 / 3;
                        Vector2 pos = new Vector2(-1, 0);
                        pos = Vector2.Transform(pos * counter["distance"], Matrix.CreateRotationZ(MathHelper.ToRadians(counter["angle"])));
                        pos += parent.hitbox.center;
                        float distFromLength = (center - pos).Length();

                        position = pos;

                        hitbox.MoveTo(center);

                        if (counter["shot0"] <= 0)
                        {
                            if (distance < 128)
                            {
                                for (float i = -40; i <= 40; i += 40 / ((parent.counter["phase"] - 1) / 2f))
                                {

                                    Projectile p = new Projectile(Assets.GetTexture("star"), Color.White, position, new Vector2(32, 32), false, 3, PlayerAngle(world.player) + i, 4f, 0, 512, 20 + (int)parent.counter["phase"] * 5).SetSpin(-8);
                                    p.givebuffs.Add(new Buff("Weak", 300, Buff.EffectWeakness));
                                    world.CreateProjectile(p);
                                }
                                counter["shot0"] = 120 - parent.counter["phase"] * 8;
                            }

                            if (parent.counter["phase"] > 2)
                            {
                                if (distance < 128)
                                {
                                    Projectile p = new Projectile(Assets.GetTexture("bolt"), Color.White, position, new Vector2(12, 32), false, 3, PlayerAngle(world.player), 8f, -45, 128, 6 - +(int)parent.counter["phase"] * 5);
                                    p.givebuffs.Add(new Buff("Paralyzed", 5, Buff.EffectParalyzed));
                                    world.CreateProjectile(p);
                                    counter["shot0"] = 120 - parent.counter["phase"] * 8;
                                }
                            }
                        }

                        if (parent.dead)
                        {
                            if (counter["dying"] == 0)
                            {
                                Flash(Color.Blue, 30, 120);
                                counter["dying"] = 1;
                            }

                            if (flashTotalDuration < 0)
                            {
                                for (int i = 0; i < 360; i += 360 / 10)
                                {
                                    Projectile p = new Projectile(Assets.GetTexture("bolt"), Color.Brown, position, new Vector2(12, 32), true, 8, i, 4f, -45, 256, 150);
                                    p.givebuffs.Add(new Buff("Bleeding", 120, Buff.EffectBleeding));
                                    world.CreateProjectile(p);
                                }
                                Die(world);
                            }
                        }
                    }
                }
                else if (type == 8)
                {
                    if (counter["dying"] == 1)
                    {
                        //counter["dying"]++;
                        if ((new Vector2(counter["dieX"], counter["dieY"]) - center).Length() <= 24)
                        {
                            for (float i = 0; i <= 360; i += 360 / 15)
                            {
                                Projectile p = new Projectile(Assets.GetTexture("tearshot"), Color.LightBlue, position, new Vector2(8, 24), true, 3, i, 4f, 180, 64, 60);
                                p.givebuffs.Add(new Buff("Slowed", 60, Buff.EffectSlowed));
                                world.CreateProjectile(p);
                            }
                            Die(world);
                        }
                    }
                    if (distance > 24 && distance <= 480)
                    {
                        Vector2 moveDirec = Vector2.Zero;
                        if (counter["dying"] == 1)
                            moveDirec = Vector2.Normalize(new Vector2(counter["dieX"], counter["dieY"]) - center);
                        else moveDirec = Vector2.Normalize(world.player.center - center);
                        velocity = moveDirec * (flashTotalDuration <= -1 ? 5 : 15);
                        Move(velocity);
                    }
                    if ((distance <= 24 || health < 300 || (parent == null ? false : parent.counter["phase"] == 5)) && flashTotalDuration <= -1)
                    {
                        counter["dieX"] = world.player.center.X;
                        counter["dieY"] = world.player.center.Y;
                        counter["dying"] = 1;
                        Flash(Color.Blue, 15, 60, true);
                    }
                }
                #endregion
            }
            else
            {
                #region AI Bosses
                #region SHU
                if (type == 0)
                {
                    int countSarc = 0;
                    if (counter["phase"] == 0)
                    {
                        foreach (Enemy2 c in children.ToList())
                        {
                            if (c.counter["type"] == 2)
                                countSarc++;

                            if (c.dead)
                                children.Remove(c);
                        }

                        if (countSarc <= 0)
                        {
                            if (!active)
                            {
                                active = true;
                                Main.cbox.AddShout(name, "YOU HAVE AWAKENED US!");
                            }
                        }
                        else active = false; invulnerable = true; invulnOverride = true;
                    }

                    if (active)
                    { 
                        if (!stunned)
                        {
                            counter["shot0"]--;
                            counter["shot1"]--;
                            counter["shot2"]--;
                            counter["child"]--;
                            counter["changedistance"]--;
                        }
                        bool worldRotation = true;

                        bool shootshields = false;
                        bool shootspinners = false;
                        bool shootspinnersatplayer = false;
                        float anglebetweenspinners = 15;
                        float numspinners = 4;
                        bool shootbolts = false;
                        float anglebetweenbolts = 15;
                        float numbolts = 1;

                        bool changeDistance = false;

                        bool createChild = false;

                        if (health <= maxHealth - 100 && health > maxHealth - 10000)
                        {
                            while (children.Count < 2)
                            {
                                Enemy2 child = world.CreateEnemy2(Create(7, hitbox.center, false));
                                if (children.Count == 0)
                                    child.counter["angle"] = 0;
                                else
                                    child.counter["angle"] = 180;
                                child.parent = this;
                                children.Add(child);
                            }
                            shootspinners = true;
                            shootbolts = true;
                            numspinners = 4;
                            if (counter["phase"] == 0)
                            {
                                Main.cbox.AddShout(name, "Why do you awaken me? Do you seek death? I can show you its sweet embrace.");
                                Flash(Color.Red, 30, 240, true);
                                counter["phase"]++;

                                counter["shot1"] = 0;
                                counter["shot2"] = 0;
                                counter["child"] = 0;
                            }
                        }
                        else if (health <= maxHealth - 10000 && health > maxHealth - 25000)
                        {
                            shootshields = true;
                            shootspinners = true;
                            numspinners = 8;
                            shootbolts = true;
                            anglebetweenbolts = 15;
                            numbolts = 3;

                            changeDistance = true;
                            if (counter["phase"] == 1)
                            {
                                Main.cbox.AddShout(name, "Your efforts are futile. Cower.");
                                Flash(Color.Red, 30, 240, true);
                                counter["phase"]++;

                                counter["shot1"] = 0;
                                counter["shot2"] = 0;
                                counter["child"] = 0;
                                counter["changedistance"] = 0;
                            }
                        }
                        else if (health <= maxHealth - 25000 && health > maxHealth - 40000)
                        {
                            shootshields = true;
                            shootspinners = true;
                            numspinners = 6;
                            shootspinnersatplayer = true;
                            anglebetweenspinners = 90;
                            shootbolts = true;
                            anglebetweenbolts = 20;
                            numbolts = 4;
                            changeDistance = true;
                            if (counter["phase"] == 2)
                            {
                                Main.cbox.AddShout(name, "You are quite the pest...");
                                Flash(Color.Red, 30, 240, true);
                                counter["phase"]++;

                                counter["shot1"] = 0;
                                counter["shot2"] = 0;
                                counter["child"] = 0;
                                counter["changedistance"] = 0;
                            }

                            foreach (Enemy2 child in children)
                            {
                                if (child.counter["type"] == 1)
                                {
                                    if (distance < 512)
                                    {
                                        child.counter["distance"] = distance;
                                    }
                                    else
                                        child.counter["distance"] = 512;
                                }
                            }
                        }
                        else if (health <= maxHealth - 40000 && health > maxHealth - 70000)
                        {
                            shootshields = true;
                            shootspinners = true;
                            numspinners = 8;
                            shootspinnersatplayer = true;
                            anglebetweenspinners = 90;
                            shootbolts = true;
                            anglebetweenbolts = 30;
                            numbolts = 5;
                            createChild = true;
                            changeDistance = true;
                            if (counter["phase"] == 3)
                            {
                                Main.cbox.AddShout(name, "My artifacts shall stop you in your tracks.");
                                Flash(Color.Red, 30, 240, true);
                                counter["phase"]++;

                                counter["shot1"] = 0;
                                counter["shot2"] = 0;
                                counter["child"] = 0;
                                counter["changedistance"] = 0;
                            }

                            foreach (Enemy2 child in children)
                            {
                                if (distance < 256)
                                {
                                    child.counter["distance"] = distance;
                                }
                                else
                                    child.counter["distance"] = 256;
                            }
                        }
                        else if (health <= maxHealth - 45000)
                        {
                            shootshields = true;
                            shootspinners = true;
                            numspinners = 8;
                            shootspinnersatplayer = true;
                            anglebetweenspinners = 90;
                            shootbolts = true;
                            anglebetweenbolts = 40;
                            numbolts = 6;
                            createChild = true;
                            if (counter["phase"] == 4)
                            {
                                Main.cbox.AddShout(name, "You FOOL! DIE!");
                                Flash(Color.Black, 30, 120, true);
                                counter["phase"]++;

                                counter["shot1"] = 0;
                                counter["shot2"] = 0;
                                counter["child"] = 0;
                            }
                            foreach (Enemy2 child in children)
                            {
                                if (child.counter["type"] == 0)
                                {
                                    if (distance < 256)
                                    {
                                        child.counter["distance"] = distance;
                                    }
                                    else
                                        child.counter["distance"] = 256;
                                }
                            }
                            worldRotation = false;
                        }

                        if (worldRotation)
                        {
                            if (speed > 0)
                            {
                                counter["angle"] += .25f * speed;
                                if (counter["angle"] > 360)
                                    counter["angle"] = 0;

                                Vector2 pos = new Vector2(-1, 0);
                                pos = Vector2.Transform(pos * counter["distance"], Matrix.CreateRotationZ(MathHelper.ToRadians(counter["angle"])));
                                pos += startLocation;
                                float distFromLength = (pos - position).Length();

                                if (distFromLength > 8 && alive > 20)
                                {
                                    velocity = Vector2.Normalize(pos - position) * 2;
                                    Move(velocity);
                                }
                                else
                                    position = pos;

                                hitbox.MoveTo(center);
                            }

                            if (counter["changedistance"] <= 0 && changeDistance)
                            {
                                counter["distance"] = Main.rand.Next(128, 384);
                                counter["changedistance"] = 1200;   //20 seconds
                            }
                        }
                        else
                        {
                            if (distance > 128 && distance <= 512)
                            {
                                Vector2 moveDirec = Vector2.Normalize(world.player.center - center);
                                velocity = moveDirec * 5;
                                Move(velocity);
                            }
                        }
                        if (flashTotalDuration <= -1)
                        {
                            if (counter["shot0"] <= 0)
                            {
                                if (shootshields && !stunned)
                                {
                                    for (float i = -20; i <= 20; i += 2.5f)
                                    {
                                        Projectile p = new Projectile(Assets.GetTexture("shield"), new Color(137, 208, 195, 127), hitbox.position + new Vector2(8), new Vector2(48, 16), true, 8, counter["dist"] + i, 12, 135, 1536, 20);
                                        p.givebuffs.Add(new Buff("Stunned", 120, Buff.EffectStunned));
                                        world.CreateProjectile(p);
                                    }

                                    counter["dist"] += 10;
                                    if (counter["dist"] >= 360)
                                        counter["dist"] = 0;

                                    if (health < maxHealth - 15000)
                                        counter["shot0"] = 15;
                                    else
                                        counter["shot0"] = 35;
                                }
                            }

                            if (counter["shot1"] <= 0)
                            {
                                if (distance < 320)
                                {
                                    if (shootspinners && !stunned)
                                    {
                                        if (!shootspinnersatplayer)
                                        {
                                            for (float i = 0; i <= 360; i += 360 / numspinners)
                                            {
                                                Projectile p = new Projectile(Assets.GetTexture("tearSpinner"), new Color(137, 208, 195, 127), hitbox.position + new Vector2(16), new Vector2(32, 32), false, 6, i + 45, 3.3f, 0, 512, 75).SetBoomerang().SetSpin(20);
                                                p.givebuffs.Add(new Buff("Slowed", 30, Buff.EffectSlowed));
                                                world.CreateProjectile(p);
                                                counter["shot1"] = 95;
                                            }
                                        }
                                        else
                                        {
                                            for (float i = -anglebetweenspinners; i <= anglebetweenspinners; i += anglebetweenspinners / (numspinners / 2))
                                            {
                                                Projectile p = new Projectile(Assets.GetTexture("tearSpinner"), new Color(137, 208, 195, 127), hitbox.position + new Vector2(16), new Vector2(32, 32), false, 6, PlayerAngle(world.player) + i, 3.3f, 0, 512, 75).SetBoomerang().SetSpin(25);
                                                p.givebuffs.Add(new Buff("Slowed", 90, Buff.EffectSlowed));
                                                world.CreateProjectile(p);
                                                counter["shot1"] = worldRotation ? 240 : 80;
                                            }
                                        }
                                    }
                                }
                            }
                            if (counter["shot2"] <= 0)
                            {
                                if (distance < 320)
                                {
                                    if (shootbolts && !stunned)
                                    {
                                        if (numbolts == 1)
                                        {
                                            Projectile p = new Projectile(Assets.GetTexture("bolt"), Color.Gray, hitbox.position + new Vector2(8), new Vector2(10, 64), true, 8, PlayerAngle(world.player), 6, 135, 1536, 150);
                                            p.givebuffs.Add(new Buff("Paralyzed", 30, Buff.EffectParalyzed));
                                            world.CreateProjectile(p);
                                        }
                                        else
                                        {
                                            for (float i = -anglebetweenbolts; i <= anglebetweenbolts; i += anglebetweenbolts / ((numbolts - 1) / 2))
                                            {
                                                Projectile p = new Projectile(Assets.GetTexture("bolt"), Color.Gray, hitbox.position + new Vector2(8), new Vector2(10, 64), true, 8, PlayerAngle(world.player) + i, 6, 135, 1536, 200);
                                                p.givebuffs.Add(new Buff("Paralyzed", 60, Buff.EffectParalyzed));
                                                world.CreateProjectile(p);
                                            }
                                        }
                                        counter["shot2"] = 208;
                                    }
                                }
                            }
                            if (counter["child"] <= 0)
                            {
                                if (createChild)
                                {
                                    int parentedCount = 0;
                                    int childCount = 0;
                                    foreach (Enemy2 child in children.ToList())
                                    {
                                        if (!child.dead && child.counter["type"] == 1)
                                        {
                                            childCount++;

                                            if (child.parent != null)
                                                parentedCount++;
                                        }
                                        else
                                            children.Remove(child);
                                    }

                                    if (childCount <= 3)
                                    {
                                        bool madeparented = false;
                                        for (int i = 1; i <= 3 - childCount; i++)
                                        {
                                            Enemy2 child = world.CreateEnemy2(Create(6, hitbox.center + new Vector2(Main.rand.Next(-16, 16), Main.rand.Next(-16, 16)), false));
                                            if (i == 1 && parentedCount == 0 && !madeparented && counter["phase"] != 5)
                                            {
                                                child.parent = this;
                                                madeparented = true;
                                            }
                                        }
                                        counter["child"] = 600;
                                    }
                                }
                            }
                        }
                    }   
                }
                #endregion
                #region TEFNUT
                else if (type == 1)
                {
                    if (counter["phase"] == 0)
                    {
                        int countSarc = 0;
                        foreach (Enemy2 c in children.ToList())
                        {
                            if (c.counter["type"] == 2)
                                countSarc++;

                            if (c.dead)
                                children.Remove(c);
                        }

                        if (countSarc <= 0)
                        {
                            if (!active)
                            {
                                active = true;
                                Main.cbox.AddShout(name, "WHY ARE YOU HERE?");
                            }
                        }
                        else active = false; invulnerable = true; invulnOverride = true;
                    }

                    if (active)
                    {
                        if (!stunned)
                        {
                            counter["shot1"]--;
                            counter["shot2"]--;
                            counter["shot3"]--;
                            counter["shot4"]--;
                            counter["child"]--;
                        }
                        bool worldRotation = true;
                        bool shootWaveShots = false;
                        bool shootBow1 = false;
                        bool shootBow2 = false;
                        bool shootRotShots = false;
                        bool spawnChildren = false;

                        bool changeDistance = false;

                        if (health <= maxHealth - 100 && health > maxHealth - 15000)
                        {
                            shootWaveShots = true;
                            shootBow1 = true;
                            if (counter["phase"] == 0)
                            {
                                Main.cbox.AddShout(name, "You seek to destroy me? You know not what you try.");
                                Flash(Color.Black, 30, 240, true);
                                counter["phase"]++;

                                counter["shot1"] = 0;
                                counter["shot2"] = 0;
                                counter["shot3"] = 0;
                                counter["shot4"] = 0;
                                counter["child"] = 0;
                                counter["changedistance"] = 0;
                            }
                        }
                        else if (health <= maxHealth - 15000 && health > maxHealth - 35000)
                        {
                            shootWaveShots = true;
                            shootBow1 = true;
                            shootBow2 = true;
                            changeDistance = true;
                            if (counter["phase"] == 1)
                            {
                                Main.cbox.AddShout(name, "My archery skills are rusty, but more than enough for the likes of you.");
                                Flash(Color.Black, 30, 240, true);
                                counter["phase"]++;

                                counter["shot1"] = 0;
                                counter["shot2"] = 0;
                                counter["shot3"] = 0;
                                counter["shot4"] = 0;
                                counter["child"] = 0;
                                counter["changedistance"] = 0;
                            }
                        }
                        else if (health <= maxHealth - 35000 && health > maxHealth - 50000)
                        {
                            shootWaveShots = true;
                            shootBow1 = true;
                            shootBow2 = true;
                            spawnChildren = true;
                            changeDistance = true;
                            if (counter["phase"] == 2)
                            {
                                Main.cbox.AddShout(name, "Fail. Die and fail, pest!");
                                Flash(Color.Black, 30, 240, true);
                                counter["phase"]++;

                                counter["shot1"] = 0;
                                counter["shot2"] = 0;
                                counter["shot3"] = 0;
                                counter["shot4"] = 0;
                                counter["child"] = 0;
                                counter["changedistance"] = 0;
                            }
                        }
                        else if (health <= maxHealth - 50000 && health > maxHealth - 60000)
                        {
                            shootWaveShots = true;
                            shootBow1 = true;
                            shootBow2 = true;
                            shootRotShots = true;
                            spawnChildren = true;
                            changeDistance = true;
                            if (counter["phase"] == 3)
                            {
                                Main.cbox.AddShout(name, "It seems you have a deathwish.");
                                Flash(Color.Black, 30, 240, true);
                                counter["phase"]++;

                                counter["shot1"] = 0;
                                counter["shot2"] = 0;
                                counter["shot3"] = 0;
                                counter["shot4"] = 0;
                                counter["child"] = 0;
                                counter["changedistance"] = 0;
                            }
                        }
                        else if (health <= maxHealth - 60000)
                        {
                            shootWaveShots = true;
                            shootBow1 = true;
                            shootBow2 = true;
                            shootRotShots = true;
                            worldRotation = false;
                            spawnChildren = true;
                            if (counter["phase"] == 4)
                            {
                                Main.cbox.AddShout(name, "You know nothing!!");
                                Flash(Color.Red, 60, 999999999, false);
                                counter["phase"]++; //first hit turns to phase 5

                                counter["shot1"] = 0;
                                counter["shot2"] = 0;
                                counter["shot3"] = 0;
                                counter["shot4"] = 0;
                                counter["child"] = 0;
                            }
                        }

                        if (worldRotation)
                        {
                            if (speed > 0)
                            {
                                counter["angle"] += .25f * speed;
                                if (counter["angle"] > 360)
                                    counter["angle"] = 0;

                                Vector2 pos = new Vector2(-1, 0);
                                pos = Vector2.Transform(pos * 384, Matrix.CreateRotationZ(MathHelper.ToRadians(counter["angle"])));
                                pos += startLocation;
                                float distFromLength = (pos - position).Length();

                                if (distFromLength > 8 && alive > 20)
                                {
                                    velocity = Vector2.Normalize(pos - position) * 2;
                                    Move(velocity);
                                }
                                else
                                    position = pos;

                                hitbox.MoveTo(center);
                            }

                            if (counter["changedistance"] <= 0 && changeDistance)
                            {
                                counter["distance"] = Main.rand.Next(128, 384);
                                counter["changedistance"] = 1200;   //20 seconds
                            }
                        }
                        else
                        {
                            if (distance > 128 && distance <= 512)
                            {
                                Vector2 moveDirec = Vector2.Normalize(world.player.center - center);
                                velocity = moveDirec * 5;
                                Move(velocity);
                            }
                        }
                        if (flashTotalDuration <= -1 || counter["phase"] == 5)
                        {
                            if (counter["child"] <= 0)
                            {
                                if (spawnChildren)
                                {
                                    int childCount = 0;
                                    foreach (Enemy2 child in children.ToList())
                                    {
                                        if (!child.dead)
                                            childCount++;
                                        else
                                            children.Remove(child);
                                    }
                                    if (childCount < 3)
                                    {
                                        Enemy2 e = Create(8, hitbox.center, false);
                                        children.Add(e);
                                        e.parent = this;
                                        world.CreateEnemy2(e);
                                        counter["child"] = 120;
                                    }
                                }
                            }
                            if (counter["wait1"] <= 0)
                            {
                                if (counter["shot1"] <= 0)
                                {
                                    if (shootWaveShots && !stunned)
                                    {
                                        Projectile p1 = new Projectile(Assets.GetTexture("arrowtail"), Color.RoyalBlue, hitbox.position + new Vector2(8), new Vector2(8, 8), true, 4, counter["shot1angle"], 4, 135, 1028, 45);
                                        p1.noColorTex = Assets.GetTexture("arrow");
                                        p1.givebuffs.Add(new Buff("Weak", 240, Buff.EffectWeakness));

                                        Projectile p2 = new Projectile(Assets.GetTexture("arrowtail"), Color.RoyalBlue, hitbox.position + new Vector2(8), new Vector2(8, 8), true, 4, counter["shot1angle"] + 90, 4, 135, 1028, 45);
                                        p2.noColorTex = Assets.GetTexture("arrow");
                                        p2.givebuffs.Add(new Buff("Weak", 240, Buff.EffectWeakness));

                                        Projectile p3 = new Projectile(Assets.GetTexture("arrowtail"), Color.RoyalBlue, hitbox.position + new Vector2(8), new Vector2(8, 8), true, 4, counter["shot1angle"] + 180, 4, 135, 1028, 45);
                                        p3.noColorTex = Assets.GetTexture("arrow");
                                        p3.givebuffs.Add(new Buff("Weak", 240, Buff.EffectWeakness));

                                        Projectile p4 = new Projectile(Assets.GetTexture("arrowtail"), Color.RoyalBlue, hitbox.position + new Vector2(8), new Vector2(8, 8), true, 4, counter["shot1angle"] + 270, 4, 135, 1028, 45);
                                        p4.noColorTex = Assets.GetTexture("arrow");
                                        p4.givebuffs.Add(new Buff("Weak", 240, Buff.EffectWeakness));

                                        world.CreateProjectile(p1);
                                        world.CreateProjectile(p2);
                                        world.CreateProjectile(p3);
                                        world.CreateProjectile(p4);

                                        counter["shot1"] = 8;
                                        counter["shot1count"]++;

                                        if (counter["shot1count"] >= 0 && counter["shot1count"] < 12)
                                            counter["shot1angle"] += 7.5f;
                                        else
                                            counter["shot1angle"] -= 7.5f;
                                        if (counter["shot1count"] >= 24)
                                        {
                                            counter["shot1count"] = 0;
                                            if (counter["phase"] <= 1)
                                                counter["wait1"] = 120;
                                            else if (counter["phase"] > 1 && counter["phase"] <= 3)
                                                counter["wait1"] = 60;
                                            else if (counter["phase"] >= 4)
                                                counter["wait1"] = 0;
                                        }
                                    }
                                }
                            }
                            else counter["wait1"]--;

                            if (counter["shot2"] <= 0)
                            {
                                if (shootBow1 && !stunned)
                                {
                                    Projectile p = new Projectile(Assets.GetTexture("arrowtail"), Color.Red, hitbox.position + new Vector2(8), new Vector2(8, 16), true, 4, rotToPlayer, 8 + counter["phase"], 135, 1024, 85 + (int)(15 * (counter["phase"] - 1)), true);
                                    p.noColorTex = Assets.GetTexture("arrow");
                                    world.CreateProjectile(p);
                                    counter["shot2"] = 60 / (counter["phase"] == 0 ? 1 : counter["phase"]);
                                }
                            }

                            if (counter["shot3"] <= 0)
                            {
                                if (shootRotShots && !stunned)
                                {
                                    Projectile p1 = new Projectile(Assets.GetTexture("bolt"), Color.Teal, hitbox.position + new Vector2(8), new Vector2(12, 32), true, 6, counter["shot3angle"] + 90, 8, -45, 1024, 120);
                                    Projectile p2 = new Projectile(Assets.GetTexture("bolt"), Color.Teal, hitbox.position + new Vector2(8), new Vector2(12, 32), true, 6, -counter["shot3angle"] + 90, 8, -45, 1024, 120);
                                    world.CreateProjectile(p1);
                                    if (!(counter["shot3angle"] == 90 || counter["shot3angle"] == 180))
                                        world.CreateProjectile(p2); //so we don't get 2 stacked shots
                                    counter["shot3"] = 20;
                                    counter["shot3angle"] += 6f;
                                }
                            }

                            if (counter["shot4"] <= 0)
                            {
                                if (shootBow2 && !stunned)
                                {
                                    Projectile p = new Projectile(Assets.GetTexture("arrowtail"), Color.Black, hitbox.position + new Vector2(8), new Vector2(12, 32), true, 8, rotToPlayer, 3f, 135, 1024, 30);
                                    p.noColorTex = Assets.GetTexture("arrow");
                                    p.givebuffs.Add(new Buff("Armor Broken", 120, Buff.EffectArmorBreak));
                                    world.CreateProjectile(p);
                                    counter["shot4"] = 90;
                                }
                            }
                        }
                    }
                }
                #endregion
                else if (type == 2)
                {
                    counter["shot0"]--;
                    counter["shot1"]--;
                    counter["shot2"]--;

                    if (counter["shot0"] <= 0 && counter["shot1"] <= 120)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            Projectile p = new Projectile(Assets.GetTexture("sun"), Color.White, hitbox.position + new Vector2(8), new Vector2(8), true, 4, counter["angle0"] + i * 45, 3.5f, Main.rand.Next(360), 512, 0, true).SetSpin(-16);
                            world.CreateProjectile(p);
                        }
                        counter["angle0"] += 5;
                        counter["shot0"] = 10;
                    }

                    if (counter["shot1"] <= 0)
                    {
                        for (float i = 0; i < 360; i += 360 / 30)
                        {
                            Projectile p = new Projectile(Assets.GetTexture("redball"), Color.White, hitbox.position + new Vector2(8), new Vector2(8), true, 4, i, 3.5f, 0, 512, 0, true);
                            Projectile p2 = new Projectile(Assets.GetTexture("star"), Color.White, hitbox.position + new Vector2(8), new Vector2(8), true, 5, i, 8, Main.rand.Next(360), 512, 0, true).SetSpin(16);
                            p2.givebuffs.Add(new Buff("Weak", 360, Buff.EffectWeakness));
                            world.CreateProjectile(p);
                            world.CreateProjectile(p2);
                        }
                        counter["shot1"] = 360;
                    }

                    if (counter["shot2"] <= 0 && counter["shot1"] == 120)
                    {
                        for (float i = 0; i < 360; i += 360 / 30)
                        {
                            Projectile p = new Projectile(Assets.GetTexture("redball"), Color.White, hitbox.position + new Vector2(8), new Vector2(8), true, 4, i, 3.5f, 0, 512, 0, true);
                            world.CreateProjectile(p);
                        }
                        counter["shot2"] = 360;
                    }
                }
                #endregion
            }

            if (flips)
            {
                float angle = VectorHelper.FindAngleBetweenTwoPoints(center, world.player.center) + MathHelper.ToDegrees(Main.camera.Rotation);

                if (angle >= 360)
                    angle -= 360;
                if (angle < 0)
                    angle += 360;

                if (angle >= 90 && angle <= 270)
                    flip = false;
                else
                    flip = true;
            }

            if (invulnTicks > 0 )
                invulnTicks--;
            if (invulnTicks <= 0 && !invulnOverride)
                invulnerable = false;

            if (untargetTicks > 0)
                untargetTicks--;
            if (untargetTicks <= 0 && !untOverride)
                untargetable = false;

            for (int i = texts.Count - 1; i >= 0; i--)
            {
                texts[i].center = center;
                texts[i].Update();
                if (texts[i].dead)
                    texts.RemoveAt(i--);
            }
        }

        public void Move(Vector2 moveDirec, float speed)
        {
            position += (moveDirec * speed) * this.speed;
            hitbox.MoveTo(position);
            idleMove = false;
        }

        public void SetIdle(int timer, int speed)
        {
            if (!idleMove)
            {
                idleMove = true;
                idleTimer = timer;
                idleTimerMax = timer;
                idleSpeed = speed;
            }
        }

        private void Flash(Color color, int flashDuration, int totalDuration, bool invuln = false)
        {
            flashColor = color;
            flashDurationMax = flashDuration;
            this.flashDuration = flashDuration;
            flashTotalDuration = totalDuration;

            if (invuln)
                invulnTicks = totalDuration;
        }

        private void Shoot(World world, Projectile p)
        {
            shooting = true;
            currentShootFrame = 0;

            world.CreateProjectile(p);
        }

        /// <summary>
        /// Finds the rotation of the 2 points between this entity instance and the player.
        /// </summary>
        /// <param name="player">the player instance.</param>
        /// <returns>the rotation, in degrees.</returns>
        public float PlayerAngle(Player player)
        {
            return VectorHelper.FindAngleBetweenTwoPoints(center, player.center);
        }

        public override void TakeDamage(int amt, Projectile source, World world)
        {
            if (!invulnerable)
            {
                int numparticles = Main.rand.Next(3, 8);

                if (world.particles.Count + numparticles < 32)
                {
                    for (int i = 0; i <= numparticles; i++)
                    {
                        Color c = texture.GetPixels().GetPixel(Main.rand.Next(0, texture.Width), Main.rand.Next(0, texture.Height), 1);
                        if (c != Color.Transparent)
                        {
                            float randVelX = (float)Main.rand.NextDouble(-2, 2);

                            float randVelY = (float)Main.rand.NextDouble(-2, 2);

                            world.CreateParticle(new Particle(hitbox.center + (Main.camera.down * 4), new Vector2(randVelX, randVelY), c, 4, Main.rand.Next(12, 21)));
                        }
                    }
                }

                if (source != null)
                {
                    if (source.givebuffs != null)
                    {
                        bool foundBuff = false;
                        foreach (Buff gb in source.givebuffs)
                        {
                            foreach (Buff b in buffs)
                            {
                                if (b.name == gb.name)
                                {
                                    if (b.active)
                                    {
                                        foundBuff = true;
                                        if (gb.duration > b.duration)
                                        {
                                            b.duration = gb.duration;  //Refresh the buff's duration
                                        }
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
                }
                int damagePost = CalculateDefenseResist(amt, source.armorPiercing);

                if (source.armorPiercing)
                    texts.Add(new DamageText(damagePost.ToString(), center + new Vector2(0, -texture.Height), Color.Purple));
                else
                    texts.Add(new DamageText(damagePost.ToString(), center + new Vector2(0, -texture.Height), Color.Red));
                if (health - damagePost > 0)
                {
                    if (hitSound != null)hitSound.Play();

                    health -= damagePost;
                    float healthP = (float)health / (float)maxHealth;
                    healthBarWidth = 48 * healthP;
                }
                else if (!dead)
                {
                    dieSound.Play();
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
            texts.Clear();
            
            world.CreateBag(Bag.Create(center, lootItem.Roll()));
            world.player.GiveXp(xpGive);
            dead = true;
        }

        public override void DrawOutline(SpriteBatch batch)
        {
            Vector2 offset = (Main.camera.up * offsetDist) * scale;
            Vector2 offset2 = Vector2.Zero;
            var find = (int)Math.Floor(textureCoord.X);
            offset2 = Main.camera.right * (frameWidths[find] - frameWidths[0]) * scale;

            if (!shooting)
                DrawHelper.DrawOutline(batch, texture, center - texture.Bounds.Center.ToVector2() + offset, sourceRect, DrawHelper.GetTextureOffset(texture.Bounds.Location.ToVector2(), textureSize), 1, flip, scale);
            else
            {
                if (flip)
                DrawHelper.DrawOutline(batch, texture, (center - texture.Bounds.Center.ToVector2() + offset) - offset2, new Rectangle((int)(textureCoord.X * 8), (int)(textureCoord.Y * 8), frameWidths[(int)Math.Floor(textureCoord.X)], 8), DrawHelper.GetTextureOffset(texture.Bounds.Location.ToVector2(), textureSize), 1, flip, scale);
                else
                    DrawHelper.DrawOutline(batch, texture, (center - texture.Bounds.Center.ToVector2() + offset), new Rectangle((int)(textureCoord.X * 8), (int)(textureCoord.Y * 8), frameWidths[(int)Math.Floor(textureCoord.X)], 8), DrawHelper.GetTextureOffset(texture.Bounds.Location.ToVector2(), textureSize), 1, flip, scale);
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            Vector2 offset = (Main.camera.up * offsetDist) * scale;
            Vector2 offset2 = Main.camera.right * (frameWidths[(int)Math.Floor(textureCoord.X)] - frameWidths[0]) * scale;
            batch.Draw(shadowTexture, center, null,
                new Color(0, 0, 0, 127), -Main.camera.Rotation, DrawHelper.GetTextureOffset(shadowTexture.Bounds.Location.ToVector2(), shadowTexture.Bounds.Size.ToVector2()), shadowscale, 0, 0);

            if (!shooting)
                batch.Draw(texture, center + offset, Assets.GetSourceRect(textureCoord, textureSize), color, -Main.camera.Rotation,
                    DrawHelper.GetTextureOffset(texture.Bounds.Location.ToVector2(), textureSize), scale, flip ? SpriteEffects.FlipHorizontally : 0, 0);
            else
            {
                batch.Draw(texture, flip ? center + offset - offset2 : center + offset, new Rectangle((int)(textureCoord.X * 8), (int)(textureCoord.Y * 8), frameWidths[(int)Math.Floor(textureCoord.X)], 8), color, -
                    Main.camera.Rotation, DrawHelper.GetTextureOffset(texture.Bounds.Location.ToVector2(), textureSize), scale,
                    flip ? SpriteEffects.FlipHorizontally : 0, 0);
            }
            //hitbox.DebugDraw(batch);
            //DrawGeometry.DrawHollowRectangle(batch, new Rectangle(position.ToPoint(), new Point(8 * 4)), 1, Color.White);
        }

        public void DrawHealthBar(SpriteBatch batch)
        {
            Color healthbarcolor = Color.Red;

            if (Main.camera.oldCameraRotation != Main.camera.Rotation)
                healthBarPos = Vector2.Transform(healthBarDefault, Matrix.CreateRotationZ(-Main.camera.Rotation));
            if (!untargetable)
            {
                if (invulnerable)
                    healthbarcolor = Color.Black;
                batch.Draw(Assets.GetTexture("whitePixel"), center + healthBarPos, null, Color.Gray, -Main.camera.Rotation, Vector2.Zero, new Vector2(48, 8), SpriteEffects.None, 0);
                batch.Draw(Assets.GetTexture("whitePixel"), center + healthBarPos, null, healthbarcolor, -Main.camera.Rotation, Vector2.Zero, new Vector2(healthBarWidth, 8), SpriteEffects.None, 0);
            }

            foreach (DamageText dt in texts)
            {
                dt.Draw(batch);
            }
        }

        public static float FindPredictiveAngle(Player player, Enemy2 enemy, float projspeed)
        {
            return VectorHelper.FindAngleBetweenTwoPoints(enemy.center, player.center + (player.predictVec * ((player.center - enemy.center).Length() / (projspeed / 2))));
        }

        #region creation
        public static Enemy2 Create(int type, Vector2 position, bool boss = false)
        {
            Enemy2 e = new Enemy2(type, position, boss);
            return e;
        }

        public Enemy2 Copy(Vector2 position)
        {
            Enemy2 e = new Enemy2(type, position);
            return e;
        }
        public Enemy2 Copy()
        {
            Enemy2 e = new Enemy2(type, position);
            return e;
        }
        #endregion
    }
}*/
