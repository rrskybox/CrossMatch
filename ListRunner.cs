/*
* Methods for reading in csv file
* 
* Author:           Rick McAlister
* Date:             12/5/2022
* Current Version:  1.0
* Developed in:     Visual Studio 2019
* Coded in:         C# 8.0
* App Envioronment: Windows 10 Pro, .Net 4.8, TSX 5.0 Build 13479
* 
* Change Log:
* 
* 12/5/2022 Rev 1.0  Release
* 
*/

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Forms;

namespace CrossMatch
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

        public List<TargetData> CsvTargetList = new List<TargetData>();

        public ListRunner(string csvPath, bool raIsDegrees)
        {
            //Read target data into a target list from a csv file
            // Create a UTF-8 encoding.
            Encoding decode = Encoding.GetEncoding(65001);
            //get filename and open textfile for reading
            string[] starLines = null;
            try { starLines = File.ReadAllLines(csvPath, decode); }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            CsvTargetList = BuildTargetList(starLines, raIsDegrees);
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

        private List<TargetData> BuildTargetList(string[] starLines, bool raIsDegrees)
        {
            //Note index 0 is header row
            List<TargetData> lr = new List<TargetData>();
            for (int i = 1; i < starLines.Length; i++)
            {
                if (starLines[i].Contains(","))
                {
                    TargetData target = new TargetData();
                    string[] csvEntries = starLines[i].Split(csvSplit, StringSplitOptions.None);
                    target.TargetName = ToASCII(csvEntries[NameSplit].ToString());
                    target.TargetRA = Utility.ParseRADecString(csvEntries[RASplit], ' ');
                    target.TargetDec = Utility.ParseRADecString(csvEntries[DecSplit], ' ');
                    try { target.TargetMag = Convert.ToDouble(csvEntries[MagSplit]); }
                    catch { target.TargetMag = null; }
                    //Convert RA decimal degrees to hours
                    if (raIsDegrees)
                        target.TargetRA = target.TargetRA * 24 / 360;
                    target.HasReference = false;
                    lr.Add(target);
                }
            }
            return lr;
        }

        private string ToASCII(string s)
        {
            String norm = s.Normalize(NormalizationForm.FormKC);
            String q = String.Join("",
                s.Normalize(NormalizationForm.FormD)
               .Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark));
            return q;
        }

        public string ToASCII2(string stIn)
        {
            string stFormD = stIn.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }
            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }

        public string ToASCII3(string str)
        {
            if (str == null) return null;
            var chars =
                from c in str.Normalize(NormalizationForm.FormD).ToCharArray()
                let uc = CharUnicodeInfo.GetUnicodeCategory(c)
                where uc != UnicodeCategory.NonSpacingMark
                select c;
            var cleanStr = new string(chars.ToArray()).Normalize(NormalizationForm.FormC);
            return cleanStr;
        }

        private TargetData BuildTarget(string starLine)
        {
            char splitChar = ',';

            TargetData target = new TargetData();
            string[] csvEntries = starLine.Split(splitChar);
            target.TargetName = ToASCII(csvEntries[NameSplit].ToString().TrimEnd(' '));
            target.TargetRA = Convert.ToDouble(csvEntries[RASplit]);
            target.TargetDec = Convert.ToDouble(csvEntries[DecSplit]);
            try { target.TargetMag = Convert.ToDouble(csvEntries[MagSplit]); }
            catch { target.TargetMag = null; }
            //Convert RA decimal degrees to hours
            target.TargetRA = target.TargetRA * 24 / 360;  //Convert RA to hours
            target.HasReference = false;
            return target;
        }

    }
}
