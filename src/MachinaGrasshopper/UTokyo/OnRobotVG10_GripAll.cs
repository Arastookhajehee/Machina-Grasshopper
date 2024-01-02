using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Machina;
using Rhino.Geometry;

namespace MachinaGrasshopper.UTokyo
{
    public class OnRobotVG10_GripAll : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the OnRobotVG10_GripAll class.
        /// </summary>
        public OnRobotVG10_GripAll()
          : base("OnRobotVG10_GripAll", "VG10_GripAll",
              "Turns the vaccum gripper suctions channels",
            "Machina",
            "UTokyo")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("channels", "channels", "gripper channel value in kPa", GH_ParamAccess.item);
            pManager.AddIntegerParameter("power limit", "pw_limit", "gripper power limit in mA", GH_ParamAccess.item);
            pManager.AddIntegerParameter("waitTime", "wait", "Wait for tool action to finish", GH_ParamAccess.item, 3000);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Action", "A", "OnRobotVG10_GripAll Action", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int channels = 0;
            int power_limit = 0;
            int wait = 0;
            
            if (!DA.GetData(0, ref channels)) return;
            if (!DA.GetData(1, ref power_limit)) return;
            if (!DA.GetData(2, ref wait)) return;

            power_limit = power_limit < 100 ? 100 : power_limit;
            power_limit = power_limit > 1000 ? 1000 : power_limit;

            channels = channels < 5 ? 5 : channels;
            channels = channels > 80 ? 80 : channels;

            DA.SetData(0, new ActionOnRobotVG_GripAll(channels, power_limit, wait));
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
                return MachinaGrasshopper.Properties.Resources.ToolAction_VG10_GripAll;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("BFAA8A71-FF2E-4CE8-84BB-A8D1848B2916"); }
        }
    }
}