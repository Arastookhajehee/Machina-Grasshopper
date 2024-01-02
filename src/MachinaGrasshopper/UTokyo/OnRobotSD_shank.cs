using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Machina;
using Rhino.Geometry;

namespace MachinaGrasshopper.ToolAction
{
    public class OnRobotSD_shank : GH_Component
    {
        bool relative = false;

        /// <summary>
        /// Initializes a new instance of the OnrobotRG6 class.
        /// </summary>
        public OnRobotSD_shank() : base(
            "OnRobotSD_shank",
            "SD_Shank",
            "Controls Onrobot OnRobot Screw Driver Shank Position",
            "Machina",
            "UTokyo")
        {
        }

        //public override GH_Exposure Exposure => GH_Exposure.quinary;
        public override Guid ComponentGuid => new Guid("60AB3322-A817-475A-8D7C-400C37692CF9");
        protected override System.Drawing.Bitmap Icon => Properties.Resources.ToolAction_SD_Shank;

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {

            pManager.AddIntegerParameter("shankPosition", "shank", "Shank Position (0-55mm) ", GH_ParamAccess.item, 40);
            pManager.AddIntegerParameter("waitTime", "wait", "Wait after GripperTrigger in milliseconds", GH_ParamAccess.item, 3000);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Action", "A", "OnRobotSD_shank Action", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int shank = 0;
            int wait = 0;

            if (!DA.GetData(0, ref shank)) return;
            if (!DA.GetData(1, ref wait)) return;


            shank = shank < 0 ? 0 : shank;
            shank = shank > 55 ? 55 : shank;


            DA.SetData(0, new ActionOnRobotScrewDriverShank(shank, wait));
        }



    }
}


