using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Rhino.Geometry;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

using MachinaGrasshopper.GH_Utils;

namespace MachinaGrasshopper.Bridge
{
    //  ███╗   ███╗ ██████╗ ████████╗██╗ ██████╗ ███╗   ██╗
    //  ████╗ ████║██╔═══██╗╚══██╔══╝██║██╔═══██╗████╗  ██║
    //  ██╔████╔██║██║   ██║   ██║   ██║██║   ██║██╔██╗ ██║
    //  ██║╚██╔╝██║██║   ██║   ██║   ██║██║   ██║██║╚██╗██║
    //  ██║ ╚═╝ ██║╚██████╔╝   ██║   ██║╚██████╔╝██║ ╚████║
    //  ╚═╝     ╚═╝ ╚═════╝    ╚═╝   ╚═╝ ╚═════╝ ╚═╝  ╚═══╝
    //                                                     
    //  ██╗   ██╗██████╗ ██████╗  █████╗ ████████╗███████╗ 
    //  ██║   ██║██╔══██╗██╔══██╗██╔══██╗╚══██╔══╝██╔════╝ 
    //  ██║   ██║██████╔╝██║  ██║███████║   ██║   █████╗   
    //  ██║   ██║██╔═══╝ ██║  ██║██╔══██║   ██║   ██╔══╝   
    //  ╚██████╔╝██║     ██████╔╝██║  ██║   ██║   ███████╗ 
    //   ╚═════╝ ╚═╝     ╚═════╝ ╚═╝  ╚═╝   ╚═╝   ╚══════╝ 
    //                                                     
    public class MotionUpdate : GH_Component
    {
        private const string EVENT_NAME = "motion-update";
        private const double SIMILARITY_EPSILON = 0.001;  // @TODO: use one of Machina's built-in?

        // For new events, all outputs will be updated, even if some of them have the same value (like position might be repeated on a Wait action...).
        private bool _updateOutputs;

        // Outputs
        private Plane _prevTcp, _tcp;
        private double?[] _prevAxes, _axes;
        private double?[] _prevExternalxes, _externalAxes;


        public MotionUpdate() : base(
            "MotionUpdate",
            "MotionUpdate",
            "Will update every time real-time motion data is received from the device.",
            "Machina",
            "Bridge")
        {
            _updateOutputs = true;
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;
        public override Guid ComponentGuid => new Guid("cd3e4c8f-ca72-499a-bad0-4b950bb5e540");
        protected override System.Drawing.Bitmap Icon => Properties.Resources.Bridge_MotionUpdate;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("BridgeMessage", "BM", "The last message received from the Machina Bridge.", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPlaneParameter("TCP", "tcp", "Last known real-time TCP position.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Axes", "axes", "Last known real-time robot axes.", GH_ParamAccess.list);
            pManager.AddNumberParameter("ExternalAxes", "extax", "Last known real-time external axes.", GH_ParamAccess.list);

        }

        protected override void ExpireDownStreamObjects()
        {
            if (_updateOutputs)
            {
                for (int i = 0; i < Params.Output.Count; i++)
                {
                    Params.Output[i].ExpireSolution(false);
                }
            }
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // This stops the component from assigning nulls 
            // if we don't assign anything to an output.
            DA.DisableGapLogic();

            string msg = null;

            if (!DA.GetData(0, ref msg)) return;

            // Output the values precomputed in the last solution.
            DA.SetData(0, _tcp);
            DA.SetDataList(1, _axes);
            DA.SetDataList(2, _externalAxes);

            // If on second solution, stop checking.
            if (_updateOutputs)
            {
                _updateOutputs = false;
                return;
            }

            // Otherwise, search for updated values (only if new messages have been received 
            // by the Listener), and schedule a new solution if they are new.
            bool rescheduleRightAway = ReceivedNewMessage(msg);

            // If new data came in, schedule a new solution immediately and flag outputs to expire. 
            if (rescheduleRightAway)
            {
                _updateOutputs = true;
                _prevTcp = _tcp;
                _prevAxes = _axes;
                _prevExternalxes = _externalAxes;

                this.OnPingDocument().ScheduleSolution(5, doc =>
                {
                    this.ExpireSolution(false);
                });
            }
        }

        /// <summary>
        /// Parses the message to figure out if it is new data, updates properties if applicable, 
        /// and return true if this happened.
        /// </summary>
        /// <param name="msg"></param>
        private bool ReceivedNewMessage(string msg)
        {
            try
            {
                JObject json = JsonConvert.DeserializeObject<JObject>(msg);
                if (json == null)
                    return false;

                string eType = json.Value<string>("event");
                if (string.Equals(eType, EVENT_NAME, StringComparison.Ordinal))
                {
                    UpdateCurrentValues(json);

                    // If values are new, schedule new solution
                    if (!Machina.Utilities.Numeric.AreSimilar(_axes, _prevAxes, SIMILARITY_EPSILON) ||
                        !Machina.Utilities.Numeric.AreSimilar(_externalAxes, _prevExternalxes, SIMILARITY_EPSILON) ||
                        !GH_Helpers.AreSimilar(_tcp, _prevTcp, SIMILARITY_EPSILON)) // TCP change check
                    {
                        return true;
                    }
                }
            }
            catch (JsonException)
            {
                // preserve previous silent-failure behavior
            }
            catch (Exception)
            {
                // preserve previous silent-failure behavior
            }

            // If here, values were not updated
            return false;
        }


        /// <summary>
        /// Parse most up-to-date values from parsed message.
        /// </summary>
        /// <param name="msg"></param>
        private void UpdateCurrentValues(dynamic json)
        {
            var pos = Machina.Utilities.Conversion.ToNullableDoubles(json["pos"]);
            var ori = Machina.Utilities.Conversion.ToNullableDoubles(json["ori"]);
            if (pos == null || ori == null)
            {
                _tcp = Plane.Unset;
            }
            else
            {
                _tcp = new Plane(
                    new Point3d(Convert.ToDouble(pos[0]), Convert.ToDouble(pos[1]), Convert.ToDouble(pos[2])),
                    new Vector3d(Convert.ToDouble(ori[0]), Convert.ToDouble(ori[1]), Convert.ToDouble(ori[2])),
                    new Vector3d(Convert.ToDouble(ori[3]), Convert.ToDouble(ori[4]), Convert.ToDouble(ori[5]))
                );
            }

            JToken axesToken = json["axes"];
            if (axesToken == null || axesToken.Type == JTokenType.Null)
            {
                _axes = null;
            }
            else
            {
                object[] axesArray = axesToken.ToObject<object[]>();
                _axes = axesArray != null ? Machina.Utilities.Conversion.NullableDoublesFromObjects(axesArray) : null;
            }

            JToken extAxesToken = json["extax"];
            if (extAxesToken == null || extAxesToken.Type == JTokenType.Null)
            {
                _externalAxes = null;
            }
            else
            {
                object[] extAxesArray = extAxesToken.ToObject<object[]>();
                _externalAxes = extAxesArray != null ? Machina.Utilities.Conversion.NullableDoublesFromObjects(extAxesArray) : null;
            }
        }
    }
}
