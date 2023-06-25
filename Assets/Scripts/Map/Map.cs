using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] SpriteRenderer SRenderer;
    [Space]
    [SerializeField] int MinMapWidth;
    [SerializeField] int MaxMapWidth;
    [Space]
    [SerializeField] int MinMapHeight;
    [SerializeField] int MaxMapHeight;
    [Space]
    [SerializeField] int MaxAdditionalRoads;
    [Space]
    [SerializeField] GameObject StartEncounterObject;
    [SerializeField] GameObject SingleEnemyEncounterObject;
    [SerializeField] GameObject MultipleEnemyEncounterObject;
    [SerializeField] GameObject BossEncounterObject;
    [Space]
    [SerializeField] int SingleEnemyMinFangs;
    [SerializeField] int SingleEnemyMaxFangs;
    [SerializeField] int MultipleEnemyMinFangs;
    [SerializeField] int MultipleEnemyMaxFangs;
    [Space]
    [SerializeField] GameObject UpgradeEncounterObject;
    [SerializeField] int MinUpgradeNumber;
    [SerializeField] int MaxUpgradeNumber;

    [Tooltip("Which layers should have upgrades only on show (starting from layer 1, as 0 is start)")]
    [SerializeField] List<int> UpgradeMapLayerLimit;
    [Space]
    [SerializeField] GameObject ChestEncounterObject;
    [SerializeField] int MinChestNumber;
    [SerializeField] int MaxChestNumber;

    [Tooltip("Which layers should have chests only on show (starting from layer 1, as 0 is start)")]
    [SerializeField] List<int> ChestMapLayerLimit;

    Encounter[,] MapLayout;
    int MapHeight;

    void Start()
    {
        if(RunState.currentMap == null)
        {
            GenerateMap();
        }
        else
        {
            LoadMap();
        }
    }

    void GenerateMap()
    {
        MapHeight = Random.Range(MinMapHeight, MaxMapHeight + 1);
        //start mapy
        MapLayout = new Encounter[MapHeight, MaxMapWidth];
        MapLayout[0, 0] = new StartEncounter();

        //œrodek mapy
        List<Vector2Int> EncountersCoords = new List<Vector2Int>();
        for (int i = 1; i < MapHeight - 1; i++)
        {
            int RowSize = Random.Range(MinMapWidth, MaxMapWidth + 1);
            List<int> UsedIndexes = new List<int>();
            for (int j = 0; j < RowSize; j++)
            {
                int EncounterIndex = -1;
                while (EncounterIndex == -1 || UsedIndexes.Contains(EncounterIndex))
                {
                    EncounterIndex = Random.Range(0, MaxMapWidth);
                }
                UsedIndexes.Add(EncounterIndex);
                EncountersCoords.Add(new Vector2Int(i, j));
                int EncounterType = Random.Range(0, 2);
                if (EncounterType == 0)
                {
                    MapLayout[i, EncounterIndex] = new SingleEnemyEncounter();
                    MapLayout[i, EncounterIndex].vampireFangsReward = Random.Range(SingleEnemyMinFangs, SingleEnemyMaxFangs+1);
                }
                else if(EncounterType == 1)
                {
                    MapLayout[i, EncounterIndex] = new MultipleEnemyEncounter();
                    MapLayout[i, EncounterIndex].vampireFangsReward = Random.Range(MultipleEnemyMinFangs, MultipleEnemyMaxFangs + 1);
                }
            }
        }

        //losowanie typów encounterów
        int UpgradeNumber = Random.Range(MinUpgradeNumber, MaxUpgradeNumber + 1);
        if(UpgradeMapLayerLimit == null || UpgradeMapLayerLimit.Count == 0)
        {
            for (int i = 0; i < UpgradeNumber; i++)
            {
                int index = Random.Range(0, EncountersCoords.Count);
                Vector2Int Coords = EncountersCoords[index];
                EncountersCoords.RemoveAt(index);
                MapLayout[Coords.x, Coords.y] = new UpgradeEncounter();
            }
        }
        else
        {
            List<Vector2Int> SpecificCoords = EncountersCoords.FindAll(encounter => UpgradeMapLayerLimit.Contains(encounter.x));
            for (int i = 0; i < UpgradeNumber; i++)
            {
                if(SpecificCoords.Count != 0)
                {
                    int index = Random.Range(0, SpecificCoords.Count);
                    Vector2Int Coords = SpecificCoords[index];
                    SpecificCoords.RemoveAt(index);
                    MapLayout[Coords.x, Coords.y] = new UpgradeEncounter();
                }
            }
        }

        int ChestNumber = Random.Range(MinChestNumber, MaxChestNumber + 1);
        if (ChestMapLayerLimit == null || ChestMapLayerLimit.Count == 0)
        {
            for (int i = 0; i < ChestNumber; i++)
            {
                int index = Random.Range(0, EncountersCoords.Count);
                Vector2Int Coords = EncountersCoords[index];
                EncountersCoords.RemoveAt(index);
                MapLayout[Coords.x, Coords.y] = new ChestEncounter();
            }
        }
        else
        {
            List<Vector2Int> SpecificCoords = EncountersCoords.FindAll(encounter => ChestMapLayerLimit.Contains(encounter.x));
            for (int i = 0; i < ChestNumber; i++)
            {
                if (SpecificCoords.Count != 0)
                {
                    int index = Random.Range(0, SpecificCoords.Count);
                    Vector2Int Coords = SpecificCoords[index];
                    SpecificCoords.RemoveAt(index);
                    MapLayout[Coords.x, Coords.y] = new ChestEncounter();
                }
            }
        }

        //boss
        MapLayout[MapHeight - 1, 0] = new BossEncounter();

        //droga dla startu
        for (int i = 0; i < MaxMapWidth; i++)
        {
            Encounter NextEncounter = MapLayout[1, i];
            if (NextEncounter != null)
            {
                MapLayout[0, 0].nextEncounters.Add(NextEncounter);
            }
        }
        //ustalanie bezposredniej drogi dla srodka
        for (int i = 1; i < MapHeight - 1; i++)
        {
            for (int j = 0; j < MaxMapWidth; j++)
            {
                Encounter CurrentEncounter = MapLayout[i, j];
                if (CurrentEncounter != null)
                {
                    Encounter DirectNextEncounter = MapLayout[i + 1, j];
                    if (DirectNextEncounter != null)
                    {
                        CurrentEncounter.nextEncounters.Add(DirectNextEncounter);
                        DirectNextEncounter.previousEncounters.Add(CurrentEncounter);
                    }
                    else
                    {
                        for (int a = 1; a < MaxMapWidth; a++)
                        {
                            if (j - a >= 0)
                            {
                                DirectNextEncounter = MapLayout[i + 1, j - a];
                                if (DirectNextEncounter != null)
                                {
                                    CurrentEncounter.nextEncounters.Add(DirectNextEncounter);
                                    DirectNextEncounter.previousEncounters.Add(CurrentEncounter);
                                    if (CurrentEncounter.nextEncounters.Count > 0)
                                    {
                                        break;
                                    }
                                }
                            }
                            if (j + a < MaxMapWidth)
                            {
                                DirectNextEncounter = MapLayout[i + 1, j + a];
                                if (DirectNextEncounter != null)
                                {
                                    CurrentEncounter.nextEncounters.Add(DirectNextEncounter);
                                    DirectNextEncounter.previousEncounters.Add(CurrentEncounter);
                                    if (CurrentEncounter.nextEncounters.Count > 0)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        //dodatkowe drogi
        for (int i = 1; i < MapHeight - 1; i++)
        {
            for (int j = 0; j < MaxMapWidth; j++)
            {
                Encounter CurrentEncounter = MapLayout[i, j];
                if (CurrentEncounter != null)
                {
                    int AdditionalRoads = Random.Range(0, MaxAdditionalRoads + 1);
                    int SetRoads = 0;
                    Encounter NextEncounter;
                    if(AdditionalRoads > 0)
                    {
                        for (int a = 1; a < MaxMapWidth; a++)
                        {
                            if (j - a >= 0)
                            {
                                NextEncounter = MapLayout[i + 1, j - a];
                                if (NextEncounter != null)
                                {
                                    CurrentEncounter.nextEncounters.Add(NextEncounter);
                                    NextEncounter.previousEncounters.Add(CurrentEncounter);
                                    SetRoads++;
                                    if (SetRoads >= AdditionalRoads)
                                    {
                                        break;
                                    }
                                }
                            }
                            if (j + a < MaxMapWidth)
                            {
                                NextEncounter = MapLayout[i + 1, j + a];
                                if (NextEncounter != null)
                                {
                                    CurrentEncounter.nextEncounters.Add(NextEncounter);
                                    NextEncounter.previousEncounters.Add(CurrentEncounter);
                                    SetRoads++;
                                    if (SetRoads >= AdditionalRoads)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        //sprawdzenie czy ma poprzednika
        for (int i = 1; i < MapHeight - 1; i++)
        {
            for (int j = 0; j < MaxMapWidth; j++)
            {
                Encounter CurrentEncounter = MapLayout[i, j];
                if (CurrentEncounter != null)
                {
                    if (CurrentEncounter.previousEncounters.Count == 0)
                    {
                        Encounter PreviousEncounter;
                        for (int a = 1; a < MaxMapWidth; a++)
                        {
                            if (j - a >= 0)
                            {
                                PreviousEncounter = MapLayout[i - 1, j - a];
                                if (PreviousEncounter != null)
                                {
                                    CurrentEncounter.previousEncounters.Add(PreviousEncounter);
                                    PreviousEncounter.nextEncounters.Add(CurrentEncounter);
                                    if (CurrentEncounter.previousEncounters.Count > 0)
                                    {
                                        break;
                                    }
                                }
                            }
                            if (j + a < MaxMapWidth)
                            {
                                PreviousEncounter = MapLayout[i - 1, j + a];
                                if (PreviousEncounter != null)
                                {
                                    CurrentEncounter.previousEncounters.Add(PreviousEncounter);
                                    PreviousEncounter.nextEncounters.Add(CurrentEncounter);
                                    if (CurrentEncounter.previousEncounters.Count > 0)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        RunState.currentMap = MapLayout;
        CheckCurrentEncounter(MapLayout[0,0]);
        ShowMap();
    }

    public void LoadMap()
    {
        MapLayout = RunState.currentMap;
        MapHeight = MapLayout.GetLength(0);
        ShowMap();
    }

    void ShowMap()
    {
        GameObject CurrentEncounterObject;
        Bounds MapBounds = SRenderer.bounds;
        float YDistance = MapBounds.size.y / (MapHeight + 1);
        float XDistance = MapBounds.size.x / (MaxMapWidth + 1);
        CurrentEncounterObject = Instantiate(StartEncounterObject, new Vector2(MapBounds.center.x, MapBounds.max.y - YDistance), new Quaternion(), transform);

        CurrentEncounterObject.GetComponent<MapNode>().SetEncounter(MapLayout[0, 0]);
        for (int i = 1; i < MapHeight - 1; i++)
        {
            for (int j = 0; j < MaxMapWidth; j++)
            {
                Encounter encounter = MapLayout[i, j];
                if (encounter != null)
                {
                    if(encounter.GetType().Equals(typeof(SingleEnemyEncounter)))
                    {
                        CurrentEncounterObject = Instantiate(SingleEnemyEncounterObject, new Vector2(MapBounds.min.x + XDistance * (j + 1), MapBounds.max.y - YDistance * (i + 1)), new Quaternion(), transform);
                        CurrentEncounterObject.GetComponent<MapNode>().SetEncounter(encounter);
                        
                    }
                    else if(encounter.GetType().Equals(typeof(MultipleEnemyEncounter)))
                    {
                        CurrentEncounterObject = Instantiate(MultipleEnemyEncounterObject, new Vector2(MapBounds.min.x + XDistance * (j + 1), MapBounds.max.y - YDistance * (i + 1)), new Quaternion(), transform);
                        CurrentEncounterObject.GetComponent<MapNode>().SetEncounter(encounter);
                    }
                    else if (encounter.GetType().Equals(typeof(UpgradeEncounter)))
                    {
                        CurrentEncounterObject = Instantiate(UpgradeEncounterObject, new Vector2(MapBounds.min.x + XDistance * (j + 1), MapBounds.max.y - YDistance * (i + 1)), new Quaternion(), transform);
                        CurrentEncounterObject.GetComponent<MapNode>().SetEncounter(encounter);
                    }
                    else if (encounter.GetType().Equals(typeof(ChestEncounter)))
                    {
                        CurrentEncounterObject = Instantiate(ChestEncounterObject, new Vector2(MapBounds.min.x + XDistance * (j + 1), MapBounds.max.y - YDistance * (i + 1)), new Quaternion(), transform);
                        CurrentEncounterObject.GetComponent<MapNode>().SetEncounter(encounter);
                    }
                }
            }
        }
        CurrentEncounterObject = Instantiate(BossEncounterObject, new Vector2(MapBounds.center.x, MapBounds.max.y - YDistance * MapHeight), new Quaternion(), transform);
        CurrentEncounterObject.GetComponent<MapNode>().SetEncounter(MapLayout[MapHeight - 1, 0]);

        for (int i = 0; i < MapHeight - 1; i++)
        {
            for (int j = 0; j < MaxMapWidth; j++)
            {
                Encounter encounter = MapLayout[i, j];
                if (encounter != null)
                {
                    GameObject Node = encounter.node.gameObject;
                    LineRenderer Line = Node.GetComponent<LineRenderer>();
                    int NextEncountersNumber = encounter.nextEncounters.Count;
                    Line.positionCount = NextEncountersNumber * 2;
                    for (int k=0; k<NextEncountersNumber; k++)
                    {
                        Line.SetPosition(k * 2, Node.transform.position);
                        Line.SetPosition(k * 2 + 1, encounter.nextEncounters[k].node.gameObject.transform.position);
                    }
                }
            }
        }
        MapLayout[MapHeight - 1, 0].node.gameObject.GetComponent<LineRenderer>().positionCount = 0;
    }

    public void CheckCurrentEncounter(Encounter NewCurrentEncounter)
    {
        Encounter CurrentEncounter = RunState.currentEncounter;
        if (CurrentEncounter != null)
        {
            CurrentEncounter.encounterState = EncounterStateEnum.Completed;
            foreach (Encounter Enc in CurrentEncounter.nextEncounters)
            {
                Enc.encounterState = EncounterStateEnum.Incompleted;
            }
        }
        RunState.currentEncounter = NewCurrentEncounter;

        NewCurrentEncounter.encounterState = EncounterStateEnum.Completed;
        foreach (Encounter Enc in NewCurrentEncounter.nextEncounters)
        {
            Enc.encounterState = EncounterStateEnum.Ready;
        }
     
    }
    [ContextMenu("Reset Map")]
    void ResetMap()
    {
        RunState.currentMap = null;
    }
}
