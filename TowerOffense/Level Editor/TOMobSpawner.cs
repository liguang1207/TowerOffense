using System;
using System.Linq;
using System.Text;
using TowerOffense.Entities;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace TowerOffense.Level_Editor
{
    public class TOMobSpawner
    {
        private int pSpawnsLeft = 0;
        private TOTile pSpawnZone = null;
        private DateTime pLastSpawn;
        private List<TOTile> pPath = new List<TOTile>();

        private TOWorldInfo WorldInfo = TOWorldInfo.Instance;

        public TOMobSpawner(TOTile aSpawnZone)
        {
            pSpawnZone = aSpawnZone;
        }

        public void StartWave(int aSpawnsLeft)
        {
            pSpawnsLeft = aSpawnsLeft;

            //Set Path
            TOTile EndZone = GetClosestEndZone();
            //Console.WriteLine(pSpawnZone.Position.ToString());
            //Console.WriteLine(EndZone.Position.ToString());
            pPath = AStar(pSpawnZone, EndZone);
        }

        public void Update(GameTime aGameTime)
        {
            if (DateTime.Now.Subtract(pLastSpawn).TotalSeconds >= 1 && pSpawnsLeft > 0)
            {
                pSpawnsLeft--;

                //Spawn Item
                TOBug aBug = new TOBug(pSpawnZone.Position, TOWorldInfo.Instance.GetGame().GetSprite("Bug_Generic"));
                aBug.Path = pPath;

                pLastSpawn = DateTime.Now;
            }
        }

        private TOTile GetClosestEndZone()
        {
            TOTile EndZone = null;
            float Shortest = 1000f;

            foreach (TOActor aActor in WorldInfo.AllActors)
            {
                if (aActor is TOTile)
                {
                    TOTile aTile = aActor as TOTile;
                    float aDistance = Vector2.Distance(pSpawnZone.Position, aTile.Position);
                    if (aTile.CurrentState == TowerState.EndZone && aDistance < Shortest)
                    {
                        EndZone = aTile;
                        Shortest = aDistance;
                    }
                }
            }

            return EndZone;

        }

        private List<TOTile> AStar(TOTile Start, TOTile End)
        {
            List<TOTile> Open = new List<TOTile>();
            List<TOTile> Closed = new List<TOTile>();
            List<TOTile> shortestPath = new List<TOTile>();
            TOTile[,] GameGrid = WorldInfo.GameGrid;
            Boolean TargetFound = false;

            int TempB, TempT, TempL, TempR;

            //Get Coordinates Of Start
            int CurrentX = (int)Start.Position.X / TOTile.MAX_WIDTH;
            int CurrentY = (int)Start.Position.Y / TOTile.MAX_WIDTH;

            //Get Coordinate Of End
            int TargetX = (int)End.Position.X / TOTile.MAX_WIDTH;
            int TargetY = (int)End.Position.Y / TOTile.MAX_WIDTH;

            //Initialize Start Node
            Start.C = 0;
            Start.H = (int)(Math.Abs(CurrentX - TargetX) + Math.Abs(CurrentY - TargetY));
            Start.T = Start.C + Start.H;
            Open.Add(Start);

            int GridLength = 10;

           /* foreach (TOActor aActor in WorldInfo.AllActors)
            {
                if (aActor is TOTile)
                {
                    TOTile aTile = aActor as TOTile;
                    float aDistance = Vector2.Distance(pSpawnZone.Position, aTile.Position);
                    if (aTile.CurrentState == TowerState.Open)
                    {
                        Open.Add(aTile);
                    }
                }
            }
            */
            //Bottom
            if (CurrentX + 1 > -1 && CurrentX + 1 < GridLength && CurrentY > -1 && CurrentY < GridLength && !GameGrid[CurrentX + 1,CurrentY].Blocked && !GameGrid[CurrentX + 1,CurrentY].Closed)
            {
                GameGrid[CurrentX + 1,CurrentY].Parent = GameGrid[CurrentX,CurrentY];
                GameGrid[CurrentX + 1,CurrentY].C = 10;
                GameGrid[CurrentX + 1, CurrentY].H = Math.Abs(TargetX - (CurrentX + 1)) + Math.Abs(TargetY - (CurrentY));
                GameGrid[CurrentX + 1, CurrentY].T = GameGrid[CurrentX + 1, CurrentY].C + GameGrid[CurrentX + 1, CurrentY].H;
                GameGrid[CurrentX + 1,CurrentY].Open = true;
                if (GameGrid[CurrentX + 1, CurrentY] == End)
                {
                    GameGrid[CurrentX,CurrentY].Closed = true;
                    Closed.Add(GameGrid[CurrentX,CurrentY]);
                    Closed.Add(GameGrid[CurrentX + 1, CurrentY]);
                    TargetFound = true;
                }
                if (!TargetFound)
                {
                    Open.Add(GameGrid[CurrentX + 1, CurrentY]);
                }
                TempB = GameGrid[CurrentX + 1, CurrentY].T;
            }

            //top
            if (CurrentX - 1 > -1 && CurrentX - 1 < GridLength && CurrentY > -1 && CurrentY < GridLength && !GameGrid[CurrentX - 1,CurrentY].Blocked && !GameGrid[CurrentX - 1,CurrentY].Closed)
            {
                GameGrid[CurrentX - 1, CurrentY].Parent = GameGrid[CurrentX, CurrentY];
                GameGrid[CurrentX - 1, CurrentY].C = 10;
                GameGrid[CurrentX - 1, CurrentY].H = Math.Abs(TargetX - (CurrentX - 1)) + Math.Abs(TargetY - (CurrentY));
                GameGrid[CurrentX - 1, CurrentY].T = GameGrid[CurrentX - 1, CurrentY].C + GameGrid[CurrentX - 1, CurrentY].H;
                GameGrid[CurrentX - 1, CurrentY].Open = true;
                if (GameGrid[CurrentX - 1, CurrentY] == End)
                {
                    GameGrid[CurrentX, CurrentY].Closed = true;
                    Closed.Add(GameGrid[CurrentX, CurrentY]);
                    Closed.Add(GameGrid[CurrentX - 1,CurrentY]);
                    TargetFound = true;
                }
                if (!TargetFound)
                {
                    Open.Add(GameGrid[CurrentX - 1,CurrentY]);
                }
                TempT = GameGrid[CurrentX - 1,CurrentY].T;
            }

            //Left
            if (CurrentX > -1 && CurrentX < GridLength && CurrentY - 1 > -1 && CurrentY - 1 < GridLength && !GameGrid[CurrentX,CurrentY - 1].Blocked && !GameGrid[CurrentX,CurrentY - 1].Closed)
            {
                GameGrid[CurrentX, CurrentY - 1].Parent = GameGrid[CurrentX,CurrentY];
                GameGrid[CurrentX,CurrentY - 1].C = 10;
                GameGrid[CurrentX,CurrentY - 1].H = Math.Abs(TargetX - (CurrentX)) + Math.Abs(TargetY - (CurrentY - 1));
                GameGrid[CurrentX, CurrentY - 1].T = GameGrid[CurrentX, CurrentY - 1].C + GameGrid[CurrentX, CurrentY - 1].H;
                GameGrid[CurrentX,CurrentY - 1].Open = true;
                if (GameGrid[CurrentX,CurrentY - 1] == End)
                {
                    GameGrid[CurrentX,CurrentY].Closed = true;
                    Closed.Add(GameGrid[CurrentX,CurrentY]);
                    Closed.Add(GameGrid[CurrentX,CurrentY - 1]);
                    TargetFound = true;
                }
                if (!TargetFound)
                {
                    Open.Add(GameGrid[CurrentX, CurrentY - 1]);
                }
                TempL = GameGrid[CurrentX,CurrentY - 1].T;
            }

            //Right
            if (CurrentX > -1 && CurrentX < GridLength && CurrentY + 1 > -1 && CurrentY + 1 < GridLength && !GameGrid[CurrentX,CurrentY + 1].Blocked && !GameGrid[CurrentX,CurrentY + 1].Closed)
            {
                GameGrid[CurrentX,CurrentY + 1].Parent = GameGrid[CurrentX,CurrentY];
                GameGrid[CurrentX,CurrentY + 1].C = 10;
                GameGrid[CurrentX,CurrentY + 1].H = Math.Abs(TargetX - (CurrentX)) + Math.Abs(TargetY - (CurrentY + 1));
                GameGrid[CurrentX,CurrentY + 1].T = GameGrid[CurrentX,CurrentY + 1].C + GameGrid[CurrentX,CurrentY + 1].H;
                GameGrid[CurrentX,CurrentY + 1].Open = true;
                if (GameGrid[CurrentX,CurrentY + 1] == End)
                {
                    GameGrid[CurrentX,CurrentY].Closed = true;
                    Closed.Add(GameGrid[CurrentX,CurrentY]);
                    Closed.Add(GameGrid[CurrentX,CurrentY + 1]);
                    TargetFound = true;
                }
                if (!TargetFound)
                {
                    Open.Add(GameGrid[CurrentX,CurrentY + 1]);
                }
                TempR = GameGrid[CurrentX,CurrentY + 1].T;
            }
            if (!TargetFound)
            {
                GameGrid[CurrentX, CurrentY].Closed = true;

                //move the current node to Closed list after evaluating all adjacent nodes
                Closed.Add(GameGrid[CurrentX,CurrentY]);
                Open.RemoveAt(0);

                //loop through the rest of the nodes in Open list
                int selectedIndex;
                int lowestT = 0;
                int aCurrentX = CurrentX;
                int aCurrentY = CurrentY;
                while (TargetFound == false)
                {
                    //find the lowest T C of all the node in Open list and pick it as the selected node
                    selectedIndex = 0;
                    if (Open.Count > 0)
                    {
                        lowestT = Open[0].T;
                    }

                    for (int i = 0; i < Open.Count; i++)
                    {
                        if (Open[i].T < lowestT)
                        {
                            lowestT = Open[i].T;
                            selectedIndex = i;
                        }
                    }

                    if (Open.Count > 0)
                    {
                        aCurrentX = (int)Open[selectedIndex].Position.X / TOTile.MAX_WIDTH;
                        aCurrentY = (int)Open[selectedIndex].Position.Y / TOTile.MAX_WIDTH;
                    }


                    if (GameGrid[aCurrentX, aCurrentY] == End)
                    {
                        Closed.Add(GameGrid[aCurrentX,aCurrentY]);
                        TargetFound = true;

                        break;
                    }

                    //using the selected node go through all its adjacent node and add to Open list unless already added or its a wall.
                    //bot
                    if (aCurrentX + 1 < GridLength && aCurrentX + 1 > -1 && aCurrentY > -1 && aCurrentY < GridLength && !GameGrid[aCurrentX + 1,aCurrentY].Blocked && !GameGrid[aCurrentX + 1,aCurrentY].Closed && !GameGrid[aCurrentX + 1,aCurrentY].Open)
                    {
                        GameGrid[aCurrentX + 1,aCurrentY].Parent = GameGrid[aCurrentX,aCurrentY];
                        GameGrid[aCurrentX + 1,aCurrentY].C = GameGrid[aCurrentX,aCurrentY].C + 10;
                        GameGrid[aCurrentX + 1,aCurrentY].H = Math.Abs(TargetX - (aCurrentX + 1)) + Math.Abs(TargetY - (aCurrentY));
                        GameGrid[aCurrentX + 1,aCurrentY].T = GameGrid[aCurrentX + 1,aCurrentY].C + GameGrid[aCurrentX + 1,aCurrentY].H;
                        GameGrid[aCurrentX + 1,aCurrentY].Open = true;

                        //if the adjacent node is the target then add to Closed list and break from loop
                        if (GameGrid[aCurrentX + 1, aCurrentY] == End)
                        {
                            GameGrid[aCurrentX,aCurrentY].Closed = true;
                            GameGrid[aCurrentX + 1,aCurrentY].Closed = true;
                            Closed.Add(GameGrid[aCurrentX,aCurrentY]);
                            Closed.Add(GameGrid[aCurrentX + 1,aCurrentY]);
                            TargetFound = true;

                            break;
                        }
                        Open.Add(GameGrid[aCurrentX + 1,aCurrentY]);
                    }
                    else if (aCurrentX + 1 < GridLength && aCurrentX + 1 > -1 && aCurrentY > -1 && aCurrentY < GridLength && !GameGrid[aCurrentX + 1,aCurrentY].Blocked && !GameGrid[aCurrentX + 1,aCurrentY].Closed && GameGrid[aCurrentX + 1,aCurrentY].Open)
                    {
                        if (GameGrid[aCurrentX,aCurrentY].C + 10 < GameGrid[aCurrentX + 1,aCurrentY].C)
                        {
                            GameGrid[aCurrentX + 1,aCurrentY].Parent = GameGrid[aCurrentX,aCurrentY];
                            GameGrid[aCurrentX + 1,aCurrentY].C = GameGrid[aCurrentX,aCurrentY].C + 10;
                            GameGrid[aCurrentX + 1,aCurrentY].T = GameGrid[aCurrentX + 1,aCurrentY].C + GameGrid[aCurrentX + 1,aCurrentY].H;
                            Open[Open.IndexOf(GameGrid[aCurrentX + 1,aCurrentY])] = GameGrid[aCurrentX + 1,aCurrentY];
                        }
                    }
                    
                    //top
                    if (aCurrentX - 1 > -1 && aCurrentX - 1 < GridLength && aCurrentY > -1 && aCurrentY < GridLength && !GameGrid[aCurrentX - 1,aCurrentY].Blocked && !GameGrid[aCurrentX - 1,aCurrentY].Closed && !GameGrid[aCurrentX - 1,aCurrentY].Open)
                    {
                        GameGrid[aCurrentX - 1,aCurrentY].Parent = GameGrid[aCurrentX,aCurrentY];
                        GameGrid[aCurrentX - 1,aCurrentY].C = GameGrid[aCurrentX,aCurrentY].C + 10;
                        GameGrid[aCurrentX - 1,aCurrentY].H = Math.Abs(TargetX - (aCurrentX - 1)) + Math.Abs(TargetY - (aCurrentY));
                        GameGrid[aCurrentX - 1,aCurrentY].T = GameGrid[aCurrentX - 1,aCurrentY].C + GameGrid[aCurrentX - 1,aCurrentY].H;
                        GameGrid[aCurrentX - 1,aCurrentY].Open = true;
                        if (GameGrid[aCurrentX - 1,aCurrentY] == End)
                        {
                            GameGrid[aCurrentX,aCurrentY].Closed = true;
                            Closed.Add(GameGrid[aCurrentX,aCurrentY]);
                            Closed.Add(GameGrid[aCurrentX - 1,aCurrentY]);
                            TargetFound = true;

                            break;
                        }
                        Open.Add(GameGrid[aCurrentX - 1,aCurrentY]);
                    }
                    else if (aCurrentX - 1 > -1 && aCurrentX - 1 < GridLength && aCurrentY > -1 && aCurrentY < GridLength && !GameGrid[aCurrentX - 1,aCurrentY].Blocked && !GameGrid[aCurrentX - 1,aCurrentY].Closed && GameGrid[aCurrentX - 1,aCurrentY].Open)
                    {
                        if (GameGrid[aCurrentX,aCurrentY].C + 10 < GameGrid[aCurrentX - 1,aCurrentY].C)
                        {
                            GameGrid[aCurrentX - 1,aCurrentY].Parent = GameGrid[aCurrentX,aCurrentY];
                            GameGrid[aCurrentX - 1,aCurrentY].C = GameGrid[aCurrentX,aCurrentY].C + 10;
                            GameGrid[aCurrentX - 1,aCurrentY].T = GameGrid[aCurrentX - 1,aCurrentY].C + GameGrid[aCurrentX - 1,aCurrentY].H;
                            Open[Open.IndexOf(GameGrid[aCurrentX - 1,aCurrentY])] = GameGrid[aCurrentX - 1,aCurrentY];
                        }
                    }
                    
                    //left
                    if (aCurrentX > -1 && aCurrentX < GridLength && aCurrentY - 1 > -1 && aCurrentY - 1 < GridLength && !GameGrid[aCurrentX,aCurrentY - 1].Blocked && !GameGrid[aCurrentX,aCurrentY - 1].Closed && !GameGrid[aCurrentX,aCurrentY - 1].Open)
                    {
                        GameGrid[aCurrentX,aCurrentY - 1].Parent = GameGrid[aCurrentX,aCurrentY];
                        GameGrid[aCurrentX,aCurrentY - 1].C = GameGrid[aCurrentX,aCurrentY].C + 10;
                        GameGrid[aCurrentX,aCurrentY - 1].H = Math.Abs(TargetX - (aCurrentX)) + Math.Abs(TargetY - (aCurrentY - 1));
                        GameGrid[aCurrentX,aCurrentY - 1].T = GameGrid[aCurrentX,aCurrentY - 1].C + GameGrid[aCurrentX,aCurrentY - 1].H;
                        GameGrid[aCurrentX,aCurrentY - 1].Open = true;
                        if (GameGrid[aCurrentX, aCurrentY - 1] == End)
                        {
                            GameGrid[aCurrentX,aCurrentY].Closed = true;
                            Closed.Add(GameGrid[aCurrentX,aCurrentY]);
                            Closed.Add(GameGrid[aCurrentX,aCurrentY - 1]);
                            TargetFound = true;

                            break;
                        }
                        Open.Add(GameGrid[aCurrentX,aCurrentY - 1]);
                    }
                    else if (aCurrentX > -1 && aCurrentX < GridLength && aCurrentY - 1 > -1 && aCurrentY - 1 < GridLength && !GameGrid[aCurrentX,aCurrentY - 1].Blocked && !GameGrid[aCurrentX,aCurrentY - 1].Closed && GameGrid[aCurrentX,aCurrentY - 1].Open)
                    {
                        if (GameGrid[aCurrentX,aCurrentY].C + 10 < GameGrid[aCurrentX,aCurrentY - 1].C)
                        {
                            GameGrid[aCurrentX,aCurrentY - 1].Parent = GameGrid[aCurrentX,aCurrentY];
                            GameGrid[aCurrentX,aCurrentY - 1].C = GameGrid[aCurrentX,aCurrentY].C + 10;
                            GameGrid[aCurrentX,aCurrentY - 1].T = GameGrid[aCurrentX,aCurrentY - 1].C + GameGrid[aCurrentX,aCurrentY - 1].H;
                            Open[Open.IndexOf(GameGrid[aCurrentX,aCurrentY - 1])] = GameGrid[aCurrentX,aCurrentY - 1];
                        }
                    }
                    //right
                    if (aCurrentX > -1 && aCurrentX < GridLength && aCurrentY + 1 < GridLength && aCurrentY + 1 > -1 && !GameGrid[aCurrentX,aCurrentY + 1].Blocked && !GameGrid[aCurrentX,aCurrentY + 1].Closed && !GameGrid[aCurrentX,aCurrentY + 1].Open)
                    {
                        GameGrid[aCurrentX,aCurrentY + 1].Parent = GameGrid[aCurrentX,aCurrentY];
                        GameGrid[aCurrentX,aCurrentY + 1].C = GameGrid[aCurrentX,aCurrentY].C + 10;
                        GameGrid[aCurrentX,aCurrentY + 1].H = Math.Abs(TargetX - (aCurrentX)) + Math.Abs(TargetY - (aCurrentY + 1));
                        GameGrid[aCurrentX,aCurrentY + 1].T = GameGrid[aCurrentX,aCurrentY + 1].C + GameGrid[aCurrentX,aCurrentY + 1].H;
                        GameGrid[aCurrentX,aCurrentY + 1].Open = true;
                        if (GameGrid[aCurrentX, aCurrentY + 1] == End)
                        {
                            GameGrid[aCurrentX,aCurrentY].Closed = true;
                            Closed.Add(GameGrid[aCurrentX,aCurrentY]);
                            Closed.Add(GameGrid[aCurrentX,aCurrentY + 1]);
                            TargetFound = true;

                            break;
                        }
                        Open.Add(GameGrid[aCurrentX,aCurrentY + 1]);
                    }
                    else if (aCurrentX > -1 && aCurrentX < GridLength && aCurrentY + 1 < GridLength && aCurrentY + 1 > -1 && !GameGrid[aCurrentX,aCurrentY + 1].Blocked && !GameGrid[aCurrentX,aCurrentY + 1].Closed && GameGrid[aCurrentX,aCurrentY + 1].Open)
                    {
                        if (GameGrid[aCurrentX,aCurrentY].C + 10 < GameGrid[aCurrentX,aCurrentY + 1].C)
                        {
                            GameGrid[aCurrentX,aCurrentY + 1].Parent = GameGrid[aCurrentX,aCurrentY];
                            GameGrid[aCurrentX,aCurrentY + 1].C = GameGrid[aCurrentX,aCurrentY].C + 10;
                            GameGrid[aCurrentX,aCurrentY + 1].T = GameGrid[aCurrentX,aCurrentY + 1].C + GameGrid[aCurrentX,aCurrentY + 1].H;
                            Open[Open.IndexOf(GameGrid[aCurrentX,aCurrentY + 1])] = GameGrid[aCurrentX,aCurrentY + 1];
                        }
                    }
                    GameGrid[aCurrentX,aCurrentY].Closed = true;

                    //move the selected node into Closed list after inspecting all its adjacent nodes
                    Closed.Add(GameGrid[aCurrentX,aCurrentY]);


                    if (Open.Count > 0)
                    {
                        //remove the current selected node from Open list
                        Open.RemoveAt(selectedIndex);
                    }

                }
            }

            //if target found traverse the shortest path through the close nodes
            //each node is linked to a parent, so the shortest path is the link of parents from the target node
            if (TargetFound)
            {
                int tempX = TargetX;
                int tempY = TargetY;
                int placeholderX = tempX, placeholderY = tempY;
                while (true)
                {
                    if (tempX == CurrentX && tempY == CurrentY)
                    {
                        shortestPath.Reverse();
                        for (int i = 0; i < shortestPath.Count; i++)
                        {
                            Console.WriteLine(shortestPath[i].CenterPoint.ToString());
                        }
                        break;
                    }
                    shortestPath.Add(GameGrid[tempX,tempY]);

                    placeholderX = tempX;
                    placeholderY = tempY;
                    tempX = (int)GameGrid[placeholderX, placeholderY].Parent.Position.X / TOTile.MAX_WIDTH;
                    tempY = (int)GameGrid[placeholderX, placeholderY].Parent.Position.Y / TOTile.MAX_WIDTH;

                }

            }

            return shortestPath;
        }

        public Boolean DoneSpawning
        {
            get { return pSpawnsLeft == 0; }
        }
    }
}
