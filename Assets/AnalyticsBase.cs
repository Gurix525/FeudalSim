using System.Collections.Generic;
using System.Linq;

namespace Assets
{
    public static class AnalyticsBase
    {
        public static Dictionary<string, Dictionary<string, int>> Events { get; private set; } = GetClearDictionary();

        public static void Add(string eventName, string parameterName, int number = 1)
        {
            if (!Events.ContainsKey(eventName))
                return;
            if (!Events[eventName].ContainsKey(parameterName))
                return;
            Events[eventName][parameterName] += number;
        }

        public static void Send()
        {
            foreach (var item in Events)
            {
                Analytics.SendEvent(item.Key, dictionary: item.Value.ToDictionary(
                    (lol) => lol.Key,
                    (lol) => (object)lol.Value));
            }
            Events = GetClearDictionary();
        }

        private static Dictionary<string, Dictionary<string, int>> GetClearDictionary()
        {
            return new()
        {
            { "buttonClicked",
                new()
                {
                    { "mainMenuPlayButton", 0 },
                    { "mainMenuSettingsButton", 0 },
                    { "mainMenuExitButton", 0 },
                    { "settingsReturnButton", 0 },
                    { "selectWorldReturnButton", 0 },
                    { "selectWorldCreateNewWorldButton", 0 },
                    { "selectWorldWorldButton", 0 },
                    { "selectWorldDeleteButton", 0 },
                    { "createANewWorldReturnButton", 0 },
                    { "createANewWorldCreateWorldButton", 0 },
                    { "gameMenuReturnButton", 0 },
                    { "gameMenuSettingsButton", 0 },
                    { "gameMenuSettingsReturnButton", 0 },
                    { "gameMenuSaveQuitButton", 0 }
                }
            },
            { "buildingBuilt",
                new()
                {
                    { "plankFloor", 0 },
                    { "plankWall", 0 },
                    { "plankStairs", 0 },
                    { "stoneFloor", 0 },
                    { "stoneStairs", 0 },
                    { "stoneWall", 0 },
                    { "woodFloor", 0 },
                    { "woodStairs", 0 },
                    { "woodWall", 0 },
                    { "chest", 0 },
                    { "door", 0 },
                }
            },
            { "buildingDestroyed",
                new()
                {
                    { "plankFloor", 0 },
                    { "plankWall", 0 },
                    { "plankStairs", 0 },
                    { "stoneFloor", 0 },
                    { "stoneStairs", 0 },
                    { "stoneWall", 0 },
                    { "woodFloor", 0 },
                    { "woodStairs", 0 },
                    { "woodWall", 0 },
                    { "chest", 0 },
                    { "door", 0 },
                }
            },
            { "itemPicked",
                new()
                {
                    { "boarSkin", 0 },
                    { "hareSkin", 0 },
                    { "plank", 0 },
                    { "stone", 0 },
                    { "wolfSkin", 0 },
                    { "wood", 0 },
                }
            },
            { "itemDropped",
                new()
                {
                    { "boarSkin", 0 },
                    { "hareSkin", 0 },
                    { "plank", 0 },
                    { "stone", 0 },
                    { "wolfSkin", 0 },
                    { "wood", 0 },
                }
            },
            { "entityKilled",
                new()
                {
                    { "boar", 0 },
                    { "hare", 0 },
                    { "player", 0 },
                    { "wolf", 0 }
                }
            },
            { "natureDestroyed",
                new()
                {
                    { "boulder", 0 },
                    { "tree", 0 }
                }
            }
        };
        }
    }
}