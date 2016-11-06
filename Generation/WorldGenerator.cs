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
    public class Chunk
    {
        List<Tile> tiles = new List<Tile>();
        public Vector2 center;
        public bool edge;

        List<Chunk> adjacent = new List<Chunk>();
    }

    public class VoroniPoly
    {
        public List<VoroniPoly> adjacent = new List<VoroniPoly>();
        public List<Tile> tiles = new List<Tile>();
        public Vector2 center;
        public bool edge = false;

        public int index;
        public VoroniPoly(int index, Vector2 center)
        {
            this.index = index;
            this.center = center;
        }

        public bool Contains(Tile tile)
        {
            return tiles.Contains(tile);
        }

        public void Fill(Tile tile)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                Tile copy = tile.Copy(tiles[i].position);
                tiles[i] = copy;
            }
        }
    }
    public class VoronoiEdge
    {
        Vector2 left, right;

        public VoronoiEdge(Vector2 left, Vector2 right)
        {
            this.left = left;
            this.right = right;
        }
    }
    public delegate Tile[,] GeneratorFunction(Tile[,] tiles);
    public class WorldGenerator
    {
        /*public static volatile string worldGenText = "NULL";

        private List<VoroniPoly> polys = new List<VoroniPoly>();
        private VoroniPoly centerPoly;

        private Tile[,] tiles;
        private Chunk[,] chunks;
        private World world;

        private Coordinate centerTile { get { return new Coordinate(tiles.GetLength(0) / 2, tiles.GetLength(1) / 2); } }
        public WorldGenerator(World world, Coordinate size)//, GeneratorFunction function)
        {
            tiles = new Tile[size.x, size.y];
            //chunks = new Chunk[size.x / 8, size.y / 8];
            //function(tiles);
            this.world = world;
            worldGenText = "Starting...";
        }

        public void Create()
        {
            worldGenText = "Filling world with water tiles...";
            GenerateOceanFill(tiles);

            //Create();
            CreatePolys(6);
            BindTilesToPolygons();
            //DeterminePolyAdjacents();
            foreach (VoroniPoly poly in polys)
            {
                int rand = Main.rand.Next(1, 7); 
                poly.Fill(TileFloor.Create(Coordinate.Zero, "beach" + rand, (Biomes)rand));
            }
            MovePolysToWorld();
            worldGenText = "DONE!";
        }

        public void DeterminePolyAdjacents()
        {
            worldGenText = "Determining adjacency...";
            bool adjacent = true;
            int index = -1;
            List<VoroniPoly> done = new List<VoroniPoly>();
            foreach (VoroniPoly poly in polys)
            {
                index++;
                worldGenText = "Determining adjacency of poly " + index;
                foreach (VoroniPoly polyo in polys) //For each polygon, we loop through all the others
                {
                    if (polyo.index != poly.index)  //if their indexes are not equal, so it doesn't see itsself as adjacent.
                    {
                        float l = (poly.center - polyo.center).Length();
                        float rot = VectorHelper.FindAngleBetweenTwoPoints(poly.center, polyo.center);
                        Vector2 a = new Vector2(1, 0);
                        a = Vector2.Transform(a, Matrix.CreateRotationZ(MathHelper.ToRadians(rot)));
                        a.Normalize();

                        Vector2 near = poly.center + (a * ((l / 2) - 1));
                        Vector2 far = poly.center + (a * ((l / 2) + 1));


                        //Vector2 mid = VectorHelper.GetMidPoint(poly.center, polyo.center);
                        //float l = mid.Length();


                        /*foreach (VoroniPoly adj in poly.adjacent)
                        {   //if one of the polygons is already determined to be adjacent to the other, we can skip it.
                            if (adj.index == polyo.index) break;
                        }
                        foreach (VoroniPoly adj in polyo.adjacent)
                        {
                            if (adj.index == poly.index) break;
                        }
                        List<Tile> ts = World.GetTilesAlongLine(tiles, poly.center, polyo.center, 0); 

                        foreach (Tile tile in ts)
                        {   //for each tile, we check its polygon's index to see if it is the current poly or the current "other" poly. if it's neither, we can say the two aren't adjacent
                            if (tile.poly.index == poly.index || tile.poly.index == polyo.index)
                                continue;
                            else
                                adjacent = false; break;
                        }
                        if (adjacent)
                        {   //add both to the other's adjacent lists.
                            poly.adjacent.Add(polyo);
                            polyo.adjacent.Add(poly);

                            done.Add(poly);
                            done.Add(polyo);
                        }
                    }
                }
            }
        }

        private void CreatePolys(int num)
        {
            worldGenText = "Generating Voronoi points...";
            for (int i = 0; i < num; i++)
            {
                int pointX = Main.rand.Next(0, tiles.GetLength(0)) * Coordinate.coordSize;
                int pointY = Main.rand.Next(0, tiles.GetLength(1)) * Coordinate.coordSize;

                polys.Add(new VoroniPoly(i, new Vector2(pointX, pointY)));
                worldGenText = "Generating Voronoi points: " + i + " out of " + num;
            }
        }

        private void BindTilesToPolygons()
        {
            worldGenText = "binding tiles to polygons...";
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    VoroniPoly shortest = null;
                    foreach(VoroniPoly poly in polys)
                    {
                        if (shortest == null)
                            shortest = poly;    //if there's no shortest currently set, set it to the first one it goes through.
                        else
                        {   //otherwise
                            Coordinate polyCenterCoord = poly.center.ToCoordinate();    //get the coordinate centers so I can convert these centers to vector2 *without* converting them to vector space
                            Coordinate polyCenterCoord2 = shortest.center.ToCoordinate();
                            float distance = (new Vector2(polyCenterCoord.x, polyCenterCoord.y) - new Vector2(x, y)).Length();  //get the distance in coordinate space.
                            float distFromShortest = (new Vector2(polyCenterCoord2.x, polyCenterCoord2.y) - new Vector2(x, y)).Length();

                            if (distance < distFromShortest)
                            {//if the current shortest is less than the current iteration's shortest,
                                shortest = poly;                //set the current to be the shortest.
                            }
                        }
                    }
                    Tile t = TileFloor.Create(new Coordinate(x, y));
                    tiles[x, y] = t;
                    shortest.tiles.Add(tiles[x, y]);

                    /*for (int a = -1; a <= 1; a++)   //now we check to see if it's an edge tile.
                    {
                        for (int b = -1; b <= 1; b++)
                        {
                            bool isOutX = tiles[x, y].position.x + a < 0 || tiles[x, y].position.x + a >= tiles.GetLength(0);  //find if any of the tiles inside the polygon 
                            bool isOutY = tiles[x, y].position.y + b < 0 || tiles[x, y].position.y + b >= tiles.GetLength(1);   //are bordering an edge of the tiles array.
                            if (isOutX || isOutY)   //if they are, set the whole poly to be an edge.
                                shortest.edge = true;
                            /*else if (!isOutX && !isOutY)
                            {
                                Tile ct = tiles[x + a, y + b];
                                if (ct != null && ct.poly != shortest)
                                {
                                    tiles[x, y].border = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void MovePolysToWorld()
        {
            worldGenText = "Moving polygon tiles to worldgenerator array...";
            foreach (VoroniPoly p in polys)
            {
                foreach(Tile t in p.tiles)
                {
                    tiles[t.position.x, t.position.y] = t;
                }
            }
        }

        public Rectangle MoveTilesToWorld(Coordinate position)
        {
            foreach (Tile t in tiles)
            {
                if (t != null)
                {
                    t.position += position;
                }
                world.CreateTile(t);
            }

            return new Rectangle(new Point(position.x, position.y), new Point(tiles.GetLength(0), tiles.GetLength(1)));
        }

        #region oldgen
        public static Tile[,] MoveToTilemap(Tile[,] toTilemap, Tile[,] fromTilemap, Coordinate position = null)
        {
            foreach (Tile t in fromTilemap)
            {
                if (t != null)
                {
                    toTilemap[t.position.x, t.position.y] = t;
                }
            }

            return toTilemap;
        }

        public static Tile[,] GenerateOceanFill(Tile[,] tiles)
        {
            //List<Tile> returnTiles = new List<Tile>();

            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    tiles[x, y] = TileFloor.Create(new Coordinate(x, y), "water1", Biomes.Sea);
                    //returnTiles.Add(tiles[x, y]);
                }
            }

            return tiles;
        }

        public void GenerateIsland()
        {
            GenerateOceanFill(tiles);
            float b = 35840;
            worldGenText = "Generating lowlands...";
            GenerateRand(tiles, TileFloor.Create(Coordinate.Zero, "grassLowlands1", Biomes.LowLands), new Vector2(36864), new Vector2(256, 256), new Vector2(b * .80f, b), 2048, true, true); //Note that min is 6273 instead of 6272 because it will generate odd beaches if the midlands overlap the beach
            worldGenText = "Generating midlands...";
            GenerateRand(tiles, TileFloor.Create(Coordinate.Zero, "grassMidlands1", Biomes.MidLands), new Vector2(36864), new Vector2(256, 256), new Vector2(b * .60f, b * .78f), 2048, true, true);
            worldGenText = "Generating highlands...";
            GenerateRand(tiles, TileFloor.Create(Coordinate.Zero, "grassHighlands1", Biomes.HighLands), new Vector2(36864), new Vector2(256, 256), new Vector2(b * .40f, b * .58f), 2048, true, true);
            worldGenText = "Generating ancientlands...";
            GenerateRand(tiles, TileFloor.Create(Coordinate.Zero, "grassAncientlands1", Biomes.AncientLands), new Vector2(36864), new Vector2(256, 256), new Vector2(b * .20f, b * .38f), 2048, true, true);

            worldGenText = "Generating beach...";
            GenerateBeach(tiles, 8);

            worldGenText = "Generating rocks...";
            GenerateRock(tiles, 8, TileBreakable.Create(Coordinate.Zero, Assets.GetTexture("rock1"), TileFloor.Create(Coordinate.Zero, "grassAncientlands1", Biomes.Rock), 40, Biomes.Rock));
            GenerateRocks(tiles, Biomes.LowLands, 8192);
            GenerateRocks(tiles, Biomes.MidLands, 8192);
            GenerateRocks(tiles, Biomes.HighLands, 8192);
            GenerateRocks(tiles, Biomes.AncientLands, 8192);

            //GenerateSpawners(world, tiles, 15000);
            worldGenText = "Done!";

            world.islandRects.Add(MoveTilesToWorld(Coordinate.Zero));
            CreateSpawn(Biomes.Beach);
        }

        public void Blank()
        {

        }
        public void GenerateSmall()
        {
            //GenerateOceanFill(tiles);
            //GenerateRand(tiles, new TileFloor(Coordinate.Zero, Assets.GetTexture("grassAncientlands1"), Biomes.AncientLands, false), new Vector2(32 * Coordinate.coordSize), new Vector2(40, 40), new Vector2(16 * Coordinate.coordSize, 16 * Coordinate.coordSize), 0, true);
            //tiles[32, 32] = new TileWall(new Coordinate(32), Assets.GetTexture("grassLowlands1"), Biomes.Rock, true, false);
            //GenerateRand(tiles, new TileWall(Coordinate.Zero, Assets.GetTexture("rock1"), Assets.GetTexture("rocksAncientlands2"), Biomes.AncientLands, true, false), new Vector2(32 * Coordinate.coordSize), new Vector2(40, 40), new Vector2(16 * Coordinate.coordSize, 16 * Coordinate.coordSize), 0, false);
            //world.CreateEnemy2(Enemy2.Create(0, new Vector2(31 * Coordinate.coordSize), true));
            //world.CreateEnemy2(Enemy2.Create(1, new Vector2(31 * Coordinate.coordSize), true));
            world.islandRects.Add(MoveTilesToWorld(Coordinate.Zero));
            CreateSpawn(Biomes.AncientLands);
        }

        private void CreateSpawn(Biomes biome)
        {
            int iterations = 0;
            while (world.spawnLocation == Coordinate.Zero)
            {
                iterations++;
                if (iterations >= 500)
                {
                    Logger.Log("Could not create spawn location - could not find any tiles of spawn biome!", true);
                    break;
                }
                foreach (Rectangle rects in world.islandRects)
                {
                    bool done = false;
                    int yloc = Main.rand.Next(rects.Size.Y);

                    for (int x = 0; x < tiles.GetLength(0); x++)
                    {
                        if (tiles[rects.Location.X + x, rects.Location.Y + yloc] != null && tiles[rects.Location.X + x, rects.Location.Y + yloc].location == biome)
                        {
                            if (Main.rand.Next(128) == 1)
                            {
                                world.spawnLocation = new Coordinate(rects.Location.X + x, rects.Location.Y + yloc);
                                Logger.Log("Created spawn location at " + world.spawnLocation.x + ", " + world.spawnLocation.y + "!", true);
                                done = true;
                                break;
                            }
                        }
                    }

                    if (done)
                        break;
                }
            }
            //world.spawnLocation = Coordinate.Zero;
            world.player = new Player(new Vector2(128, 128)/*world.spawnLocation.ToVector2(), Class.Archer);
            world.entities.Add(world.player);
        }
        public static List<Tile> GenerateRock(Tile[,] tiles, int genCount, Tile tile)
        {//GenerateRock size is 64? 1536
            int numGenerated = genCount;
            while (numGenerated > 0)
            {
                worldGenText = "Generating rocks... " + numGenerated + " left...";
                int x = Main.rand.Next(0, tiles.GetLength(0));
                int y = Main.rand.Next(0, tiles.GetLength(1));
                if (tiles[x, y] != null && tiles[x, y].location != Biomes.Beach)
                {
                    worldGenText += "\nGenerating at: " + x + "/" + y;
                    Vector2 center = new Vector2(1536) + new Coordinate(x, y).ToVector2();
                    GenerateRand(tiles, tile, center, new Vector2(12, 22), new Vector2(128, 512), 72, true, true);
                    numGenerated--;
                }
            }

            return null;
        }

        public static void GenerateBeach(Tile[,] tiles, int width)
        {
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    if (tiles[x, y] != null)
                    {
                        if (tiles[x, y].location == Biomes.LowLands)
                        {
                            for (int a = -width; a <= width; a++)
                            {
                                for (int b = -width; b <= width; b++)
                                {
                                    if (a != 0 && b != 0) //if it's not the middle tile
                                    {
                                        if (tiles[x + a, y + b] != null && tiles[x + a, y + b].location == Biomes.Sea)
                                            tiles[x + a, y + b] = TileFloor.Create(new Coordinate(x + a, y + b), "beach1", Biomes.Beach);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// Generates an irregular tile circle.
        /// </summary>
        /// <param name="tiles">Array of tiles to create to.</param>
        /// <param name="created">The tile to create.</param>
        /// <param name="center">The center position of the entire </param>
        /// <param name="numSidesMinMax">A vector2 where the X is the minimum number of sides the circle will have, and the Y is the maximum.</param>
        /// <param name="distMinMax">A vector2 where the X is the minimum distance a point can be from the center, and the Y is the maximum.</param>
        /// <param name="irregularity">How big of a difference a point can be from the previous point.</param>
        /// <param name="useOverlaps">Whether or not the circle should be filled in.</param>
        /// <returns></returns>
        public static List<Tile> GenerateRand(Tile[,] tiles, Tile created,  Vector2 center, Vector2 numSidesMinMax, Vector2 distMinMax, int irregularity, bool useOverlaps = true, bool overlapByBiome = false)
        {
            List<Tile> returnTiles = new List<Tile>();
            int numSides = Main.rand.Next((int)numSidesMinMax.X, (int)numSidesMinMax.Y);

            bool pointASet = false; //Has point a been set yet?
            Vector2 firstPoint = Vector2.Zero; //the first point created
            Vector2 pointA = Vector2.Zero;  //the PREVIOUS point created
            Vector2 pointB = Vector2.Zero;  //The current point just created, OR the first point if it's on its last iteration
            float prevx = 0;                //The previous X value generated, for use by irregularity to determine how far it can go

            float num = 360 / numSides;

            for (float i = 360; i > 0; i -= num)
            {
                if (!pointASet)
                {   //if point A hasn't been set, set it as a generic size
                    pointA = new Vector2(Main.rand.Next((int)distMinMax.X, (int)distMinMax.Y), 0);
                    prevx = pointA.X;
                    firstPoint = pointA;
                    pointASet = true;
                }
                if (!(i <= num))
                {
                    Vector2 pointBur = new Vector2(MathHelper.Clamp(prevx + Main.rand.Next(-irregularity, irregularity), (int)distMinMax.X, (int)distMinMax.Y), 0); //unrotated point B
                    prevx = pointBur.X; //set the prevX to this so we can use it in the next iteration
                    pointB = Vector2.Transform(pointBur, Matrix.CreateRotationZ(MathHelper.ToRadians(i)));   //rotate it by 360 / the number of sides (devides the "circle" into that many parts) * the current side we're on to get that part

                    List<Coordinate> lineCoords = World.GetCoordsAlongLine(tiles, pointA + center, pointB + center, 0); //get all the coordinates inbetween these 2 points
                    foreach (Coordinate c in lineCoords)
                    {
                        if (c.x < tiles.GetLength(0) && c.y < tiles.GetLength(1) && c.x >= 0 && c.y >= 0)
                        {   //create  a tile at each of these positions.
                            tiles[c.x, c.y] = created.Copy(c);
                            returnTiles.Add(tiles[c.x, c.y]);
                        }
                    }
                    pointA = pointB; //set the current point to the previous point for use in the next iteration.
                }
                else//if it's on its last iteration
                {
                    pointB = firstPoint;    //use first point instead of another randomly generated point, so it connects up
                    List<Coordinate> lineCoords = World.GetCoordsAlongLine(tiles, pointA + center, pointB + center, 0);
                    foreach (Coordinate c in lineCoords)
                    {
                        if (c.x < tiles.GetLength(0) && c.y < tiles.GetLength(0) && c.x >= 0 && c.y >= 0)
                        {
                            tiles[c.x, c.y] = created.Copy(c);
                            returnTiles.Add(tiles[c.x, c.y]);
                        }
                    }
                }
            }

            if (useOverlaps)
            {
                if (overlapByBiome)
                    FillOverlaps(tiles, created, created.location);
                else
                FillOverlaps(tiles, created, returnTiles);
            }

            Console.WriteLine("Finished Generating " + created.location.ToString());

            return returnTiles; //for use by GenerateRocks
        }

        public static List<Tile> FillOverlaps(Tile[,] tiles, Tile tile, Biomes biome)
        {
            List<Tile> returnTiles = new List<Tile>();

            int[,] overlaps = new int[tiles.GetLength(0), tiles.GetLength(1)];
            overlaps = GetOverlapTilemap(tiles, biome);

            for (int x = 0; x < overlaps.GetLength(0); x++)
            {
                for (int y = 0; y < overlaps.GetLength(1); y++)
                {
                    if (overlaps[x, y] >= 4)
                    {   //if the overlap value is greater than 4 fill it in with the current biome's main tile.
                        //Tile t = tile.Copy(new Coordinate(x, y));
                        tiles[x, y] = tile.Copy(new Coordinate(x, y));
                        //returnTiles.Add(t);
                    }
                }
            }
            return returnTiles;
        }

        public static List<Tile> FillOverlaps(Tile[,] tiles, Tile tile, List<Tile> tilestouse = null)
        {
            List<Tile> returnTiles = new List<Tile>();

            int[,] overlaps = new int[tiles.GetLength(0), tiles.GetLength(1)];
            overlaps = GetOverlapTilemap(tiles, tilestouse);

            for (int x = 0; x < overlaps.GetLength(0); x++)
            {
                for (int y = 0; y < overlaps.GetLength(1); y++)
                {
                    if (overlaps[x, y] >= 4)
                    {   //if the overlap value is greater than 4 fill it in with the current biome's main tile.
                        //Tile t = tile.Copy(new Coordinate(x, y));
                        tiles[x, y] = tile.Copy(new Coordinate(x, y));
                        //returnTiles.Add(t);
                    }
                }
            }
            return returnTiles;
        }

        public static Tile[,] GenerateRocks(Tile[,] tiles, Biomes location, int numrocks)  //TODO Give biomes class more reason to exist: get the rock texture types and their chances. 
        {
            while (numrocks > 0)
            {
                Coordinate c = new Coordinate(Main.rand.Next(0, 1536), Main.rand.Next(0, 1536));
                if (tiles[c.x, c.y] != null && tiles[c.x, c.y].location == location)
                {
                    if (Biome.GetBiomeRockTexture(location) != null)
                    {
                        tiles[c.x, c.y] = TileRock.Create(c, Biome.GetBiomeRockTexture(location), Biome.GetBiomeGrassTexture(location), location);
                        numrocks--;
                    }
                    else
                    {
                        Console.WriteLine("Could not generate any rocks, because biome '" + location + "' does not have any rock textures assigned to it.");
                        break;
                    }
                }
            }
            return tiles;
        }

        public static Tile[,] GenerateRocks(Tile[,] tiles1, List<Tile> tiles, Biomes location, int numrocks)  //TODO Give biomes class more reason to exist: get the rock texture types and their chances. 
        {
            while (numrocks > 0)
            {
                Tile t = tiles[Main.rand.Next(0, tiles.Count)];
                if (t != null && t.location == location)
                {
                    if (Biome.GetBiomeRockTexture(location) != null)
                    {
                        t = TileRock.Create(t.position, Biome.GetBiomeRockTexture(location), Biome.GetBiomeGrassTexture(location), location);
                        numrocks--;
                    }
                    else
                    {
                        Console.WriteLine("Could not generate any rocks, because biome '" + location + "' does not have any rock textures assigned to it.");
                        break;
                    }
                }
            }
            foreach(Tile t in tiles)
            {
                tiles1[t.position.x, t.position.y] = t;
            }
            return tiles1;
        }

        public static int[,] GetOverlapTilemap(Tile[,] tiles, Biomes biome)
        {
            int[,] overlaps = new int[tiles.GetLength(0), tiles.GetLength(1)];

            bool found0 = false;
            for (int y = 0; y < overlaps.GetLength(1); y++)
            {
                for (int x = 0; x < overlaps.GetLength(0); x++)
                {
                    if (!found0)
                    {
                        if (World.GetTile(tiles, new Coordinate(x, y)) != null && World.GetTile(tiles, new Coordinate(x, y)).location == biome)
                        {
                            found0 = true;

                            overlaps[x, y]++;
                        }
                    }
                    else
                    {
                        overlaps[x, y]++;
                    }
                }
                found0 = false;
            }

            bool found1 = false;
            for (int x = 0; x < overlaps.GetLength(0); x++)
            {
                for (int y = 0; y < overlaps.GetLength(1); y++)
                {
                    if (!found1)
                    {
                        if (World.GetTile(tiles, new Coordinate(x, y)) != null && World.GetTile(tiles, new Coordinate(x, y)).location == biome)
                        {
                            found1 = true;

                            overlaps[x, y]++;
                            //displayDebug.Add(GetTile(new Coordinate(x, y)));
                        }
                    }
                    else
                    {
                        overlaps[x, y]++;
                        //displayDebug.Add(GetTile(new Coordinate(x, y)));
                    }
                }
                found1 = false;
            }

            bool found2 = false;
            for (int y = 0; y < overlaps.GetLength(1); y++)
            {
                for (int x = overlaps.GetLength(0) - 1; x >= 0; x--)
                {
                    if (!found2)
                    {
                        if (World.GetTile(tiles, new Coordinate(x, y)) != null && World.GetTile(tiles, new Coordinate(x, y)).location == biome)
                        {
                            found2 = true;

                            overlaps[x, y]++;
                            //displayDebug.Add(GetTile(new Coordinate(x, y)));
                        }
                    }
                    else
                    {
                        overlaps[x, y]++;
                        //displayDebug.Add(GetTile(new Coordinate(x, y)));
                    }
                }
                found2 = false;
            }

            bool found3 = false;
            for (int x = 0; x < overlaps.GetLength(0); x++)
            {
                for (int y = overlaps.GetLength(1) - 1; y >= 0; y--)
                {
                    if (!found3)
                    {
                        if (World.GetTile(tiles, new Coordinate(x, y)) != null && World.GetTile(tiles, new Coordinate(x, y)).location == biome)
                        {
                            found3 = true;

                            overlaps[x, y]++;
                            //displayDebug.Add(GetTile(new Coordinate(x, y)));
                        }
                    }
                    else
                    {
                        overlaps[x, y]++;
                        //displayDebug.Add(GetTile(new Coordinate(x, y)));
                    }
                }
                found3 = false;
            }
            return overlaps;
        }

        public static int[,] GetOverlapTilemap(Tile[,] tiles, List<Tile> tileWhitelist)
        {
            int[,] overlaps = new int[tiles.GetLength(0), tiles.GetLength(1)];

            bool found0 = false;
            for (int y = 0; y < overlaps.GetLength(1); y++)
            {
                for (int x = 0; x < overlaps.GetLength(0); x++)
                {
                    if (!found0)
                    {
                        if (World.GetTile(tiles, new Coordinate(x, y)) != null && tileWhitelist.Contains(World.GetTile(tiles, new Coordinate(x, y))))
                        {
                            found0 = true;

                            overlaps[x, y]++;
                        }
                    }
                    else
                    {
                        overlaps[x, y]++;
                    }
                }
                found0 = false;
            }

            bool found1 = false;
            for (int x = 0; x < overlaps.GetLength(0); x++)
            {
                for (int y = 0; y < overlaps.GetLength(1); y++)
                {
                    if (!found1)
                    {
                        if (World.GetTile(tiles, new Coordinate(x, y)) != null && tileWhitelist.Contains(World.GetTile(tiles, new Coordinate(x, y))))
                        {
                            found1 = true;

                            overlaps[x, y]++;
                            //displayDebug.Add(GetTile(new Coordinate(x, y)));
                        }
                    }
                    else
                    {
                        overlaps[x, y]++;
                        //displayDebug.Add(GetTile(new Coordinate(x, y)));
                    }
                }
                found1 = false;
            }

            bool found2 = false;
            for (int y = 0; y < overlaps.GetLength(1); y++)
            {
                for (int x = overlaps.GetLength(0) - 1; x >= 0; x--)
                {
                    if (!found2)
                    {
                        if (World.GetTile(tiles, new Coordinate(x, y)) != null && tileWhitelist.Contains(World.GetTile(tiles, new Coordinate(x, y))))
                        {
                            found2 = true;

                            overlaps[x, y]++;
                            //displayDebug.Add(GetTile(new Coordinate(x, y)));
                        }
                    }
                    else
                    {
                        overlaps[x, y]++;
                        //displayDebug.Add(GetTile(new Coordinate(x, y)));
                    }
                }
                found2 = false;
            }

            bool found3 = false;
            for (int x = 0; x < overlaps.GetLength(0); x++)
            {
                for (int y = overlaps.GetLength(1) - 1; y >= 0; y--)
                {
                    if (!found3)
                    {
                        if (World.GetTile(tiles, new Coordinate(x, y)) != null && tileWhitelist.Contains(World.GetTile(tiles, new Coordinate(x, y))))
                        {
                            found3 = true;

                            overlaps[x, y]++;
                            //displayDebug.Add(GetTile(new Coordinate(x, y)));
                        }
                    }
                    else
                    {
                        overlaps[x, y]++;
                        //displayDebug.Add(GetTile(new Coordinate(x, y)));
                    }
                }
                found3 = false;
            }
            return overlaps;
        }*/
    }
    //#endregion
}
