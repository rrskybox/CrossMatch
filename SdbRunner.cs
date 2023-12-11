using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;

namespace CrossMatch
{
    class SdbRunner
    {
        //Opens sdb formated text file, reads in header
        //  reads in lines to list
        const string hTag = "</TheSkyDatabaseHeader>";

        public List<TargetData> SdbTargetList = new List<TargetData>();

        public XElement xSDBHeader;
        private string[] starLines;
        private int crossRefColMax;

        public SdbRunner(string sdbPath)
        {
            //Read target data into a target list from an sdb text file
            //get filename and open textfile for reading
            string sdbTextIn = File.ReadAllText(sdbPath);
            int hdrEndIdx = sdbTextIn.IndexOf(hTag) + hTag.Length + 1;
            string sdbTextHeader = sdbTextIn.Substring(0, hdrEndIdx);
            try { xSDBHeader = XElement.Parse(sdbTextHeader); }
            catch (Exception ex)
            {
                MessageBox.Show("Read SBD Text File Failed: "+ex.Message);
                return;
            }
            //starLines = (sdbTextIn.Substring(hdrEndIdx +1, sdbTextIn.Length - hdrEndIdx - 1)).Replace('\r', ' ').Split('\n'); ;
            //starLines = (sdbTextIn.Substring(hdrEndIdx, sdbTextIn.Length - hdrEndIdx)).Replace('\r', ' ').Split('\n'); ;
            List<string> sdbRows = new List<string>();
            StringReader sdbText = new StringReader(sdbTextIn.Substring(hdrEndIdx, sdbTextIn.Length - hdrEndIdx));
            while (sdbText.Peek()>0)
            {
                sdbRows.Add(sdbText.ReadLine());
            }
            SdbTargetList = BuildTargetList(sdbRows);
            starLines = sdbRows.ToArray();
            return;
        }

        private List<TargetData> BuildTargetList(List<string> sdbRows)
        {
            //Note index 0 is header row
            List<TargetData> lr = new List<TargetData>();
            foreach (string s in sdbRows)
            {
                TargetData target = GetTargetData(s);
                target.HasReference = false;
                lr.Add(target);
            }
            return lr;
        }

        public void AddCrossReference(string crossRef, int starIdx)
        {
            //Appends a cross Reference item to the end of the starIdx entry in starLines list
            starLines[starIdx] += "  " + crossRef;
            if (crossRef.Length + 2 > crossRefColMax)
                crossRefColMax = crossRef.Length + 2;
        }

        public void AddCrossReferenceHeader()
        {
            //Changes the crossreference type to 1, adds cross reference column definition in header

        }

        public string ReadFixedCell(string sdbRow, string sdbColName)
        {
            //Returns value in row sdbRow at column sdbColName, for Name, RA or Dec fixed sdb text column names
            //Read header to get column start and len from sdbColName
            string cellValue = null;
            XName xBegin = "colBeg";
            XName xEnd = "colEnd";
            XElement xCol = xSDBHeader.Descendants(sdbColName).First();
            if (xCol != null)
            {
                int colBeg = Convert.ToInt32(xCol.Attribute(xBegin).Value);
                int colEnd = Convert.ToInt32(xCol.Attribute(xEnd).Value);
                try { cellValue = sdbRow.Substring(colBeg - 1, colEnd - colBeg + 1); }
                catch { return cellValue; }
            }
            return cellValue;
        }

        public TargetData GetTargetData(string row)
        {
            //Converts a line in the starLine to a targetdata object
            TargetData lineData = new TargetData();
            lineData.TargetName = ReadFixedCell(row, "labelOrSearch");
            lineData.TargetRA = Convert.ToDouble(ReadFixedCell(row, "raHours"));
            lineData.TargetDec = Convert.ToDouble(ReadFixedCell(row, "decDegrees"));
            if (ReadFixedCell(row, "magnitude") != null)
                lineData.TargetMag = Convert.ToDouble(ReadFixedCell(row, "magnitude"));
            else
                lineData.TargetMag = 0;
            return lineData;
        }

        public void AppendTargetData(string listPath)
        {
            //Appends cross match data as last column to input sdb text lines
            int crColBeg = FindMaxLineLength();
            for (int i = 0; i < starLines.Length; i++)
                starLines[i] += "  " + SdbTargetList[i].CrossRefName;
            int crColEnd = FindMaxLineLength();
            for (int i = 0; i < starLines.Length - 1; i++)
                starLines[i] = starLines[i].PadRight(crColEnd);
            //Update sdb header to add crossReference column
            XElement crX = new XElement("crossReference",
                        new XAttribute("colBeg", crColBeg.ToString()),
                        new XAttribute("colEnd", crColEnd.ToString()));
            if (xSDBHeader.Element("crossReference") == null)
                xSDBHeader.Add(crX);
            else
                xSDBHeader.Element("crossReference").ReplaceWith(crX);
            //XElement refX = new XElement("crossReferenceType", "1");
            XElement formData = xSDBHeader.Element("crossReferenceType");
            if (formData != null)
                formData.Value = "1"; // Set to 1
            else
                xSDBHeader.Add(new XElement("crossReferenceType", "1"));
            //Resave appended file in filname_G.txt
            string newPath = listPath.Substring(0, listPath.Length - 4) + "_G.txt";
            File.WriteAllText(newPath, xSDBHeader + "\r\n");
            File.AppendAllLines(newPath, starLines);
            return;
        }


        private int FindMaxLineLength()
        {
            //Finds the maximum number of columns in the sdb database
            int maxLen = 0;
            for (int i = 0; i < starLines.Length; i++)
            {
                if (starLines[i].Length > maxLen)
                    maxLen = starLines[i].Length;
            }
            return maxLen;
        }

    }
}
