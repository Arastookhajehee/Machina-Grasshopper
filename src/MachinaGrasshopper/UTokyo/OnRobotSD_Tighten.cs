using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Machina;
using Rhino.Geometry;

namespace MachinaGrasshopper.UTokyo
{
    public class OnRobotSD_Tighten : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the OnRobotSD_Tighten class.
        /// </summary>
        public OnRobotSD_Tighten()
          : base("OnRobotSD_Tighten", "tighten",
              "Tighten a screw with the Onrobot OnRobot Screw Driver",
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
            pManager.AddTextParameter("screwType", "screwType", "screw type : M1.6, M2, M2.5, M3, M4, M5, M6", GH_ParamAccess.item);
            pManager.AddIntegerParameter("waitTime", "wait", "Wait for tool action to finish", GH_ParamAccess.item, 3500);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Action", "A", "OnRobotSD_Tighten Action", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int screwLength = 0;
            string screwType = "";
            int wait = 0;

            if (!DA.GetData(0, ref screwLength)) return;
            if (!DA.GetData(1, ref screwType)) return;
            if (!DA.GetData(2, ref wait)) return;

            if (!OnRobotDefaults.OnRobotScewTypes.ContainsKey(screwType.ToUpper()))
            {
                string acceptableTypes = "[";
                foreach (string key in OnRobotDefaults.OnRobotScewTypes.Keys)
                {
                    acceptableTypes += key + ",";
                }

                acceptableTypes = acceptableTypes.Substring(0, acceptableTypes.Length - 1) + "]";

                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Please specify the correct screw type\n" + acceptableTypes);
                return;
            }

            DA.SetData(0, new ActionOnRobotSD_Premount(screwLength, OnRobotDefaults.OnRobotScewTypes[screwType.ToUpper()], wait));
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
                return Properties.Resources.ToolAction_SD_Tighten;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("8F0CD45E-374D-4E20-AF09-9C1FA2B585BF"); }
        }
    }
}