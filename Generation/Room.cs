using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;
using Limestone.Tiles;
using Limestone.Entities;

namespace Limestone.Generation
{
    public class Room
    {
        public Rectangle bounds;
        public List<Rectangle> adjancentsBounds = new List<Rectangle>();
        public List<Rectangle> adjancentsDoorsBounds = new List<Rectangle>();
        public bool hasValidAdjacent;

        public List<Rectangle> potentialEndRoom = new List<Rectangle>();
        public bool foundEnd = false;

        public List<Room> adjacentRooms = new List<Room>();
        public List<Door> adjacentDoors = new List<Door>();

        public bool accessByMiddle = false; //only used when generating a endroom

        public bool isDeadEndRoom, isStartRoom, isEndRoom;
        public Room(Rectangle bounds)
        {
            this.bounds = bounds;
        }

        public void FindValidEndRoom(Rectangle maxbounds, Rectangle roomBounds, int doorWidth, List<Room> rooms, string validDirections, bool accessByMiddle)
        {
            this.accessByMiddle = accessByMiddle;
            List<Rectangle> potentialEndRoom = new List<Rectangle>();
            string lower = validDirections.ToLower();

            adjancentsDoorsBounds.Clear();
            adjancentsBounds.Clear();

            foundEnd = true;
            if (lower.Contains("left"))
            {
                if (accessByMiddle)
                {
                    Rectangle doorRect = new Rectangle(bounds.X - 1, bounds.Y + ((bounds.Height / 2) - (doorWidth / 2)), 1, doorWidth);
                    Rectangle roomRect = new Rectangle(bounds.X - 1 - roomBounds.Width, bounds.Y + ((bounds.Height / 2) - (roomBounds.Height / 2)), roomBounds.Width, roomBounds.Height);

                    bool intersectsany = false;
                    foreach (Room r in rooms)
                    {
                        if (roomRect.Intersects(r.bounds))
                        {
                            intersectsany = true;
                            break;
                        }

                    }

                    if (!maxbounds.Contains(roomRect))
                    {
                        intersectsany = true;
                    }

                    if (!intersectsany)
                    {
                        adjancentsBounds.Add(roomRect);
                        adjancentsDoorsBounds.Add(doorRect);
                    }
                }
            }

            if (lower.Contains("up"))
            {
                if (accessByMiddle)
                {
                    Rectangle doorRect = new Rectangle(bounds.X + ((bounds.Width / 2) - (doorWidth / 2)), bounds.Y - 1, doorWidth, 1);
                    Rectangle roomRect = new Rectangle(bounds.X + ((bounds.Width / 2) - (roomBounds.Width / 2)), bounds.Y - 1 - roomBounds.Height, roomBounds.Width, roomBounds.Height);

                    bool intersectsany = false;
                    foreach (Room r in rooms)
                    {
                        if (roomRect.Intersects(r.bounds))
                        {
                            intersectsany = true;
                            break;
                        }

                    }

                    if (!maxbounds.Contains(roomRect))
                    {
                        intersectsany = true;
                    }

                    if (!intersectsany)
                    {
                        adjancentsBounds.Add(roomRect);
                        adjancentsDoorsBounds.Add(doorRect);
                    }
                }
            }

            if (lower.Contains("right"))
            {
                if (accessByMiddle)
                {
                    Rectangle doorRect = new Rectangle(bounds.X + bounds.Width, bounds.Y + ((bounds.Height / 2) - (doorWidth / 2)), 1, doorWidth);
                    Rectangle roomRect = new Rectangle(bounds.X + bounds.Width + 1, bounds.Y + ((bounds.Height / 2) - (roomBounds.Height / 2)), roomBounds.Width, roomBounds.Height);

                    bool intersectsany = false;
                    foreach (Room r in rooms)
                    {
                        if (roomRect.Intersects(r.bounds))
                        {
                            intersectsany = true;
                            break;
                        }
                    }

                    if (!maxbounds.Contains(roomRect))
                    {
                        intersectsany = true;
                    }

                    if (!intersectsany)
                    {
                        adjancentsBounds.Add(roomRect);
                        adjancentsDoorsBounds.Add(doorRect);
                    }
                }
            }

            if (lower.Contains("down"))
            {
                if (accessByMiddle)
                {
                    Rectangle doorRect = new Rectangle(bounds.X + ((bounds.Width / 2) - (doorWidth / 2)), bounds.Y + bounds.Height, doorWidth, 1);
                    Rectangle roomRect = new Rectangle(bounds.X + ((bounds.Width / 2) - (roomBounds.Width / 2)), bounds.Y + bounds.Height + 1, roomBounds.Width, roomBounds.Height);

                    bool intersectsany = false;
                    foreach (Room r in rooms)
                    {
                        if (roomRect.Intersects(r.bounds))
                        {
                            intersectsany = true;
                            break;
                        }

                    }

                    if (!maxbounds.Contains(roomRect))
                    {
                        intersectsany = true;
                    }

                    if (!intersectsany)
                    {
                        adjancentsBounds.Add(roomRect);
                        adjancentsDoorsBounds.Add(doorRect);
                    }
                }
            }

            if (adjancentsBounds.Count == 0)
            {
                foundEnd = false;
                return;
            }
            this.potentialEndRoom = potentialEndRoom;
        }

        public void FindValidAdjacents(Rectangle maxBounds, Rectangle roomBounds, int doorWidth, List<Room> rooms)
        {
            adjancentsBounds.Clear();
            adjancentsDoorsBounds.Clear();
            hasValidAdjacent = false;
            //Left
            for (int y = 0; y < bounds.Height - doorWidth + 1; y++)
            {
                Rectangle doorRect = new Rectangle(bounds.X - 1, bounds.Y + y, 1, doorWidth);
                Rectangle roomRect = new Rectangle(bounds.X - 1 - roomBounds.Width, bounds.Y + ((doorRect.Height / 2) - (roomBounds.Height / 2)) + y, roomBounds.Width, roomBounds.Height);

                bool intersectsany = false;
                foreach (Room r in rooms)
                {
                    if (roomRect.Intersects(r.bounds))
                    {
                        intersectsany = true;
                        break;
                    }

                }

                if (!maxBounds.Contains(roomRect))
                {
                    intersectsany = true;
                }
           
                if (intersectsany)
                    continue;
                else
                {
                    adjancentsBounds.Add(roomRect);
                    adjancentsDoorsBounds.Add(doorRect);
                }
            }

            //up
            for (int x = 0; x < bounds.Width - doorWidth + 1; x++)
            {
                Rectangle doorRect = new Rectangle(bounds.X + x, bounds.Y - 1, doorWidth, 1);
                Rectangle roomRect = new Rectangle(bounds.X + ((doorRect.Width / 2) - (roomBounds.Width / 2)) + x, bounds.Y - 1 - roomBounds.Height, roomBounds.Width, roomBounds.Height);

                bool intersectsany = false;
                foreach (Room r in rooms)
                {
                    if (roomRect.Intersects(r.bounds))
                    {
                        intersectsany = true;
                        break;
                    }

                }

                if (!maxBounds.Contains(roomRect))
                {
                    intersectsany = true;
                }

                if (intersectsany)
                    continue;
                else
                {
                    adjancentsBounds.Add(roomRect);
                    adjancentsDoorsBounds.Add(doorRect);
                }
            }

            //right
            for (int y = 0; y < bounds.Height - doorWidth + 1; y++)
            {
                Rectangle doorRect = new Rectangle(bounds.X + bounds.Width, bounds.Y + y, 1, doorWidth);
                Rectangle roomRect = new Rectangle(bounds.X + 1 + bounds.Width, bounds.Y + ((doorRect.Height / 2) - (roomBounds.Height / 2)) + y, roomBounds.Width, roomBounds.Height);

                bool intersectsany = false;
                foreach (Room r in rooms)
                {
                    if (roomRect.Intersects(r.bounds))
                    {
                        intersectsany = true;
                        break;
                    }

                }

                if (!maxBounds.Contains(roomRect))
                {
                    intersectsany = true;
                }

                if (intersectsany)
                    continue;
                else
                {
                    adjancentsBounds.Add(roomRect);
                    adjancentsDoorsBounds.Add(doorRect);
                }
            }

            //down
            for (int x = 0; x < bounds.Width - doorWidth + 1; x++)
            {
                Rectangle doorRect = new Rectangle(bounds.X + x, bounds.Y + bounds.Height, doorWidth, 1);
                Rectangle roomRect = new Rectangle(bounds.X + ((doorRect.Width / 2) - (roomBounds.Width / 2)) + x, bounds.Y + 1 + bounds.Height, roomBounds.Width, roomBounds.Height);

                bool intersectsany = false;
                foreach (Room r in rooms)
                {
                    if (roomRect.Intersects(r.bounds))
                    {
                        intersectsany = true;
                        break;
                    }

                }

                if (!maxBounds.Contains(roomRect))
                {
                    intersectsany = true;
                }

                if (intersectsany)
                    continue;
                else
                {
                    adjancentsBounds.Add(roomRect);
                    adjancentsDoorsBounds.Add(doorRect);
                }
            }

            if (adjancentsBounds.Count > 0)
                hasValidAdjacent = true;
        }

        public void AddAdjacent(Room room, Door door)
        {
            adjacentRooms.Add(room);
            adjacentDoors.Add(door);
        }
    }
}
