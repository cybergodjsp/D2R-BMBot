using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MapAreaStruc;

public class Shopper    
{
    Form1 Form1_0;

    public int CurrentStep = 0;
    public List<int> IgnoredChestList = new List<int>();
    public bool ScriptDone = false;
    public int Shoprun = 0;
    public bool HasTakenAnyChest = false;
    public Position ChestPos = new Position { X = 0, Y = 0 };

    public void SetForm1(Form1 form1_1)
    {
        Form1_0 = form1_1;
    }

    public void ResetVars()
    {
        CurrentStep = 0;
        IgnoredChestList = new List<int>();
        ScriptDone = false;
    }

    public void RunScript()
    {
        Form1_0.Town_0.ScriptTownAct = 5; //set to town act 5 when running this script

        if (!Form1_0.Running || !Form1_0.GameStruc_0.IsInGame())
        {
            ScriptDone = true;
            return;
        }

        if (Form1_0.Town_0.GetInTown())
        {
            Form1_0.SetGameStatus("GO TO WP");
            CurrentStep = 0;

            Form1_0.Town_0.GoToWPArea(5, 1);
        }
        else
        {
            if (CurrentStep == 0)
            {
                Form1_0.SetGameStatus("DOING Shopping");

                if ((Enums.Area)Form1_0.PlayerScan_0.levelNo == Enums.Area.Harrogath)
                {
                    CurrentStep++;
                }
                else
                {
                    Form1_0.Town_0.FastTowning = false;
                    Form1_0.Town_0.GoToTown();
                }
            }

            if (CurrentStep == 1)
            {
                if (TownAct == 5)
                {
                    CheckForNPCValidPos("Anya");
                    //Form1_0.PathFinding_0.MoveToNPC("Anya");  //not found
                    Form1_0.PathFinding_0.MoveToThisPos(new Position { X = 5103, Y = 5115 });
                    Form1_0.NPCStruc_0.GetNPC("Anya");
                    MovedCorrectly = true;
                }

                if (MovedCorrectly)
                {
                    //Clic store
                    Position itemScreenPos = Form1_0.GameStruc_0.World2Screen(Form1_0.PlayerScan_0.xPosFinal, Form1_0.PlayerScan_0.yPosFinal, Form1_0.NPCStruc_0.xPosFinal, Form1_0.NPCStruc_0.yPosFinal);

                    Form1_0.KeyMouse_0.MouseClicc_RealPos(itemScreenPos.X, itemScreenPos.Y);
                    if (Form1_0.UIScan_0.WaitTilUIOpen("npcInteract"))  //npcShop
                    {
                        if (TownAct == 5)
                        {
                            Form1_0.KeyMouse_0.PressKey(System.Windows.Forms.Keys.Down); //Anya press down
                        }
                        Form1_0.KeyMouse_0.PressKey(System.Windows.Forms.Keys.Enter);
                        Form1_0.WaitDelay(50);
                        Form1_0.ItemsStruc_0.ShopBotGetPurchaseItems();
                        Form1_0.UIScan_0.CloseUIMenu("npcInteract");
                        Form1_0.UIScan_0.CloseUIMenu("npcShop");
                    }
                }
                CurrentStep++;
            }

            if (CurrentStep == 2)
            {
                Position itemScreenPos = Form1_0.GameStruc_0.World2Screen(Form1_0.PlayerScan_0.xPosFinal, Form1_0.PlayerScan_0.yPosFinal, 5117, 5120);

                Form1_0.KeyMouse_0.MouseClicc_RealPos(itemScreenPos.X, itemScreenPos.Y - 15);
                Form1_0.WaitDelay(100);
                CurrentStep++;

            }

            if (CurrentStep == 3)
            {
                if ((Enums.Area)Form1_0.PlayerScan_0.levelNo == Enums.Area.NihlathaksTemple)
                {
                    if ((Enums.Area)Form1_0.Battle_0.AreaIDFullyCleared != Enums.Area.NihlathaksTemple)
                    {
                        Form1_0.Battle_0.ClearFullAreaOfMobs();

                        if (!Form1_0.Battle_0.ClearingArea)
                        {
                            CurrentStep++;
                        }
                    }
                }
                else
                {
                    CurrentStep == 2;
                }
                CurrentStep++;
            }


            if (CurrentStep == 4)
            {
                Form1_0.Town_0.Towning = true;
                Form1_0.Town_0.FastTowning = false;
                Form1_0.Town_0.UseLastTP = false;
                Shoprun++;
                CurrentStep == 1;
            }
        }
    }


}
