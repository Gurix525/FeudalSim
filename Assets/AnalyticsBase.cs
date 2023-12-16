using System.Collections.Generic;

namespace Assets
{
    public static class AnalyticsBase
    {
        public static Dictionary<string, Dictionary<string, int>> Events { get; } = new()
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


        public static void Add(string eventName, string parameterName, int number = 1)
        {
            Events[eventName][parameterName] += number;
        }
    }
}