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
    //   █████╗  ██████╗████████╗██╗ ██████╗ ███╗   ██╗                 
    //  ██╔══██╗██╔════╝╚══██╔══╝██║██╔═══██╗████╗  ██║                 
    //  ███████║██║        ██║   ██║██║   ██║██╔██╗ ██║                 
    //  ██╔══██║██║        ██║   ██║██║   ██║██║╚██╗██║                 
    //  ██║  ██║╚██████╗   ██║   ██║╚██████╔╝██║ ╚████║                 
    //  ╚═╝  ╚═╝ ╚═════╝   ╚═╝   ╚═╝ ╚═════╝ ╚═╝  ╚═══╝                 
    //                                                                  
    //  ██████╗ ███████╗██╗     ███████╗ █████╗ ███████╗███████╗██████╗ 
    //  ██╔══██╗██╔════╝██║     ██╔════╝██╔══██╗██╔════╝██╔════╝██╔══██╗
    //  ██████╔╝█████╗  ██║     █████╗  ███████║███████╗█████╗  ██║  ██║
    //  ██╔══██╗██╔══╝  ██║     ██╔══╝  ██╔══██║╚════██║██╔══╝  ██║  ██║
    //  ██║  ██║███████╗███████╗███████╗██║  ██║███████║███████╗██████╔╝
    //  ╚═╝  ╚═╝╚══════╝╚══════╝╚══════╝╚═╝  ╚═╝╚══════╝╚══════╝╚═════╝ 
    //                                                                  
    public class ActionReleased : GH_Component
    {
        // For new events, all outputs will be updated, even if some of them have the same value (like position might be repeated on a Wait action...).
        private bool _updateOutputs;
        private const string EVENT_NAME = "action-released";

        // Outputs
        private int _prevId, _id;
        private string _instruction;
        private Plane _tcp;
        private double?[] _axes;
        private double?[] _externalAxes;
        private int _pendingRelease;


        public ActionReleased() : base(
            "ActionReleased",
            "ActionReleased",
            "Will update every time an Action has been released to the robot and is pending execution.",
            "Machina",
            "Bridge")
        {
            _updateOutputs = true;
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;
        public override Guid ComponentGuid => new Guid("34021378-412e-4f12-8b1c-29c04ee74806");
        protected override System.Drawing.Bitmap Icon => Properties.Resources.Bridge_ActionReleased;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("BridgeMessage", "BM", "The last message received from the Machina Bridge.", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("LastAction", "last", "Last Action that was successfully released to the robot.", GH_ParamAccess.item);

            pManager.AddPlaneParameter("ActionTCP", "tcp", "Last known TCP position for this Action.", GH_ParamAccess.item);
            pManager.AddNumberParameter("ActionAxes", "axes", "Last known axes for this Action.", GH_ParamAccess.list);
            pManager.AddNumberParameter("ActionExternalAxes", "extax", "Last known external axes for this Action.", GH_ParamAccess.list);

            pManager.AddNumberParameter("PendingActions", "pend", "How many actions are pending release to device?", GH_ParamAccess.item);
        }

        //protected override void ExpireDownStreamObjects()
        //{
        //    if (_updateOutputs)
        //    {
        //        for (int i = 0; i < Params.Output.Count; i++)
        //        {
        //            Params.Output[i].ExpireSolution(false);
        //        }
        //    }
        //}

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // This stops the component from assigning nulls 
            // if we don't assign anything to an output.
            DA.DisableGapLogic();

            string msg = null;

            if (!DA.GetData(0, ref msg)) return;

            // Some sanity: users sometimes connect the Bridge from `Connect` to this input, 
            // rather than the message coming out of `Listen`. Display alert.
            if (msg.Equals("MachinaGrasshopper.Utils.MachinaBridgeSocket"))
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "This component requires a \"Msg\" from the \"Listen\" component as input");
                return;
            }




            // TEMPORARILY DEACTIVATED GATED OUTPUT, wasn't working well

            //// Output the values precomputed in the last solution.
            //DA.SetData(0, _instruction);
            //DA.SetData(1, _tcp);
            //DA.SetDataList(2, _axes);
            //DA.SetDataList(3, _externalAxes);
            //DA.SetData(4, _pendingRelease);

            //// If on second solution, stop checking.
            //if (_updateOutputs)
            //{
            //    _updateOutputs = false;
            //    return;
            //}

            //// Otherwise, search for updated values (only if new messages have been received 
            //// by the Listener), and schedule a new solution if they are new.
            //bool rescheduleRightAway = ReceivedNewMessage(msg);

            //// If new data came in, schedule a new solution immediately and flag outputs to expire. 
            //if (rescheduleRightAway)
            //{
            //    _updateOutputs = true;

            //    this.OnPingDocument().ScheduleSolution(5, doc =>
            //    {
            //        this.ExpireSolution(false);
            //    });
            //}

            // NO GATED UPDATES
            // Parse message
            bool valid = ReceivedNewMessage(msg);

            // Output the parsed values.
            if (valid)
            {
                DA.SetData(0, _instruction);
                DA.SetData(1, _tcp);
                DA.SetDataList(2, _axes);
                DA.SetDataList(3, _externalAxes);
                DA.SetData(4, _pendingRelease);
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
                    int id = json.Value<int>("id");
                    if (id != _prevId)
                    {
                        _id = id;
                        UpdateCurrentValues(json);
                        _prevId = _id;
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
            // @TODO: make this more programmatic, tie it to ActionExecutedArgs props
            _instruction = json["last"];

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

            _pendingRelease = json["pend"];
        }
    }
}
