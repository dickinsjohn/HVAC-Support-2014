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
    class CreateAndPlace
    {
        static Document doc;
        string singlePipe =null , parallelPipe=null;
        Family family = null;
        static FamilySymbol singlePipeSymbol = null;
        static FamilySymbol parallelPipeSymbol = null;

        //constructors
        public CreateAndPlace()
        {
            doc = null;
        }

        public CreateAndPlace(Document d)
        {
            doc = d;

            family = FindElementByName(singlePipe) as Family;
            singlePipeSymbol = GetFamilySymbol();

            family = FindElementByName(parallelPipe) as Family;
            parallelPipeSymbol = GetFamilySymbol();
        }


        //method to get family symbol
        private FamilySymbol GetFamilySymbol()
        {
            FamilySymbol symbol = null;
            try
            {
                foreach (FamilySymbol s in family.Symbols)
                {
                    symbol = s;
                    break;
                }

                return symbol;
            }
            catch
            {
                TaskDialog.Show("Error!", "Please load Family and try again.");
                throw new Exception();
            }
        }

        //method to find the element by family name
        private Element FindElementByName(string targetName)
        {
            try
            {
                return new FilteredElementCollector(doc).OfClass(typeof(Family))
                    .FirstOrDefault<Element>(e => e.Name.Equals(targetName));
            }
            catch
            {
                throw new Exception();
            }
        }

        //method to create an element
        public static Element CreateElement(XYZ point, Element host, Level lvl)
        {
            try
            {
                return doc.Create.NewFamilyInstance(point, parallelPipeSymbol, host, lvl, StructuralType.NonStructural);
            }
            catch
            {
                throw new Exception();
            }
        }
    }
}
