﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grasshopper.Kernel;
using Rhino.Geometry;

using Machina;
using WebSocketSharp;
using MachinaGrasshopper.Utils;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MachinaGrasshopper.Bridge
{
    //  ██████╗ ██████╗ ██╗██████╗  ██████╗ ███████╗                  
    //  ██╔══██╗██╔══██╗██║██╔══██╗██╔════╝ ██╔════╝                  
    //  ██████╔╝██████╔╝██║██║  ██║██║  ███╗█████╗                    
    //  ██╔══██╗██╔══██╗██║██║  ██║██║   ██║██╔══╝                    
    //  ██████╔╝██║  ██║██║██████╔╝╚██████╔╝███████╗                  
    //  ╚═════╝ ╚═╝  ╚═╝╚═╝╚═════╝  ╚═════╝ ╚══════╝                  
    //                                                                
    //   ██████╗ ██████╗ ███╗   ██╗███╗   ██╗███████╗ ██████╗████████╗
    //  ██╔════╝██╔═══██╗████╗  ██║████╗  ██║██╔════╝██╔════╝╚══██╔══╝
    //  ██║     ██║   ██║██╔██╗ ██║██╔██╗ ██║█████╗  ██║        ██║   
    //  ██║     ██║   ██║██║╚██╗██║██║╚██╗██║██╔══╝  ██║        ██║   
    //  ╚██████╗╚██████╔╝██║ ╚████║██║ ╚████║███████╗╚██████╗   ██║   
    //   ╚═════╝ ╚═════╝ ╚═╝  ╚═══╝╚═╝  ╚═══╝╚══════╝ ╚═════╝   ╚═╝   
    //                                                                
    public class Connect : GH_Component
    {
        private List<string> msgevents = new List<string>()
        {
        "action-executed",
        "action-issued",
        "action-released",
        "motion-update"
        };

        private MachinaBridgeSocket _ms;

        public Connect() : base(
            "Connect",
            "Connect",
            "Establish connection with the Machina Bridge.",
            "Machina",
            "Bridge")
        { }
        public override GH_Exposure Exposure => GH_Exposure.primary;
        public override Guid ComponentGuid => new Guid("c72d426f-cf9c-4606-8023-f4d928ad88e6");
        protected override System.Drawing.Bitmap Icon => Properties.Resources.Bridge_Connect;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("URL", "URL", "The URL of the Machina Bridge App. Leave to default unless you know what you are doing ;)", GH_ParamAccess.item, "ws://127.0.0.1:6999/Bridge");
            pManager.AddTextParameter("Name", "Name", "The name of this connecting client", GH_ParamAccess.item, "GH");
            pManager.AddTextParameter("Key", "Key", "Optional authorization key if connecting to a remote server", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Connect?", "C", "Connect to Machina Bridge App?", GH_ParamAccess.item, false);

            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Status", "log", "Status messages", GH_ParamAccess.list);
            pManager.AddGenericParameter("Bridge", "MB", "The (websocket) object managing connection to the Machina Bridge", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string url = "";
            string clientName = "";
            string key = "";
            bool connect = false;

            if (!DA.GetData(0, ref url)) return;
            if (!DA.GetData(1, ref clientName)) return;
            DA.GetData(2, ref key);
            if (!DA.GetData(3, ref connect)) return;

            url += "?name=" + clientName + "&client=Grasshopper";
            if (key != "")
            {
                url += "&authkey=" + key;
            }

            _ms = _ms ?? new MachinaBridgeSocket(clientName);

            bool connectedResult = false;
            List<string> msgs = new List<string>();

            // @TODO: move all socket management inside the wrapper
            if (connect)
            {
                if (_ms.socket == null)
                {
                    _ms.socket = new WebSocket(url);
                }

                if (!_ms.socket.IsAlive)
                {
                    _ms.socket.Connect();
                    _ms.socket.OnMessage += Socket_OnMessage;
                    _ms.socket.OnClose += Socket_OnClose;
                }

                connectedResult = _ms.socket.IsAlive;

                if (connectedResult)
                {
                    msgs.Add("Connected to Machina Bridge");
                } 
                else
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Could not connect to Machina Bridge app");
                    return;
                }
            }
            else
            {
                if (_ms.socket != null)
                {
                    _ms.socket.Close(CloseStatusCode.Normal, "k thx bye!");
                    _ms.socket = null;
                    _ms.Flush();

                    msgs.Add("Disconnected from the bridge");
                }
                connectedResult = false;
            }

            DA.SetDataList(0, msgs);
            DA.SetData(1, connectedResult ? _ms : null);
        }

        private void Socket_OnMessage(object sender, MessageEventArgs e)
        {
            _ms.Log(e.Data);

            // since the component is downstream, we can expire it's solution with every message the client receives
            // using the params property of this component we can find out what type of components we're connected to
            // since we can access them as class instance objects, we can push data to their **public** properties
            // here the _latestMessage is a public string that we can re-write with every OnMessage event
            // this allows us to bypass the DA.SetData method, which requires component expiration 
            // new code by Arastoo Khajehee (https://github.com/Arastookhajehee/)
            if (this.Params.Output[1].Recipients.Count != 0)
            {
                foreach (var item in this.Params.Output[1].Recipients)
                {
                    if (item.Attributes.Parent == null) continue;
                    if(item.Attributes.Parent.DocObject.GetType().ToString() == "MachinaGrasshopper.Bridge.Send") continue;
                    if (item.Attributes.Parent.DocObject.GetType().ToString() == "MachinaGrasshopper.Bridge.Listen") 
                    {
                        try
                        {
                            // using Newtonsoft.Json for ease of parsing
                            JObject jsonObject = JObject.Parse(e.Data);
                            string eType = (string)jsonObject["event"];

                            if (msgevents.Contains(eType))
                            {
                                Listen listenComponent = (Listen)item.Attributes.Parent.DocObject;
                                listenComponent._latestMessage = e.Data;
                            }

                            // expiring components needs to be in a shceduleSolution method to prevent
                            // solution exiration errors and popups in Grasshopper
                            Grasshopper.Instances.ActiveCanvas.Document.ScheduleSolution(1, doc => 
                            {
                                item.Attributes.Parent.DocObject.ExpireSolution(false);
                            });
                            

                        }
                        catch
                        {
                            AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Parsing or component wiring problems!");
                            return;
                        }
                        
                    }
 
                    
                }
            }

        }

        private void Socket_OnClose(object sender, CloseEventArgs e)
        {
            // Was getting duplicate logging when connecting/disconneting/connecting again...
            // When closing, remove all handlers.
            // Apparently, this is safe (although not thread-safe) even if no handlers were attached: https://stackoverflow.com/a/7065771/1934487
            _ms.socket.OnMessage -= Socket_OnMessage;
            _ms.socket.OnClose -= Socket_OnClose;
        }
    }
}
