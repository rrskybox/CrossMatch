/*
* AAVSO Reader is a VSX client for assembling nova data
* 
* Author:           Rick McAlister
* Date:             01/05/24
* Current Version:  0.1
* Developed in:     Visual Studio 2022
* Coded in:         C# 8.0 WPF
* App Envioronment: Windows 10 Pro x32 and x64 (DB12978)
* 
* Change Log:
* 
* 
* 
*/

using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Web;
using System.Drawing;

namespace CrossMatch
{
    class SIMBAD
    {

        //https://simbad.cds.unistra.fr/simbad/sim-id?output.format=votable&Ident=HIP%2077442&output.params=ids

        const string SIMBADGETURL = "https://simbad.cds.unistra.fr/simbad/sim-id?output.format=votable&";
        const string tsxHead = "name, etc";
        const string Q_Identifier = "Ident=";
        //const string Query_Command_ListIdentifiers = "Ids";
        const string Q_OutputFormat = "output.params=Ids";

        static public string SimbadQueryResults(string identifier)
        {
            string url_SIMBAD_Search = SIMBADGETURL;
            //const string tsxHead = "SN      Host Galaxy      Date         R.A.    Decl.    Offset   Mag.   Disc. Ref.            SN Position         Posn. Ref.       Type  SN      Discoverer(s)";
            string contents;

            WebClient client = new WebClient();
            System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };


            string simbadURLquery = url_SIMBAD_Search + Q_Identifier + identifier + "&" + Q_OutputFormat;
            simbadURLquery = simbadURLquery.Replace(" ", "%20");

            try
            {
                contents = client.DownloadString(simbadURLquery);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Download Error: " + ex.Message);
                return "";
            };
            //Clear out all the "Votable" attributes because they kill the XML parsing for some reason
            int sAttrib = contents.IndexOf("<VOTABLE ");
            //clear the string until votable
            string clear1 = contents.Remove(1, sAttrib);
            int eAttrib = clear1.IndexOf(">");
            string clear2 = clear1.Remove(9, eAttrib - 9);

            XElement xmlDoc = XElement.Parse(clear2);

            IEnumerable<XElement> dList1 = xmlDoc.Descendants("TR");
            if (dList1.Count() > 0)
            {
                List<string> refList = dList1.First().Value.Split('|').ToList();
                string gaiaRef = refList.FirstOrDefault(x => x.Contains("Gaia DR3"));
                return gaiaRef;
            }
            else
                return null;
        }

 
        
        
        public static string FitFormat(string entry, int slotSize)
        {
            //Returns a string which is the entry truncated to the slot Size, if necessary
            if (entry == null) return "                    ".Substring(0, slotSize);
            if (entry.Length > slotSize)
                return entry.Substring(0, slotSize - 1).PadRight(slotSize);
            else
                return entry.PadRight(slotSize);
        }

        public static string ParseToSexidecimal(string sex, bool doRA)
        {
            //converts a string in decimal format to a string in sexidecimal format
            //  uses hours if doRA is true
            //  note the AAVSO reports RA in degrees
            double d = Convert.ToDouble(sex);
            int dsign = Math.Sign(d);
            double dAbs = Math.Abs(d);
            if (doRA) //Convert RA degrees to hours
            {
                dAbs = dAbs * 24.0 / 360.0;
            }
            int degHrs = (int)(dAbs);
            dAbs -= degHrs;
            int min = (int)(dAbs * 60);
            dAbs -= (min / 60.0);
            double sec = dAbs * 3600;
            string degHrOut = String.Format("{00}", (dsign * degHrs)).PadLeft(2, '0');
            string minOut = String.Format("{00}", min).PadLeft(2, '0');
            string secOut = sec.ToString("0.000").PadLeft(5, '0');
            //return (dsign * degHrs).ToString("D" + 2) + ":" + min.ToString("I" + 2) + ":" + sec.ToString("D" + 5);
            string leadingSign = "";
            if (!doRA && dsign >= 0)
                leadingSign = "+";
            string sexOut = leadingSign + degHrOut + ":" + minOut + ":" + secOut;
            return sexOut;
        }

        public static string StringafyRecords(IEnumerable<XElement> dList)
        {
            string cbText = "";
            foreach (XElement tr in dList)
            {
                AAVSOData votable = new AAVSOData(tr);
                //Duplicate the TSX photo input format -- i.e. make it the same as the Harvard IUA display format
                //  as it is copied into the clipboard
                //Create a text string to be filled in for the clipboard: Column headings and two newlines.

                //Create a name that might fit in a 8 char slot -- AAVSO's won't
                //Put the actual name in the "Galaxy" slot where there is a lot more room
                string novaName = votable.NovaYear + votable.Const;
                //And, convert ra/dec to sexidecimal
                string raSex = ParseToSexidecimal(votable.Coords_J2000_RA, true);
                string decSex = ParseToSexidecimal(votable.Coords_J2000_Dec, false);
                //Name
                cbText += FitFormat(novaName, 8).PadRight(8);
                //Name of the Host Galaxy as Milky Way
                cbText += FitFormat(votable.Name, 17);
                //Discovery Date
                cbText += FitFormat(votable.NovaYear + "-01-01", 12);
                //Truncated RA and Dec for locale
                cbText += FitFormat(raSex, 8);
                cbText += FitFormat(decSex, 12);
                //Offsets?
                cbText += "       ";  //offsets
                //Magnitude
                cbText += FitFormat(votable.MaxMag, 8);
                //Ext_catalogs"
                cbText += FitFormat("", 15);
                //Actual RA/Dec location
                cbText += FitFormat(raSex, 12);
                cbText += FitFormat(decSex, 14);
                //filler for Position Reference
                cbText += "                 ";
                //Supernova Type
                cbText += FitFormat(votable.VarType, 6);
                //Supernova Name (as derived from entry name
                cbText += FitFormat(votable.Name, 8);
                //Discoverer
                //cbText += FitFormat(xmlItem.Element("Discovering_Groups").Value, 12);
                //New Line
                cbText += "\n";
            }
            return cbText;
        }

        public class AAVSOData
        {
            public AAVSOData(XElement trRecord)
            {
                const string AUID = "auid";
                const string NAME = "name";
                const string CONST = "const";
                const string COORDS_J2000 = "radec2000";
                const string VARTYPE = "varType";
                const string MAXMAG = "maxMag";
                const string MAXPASS = "maxPass";
                const string MINMAG = "minMag";
                const string MINPASS = "minPass";
                const string EPOCH = "epoch";
                const string NOVAYR = "novaYr";
                const string PERIOD = "period";
                const string RISEDUR = "riseDur";
                const string SPECTYPE = "specType";
                const string DISC = "disc";
                //Load the class structure
                List<XElement> records = trRecord.Elements("TD").ToList();
                Auid = records[0].Value;
                Name = records[1].Value;
                Const = records[2].Value;
                Coords_J2000_RA = records[3].Value.Split(',')[0];
                Coords_J2000_Dec = records[3].Value.Split(',')[1];
                VarType = records[4].Value;
                MaxMag = records[5].Value;
                MaxMagPassband = records[6].Value;
                MinMag = records[7].Value;
                MinMagPassband = records[8].Value;
                Epoch = records[9].Value;
                NovaYear = records[10].Value;
                if (NovaYear == "") NovaYear = "1000";
                Period = records[11].Value;
                RiseDuration = records[12].Value;
                SpecType = records[13].Value;
                Discoverer = records[14].Value;
                return;
            }

            public string Auid { get; set; }
            public string Name { get; set; }
            public string Const { get; set; }
            public string Coords_J2000_RA { get; set; }
            public string Coords_J2000_Dec { get; set; }
            public string VarType { get; set; }
            public string MaxMag { get; set; }
            public string MaxMagPassband { get; set; }
            public string MinMag { get; set; }
            public string MinMagPassband { get; set; }
            public string Epoch { get; set; }
            public string NovaYear { get; set; }
            public string Period { get; set; }
            public string RiseDuration { get; set; }
            public string SpecType { get; set; }
            public string Discoverer { get; set; }
        }
    }

}

