using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Machina;
using Rhino.Geometry;
using Robots;

using System.Linq;

namespace MachinaGrasshopper.UTokyo
{
    public class MachinaToRobots : GH_Component
    {

        enum ControlMode
        {
            initial = 0,
            axesMode = 1,
            cartesianMode = 2
        }

        Plane currentOrient = Plane.Unset;
        Robots.Tool currentTool = null;
        Robots.RobotConfigurations currentConfig = Robots.RobotConfigurations.None;
        Robots.Motions currentmotion = Robots.Motions.Linear;

        bool axesNotSetYet = true;
        double[] currentAxesValue = { 0, 0, 0, 0, 0, 0 };

        ControlMode currentMode = ControlMode.initial;



        void InitializeSim(Robots.CartesianTarget init)
        {
            this.currentOrient = init.Plane;
            this.currentTool = (Robots.Tool)init.Tool;
            this.currentConfig = (Robots.RobotConfigurations)init.Configuration;
            this.currentmotion = (Robots.Motions)init.Motion;
        }

        Plane GetPlaneFromMachinaTransform(ActionTransformation x)
        {
            Machina.ActionTransformation transform = x as Machina.ActionTransformation;
            Machina.Types.Geometry.Orientation orientation = transform.rotation.ToOrientation();
            Machina.Types.Geometry.Vector translation = transform.translation;

            Plane pl = new Plane(
              new Point3d(translation.X, translation.Y, translation.Z),
              new Vector3d(orientation.XAxis.X, orientation.XAxis.Y, orientation.XAxis.Z),
              new Vector3d(orientation.YAxis.X, orientation.YAxis.Y, orientation.YAxis.Z));

            return pl;
        }

        Plane GetPlaneFromMachinaRotation(ActionRotation x)
        {
            Machina.ActionRotation transform = x as Machina.ActionRotation;
            Machina.Types.Geometry.Orientation orientation = transform.rotation.ToOrientation();

            Plane pl = new Plane(
              new Point3d(0, 0, 0),
              new Vector3d(orientation.XAxis.X, orientation.XAxis.Y, orientation.XAxis.Z),
              new Vector3d(orientation.YAxis.X, orientation.YAxis.Y, orientation.YAxis.Z));

            return pl;
        }

        Vector3d GetVectorFromMachinaTranslation(ActionTranslation x)
        {
            var machinaTranslation = x.translation;
            Vector3d vec = new Vector3d(machinaTranslation.X, machinaTranslation.Y, machinaTranslation.Z);
            return vec;
        }

        /// <summary>
        /// Initializes a new instance of the MachinaToRobots class.
        /// </summary>
        public MachinaToRobots()
          : base("MachinaToRobots", "Mn2Rt",
              "Converts Machina actions to Robots Target for simulation.",
              "Machina",
              "UTokyo")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("initialTarget", "inti", "Initial Robots target with robot joint configuration and other settings.", GH_ParamAccess.item);
            pManager.AddGenericParameter("machinaActions", "actions", "Machina Actions to be converted to Robots targets", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("RobotsTargets", "targets", "Converted Robots Targets", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Robots.Tool> toolDefintions = new List<Robots.Tool>();

            Robots.Target init = null;
            DA.GetData(0, ref init);
            if (init == null) return;

            //Plane initPlane = init
            Robots.CartesianTarget initalTarget = init as Robots.CartesianTarget;


            List<Machina.Action> actions = new List<Machina.Action>();
            DA.GetDataList(1, actions);

            

            InitializeSim(initalTarget);

            List<Robots.Target> simTargets = new List<Robots.Target>();
            simTargets.Add(initalTarget);

            foreach (var item in actions)
            {

                Machina.Action action = item as Machina.Action;
                Robots.Target target = null;
                switch (action.Type)
                {


                    case (Machina.ActionType.Transformation):
                        ActionTransformation transform = (ActionTransformation)action;
                        if (!transform.relative)
                        {
                            Plane pl = GetPlaneFromMachinaTransform(transform);
                            this.currentOrient = pl;

                            target = new CartesianTarget(this.currentOrient, this.currentConfig, this.currentmotion, this.currentTool, null, null, null, null, null);
                            simTargets.Add(target);
                            currentMode = ControlMode.cartesianMode;
                        }
                        else
                        {
                            this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Relative transforms are not supported!");
                            return;
                        }
                        break;

                    case (Machina.ActionType.Translation):
                        Machina.ActionTranslation translation = (Machina.ActionTranslation)action;

                        if (!translation.relative)
                        {
                            Vector3d planeOrigin = GetVectorFromMachinaTranslation(translation);
                            this.currentOrient.Origin = (Point3d)planeOrigin;

                            target = new CartesianTarget(this.currentOrient, this.currentConfig, this.currentmotion, this.currentTool, null, null, null, null, null);
                            simTargets.Add(target);
                            currentMode = ControlMode.cartesianMode;
                        }
                        else
                        {
                            if (currentMode == ControlMode.axesMode)
                            {
                                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Please add absolute action before switching between axes/cartesian modes!");
                                return;
                            }
                            Vector3d translationVector = GetVectorFromMachinaTranslation(translation);
                            this.currentOrient.Origin = this.currentOrient.Origin + translationVector;

                            target = new CartesianTarget(this.currentOrient, this.currentConfig, this.currentmotion, this.currentTool, null, null, null, null, null);
                            simTargets.Add(target);
                            currentMode = ControlMode.cartesianMode;
                        }

                        break;

                    case (Machina.ActionType.Rotation):
                        Machina.ActionRotation rotation = (Machina.ActionRotation)action;

                        if (!rotation.relative)
                        {
                            Plane newOrientation = GetPlaneFromMachinaRotation(rotation);

                            this.currentOrient = new Plane(this.currentOrient.Origin, newOrientation.XAxis, newOrientation.YAxis);
                            target = new CartesianTarget(this.currentOrient, this.currentConfig, this.currentmotion, this.currentTool, null, null, null, null, null);
                            simTargets.Add(target);
                            currentMode = ControlMode.cartesianMode;
                        }
                        else
                        {
                            if (currentMode == ControlMode.axesMode)
                            {
                                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Please add absolute action before switching between axes/cartesian modes!");
                                return;
                            }

                            this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Relative rotations are WIP!");
                            return;
                        }
                        break;

                    case (Machina.ActionType.DefineTool):
                        Machina.ActionDefineTool defineTool = (Machina.ActionDefineTool)action;

                        Machina.Types.Geometry.Orientation tcpOrient = defineTool.tool.TCPOrientation;
                        Vector3d xVec = new Vector3d(tcpOrient.XAxis.X, tcpOrient.XAxis.Y, tcpOrient.XAxis.Z);
                        Vector3d yVec = new Vector3d(tcpOrient.YAxis.X, tcpOrient.YAxis.Y, tcpOrient.YAxis.Z);
                        Machina.Types.Geometry.Point tcpPoint = defineTool.tool.TCPPosition;

                        Plane toolOrientation = new Plane(Point3d.Origin, xVec, yVec);
                        Point3d toolOrigin = new Point3d(tcpPoint.X, tcpPoint.Y, tcpPoint.Z);

                        double weight = defineTool.tool.Weight;
                        string toolName = defineTool.tool.name;

                        Plane toolPlane = new Plane(toolOrigin, toolOrientation.XAxis, toolOrientation.YAxis);
                        Robots.Tool newTool = new Robots.Tool(toolPlane, toolName, weight, null, null, null, false, null);
                        toolDefintions.Add(newTool);
                        break;

                    case (Machina.ActionType.AttachTool):
                        Machina.ActionAttachTool attachTool = (Machina.ActionAttachTool)action;

                        foreach (Robots.Tool tool in toolDefintions)
                        {
                            if (attachTool.toolName.Equals(tool.Name))
                            {
                                currentTool = tool;
                                break;
                            }
                        }

                        break;
                    case (Machina.ActionType.OnrobotRG6):
                        Machina.ActionRG6Gripper rgGripper = (Machina.ActionRG6Gripper)action;

                        currentTool = new Robots.Tool(currentTool.Tcp, currentTool.Name, currentTool.Weight, null, null, null, false, null);

                        break;
                    case (Machina.ActionType.Axes):
                        break; // WIP
                    /*
                    Machina.ActionAxes actionAxes = (Machina.ActionAxes) action;

                    if (!actionAxes.relative)
                    {
                      axesNotSetYet = false;
                      this.currentAxesValue[0] = actionAxes.joints.J1;
                      this.currentAxesValue[1] = actionAxes.joints.J2;
                      this.currentAxesValue[2] = actionAxes.joints.J3;
                      this.currentAxesValue[3] = actionAxes.joints.J4;
                      this.currentAxesValue[4] = actionAxes.joints.J5;
                      this.currentAxesValue[5] = actionAxes.joints.J6;

                      Robots.JointTarget jointTarget = new Robots.JointTarget(this.currentAxesValue, currentTool, null, null, null, null, null);
                      simTargets.Add(jointTarget);
                      currentMode = ControlMode.axesMode;
                    }
                    else
                    {
                      if (axesNotSetYet)
                      {
                        this.Component.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Before a relative joint action be sure to set an absolute joint action!");
                        return;
                      }
                      else
                      {
                        if (currentMode == ControlMode.cartesianMode)
                        {
                          this.Component.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Please add absolute action before switching between axes/cartesian modes!");
                          return;
                        }

                        this.currentAxesValue[0] += actionAxes.joints.J1;
                        this.currentAxesValue[1] += actionAxes.joints.J2;
                        this.currentAxesValue[2] += actionAxes.joints.J3;
                        this.currentAxesValue[3] += actionAxes.joints.J4;
                        this.currentAxesValue[4] += actionAxes.joints.J5;
                        this.currentAxesValue[5] += actionAxes.joints.J6;

                        Robots.JointTarget jointTarget = new Robots.JointTarget(this.currentAxesValue, currentTool, null, null, null, null, null);
                        simTargets.Add(jointTarget);
                        currentMode = ControlMode.axesMode;
                      }

                    }

                    break;
                    */

                    case (Machina.ActionType.DetachTool):

                        currentTool = null;
                        break;

                    default:
                        break;
                }
            }


            DA.SetDataList(0, simTargets);
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
                return Properties.Resources.MachinaToRobots;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("79E53EC1-F27D-472C-8010-8BF84503A16C"); }
        }
    }
}