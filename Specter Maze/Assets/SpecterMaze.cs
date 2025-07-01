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
      new string[][] {
         new string[] { "RD", "LR", "L", "R", "R", "DL" },
         new string[] { "UD", "R", "LDR", "L", "D", "DU"},
         new string[] { "U", "D", "UD", "R", "R", "U"},
         new string[] { "RD", "LU", "UD", "D", "RU", "L"},
         new string[] { "UD", "D", "U", "UR", "LR", "LD"},
         new string[] { "U", "UR", "LR", "LR", "L", "U"},
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
