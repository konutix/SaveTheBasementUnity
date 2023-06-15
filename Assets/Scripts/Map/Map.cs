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
    [SerializeField] GameObject EncounterObject;

    Encounter[,] MapLayout;
    int MapHeight;

    void Start()
    {
        if(RunState.CurrentMap == null)
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
        MapLayout[0, 0] = new Encounter();

        //œrodek mapy
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
                MapLayout[i, EncounterIndex] = new Encounter();
            }
        }

        //boss
        MapLayout[MapHeight - 1, 0] = new Encounter();

        //droga dla startu
        for (int i = 0; i < MaxMapWidth; i++)
        {
            Encounter NextEncounter = MapLayout[1, i];
            if (NextEncounter != null)
            {
                MapLayout[0, 0].NextEncounters.Add(NextEncounter);
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
                        CurrentEncounter.NextEncounters.Add(DirectNextEncounter);
                        DirectNextEncounter.PreviousEncounters.Add(CurrentEncounter);
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
                                    CurrentEncounter.NextEncounters.Add(DirectNextEncounter);
                                    DirectNextEncounter.PreviousEncounters.Add(CurrentEncounter);
                                    if (CurrentEncounter.NextEncounters.Count > 0)
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
                                    CurrentEncounter.NextEncounters.Add(DirectNextEncounter);
                                    DirectNextEncounter.PreviousEncounters.Add(CurrentEncounter);
                                    if (CurrentEncounter.NextEncounters.Count > 0)
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
                                    CurrentEncounter.NextEncounters.Add(NextEncounter);
                                    NextEncounter.PreviousEncounters.Add(CurrentEncounter);
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
                                    CurrentEncounter.NextEncounters.Add(NextEncounter);
                                    NextEncounter.PreviousEncounters.Add(CurrentEncounter);
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
                    if (CurrentEncounter.PreviousEncounters.Count == 0)
                    {
                        Encounter PreviousEncounter;
                        for (int a = 1; a < MaxMapWidth; a++)
                        {
                            if (j - a >= 0)
                            {
                                PreviousEncounter = MapLayout[i - 1, j - a];
                                if (PreviousEncounter != null)
                                {
                                    CurrentEncounter.PreviousEncounters.Add(PreviousEncounter);
                                    PreviousEncounter.NextEncounters.Add(CurrentEncounter);
                                    if (CurrentEncounter.PreviousEncounters.Count > 0)
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
                                    CurrentEncounter.PreviousEncounters.Add(PreviousEncounter);
                                    PreviousEncounter.NextEncounters.Add(CurrentEncounter);
                                    if (CurrentEncounter.PreviousEncounters.Count > 0)
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

        RunState.CurrentMap = MapLayout;
        ShowMap();
    }

    public void LoadMap()
    {
        MapLayout = RunState.CurrentMap;
        MapHeight = MapLayout.GetLength(0);
        ShowMap();
    }

    void ShowMap()
    {
        GameObject CurrentEncounterObject;
        Bounds MapBounds = SRenderer.bounds;
        float YDistance = MapBounds.size.y / (MapHeight + 1);
        float XDistance = MapBounds.size.x / (MaxMapWidth + 1);
        CurrentEncounterObject = Instantiate(EncounterObject, new Vector2(MapBounds.center.x, MapBounds.max.y - YDistance), new Quaternion(), transform);

        CurrentEncounterObject.GetComponent<MapNode>().SetEncounter(MapLayout[0, 0]);
        for (int i = 1; i < MapHeight - 1; i++)
        {
            for (int j = 0; j < MaxMapWidth; j++)
            {
                Encounter encounter = MapLayout[i, j];
                if (encounter != null)
                {
                    CurrentEncounterObject = Instantiate(EncounterObject, new Vector2(MapBounds.min.x + XDistance * (j + 1), MapBounds.max.y - YDistance * (i + 1)), new Quaternion(), transform);
                    CurrentEncounterObject.GetComponent<MapNode>().SetEncounter(encounter);
                }
            }
        }
        CurrentEncounterObject = Instantiate(EncounterObject, new Vector2(MapBounds.center.x, MapBounds.max.y - YDistance * MapHeight), new Quaternion(), transform);
        CurrentEncounterObject.GetComponent<MapNode>().SetEncounter(MapLayout[MapHeight - 1, 0]);

        for (int i = 0; i < MapHeight - 1; i++)
        {
            for (int j = 0; j < MaxMapWidth; j++)
            {
                Encounter encounter = MapLayout[i, j];
                if (encounter != null)
                {
                    GameObject Node = encounter.Node.gameObject;
                    LineRenderer Line = Node.GetComponent<LineRenderer>();
                    int NextEncountersNumber = encounter.NextEncounters.Count;
                    Line.positionCount = NextEncountersNumber * 2;
                    for (int k=0; k<NextEncountersNumber; k++)
                    {
                        Line.SetPosition(k * 2, Node.transform.position);
                        Line.SetPosition(k * 2 + 1, encounter.NextEncounters[k].Node.gameObject.transform.position);
                    }
                }
            }
        }
    }

    [ContextMenu("Reset Map")]
    void ResetMap()
    {
        RunState.CurrentMap = null;
    }

}
