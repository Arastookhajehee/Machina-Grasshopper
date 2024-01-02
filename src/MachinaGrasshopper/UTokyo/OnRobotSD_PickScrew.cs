using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Machina;
using Rhino.Geometry;

namespace MachinaGrasshopper.UTokyo
{
    public class OnRobotSD_PickScrew : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the OnRobotSD_PickScrew class.
        /// </summary>
        public OnRobotSD_PickScrew()
          : base("OnRobotSD_PickScrew",
                "pickScrew",
              "Pick a screw with the Onrobot OnRobot Screw Driver",
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
            pManager.AddIntegerParameter("waitTime", "wait", "Wait for tool action to finish", GH_ParamAccess.item, 4000);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Action", "A", "OnRobotSD_PickScrew Action", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int screwLength = 0;
            int wait = 0;

            if (!DA.GetData(0, ref screwLength)) return;
            if (!DA.GetData(1, ref wait)) return;


            DA.SetData(0, new ActionOnRobotSD_PickScrew(screwLength, wait));
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
                return Properties.Resources.ToolAction_SD_PickScrew;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("585DDD85-40B2-466A-892F-5D70E2595C12"); }
        }
    }
}