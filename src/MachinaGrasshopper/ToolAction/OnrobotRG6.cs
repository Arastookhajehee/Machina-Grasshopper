using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Machina;
using Rhino.Geometry;

namespace MachinaGrasshopper.ToolAction
{
    public class OnrobotRG6 : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the OnrobotRG6 class.
        /// </summary>
        public OnrobotRG6() : base(
            "OnrobotRG6",
            "OnrobotRG6",
            "Controls Onrobot RG6 two finger Gripper",
            "Machina",
            "ToolAction")
        { }

        //public override GH_Exposure Exposure => GH_Exposure.quinary;
        public override Guid ComponentGuid => new Guid("EBE070BF-9E3D-4F64-8E0E-FFDD8DA1379B");
        protected override System.Drawing.Bitmap Icon => Properties.Resources.ToolAction_RG6;

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {

            pManager.AddNumberParameter("Value", "V", "Gripper Finger Distance in (mm)", GH_ParamAccess.item, 0);

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
            double val = 0;
            bool tool = false;

            if (!DA.GetData(0, ref val)) return;

            DA.SetData(0, new ActionIOAnalog("10000", val, tool));
        }
    }
}