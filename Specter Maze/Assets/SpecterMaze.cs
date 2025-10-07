using KModkit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Math = ExMath;
using Rnd = UnityEngine.Random;

public class SpecterMaze : MonoBehaviour
{

    public KMBombInfo Bomb;
    public KMAudio Audio;

    static int ModuleIdCounter = 1;
    int ModuleId;
    private bool ModuleSolved;

    public KMSelectable ForwardBtn;
    public KMSelectable LeftBtn;
    public KMSelectable RightBtn;

    public KMSelectable SwapBtn;
    public KMSelectable SubmitBtn;

    int[] Position = { 0, 0 };
    int Dir = 0; //ULDR 0123
    int SelectedMazeBody = 0;
    int SelectedMazeGhost = 0;
    bool BodyMode = true;

    public Material[] LettersMats;
    public GameObject ShownLetter;
    public GameObject FutureLetter;

    public GameObject bleObj, blwObj, breObj, brwObj, bwObj, fleObj, flwObj, freObj, frwObj, fwObj;
    bool bleVis, blwVis, breVis, brwVis, bwVis, fleVis, flwVis, freVis, frwVis, fwVis;

    string GoalColorBody = ""; //RYGB
    string GoalColorGhost = "";
    int GoalBody;
    int GoalSoul;

    bool BodyDeposited;
    bool SoulDeposited;

    public SpriteRenderer ShownLoot;
    public Sprite[] PossibleLoot;

    string[][][] Mazes = new string[][][] { //Indicates where you can go.
      new string[][] { //a
         new string[] { "RD", "RL", "L", "R", "RL", "DL" },
         new string[] { "UD", "R", "RDL", "L", "D", "UD" },
         new string[] { "U", "D", "UD", "R", "UDL", "U" },
         new string[] { "RD", "UL", "UD", "D", "UR", "L" },
         new string[] { "UD", "D", "U", "UR", "RL", "DL" },
         new string[] { "U", "UR", "RL", "RL", "L", "U" },
      },
         new string[][] { //b
         new string[] { "RD", "DL", "RD", "L", "RD", "L" },
         new string[] { "URD", "UL", "UD", "R", "UDL", "D" },
         new string[] { "U", "D", "UR", "L", "U", "UD" },
         new string[] { "D", "UR", "RDL", "RL", "L", "UD" },
         new string[] { "URD", "DL", "U", "D", "R", "UL" },
         new string[] { "UR", "UL", "R", "URL", "RL", "L" },
      },
         new string[][] { //c
         new string[] { "R", "DL", "RD", "RL", "RL", "L" },
         new string[] { "D", "UD", "U", "RD", "L", "D" },
         new string[] { "UD", "UR", "L", "UD", "RD", "UL" },
         new string[] { "UD", "D", "R", "UDL", "UD", "D" },
         new string[] { "UD", "URD", "DL", "U", "U", "UD" },
         new string[] { "U", "UR", "UL", "R", "RL", "UL" },
      },
         new string[][] { //d
         new string[] { "R", "DL", "R", "RL", "RL", "DL" },
         new string[] { "D", "UR", "DL", "RD", "L", "U" },
         new string[] { "URD", "L", "U", "UD", "R", "DL" },
         new string[] { "UD", "RD", "L", "UD", "D", "UD" },
         new string[] { "U", "UD", "R", "UL", "UD", "UD" },
         new string[] { "R", "UL", "R", "RL", "UL", "U" },
      },
         new string[][] { //e
         new string[] { "R", "DL", "R", "DL", "RD", "DL" },
         new string[] { "D", "URD", "L", "UD", "UR", "UDL" },
         new string[] { "UD", "U", "D", "UR", "L", "U" },
         new string[] { "UD", "R", "URL", "RDL", "L", "D" },
         new string[] { "UR", "L", "D", "U", "RD", "UDL" },
         new string[] { "R", "RL", "URL", "L", "UR", "UL" },
      },
         new string[][] { //f
         new string[] { "D", "R", "RL", "RL", "DL", "D" },
         new string[] { "URD", "RL", "L", "D", "U", "UD" },
         new string[] { "U", "R", "RDL", "UL", "R", "UDL" },
         new string[] { "RD", "DL", "UD", "D", "D", "U" },
         new string[] { "URD", "UL", "U", "UD", "URD", "DL" },
         new string[] { "U", "R", "RL", "UL", "UR", "UL" },
      },
         new string[][] { //g
         new string[] { "RD", "RL", "RL", "L", "RD", "L" },
         new string[] { "U", "RD", "L", "D", "UD", "D" },
         new string[] { "RD", "UL", "RD", "UL", "UD", "UD" },
         new string[] { "U", "R", "UDL", "D", "U", "UD" },
         new string[] { "RD", "DL", "U", "URD", "DL", "UD" },
         new string[] { "UR", "URL", "L", "UR", "UL", "U" },
      },
         new string[][] { //h
         new string[] { "R", "RL", "RL", "RL", "L", "D" },
         new string[] { "RD", "RL", "DL", "R", "RL", "UDL" },
         new string[] { "U", "D", "U", "R", "DL", "U" },
         new string[] { "D", "UR", "RL", "DL", "UR", "DL" },
         new string[] { "URD", "RL", "L", "UR", "L", "U" },
         new string[] { "U", "R", "RL", "RL", "RL", "L" },
      },
         new string[][] { //i
         new string[] { "D", "R", "RL", "RL", "RL", "L" },
         new string[] { "UD", "D", "R", "RL", "RL", "DL" },
         new string[] { "UD", "UR", "DL", "R", "DL", "U" },
         new string[] { "UD", "D", "UR", "DL", "URD", "L" },
         new string[] { "U", "URD", "L", "U", "U", "D" },
         new string[] { "R", "UL", "R", "RL", "RL", "UL" },
      },
   };

    string[] MazesForWalls = new string[] {
        "+-+-+-+-+-+-+"+
        "|. . .|. . .|"+
        "| +-+-+-+-+ |"+
        "|.|. . .|.|.|"+
        "| +-+ +-+ + |"+
        "|.|.|.|. .|.|"+
        "|-+ + +-+ +-|"+
        "|. .|.|.|. .|"+
        "| +-+ + +-+-|"+
        "|.|.|.|. . .|"+
        "| + +-+-+-+ |"+
        "|.|. . . .|.|"+
        "+-+-+-+-+-+-+",

      // 0123456789012
        "+-+-+-+-+-+-+"+//0
        "|. .|. .|. .|"+//1
        "| + + +-+ +-|"+//2
        "|. .|.|. .|.|"+//3
        "| +-+ +-+ + |"+//4
        "|.|.|. .|.|.|"+//5
        "|-+ +-+-+-+ |"+//6
        "|.|. . . .|.|"+//7
        "| +-+ +-+-+ |"+//8
        "|. .|.|.|. .|"+//9
        "| + +-+ +-+-|"+//10
        "|. .|. . . .|"+//11
        "+-+-+-+-+-+-+",//12

        "+-+-+-+-+-+-+"+
        "|. .|. . . .|"+
        "|-+ + +-+-+-|"+
        "|.|.|.|. .|.|"+
        "| + +-+ +-+ |"+
        "|.|. .|.|. .|"+
        "| +-+-+ + +-|"+
        "|.|.|. .|.|.|"+
        "| + +-+ + + |"+
        "|.|. .|.|.|.|"+
        "| + + +-+-+ |"+
        "|.|. .|. . .|"+
        "+-+-+-+-+-+-+",

        "+-+-+-+-+-+-+"+
        "|. .|. . . .|"+
        "|-+ +-+-+-+ |"+
        "|.|. .|. .|.|"+
        "| +-+ + +-+-|"+
        "|. .|.|.|. .|"+
        "| +-+-+ +-+ |"+
        "|.|. .|.|.|.|"+
        "| + +-+ + + |"+
        "|.|.|. .|.|.|"+
        "|-+ +-+-+ + |"+
        "|. .|. . .|.|"+
        "+-+-+-+-+-+-+",

        "+-+-+-+-+-+-+"+
        "|. .|. .|. .|"+
        "|-+ +-+ + + |"+
        "|.|. .|.|. .|"+
        "| + +-+ +-+ |"+
        "|.|.|.|. .|.|"+
        "| +-+ +-+-+-|"+
        "|.|. . . .|.|"+
        "| +-+-+ +-+ |"+
        "|. .|.|.|. .|"+
        "|-+-+ +-+ + |"+
        "|. . . .|. .|"+
        "+-+-+-+-+-+-+",

        "+-+-+-+-+-+-+"+
        "|.|. . . .|.|"+
        "| +-+-+-+ + |"+
        "|. . .|.|.|.|"+
        "| +-+-+ +-+ |"+
        "|.|. . .|. .|"+
        "|-+-+ +-+-+ |"+
        "|. .|.|.|.|.|"+
        "| + + + + +-|"+
        "|. .|.|.|. .|"+
        "| +-+-+ + + |"+
        "|.|. . .|. .|"+
        "+-+-+-+-+-+-+",

        "+-+-+-+-+-+-+"+
        "|. . . .|. .|"+
        "| +-+-+-+ +-|"+
        "|.|. .|.|.|.|"+
        "|-+ +-+ + + |"+
        "|. .|. .|.|.|"+
        "| +-+ +-+ + |"+
        "|.|. .|.|.|.|"+
        "|-+-+ + +-+ |"+
        "|. .|.|. .|.|"+
        "| + +-+ + + |"+
        "|. . .|. .|.|"+
        "+-+-+-+-+-+-+",

        "+-+-+-+-+-+-+"+
        "|. . . . .|.|"+
        "|-+-+-+-+-+ |"+
        "|. . .|. . .|"+
        "| +-+ +-+-+ |"+
        "|.|.|.|. .|.|"+
        "|-+ +-+-+ +-|"+
        "|.|. . .|. .|"+
        "| +-+-+ +-+ |"+
        "|. . .|. .|.|"+
        "| +-+-+-+-+-|"+
        "|.|. . . . .|"+
        "+-+-+-+-+-+-+",

        "+-+-+-+-+-+-+"+
        "|.|. . . . .|"+
        "| +-+-+-+-+-|"+
        "|.|.|. . . .|"+
        "| + +-+-+-+ |"+
        "|.|. .|. .|.|"+
        "| +-+ +-+ +-|"+
        "|.|.|. .|. .|"+
        "| + +-+ + +-|"+
        "|.|. .|.|.|.|"+
        "|-+ +-+-+-+ |"+
        "|. .|. . . .|"+
        "+-+-+-+-+-+-+"
   };

    string[] Indicators = {
      "ZCEVVC" +
      "EZEZVC" +
      "CEZVZV" +
      "CEVCZE" +
      "EZCVCV" +
      "EZZEVC",

      "VZECEC" +
      "VZZECV" +
      "CVZEVZ" +
      "CEECZV" +
      "ZVECCE" +
      "VZEVCZ",

      "CZEVZC" +
      "VEVEZC" +
      "EVZCZC" +
      "VEVECZ" +
      "CZEVEC" +
      "VZZVEC",

      "CEZVVZ" +
      "CEECZV" +
      "ZVECCE" +
      "VZVZCE" +
      "EZVCCV" +
      "ZEVCEZ",

      "ZECVEZ" +
      "CVCVZE" +
      "VCEZZE" +
      "VCCVEZ" +
      "ZECVEZ" +
      "VCVCZE",

      "CEVZVZ" +
      "ECECZV" +
      "ZVCECE" +
      "ZVVZEC" +
      "ECVZZV" +
      "CECZVE",

      "VECZZV" +
      "ECECZV" +
      "CZEVVE" +
      "ZCECVZ" +
      "ZVCEZV" +
      "ECECZV",

      "CEVZVZ" +
      "CEZVCE" +
      "ECZVCE" +
      "VZVZEC" +
      "ZEVCCV" +
      "EZVCZE",

      "EZCVZE" +
      "CVCVEZ" +
      "EZVCVC" +
      "ZEZCVE" +
      "EVCZCE" +
      "ZVVZEC"
   };

    int[] LetterVisibilitiesBody = Enumerable.Range(0, 36).ToArray();
    int[] LetterVisibilitiesGhost = Enumerable.Range(0, 36).ToArray();

    string[] Goals = new string[] {
      "B.....R.G.................Y.........",
      "................RG.Y....B...........",
      ".....R.............BG..........Y....",
      ".....B.Y.....G...............R......",
      ".....Y...B........R......G..........",
      "..............R...G..............BY.",
      "...G.....Y......R...........B.......",
      "....................GR.........Y..B.",
      "...R.......B....G.......Y...........",
   };

    void Awake()
    { //Avoid doing calculations in here regarding edgework. Just use this for setting up buttons for simplicity.
        ModuleId = ModuleIdCounter++;
        GetComponent<KMBombModule>().OnActivate += Activate;
        /*
        foreach (KMSelectable object in keypad) {
            object.OnInteract += delegate () { keypadPress(object); return false; };
        }
        */

        ForwardBtn.OnInteract += delegate () { Move(); return false; };
        LeftBtn.OnInteract += delegate () { TurnLeft(); return false; };
        RightBtn.OnInteract += delegate () { TurnRight(); return false; };
        SwapBtn.OnInteract += delegate () { Swap(); return false; };
        SubmitBtn.OnInteract += delegate () { SubmitModule(); return false; };
    }

    void TurnRight()
    {
        if (ModuleSolved)
        {
            return;
        }
        Dir = Dir - 1 == -1 ? 3 : Dir - 1;
        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, this.transform);
        ToggleWalls();
        AdjustLetter();
    }

    void TurnLeft()
    {
        if (ModuleSolved)
        {
            return;
        }
        Dir = (Dir + 1) % 4;
        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, this.transform);
        ToggleWalls();
        AdjustLetter();
    }

    void Swap()
    {
        if (ModuleSolved)
        {
            return;
        }
        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, this.transform);
        BodyMode ^= true;
        if (BodyMode && !BodyDeposited)
        {
            ShownLoot.sprite = PossibleLoot[GoalBody];
        }
        else if (!BodyMode && !SoulDeposited)
        {
            ShownLoot.sprite = PossibleLoot[GoalSoul];
        }
        else
        {
            ShownLoot.sprite = null;
        }
        ToggleWalls();
        AdjustLetter();
    }

    void SubmitModule()
    {
        if (ModuleSolved)
        {
            return;
        }
        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, this.transform);
        int index = Position[0] * 6 + Position[1];
        if (BodyMode)
        {
            if (Goals[SelectedMazeBody][index].ToString() == GoalColorBody)
            {
                BodyDeposited = true;
                ShownLoot.sprite = null;
            }
            else
            {
                Strike();
            }
        }
        else
        {
            if (Goals[SelectedMazeGhost][index].ToString() == GoalColorGhost)
            {
                SoulDeposited = true;
                ShownLoot.sprite = null;
            }
            else
            {
                Strike();
            }
        }

        if (BodyDeposited && SoulDeposited)
        {
            Solve();
        }
    }


    void Move()
    {
        if (ModuleSolved)
        {
            return;
        }
        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, this.transform);
        switch (Dir)
        {
            case 0:
                if (Mazes[BodyMode ? SelectedMazeBody : SelectedMazeGhost][Position[0]][Position[1]].Contains("U"))
                {
                    Position[0]--;
                }
                else
                {
                    Strike();
                }
                break;
            case 1:
                if (Mazes[BodyMode ? SelectedMazeBody : SelectedMazeGhost][Position[0]][Position[1]].Contains("L"))
                {
                    Position[1]--;
                }
                else
                {
                    Strike();
                }
                break;
            case 2:
                if (Mazes[BodyMode ? SelectedMazeBody : SelectedMazeGhost][Position[0]][Position[1]].Contains("D"))
                {
                    Position[0]++;
                }
                else
                {
                    Strike();
                }
                break;
            case 3:
                if (Mazes[BodyMode ? SelectedMazeBody : SelectedMazeGhost][Position[0]][Position[1]].Contains("R"))
                {
                    Position[1]++;
                }
                else
                {
                    Strike();
                }
                break;
            default:
                break;
        }
        ToggleWalls();
        AdjustLetter();
    }

    void OnDestroy()
    { //Shit you need to do when the bomb ends

    }

    void Activate()
    { //Shit that should happen when the bomb arrives (factory)/Lights turn on

    }

    void Start()
    { //Shit that you calculate, usually a majority if not all of the module
        Position = new int[] { Rnd.Range(0, 6), Rnd.Range(0, 6) };
        SelectedMazeBody = Rnd.Range(0, 9);
        do
        {
            SelectedMazeGhost = Rnd.Range(0, 9);
        } while (SelectedMazeBody == SelectedMazeGhost);

        LetterVisibilitiesBody.Shuffle();
        LetterVisibilitiesGhost.Shuffle();

        Dir = Rnd.Range(0, 4);
        GoalBody = Rnd.Range(0, 4);
        GoalSoul = Rnd.Range(0, 4);
        GoalColorBody = "RYGB"[GoalBody].ToString();
        GoalColorGhost = "RYGB"[GoalSoul].ToString();
        ShownLoot.sprite = PossibleLoot[GoalBody];
        Debug.LogFormat("[Specter Maze #{0}] The body's maze is {1}st/nd/rd/th and the body's goal color is {2}.", ModuleId, SelectedMazeBody + 1, GoalColorBody);
        Debug.LogFormat("[Specter Maze #{0}] The soul's maze is {1}st/nd/rd/th and the soul's goal color is {2}.", ModuleId, SelectedMazeGhost + 1, GoalColorGhost);
        Debug.LogFormat("[Specter Maze #{0}] Starting position is ({1},{2}), where the first coordinate indicates the row (0-indexed) starting from the top.", ModuleId, Position[0], Position[1]);
        ToggleWalls();
        AdjustLetter();
    }

    void AdjustLetter()
    {

        int index = Position[0] * 6 + Position[1];
        string obtainedLetter = Indicators[BodyMode ? SelectedMazeBody : SelectedMazeGhost][index].ToString();
        int matsIndex = 0;
        switch (obtainedLetter)
        {
            case "C":
                matsIndex = 0;
                break;
            case "E":
                matsIndex = 1;
                break;
            case "V":
                matsIndex = 2;
                break;
            case "Z":
                matsIndex = 3;
                break;
            default: //Force a break;
                matsIndex = 4;
                break;
        }

        ShownLetter.GetComponent<MeshRenderer>().material = LettersMats[matsIndex];
        bool WillShowLetter = false;
        if (BodyMode)
        {
            for (int i = 0; i < 12; i++)
            {
                if (index == LetterVisibilitiesBody[i])
                {
                    WillShowLetter = true;
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < 12; i++)
            {
                if (index == LetterVisibilitiesGhost[i])
                {
                    WillShowLetter = true;
                    break;
                }
            }
        }

        if (WillShowLetter)
        {
            ShownLetter.SetActive(true);
        }
        else
        {
            ShownLetter.SetActive(false);
        }

        bool ShowFutureLetter = false;

        int offset = 0;

        switch (Dir)
        {
            case 0:
                offset = -6;
                break;
            case 1:
                offset = -1;
                break;
            case 2:
                offset = 6;
                break;
            case 3:
                offset = 1;
                break;
            default:
                break;
        }

        if (BodyMode)
        {
            for (int i = 0; i < 12; i++)
            {
                if (LetterVisibilitiesBody[i] == index + offset)
                {
                    ShowFutureLetter = true;
                }
            }
        }
        else
        {
            for (int i = 0; i < 12; i++)
            {
                if (LetterVisibilitiesGhost[i] == index + offset)
                {
                    ShowFutureLetter = true;
                }
            }
        }

        if (ShowFutureLetter)
        {
            FutureLetter.SetActive(true);
        }
        else
        {
            FutureLetter.SetActive(false);
        }
    }

    void ToggleWalls()
    {
        /*
      // 0123456789012
        "+-+-+-+-+-+-+" +//0
        "|. .|. .|. .|" +//1
        "| + + +-+ +-|" +//2
        "|. .|.|. .|.|" +//3
        "| +-+ +-+ + |" +//4
        "|.|.|. .|.|.|" +//5
        "|-+ +-+-+-+ |" +//6
        "|.|. . . .|.|" +//7
        "| +-+ +-+-+ |" +//8
        "|. .|.|.|. .|" +//9
        "| + +-+ +-+-|" +//10
        "|. .|. . . .|" +//11
        "+-+-+-+-+-+-+",//12
        */
        string NEWS = "ULDR";
        int[] RowColIndices = { 1, 3, 5, 7, 9, 11 };
        int curPos = RowColIndices[Position[0]] * 13 + RowColIndices[Position[1]];


        switch (Dir)
        { //ULDR 0123
            case 0:



                if (Position[0] == 0 || MazesForWalls[BodyMode ? SelectedMazeBody : SelectedMazeGhost][curPos - 27].ToString() != " ")
                {
                    bleVis = true;
                }
                else
                {
                    bleVis = false;
                }

                if (Position[0] == 0 || MazesForWalls[BodyMode ? SelectedMazeBody : SelectedMazeGhost][curPos - 25].ToString() != " ")
                {
                    breVis = true;
                }
                else
                {
                    breVis = false;
                }

                if (Position[0] == 0 || MazesForWalls[BodyMode ? SelectedMazeBody : SelectedMazeGhost][curPos - 39].ToString() != " ")
                {
                    bwVis = true;
                }
                else
                {
                    bwVis = false;
                }

                if (Position[0] == 0 || Position[0] == 1 || MazesForWalls[BodyMode ? SelectedMazeBody : SelectedMazeGhost][curPos - 41].ToString() != " ")
                {
                    blwVis = true;
                }
                else
                {
                    blwVis = false;
                }

                if (Position[0] == 0 || MazesForWalls[BodyMode ? SelectedMazeBody : SelectedMazeGhost][curPos - 37].ToString() != " ")
                {
                    brwVis = true;
                }
                else
                {
                    brwVis = false;
                }

                if (MazesForWalls[BodyMode ? SelectedMazeBody : SelectedMazeGhost][curPos + 1].ToString() != " ")
                {
                    freVis = true;
                }
                else
                {
                    freVis = false;
                }

                if (MazesForWalls[BodyMode ? SelectedMazeBody : SelectedMazeGhost][curPos - 1].ToString() != " ")
                {
                    fleVis = true;
                }
                else
                {
                    fleVis = false;
                }

                if (Position[0] == 0 || MazesForWalls[BodyMode ? SelectedMazeBody : SelectedMazeGhost][curPos - 15].ToString() != " ")
                {
                    flwVis = true;
                }
                else
                {
                    flwVis = false;
                }

                if (MazesForWalls[BodyMode ? SelectedMazeBody : SelectedMazeGhost][curPos - 11].ToString() != " ")
                {
                    frwVis = true;
                }
                else
                {
                    frwVis = false;
                }

                if (MazesForWalls[BodyMode ? SelectedMazeBody : SelectedMazeGhost][curPos - 13].ToString() != " ")
                {
                    fwVis = true;
                }
                else
                {
                    fwVis = false;
                }

                break;
            case 1:
                /*
        // 0123456789012
          "+-+-+-+-+-+-+" +//0
          "|. .|. .|. .|" +//1
          "| + + +-+ +-|" +//2
          "|. .|.|. .|.|" +//3
          "| +-+ +-+ + |" +//4
          "|.|.|. .|.|.|" +//5
          "|-+ +-+-+-+ |" +//6
          "|.|. . . .|.|" +//7
          "| +-+ +-+-+ |" +//8
          "|. .|.|.|. .|" +//9
          "| + +-+ +-+-|" +//10
          "|. .|. . . .|" +//11
          "+-+-+-+-+-+-+",//12
          */

                if (MazesForWalls[BodyMode ? SelectedMazeBody : SelectedMazeGhost][curPos + 11].ToString() != " ")
                {
                    bleVis = true;
                }
                else
                {
                    bleVis = false;
                }

                if (Position[0] == 0 || MazesForWalls[BodyMode ? SelectedMazeBody : SelectedMazeGhost][curPos - 15].ToString() != " ")
                {
                    breVis = true;
                }
                else
                {
                    breVis = false;
                }

                if (MazesForWalls[BodyMode ? SelectedMazeBody : SelectedMazeGhost][curPos - 3].ToString() != " ")
                {
                    bwVis = true;
                }
                else
                {
                    bwVis = false;
                }

                if (Position[0] == 5 || MazesForWalls[BodyMode ? SelectedMazeBody : SelectedMazeGhost][curPos + 23].ToString() != " ")
                {
                    blwVis = true;
                }
                else
                {
                    blwVis = false;
                }

                if (Position[0] == 0 || MazesForWalls[BodyMode ? SelectedMazeBody : SelectedMazeGhost][curPos - 29].ToString() != " ")
                {
                    brwVis = true;
                }
                else
                {
                    brwVis = false;
                }

                if (MazesForWalls[BodyMode ? SelectedMazeBody : SelectedMazeGhost][curPos - 13].ToString() != " ")
                {
                    freVis = true;
                }
                else
                {
                    freVis = false;
                }

                if (MazesForWalls[BodyMode ? SelectedMazeBody : SelectedMazeGhost][curPos + 13].ToString() != " ")
                {
                    fleVis = true;
                }
                else
                {
                    fleVis = false;
                }

                if (Position[0] == 0 || MazesForWalls[BodyMode ? SelectedMazeBody : SelectedMazeGhost][curPos - 27].ToString() != " ")
                {
                    frwVis = true;
                }
                else
                {
                    frwVis = false;
                }

                if (Position[0] == 5 || MazesForWalls[BodyMode ? SelectedMazeBody : SelectedMazeGhost][curPos + 25].ToString() != " ")
                {
                    flwVis = true;
                }
                else
                {
                    flwVis = false;
                }

                if (MazesForWalls[BodyMode ? SelectedMazeBody : SelectedMazeGhost][curPos - 1].ToString() != " ")
                {
                    fwVis = true;
                }
                else
                {
                    fwVis = false;
                }

                break;
            case 2:
                /*
        // 0123456789012
          "+-+-+-+-+-+-+" +//0
          "|. .|. .|. .|" +//1
          "| + + +-+ +-|" +//2
          "|. .|.|. .|.|" +//3
          "| +-+ +-+ + |" +//4
          "|.|.|. .|.|.|" +//5
          "|-+ +-+-+-+ |" +//6
          "|.|. . . .|.|" +//7
          "| +-+ +-+-+ |" +//8
          "|. .|.|.|. .|" +//9
          "| + +-+ +-+-|" +//10
          "|. .|. . . .|" +//11
          "+-+-+-+-+-+-+",//12
          */

                if (Position[0] == 5 || MazesForWalls[BodyMode ? SelectedMazeBody : SelectedMazeGhost][curPos + 27].ToString() != " ")
                {
                    bleVis = true;
                }
                else
                {
                    bleVis = false;
                }

                if (Position[0] == 5 || MazesForWalls[BodyMode ? SelectedMazeBody : SelectedMazeGhost][curPos + 25].ToString() != " ")
                {
                    breVis = true;
                }
                else
                {
                    breVis = false;
                }

                if (Position[0] == 5 || MazesForWalls[BodyMode ? SelectedMazeBody : SelectedMazeGhost][curPos + 39].ToString() != " ")
                {
                    bwVis = true;
                }
                else
                {
                    bwVis = false;
                }

                if (Position[0] == 5 || Position[0] == 4 || MazesForWalls[BodyMode ? SelectedMazeBody : SelectedMazeGhost][curPos + 41].ToString() != " ")
                {
                    blwVis = true;
                }
                else
                {
                    blwVis = false;
                }

                if (Position[0] == 5 || MazesForWalls[BodyMode ? SelectedMazeBody : SelectedMazeGhost][curPos + 37].ToString() != " ")
                {
                    brwVis = true;
                }
                else
                {
                    brwVis = false;
                }

                if (Position[0] == 5 || MazesForWalls[BodyMode ? SelectedMazeBody : SelectedMazeGhost][curPos + 11].ToString() != " ")
                {
                    frwVis = true;
                }
                else
                {
                    frwVis = false;
                }

                if (Position[0] == 5 || MazesForWalls[BodyMode ? SelectedMazeBody : SelectedMazeGhost][curPos + 15].ToString() != " ")
                {
                    flwVis = true;
                }
                else
                {
                    flwVis = false;
                }

                if (MazesForWalls[BodyMode ? SelectedMazeBody : SelectedMazeGhost][curPos + 1].ToString() != " ")
                {
                    fleVis = true;
                }
                else
                {
                    fleVis = false;
                }

                if (MazesForWalls[BodyMode ? SelectedMazeBody : SelectedMazeGhost][curPos - 1].ToString() != " ")
                {
                    freVis = true;
                }
                else
                {
                    freVis = false;
                }

                if (MazesForWalls[BodyMode ? SelectedMazeBody : SelectedMazeGhost][curPos + 13].ToString() != " ")
                {
                    fwVis = true;
                }
                else
                {
                    fwVis = false;
                }

                break;
            case 3:
                /*
        // 0123456789012
          "+-+-+-+-+-+-+" +//0
          "|. .|. .|. .|" +//1
          "| + + +-+ +-|" +//2
          "|. .|.|. .|.|" +//3
          "| +-+ +-+ + |" +//4
          "|.|.|. .|.|.|" +//5
          "|-+ +-+-+-+ |" +//6
          "|.|. . . .|.|" +//7
          "| +-+ +-+-+ |" +//8
          "|. .|.|.|. .|" +//9
          "| + +-+ +-+-|" +//10
          "|. .|. . . .|" +//11
          "+-+-+-+-+-+-+",//12
          */

                if (MazesForWalls[BodyMode ? SelectedMazeBody : SelectedMazeGhost][curPos - 11].ToString() != " ")
                {
                    bleVis = true;
                }
                else
                {
                    bleVis = false;
                }

                if (Position[0] == 5 || MazesForWalls[BodyMode ? SelectedMazeBody : SelectedMazeGhost][curPos + 15].ToString() != " ")
                {
                    breVis = true;
                }
                else
                {
                    breVis = false;
                }

                if (MazesForWalls[BodyMode ? SelectedMazeBody : SelectedMazeGhost][curPos + 3].ToString() != " ")
                {
                    bwVis = true;
                }
                else
                {
                    bwVis = false;
                }

                if (Position[0] == 0 || MazesForWalls[BodyMode ? SelectedMazeBody : SelectedMazeGhost][curPos - 23].ToString() != " ")
                {
                    blwVis = true;
                }
                else
                {
                    blwVis = false;
                }

                if (Position[0] == 5 || MazesForWalls[BodyMode ? SelectedMazeBody : SelectedMazeGhost][curPos + 29].ToString() != " ")
                {
                    brwVis = true;
                }
                else
                {
                    brwVis = false;
                }

                if (MazesForWalls[BodyMode ? SelectedMazeBody : SelectedMazeGhost][curPos + 13].ToString() != " ")
                {
                    freVis = true;
                }
                else
                {
                    freVis = false;
                }

                if (MazesForWalls[BodyMode ? SelectedMazeBody : SelectedMazeGhost][curPos - 13].ToString() != " ")
                {
                    fleVis = true;
                }
                else
                {
                    fleVis = false;
                }

                if (Position[0] == 0 || MazesForWalls[BodyMode ? SelectedMazeBody : SelectedMazeGhost][curPos - 25].ToString() != " ")
                {
                    flwVis = true;
                }
                else
                {
                    flwVis = false;
                }

                if (Position[0] == 5 || MazesForWalls[BodyMode ? SelectedMazeBody : SelectedMazeGhost][curPos + 27].ToString() != " ")
                {
                    frwVis = true;
                }
                else
                {
                    frwVis = false;
                }

                if (MazesForWalls[BodyMode ? SelectedMazeBody : SelectedMazeGhost][curPos + 1].ToString() != " ")
                {
                    fwVis = true;
                }
                else
                {
                    fwVis = false;
                }

                break;
            default:
                break;
        }
        bleObj.SetActive(bleVis);
        blwObj.SetActive(blwVis);
        breObj.SetActive(breVis);
        brwObj.SetActive(brwVis);
        bwObj.SetActive(bwVis);

        fleObj.SetActive(fleVis);
        flwObj.SetActive(flwVis);
        freObj.SetActive(freVis);
        frwObj.SetActive(frwVis);
        fwObj.SetActive(fwVis);
    }

    void Update()
    { //Shit that happens at any point after initialization

    }

    void Solve()
    {
        ModuleSolved = true;
        ShownLetter.SetActive(false);
        FutureLetter.SetActive(false);
        bleObj.SetActive(false);
        blwObj.SetActive(false);
        breObj.SetActive(false);
        brwObj.SetActive(false);
        bwObj.SetActive(false);

        fleObj.SetActive(false);
        flwObj.SetActive(false);
        freObj.SetActive(false);
        frwObj.SetActive(false);
        fwObj.SetActive(false);
        GetComponent<KMBombModule>().HandlePass();
    }

    void Strike()
    {
        GetComponent<KMBombModule>().HandleStrike();
    }

#pragma warning disable 414
    private readonly string TwitchHelpMessage = @"!{0} move L F R F U [moves that many directions] [L = left, R = right, F = forward, U = u-turn] || !{0} submit [submits the position you are in] || !{0} swap [swaps between the body and the soul maze].";
#pragma warning restore 414

    private struct QueueInfo
    {
        public int[] CurrentCell { get; private set; }
        public int[] ParentCell { get; private set; }
        public int? Direction { get; private set; }
        public bool IsBody { get; private set; }

        public QueueInfo(int[] currentCell, int[] parentCell, int? direction, bool isBody)
        {
            CurrentCell = currentCell;
            ParentCell = parentCell;
            Direction = direction;
            IsBody = isBody;
        }
    }

    IEnumerator ProcessTwitchCommand(string command)
    {
        var split = command.ToUpperInvariant().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();

        switch (split[0])
        {
            case "MOVE":
            case "M":
                split.RemoveAt(0);
                break;
            case "SUBMIT":
            case "S":
                if (split.Count > 1)
                {
                    yield return "sendtochaterror Too many parameters!";
                    yield break;
                }

                yield return null;
                SubmitBtn.OnInteract();
                yield return new WaitForSeconds(0.1f);
                yield break;
            case "SWAP":
            case "W":
                if (split.Count > 1)
                {
                    yield return "sendtochaterror Too many parameters!";
                    yield break;
                }

                yield return null;
                SwapBtn.OnInteract();
                yield return new WaitForSeconds(0.1f);
                yield break;
            default:
                yield return "sendtochaterror valid commands are m = \'Move\', s = \'Submit\', w = \'Swap\'. Valid movements are l = \'Left\', r = \'Right\', f = \'Forward\', or u = \'U-Turn\'";
                yield break;
        }

        var moves = split.SelectMany(ShortenDirection).ToList();

        if (!moves.Any())
        {
            yield return "sendtochaterror There are no moves found. Please add some moves!";
            yield break;
        }

        if (moves.Any(x => x == null))
        {
            var invalidMove = split.FirstOrDefault(x => ShortenDirection(x)[0] == null);

            if (!string.IsNullOrEmpty(invalidMove))
                yield return string.Format("sendtochaterror I don't know how to move in the direction of {0}.", invalidMove);

            yield break;
        }

        yield return null;

        foreach (var move in moves)
        {
            move.OnInteract();
            yield return "trycancel";
            yield return new WaitForSeconds(0.1f);
        }
    }

    private KMSelectable[] ShortenDirection(string dir)
    {
        switch (dir)
        {
            case "L":
            case "LEFT":
                return new[] { LeftBtn };
            case "R":
            case "RIGHT":
                return new[] { RightBtn };
            case "F":
            case "FORWARD":
                return new[] { ForwardBtn };
            case "U":
            case "U-TURN":
            case "UTURN":
            case "TURNAROUND":
            case "TURN-AROUND":
                return Enumerable.Repeat(RightBtn, 2).ToArray();
            case "SUBMIT":
            case "S":
                return new[] { SubmitBtn };
            case "SWAP":
            case "W":
                return new[] { SwapBtn };
            default:
                return new KMSelectable[] { null };
        }
    }

    IEnumerator TwitchHandleForcedSolve()
    {

        for (int goal = 0; goal < 2; goal++)
        {
            var q = new Queue<QueueInfo>();

            var visited = new Dictionary<int[], QueueInfo>();

            int[] goalPosition = null;

            q.Enqueue(new QueueInfo(Position.ToArray(), null, null, true));

            while (q.Count > 0)
            {
                var qi = q.Dequeue();

                if (visited.ContainsKey(qi.CurrentCell))
                    continue;

                visited[qi.CurrentCell] = qi;

                bool[] conditions = new bool[]
                {
                    Goals[SelectedMazeBody][qi.CurrentCell[0] * 6 + qi.CurrentCell[1]].ToString() == GoalColorBody,
                    Goals[SelectedMazeGhost][qi.CurrentCell[0] * 6 + qi.CurrentCell[1]].ToString() == GoalColorGhost
                };

                if (conditions[goal])
                {
                    goalPosition = qi.CurrentCell;
                    break;    
                }

                var dirConditions = new[]
                {
                Mazes[SelectedMazeBody][qi.CurrentCell[0]][qi.CurrentCell[1]].Contains("U"),
                Mazes[SelectedMazeBody][qi.CurrentCell[0]][qi.CurrentCell[1]].Contains("L"),
                Mazes[SelectedMazeBody][qi.CurrentCell[0]][qi.CurrentCell[1]].Contains("D"),
                Mazes[SelectedMazeBody][qi.CurrentCell[0]][qi.CurrentCell[1]].Contains("R"),
                Mazes[SelectedMazeGhost][qi.CurrentCell[0]][qi.CurrentCell[1]].Contains("U"),
                Mazes[SelectedMazeGhost][qi.CurrentCell[0]][qi.CurrentCell[1]].Contains("L"),
                Mazes[SelectedMazeGhost][qi.CurrentCell[0]][qi.CurrentCell[1]].Contains("D"),
                Mazes[SelectedMazeGhost][qi.CurrentCell[0]][qi.CurrentCell[1]].Contains("R"),
            };

                var validDirOffsets = new[]
                {
                new[] { qi.CurrentCell[0] - 1, qi.CurrentCell[1] },
                new[] { qi.CurrentCell[0], qi.CurrentCell[1] - 1 },
                new[] { qi.CurrentCell[0] + 1, qi.CurrentCell[1] },
                new[] { qi.CurrentCell[0], qi.CurrentCell[1] + 1 }
            };

                for (int i = 0; i < 8; i++)
                    if (dirConditions[i])
                        q.Enqueue(new QueueInfo(validDirOffsets[i % 4], qi.CurrentCell, i % 4, i < 4));
            }

            var r = goalPosition;
            var path = new List<QueueInfo>();

            while (true)
            {
                var nr = visited[r];

                if (nr.ParentCell == null)
                    break;

                path.Add(nr);
                r = nr.ParentCell;
            }

            for (int move = path.Count - 1; move >= 0; move--)
            {
                if (path[move].IsBody ^ BodyMode)
                {
                    SwapBtn.OnInteract();
                    yield return new WaitForSeconds(0.1f);
                }

                if (Dir == (path[move].Direction.Value + 1) % 4)
                {
                    RightBtn.OnInteract();
                    yield return new WaitForSeconds(0.1f);
                }

                else if (Dir == (path[move].Direction.Value + 2) % 4)
                {
                    RightBtn.OnInteract();
                    yield return new WaitForSeconds(0.1f);
                    RightBtn.OnInteract();
                    yield return new WaitForSeconds(0.1f);
                }

                else if (Dir == (path[move].Direction.Value + 3) % 4)
                {
                    LeftBtn.OnInteract();
                    yield return new WaitForSeconds(0.1f);
                }

                ForwardBtn.OnInteract();
                yield return new WaitForSeconds(0.1f);
            }

            if (goal == 0 ^ BodyMode)
            {
                SwapBtn.OnInteract();
                yield return new WaitForSeconds(0.1f);
            }

            SubmitBtn.OnInteract();
            yield return new WaitForSeconds(0.1f);
        }
    }
}
