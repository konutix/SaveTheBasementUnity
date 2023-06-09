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
        GenerateMap();
    }

    void GenerateMap()
    {
        MapHeight = Random.Range(MinMapHeight, MaxMapHeight + 1);
        GameObject CurrentEncounterObject;
        //start mapy
        MapLayout = new Encounter[MapHeight, MaxMapWidth];
        Bounds MapBounds = SRenderer.bounds;
        float YDistance = MapBounds.size.y / (MapHeight + 1);
        CurrentEncounterObject = Instantiate(EncounterObject, new Vector2(MapBounds.center.x, MapBounds.max.y - YDistance), new Quaternion(), transform);
        MapLayout[0, 0] = CurrentEncounterObject.GetComponent<Encounter>();

        //œrodek mapy
        for (int i = 1; i < MapHeight - 1; i++)
        {
            int RowSize = Random.Range(MinMapWidth, MaxMapWidth + 1);
            float XDistance = MapBounds.size.x / (MaxMapWidth + 1);
            List<int> UsedIndexes = new List<int>();
            int SetEncounters = 0;
            for (int j = 0; j < RowSize; j++)
            {
                int EncounterIndex = -1;
                while (EncounterIndex == -1 || UsedIndexes.Contains(EncounterIndex))
                {
                    EncounterIndex = Random.Range(0, MaxMapWidth);
                }
                UsedIndexes.Add(EncounterIndex);
                SetEncounters++;
                CurrentEncounterObject = Instantiate(EncounterObject, new Vector2(MapBounds.min.x + XDistance * (EncounterIndex + 1), MapBounds.max.y - YDistance * (i+1)), new Quaternion(), transform);
                MapLayout[i, EncounterIndex] = CurrentEncounterObject.GetComponent<Encounter>();
            }
        }

        //boss
        CurrentEncounterObject = Instantiate(EncounterObject, new Vector2(MapBounds.center.x, MapBounds.max.y - YDistance * MapHeight), new Quaternion(), transform);
        MapLayout[MapHeight - 1, 0] = CurrentEncounterObject.GetComponent<Encounter>();

        //droga dla startu
        for (int i = 0; i < MaxMapWidth; i++)
        {
            Encounter NextEncounter = MapLayout[1, i];
            if (NextEncounter != null)
            {
                MapLayout[0, 0].NextEncounters.Add(NextEncounter);
                LineRenderer Line = MapLayout[0, 0].gameObject.GetComponent<LineRenderer>();
                if (Line.GetPosition(Line.positionCount - 1) != Vector3.zero)
                {
                    Line.positionCount += 2;
                }
                Line.SetPosition(Line.positionCount - 2, MapLayout[0, 0].gameObject.transform.position);
                Line.SetPosition(Line.positionCount - 1, NextEncounter.gameObject.transform.position);
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
                        LineRenderer Line = CurrentEncounter.gameObject.GetComponent<LineRenderer>();
                        Line.SetPosition(0, CurrentEncounter.gameObject.transform.position);
                        Line.SetPosition(1, DirectNextEncounter.gameObject.transform.position);
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
                                    LineRenderer Line = CurrentEncounter.gameObject.GetComponent<LineRenderer>();
                                    if (Line.GetPosition(Line.positionCount - 1) != Vector3.zero)
                                    {
                                        Line.positionCount += 2;
                                    }
                                    Line.SetPosition(Line.positionCount - 2, CurrentEncounter.gameObject.transform.position);
                                    Line.SetPosition(Line.positionCount - 1, DirectNextEncounter.gameObject.transform.position);
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
                                    LineRenderer Line = CurrentEncounter.gameObject.GetComponent<LineRenderer>();
                                    if (Line.GetPosition(Line.positionCount - 1) != Vector3.zero)
                                    {
                                        Line.positionCount += 2;
                                    }
                                    Line.SetPosition(Line.positionCount - 2, CurrentEncounter.gameObject.transform.position);
                                    Line.SetPosition(Line.positionCount - 1, DirectNextEncounter.gameObject.transform.position);
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
                                    LineRenderer Line = CurrentEncounter.gameObject.GetComponent<LineRenderer>();
                                    if(Line.GetPosition(Line.positionCount-1) != Vector3.zero)
                                    {
                                        Line.positionCount += 2;
                                    }
                                    Line.SetPosition(Line.positionCount-2, CurrentEncounter.gameObject.transform.position);
                                    Line.SetPosition(Line.positionCount-1, NextEncounter.gameObject.transform.position);
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
                                    LineRenderer Line = CurrentEncounter.gameObject.GetComponent<LineRenderer>();
                                    if (Line.GetPosition(Line.positionCount - 1) != Vector3.zero)
                                    {
                                        Line.positionCount += 2;
                                    }
                                    Line.SetPosition(Line.positionCount - 2, CurrentEncounter.gameObject.transform.position);
                                    Line.SetPosition(Line.positionCount - 1, NextEncounter.gameObject.transform.position);
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
                                    PreviousEncounter.NextEncounters.Add(PreviousEncounter);
                                    LineRenderer Line = PreviousEncounter.gameObject.GetComponent<LineRenderer>();
                                    if (Line.GetPosition(Line.positionCount - 1) != Vector3.zero)
                                    {
                                        Line.positionCount += 2;
                                    }
                                    Line.SetPosition(Line.positionCount - 2, PreviousEncounter.gameObject.transform.position);
                                    Line.SetPosition(Line.positionCount - 1, CurrentEncounter.gameObject.transform.position);
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
                                    PreviousEncounter.NextEncounters.Add(PreviousEncounter);
                                    LineRenderer Line = PreviousEncounter.gameObject.GetComponent<LineRenderer>();
                                    if (Line.GetPosition(Line.positionCount - 1) != Vector3.zero)
                                    {
                                        Line.positionCount += 2;
                                    }
                                    Line.SetPosition(Line.positionCount - 2, PreviousEncounter.gameObject.transform.position);
                                    Line.SetPosition(Line.positionCount - 1, CurrentEncounter.gameObject.transform.position);
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
    }

    
}
