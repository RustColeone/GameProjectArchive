using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace FgoAutoPlayConsole
{
    class Program
    {
        #region DLL Import
        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string strClassName, string strWindowName);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);

        #endregion
        #region Variables
        public struct Rect
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
        }

        public static int PosX;
        public static int PosY;
        public static int PosXR;
        public static int PosYB;

        public static string ProcessName = "Nox";

        public static Rect SIMRect = new Rect();
        #endregion
        #region SimulateOperation
        public static void GetSimulatorPosition(string TempProcessName)
        {
            Process[] processes = Process.GetProcessesByName(TempProcessName);
            Process Simulator = processes[0];
            IntPtr ptr = Simulator.MainWindowHandle;
            GetWindowRect(ptr, ref SIMRect);
            PosX = SIMRect.Left;
            PosY = SIMRect.Top;
            PosXR = SIMRect.Right;
            PosYB = SIMRect.Bottom;
        }

        //This simulates a left mouse click
        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;
        public static void LeftMouseClick(float x, float y)
        {
            int xpos = Convert.ToInt32(x);
            int ypos = Convert.ToInt32(y);
            SetCursorPos(xpos, ypos);
            mouse_event(MOUSEEVENTF_LEFTDOWN, xpos, ypos, 0, 0);
            System.Threading.Thread.Sleep(10);
            mouse_event(MOUSEEVENTF_LEFTUP, xpos, ypos, 0, 0);
        }
        #endregion
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to FGO auto player \nPlease input the name of the simulator you are using");
            Console.WriteLine("For example, if you are using NoxPlayer, type: Nox");
            string TempProcessName = Console.ReadLine();
            try
            {
                GetSimulatorPosition(TempProcessName);
                Console.WriteLine("Found simulator, it's borders are...");
                Console.WriteLine("Top: " + PosY + " Bottom: " + PosYB + " Left: " + PosX + " Right: " + PosXR);
                ProcessName = TempProcessName;
            }
            catch
            {
                GetSimulatorPosition(ProcessName);
                Console.WriteLine("Phrase Error\n >" + ProcessName + "Used instead\n");
            }
            while (true)
            {
                Console.WriteLine("Press Enter to Start or ESC to Exit");
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    //DefaultR();
                    EventR();
                    Console.WriteLine("Run Complete");
                }
                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    break;
                }
            }
            Console.WriteLine("Session Closed");
        }
        public static void DefaultR()
        {
            //UseSkills(3, 3, 2);//Option 1

            UseSkills(2, 3, 0);
            OpenCards();
            FinalAttack(2);
            ChooseCards(1);
            ChooseCards(2);
            EndTurnDelay();

            //UseSkills(3, 1, 0);//Option 1
            UseSkills(3, 1, 0);//Option 2
            UseSkills(3, 3, 0);//Option 2

            UseSkills(1, 1, 0);
            UseSkills(1, 3, 0);
            OpenCards();
            FinalAttack(1);
            ChooseCards(1);
            ChooseCards(2);
            EndTurnDelay();

            UseSkills(2, 3, 0);
            OpenCards();
            FinalAttack(2);
            ChooseCards(1);
            ChooseCards(2);
            //EndTurnDelay();
        }

        public static void EventR()
        {
            UseSkills(2, 3, 0);
            OpenCards();
            FinalAttack(2);
            ChooseCards(1);
            ChooseCards(2);
            EndTurnDelay();

            UseSkills(3, 3, 1);

            UseSkills(1, 1, 0);
            UseSkills(1, 3, 0);
            OpenCards();
            FinalAttack(1);
            ChooseCards(1);
            ChooseCards(2);
            EndTurnDelay();

            OpenCards();
            ChooseCards(1);
            ChooseCards(2);
            ChooseCards(3);
            EndTurnDelay();

            UseSkills(3, 1, 0);
            UseSkills(2, 3, 0);
            UseSkills(2, 1, 0);

            MasterUseSkills(2, 2);

            OpenCards();
            FinalAttack(2);
            ChooseCards(1);
            ChooseCards(2);
            //EndTurnDelay();
        }
        public static void UseSkills(int CharPos, int SkillNo, int TargetPos)
        {
            float X;
            float Y = 650 / 809f * (PosYB - PosY);
            switch (CharPos)
            {
                case 1:
                    X = (80 + 100 * (SkillNo - 1)) / 1422f * (PosXR - PosX);
                    LeftMouseClick(PosX + X, PosY + Y);
                    break;
                case 2:
                    X = (420 + 100 * (SkillNo - 1)) / 1422f * (PosXR - PosX);
                    LeftMouseClick(PosX + X, PosY + Y);
                    break;
                case 3:
                    X = (760 + 100 * (SkillNo - 1)) / 1422f * (PosXR - PosX);
                    LeftMouseClick(PosX + X, PosY + Y);
                    break;
            }
            Console.WriteLine("Character " + CharPos + " used the " + SkillNo  + " skill");

            X = 1070 / 1604f * (PosXR - PosX);
            Y = 560 / 934f * (PosYB - PosY);
            System.Threading.Thread.Sleep(500);//Skill "Confirm" Button
            LeftMouseClick(PosX + X, PosY + Y);

            if (TargetPos != 0)
            {
                System.Threading.Thread.Sleep(500);
                Y = 500f / 809f * (PosYB - PosY);
                X = (300 + 400 * (TargetPos - 1)) / 1422f * (PosXR - PosX);
                LeftMouseClick(PosX + X, PosY + Y);
                Console.WriteLine("The skill targeted " + TargetPos + " and was used");
            }

            System.Threading.Thread.Sleep(3000);
        }

        public static void MasterUseSkills(int SkillNo, int TargetPos)
        {
            float X;
            float Y = 400 / 809f * (PosYB - PosY);

            X = (1350) / 1422f * (PosXR - PosX);
            LeftMouseClick(PosX + X, PosY + Y);
            System.Threading.Thread.Sleep(500);

            X = (1000 + 80 * (SkillNo - 1)) / 1422f * (PosXR - PosX);
            LeftMouseClick(PosX + X, PosY + Y);
            System.Threading.Thread.Sleep(500);

            Console.WriteLine("Master " + " used the " + SkillNo + " skill");

            X = 1070 / 1604f * (PosXR - PosX);
            Y = 560 / 934f * (PosYB - PosY);
            System.Threading.Thread.Sleep(500);//Skill "Confirm" Button
            LeftMouseClick(PosX + X, PosY + Y);

            if (TargetPos != 0)
            {
                System.Threading.Thread.Sleep(500);
                Y = 500f / 809f * (PosYB - PosY);
                X = (300 + 400 * (TargetPos - 1)) / 1422f * (PosXR - PosX);
                LeftMouseClick(PosX + X, PosY + Y);
                Console.WriteLine("The skill targeted " + TargetPos + " and was used");
            }

            System.Threading.Thread.Sleep(3000);
        }
        public static void OpenCards()
        {
            float X = 1220 / 1422f * (PosXR - PosX);
            float Y = 650 / 809f * (PosYB - PosY);
            LeftMouseClick(PosX + X, PosY + Y);
            Console.WriteLine("Opened card selection panel");
            System.Threading.Thread.Sleep(1500);
        }
        public static void ChooseCards(int CardPos)
        {
            float X;
            float Y = 580f / 809f * (PosYB - PosY);
            X = (135 + 280 * (CardPos - 1)) / 1422f * (PosXR - PosX);
            LeftMouseClick(PosX + X, PosY + Y);
            Console.WriteLine("Choosed " + CardPos + " Card");
            System.Threading.Thread.Sleep(500);
        }
        public static void FinalAttack(int CardPos)
        {
            float X;
            float Y = 255f / 809f * (PosYB - PosY);
            X = (440 + 250 * (CardPos - 1)) / 1422f * (PosXR - PosX);
            LeftMouseClick(PosX + X, PosY + Y);
            Console.WriteLine("Choosed " + CardPos + " Final Attack");
            System.Threading.Thread.Sleep(500);
        }
        public static void EndTurnDelay()
        {
            Console.WriteLine("Turn Ended, wait for 25s");
            System.Threading.Thread.Sleep(45000);
        }
    }
}
