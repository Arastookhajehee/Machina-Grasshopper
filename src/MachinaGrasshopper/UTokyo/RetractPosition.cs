using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Machina;
using Rhino.Geometry;
using Robots;

namespace MachinaGrasshopper.UTokyo
{
    public class RetractPosition : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the HomeAxes class.
        /// </summary>
        public RetractPosition()
          : base("RetractPosition", "retract",
              "a shortcut AxesTo action to return to a retracted position to turn off the robot",
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
            pManager.AddGenericParameter("Retract", "retract", "Axes To Retracted Pose ShortCut", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            DA.SetData(0, new Machina.ActionAxes(
                new Machina.Types.Geometry.Joints(0, -70, -160, -0, 180, 90), false));
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
                return Properties.Resources.ShortCutAction_Retract;
            }
        }


        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("09F0699C-1484-4542-AFA5-5C71AE0AD4AF"); }
        }
    }
}