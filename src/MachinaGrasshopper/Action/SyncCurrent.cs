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
            "Creates a SyncCurrent action. Optionally flush pending actions when it executes so Machina can rebase its internal state from the physical robot.",
            "Machina",
            "Action")
        { }

        public override GH_Exposure Exposure => GH_Exposure.secondary;
        public override Guid ComponentGuid => new Guid("e2e3fac7-cae8-4d5e-ad97-2241d8a4d512");
        protected override System.Drawing.Bitmap Icon => Properties.Resources.Action_Comment;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("FlushPending", "T/F", "If true, pending actions are flushed when SyncCurrent executes before rebasing Machina's internal state.", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Action", "A", "SyncCurrent Action", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool flushPending = false;
            if (!DA.GetData(0, ref flushPending)) return;

            DA.SetData(0, new ActionSyncCurrent(flushPending));
        }
    }
}
