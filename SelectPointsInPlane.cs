using Rhino.Geometry;
using Rhino;
using Rhino.Input.Custom;
using Rhino.Input;
using System;
using System.Collections.Generic;

namespace QuickSplit
{
    public class SelectPointsInPlane
    {
        public static (List<Point3d> selectedPoints, List<Guid> pointGuids, List<Guid> lineGuids) SelectPoints(RhinoDoc doc, Rhino.Geometry.Plane plane)
        {

            List<Point3d> selectedPoints = new List<Point3d>();
            List<Guid> pointGuids = new List<Guid>();
            List<Guid> lineGuids = new List<Guid>();

            while (true)
            {
                GetPoint gp = new GetPoint();
                gp.SetCommandPrompt("Select points on the plane (Enter when done)");
                gp.Constrain(plane, true);
                GetResult getResult = gp.Get();

                if (getResult == GetResult.Point)
                {

                    Point3d point = gp.Point();
                    Point3d projectedPoint = plane.ClosestPoint(point);
                    selectedPoints.Add(projectedPoint);
                    Guid guid = doc.Objects.AddPoint(point);
                    if (guid != Guid.Empty)
                    {
                        pointGuids.Add(guid);
                    }

                    if (selectedPoints.Count > 1)
                    {
                        Line line = new Line(selectedPoints[selectedPoints.Count - 2], selectedPoints[selectedPoints.Count - 1]);
                        Guid lineGuid = doc.Objects.AddLine(line);
                        if (lineGuid != Guid.Empty)
                        {
                            lineGuids.Add(lineGuid);
                        }
                    }
                    doc.Views.Redraw();
                }
                else if (getResult == GetResult.Nothing)
                {
                    if (selectedPoints.Count < 3)
                        throw new InvalidOperationException("Please select at least 3 points");

                    Line closingLine = new Line(selectedPoints[selectedPoints.Count - 1], selectedPoints[0]);
                    Guid closingLineGuid = doc.Objects.AddLine(closingLine);
                    if (closingLineGuid != Guid.Empty)
                    {
                        lineGuids.Add(closingLineGuid);
                    }

                    doc.Views.Redraw();
                    break;
                }
                else
                {
                    break;
                }
            }

            RhinoApp.WriteLine($"Selected {selectedPoints.Count} points:");
            foreach (Point3d pt in selectedPoints)
            {
                RhinoApp.WriteLine(pt.ToString());
            }

            if (selectedPoints.Count < 3)
                throw new InvalidOperationException("Please select at least 3 points");

            selectedPoints.Add(selectedPoints[0]);

            return (selectedPoints, pointGuids, lineGuids);
        }
    }
}
