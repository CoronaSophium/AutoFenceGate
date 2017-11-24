using Microsoft.Xna.Framework;

using StardewValley;
using StardewValley.Locations;
using StardewValley.Buildings;
using StardewModdingAPI;

namespace Passage
{
    class Utility
    {
        public static Point GetPointAheadPlayer(StardewValley.Farmer player, int tileSize)
        {
            int tileAheadPlayerX = player.getTileX();
            int tileAheadPlayerY = player.getTileY();

            // Get tile directly ahead of player based on facing
            switch (player.facingDirection)
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
            return new Point(tileAheadPlayerX * tileSize, tileAheadPlayerY * tileSize);
        }

        public static int GetDoorType(Point tile, GameLocation currentLocation)
        {
            if (currentLocation.doors.ContainsKey(tile)) return 1;
            if (currentLocation.doesTileHaveProperty(tile.X, tile.Y, "Action", "Buildings")?.Split(' ')?[0] == "Door") return 2;
            if (currentLocation is BuildableGameLocation)
            {
                foreach (Building building in (currentLocation as BuildableGameLocation).buildings)
                {
                    if (building.indoors == null) continue;
                    if (tile != new Point(building.tileX + building.humanDoor.X, building.tileY + building.humanDoor.Y)) continue;
                    return 3;
                }
            }

            return 0;
        }
    }

    class ModConfig
    {
        public bool EnableAutoDoorInteract { get; set; } = true;
        public bool EnableAutoFenceGateManagement { get; set; } = true;
        public bool OnlyOpenFenceGateWhileRidingHorse { get; set; } = false;
        public float MaxDistanceToKeepFenceGateOpen { get; set; } = 1.1f;
    }
}
