using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RunState
{
    public static float maxPlayerHealth;
    public static float currentPlayerHealth;

    public static int Mana;

    public static List<int> deck;
    public static int cardsDrawn;
    
    public static int vampireFangs;

    public static Encounter[,] currentMap;
    public static Encounter currentEncounter;

}
