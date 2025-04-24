using Rhino.Geometry;
using Rhino;
using System;
using System.Collections.Generic;


namespace QuickTrim
{
    public class CreateCutter
    {
        public static (Curve curve, Guid curveGuid) GetCutterCurve(RhinoDoc doc, List<Point3d> selectedPoints)
        {
            Polyline polyline = new Polyline(selectedPoints);
            if (!polyline.IsValid)
                throw new InvalidOperationException("Invalid Points selection");

            Curve curve = polyline.ToNurbsCurve();
            if (curve == null)
                throw new InvalidOperationException("Failed to convert polyline to curve");

            if (!curve.IsValid)
                throw new InvalidOperationException("Created curve is not valid");

            Guid curveGuid = doc.Objects.AddCurve(curve);

            return (curve, curveGuid);
        }
    }
}
