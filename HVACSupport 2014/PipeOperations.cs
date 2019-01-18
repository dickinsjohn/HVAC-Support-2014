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
    class PipeOperations
    {
        public static List<Element> pipeList = new List<Element>();

        //method to find all the parallel pipe to element
        public ICollection<ElementId> ParallelElements(Document m_doc, Element ele)
        {
            try
            {
                const double _eps = 1.0e-9;

                ICollection<ElementId> parallel = new List<ElementId>();

                Line eleLine = ((LocationCurve)ele.Location).Curve as Line;

                XYZ eleNormal = (eleLine.GetEndPoint(0) - eleLine.GetEndPoint(1)).CrossProduct(XYZ.BasisZ).Normalize();

                foreach (Element e in pipeList)
                {
                    if (ele != e)
                    {
                        Line tempLine = ((LocationCurve)e.Location).Curve as Line;

                        XYZ normal = (tempLine.GetEndPoint(0) - tempLine.GetEndPoint(1)).CrossProduct(XYZ.BasisZ).Normalize();

                        double angle = normal.AngleTo(eleNormal);

                        if ((_eps > angle || Math.Abs(angle - Math.PI) < _eps)
                            && eleLine.GetEndPoint(0).Z != tempLine.GetEndPoint(0).Z)
                        {
                            parallel.Add(e.Id);
                        }
                    }
                }
                return parallel;
            }
            catch
            {
                throw new Exception();
            }
        }

        //method to get the pipe insulation
        public double InsulationThickness(Pipe pipe)
        {
            try
            {
                Document doc = pipe.Document;

                FilteredElementCollector fec = new FilteredElementCollector(doc).OfClass(typeof(PipeInsulation));

                PipeInsulation pipeInsulation = null;

                foreach (PipeInsulation pi in fec)
                {
                    if (pi.HostElementId == pipe.Id)
                        pipeInsulation = pi;
                }

                return pipeInsulation.Thickness;
            }
            catch
            {
                throw new Exception();
            }
        }


    }
}
