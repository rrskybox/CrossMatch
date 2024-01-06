using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CrossMatch
{
    public class TargetData
    {
        public string TargetName { get; set; }
        public double TargetRA { get; set; }
        public double TargetDec { get; set; }
        public double? TargetMag { get; set; }
        public bool HasReference { get; set; }
        public string CrossRefName { get; set; } = "None";
        public string SIMBAD { get; set; } = "NA";

        public StarFinder.ReferenceData ReferenceStar = new StarFinder.ReferenceData();

    }

}

