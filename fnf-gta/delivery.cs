using System;
using System.Windows.Forms;
using GTA;
using GTA.Native;
using GTA.Math;
using System.Drawing;

namespace delivery
{
    public class Main : Script
    {
        // Mission 1
        int MissionIndex = -1;
        int PackageIndex = -1;
        // Section 1
        bool isPackage = false;
        // Section 2
        bool isMission = false;
 
        Blip MissionBlip;

        Prop MissionPackage;

        Vector3 oneMissionPos = new Vector3(-847.4676f, 158.6787f, 66.28177f);
        Vector3 lesterPos = new Vector3(1274.928f, -1721.641f, 54.65506f);
        public Main()
        {
            Tick += onTick;
            KeyDown += onKeyDown;
        }
        private void onTick(object sender, EventArgs e)
        {
            if(isMission)
            {
                switch(MissionIndex)
                {
                    case 0:
                        {
                            MissionBlip = World.CreateBlip(oneMissionPos);
                            if(MissionBlip.Exists())
                            {
                                MissionBlip.Sprite = (BlipSprite)1;
                                MissionBlip.Color = BlipColor.Green2;
                                MissionBlip.Name = "Michael's Package";
                                MissionBlip.ShowRoute = true;
                            }
                        }
                        break;
                    case 10:
                        {
                            if (Game.Player.Character.Position.DistanceTo(oneMissionPos) < 50f);
                            {
                                if(MissionBlip.Exists())
                                {
                                    MissionBlip.ShowRoute = false;
                                }
                                UI.Notify("Steal Michael's Package");

                                Model packageModel = new Model("prop_cs_package_01");
                                packageModel.Request(10000);
                                
                                if (packageModel.IsValid & packageModel.IsInCdImage)
                                {
                                    while (!packageModel.IsLoaded) Script.Wait(50);
                                    MissionPackage = World.CreateProp(packageModel, oneMissionPos, false, true);
                                }

                                MissionIndex = 20;
                            }
                        }
                        break;
                    case 20:
                        {
                            World.DrawMarker(MarkerType.VerticalCylinder, new Vector3(-847.4676f, 158.6787f, 65.3612f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(1.5f, 1.5f, 0.5f), Color.FromArgb(0, 0, 255));
                            if (Game.Player.Character.Position.DistanceTo(oneMissionPos) < 1.5f)
                            {
                                DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to take Michael's ~y~Package.");
                                if(Game.IsControlJustPressed(2, GTA.Control.Context))
                                {
                                    MissionIndex = 30;
                                }
                            }
                        }
                        break;
                    case 30:
                        {
                            UI.Notify("Package stolen!");
                            MissionIndex = 40;
                        }
                        break;
                    case 40:
                        {
                            if(MissionBlip.Exists())
                            {
                                MissionBlip.Alpha = 0;
                                MissionBlip.Remove();

                                MissionPackage.Alpha = 0;
                                MissionPackage.Delete();

                                isPackage = true;
                                PackageIndex = 0;
                            }
                            isMission = false;
                            MissionIndex = -1;
                        }
                        break;
                }
            }
            if(isPackage)
            {
                switch(PackageIndex)
                {
                    case 0:
                        {
                            UI.Notify("Take the package to ~y~Lester.");
                            PackageIndex = 10;
                        }
                        break;
                    case 10:
                        {
                            MissionBlip = World.CreateBlip(lesterPos);
                            if(MissionBlip.Exists())
                            {
                                MissionBlip.Sprite = (BlipSprite)1;
                                MissionBlip.Color = BlipColor.Yellow;
                                MissionBlip.Name = "Lester";
                                MissionBlip.ShowRoute = true;
                                PackageIndex = 20;
                            }
                        }
                        break;
                    case 20:
                        {
                            World.DrawMarker(MarkerType.VerticalCylinder, new Vector3(1274.9387f, -1721.593f, 53.7390f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(1.5f, 1.5f, 0.5f), Color.FromArgb(0, 0, 255));
                            if (Game.Player.Character.Position.DistanceTo(lesterPos) < 1.5f)
                            {
                                DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to drop off the ~y~Package.");
                                if (Game.IsControlJustPressed(2, GTA.Control.Context))
                                {
                                    PackageIndex = 30;
                                }
                            }
                        }
                        break;
                    case 30:
                        {
                            UI.Notify("Package Dropped off");
                            PackageIndex = 40;
                        }
                        break;
                    case 40:
                        {
                            if(MissionBlip.Exists())
                            {
                                MissionBlip.Alpha = 0;
                                MissionBlip.Remove();

                                MissionPackage.Alpha = 0;
                                MissionPackage.Delete();

                                PackageIndex = 50;
                            }
                        }
                        break;
                    case 50:
                        {
                            Model packageModel = new Model("prop_cs_package_01");
                            packageModel.Request(10000);
                            if (packageModel.IsValid & packageModel.IsInCdImage)
                            {
                                while (!packageModel.IsLoaded) Script.Wait(50);
                                MissionPackage = World.CreateProp(packageModel, lesterPos, false, true);

                                isPackage = false;
                                MissionIndex = -1;
                            }
                        }
                        break;
                }
            }
        }
        private void onKeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Z)
            {
                // temp gonna add a event function later
                isMission = true;
                MissionIndex = 0;
            }
        }
        // This Function can be found on gtaforums.com/topic/820813-displaying-help-text/
        void DisplayHelpTextThisFrame(string text)
        {
            Function.Call(Hash._SET_TEXT_COMPONENT_FORMAT, "STRING");
            Function.Call(Hash._ADD_TEXT_COMPONENT_STRING, text);
            Function.Call(Hash._0x238FFE5C7B0498A6, 0, 0, 1, -1);
        }
    }
}
