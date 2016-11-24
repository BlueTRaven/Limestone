using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;
using Limestone.Tiles;
using Limestone.Items;

using Limestone.Interface;
using Limestone.Guis;

namespace Limestone.Entities.Enemies
{
    public enum CardinalDirection
    {
        North,
        South,
        East,
        West
    }

    public class EnemyBossCardguy : Enemy
    {
        private int healthLost;

        private float facingDirection;

        public EnemyBossCardguy(Vector2 position) : base(position)
        { 
            SetDefaults();
        }

        private void SetDefaults()
        {
            scale = 4;
            hitbox = new RotateableRectangle(new Rectangle(position.ToPoint(), new Point(32)));
            activeDistance = 5120;
            health = 10;
            maxHealth = 10;

            setSize = new Rectangle(0, 0, 8, 8);
        }

        public override void Update(World world)
        {
            base.Update(world);

            healthLost = maxHealth - health;

            if (currentMove == null || currentMove.finished)
            {
                if (moveQueue.Count <= 0)
                    ChooseNextMove(world);

                previousMove = currentMove;
                currentMove = moveQueue.Dequeue();   //Set current style to the next style in the queue.
            }

            currentMove.Update();
        }

        private void ChooseNextMove(World world)
        {
            int next = Main.rand.Next(3);

            if (next >= 0 && next < 3)
            {
                switch (next)
                {
                    default:
                        {
                            moveQueue.Enqueue(new MoveStyle(0, () => ShootTest(world)));
                            moveQueue.Enqueue(new MoveStyle(600, MoveWait));
                            break;
                        }
                    /*case 2:
                        {
                            int value = (int)Main.rand.Next(2, 8) / 2;

                            for (int i = 1; i <= value; i++)
                            {
                                moveQueue.Enqueue(new MoveStyle(0, () => MoveTeleport(world)));
                                moveQueue.Enqueue(new MoveStyle(30 - (int)(healthLost * 1.5f), MoveWait));
                                moveQueue.Enqueue(new MoveStyle(50, () => MoveCharge(world)));

                                if (i != value)
                                    moveQueue.Enqueue(new MoveStyle(15, MoveWait));
                            }
                            moveQueue.Enqueue(new MoveStyle(120, MoveWait));

                            moveQueue.Enqueue(new MoveStyle(0, () => MoveReturn(world)));
                            moveQueue.Enqueue(new MoveStyle(120, MoveWait));
                            break;
                        }*/
                    /*case 1:
                        {
                            int value = (int)Main.rand.Next(2, 8) / 2;
                            Console.WriteLine("numqueued: " + value);
                            for (float i = 1; i <= value; i++)
                            {
                                moveQueue.Enqueue(new MoveStyle(0, () => MoveTeleport(world)));
                                moveQueue.Enqueue(new MoveStyle(30 - (int)(healthLost * 1.5f), MoveWait));
                                moveQueue.Enqueue(new MoveStyle(0, () => MoveThrowCards(world)));
                                moveQueue.Enqueue(new MoveStyle(120 - (int)(healthLost * 4f), MoveWait));
                            }

                            moveQueue.Enqueue(new MoveStyle(0, () => MoveReturn(world)));
                            moveQueue.Enqueue(new MoveStyle(120, MoveWait));
                            break;
                        }*/
                }
            }
        }

        #region moves
        private void ShootTest(World world)
        {
            Projectile p = new Projectile(Assets.GetTexFromSource("projectilesFull", 0, 0), Color.White, 4, center, Vector2.Zero, new Vector2(8, 32), rotToPlayer, 45, 2, 10240, 1);
            p.deathFunction = (x => 
            {
                if (!x.hitEntities.Contains(world.player))
                {
                    Projectile p2 = new Projectile(Assets.GetTexFromSource("projectilesFull", 0, 0), Color.White, 4, x.center, Vector2.Zero, new Vector2(8, 32), -x.angle, 45, 2, 10240, 1);
                    p2.invulnTicks = 60;
                    p2.tileCollides = false;
                    world.CreateProjectile(p2);
                }
            });
            world.CreateProjectile(p);
        }

        private void MovePlayer(World world)
        {
            float dist = (world.player.position - position).Length();

            float angle = VectorHelper.GetAngleBetweenPoints(position, world.player.position);

            int card = Main.rand.Next(4);
            FrameConfiguration conf = new FrameConfiguration(FrameConfiguration.FrameActionPreset3, null,
                                        new FrameCollection(false,
                                        new Frame(5, new Rectangle(0, card * 8, 8, 8)),
                                        new Frame(5, new Rectangle(8, card * 8, 8, 8)),
                                        new Frame(5, new Rectangle(16, card * 8, 8, 8)),
                                        new Frame(5, new Rectangle(24, card * 8, 8, 8)),
                                        new Frame(5, new Rectangle(32, card * 8, 8, 8)),
                                        new Frame(5, new Rectangle(40, card * 8, 8, 8)),
                                        new Frame(5, new Rectangle(48, card * 8, 8, 8)),
                                        new Frame(5, new Rectangle(56, card * 8, 8, 8))));

            Projectile p = new Projectile(conf, Assets.GetTexture("framesCard1"), Color.White, 4, center, Vector2.Zero, new Vector2(8, 4), angle, -135, 8, dist, 1);
            world.CreateProjectile(p);

            position = world.player.position;
        }

        private void MoveCharge(World world)
        {
            Move(VectorHelper.GetAngleNormVector(facingDirection + 180) * 8);

            if (alive % 4 == 0)
            {
                Projectile p = new Projectile(Assets.GetTexture("whitePixel"), Color.White, 0, center, Vector2.Zero, new Vector2(32, 32), 0, 0, .25f, 1, 1);
                world.CreateProjectile(p);
            }
        }

        private void MoveReturn(World world)
        {
            float dist = (startPosition - position).Length();

            float angle = VectorHelper.GetAngleBetweenPoints(position, startPosition);

            int card = Main.rand.Next(4);
            FrameConfiguration conf = new FrameConfiguration(FrameConfiguration.FrameActionPreset3, null,
                                        new FrameCollection(false,
                                        new Frame(5, new Rectangle(0, card * 8, 8, 8)),
                                        new Frame(5, new Rectangle(8, card * 8, 8, 8)),
                                        new Frame(5, new Rectangle(16, card * 8, 8, 8)),
                                        new Frame(5, new Rectangle(24, card * 8, 8, 8)),
                                        new Frame(5, new Rectangle(32, card * 8, 8, 8)),
                                        new Frame(5, new Rectangle(40, card * 8, 8, 8)),
                                        new Frame(5, new Rectangle(48, card * 8, 8, 8)),
                                        new Frame(5, new Rectangle(56, card * 8, 8, 8))));

            Projectile p = new Projectile(conf, Assets.GetTexture("framesCard1"), Color.White, 4, center, Vector2.Zero, new Vector2(8, 4), angle, -135, 8, dist, 1);
            world.CreateProjectile(p);

            invulnTicks = 120;

            position = startPosition;
        }

        private void MoveTeleport(World world)
        {
            int next = Main.rand.Next(4);
            Console.WriteLine(next);

            facingDirection = next * 90;
            Vector2 nextPos = VectorHelper.GetAngleNormVector(facingDirection) * 196;

            Bomb b = new Bomb(null, Color.Red, 16, center, new Vector2(4), 128, 0, 0, 0, 0, .01f, 1, 1);
            world.CreateBomb(b);

            position = world.player.center + nextPos;
        }

        private void MoveThrowCards(World world)
        {
            int card = Main.rand.Next(4);

            for (int i = -10; i <= 10; i += 10)
            {
                card = Main.rand.Next(4);
                FrameConfiguration conf = new FrameConfiguration(FrameConfiguration.FrameActionPreset3, null,
                                            new FrameCollection(false,
                                            new Frame(5, new Rectangle(0, card * 8, 8, 8)),
                                            new Frame(5, new Rectangle(8, card * 8, 8, 8)),
                                            new Frame(5, new Rectangle(16, card * 8, 8, 8)),
                                            new Frame(5, new Rectangle(24, card * 8, 8, 8)),
                                            new Frame(5, new Rectangle(32, card * 8, 8, 8)),
                                            new Frame(5, new Rectangle(40, card * 8, 8, 8)),
                                            new Frame(5, new Rectangle(48, card * 8, 8, 8)),
                                            new Frame(5, new Rectangle(56, card * 8, 8, 8))));

                Projectile p = new Projectile(conf, Assets.GetTexture("framesCard1"), Color.White, 4, center, Vector2.Zero, new Vector2(8, 4), facingDirection + 180 + (i * 5), -135, 8, 1024, 1);
                world.CreateProjectile(p);
            }
        }

        private void MoveThrowDirectionalCards(World world, int time)
        {
            float offset = (float)Main.rand.NextDouble(0, 360);

            for (float i = 0; i < 360f; i += 360f / 12f)
            {
                int card = Main.rand.Next(4);
                FrameConfiguration conf = new FrameConfiguration(FrameConfiguration.FrameActionPreset3, null,
                                            new FrameCollection(false,
                                            new Frame(5, new Rectangle(0, card * 8, 8, 8)),
                                            new Frame(5, new Rectangle(8, card * 8, 8, 8)),
                                            new Frame(5, new Rectangle(16, card * 8, 8, 8)),
                                            new Frame(5, new Rectangle(24, card * 8, 8, 8)),
                                            new Frame(5, new Rectangle(32, card * 8, 8, 8)),
                                            new Frame(5, new Rectangle(40, card * 8, 8, 8)),
                                            new Frame(5, new Rectangle(48, card * 8, 8, 8)),
                                            new Frame(5, new Rectangle(56, card * 8, 8, 8))));

                Projectile p = new Projectile(conf, Assets.GetTexture("framesCard1"), Color.White, 4, center, Vector2.Zero, new Vector2(8, 4), i + offset, -135, 8, 1024, 1);
                world.CreateProjectile(p);
            }
        }

        private void MoveThrowBombs(World world)
        {
            for (float i = 0; i < 360f; i += 360f / 12f)
            {
                Bomb b = new Bomb(null, Color.Red, 8, center, new Vector2(4), 64, 0, 0, i, 0, 2, 128, 1);
                world.CreateBomb(b);
            }

        }

        private void MoveThrowDice(World world)
        {
            Projectile p = new Projectile(Assets.GetTexFromSource("projectilesFull", Main.rand.Next(2), 3), Color.White, 4, center, Vector2.Zero, new Vector2(16), rotToPlayer, (float)Main.rand.NextDouble(0, 360), 4, 272, 1).SetSpin(10).SetWavy(256, .015f, false);
            world.CreateProjectile(p);
            p = new Projectile(Assets.GetTexFromSource("projectilesFull", Main.rand.Next(2), 3), Color.White, 4, center, Vector2.Zero, new Vector2(16), rotToPlayer, (float)Main.rand.NextDouble(0, 360), 4, 272, 1).SetSpin(10).SetWavy(256, .015f, true);
            world.CreateProjectile(p);

        }

        private void MoveWait()
        {
        }
#endregion

        public override void TakeDamage(int amt, IDamageDealer source, World world)
        {
            base.TakeDamage(amt, source, world);

            GuiNone none = (GuiNone)Main.camera.activeGui;
            if (dead)
            {
                //TODO death dialogue
            }
        }

        public override Entity Copy()
        {
            return new EnemyBossCardguy(position);
        }
    }
}
