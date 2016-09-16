using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;

namespace Limestone
{
    public class Enemy : EntityLiving
    {
        int type;

        public override Vector2 center { get { return new Vector2(position.X + (texture.Width * (scale / 2)), position.Y + (texture.Height * (scale  / 2))); } set { } }

        public RotateableRectangle hitbox;

        private Vector2 offset { get { return new Vector2(hitbox.width / 4, hitbox.height / 4); } }

        public Enemy(int type, Vector2 position, float scale)
        {
            this.position = position;
            this.type = type;
            if (type == 0)
            {
                texture = Assets.GetTexture("boss1");
                counter.Add("lasershot", 0);
                counter.Add("shotgun", 0);

                health = 30;
            }
            else if (type == 1)
            {
                texture = Assets.GetTexture("boss1");
                counter.Add("spiralshot", 0);
                counter.Add("angle", 0);

                health = 30;
            }
            else if (type == 2)
            {
                texture = Assets.GetTexture("boss1");
                counter.Add("shotgun", 128);
                counter.Add("phase", 10);
                counter.Add("colorflash", 0);
            }
            this.scale = scale;
            hitbox = new RotateableRectangle(new Rectangle(position.ToPoint(), new Point(64)));
        }

        public override void Update(World world)
        {
            if (type == 0)
            {
                counter["lasershot"]--;

                if (counter["shotgun"] > 0)
                    counter["shotgun"]--;

                if (counter["lasershot"] <= 0)
                {
                    counter["lasershot"] = 100;

                    world.CreateProjectile(1, 20, position + offset, VectorHelper.FindAngleBetweenTwoPoints(center, world.player.center), 12, 512, 4);
                }

                if ((world.player.center - center).Length() <= 128)
                {
                    if (counter["shotgun"] <= 0)
                    {
                        counter["shotgun"] = 40;
                        //Bolts
                        world.CreateProjectile(0, 80, position + offset, VectorHelper.FindAngleBetweenTwoPoints(center, world.player.center), 4, 128, 4);
                        world.CreateProjectile(0, 80, position + offset, VectorHelper.FindAngleBetweenTwoPoints(center, world.player.center) - 15, 4, 128, 4);
                        world.CreateProjectile(0, 80, position + offset, VectorHelper.FindAngleBetweenTwoPoints(center, world.player.center) + 15, 4, 128, 4);
                        //Shields
                        world.CreateProjectile(2, 5, position + offset, VectorHelper.FindAngleBetweenTwoPoints(center, world.player.center), 5, 128, 4);
                        world.CreateProjectile(2, 5, position + offset, VectorHelper.FindAngleBetweenTwoPoints(center, world.player.center) - 7.5f, 5, 128, 4);
                        world.CreateProjectile(2, 5, position + offset, VectorHelper.FindAngleBetweenTwoPoints(center, world.player.center) - 15, 5, 128, 4);
                        world.CreateProjectile(2, 5, position + new Vector2(16, 16), VectorHelper.FindAngleBetweenTwoPoints(center, world.player.center) + 7.5f, 5, 128, 4);
                        world.CreateProjectile(2, 5, position + new Vector2(16, 16), VectorHelper.FindAngleBetweenTwoPoints(center, world.player.center) + 15, 5, 128, 4);
                    }
                }
            }
            /*else if (type == 1)
            {
                counter["spiralshot"]--;

                if (counter["spiralshot"] <= 0)
                {
                    counter["spiralshot"] = 10;

                    world.CreateProjectile(0, center, counter["angle"], 5, 256);
                    world.CreateProjectile(0, center, counter["angle"] + 90, 5, 256);
                    world.CreateProjectile(0, center, counter["angle"] + 180, 5, 256);
                    world.CreateProjectile(0, center, counter["angle"] + 270, 5, 256);

                    if (counter["angle"] < 360)
                        counter["angle"] += 8;
                    else counter["angle"] = 0;
                }
            }
            else if (type == 2)
            {
                if (counter["phase"] < 6)
                {
                    counter["shotgun"]--;

                    if (counter["shotgun"] <= 0)
                    {
                        counter["shotgun"] = 20;

                        if (counter["phase"] < 3)
                        {
                            for (float i = 0; i < 123.75; i += 11.25f)
                                world.CreateProjectile(4, center, i + 33.75f, 8, 512);
                            for (float i = 0; i < 123.75; i += 11.25f)
                                world.CreateProjectile(4, center, i + 213.75f, 8, 512);

                            if (counter["phase"] == 2)
                                counter["shotgun"] = 100;   //after 3 shots, delay so player can react
                        }
                        else
                        {
                            for (float i = 0; i < 123.75; i += 11.25f)
                                world.CreateProjectile(4, center, i + 123.75f, 8, 512);
                            for (float i = 0; i < 123.75; i += 11.25f)
                                world.CreateProjectile(4, center, i + 303.75f, 8, 512);

                            if (counter["phase"] == 5)
                            {
                                counter["shotgun"] = 180;
                            }
                        }
                        counter["phase"]++;
                    }
                }
                else if (counter["phase"] == 6)
                {
                    counter["shotgun"]--;

                    counter["colorflash"]--;

                    if (counter["colorflash"] <= 0)
                    {
                        if (color.G == 255)
                            color.G = 130;
                        else color.G = 255;

                        if (color.B == 255)
                            color.B = 130;
                        else color.B = 255;

                        counter["colorflash"] = 5;
                    }

                    if (counter["shotgun"] <= 0)
                        counter["phase"]++;
                }
                else if (counter["phase"] > 6 && counter["phase"] < 10)
                {
                    counter["shotgun"]--;

                    if (counter["shotgun"] <= 0)
                    {
                        counter["shotgun"] = 60;

                        world.CreateProjectile(5, center, VectorHelper.FindAngleBetweenTwoPoints(center, world.player.center), 5, 192).color = Color.Yellow;

                        counter["phase"]++;
                    }
                }
                else if (counter["phase"] >= 10 && counter["phase"] < 12)
                {
                    counter["shotgun"]--;

                    counter["colorflash"]--;

                    if (counter["colorflash"] <= 0)
                    {
                        if (color.G == 255)
                            color.G = 130;
                        else color.G = 255;

                        if (color.B == 255)
                            color.B = 130;
                        else color.B = 255;

                        counter["colorflash"] = 5;
                    }

                    if (counter["shotgun"] <= 0)
                        counter["phase"]++;
                    counter["counterflash"] = 0;
                }
                else if (counter["phase"] >= 12)
                {
                    counter["shotgun"]--;

                    if (counter["shotgun"] <= 0)
                    {
                        counter["shotgun"] = 12;
                        counter["counterflash"] += 8;

                        for (int i = 0; i <= 315; i += 45)
                        {
                            world.CreateProjectile(6, center, counter["counterflash"] + i, 2, 256);
                            world.CreateProjectile(6, center, -counter["counterflash"] + i, 2, 256);
                        }

                        for (int i = 0; i < 360; i += 10)
                        {
                            world.CreateProjectile(2, center, counter["counterflash"] + i, 1.2f, 48);
                            world.CreateProjectile(4, center, counter["counterflash"] + i, 20, 128);
                        }
                    }
                }
            }*/
        }

        public override void TakeDamage(int amt, Projectile source)
        {
            if (health > 0)
            {
                health -= amt;
            }
        }

        public override void Die(World world)
        {
        }

        public override void DrawOutline(SpriteBatch batch)
        {
            DrawHelper.DrawOutline(batch, texture, position, DrawHelper.GetTextureOffset(texture), scale, 1);
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(texture, position + texture.Bounds.Center.ToVector2() * scale, null, color, -Main.camera.Rotation, DrawHelper.GetTextureOffset(texture), scale, 0, 0);

            hitbox.DebugDraw(batch);
        }
    }
}
