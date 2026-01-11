using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace MachinaGrasshopper.UTokyo
{
    public class OnRobotRG6_TCP : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the OnRobotRG6_TCP class.
        /// </summary>
        public OnRobotRG6_TCP()
          : base("OnRobotRG6_TCP", "RG6_TCP",
              "The TCP Plane for the RG6 OnRobot Gripper",
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
            pManager.AddPlaneParameter("RG6_TCP", "RG6_TCP", "The TCP Plane for the RG6 OnRobot Gripper", GH_ParamAccess.item);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Plane pl = new Plane(new Point3d(0, 0, 269.7), Plane.WorldXY.XAxis, Plane.WorldXY.YAxis);
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
                return MachinaGrasshopper.Properties.Resources.ToolAction_RG6_TCP;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("1CA3F8EB-3291-4A43-B2DA-48B4A4982F7D"); }
        }
    }
}