using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace MachinaGrasshopper.UTokyo
{
    public class OnRobotSD_TCP : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the OnRobotSD_TCP class.
        /// </summary>
        public OnRobotSD_TCP()
          : base("OnRobotSD_TCP", "SD_TCP",
              "The TCP Plane for the OnRobot Screw Driver",
              "Machina",
              "UTokyo")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPlaneParameter("SD_TCP", "SD_TCP", "The TCP Plane for the OnRobot Screw Driver", GH_ParamAccess.item);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Plane pl = new Plane(new Point3d(153, 0, 95.6), Plane.WorldXY.YAxis, Plane.WorldXY.XAxis);
            pl.Rotate(Math.PI, pl.YAxis, pl.Origin);
            pl.Rotate(-Math.PI/2.0, pl.XAxis, pl.Origin);
            pl.Rotate(-Math.PI/2.0, pl.ZAxis, pl.Origin);
            DA.SetData(0, pl);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return MachinaGrasshopper.Properties.Resources.ToolAction_SD_TCP;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("A8ED6491-8919-4BAC-88D3-730707E94BF9"); }
        }
    }
}