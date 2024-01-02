using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Machina;
using Rhino.Geometry;

namespace MachinaGrasshopper.UTokyo
{
    public class OnRobotVG10_ChannelGrip : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the OnRobotVG10_ChannelGrip class.
        /// </summary>
        public OnRobotVG10_ChannelGrip()
          : base("OnRobotVG10_ChannelGrip", "VG10_ChannelGrip",
              "Turns the vaccum gripper suctions channels with differnt values for each channel",
            "Machina",
            "UTokyo")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("channel01", "channel01", "gripper channel01 value in kPa", GH_ParamAccess.item);
            pManager.AddIntegerParameter("channel02", "channel02", "gripper channel02 value in kPa", GH_ParamAccess.item);
            pManager.AddIntegerParameter("power limit", "pw_limit", "gripper power limit in mA", GH_ParamAccess.item);
            pManager.AddIntegerParameter("waitTime", "wait", "Wait for tool action to finish", GH_ParamAccess.item, 3000);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Action", "A", "OnRobotVG10_ChannelGrip Action", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int channel01 = 0;
            int channel02 = 0;
            int power_limit = 0;
            int wait = 0;

            if (!DA.GetData(0, ref channel01)) return;
            if (!DA.GetData(1, ref channel02)) return;
            if (!DA.GetData(2, ref power_limit)) return;
            if (!DA.GetData(3, ref wait)) return;

            power_limit = power_limit < 100 ? 100 : power_limit;
            power_limit = power_limit > 1000 ? 1000 : power_limit;

            channel01 = channel01 < 5 ? 5 : channel01;
            channel01 = channel01 > 80 ? 80 : channel01;

            channel02 = channel02 < 5 ? 5 : channel02;
            channel02 = channel02 > 80 ? 80 : channel02;

            DA.SetData(0, new ActionOnRobotVG_ChannelGrip(channel01, channel02, power_limit, wait));
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
                return MachinaGrasshopper.Properties.Resources.ToolAction_VG10_ChannelGrip;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("2E33733C-FB81-436D-9E9C-804A7E2E48F2"); }
        }
    }
}