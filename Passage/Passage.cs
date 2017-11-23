using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using StardewModdingAPI;
using StardewModdingAPI.Events;

using StardewValley;
using StardewValley.Buildings;
using StardewValley.Locations;

namespace Passage
{
    public class ModEntry : Mod
    {

        private ModConfig config;
        private Dictionary<Point, Fence> TrackedFenceGates = new Dictionary<Point, Fence>();

        public override void Entry(IModHelper helper)
        {
            this.config = helper.ReadConfig<ModConfig>();

            GameEvents.FourthUpdateTick += this.Passage;

            if (config.EnableAutoFenceGateManagement)
            {
                GameEvents.OneSecondTick += this.ManageFenceGates;
            }
        }

        private void Passage(object Sender, EventArgs e)
        {
            if (!Context.IsWorldReady || !Context.CanPlayerMove) return;
            if (!Game1.player.isMoving()) return;

            StardewValley.Farmer player = Game1.player;
            GameLocation currLocation = Game1.currentLocation;
            Point tileAhead = this.GetTileAheadPlayer(player);

            if (this.config.EnableAutoFenceGateManagement)
            {
                Fence fenceGate = this.GetFenceGateFromTile(tileAhead, currLocation);
                if (fenceGate != null)
                {
                    if (this.TrackedFenceGates.ContainsKey(tileAhead)) return;
                    if (this.config.OnlyOpenFenceGateWhileRidingHorse && !player.isRidingHorse()) return;
                    if (fenceGate.gatePosition == 88) return;

                    fenceGate.checkForAction(player, false);
                    this.TrackedFenceGates.Add(tileAhead, fenceGate);
                    return;
                }
            }

            if (this.config.EnableAutoDoorWarp && !player.isRidingHorse())
            {
                Warp doorWarp = this.GetDoorWarpFromTile(tileAhead, currLocation);
                if (doorWarp != null)
                {
                    player.warpFarmer(doorWarp);
                    Game1.playSound("doorClose");
                    return;
                }
                
                if (currLocation is BuildableGameLocation)
                {
                    foreach (Building building in ((BuildableGameLocation)currLocation).buildings)
                    {
                        if (building.indoors == null) continue;

                        Point humanDoorTile = new Point(building.tileX + building.humanDoor.X, building.tileY + building.humanDoor.Y);
                        if (humanDoorTile != tileAhead) continue;

                        Game1.warpFarmer(building.indoors, building.indoors.warps[0].X, building.indoors.warps[0].Y - 1, player.facingDirection, true);
                        Game1.playSound("doorClose");
                        return;
                    }
                }
            }
        }

        private void ManageFenceGates(object Sender, EventArgs e)
        {
            if (!Convert.ToBoolean(this.TrackedFenceGates.Keys.Count)) return;

            List<Point> fenceGatesToUntrack = new List<Point>();
            foreach (Point fenceGateLocation in this.TrackedFenceGates.Keys)
            {
                Fence fenceGate = TrackedFenceGates[fenceGateLocation];

                bool IsPlayerInProximityOfGate = Vector2.Distance(Game1.player.getTileLocation(), new Vector2(fenceGateLocation.X, fenceGateLocation.Y)) <= this.config.MaxDistanceToKeepFenceGateOpen;
                bool HasGateBeenClosed = fenceGate.gatePosition == 0 ? true : false;

                if (HasGateBeenClosed)
                {
                    fenceGatesToUntrack.Add(fenceGateLocation);
                    continue;
                }
                if (IsPlayerInProximityOfGate) continue;

                fenceGate.checkForAction(Game1.player, false);
                fenceGatesToUntrack.Add(fenceGateLocation);
            }

            foreach (Point fenceGateToUntrack in fenceGatesToUntrack)
            {
                TrackedFenceGates.Remove(fenceGateToUntrack);
            }
        }

        private Point GetTileAheadPlayer(StardewValley.Farmer player)
        {
            int tileAheadPlayerX = player.getTileX();
            int tileAheadPlayerY = player.getTileY();
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

            return new Point(tileAheadPlayerX, tileAheadPlayerY);
        }

        private Fence GetFenceGateFromTile(Point tile, GameLocation currLocation)
        {
            if (!currLocation.isObjectAt(tile.X * Game1.tileSize, tile.Y * Game1.tileSize)) return null;

            StardewValley.Object tileObject = currLocation.getObjectAt(tile.X * Game1.tileSize, tile.Y * Game1.tileSize);
            if (!(tileObject is Fence)) return null;

            Fence tileFence = (Fence)tileObject;
            if (!tileFence.isGate) return null;

            return tileFence;
        }

        private Warp GetDoorWarpFromTile(Point tile, GameLocation currLocation)
        {
            if (!currLocation.doors.ContainsKey(tile)) return null;
            return currLocation.getWarpFromDoor(tile);
        }
    }

    class ModConfig
    {
        public bool EnableAutoDoorWarp { get; set; } = true;
        public bool EnableAutoFenceGateManagement { get; set; } = true;

        public bool OnlyOpenFenceGateWhileRidingHorse { get; set; } = false;
        public float MaxDistanceToKeepFenceGateOpen { get; set; } = 1.1f;
    }
}
