using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino;
using Rhino.DocObjects;
using System;

namespace QuickSplit
{
    public class SurfaceUtil
    {
        public static (Brep originalBrep, ObjRef surfaceRef, Plane plane) GetSurfaceAndPlane(RhinoDoc doc)
        {
            ObjRef surfaceRef;
            Result res = RhinoGet.GetOneObject("Select a Surface", false, ObjectType.Surface, out surfaceRef);

            if (res != Result.Success)
            {
                if (res == Result.Cancel)
                    throw new OperationCanceledException("Surface selection was cancelled by user.");
                throw new InvalidOperationException("Surface selection failed.");
            }


            if (surfaceRef == null)
                throw new ArgumentNullException(nameof(surfaceRef), "No surface reference was returned.");

            BrepFace originalFace = surfaceRef.Face();
            if (!originalFace.TryGetPlane(out Plane plane))
            {
                throw new Exception("Could not extract a plane from the Brep face.");
            }

            Brep originalBrep = surfaceRef.Brep();
            if (originalBrep == null)
                throw new InvalidOperationException("Could not convert selected surface to a Brep.");


            if (!originalBrep.IsValid)
                throw new InvalidOperationException("The extracted Brep is not valid.");


            return (originalBrep, surfaceRef, plane);
        }
    }
}