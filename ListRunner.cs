using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;


namespace GaiaReferral
{
    public class ListRunner
    {
        char[] csvSplit = new char[] { ',' };

        //Split indices for "standard" csv file
        const int NameSplit = 0;
        const int RASplit = 1;
        const int DecSplit = 2;
        const int MagSplit = 3;

        //Split indices for raw IAU csv file -- not in use
        //const int NameSplit = 0;
        //const int RASplit = 7;
        //const int DecSplit = 8;
        //const int MagSplit = 6;

        //Column indices for raw IAU text file -- not in use
        const int StartName = 0;
        const int LengthName = 14;
        const int StartRA = 76;
        const int LengthRA = 12;
        const int StartDec = 96;
        const int LengthDec = 10;
        const int StartMag = 66;
        const int LengthMag = 5;

        public List<TargetData> TargetList = new List<TargetData>();

        public ListRunner(string csvPath)
        {
            //Read target data into a target list from a csv file
            //get filename and open textfile for reading
            string[] starLines = File.ReadAllLines(csvPath);
            //BuildColumnList(starLines[0]);
            TargetList = BuildTargetList(starLines);
            return;
        }

        private void BuildColumnList(string topLine)
        {
            //Read in the column headers from the input csv file to start column set up
            List<string> columnHeaders = topLine.Split(csvSplit, System.StringSplitOptions.None).Select(x => x.Trim('\"')).ToList();
            for (int i = 0; i < columnHeaders.Count; i++)
            {
                columnHeaders[i] = columnHeaders[i].Replace(" ", "");
                columnHeaders[i] = columnHeaders[i].Replace("#", "Num");
                columnHeaders[i] = columnHeaders[i].Replace("(", "");
                columnHeaders[i] = columnHeaders[i].Replace(")", "");
            }
        }

        private List<TargetData> BuildTargetList(string[] starLines)
        {
            //Note index 0 is header row
            List<TargetData> lr = new List<TargetData>();
            for (int i = 1; i < starLines.Length; i++)
            {
                if (starLines[i].Contains(","))
                {
                    TargetData target = new TargetData();
                    string[] csvEntries = starLines[i].Split(csvSplit, StringSplitOptions.None);
                    target.TargetName = csvEntries[NameSplit].ToString();
                    target.TargetRA = Convert.ToDouble(csvEntries[RASplit]);
                    target.TargetDec = Convert.ToDouble(csvEntries[DecSplit]);
                    try { target.TargetMag = Convert.ToDouble(csvEntries[MagSplit]); }
                    catch { target.TargetMag = null; }
                    //Convert RA decimal degrees to hours
                    target.TargetRA = target.TargetRA * 24 / 360;
                    target.HasReference = false;
                    lr.Add(target);
                }
            }
            return lr;
        }

        private TargetData BuildTarget(string starLine)
        {
            char splitChar = ',';

            TargetData target = new TargetData();
            string[] csvEntries = starLine.Split(splitChar);
            target.TargetName = csvEntries[NameSplit].ToString().TrimEnd(' ');
            target.TargetRA = Convert.ToDouble(csvEntries[RASplit]);
            target.TargetDec = Convert.ToDouble(csvEntries[DecSplit]);
            try { target.TargetMag = Convert.ToDouble(csvEntries[MagSplit]); }
            catch { target.TargetMag = null; }
            //Convert RA decimal degrees to hours
            target.TargetRA = target.TargetRA * 24 / 360;  //Convert RA to hours
            target.HasReference = false;
            return target;
        }

        public class TargetData
        {
            public string TargetName { get; set; }
            public double TargetRA { get; set; }
            public double TargetDec { get; set; }
            public double? TargetMag { get; set; }
            public bool HasReference { get; set; }
            public string CrossRefName { get; set; } = "None";
            public StarFinder.ReferenceData ReferenceStar = new StarFinder.ReferenceData();
            
        }

    }
}
