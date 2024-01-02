using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace MachinaGrasshopper.UTokyo
{
    public class OnRobotVG10_TCP : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the OnRobotVG10_TCP class.
        /// </summary>
        public OnRobotVG10_TCP()
          : base("OnRobotVG10_TCP", "VG10_TCP",
              "The TCP Plane for the VG10 OnRobot Gripper",
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
            pManager.AddPlaneParameter("VG10_TCP", "VG10_TCP", "The TCP Plane for the VG10 OnRobot Gripper", GH_ParamAccess.item);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Plane pl = new Plane(new Point3d(0, 0, 118.6), Plane.WorldXY.XAxis, Plane.WorldXY.YAxis);
            DA.SetData(0,pl);
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
                return MachinaGrasshopper.Properties.Resources.ToolAction_VG10_TCP;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("D33ECDA3-6CD7-48C5-B457-56CF1F0A272A"); }
        }
    }
}