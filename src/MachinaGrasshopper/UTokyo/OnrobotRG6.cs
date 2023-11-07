using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Machina;
using Rhino.Geometry;

namespace MachinaGrasshopper.ToolAction
{
    public class OnrobotRG6 : GH_Component
    {
        bool relative = false;
        
        /// <summary>
        /// Initializes a new instance of the OnrobotRG6 class.
        /// </summary>
        public OnrobotRG6() : base(
            "OnrobotRG6",
            "RG6",
            "Controls Onrobot RG6 two finger Gripper",
            "Machina",
            "UTokyo")
        {
        }

        //public override GH_Exposure Exposure => GH_Exposure.quinary;
        public override Guid ComponentGuid => new Guid("EBE070BF-9E3D-4F64-8E0E-FFDD8DA1379B");
        protected override System.Drawing.Bitmap Icon => Properties.Resources.ToolAction_RG6;

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {

            pManager.AddNumberParameter("fingerDistance", "distance", "Gripper Finger Distance in (mm)", GH_ParamAccess.item, 75);
            pManager.AddNumberParameter("objectWeight", "weight", "the weight of the picked up object in (kg)", GH_ParamAccess.item, 0);
            pManager.AddTextParameter("runMode", "mode", "'inplace' for stationary gripper finger action. \n" +
                                                       "'moving for finger action not stopping while the robot moves.", GH_ParamAccess.item, "inplace");
            //pManager.AddMeshParameter("mesh", "mesh", "The mesh geometry of the object you are gripping", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Action", "A", "OnrobotRG6 Action", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double distance = 0;
            double weight = 0;
            string mode = "inplace";

            if (!DA.GetData(0, ref distance)) return;
            if (!DA.GetData(1, ref weight)) return;
            if (!DA.GetData(2, ref mode)) return;

            GripperRunStop runMode = mode == "inplace" ? GripperRunStop.Inplace : GripperRunStop.Moving;

            DA.SetData(0, new ActionRG6Gripper(GripperType.Analouge, distance, weight, runMode, relative, GenerateMeshGeometry(distance)));

            this.Message = relative ? "Relative" : "Absolute";
        }


        Mesh GenerateMeshGeometry(double distance) 
        {
            string meshData = "{\"version\":10000,\"archive3dm\":70,\"opennurbs\":-1904966255,\"data\":\"+n8CALsnAAAAAAAA+/8CABQAAAAAAAAA5NTXTkfp0xG/5QAQgwEi8C25G1z8/wIAgycAAAAAAAA49AEAAEABAADSHTOevfjl/9IdM569+OX/0h0znr345f/SHTOevfjl/zxuF9esIHrAqcnQxnFdI0Bch+Zo47UpwHCNLxLd4URAAAAAAAAAAAAAAAAAAAAAAHGJNMJxiTTC0koXqXGJNEJxiTRC7f0lQwAAgD8AAIA/AACAPwAAgL8AAIC/AACAvwAAgD8AAIA/AACAvwAAgL8BAAAAAAAAAAACAAAAFgARAAAAAAABABcAFgAAAAIAGQAYAAEAAwAbABoAAgABABgAFwAXAAMAHAAbABsAAgAaABkAGQAEABIAHAADAAUAHgAdAAQABgAgAB8ABQAIABMAIwAHAAkAJQAkAAgABwAiACEABgALACkAKAAKAAwAFAAqAAsADgAuAC0ADQAQABUAMQAPAA8AMAAvAA4ADQAsACsADAAKACcAJgAJAAQAHQASABIABgAhACAAIAAFAB8AHgAeAAcAIwAiACIACQAmACUAJQAIACQAEwATAAoAKAAnACcADAArABQAFAALACoAKQApAA0ALQAsACwADwAxADAAMAAOAC8ALgAuADcAMgBjAGMAOABjAGIAYgA5AGEAYABgADkAYABfAF8AYQA4AGIAYgA6AF4AXQBdADsAXQAzADMAPABcAFsAWwA8AFsAWgBaAFwAOwAzADMAXgA6AF8AXwA9AFkAWABYAD4AWABXAFcAPwBWADQANAA/ADQAVQBVAFYAPgBXAFcAQABUAFMAUwBBAFMAUgBSAEIAUQBQAFAAQwBPADUANQBPAEIAUABQAFEAQQBSAFIAVABAAFUAVQBZAD0AWgBaADgAYQA5ADkAOQBfADoAOgA7AFwAPAA8ADwAWgA9AD0AOgBdADsAOwA+AFYAPwA/AD8AVQBAAEAAQQBRAEIAQgBCAE8AQwBDAEAAUwBBAEEAPQBYAD4APgBEAE4ATQBNAEQATQBMAEwARQBLAEoASgBGAEoASQBJAEsARQBMAEwARABMAEUARQBFAEoARgBGAEcASAA2ADYASABHAEYARgBIAEYASQBJAEMATgBEAEQATgBDADUANQA3AGMAOAA4AGkAbQBoAGgAaQBqAGsAawBxAGUAaABoAGUAZgBnAGcAaABlAGcAZwBkAGUAcwBzAGkAawBtAG0AaABtAHEAcQBtAG4AbwBvAG8AcQBtAG0AcQByAHMAcwBxAHMAZQBlAHEAbwBwAHAAbABtAGsAawCCAH0AgQCBAIMAfQCCAIIAgwB6AH0AfQB/AIAAgQCBAH8AfQB+AH4AfwCBAH0AfQB1AIMAdAB0AIMAdgB6AHoAdgB3AHgAeAB4AHoAdgB2AHoAewB8AHwAegB8AH0AfQB6AHgAeQB5AHUAdgCDAIMAlACVAJYAhACFAIQAlgCWAI0AlgCVAJUAlgCXAIYAhQCMAIcAhgCGAIsAjACXAJcAhgCXAIwAjACKAIsAlwCXAJcAlgCPAJAAjQCOAJYAlgCQAJEAlwCXAI4AjwCWAJYAlwCSAJMAkwCSAJcAkQCRAJcAkwCKAIoAigCTAIkAiQCIAI0AlQCVAJoAmwCxAJgAnACdAK8AsACbAJwAsACxAJ4AnwCtAK4AoAChAKsArACfAKAArACtAJ0AngCuAK8AqgCrAKEAogCpAKoAogCyALMAtACnAKgAsgCzAKgAqQC1ALYApQCmALcAuACjAKQAtgC3AKQApQC0ALUApgCnAJkAowC4ALkAyADJAL0AvQDHAMgAvgC+AMoAvADJAMkAxgDAAMUAxQDFAMEAxADEAMcAvwDGAMYAuwDKALoAugC9AL4AyADIAMkAvAC9AL0AxwC+AL8AvwDCAMMAxADEAMQAwQDCAMIAwQDFAMAAwADGAL8AwADAALsAvADKAMoA0QDMAMsAywDdANEA0gDSANMA3QDSANIAzADRAN0A3QDNAMwA3QDdAN4AzgDNAM0A3ADPAM4A3gDeANoA3ADcAN4AzQDdAN0A3gDdANcA2ADUANMA0ADQANQA1QDdAN0A1QDWAN0A3QDdANYA1wDXANkA2gDeAN4A2gDbANwA3ADYANkA3gDeANMA1ADdAN0A5gDlAOEA4QDlAOQA4QDhAOAA5wDmAOYA4ADmAOEA4QDjAOIA5ADkAOIA4QDkAOQA3wDnAOAA4ADpAOsA6gDoAO0A7wDuAOwA8QDzAPIA8AD2APQA+AD4APUA9wD4APgA9gD4APcA9wD6APwA+wD5AP8A/QABAQEB/gAAAQEBAQH/AAEBAAEAAQIBAwEHAQcBBgEFAQcBBwEHAQQBAgECAQUBBAEHAQcBGAEIARoBGQEJARoBCAEIAREBGQEaARoBGgEJAQoBGwEQAQoBCwELAQ8BGwEQARABCgEQARsBGwEOARsBDwEPARsBFAETARoBEQEaARIBEgEUARsBFQEVARIBGgETARMBGwEXARYBFgEWARUBGwEbARsBDgEXARcBDgENARcBFwEMARkBEQERAR4BHAE1AR8BIAE0ATMBIQEfATUBNAEgASIBMgExASMBJAEwAS8BJQEjATEBMAEkASEBMwEyASIBLgEmASUBLwEtATYBJgEuATcBLAErATgBNgEtASwBNwE5ASoBKQE6ATsBKAEnATwBOgEpASgBOwE4ASsBKgE5AR0BPQE8AScBTAFBAU0BTQFLAUIBTAFMAU4BTQFAAUABSgFJAUQBRAFJAUgBRQFFAUsBSgFDAUMBPwE+AU4BTgFBAUwBQgFCAU0BQQFAAUABSwFDAUIBQgFGAUgBRwFHAUgBRgFFAUUBRQFEAUkBSQFKAUQBQwFDAT8BTgFAAUABUAFVAU8BTwFYAVYBUAFQAVABVgFVAVUBUQFbAVoBUAFTAV8BXgFSAVIBXQFcAVEBWAFXAVYBVgFXAVgBVAFUAVABWgFZAVkBUAFZAVgBWAFRAVwBWwFbAVIBXgFdAV0BZwFiAWYBZgFmAWIBZQFlAWEBZwFoAWgBYQFiAWcBZwFkAWUBYwFjAWMBZQFiAWIBYAFhAWgBaAFqAWkBawFsAW4BbQFvAXABcgFxAXMBdAF4AXYBdQF1AXcBdgF5AXkBeAF5AXYBdgF7AXoBfAF9AX8BfgGAAYEBggGHAYMBgwGGAYcBhQGFAYcBggGEAYQBhQGHAYQBhAGIAYoBiwGJAYwBjgGPAY0BkAGSAZMBkQGUAZYBlwGVAZgBmgGbAZkBnAGeAZ8BnQGgAaIBowGhAaQBpgGnAaUBqAGqAasBqQGsAa4BrwGtAbABsgGzAbEBtAG2AbcBtQG4AboBuwG5AbwBvgG/Ab0BwAHCAcMBwQHEAcYBxwHFAcgBygHLAckBzAHOAc8BzQHaAeEB4AHbAdsB3QHcAdwB2gHZAeEB4QHZAdgB1wHXAdYB1QHZAdkB1AHQAdkB1QHXAdYB2QHZAd0B2wHgAeAB4AHfAd4B3gHhAdkB0AHQAdAB1AHTAdMB0gHRAdAB0AHTAdIB0AHQAeAB3gHdAd0B8gHzAewB7QHtAe4B7wHvAewB8wHrAesB6wHpAeoB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\"}";
            string curve01Data = "{\"version\":10000,\"archive3dm\":70,\"opennurbs\":-1904966255,\"data\":\"+n8CAKkBAAAAAAAA+/8CABQAAAAAAAAA5tTXTkfp0xG/5QAQgwEi8E6cu9v8/wIAcQEAAAAAAAAQCwAAAAAAAAAAAAAAywd7LlorCEAKuuAdqlBhQAAAAAAAAAAAh7ZOjD19QUCpmHp6C1NhQAAAAAAAAAAAzJ4rzIKzP0Dj8CfBL5FrQAAAAAAAAAAAwqdJZJDfKkAP0y3zbQxwQAAAAAAAAAAAwqdJZJDfKkAAAAAAACByQAAAAAAAAAAAAAAAAAAAAAAAAAAAACByQAAAAAAAAAAAuLiUkdOhPL0rnepXrM5uQAAAAAAAAAAAncGA5dT5E0ArnepXrM5uQAAAAAAAAAAAO+3oLrZKIEB91H6aRmptQAAAAAAAAAAAX8AGN0KVBkAkkJjdsfZrQAAAAAAAAAAAywd7LlorCEAKuuAdqlBhQAsAAAAAAAAAAAAAANzlRdqcq/8/4ulNnD48HEBwn9YSFyUjQI0aKfiuQidAiiYTeMvsKEARq43Pm1EuQDr5eA/77y5AQ2sMSoYvMEC/HZMUEvowQPTWsEPZQDZAAwAAAMHpRsX/fwKAAAAAAAAAAAA=\"}";
            string curve02Data = "{\"version\":10000,\"archive3dm\":70,\"opennurbs\":-1904966255,\"data\":\"+n8CAKkBAAAAAAAA+/8CABQAAAAAAAAA5tTXTkfp0xG/5QAQgwEi8E6cu9v8/wIAcQEAAAAAAAAQCwAAAAAAAAAAAAAAmy7+BfCrQUDidX8FZIxfQAAAAAAAAAAA9mk2teEiWkA3JFfm3FJjQAAAAAAAAAAAxD3+YeLRWkBwcQhWy3plQAAAAAAAAAAAqgPENlV0V0AXNjVFk4doQAAAAAAAAAAAIHdjLwYxV0Asct2zNHlsQAAAAAAAAAAA3AwVKlFaU0CH0JckcNdsQAAAAAAAAAAADDu7wGQ/U0AySxrNCOZnQAAAAAAAAAAA5ymyvxcJVUDV7RZ+19FnQAAAAAAAAAAAF1hYVivuVEDB1FVvOUtnQAAAAAAAAAAAOAE+ESJNLkAB+ozW8ZVkQAAAAAAAAAAAmy7+BfCrQUDidX8FZIxfQAsAAAAAAAAAAAAAADTzwm7SiQVA7LZKyjBNHUDQ/lGwZtAgQMzzIg4gRCRAAwKPkT4tKED0M3orDx0qQD7Mf3YrAy9Ax6vUZdjmL0DwQrqmdTYwQDREmHe3qjRAAwAAAEo4W4z/fwKAAAAAAAAAAAA=\"}";

            Mesh handMesh = (Mesh)Mesh.FromJSON(meshData);
            Curve curve01 = (Curve)Curve.FromJSON(curve01Data);
            Curve curve02 = (Curve)Curve.FromJSON(curve02Data);

            double tolerance = 0.001;

            List<Curve> midCurves = Curve.CreateTweenCurves(curve01, curve02, 10, tolerance).ToList();
            midCurves.Insert(0, curve01);
            midCurves.Add(curve02);


            distance = distance < 0 ? 0 : distance;
            distance = distance > 150 ? 150 : distance;

            int index = Convert.ToInt32(distance / 150 * 10);


            Curve midCurve = midCurves[index];
            midCurve.Translate(Vector3d.XAxis * -12.5);
            Curve[] subCurves = midCurve.DuplicateSegments();
            Brep[] fingerSrfs = new Brep[subCurves.Length];
            for (int i = 0; i < fingerSrfs.Length; i++)
            {
                Surface srf = Surface.CreateExtrusion(subCurves[i], Vector3d.XAxis * 25);
                fingerSrfs[i] = srf.ToBrep();
            }
            Brep fingerBrep = Brep.JoinBreps(fingerSrfs, tolerance)[0];
            fingerBrep = fingerBrep.CapPlanarHoles(tolerance);

            MeshingParameters param = new MeshingParameters(20, 10);

            Mesh[] fingerMesh = Mesh.CreateFromBrep(fingerBrep, param);
            Mesh joinedFingerMesh = new Mesh();
            foreach (var item in fingerMesh)
            {
                joinedFingerMesh.Append(item);
            }

            Transform mirror = Transform.Mirror(Plane.WorldZX);
            Mesh dupFinger = joinedFingerMesh.DuplicateMesh();
            dupFinger.Transform(mirror);

            joinedFingerMesh.Append(dupFinger);
            joinedFingerMesh.Append(handMesh);

            return joinedFingerMesh;
        }

        // http://james-ramsden.com/append-menu-items-to-grasshopper-components-with-c/
        //protected override void AppendAdditionalComponentMenuItems(System.Windows.Forms.ToolStripDropDown menu)
        //{
        //    base.AppendAdditionalComponentMenuItems(menu);
        //    Menu_AppendItem(menu, "Absolute", Absolute_Menu);
        //    Menu_AppendItem(menu, "Relative", Relative_Menu);
        //}

        //private void Absolute_Menu(object sender, EventArgs e)
        //{
        //    relative = false;
        //    this.ExpireSolution(true);
        //}
        //private void Relative_Menu(object sender, EventArgs e)
        //{
        //    relative = true;
        //    this.ExpireSolution(true);
        //}
    }
}