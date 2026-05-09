using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class LevelManager : MonoBehaviour
    {
        [Header("Level Configuration")]
        public int requiredKeys = 1;
        public Transform[] spawnPoints;
        public Transform exitDoor;
        public float respawnDelay = 0.5f;

        [Header("Timer")]
        public bool countUp = true;

        private int _keysCollected;
        private bool _levelInitialized;

        private void Start()
        {
            InitializeLevel();
        }

        private void InitializeLevel()
        {
            if (_levelInitialized) return;
            _levelInitialized = true;

            SpawnPlayers();

            var door = exitDoor != null ? exitDoor.GetComponent<Gameplay.Door>() : null;
            if (door != null && requiredKeys > 0)
            {
                door.Lock(requiredKeys);
            }
        }

        private void SpawnPlayers()
        {
            var gm = GameManager.Instance;
            if (gm == null || gm.playerPrefab == null) return;

            int count = gm.activePlayerCount;
            for (int i = 0; i < count; i++)
            {
                Transform spawn = i < spawnPoints.Length ? spawnPoints[i] : spawnPoints[^1];
                var player = Instantiate(gm.playerPrefab, spawn.position, spawn.rotation);
                var controller = player.GetComponent<Player.PlayerController>();
                if (controller != null)
                {
                    controller.Initialize(i, spawn.position);
                }
                gm.RegisterPlayer(player);
            }
        }

        public void CollectKey()
        {
            _keysCollected++;
            var door = exitDoor != null ? exitDoor.GetComponent<Gameplay.Door>() : null;
            if (door != null)
            {
                door.Unlock(_keysCollected);
            }
        }

        public int GetKeysCollected() => _keysCollected;

        public void RespawnPlayer(Player.PlayerController controller)
        {
            if (controller == null) return;

            int index = controller.PlayerIndex;
            Transform spawn = index < spawnPoints.Length ? spawnPoints[index] : spawnPoints[^1];
            controller.Respawn(spawn.position);
        }

        private void OnDestroy()
        {
            var gm = GameManager.Instance;
            if (gm != null)
            {
                var players = gm.GetPlayers();
                foreach (var p in players)
                {
                    gm.UnregisterPlayer(p);
                }
            }
        }
    }
}
