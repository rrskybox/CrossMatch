/*
* Methods for creating and saving reference data in SDB text input format
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
using System.Linq;
using System.Xml.Linq;
using System.Windows.Forms;
using System.IO;

namespace CrossMatch
{
    public class SDBDesigner
    {
        #region xml names

        public const string TheSkyDatabaseX = "TheSkyDatabase";
        public const string SDBHeaderX = "TheSkyDatabaseHeader";

        public const string IdentifierX = "identifier";
        public const string SDBDescriptionX = "sdbDescription";

        //Control Data Fields
        public const string SearchPrefixX = "searchPrefix";
        public const string SpecialSDBX = "specialSDB";
        public const string PlotObjectsX = "plotObjects";
        public const string PlotLabelsX = "plotLabels";
        public const string PlotOrderX = "plotOrder";
        public const string SearchableX = "searchable";
        public const string ClickIdentifyX = "clickIdentify";
        public const string EpochX = "epoch";
        public const string ReferenceFrameX = "referenceFrame";
        public const string CrossReferenceTypeX = "crossReferenceType";
        public const string DefaultMaxFOVX = "defaultMaxFOV";
        public const string DefaultObjectTypeX = "defaultObjectType";
        public const string DefaultObjectTypeIndexX = "index";
        public const string DefaultObjectTypeDescriptionX = "description";

        //Built-in Data Fields
        public const string RAHoursX = "raHours";
        public const string RAMinutesX = "raMinutes";
        public const string RASecondsX = "raSeconds";
        public const string DecSignX = "decSign";
        public const string DecDegreesX = "decDegrees";
        public const string DecMinutesX = "decMinutes";
        public const string DecSecondsX = "decSeconds";
        public const string MagnitudeX = "magnitude";
        public const string CrossReferenceX = "crossReference";
        public const string LabelOrSearchX = "labelOrSearch";
        public const string ObjectTypeX = "objectType";
        public const string MajorAxisX = "majorAxis";
        public const string MinorAxisX = "minorAxis";
        public const string PositionAngleX = "positionAngle";
        public const string ScaleX = "scale";
        public const string DrawCommandX = "drawCommand";
        public const string ToggleGroupX = "toggleGroup";
        public const string FileNameX = "fileName";
        public const string MinimumFOVX = "minimumFOV";
        public const string MaximumFOVX = "maximumFOV";
        public const string RAMultiplierX = "raMultiplier";
        public const string SampleColumnHeaderX = "sampleColumnHeader";
        public const string MinFOVX = "minimumFOV";
        public const string MaxFOVX = "maximumFOV";

        //Custom data fields
        const string UserFieldX = "userField";
        //Field attributes
        const string IndexX = "index";
        const string DescriptionX = "description";
        const string ColBegX = "colBeg";
        const string ColEndX = "colEnd";
        const string FieldIDX = "fieldID";

        #endregion

        const int DefaultObjectIndex = 55;
        const string DefaultObjectDescription = "Reference Point";

        public List<DataColumn> HeaderMap = new List<DataColumn>();

        public List<ControlDesc> ControlFields = new List<ControlDesc>()
        {
            //List of values for generating TSX SDB fields
            //  that are used for general conversion to and display of the
            //  display in the "Find" function
            //
            new ControlDesc {ControlName = IdentifierX, ControlValue= "Star Labels"},
            new ControlDesc {ControlName = SDBDescriptionX, ControlValue  = ""},
            new ControlDesc {ControlName = SearchPrefixX, ControlValue = "" },
            new ControlDesc {ControlName = SpecialSDBX, ControlValue = "0" },
            new ControlDesc {ControlName = PlotObjectsX, ControlValue = "0" },
            new ControlDesc {ControlName = PlotLabelsX, ControlValue = "1" },
            new ControlDesc {ControlName = PlotOrderX, ControlValue = "0" },
            new ControlDesc {ControlName = SearchableX, ControlValue = "1" },
            new ControlDesc {ControlName = ClickIdentifyX, ControlValue = "1" },
            new ControlDesc {ControlName = EpochX, ControlValue = "2000.0" },
            new ControlDesc {ControlName = ReferenceFrameX, ControlValue = "0" },
            new ControlDesc {ControlName = CrossReferenceTypeX, ControlValue = "1" },
            new ControlDesc {ControlName = DefaultMaxFOVX, ControlValue = "360.0000" }
            //new ControlDesc {ControlName = RAMultiplierX , ControlValue = "1.0" },
        };
        public string SearchPrefix
        {
            set { ControlFields.Single(c => c.ControlName == SearchPrefixX).ControlValue = value; }
        }

        public void MakeHeaderMap(List<TargetData> tgData, bool IsIAU)
        {
            //create a map of datafields to textcolumns
            //for the tns xml data file to be convert to a sdb.text file
            HeaderMap = new List<DataColumn>();
            //Create Headers for columns
            string col0;
            string col1;
            string col2;
            string col3;
            string col4;
            string col5;
            string col6;
            if (IsIAU)
            {
                col0 = "IAUName";
                col1 = "Reference";
                col2 = "ReferenceRA";
                col3 = "ReferenceDec";
                col4 = "ReferenceMag";
                col5 = "MaxFOV";
                col6 = "MinFOV";
            }
            else
            {
                col0 = "Name";
                col1 = "NA";
                col2 = "RA";
                col3 = "Dec";
                col4 = "Mag";
                col5 = "MaxFOV";
                col6 = "MinFOV";
            }


            HeaderMap.Add(new DataColumn()
            {
                SourceDataName = col0,
                ColumnWidth = tgData.Max(x => x.TargetName.Length)
            });
            HeaderMap.Add(new DataColumn()
            {
                SourceDataName = col1,
                ColumnWidth = tgData.Max(x => x.CrossRefName.Length)
            });

            HeaderMap.Add(new DataColumn()
            {
                SourceDataName = col2,
                ColumnWidth = tgData.Max(x => x.ReferenceStar.RA.ToString().Length)
            });

            HeaderMap.Add(new DataColumn()
            {
                SourceDataName = col3,
                ColumnWidth = tgData.Max(x => x.ReferenceStar.Dec.ToString().Length)
            });

            HeaderMap.Add(new DataColumn()
            {
                SourceDataName = col4,
                ColumnWidth = tgData.Max(x => x.ReferenceStar.Magnitude.ToString().Length)
            });

            //Add fake FOV columns
            HeaderMap.Add(new DataColumn()
            {
                SourceDataName = col5,
                ColumnWidth = tgData.Max(x => x.ReferenceStar.MinFOV.ToString("0.00").Length)
            });

            HeaderMap.Add(new DataColumn()
            {
                SourceDataName = col6,
                ColumnWidth = tgData.Max(x => x.ReferenceStar.MaxFOV.ToString("0.00").Length)
            });


        }

        public List<DataColumn> ResultstoSDBHeader()
        {
            //Create a TSXSDB formatter to work with
            //  this object will include a standard set of control fields
            //  and empty sets of builtin and user data fields
            //tsxSDBdesign = new SDBDesigner();
            //Stick with the standard set of control fields
            //Map the tns fields on to the tsx built-in and user-defined fields
            //  keeping track of the start of the column
            const int fieldPad = 4;

            List<DataColumn> dataFields = new List<DataColumn>();
            int fieldStart = 1;
            int fieldWidth = 0;
            foreach (DataColumn sb in HeaderMap)
            {
                string fieldName = sb.SourceDataName;
                fieldWidth = sb.ColumnWidth;
                sb.TSXEntryName = fieldName;
                sb.ColumnStart = fieldStart;
                sb.ColumnWidth = fieldWidth + fieldPad;

                switch (fieldName)
                {
                    case "IAUName":
                        sb.TSXEntryName = LabelOrSearchX; //1
                        sb.IsBuiltIn = true;
                        sb.IsPassed = true;
                        dataFields.Add(sb);
                        fieldStart += sb.ColumnWidth;
                        break;
                    case "Reference": //2
                        sb.TSXEntryName = CrossReferenceX;
                        sb.IsBuiltIn = true;
                        sb.IsPassed = true;
                        dataFields.Add(sb);
                        fieldStart += sb.ColumnWidth;
                        break;
                    case "ReferenceRA":  //4
                        sb.TSXEntryName = RAHoursX;
                        sb.IsBuiltIn = true;
                        sb.IsPassed = true;
                        dataFields.Add(sb);
                        fieldStart += sb.ColumnWidth;
                        break;
                    case "ReferenceDec": //5
                        sb.TSXEntryName = DecDegreesX;
                        sb.IsBuiltIn = true;
                        sb.IsPassed = true;
                        dataFields.Add(sb);
                        fieldStart += sb.ColumnWidth;
                        break;
                    case "ReferenceMag":  //6
                        sb.TSXEntryName = MagnitudeX;
                        sb.IsBuiltIn = true;
                        sb.IsPassed = true;
                        dataFields.Add(sb);
                        fieldStart += sb.ColumnWidth;
                        break;
                    case "Name":
                        sb.TSXEntryName = LabelOrSearchX; //1
                        sb.IsBuiltIn = true;
                        sb.IsPassed = true;
                        dataFields.Add(sb);
                        fieldStart += sb.ColumnWidth;
                        break;
                    case "RA":  //4
                        sb.TSXEntryName = RAHoursX;
                        sb.IsBuiltIn = true;
                        sb.IsPassed = true;
                        dataFields.Add(sb);
                        fieldStart += sb.ColumnWidth;
                        break;
                    case "Dec": //5
                        sb.TSXEntryName = DecDegreesX;
                        sb.IsBuiltIn = true;
                        sb.IsPassed = true;
                        dataFields.Add(sb);
                        fieldStart += sb.ColumnWidth;
                        break;
                    default:
                        sb.IsPassed = false;
                        break;
                }
            }
            //Add two more columns for FOV

            DataColumn minsb = new DataColumn()
            {
                TSXEntryName = MinFOVX,
                ColumnStart = fieldStart,
                ColumnWidth = fieldWidth + fieldPad,
                IsBuiltIn = true,
                IsPassed = true
            };
            fieldStart += minsb.ColumnWidth;
            dataFields.Add(minsb);

            DataColumn maxsb = new DataColumn()
            {
                TSXEntryName = MaxFOVX,
                ColumnStart = fieldStart,
                ColumnWidth = fieldWidth + fieldPad,
                IsBuiltIn = true,
                IsPassed = true
            };
            dataFields.Add(maxsb);
            //

            return dataFields;
        }

        public void SDBToClipboard(List<TargetData> tgtDataList, bool IsIAU)
        {
            //Add the header
            //Build and add text lines according to the header
            //Write the XDocument to the text file
            MakeHeaderMap(tgtDataList, IsIAU);
            List<DataColumn> dataFields = ResultstoSDBHeader();
            string sdbSection = SDBHeaderGenerator(dataFields).ToString();
            //Write the sdb text according to the column map
            foreach (TargetData tgtData in tgtDataList)
                sdbSection += "\n" + SDBtoTextLine(tgtData, dataFields, IsIAU);
            Clipboard.SetText(sdbSection);
            return;
        }

        public void SDBToCSVFile(List<TargetData> tgtDataList, string fileName, bool IsIAU)
        {
            //Do all the same things as clipboard, but write it to a file *sdb.csv

            MakeHeaderMap(tgtDataList, IsIAU);
            List<DataColumn> dataFields = ResultstoSDBHeader();
            string sdbSection = SDBHeaderGenerator(dataFields).ToString();
            //Write the sdb text according to the column map
            foreach (TargetData tgtData in tgtDataList)
                sdbSection += "\n" + SDBtoTextLine(tgtData, dataFields, IsIAU);
            //open file for writing
            string filePath = fileName.Replace(".csv", ".sdb.txt");
            if (File.Exists(filePath))
                File.Delete(filePath);
            File.WriteAllText(filePath, sdbSection);
            return;
        }

        private string SDBtoTextLine(TargetData tgtData, List<DataColumn> dataFields, bool IsIAU)
        {
            if (IsIAU)
                return
                    tgtData.TargetName.PadRight(dataFields[0].ColumnWidth) +
                    tgtData.CrossRefName.PadRight(dataFields[1].ColumnWidth) +
                    tgtData.ReferenceStar.RA.ToString().PadRight(dataFields[2].ColumnWidth) +
                    tgtData.ReferenceStar.Dec.ToString().PadRight(dataFields[3].ColumnWidth) +
                    tgtData.ReferenceStar.Magnitude.ToString().PadRight(dataFields[4].ColumnWidth) +
                    tgtData.ReferenceStar.MinFOV.ToString("0.00").PadRight(dataFields[5].ColumnWidth) +
                    tgtData.ReferenceStar.MaxFOV.ToString("0.00").PadRight(dataFields[6].ColumnWidth);
            else
                return
                    tgtData.TargetName.PadRight(dataFields[0].ColumnWidth) +
                    tgtData.TargetRA.ToString().PadRight(dataFields[2].ColumnWidth) +
                    tgtData.TargetDec.ToString().PadRight(dataFields[3].ColumnWidth);
        }

        public XDocument SDBHeaderGenerator(List<DataColumn> dataFields)
        {
            //Catalog Fields
            XElement xHeader = new XElement(SDBHeaderX, new XAttribute("version", "1.00"));
            foreach (ControlDesc cf in ControlFields)
            { xHeader.Add(new XElement(cf.ControlName, cf.ControlValue)); }
            //Default Object type -- may fix this up later
            XElement dotx = new XElement(DefaultObjectTypeX,
                    new XAttribute(IndexX, DefaultObjectIndex),
                    new XAttribute(DescriptionX, DefaultObjectDescription));
            xHeader.Add(dotx);
            //Column definitions
            //Custom fields
            foreach (DataColumn dc in dataFields)
            {
                if (dc.IsBuiltIn)
                    xHeader.Add(BuiltInFieldGen(dc));
                else
                    xHeader.Add(CustomFieldGen(dc));
            }
            //Add header
            XDocument xdoc = new XDocument(
                new XDeclaration("1.0", null, null),
                new XDocumentType(TheSkyDatabaseX, null, null, null),
                xHeader);
            return xdoc;
        }

        public XElement BuiltInFieldGen(DataColumn fc)
        {
            return new XElement(fc.TSXEntryName,
                        new XAttribute(ColBegX, fc.ColumnStart.ToString()),
                        new XAttribute(ColEndX, Utility.ColumnEnd(fc.ColumnStart, fc.ColumnWidth)));
        }

        public XElement CustomFieldGen(DataColumn uc)
        {
            return new XElement(UserFieldX,
                        new XAttribute(FieldIDX, uc.SourceDataName),
                         new XAttribute(ColBegX, uc.ColumnStart.ToString()),
                       new XAttribute(ColEndX, Utility.ColumnEnd(uc.ColumnStart, uc.ColumnWidth)));
        }

        public class DataColumn
        {
            public bool IsBuiltIn = false;

            public string? TSXEntryName = null;
            public string? SourceDataName { get; set; } = null;
            public int ColumnStart { get; set; } = 0;
            public int ColumnWidth { get; set; } = 0;
            public bool IsPassed { get; set; } = false;
            public bool IsDuplicate { get; set; } = false;
        }

        public class ControlDesc
        {
            public string ControlName { get; set; }
            public string ControlValue { get; set; }
        }



    }
}

