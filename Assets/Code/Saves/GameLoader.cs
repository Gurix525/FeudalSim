using System;
using System.IO;
using System.IO.Compression;
using Misc;
using UnityEngine;
using World;

namespace Saves
{
    public class GameLoader
    {
        private string _worldName;
        private string _savePath;

        public GameLoader(string worldName)
        {
            _worldName = worldName;
            _savePath = Path.Combine(
                Application.persistentDataPath, "Saves", _worldName);
        }

        public void LoadGame()
        {
            try
            {
                ZipFile.ExtractToDirectory(_savePath + ".zip", _savePath);
                LoadWorldInfo();
                LoadPlayerInfo();
                LoadChunkInfos();

                Directory.Delete(_savePath, true);
                //NoiseSampler.SetSeed(random.Next());
                GrassInstancer.MarkToReload();
                //References.GetReference("Player").transform.position = new(0F, originHeight, 0F);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        private void LoadWorldInfo()
        {
            WorldInfo worldInfo = JsonUtility.FromJson<WorldInfo>(
                                File.ReadAllText(
                                Path.Combine(_savePath, "World.txt")));
        }

        private void LoadPlayerInfo()
        {
            PlayerInfo playerInfo = JsonUtility.FromJson<PlayerInfo>(
                                File.ReadAllText(
                                Path.Combine(_savePath, "Player.txt")));
        }

        private void LoadChunkInfos()
        {
            string[] fileNames = Directory.GetFiles(Path.Combine(_savePath, "Chunks"));
            ChunkInfo[] chunkInfos = new ChunkInfo[fileNames.Length];
            for (int i = 0; i < fileNames.Length; i++)
            {
                chunkInfos[i] = JsonUtility.FromJson<ChunkInfo>(
                    File.ReadAllText(fileNames[i]));
            }
        }
    }
}