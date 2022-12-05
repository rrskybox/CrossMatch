/*
* ReadReferral Class
*
* Class for downloading and parsing database query results
* 
* This class serves as method template for conversions from all 
*  catalog sources
* 
* Author:           Rick McAlister
* Date:             12/5/22
* Current Version:  1.0
* Developed in:     Visual Studio 2019
* Coded in:         C# 8.0
* App Envioronment: Windows 10 Pro, .Net 4.8, TSX 5.0 Build 12978
* 
* Change Log:
* 
* 4/23/21 Rev 1.0  Release
* 
*/

using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.Xml.Linq;

namespace GaiaReferral
{
    public class ReadReferral
    {

        #region column set up


        #endregion

        //private SDBDesigner sdbDesign;
        private List<string> columnHeaders;
        private XElement sdbXResults;
        private XDocument sdbXDoc;

        //
        public string SDBIdentifier { get; set; } = "Star Label";
        public string SDBDescription { get; set; } = "";
        public int SearchBackDays { get; set; }
        public int MaxRecordCount { get; set; }
        public bool IsNGCHosted { get; set; }
        public bool SearchSN { get; set; }
        public bool SearchClassified { get; set; }

        public ReadReferral(string topLine)
        {
        }

        //public void IAUResultsCSVToIAUResultsXML(List<ListRunner.TargetData> tgtData, string textSDBpath)
        //{
        //    //To be ussed to convert IAU result (after references) to SDB reference query input format

        //    SDBDesigner sdbDesign = new SDBDesigner();
        //    XElement headerRecordX = new XElement("SDBColumnFields");
        //    //Make a map of the headers
        //    sdbDesign.MakeHeaderMap(tgtData);
        //    XMLParser.XMLToSDBClipboard(sdbDesign, sdbXDoc, sdbXResults);
        //    XMLParser.XMLToSDBText(sdbDesign, sdbXDoc, sdbXResults, textSDBpath);
        //    return;
        //}







    }
}
