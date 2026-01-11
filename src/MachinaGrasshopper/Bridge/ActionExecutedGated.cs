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
    //  ███████╗██╗  ██╗███████╗ ██████╗██╗   ██╗████████╗███████╗██████╗ 
    //  ██╔════╝╚██╗██╔╝██╔════╝██╔════╝██║   ██║╚══██╔══╝██╔════╝██╔══██╗
    //  █████╗   ╚███╔╝ █████╗  ██║     ██║   ██║   ██║   █████╗  ██║  ██║
    //  ██╔══╝   ██╔██╗ ██╔══╝  ██║     ██║   ██║   ██║   ██╔══╝  ██║  ██║
    //  ███████╗██╔╝ ██╗███████╗╚██████╗╚██████╔╝   ██║   ███████╗██████╔╝
    //  ╚══════╝╚═╝  ╚═╝╚══════╝ ╚═════╝ ╚═════╝    ╚═╝   ╚══════╝╚═════╝ 
    //                                                                    

    // WORK IN PROGRESS! 
    public class ActionExecutedGated : GH_Component
    {
        // For new events, all outputs will be updated, even if some of them have the same value (like position might be repeated on a Wait action...).
        private bool _updateOutputs;
        private const string EVENT_NAME = "action-executed";
        private readonly int UPDATE_DELAY = 2000;

        // Outputs
        private List<string> _receivedMessages;
        private int _prevId, _id;
        private string _instruction;
        private int _pendingExecutionOnDevice;
        private int _pendingExecutionTotal;
        private Plane _tcp;
        private double?[] _axes;
        private double?[] _externalAxes;

        public ActionExecutedGated() : base(
            "ActionExecutedGated",
            "ActionExecutedGated",
            "Will update every time an Action has been successfully executed by the robot.",
            "Machina",
            "Bridge")
        {
            //_updateOutputs = true;
            _receivedMessages = new List<string>();
        }

        public override GH_Exposure Exposure => GH_Exposure.hidden;
        public override Guid ComponentGuid => new Guid("6aca9a1e-cdcf-435a-a627-7d8dda85ae6f");
        protected override System.Drawing.Bitmap Icon => Properties.Resources.Bridge_ActionExecuted;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("BridgeMessage", "BM", "The last message received from the Machina Bridge.", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("LastAction", "last", "Last Action that was successfully executed by the robot.", GH_ParamAccess.item);

            pManager.AddPlaneParameter("ActionTCP", "tcp", "Last known TCP position for this Action.", GH_ParamAccess.item);
            pManager.AddNumberParameter("ActionAxes", "axes", "Last known axes for this Action.", GH_ParamAccess.list);
            pManager.AddNumberParameter("ActionExternalAxes", "extax", "Last known external axes for this Action.", GH_ParamAccess.list);

            pManager.AddNumberParameter("PendingActions", "pendTot", "How many Actions are left in the queue to be executed?", GH_ParamAccess.item);
            pManager.AddNumberParameter("PendingActionsOnDevice", "pendDev", "How many Actions are left on the device to be executed? This only accounts for the ones that have already been released to it.", GH_ParamAccess.item);

            pManager.AddTextParameter("MSGS", "", "", GH_ParamAccess.list);
            pManager.AddNumberParameter("MSGCOUNT", "", "", GH_ParamAccess.item);
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

            //if (!DA.GetData(0, ref msg)) return;
            DA.GetData(0, ref msg);

            // Add message to list
            // if (msg != null)
            // {
            //     dynamic json = _serializer.Deserialize<dynamic>(msg);
            //     string eType = json["event"];
            //     if (eType.Equals(EVENT_NAME))
            //     {

            //     }
            // }



        }

        /// <summary>
        /// Parses the message to figure out if it is new data, updates properties if applicable, 
        /// and return true if this happened.
        /// </summary>
        /// <param name="msg"></param>
        private bool ReceivedNewMessage(string msg)
        {
            if (msg == null)
                return false;

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
                // swallow JSON errors to preserve original behavior
            }
            catch (Exception)
            {
                // swallow unexpected errors to preserve original behavior
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

            _pendingExecutionOnDevice = json["pendDev"];
            _pendingExecutionTotal = json["pendTot"];
        }
    }
}
