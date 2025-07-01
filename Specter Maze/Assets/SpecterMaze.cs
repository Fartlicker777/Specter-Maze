using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;
using Rnd = UnityEngine.Random;
using Math = ExMath;

public class SpecterMaze : MonoBehaviour {

   public KMBombInfo Bomb;
   public KMAudio Audio;

   static int ModuleIdCounter = 1;
   int ModuleId;
   private bool ModuleSolved;

   public KMSelectable ForwardBtn;
   public KMSelectable LeftBtn;
   public KMSelectable RightBtn;

   int[] Position = { 0, 0};
   int Dir = 0; //ULDR 0123

   public GameObject bleObj, blwObj, breObj, brwObj, bwObj, fleObj, flwObj, freObj, frwObj, fwObj;
   bool bleVis, blwVis, breVis, brwVis, bwVis, fleVis, flwVis, freVis, frwVis, fwVis;

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

   void Awake () { //Avoid doing calculations in here regarding edgework. Just use this for setting up buttons for simplicity.
      ModuleId = ModuleIdCounter++;
      GetComponent<KMBombModule>().OnActivate += Activate;
      /*
      foreach (KMSelectable object in keypad) {
          object.OnInteract += delegate () { keypadPress(object); return false; };
      }
      */

      LeftBtn.OnInteract += delegate () { Dir = Dir - 1 == -1 ? 3 : Dir - 1; return false; };
      RightBtn.OnInteract += delegate () { Dir = (Dir + 1) % 4; return false; };
   }

   void OnDestroy () { //Shit you need to do when the bomb ends
      
   }

   void Activate () { //Shit that should happen when the bomb arrives (factory)/Lights turn on

   }

   void Start () { //Shit that you calculate, usually a majority if not all of the module
      Position = new int[] { Rnd.Range(0, 6), Rnd.Range(0, 6) };
      Dir = Rnd.Range(0, 4);
   }

   void ToggleWalls () {

      string NEWS = "ULDR";

      if (!Mazes[0][Position[0]][Position[1]].Contains(NEWS[Dir])) {
         fwVis = true;
      }
      else {
         fwVis = false;
      }

      if (Position[0] == 0) {
         switch (Dir) {
            case 0:
               flwVis = true;
               fleVis = true;
               break;
            case 1:
               fwVis = true;
               flwVis = true;
               frwVis = true;
               break;
            case 2:

               break;
            default:
               break;
         }
      }

      switch (Dir) { //ULDR 0123
         case 0:
            if (Position[0] > 0) {

            }
            else {
               fwVis = true;
            }
            break;
         default:
            break;
      }
   }

   void Update () { //Shit that happens at any point after initialization
      ToggleWalls();
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

   void Solve () {
      GetComponent<KMBombModule>().HandlePass();
   }

   void Strike () {
      GetComponent<KMBombModule>().HandleStrike();
   }

#pragma warning disable 414
   private readonly string TwitchHelpMessage = @"Use !{0} to do something.";
#pragma warning restore 414

   IEnumerator ProcessTwitchCommand (string Command) {
      yield return null;
   }

   IEnumerator TwitchHandleForcedSolve () {
      yield return null;
   }
}
