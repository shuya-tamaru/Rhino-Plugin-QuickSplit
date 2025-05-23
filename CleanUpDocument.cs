﻿using System;
using System.Collections.Generic;
using Rhino;
using Rhino.DocObjects;

namespace QuickSplit
{
    public class CleanUpDocument
    {
        public static void DeleteObjects(RhinoDoc doc, List<Guid> pointGuids,List<Guid> lineGuids, Guid curveGuid, ObjRef surfaceRef)
        {
            foreach (Guid guid in pointGuids)
            {
                doc.Objects.Delete(guid, true);
            }

            foreach (Guid guid in lineGuids)
            {
                doc.Objects.Delete(guid, true);
            }

            if (surfaceRef.Object() != null)
            {
                doc.Objects.Delete(surfaceRef.Object(), true);
            }

            if (curveGuid != Guid.Empty)
            {
                doc.Objects.Delete(curveGuid, true);
            }
        }
    }
}

