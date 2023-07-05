using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameWinStats {

    public static float gameTime;
    public static int resourcesGathered;
    public static int zombiesKilled;
    public static int trackCheckpoints;
    public static int carsBuilt;

    public static void Init() {
        gameTime = 0;
        resourcesGathered = 0;
        zombiesKilled = 0;
        trackCheckpoints = 0;
        carsBuilt = 0;
    }

}
