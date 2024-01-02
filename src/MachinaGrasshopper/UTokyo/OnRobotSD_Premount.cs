using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Machina;
using Rhino.Geometry;

namespace MachinaGrasshopper.UTokyo
{
    public class OnRobotSD_Premount : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the OnRobotSD_Premount class.
        /// </summary>
        public OnRobotSD_Premount()
          : base("OnRobotSD_Premount", "premount",
              "Premount a screw with the Onrobot OnRobot Screw Driver",
            "Machina",
            "UTokyo")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("screwLength", "length", "Screw Length", GH_ParamAccess.item);
            pManager.AddNumberParameter("Torque", "torque", "controls the screw driver's spining power_limit : 0~5Nm",GH_ParamAccess.item,0.5);
            pManager.AddIntegerParameter("waitTime", "wait", "Wait for tool action to finish", GH_ParamAccess.item, 4000);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Action", "A", "OnRobotSD_Premount Action", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int screwLength = 0;
            double torque = 0.5;
            int wait = 0;

            if (!DA.GetData(0, ref screwLength)) return;
            if (!DA.GetData(1, ref torque)) return;
            if (!DA.GetData(2, ref wait)) return;

            torque *= OnRobotDefaults.tourqueScaleRatio;

            torque = torque < 17 ? 17 : torque;
            torque = torque > 500 ? 500 : torque;

            DA.SetData(0, new ActionOnRobotSD_Premount(screwLength, (int) torque, wait));
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
                return Properties.Resources.ToolAction_SD_Premount;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("501FFAC3-BB1B-4220-B6E5-99B835A9DC80"); }
        }
    }
}