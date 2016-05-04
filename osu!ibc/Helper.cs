using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace osu_ibc
{
    static class Helper
    {
        public static string GetBackgroundFromBeatmap(string beatmapPath)
        {
            try {
                using (StreamReader sr = new StreamReader(File.OpenRead(beatmapPath))) {
                    string line;
                    while ((sr.ReadLine() != "[Events]") && !sr.EndOfStream) { }

                    //read
                    while ((line = sr.ReadLine()) != "[TimingPoints]") {
                        if (string.IsNullOrWhiteSpace(line)) continue;          //[X] bad line
                        if (line.StartsWith("//")) continue;                    //[X] comments
                        if (line.StartsWith("0,")) return line.GetBetween('"'); //[v] background line
                    }
                }
            }
            catch (Exception) {
                return "";
            }
            
            return "";
        }

        public static bool AddBeatmapsToDictionary(string directory, ref Dictionary<string, string> dictionary)
        {
            bool containsBeatmaps = false;
            foreach (string file in Directory.GetFiles(directory).Where(file => Path.GetExtension(file) == ".osu")) {
                containsBeatmaps = true;
                if (dictionary.ContainsKey(file)) continue;     //ignore if already read
                string img = GetBackgroundFromBeatmap(file);    //get file name
                if (!string.IsNullOrEmpty(img)) dictionary.Add(file, directory + img);  //add to dictionary
            }
            return containsBeatmaps;
        }

        public static string GetBetween(this string str, char bounds)
        {
            int firstIndex = str.IndexOf(bounds)+1;
            return str.Substring(firstIndex, str.LastIndexOf(bounds) - firstIndex);
        }
    }
}
