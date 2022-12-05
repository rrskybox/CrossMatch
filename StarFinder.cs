using System;
using System.Collections.Generic;
using System.Linq;
using TheSky64Lib;

namespace GaiaReferral
{
    public class StarFinder
    {
        public static void SetStarChart(double ra, double dec)
        {
            const double imageWidthInArcSec = 30 * 60;

            //Sets Starchart to center on target with 30 arcmin width
            sky6StarChart tsxsc = new sky6StarChart();
            //Turn on star display -- otherwise clickfind doesn't work
            tsxsc.SetDisplayProperty(Sk6DisplayPropertyObjectType.OT6_STAR,
                                     Sk6DisplayPropertySkyMode.sk6DisplayPropertySkyModeChartMode,
                                     Sk6DisplayProperty.sk6DisplayPropertyVisible,
                                     Sk6DisplayPropertyItem.sk6DisplayPropertyItemVisibleValue,
                                     1);

            tsxsc.RightAscension = ra;
            tsxsc.Declination = dec;
            tsxsc.FieldOfView = (imageWidthInArcSec * 1.5) / 3600; //in degrees
            return;
        }

        public static (double, double) LookupClickFind()
        {
            //Set TSX objects -- starchart and object information
            sky6StarChart tsxsc = new sky6StarChart();
            sky6ObjectInformation tsxoi = new sky6ObjectInformation();

            //Get "FInd" celestial location for mouse click
            tsxoi.Property(Sk6ObjectInformationProperty.sk6ObjInfoProp_RA_2000);
            double ra = tsxoi.ObjInfoPropOut;
            tsxoi.Property(Sk6ObjectInformationProperty.sk6ObjInfoProp_DEC_2000);
            double dec = tsxoi.ObjInfoPropOut;
            return (ra, dec);
        }

        public static List<ReferenceData> FindNearbyStars(double ra, double dec)
        {
            //Set TSX objects -- starchart and object information
            sky6StarChart tsxsc = new sky6StarChart();
            sky6ObjectInformation tsxoi = new sky6ObjectInformation();
            //Look up star(s) at the celestial location
            tsxsc.EquatorialToStarChartXY(ra, dec);
            double scX = tsxsc.dOut0;
            double scY = tsxsc.dOut1;
            tsxsc.ClickFind((int)scX, (int)scY);
            //Read in each star object and save name, ra and dec, magnitude and id's
            List<ReferenceData> sdList = new List<ReferenceData>();
            for (int rIdx = 0; rIdx < tsxoi.Count; rIdx++)
            {
                ReferenceData sd = new ReferenceData();
                tsxoi.Index = rIdx;
                tsxoi.Property(Sk6ObjectInformationProperty.sk6ObjInfoProp_NAME1);
                sd.ReferenceName = tsxoi.ObjInfoPropOut;
                tsxoi.Property(Sk6ObjectInformationProperty.sk6ObjInfoProp_RA_2000);
                sd.RA = tsxoi.ObjInfoPropOut;
                tsxoi.Property(Sk6ObjectInformationProperty.sk6ObjInfoProp_DEC_2000);
                sd.Dec = tsxoi.ObjInfoPropOut;
                tsxoi.Property(Sk6ObjectInformationProperty.sk6ObjInfoProp_MAG);
                sd.Magnitude = tsxoi.ObjInfoPropOut;
                tsxoi.Property(Sk6ObjectInformationProperty.sk6ObjInfoProp_CATALOGID);
                sd.CatalogId = tsxoi.ObjInfoPropOut;
                if (sd.CatalogId.Contains("APASS"))
                {
                    tsxoi.Property(Sk6ObjectInformationProperty.sk6ObjInfoProp_DB_FIELD_7);
                    sd.CatalogId = tsxoi.ObjInfoPropOut.ToString();
                }
                if (!sd.ReferenceName.Contains("Mouse"))
                    sdList.Add(sd);
            }
            return sdList;
        }

        public static ReferenceData? FindNearestByCatalog(List<ReferenceData> sdList, string catalog, double? magnitude, double magDiff)
        {
            //Get all stars from Gaia catalog whose magnitude is near the desired magnitude
            List<StarFinder.ReferenceData> closeMagList = sdList.FindAll(x => x.ReferenceName.Contains(catalog) && IsMagnitudeClose(x.Magnitude, magnitude, magDiff));
            //Find the closest of the set
            ReferenceData sd = new ReferenceData();
            if (closeMagList.Count > 0)
            {
                double minSep = closeMagList.Min(x => x.Separation);
                sd = closeMagList.First(x => x.Separation == minSep);
                return sd;
            }
            return null;
        }

        public static bool IsMagnitudeClose(double? mag1, double? mag2, double magDiff)
        {
            //Determines if mag1 is within a magnitude of mag2
            //  if mag1 is null then return true anyway
            if (mag1 != null && mag2 != null && Math.Abs((double)mag2 - (double)mag1) <= magDiff)
                return true;
            else
                return false;
        }

        public static List<ReferenceData> ComputeAllSeparations(List<ReferenceData> sdList, double ra2, double dec2)
        {
            sky6Utils tsxu = new sky6Utils();
            List<ReferenceData> sdOutList = new List<ReferenceData>();
            foreach (ReferenceData sd in sdList)
            {
                ReferenceData sdOut = new ReferenceData();
                sdOut = sd;
                tsxu.ComputeAngularSeparation(sdOut.RA, sdOut.Dec, ra2, dec2);
                sdOut.Separation = tsxu.dOut0 * 3600;
                sdOutList.Add(sdOut);
            }
            return sdOutList;
        }

        public class ReferenceData
        {
            public string ReferenceName;
            public string CatalogId;
            public double RA = 0;
            public double Dec = 0;
            public double? Magnitude = 0;
            public string FieldId;
            public double NormalRA;  //in arcsec
            public double NormalDec;  //in arcsec
            public double Separation;  //in arcsec
            public double MinFOV = 0.01;
            public double MaxFOV = 360;
        }

    }
}
