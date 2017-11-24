using Microsoft.Xna.Framework;
using xTile.Dimensions;
using xTile.Tiles;

using StardewValley;
using StardewValley.Locations;
using StardewValley.Buildings;

namespace Passage
{
    class Utility
    {
        public static Point GetPointAheadPlayer()
        {
            int tileAheadPlayerX = Game1.player.getTileX();
            int tileAheadPlayerY = Game1.player.getTileY();

            // Get tile directly ahead of player based on facing
            switch (Game1.player.facingDirection)
            {
                case 0:
                    tileAheadPlayerY -= 1;
                    break;

                case 1:
                    tileAheadPlayerX += 1;
                    break;

                case 2:
                    tileAheadPlayerY += 1;
                    break;

                case 3:
                    tileAheadPlayerX -= 1;
                    break;

                default:
                    break;
            }

            // Transform tile coords into absolute coords prior to returning
            return new Point(tileAheadPlayerX * Game1.tileSize, tileAheadPlayerY * Game1.tileSize);
        }

        public static int GetDoorType(Point tile)
        {
            if (Game1.currentLocation.doors.ContainsKey(tile)) return 1;
            if (Game1.currentLocation.doesTileHaveProperty(tile.X, tile.Y, "Action", "Buildings")?.Split(' ')?[0] == "Door") return 2;
            if (Game1.currentLocation is BuildableGameLocation)
            {
                foreach (Building building in (Game1.currentLocation as BuildableGameLocation).buildings)
                {
                    if (building.indoors == null) continue;
                    if (tile != new Point(building.tileX + building.humanDoor.X, building.tileY + building.humanDoor.Y)) continue;
                    return 3;
                }
            }
            if (Game1.currentLocation.doesTileHaveProperty(tile.X, tile.Y, "Action", "Buildings") == "EnterSewer") return 4;
            // TODO: return 5 for Sewer entrance in the forest maybe?
            if (Game1.currentLocation.name == "Mine" || Game1.currentLocation is MineShaft)
            {
                Tile trueTile = Game1.currentLocation.map.GetLayer("Buildings").PickTile(new Location(tile.X * Game1.tileSize, tile.Y * Game1.tileSize), Game1.viewport.Size);
                if (trueTile != null)
                {
                    switch (trueTile.TileIndex)
                    {
                        // Elevator
                        case 112:
                            return 6;

                        // Ladder (up)
                        case 115:
                            return 7;

                        // Ladder (down)
                        case 173:
                            return 8;
                    }
                }
            }

            return 0;
        }
    }

    class ModConfig
    {
        public bool EnableAutoTransport { get; set; } = true;
        public bool EnableAutoMineDownLadder { get; set; } = false;
        public bool EnableAutoFenceGateManagement { get; set; } = true;
        public bool OnlyOpenFenceGateWhileRidingHorse { get; set; } = false;
        public float MaxDistanceToKeepFenceGateOpen { get; set; } = 1.1f;
    }
}
