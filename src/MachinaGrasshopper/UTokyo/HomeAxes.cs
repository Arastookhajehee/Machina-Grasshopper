using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

using Grasshopper.Kernel;
using Machina;
using Rhino.Geometry;
using Robots;

namespace MachinaGrasshopper.UTokyo
{
    public class HomeAxes : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the HomeAxes class.
        /// </summary>
        public HomeAxes()
          : base("HomeAxes", "home",
              "a shortcut AxesTo action to return the robot to the home position",
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
            pManager.AddGenericParameter("Home", "home", "AxesToHome ShortCut", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            DA.SetData(0, new Machina.ActionAxes(
                new Machina.Types.Geometry.Joints(0,-90,-90,-90,90,0), false));
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
                return Properties.Resources.ShortCutAction_Home;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("5B2B9921-949F-4593-92C7-D6C4FF1E525B"); }
        }
    }
}