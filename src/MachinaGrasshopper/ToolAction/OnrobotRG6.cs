using System;
using System.Collections.Generic;

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

            pManager.AddNumberParameter("fingerDistance", "distance", "Gripper Finger Distance in (mm)", GH_ParamAccess.item, 75);
            pManager.AddNumberParameter("objectWeight", "weight", "the weight of the picked up object in (kg)", GH_ParamAccess.item, 0);
            pManager.AddTextParameter("runMode", "mode", "'inplace' for stationary gripper finger action. \n" +
                                                       "'moving for finger action not stopping while the robot moves.", GH_ParamAccess.item, "inplace");

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

            DA.SetData(0, new ActionRG6Gripper(GripperType.Analouge, distance, weight, runMode, relative));

            this.Message = relative ? "Relative" : "Absolute";
        }

        // http://james-ramsden.com/append-menu-items-to-grasshopper-components-with-c/
        protected override void AppendAdditionalComponentMenuItems(System.Windows.Forms.ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);
            Menu_AppendItem(menu, "Absolute", Absolute_Menu);
            Menu_AppendItem(menu, "Relative", Relative_Menu);
        }

        private void Absolute_Menu(object sender, EventArgs e)
        {
            relative = false;
            this.ExpireSolution(true);
        }
        private void Relative_Menu(object sender, EventArgs e)
        {
            relative = true;
            this.ExpireSolution(true);
        }
    }
}