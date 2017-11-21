using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace AutoFenceGate
{
    public class ModEntry : Mod
    {

        private Dictionary<Vector2, Fence> trackedFenceGates = new Dictionary<Vector2, Fence>();

        public override void Entry(IModHelper helper)
        {
            GameEvents.SecondUpdateTick += this.autoOpenFenceGate;
            GameEvents.HalfSecondTick += this.autoCloseFenceGate;
        }

        private void autoOpenFenceGate(object Sender, EventArgs e)
        {
            if (!Context.IsWorldReady) return;
            if (!Context.CanPlayerMove || !Game1.player.isMoving()) return;

            // TODO:
            // Add a config option to only enable this feature while riding a horse

            int playerX = Game1.player.getTileX();
            int playerY = Game1.player.getTileY();

            int nextX = playerX;
            int nextY = playerY;
            int playerFacing = Game1.player.facingDirection;
            switch (playerFacing)
            {
                case 0:
                    nextY -= 1;
                    break;
                case 1:
                    nextX += 1;
                    break;
                case 2:
                    nextY += 1;
                    break;
                case 3:
                    nextX -= 1;
                    break;
            }

            if (!Game1.currentLocation.isObjectAt(nextX * Game1.tileSize, nextY * Game1.tileSize)) return;
            StardewValley.Object nextObject = Game1.currentLocation.getObjectAt(nextX * Game1.tileSize, nextY * Game1.tileSize);
            if (!(nextObject is Fence)) return;
            Fence nextObjectFence = (Fence)nextObject;
            if (!nextObjectFence.isGate || nextObjectFence.gatePosition != 0) return;
            if (trackedFenceGates.ContainsKey(nextObjectFence.tileLocation)) return;

            nextObjectFence.checkForAction(Game1.player, false);
            trackedFenceGates.Add(nextObjectFence.tileLocation, nextObjectFence);
        }

        private void autoCloseFenceGate(object Sender, EventArgs e)
        {
            Vector2 playerPosition = new Vector2(Game1.player.position.X / Game1.tileSize, Game1.player.position.Y / Game1.tileSize);

            List<Vector2> fenceGatesToUntrack = new List<Vector2>();
            foreach (Vector2 fenceGateLocation in trackedFenceGates.Keys)
            {
                if (trackedFenceGates[fenceGateLocation].gatePosition == 0)
                {
                    fenceGatesToUntrack.Add(fenceGateLocation);
                    continue;
                }

                // TODO:
                // Make max fence-to-player distance configurable
                float playerDistance = Vector2.Distance(playerPosition, fenceGateLocation);
                if (playerDistance > 1.1f)
                {
                    trackedFenceGates[fenceGateLocation].checkForAction(Game1.player, false);
                    fenceGatesToUntrack.Add(fenceGateLocation);
                }
            }

            foreach (Vector2 fenceGateLocation in fenceGatesToUntrack)
            {
                trackedFenceGates.Remove(fenceGateLocation);
            }
        }
    }
}
