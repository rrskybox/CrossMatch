using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaiaReferral
{
    class ListRunner
    {
        //
        const int NameSplit = 0;
        const int RASplit = 7;
        const int DecSplit = 8;
        const int MagSplit = 6;

        const int StartName = 0;
        const int LengthName = 14;
        const int StartRA = 76;
        const int LengthRA = 12;
        const int StartDec = 96;
        const int LengthDec = 10;
        const int StartMag = 66;
        const int LengthMag = 5;

        public static List<Referral> BuildReferenceList(string[] lines)
        {
            //Note index 0 is header row
            List<Referral> lr = new List<Referral>();
            for (int i = 1; i < lines.Length; i++)
            {
                Referral refer = new Referral();
                refer.Name = lines[i].Substring(0, 14).TrimEnd();
                refer.RA = Convert.ToDouble(lines[i].Substring(76, 10).TrimStart());
                refer.Dec = Convert.ToDouble(lines[i].Substring(96, 8).TrimStart());
                refer.Magnitude = Convert.ToDouble(lines[i].Substring(65, 6).TrimStart());
                //Convert RA to hours
                refer.RA = refer.RA * 24 / 360;
                lr.Add(refer);
            }
            return lr;
        }

        public static List<Referral> BuildReferenceListCSV(string[] lines)
        {
            //Note index 0 is header row
            List<Referral> lr = new List<Referral>();
            for (int i = 1; i < lines.Length; i++)
            {
                Referral refer = new Referral();
                string[] csvEntries = lines[i].Split(new char[',']);
                refer.Name = csvEntries[NameSplit].ToString();
                refer.RA = Convert.ToDouble(csvEntries[RASplit]);
                refer.Dec = Convert.ToDouble(csvEntries[DecSplit]);
                try { refer.Magnitude = Convert.ToDouble(csvEntries[MagSplit]); }
                catch { refer.Magnitude = null; }
                refer.RA = refer.RA * 24 / 360;
                lr.Add(refer);
            }
            return lr;
        }

        public static Referral BuildReference(string starLine)
        {

            //Note index 0 is header row
            Referral refer = new Referral();
            refer.Name = starLine.Substring(StartName, LengthName).TrimEnd();
            refer.RA = Convert.ToDouble(starLine.Substring(StartRA, LengthRA).TrimStart());
            refer.Dec = Convert.ToDouble(starLine.Substring(StartDec, LengthDec).TrimStart());
            try { refer.Magnitude = Convert.ToDouble(starLine.Substring(StartMag, LengthMag).TrimStart()); }
            catch { refer.Magnitude = null; }
            //Convert RA to hours
            refer.RA = refer.RA * 24 / 360;
            return refer;
        }

        public static Referral BuildReferenceCSV(string starLine)
        {
            char splitChar = ',';

            Referral refer = new Referral();
            string[] csvEntries = starLine.Split(splitChar);
            refer.Name = csvEntries[NameSplit].ToString();
            refer.RA = Convert.ToDouble(csvEntries[RASplit]);
            refer.Dec = Convert.ToDouble(csvEntries[DecSplit]);
            try { refer.Magnitude = Convert.ToDouble(csvEntries[MagSplit]); }
            catch { refer.Magnitude = null; }            
            refer.RA = refer.RA * 24 / 360;  //Convert RA to hours
            return refer;
        }

        public struct Referral
        {
            public string Name;
            public double RA;
            public double Dec;
            public double? Magnitude;
            public string CrossGaiaName;

        }
    }
}
