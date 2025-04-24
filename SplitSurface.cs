using Rhino.Geometry;
using Rhino;
using System;

namespace QuickSplit
{
    public class SplitSurface
    {
        public static Brep[] Split(RhinoDoc doc, Surface surface, Curve curve)
        {
            Brep originalBrep = surface.ToBrep();
            if (originalBrep == null)
            {
                throw new Exception("Failed to convert surface to Brep.");
            }

            Brep[] splitBreps;
            try
            {
                splitBreps = originalBrep.Split(new Curve[] { curve }, doc.ModelAbsoluteTolerance);
            }
            catch (Exception ex)
            {
                throw new Exception("Error during Brep splitting", ex);
            }

            if (splitBreps == null || splitBreps.Length == 0)
            {
                throw new Exception("No Breps resulted from splitting.");
            }


            return splitBreps;
        }
    }
}
