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
    class Execution
    {
        static Document doc;

        string familyName;

        PipeOperations pipeOperations = new PipeOperations();

        double minSpacing;

        //constructor for the class
        public Execution()
        {
            doc = null;
        }

        //methdo to execute all the other methods
        public Execution(Document tempDoc, ElementSet tempSelected, string name, int minSpace)
        {
            doc = tempDoc;
            familyName = name;
            minSpacing = minSpace;

            ManageView view = new ManageView(doc);

            Intersectors.view3D = ManageView.view3D;

            PipeOperations.pipeList = ConverToList(tempSelected);

            CreateAndPlace createAndPlace = new CreateAndPlace(doc);

            using (TransactionGroup tx = new TransactionGroup(doc, "Support Generation"))
            {
                tx.Start();

                view.GetBoundsDetail();

                StartFromFirst();

                view.SetBounds();

                tx.Assimilate();
            }
        }

        //method o find the number of pipes parallel to a pipe
        private void StartFromFirst()
        {
            List<XYZ> placementPoints = null;
            Element current =PipeOperations.pipeList.First();

            placementPoints = Points.PlacementPoints(((LocationCurve)current.Location).Curve, minSpacing);

            
            ICollection<ElementId> parallels = pipeOperations.ParallelElements(doc, current);

            List<Element> intersectedParallel = new List<Element>();

            foreach (XYZ point in placementPoints)
            {
                intersectedParallel = Intersectors.ParallelElements(point, doc, parallels);


                //edit intersector here
            }
        }


        //methdo to convert element set into a list of elements with pipes only
        private List<Element> ConverToList(ElementSet eSet)
        {
            try
            {
                List<Element> eleList = new List<Element>();

                foreach (Element e in eSet)
                {
                    if (e is Pipe)
                    {
                        eleList.Add(e);
                    }
                }
                return RemoveVertical(eleList);
            }
            catch
            {
                throw new Exception();
            }
        }

        //method to remove all vertical elements from list
        private List<Element> RemoveVertical(List<Element> inList)
        {
            List<Element> outlist = new List<Element>();

            try
            {
                foreach (Element e in inList)
                {
                    Curve curve = ((LocationCurve)e.Location).Curve;

                    if (curve.GetEndPoint(0).X != curve.GetEndPoint(1).X && curve.GetEndPoint(0).Y != curve.GetEndPoint(1).Y)
                        outlist.Add(e);
                }

                return outlist;
            }
            catch
            {
                throw new Exception();
            }
        }
    }
}
