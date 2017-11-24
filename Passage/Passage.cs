using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using xTile.Dimensions;

using StardewValley;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley.Locations;

namespace Passage
{
    public class ModEntry : Mod
    {

        private ModConfig config;
        private Dictionary<Point, GameLocation> TrackedFenceGates = new Dictionary<Point, GameLocation>();

        public override void Entry(IModHelper helper)
        {
            this.config = helper.ReadConfig<ModConfig>();
            GameEvents.FourthUpdateTick += this.InteractWithObjectAhead;
            GameEvents.EighthUpdateTick += this.ManageFenceGates;
        }

        private void InteractWithObjectAhead(object Sender, EventArgs e)
        {
            if (!Context.IsWorldReady || !Context.CanPlayerMove) return;
            if (!Game1.player.isMoving()) return;

            Point pointAhead = Utility.GetPointAheadPlayer(Game1.player, Game1.tileSize);
            Point tileAhead = new Point(pointAhead.X / Game1.tileSize, pointAhead.Y / Game1.tileSize);
            StardewValley.Object objectAhead = (
                Game1.currentLocation.isObjectAt(pointAhead.X, pointAhead.Y) ? (
                    Game1.currentLocation.getObjectAt(pointAhead.X, pointAhead.Y)
                ) : null
            );
            
            if (this.config.EnableAutoFenceGateManagement && objectAhead is Fence)
            {
                // Check for horse control (config-based)
                if (this.config.OnlyOpenFenceGateWhileRidingHorse && !Game1.player.isRidingHorse()) return;
                // Check if objectAhead is an untracked, closed fence gate
                if (this.TrackedFenceGates.ContainsKey(pointAhead)) return;
                if (!(objectAhead as Fence).isGate || (objectAhead as Fence).gatePosition != 0) return;

                // Interact with objectAhead and track it for automatic closing
                objectAhead.checkForAction(Game1.player, false);
                TrackedFenceGates.Add(pointAhead, Game1.currentLocation);

                return;
            }

            if (this.config.EnableAutoDoorInteract && !Convert.ToBoolean(Utility.GetDoorType(tileAhead, Game1.currentLocation)))
            {
                // Interact with door tile ahead
                Game1.currentLocation.checkAction(new Location(tileAhead.X, tileAhead.Y), Game1.viewport, Game1.player);

                return;
            }
        }

        private void ManageFenceGates(object Sender, EventArgs e)
        {
            if (!Context.IsWorldReady) return;
            if (!this.config.EnableAutoFenceGateManagement) return;
            if (!Convert.ToBoolean(this.TrackedFenceGates.Count)) return;

            // Hold all fence gates to untrack here
            List<Point> fenceGatesToUntrack = new List<Point>();
            foreach (Point location in TrackedFenceGates.Keys)
            {
                Fence fenceGate = TrackedFenceGates[location].getObjectAt(location.X, location.Y) as Fence;
                if (fenceGate.gatePosition == 0)
                {
                    // If fence gate is already closed, simply add it to untrack list
                    fenceGatesToUntrack.Add(location);
                    continue;
                }

                float playerDistance = Vector2.Distance(Game1.player.getTileLocation(), fenceGate.tileLocation);
                if (playerDistance > this.config.MaxDistanceToKeepFenceGateOpen || Game1.currentLocation != TrackedFenceGates[location])
                {
                    // If player is too far from fence gate, or if the player has changed maps, close and add it to untrack list
                    fenceGatesToUntrack.Add(location);
                    fenceGate.checkForAction(Game1.player, false);
                }
            }

            // Remove all fence gates mark for untracking
            foreach (Point location in fenceGatesToUntrack)
            {
                TrackedFenceGates.Remove(location);
            }
        }
    }
}
