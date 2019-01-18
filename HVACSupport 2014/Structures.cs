using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Structure;

namespace HVACSupport_2014
{
    //structure to store specification data from file
    public struct SpecificationData
    {
        public string selectedFamily;
        public string discipline;
        public int minSpacing;
        public string supportType;
        public string specsFile;
    };

    //intersection points for each point on duct/tray
    public struct ReferPoints
    {
        public XYZ left;
        public XYZ right;
    };

    //structure to store paralel tray and points placed 
    public struct CompletedTrays
    {
        public int init;
        public ElementId eId;
        public List<XYZ> points;

        public CompletedTrays(int initializer)
        {
            init = initializer;
            eId = null;
            points = new List<XYZ>();
        }
    };

}
