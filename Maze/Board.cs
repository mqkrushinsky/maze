using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    public class Board
    {
        public Tile[] Tiles { get; set; }
        public Tile StartTile { get; set; }
        public Tile EndTile { get; set; }

        public int Width = 15;
        public int Height = 15;

        public Board()
        {
            Tiles = CreateTiles(Width, Height);

            var startX = Randomizer.RollDie(Width - 1);
            var startY = Randomizer.RollDie(Height - 1);

            StartTile = Tiles.Single(t => t.X == startX && t.Y == startY);

            var endX = Randomizer.RollDie(Width - 1);

            while (endX == StartTile.X)
            {
                endX = Randomizer.RollDie(Width - 1);
            }

            var endY = Randomizer.RollDie(Height - 1);

            while (endY == StartTile.Y)
            {
                endY = Randomizer.RollDie(Height - 1);
            }

            EndTile = Tiles.Single(t => t.X == endX && t.Y == endY);

            CreateValidPath();
        }

        public Tile[] CreateTiles(int width, int height)
        {
            Tile[] tiles = new Tile[width * height];

            int tileCount = 0;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    tiles[tileCount] = new Tile(i, j);

                    if (i > 0 && !(tiles[tileCount].TopBorder && tiles[tileCount - Width].BottomBorder))
                    {
                        tiles[tileCount].TopBorder = false;
                        tiles[tileCount - Width].BottomBorder = false;
                    }

                    if (j > 0 && !(tiles[tileCount].LeftBorder && tiles[tileCount - 1].RightBorder))
                    {
                        tiles[tileCount].LeftBorder = false;
                        tiles[tileCount - 1].RightBorder = false;
                    }

                    if (tiles[tileCount].LeftBorder && tiles[tileCount].RightBorder && tiles[tileCount].TopBorder && tiles[tileCount].BottomBorder)
                    {
                        int r = Randomizer.RollDie(4);

                        switch (r)
                        {
                            case 1:
                                tiles[tileCount].TopBorder = false;
                                break;

                            case 2:
                                tiles[tileCount].BottomBorder = false;
                                break;

                            case 3:
                                tiles[tileCount].RightBorder = false;
                                break;

                            case 4:
                                tiles[tileCount].LeftBorder = false;
                                break;
                        }
                    }

                    tileCount++;
                }
            }

            return tiles;
        }

        public void CreateValidPath()
        {
            Tile currentTile = StartTile;
            List<Tile> visitedTiles = new List<Tile>();

            visitedTiles.Add(currentTile);

            while (currentTile != EndTile)
            {
                bool deadEnd = false;

                if (currentTile.X < Width - 1)
                {
                    Tile rightNeighbor = Tiles.Single(t => t.X == currentTile.X + 1 && t.Y == currentTile.Y);
                    if (!currentTile.RightBorder && !(rightNeighbor.LeftBorder))
                    {
                        if (!visitedTiles.Contains(rightNeighbor))
                        {
                            visitedTiles.Add(rightNeighbor);
                            currentTile = rightNeighbor;
                            continue;
                        }
                    }
                }


                if (currentTile.Y > 0)
                {
                    Tile topNeighbor = Tiles.Single(t => t.Y == currentTile.Y - 1 && t.X == currentTile.X);
                    if (!currentTile.TopBorder && !(topNeighbor.BottomBorder))
                    {
                        if (!visitedTiles.Contains(topNeighbor))
                        {
                            visitedTiles.Add(topNeighbor);
                            currentTile = topNeighbor;
                            continue;
                        }
                    }
                }


                if (currentTile.Y < Height - 1)
                {
                    Tile bottomNeighbor = Tiles.Single(t => t.Y == currentTile.Y + 1 && t.X == currentTile.X);
                    if (!currentTile.BottomBorder && !(bottomNeighbor.TopBorder))
                    {
                        if (!visitedTiles.Contains(bottomNeighbor))
                        {
                            visitedTiles.Add(bottomNeighbor);
                            currentTile = bottomNeighbor;
                            continue;
                        }
                    }
                }


                if (currentTile.X > 0)
                {
                    Tile leftNeighbor = Tiles.Single(t => t.X == currentTile.X - 1 && t.Y == currentTile.Y);
                    if (!currentTile.LeftBorder && !(leftNeighbor.RightBorder))
                    {
                        if (!visitedTiles.Contains(leftNeighbor))
                        {
                            visitedTiles.Add(leftNeighbor);
                            currentTile = leftNeighbor;
                            continue;
                        }
                    }
                }

                //If we reached here, it's a dead end, break a wall
                deadEnd = true;

                while (deadEnd)
                {
                    int roll = Randomizer.RollDie(4);

                    switch (roll)
                    {

                        case 1:
                            if (currentTile.X < Width - 1)
                            {
                                Tile rightNeighbor = Tiles.Single(t => t.X == currentTile.X + 1 && t.Y == currentTile.Y);

                                currentTile.RightBorder = false;
                                rightNeighbor.LeftBorder = false;
                                currentTile = rightNeighbor;
                                deadEnd = false;
                            }
                            break;

                        case 2:
                            if (currentTile.Y > 0)
                            {
                                Tile topNeighbor = Tiles.Single(t => t.Y == currentTile.Y - 1 && t.X == currentTile.X);

                                currentTile.TopBorder = false;
                                topNeighbor.BottomBorder = false;
                                currentTile = topNeighbor;
                                deadEnd = false;
                            }
                            break;

                        case 3:
                            if (currentTile.Y < Height - 1)
                            {
                                Tile bottomNeighbor = Tiles.Single(t => t.Y == currentTile.Y + 1 && t.X == currentTile.X);

                                currentTile.BottomBorder = false;
                                bottomNeighbor.TopBorder = false;
                                currentTile = bottomNeighbor;
                                deadEnd = false;
                            }
                            break;

                        case 4:
                            if (currentTile.X > 0)
                            {
                                Tile leftNeighbor = Tiles.Single(t => t.X == currentTile.X - 1 && t.Y == currentTile.Y);

                                currentTile.LeftBorder = false;
                                leftNeighbor.RightBorder = false;
                                currentTile = leftNeighbor;
                                deadEnd = false;
                            }
                            break;
                    }
                }
            }
        }

        public List<Direction> Run()
        {
            List<Direction> directions = new List<Direction>();
            Tile currentTile = StartTile;
            List<Tile> visited = new List<Tile>();
            List<Tile> toVisit = new List<Tile>();
            int g = 0;

            toVisit.Add(StartTile);

            while (toVisit.Count > 0)
            {
                var lowest = toVisit.Min(l => l.F);
                currentTile = toVisit.First(l => l.F == lowest);

                visited.Add(currentTile);

                toVisit.Remove(currentTile);

                if (visited.FirstOrDefault(l => l == EndTile) != null)
                {
                    break;
                }

                g++;

                List<Tile> neighbors = new List<Tile>();

                if (!currentTile.RightBorder && currentTile.X < Width - 1)
                {
                    if (!Tiles.Single(t => t.X == currentTile.X + 1 && t.Y == currentTile.Y).LeftBorder)
                    {
                        neighbors.Add(Tiles.Single(t => t.X == currentTile.X + 1 && t.Y == currentTile.Y));
                    }
                }

                if (!currentTile.TopBorder && currentTile.Y > 0)
                {
                    if (!Tiles.Single(t => t.X == currentTile.X && t.Y == currentTile.Y - 1).BottomBorder)
                    {
                        neighbors.Add(Tiles.Single(t => t.X == currentTile.X && t.Y == currentTile.Y - 1));
                    }
                }

                if (!currentTile.BottomBorder && currentTile.Y < Height - 1)
                {
                    if (!Tiles.Single(t => t.X == currentTile.X && t.Y == currentTile.Y + 1).TopBorder)
                    {
                        neighbors.Add(Tiles.Single(t => t.X == currentTile.X && t.Y == currentTile.Y + 1));
                    }
                }

                if (!currentTile.LeftBorder && currentTile.X > 0)
                {
                    if (!Tiles.Single(t => t.X == currentTile.X - 1 && t.Y == currentTile.Y).RightBorder)
                    {
                        neighbors.Add(Tiles.Single(t => t.X == currentTile.X - 1 && t.Y == currentTile.Y));
                    }
                }

                foreach (Tile neighbor in neighbors)
                {
                    if (visited.FirstOrDefault(l => l == neighbor) == null)
                    {
                        if (toVisit.FirstOrDefault(l => l == neighbor) == null)
                        {
                            neighbor.G = g;
                            neighbor.H = Math.Abs(neighbor.X - currentTile.X) + Math.Abs(neighbor.Y - currentTile.Y);
                            neighbor.F = neighbor.G + neighbor.H;
                            neighbor.PathParent = currentTile;

                            toVisit.Insert(0, neighbor);
                        }
                        else
                        {
                            if (g + neighbor.H < neighbor.F)
                            {
                                neighbor.G = g;
                                neighbor.F = neighbor.G + neighbor.H;
                                neighbor.PathParent = currentTile;
                            }
                        }
                    }
                }
            }

            while (currentTile != null)
            {
                Tile parent = currentTile.PathParent;

                if (parent != null)
                {
                    if (parent.X < currentTile.X && parent.Y == currentTile.Y)
                    {
                        directions.Insert(0, Direction.Right);
                    }

                    if (parent.X > currentTile.X && parent.Y == currentTile.Y)
                    {
                        directions.Insert(0, Direction.Left);
                    }

                    if (parent.X == currentTile.X && parent.Y < currentTile.Y)
                    {
                        directions.Insert(0, Direction.Down);
                    }

                    if (parent.X == currentTile.X && parent.Y > currentTile.Y)
                    {
                        directions.Insert(0, Direction.Up);
                    }
                }

                currentTile = parent;
            }

            return directions;
        }
    }
}
