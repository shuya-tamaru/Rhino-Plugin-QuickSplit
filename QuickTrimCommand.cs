using System;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;

namespace QuickSplit
{
    public class QuickSplitCommand : Command
    {
        public QuickSplitCommand()
        {
            Instance = this;
        }

        public static QuickSplitCommand Instance { get; private set; }

        public override string EnglishName => "QuickSplit";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {

            try
            {
                var (originalBrep, surfaceRef, plane) = SurfaceUtil.GetSurfaceAndPlane(doc);
                var (selectedPoints, pointGuids, lineGuids) = SelectPointsInPlane.SelectPoints(doc, plane);
                var (curve, curveGuid) = CreateCutter.GetCutterCurve(doc, selectedPoints);

                Brep cutterBrep = Brep.CreatePlanarBreps(curve, doc.ModelAbsoluteTolerance)[0];
                Brep[] splitBreps = originalBrep.Split(cutterBrep, doc.ModelAbsoluteTolerance);


                if (splitBreps != null && splitBreps.Length > 0)
                {
                    foreach (Brep resultBrep in splitBreps)
                    {
                        doc.Objects.AddBrep(resultBrep);
                    }

                }

                CleanUpDocument.DeleteObjects(doc, pointGuids, lineGuids, curveGuid, surfaceRef);

                doc.Views.Redraw();
                RhinoApp.WriteLine("Finish");
                return Result.Success;
            }
            catch (OperationCanceledException ex)
            {
                RhinoApp.WriteLine($"Operation cancelled: {ex.Message}");
                return Result.Cancel;
            }
            catch (ArgumentNullException ex)
            {
                RhinoApp.WriteLine($"Invalid input: {ex.Message}");
                return Result.Failure;
            }
            catch (InvalidOperationException ex)
            {
                RhinoApp.WriteLine($"Operation failed: {ex.Message}");
                return Result.Failure;
            }
            catch (Exception ex)
            {
                RhinoApp.WriteLine($"Unexpected error: {ex.Message}");
                return Result.Failure;
            }

        }
    }
}
