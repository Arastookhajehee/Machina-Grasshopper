using System;
using Machina;

using Grasshopper.Kernel;

namespace MachinaGrasshopper.Action
{
    public class SyncCurrent : GH_Component
    {
        public SyncCurrent() : base(
            "SyncCurrent",
            "SyncCurrent",
            "Creates a SyncCurrent action. Send this action alone in its own batch so Machina can refresh its internal state from the physical robot before subsequent actions.",
            "Machina",
            "Action")
        { }

        public override GH_Exposure Exposure => GH_Exposure.secondary;
        public override Guid ComponentGuid => new Guid("e2e3fac7-cae8-4d5e-ad97-2241d8a4d512");
        protected override System.Drawing.Bitmap Icon => Properties.Resources.Action_Comment;

        protected override void RegisterInputParams(GH_InputParamManager pManager) { }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Action", "A", "SyncCurrent Action", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            DA.SetData(0, new ActionSyncCurrent());
        }
    }
}
