using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace ACG_GameBuild
{
    public class StoryControl
    {
        public string[] Story;
        public int row { get; set; }
        bool initiated = false;

        public void Initiates()
        {
            Story = InternalFileRead();
            initiated = true;
        }

        static string[] InternalFileRead()
        {
            string[] Stories;
            var Names = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("AVG_GameBuild.Resources.TestStory.txt"))
            using (StreamReader reader = new StreamReader(stream, Encoding.Default, true))
            {
                string result = reader.ReadToEnd();
                Stories = result.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            }
            return Stories;
        }

        public string ReadLine(int i)
        {
            if (!initiated) { Initiates(); }
            if (i >= Story.Length) { row = i = Story.Length - 1; return "The-End"; }
            string strings = Story[i];
            return strings;
        }

        public bool IsCommandCheck(string PossibleCommand)
        {
            switch (PossibleCommand)
            {
                default: return false;
                case "":
                    break;
                case "[Name]":
                    break;
                case "[Choice]":
                    break;
                case "[Jump]":
                    break;
                case "[Background Image]":
                    break;
                case "[Character Image]":
                    break;
                case "[BGM]":
                    break;
            }
            return true;
        }
    }
}
