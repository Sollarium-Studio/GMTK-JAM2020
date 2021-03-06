﻿using System.Collections.Generic;
using Scriptable;
using UnityEngine;

namespace Map
{
    public class LevelLayoutGenerator : MonoBehaviour
    {
        public List<LevelChunkData> levelChunkData;
        public LevelChunkData firstChunk;

        private LevelChunkData _previousChunk;

        public Vector3 spawnOrigin;

        private Vector3 _spawnPosition;
        public int chunksToSpawn = 10;


        [SerializeField] private GameObject dynamitePrefab;
        [SerializeField] private Transform dynamiteSpawn;
        [SerializeField] private bool spawnDynamite = true;


        void OnEnable()
        {
            TriggerExit.OnChunkExited += PickAndSpawnChunk;
        }

        private void OnDisable()
        {
            TriggerExit.OnChunkExited -= PickAndSpawnChunk;
        }

        void Start()
        {
            _previousChunk = firstChunk;

            for (int i = 0; i < chunksToSpawn; i++)
            {
                PickAndSpawnChunk();
            }
        }

        void PickAndSpawnChunk()
        {
            LevelChunkData chunkToSpawn = PickNextChunk();

            GameObject objectFromChunk = chunkToSpawn.levelChunks[Random.Range(0, chunkToSpawn.levelChunks.Length)];
            _previousChunk = chunkToSpawn;
            if (spawnDynamite)
            {
                if (chunkToSpawn.entryDirection == LevelChunkData.Direction.South)
                {
                    if (Random.Range(0.0f, 50.0f) < 5.0f)
                    {
                        var pos = _spawnPosition + spawnOrigin + new Vector3(0.0f, 0.5f, -1.0f);
                        Instantiate(dynamitePrefab, pos, dynamitePrefab.transform.rotation);
                    }
                }
            }

            Instantiate(objectFromChunk, _spawnPosition + spawnOrigin, objectFromChunk.transform.rotation);
        }

        LevelChunkData PickNextChunk()
        {
            List<LevelChunkData> allowedChunkList = new List<LevelChunkData>();
            LevelChunkData nextChunk = null;

            LevelChunkData.Direction nextRequiredDirection = LevelChunkData.Direction.North;

            switch (_previousChunk.exitDirection)
            {
                case LevelChunkData.Direction.North:
                    nextRequiredDirection = LevelChunkData.Direction.South;
                    _spawnPosition = _spawnPosition + new Vector3(0f, 0, _previousChunk.chunkSize.y);

                    break;
                case LevelChunkData.Direction.East:
                    nextRequiredDirection = LevelChunkData.Direction.East;
                    _spawnPosition = _spawnPosition + new Vector3(0f, 0, _previousChunk.chunkSize.y);
                    break;
                case LevelChunkData.Direction.South:
                    nextRequiredDirection = LevelChunkData.Direction.South;
                    _spawnPosition = _spawnPosition + new Vector3(0, 0, -_previousChunk.chunkSize.y);
                    break;
                case LevelChunkData.Direction.West:
                    nextRequiredDirection = LevelChunkData.Direction.West;
                    _spawnPosition = _spawnPosition + new Vector3(0f, 0, _previousChunk.chunkSize.y);

                    break;
                default:
                    break;
            }

            foreach (var t in levelChunkData)
            {
                if (t.entryDirection == nextRequiredDirection)
                {
                    allowedChunkList.Add(t);
                }
            }

            nextChunk = allowedChunkList[Random.Range(0, allowedChunkList.Count)];

            return nextChunk;
        }

        public void UpdateSpawnOrigin(Vector3 originDelta)
        {
            spawnOrigin = spawnOrigin + originDelta;
        }
    }
}