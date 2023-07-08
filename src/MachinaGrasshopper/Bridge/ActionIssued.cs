﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web.Script.Serialization;

using Rhino.Geometry;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

using MachinaGrasshopper.GH_Utils;
using WebSocketSharp;

namespace MachinaGrasshopper.Bridge
{
    //   █████╗  ██████╗████████╗██╗ ██████╗ ███╗   ██╗
    //  ██╔══██╗██╔════╝╚══██╔══╝██║██╔═══██╗████╗  ██║
    //  ███████║██║        ██║   ██║██║   ██║██╔██╗ ██║
    //  ██╔══██║██║        ██║   ██║██║   ██║██║╚██╗██║
    //  ██║  ██║╚██████╗   ██║   ██║╚██████╔╝██║ ╚████║
    //  ╚═╝  ╚═╝ ╚═════╝   ╚═╝   ╚═╝ ╚═════╝ ╚═╝  ╚═══╝
    //                                                 
    //  ██╗███████╗███████╗██╗   ██╗███████╗██████╗    
    //  ██║██╔════╝██╔════╝██║   ██║██╔════╝██╔══██╗   
    //  ██║███████╗███████╗██║   ██║█████╗  ██║  ██║   
    //  ██║╚════██║╚════██║██║   ██║██╔══╝  ██║  ██║   
    //  ██║███████║███████║╚██████╔╝███████╗██████╔╝   
    //  ╚═╝╚══════╝╚══════╝ ╚═════╝ ╚══════╝╚═════╝    
    //                                                 
    public class ActionIssued : GH_Component
    {
        // For new events, all outputs will be updated, even if some of them have the same value (like position might be repeated on a Wait action...).
        private bool _updateOutputs;
        private const string EVENT_NAME = "action-issued";

        // Outputs
        private int _prevId, _id;
        private string _instruction;
        private Plane _tcp;
        private double?[] _axes;
        private double?[] _externalAxes;

        private JavaScriptSerializer ser;

        public ActionIssued() : base(
            "ActionIssued",
            "ActionIssued",
            "Will update every time an Action has been successfully issued and is pending release to the device.",
            "Machina",
            "Bridge")
        {
            _updateOutputs = true;
            ser = new JavaScriptSerializer();
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;
        public override Guid ComponentGuid => new Guid("81f80c94-2ef4-4620-8248-227a1b07c3ad");
        protected override System.Drawing.Bitmap Icon => Properties.Resources.Bridge_ActionIssued;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("BridgeMessage", "BM", "The last message received from the Machina Bridge.", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("LastAction", "last", "Last Action that was successfully issued to the robot.", GH_ParamAccess.item);

            pManager.AddPlaneParameter("ActionTCP", "tcp", "Last known TCP position for this Action.", GH_ParamAccess.item);
            pManager.AddNumberParameter("ActionAxes", "axes", "Last known axes for this Action.", GH_ParamAccess.list);
            pManager.AddNumberParameter("ActionExternalAxes", "extax", "Last known external axes for this Action.", GH_ParamAccess.list);
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


            // adding temporary data placeholders before data validation to prevent null output
            // by Arastoo Khajehee (https://github.com/Arastookhajehee)
            string tempInstruction = _instruction;
            Plane tempTcp = _tcp;
            double?[] tempAxes = _axes;
            double?[] tempExternalAxes = _externalAxes;

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
            }
            else
            {
                DA.SetData(0, tempInstruction);
                DA.SetData(1, tempTcp);
                DA.SetDataList(2, tempAxes);
                DA.SetDataList(3, tempExternalAxes);
            }
        }

        /// <summary>
        /// Parses the message to figure out if it is new data, updates properties if applicable, 
        /// and return true if this happened.
        /// </summary>
        /// <param name="msg"></param>
        private bool ReceivedNewMessage(string msg)
        {
            // a better check for empty and null messages [Arastoo Khajehee (https://github.com/Arastookhajehee/)]
            if (msg.IsNullOrEmpty()) return false;

            dynamic json = ser.Deserialize<dynamic>(msg);
            string eType = json["event"];
            if (eType.Equals(EVENT_NAME))
            {
                _id = json["id"];
                if (_id != _prevId)
                {
                    UpdateCurrentValues(json);
                    _prevId = _id;
                    return true;
                }
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

            var pos = Machina.Utilities.Conversion.NullableDoublesFromObjects(json["pos"]);
            var ori = Machina.Utilities.Conversion.NullableDoublesFromObjects(json["ori"]);
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

            _axes = Machina.Utilities.Conversion.NullableDoublesFromObjects(json["axes"]);
            _externalAxes = Machina.Utilities.Conversion.NullableDoublesFromObjects(json["extax"]);
        }
    }
}
