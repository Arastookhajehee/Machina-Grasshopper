﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Parameters;

using Machina;
using WebSocketSharp;
using MachinaGrasshopper.GH_Utils;

namespace MachinaGrasshopper.Graveyard
{
    //   ██████╗ ██████╗  █████╗ ██╗   ██╗███████╗██╗   ██╗ █████╗ ██████╗ ██████╗ 
    //  ██╔════╝ ██╔══██╗██╔══██╗██║   ██║██╔════╝╚██╗ ██╔╝██╔══██╗██╔══██╗██╔══██╗
    //  ██║  ███╗██████╔╝███████║██║   ██║█████╗   ╚████╔╝ ███████║██████╔╝██║  ██║
    //  ██║   ██║██╔══██╗██╔══██║╚██╗ ██╔╝██╔══╝    ╚██╔╝  ██╔══██║██╔══██╗██║  ██║
    //  ╚██████╔╝██║  ██║██║  ██║ ╚████╔╝ ███████╗   ██║   ██║  ██║██║  ██║██████╔╝
    //   ╚═════╝ ╚═╝  ╚═╝╚═╝  ╚═╝  ╚═══╝  ╚══════╝   ╚═╝   ╚═╝  ╚═╝╚═╝  ╚═╝╚═════╝ 
    //                                                                             
    /// <summary>
    /// A shrine of peace and remembrance for the heroes of the past, an ode to our proud legacy!
    /// 
    /// Components will be marked 'OLD' if the classname contains the string "obsolete" or the class
    /// has been decorated with the ObsoleteAttribute.
    /// </summary>


    //  ██╗    ██╗ ██████╗ 
    //  ██║   ██╔╝██╔═══██╗
    //  ██║  ██╔╝ ██║   ██║
    //  ██║ ██╔╝  ██║   ██║
    //  ██║██╔╝   ╚██████╔╝
    //  ╚═╝╚═╝     ╚═════╝ 
    //                     
    [Obsolete("Deprecated", false)]
    public class TurnOn : GH_Component
    {
        public TurnOn() : base(
            "TurnOn",
            "TurnOn",
            "Turn digital output on. Alias for `WriteDigital(ioNum, true)`",
            "Machina",
            "Action")
        { }
        public override GH_Exposure Exposure => GH_Exposure.hidden;
        public override Guid ComponentGuid => new Guid("8bd5bc0d-249e-4744-8530-cf8fced77492");
        protected override System.Drawing.Bitmap Icon => null;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("DigitalPinNumber", "N", "Digital pin number", GH_ParamAccess.item, 1);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Action", "A", "TurnOn Action", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string id = "1";

            if (!DA.GetData(0, ref id)) return;

            DA.SetData(0, new ActionIODigital(id, true, false));
        }
    }
    [Obsolete("Deprecated", false)]
    public class TurnOff : GH_Component
    {
        public TurnOff() : base(
            "TurnOff",
            "TurnOff",
            "Turn digital output off. Alias for `WriteDigital(ioNum, false)`",
            "Machina",
            "Action")
        { }
        public override GH_Exposure Exposure => GH_Exposure.hidden;
        public override Guid ComponentGuid => new Guid("15d234aa-2f63-488e-a95e-cc89ffcca3b6");
        protected override System.Drawing.Bitmap Icon => null;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("DigitalPinNumber", "N", "Digital pin number", GH_ParamAccess.item, 1);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Action", "A", "TurnOff Action", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string id = "1";

            if (!DA.GetData(0, ref id)) return;

            DA.SetData(0, new ActionIODigital(id, false, false));
        }
    }



    //  ██████╗ ██████╗ ███████╗ ██████╗██╗███████╗██╗ ██████╗ ███╗   ██╗
    //  ██╔══██╗██╔══██╗██╔════╝██╔════╝██║██╔════╝██║██╔═══██╗████╗  ██║
    //  ██████╔╝██████╔╝█████╗  ██║     ██║███████╗██║██║   ██║██╔██╗ ██║
    //  ██╔═══╝ ██╔══██╗██╔══╝  ██║     ██║╚════██║██║██║   ██║██║╚██╗██║
    //  ██║     ██║  ██║███████╗╚██████╗██║███████║██║╚██████╔╝██║ ╚████║
    //  ╚═╝     ╚═╝  ╚═╝╚══════╝ ╚═════╝╚═╝╚══════╝╚═╝ ╚═════╝ ╚═╝  ╚═══╝
    //                                                                   
    [Obsolete("Updated", false)]
    public class Precision : GH_Component
    {
        public Precision() : base(
            "Precision",
            "Precision",
            "Increase the default precision value new actions will be given. Precision is measured as the radius of the smooth interpolation between motion targets. This is refered to as \"Zone\", \"Approximate Positioning\" or \"Blending Radius\" in different platforms.",
            "Machina",
            "Action")
        { }
        public override GH_Exposure Exposure => GH_Exposure.hidden;
        public override Guid ComponentGuid => new Guid("eaadd1fc-caa9-442b-af5e-273bc3961b73");
        protected override System.Drawing.Bitmap Icon => Properties.Resources.Action_Precision;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("RadiusIncrement", "RI", "Smoothing radius increment in mm", GH_ParamAccess.item, 0);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Action", "A", "Precision Action", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double radiusInc = 0;

            if (!DA.GetData(0, ref radiusInc)) return;

            DA.SetData(0, new ActionPrecision((int)Math.Round(radiusInc), true));
        }
    }

    [Obsolete("Updated", false)]
    public class PrecisionTo : GH_Component
    {
        public PrecisionTo() : base(
            "PrecisionTo",
            "PrecisionTo",
            "Set the default precision value new actions will be given. Precision is measured as the radius of the smooth interpolation between motion targets. This is refered to as \"Zone\", \"Approximate Positioning\" or \"Blending Radius\" in different platforms.",
            "Machina",
            "Action")
        { }
        public override GH_Exposure Exposure => GH_Exposure.hidden;
        public override Guid ComponentGuid => new Guid("f7127638-e4bc-4cd1-904d-ad301bd63d9a");
        protected override System.Drawing.Bitmap Icon => Properties.Resources.Action_Precision;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Radius", "R", "Smoothing radius value in mm", GH_ParamAccess.item, 5);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Action", "A", "PrecisionTo Action", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double radius = 0;

            if (!DA.GetData(0, ref radius)) return;

            DA.SetData(0, new ActionPrecision((int)Math.Round(radius), false));
        }
    }


    //  ███████╗██████╗ ███████╗███████╗██████╗ 
    //  ██╔════╝██╔══██╗██╔════╝██╔════╝██╔══██╗
    //  ███████╗██████╔╝█████╗  █████╗  ██║  ██║
    //  ╚════██║██╔═══╝ ██╔══╝  ██╔══╝  ██║  ██║
    //  ███████║██║     ███████╗███████╗██████╔╝
    //  ╚══════╝╚═╝     ╚══════╝╚══════╝╚═════╝ 
    //
    [Obsolete("Updated", false)]
    public class Speed : GH_Component
    {
        public Speed() : base(
            "Speed",
            "Speed",
            "Increases the speed in mm/s at which future transformation actions will run.",
            "Machina",
            "Action")
        { }
        public override GH_Exposure Exposure => GH_Exposure.hidden;
        public override Guid ComponentGuid => new Guid("5ce2951b-fdee-4d67-ab2b-bb97204bfdc7");
        protected override System.Drawing.Bitmap Icon => Properties.Resources.Action_Speed;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("SpeedIncrement", "SI", "Speed increment in mm/s", GH_ParamAccess.item, 0);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Action", "A", "Speed Action", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double speedInc = 0;

            if (!DA.GetData(0, ref speedInc)) return;

            DA.SetData(0, new ActionSpeed((int)Math.Round(speedInc), true));
        }
    }
    [Obsolete("Updated", false)]
    public class SpeedTo : GH_Component
    {
        public SpeedTo() : base(
            "SpeedTo",
            "SpeedTo",
            "Sets the speed in mm/s at which future transformation actions will run.",
            "Machina",
            "Action")
        { }
        public override GH_Exposure Exposure => GH_Exposure.hidden;
        public override Guid ComponentGuid => new Guid("3067745a-9183-4f51-96af-95efec967888");
        protected override System.Drawing.Bitmap Icon => Properties.Resources.Action_Speed;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Speed", "S", "Speed value in mm/s", GH_ParamAccess.item, 20);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Action", "A", "SpeedTo Action", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double speed = 0;

            if (!DA.GetData(0, ref speed)) return;

            if (speed < 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "The value of the speed cannot be negative");
            }

            DA.SetData(0, new ActionSpeed((int)Math.Round(speed), false));
        }
    }


    //  ███╗   ███╗ ██████╗ ████████╗██╗ ██████╗ ███╗   ██╗
    //  ████╗ ████║██╔═══██╗╚══██╔══╝██║██╔═══██╗████╗  ██║
    //  ██╔████╔██║██║   ██║   ██║   ██║██║   ██║██╔██╗ ██║
    //  ██║╚██╔╝██║██║   ██║   ██║   ██║██║   ██║██║╚██╗██║
    //  ██║ ╚═╝ ██║╚██████╔╝   ██║   ██║╚██████╔╝██║ ╚████║
    //  ╚═╝     ╚═╝ ╚═════╝    ╚═╝   ╚═╝ ╚═════╝ ╚═╝  ╚═══╝
    //                                                     
    [Obsolete("Updated", false)]
    public class MotionType : GH_Component
    {
        public MotionType() : base(
            "MotionType",  // the name that shows up on the tab, on yellow bar on toolip, on component on 'Draw Full Names'
            "MotionType",  // the name that shows up on the non-icon component with 'DFN' off, and in parenthesis after the main name on the yellow bar on tooltip
            "Sets the current type of motion to be applied to future translation actions. This can be \"linear\" (default) for straight line movements in euclidean space, or \"joint\" for smooth interpolation between joint angles. NOTE: \"joint\" motion may produce unexpected trajectories resulting in reorientations or collisions. Use with caution!",
            "Machina",
            "Action")
        { }
        public override GH_Exposure Exposure => GH_Exposure.hidden;
        public override Guid ComponentGuid => new Guid("1a97b12b-0422-46aa-945f-373f9afdc39a");
        protected override System.Drawing.Bitmap Icon => Properties.Resources.Action_MotionMode;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Type", "T", "\"linear\" or \"joint\"", GH_ParamAccess.item, "linear");
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Action", "A", "MotionType Action", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Obsolete component, please update Machina and use newer version.");
        }
    }



    [Obsolete("Updated", false)]
    public class Move : GH_Component
    {
        public Move() : base(
            "Move",
            "Move",
            "Moves the device along a speficied vector relative to its current position.",
            "Machina",
            "Action")
        { }
        public override GH_Exposure Exposure => GH_Exposure.hidden;
        public override Guid ComponentGuid => new Guid("1969ae91-3dad-4af2-991b-b23e60724873");
        protected override System.Drawing.Bitmap Icon => Properties.Resources.Action_Move;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddVectorParameter("Vector", "V", "Translation vector", GH_ParamAccess.item, Vector3d.Zero);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Action", "A", "Move Action", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Vector3d v = Vector3d.Zero;

            if (!DA.GetData(0, ref v)) return;

            DA.SetData(0, new ActionTranslation(new Machina.Vector(v.X, v.Y, v.Z), true));
        }
    }

    [Obsolete("Updated", false)]
    public class MoveTo : GH_Component
    {
        public MoveTo() : base(
            "MoveTo",
            "MoveTo",
            "Moves the device to an absolute position in global coordinates.",
            "Machina",
            "Action")
        { }
        public override GH_Exposure Exposure => GH_Exposure.hidden;
        public override Guid ComponentGuid => new Guid("c90a1258-1dd2-4d14-ab13-8dc53a47b547");
        protected override System.Drawing.Bitmap Icon => Properties.Resources.Action_Move;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Point", "P", "Target position", GH_ParamAccess.item, Point3d.Origin);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Action", "A", "MoveTo Action", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Point3d p = Point3d.Origin;

            if (!DA.GetData(0, ref p)) return;

            DA.SetData(0, new ActionTranslation(new Machina.Point(p.X, p.Y, p.Z), false));
        }
    }


    //  ██████╗  ██████╗ ████████╗ █████╗ ████████╗███████╗
    //  ██╔══██╗██╔═══██╗╚══██╔══╝██╔══██╗╚══██╔══╝██╔════╝
    //  ██████╔╝██║   ██║   ██║   ███████║   ██║   █████╗  
    //  ██╔══██╗██║   ██║   ██║   ██╔══██║   ██║   ██╔══╝  
    //  ██║  ██║╚██████╔╝   ██║   ██║  ██║   ██║   ███████╗
    //  ╚═╝  ╚═╝ ╚═════╝    ╚═╝   ╚═╝  ╚═╝   ╚═╝   ╚══════╝
    //                                                     
    [Obsolete("Updated", false)]
    public class Rotate : GH_Component
    {
        public Rotate() : base(
            "Rotate",
            "Rotate",
            "Rotates the device a specified angle in degrees along the specified vector.",
            "Machina",
            "Action")
        { }
        public override GH_Exposure Exposure => GH_Exposure.hidden;
        public override Guid ComponentGuid => new Guid("db2e3c56-5973-4f07-8d6a-ba31c659704d");
        protected override System.Drawing.Bitmap Icon => Properties.Resources.Action_Rotate;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddVectorParameter("Axis", "V", "Rotation axis (positive rotation direction is defined by the right-hand rule).", GH_ParamAccess.item, Vector3d.XAxis);
            pManager.AddNumberParameter("Angle", "A", "Rotation angle in degrees", GH_ParamAccess.item, 0);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Action", "A", "Rotate Action", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Vector3d v = Vector3d.Zero;
            double ang = 0;

            if (!DA.GetData(0, ref v)) return;
            if (!DA.GetData(1, ref ang)) return;

            DA.SetData(0, new ActionRotation(new Rotation(v.X, v.Y, v.Z, ang), true));
        }
    }

    [Obsolete("Updated", false)]
    public class RotateTo : GH_Component
    {
        public RotateTo() : base(
            "RotateTo",
            "RotateTo",
            "Rotate the devices to an absolute orientation defined by the two main X and Y axes of specified Plane.",
            "Machina",
            "Action")
        { }
        public override GH_Exposure Exposure => GH_Exposure.hidden;
        public override Guid ComponentGuid => new Guid("9410b629-1016-486f-8464-85ecfd9500f7");
        protected override System.Drawing.Bitmap Icon => Properties.Resources.Action_Rotate;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddPlaneParameter("Plane", "P", "Target spatial orientation", GH_ParamAccess.item, Plane.WorldXY);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Action", "A", "RotateTo Action", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Plane pl = Plane.Unset;

            if (!DA.GetData(0, ref pl)) return;

            DA.SetData(0, new ActionRotation(new Machina.Orientation(pl.XAxis.X, pl.XAxis.Y, pl.XAxis.Z, pl.YAxis.X, pl.YAxis.Y, pl.YAxis.Z), false));
        }
    }



    //  ████████╗██████╗  █████╗ ███╗   ██╗███████╗███████╗ ██████╗ ██████╗ ███╗   ███╗
    //  ╚══██╔══╝██╔══██╗██╔══██╗████╗  ██║██╔════╝██╔════╝██╔═══██╗██╔══██╗████╗ ████║
    //     ██║   ██████╔╝███████║██╔██╗ ██║███████╗█████╗  ██║   ██║██████╔╝██╔████╔██║
    //     ██║   ██╔══██╗██╔══██║██║╚██╗██║╚════██║██╔══╝  ██║   ██║██╔══██╗██║╚██╔╝██║
    //     ██║   ██║  ██║██║  ██║██║ ╚████║███████║██║     ╚██████╔╝██║  ██║██║ ╚═╝ ██║
    //     ╚═╝   ╚═╝  ╚═╝╚═╝  ╚═╝╚═╝  ╚═══╝╚══════╝╚═╝      ╚═════╝ ╚═╝  ╚═╝╚═╝     ╚═╝
    //                                                                                 
    [Obsolete("Updated", false)]
    public class Transform : GH_Component
    {
        public Transform() : base(
            "Transform",
            "Transform",
            "Performs a compound relative rotation + translation transformation in a single action. Note that when performing relative transformations, the R+T versus T+R order matters.",
            "Machina",
            "Action")
        { }
        public override GH_Exposure Exposure => GH_Exposure.hidden;
        public override Guid ComponentGuid => new Guid("81226a72-37d6-4dad-a7a3-711adb51b26e");
        protected override System.Drawing.Bitmap Icon => Properties.Resources.Action_Transform;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddVectorParameter("Direction", "V", "Translation vector", GH_ParamAccess.item, Vector3d.Zero);
            pManager.AddVectorParameter("Axis", "V", "Rotation axis (positive rotation direction is defined by the right-hand rule).", GH_ParamAccess.item, Vector3d.XAxis);
            pManager.AddNumberParameter("Angle", "A", "Rotation angle in degrees", GH_ParamAccess.item, 0);
            pManager.AddBooleanParameter("TranslationFirst", "t", "Apply translation first? Note that when performing relative transformations, the R+T versus T+R order matters.", GH_ParamAccess.item, true);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Action", "A", "Transform Action", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Vector3d dir = Vector3d.Zero;
            Vector3d axis = Vector3d.Zero;
            double ang = 0;
            bool trans = false;

            if (!DA.GetData(0, ref dir)) return;
            if (!DA.GetData(1, ref axis)) return;
            if (!DA.GetData(2, ref ang)) return;
            if (!DA.GetData(3, ref trans)) return;

            DA.SetData(0, new ActionTransformation(
                new Machina.Vector(dir.X, dir.Y, dir.Z),
                new Rotation(axis.X, axis.Y, axis.Z, ang),
                true,
                trans));
        }
    }

    [Obsolete("Updated", false)]
    public class TransformTo : GH_Component
    {
        public TransformTo() : base(
            "TransformTo",
            "TransformTo",
            "Performs a compound absolute rotation + translation transformation, or in other words, sets both a new absolute position and orientation for the device in the same action, based on specified Plane.",
            "Machina",
            "Action")
        { }
        public override GH_Exposure Exposure => GH_Exposure.hidden;
        public override Guid ComponentGuid => new Guid("7bf2965f-7fa6-4cf0-84ac-4948b777b478");
        protected override System.Drawing.Bitmap Icon => Properties.Resources.Action_Transform;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddPlaneParameter("Plane", "P", "Target Plane to transform to", GH_ParamAccess.item, Plane.WorldXY);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Action", "A", "TransformTo Action", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Plane pl = Plane.Unset;

            if (!DA.GetData(0, ref pl)) return;

            DA.SetData(0, new ActionTransformation(
                new Machina.Vector(pl.Origin.X, pl.Origin.Y, pl.Origin.Z),
                new Machina.Orientation(pl.XAxis.X, pl.XAxis.Y, pl.XAxis.Z, pl.YAxis.X, pl.YAxis.Y, pl.YAxis.Z),
                false,
                true));
        }
    }



    //   █████╗ ██╗  ██╗███████╗███████╗
    //  ██╔══██╗╚██╗██╔╝██╔════╝██╔════╝
    //  ███████║ ╚███╔╝ █████╗  ███████╗
    //  ██╔══██║ ██╔██╗ ██╔══╝  ╚════██║
    //  ██║  ██║██╔╝ ██╗███████╗███████║
    //  ╚═╝  ╚═╝╚═╝  ╚═╝╚══════╝╚══════╝
    //                                 
    [Obsolete("Updated", false)]
    public class Axes : GH_Component
    {
        public Axes() : base(
            "Axes",
            "Axes",
            "Increase the axes' rotation angle in degrees at the joints of mechanical devices, specially robotic arms.",
            "Machina",
            "Action")
        { }
        public override GH_Exposure Exposure => GH_Exposure.hidden;
        public override Guid ComponentGuid => new Guid("15ea7b44-c5d8-470b-9edb-867cc4c0b1aa");
        protected override System.Drawing.Bitmap Icon => Properties.Resources.Action_Axes;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("A1Inc", "A1", "Rotational increment in degrees for Axis 1", GH_ParamAccess.item, 0);
            pManager.AddNumberParameter("A2Inc", "A2", "Rotational increment in degrees for Axis 2", GH_ParamAccess.item, 0);
            pManager.AddNumberParameter("A3Inc", "A3", "Rotational increment in degrees for Axis 3", GH_ParamAccess.item, 0);
            pManager.AddNumberParameter("A4Inc", "A4", "Rotational increment in degrees for Axis 4", GH_ParamAccess.item, 0);
            pManager.AddNumberParameter("A5Inc", "A5", "Rotational increment in degrees for Axis 5", GH_ParamAccess.item, 0);
            pManager.AddNumberParameter("A6Inc", "A6", "Rotational increment in degrees for Axis 6", GH_ParamAccess.item, 0);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Action", "A", "Axis Action", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double a1inc = 0;
            double a2inc = 0;
            double a3inc = 0;
            double a4inc = 0;
            double a5inc = 0;
            double a6inc = 0;

            if (!DA.GetData(0, ref a1inc)) return;
            if (!DA.GetData(1, ref a2inc)) return;
            if (!DA.GetData(2, ref a3inc)) return;
            if (!DA.GetData(3, ref a4inc)) return;
            if (!DA.GetData(4, ref a5inc)) return;
            if (!DA.GetData(5, ref a6inc)) return;

            DA.SetData(0, new ActionAxes(new Joints(a1inc, a2inc, a3inc, a4inc, a5inc, a6inc), true));
        }
    }
    [Obsolete("Updated", false)]
    public class AxesTo : GH_Component
    {
        public AxesTo() : base(
            "AxesTo",
            "AxesTo",
            "Set the axes' rotation angle in degrees at the joints of mechanical devices, specially robotic arms.",
            "Machina",
            "Action")
        { }
        public override GH_Exposure Exposure => GH_Exposure.hidden;
        public override Guid ComponentGuid => new Guid("5b9bc63f-a0f1-4d66-b6a6-679c38ed8014");
        protected override System.Drawing.Bitmap Icon => Properties.Resources.Action_Axes;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("A1", "A1", "Angular value in degrees for Axis 1", GH_ParamAccess.item, 0);
            pManager.AddNumberParameter("A2", "A2", "Angular value in degrees for Axis 2", GH_ParamAccess.item, 0);
            pManager.AddNumberParameter("A3", "A3", "Angular value in degrees for Axis 3", GH_ParamAccess.item, 0);
            pManager.AddNumberParameter("A4", "A4", "Angular value in degrees for Axis 4", GH_ParamAccess.item, 0);
            pManager.AddNumberParameter("A5", "A5", "Angular value in degrees for Axis 5", GH_ParamAccess.item, 0);
            pManager.AddNumberParameter("A6", "A6", "Angular value in degrees for Axis 6", GH_ParamAccess.item, 0);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Action", "A", "Axis Action", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double a1 = 0;
            double a2 = 0;
            double a3 = 0;
            double a4 = 0;
            double a5 = 0;
            double a6 = 0;

            if (!DA.GetData(0, ref a1)) return;
            if (!DA.GetData(1, ref a2)) return;
            if (!DA.GetData(2, ref a3)) return;
            if (!DA.GetData(3, ref a4)) return;
            if (!DA.GetData(4, ref a5)) return;
            if (!DA.GetData(5, ref a6)) return;

            DA.SetData(0, new ActionAxes(new Joints(a1, a2, a3, a4, a5, a6), false));
        }
    }

    //  ███████╗███████╗████████╗██╗ ██████╗ ███╗   ██╗ █████╗ ███╗   ███╗███████╗
    //  ██╔════╝██╔════╝╚══██╔══╝██║██╔═══██╗████╗  ██║██╔══██╗████╗ ████║██╔════╝
    //  ███████╗█████╗     ██║   ██║██║   ██║██╔██╗ ██║███████║██╔████╔██║█████╗  
    //  ╚════██║██╔══╝     ██║   ██║██║   ██║██║╚██╗██║██╔══██║██║╚██╔╝██║██╔══╝  
    //  ███████║███████╗   ██║   ██║╚██████╔╝██║ ╚████║██║  ██║██║ ╚═╝ ██║███████╗
    //  ╚══════╝╚══════╝   ╚═╝   ╚═╝ ╚═════╝ ╚═╝  ╚═══╝╚═╝  ╚═╝╚═╝     ╚═╝╚══════╝
    //                                                                            
    [Obsolete("WriteDigital/Analog can now take a string as input", false)]
    public class SetIOName : GH_Component
    {
        public SetIOName() : base(
            "SetIOName",
            "SetIOName",
            "Change the name of a Robot's IO pins.",
            "Machina",
            "Robots")
        { }
        public override GH_Exposure Exposure => GH_Exposure.hidden;
        public override Guid ComponentGuid => new Guid("7c3fe2f8-bc12-4eaa-92c1-27a6729504ac");
        protected override System.Drawing.Bitmap Icon => null;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Robot", "R", "Robot to change the IO name to", GH_ParamAccess.item);
            pManager.AddTextParameter("Name", "N", "New IO name", GH_ParamAccess.item, "Digital_IO_1");
            pManager.AddIntegerParameter("Pin", "N", "Pin number", GH_ParamAccess.item, 1);
            pManager.AddBooleanParameter("Digital", "d", "Is this a digital pin?", GH_ParamAccess.item, true);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Robot", "R", "Robot with named IO", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //Machina.Robot bot = null;
            //string name = "";
            //int pin = 1;
            //bool digital = true;

            //if (!DA.GetData(0, ref bot)) return;
            //if (!DA.GetData(1, ref name)) return;
            //if (!DA.GetData(2, ref pin)) return;
            //if (!DA.GetData(3, ref digital)) return;

            //bot.SetIOName(name, pin, digital);
            //DA.SetData(0, "DEPRECATED, use named pins instead");

            AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"ERROR: DEPRECATED component, use named pins instead");
        }
    }



    //  ███████╗███████╗███╗   ██╗██████╗ ████████╗ ██████╗ ██████╗ ██████╗ ██╗██████╗  ██████╗ ███████╗
    //  ██╔════╝██╔════╝████╗  ██║██╔══██╗╚══██╔══╝██╔═══██╗██╔══██╗██╔══██╗██║██╔══██╗██╔════╝ ██╔════╝
    //  ███████╗█████╗  ██╔██╗ ██║██║  ██║   ██║   ██║   ██║██████╔╝██████╔╝██║██║  ██║██║  ███╗█████╗  
    //  ╚════██║██╔══╝  ██║╚██╗██║██║  ██║   ██║   ██║   ██║██╔══██╗██╔══██╗██║██║  ██║██║   ██║██╔══╝  
    //  ███████║███████╗██║ ╚████║██████╔╝   ██║   ╚██████╔╝██████╔╝██║  ██║██║██████╔╝╚██████╔╝███████╗
    //  ╚══════╝╚══════╝╚═╝  ╚═══╝╚═════╝    ╚═╝    ╚═════╝ ╚═════╝ ╚═╝  ╚═╝╚═╝╚═════╝  ╚═════╝ ╚══════╝
    //                                                       
    [Obsolete("Updated", false)]
    public class SendToBridge : GH_Component
    {
        //private bool _sentOnce = false;
        private WebSocket _ws;
        //private string _url;

        public SendToBridge() : base(
            "SendToBridge",
            "SendToBridge",
            "Send a list of Action to the Machina Bridge App to be streamed to a robot.",
            "Machina",
            "Program")
        { }
        public override GH_Exposure Exposure => GH_Exposure.hidden;
        public override Guid ComponentGuid => new Guid("4442d8a9-3b6e-4197-ad58-93caa6c83e3e");
        protected override System.Drawing.Bitmap Icon => null;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("BridgeURL", "URL", "The URL of the Machina Bridge App.", GH_ParamAccess.item, "ws://127.0.0.1:6999/Bridge");
            pManager.AddBooleanParameter("Connect?", "C", "Connect to Machina Bridge App?", GH_ParamAccess.item, false);
            pManager.AddGenericParameter("Action", "A", "A program in the form of a list of Action", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Send", "S", "Send Actions?", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddBooleanParameter("Connected?", "C", "Is the connection to the Bridge live?", GH_ParamAccess.item);
            pManager.AddTextParameter("Sent?", "ok", "Correctly sent?", GH_ParamAccess.item);
            pManager.AddTextParameter("Instructions", "I", "Streamed Instructions", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Obsolete component, please update Machina and use newer version.");
        }

    }


    //       ██╗ ██████╗ ██╗███╗   ██╗████████╗ █████╗  ██████╗ ██████╗███████╗██╗     ███████╗██████╗  █████╗ ████████╗██╗ ██████╗ ███╗   ██╗
    //       ██║██╔═══██╗██║████╗  ██║╚══██╔══╝██╔══██╗██╔════╝██╔════╝██╔════╝██║     ██╔════╝██╔══██╗██╔══██╗╚══██╔══╝██║██╔═══██╗████╗  ██║
    //       ██║██║   ██║██║██╔██╗ ██║   ██║   ███████║██║     ██║     █████╗  ██║     █████╗  ██████╔╝███████║   ██║   ██║██║   ██║██╔██╗ ██║
    //  ██   ██║██║   ██║██║██║╚██╗██║   ██║   ██╔══██║██║     ██║     ██╔══╝  ██║     ██╔══╝  ██╔══██╗██╔══██║   ██║   ██║██║   ██║██║╚██╗██║
    //  ╚█████╔╝╚██████╔╝██║██║ ╚████║   ██║   ██║  ██║╚██████╗╚██████╗███████╗███████╗███████╗██║  ██║██║  ██║   ██║   ██║╚██████╔╝██║ ╚████║
    //   ╚════╝  ╚═════╝ ╚═╝╚═╝  ╚═══╝   ╚═╝   ╚═╝  ╚═╝ ╚═════╝ ╚═════╝╚══════╝╚══════╝╚══════╝╚═╝  ╚═╝╚═╝  ╚═╝   ╚═╝   ╚═╝ ╚═════╝ ╚═╝  ╚═══╝
    //            
    [Obsolete("Deprecated", false)]
    public class JointAcceleration : GH_MutableComponent
    {
        public JointAcceleration() : base(
            "JointAcceleration",
            "JointAcceleration",
            "Change the maximum joint angular rotation acceleration value. Movement will be constrained so that the fastest joint accelerates below this threshold.",
            "Machina",
            "Action")
        { }
        public override GH_Exposure Exposure => GH_Exposure.hidden;
        public override Guid ComponentGuid => new Guid("eb2e55d0-2cc3-46b5-86e6-d5beaf9cf3c8");
        protected override System.Drawing.Bitmap Icon => null;

        protected override bool ShallowInputMutation => true;

        protected override void RegisterMutableInputParams(GH_MutableInputParamManager mpManager)
        {
            // Absolute
            mpManager.AddComponentNames(false, "JointAccelerationTo", "JointAccelerationTo", "Set the maximum joint angular rotation acceleration value. Movement will be constrained so that the fastest joint accelerates below this threshold.");
            mpManager.AddParameter(false, typeof(Param_Number), "JointAccelerationInc", "JA", "Maximum joint angular rotation acceleration increment in deg/s^2. Decreasing the total to zero or less will reset it back to the robot's default.", GH_ParamAccess.item);

            // Relative
            mpManager.AddComponentNames(true, "JointAcceleration", "JointAcceleration", "Increase the maximum joint angular rotation acceleration value. Movement will be constrained so that the fastest joint accelerates below this threshold.");
            mpManager.AddParameter(true, typeof(Param_Number), "JointAccelerationInc", "JA", "Maximum joint angular rotation acceleration increment in deg/s^2. Decreasing the total to zero or less will reset it back to the robot's default.", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Action", "A", "JointAcceleration Action", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Obsolete component, please update Acceleration instead.");
        }
    }

    //       ██╗ ██████╗ ██╗███╗   ██╗████████╗███████╗██████╗ ███████╗███████╗██████╗ 
    //       ██║██╔═══██╗██║████╗  ██║╚══██╔══╝██╔════╝██╔══██╗██╔════╝██╔════╝██╔══██╗
    //       ██║██║   ██║██║██╔██╗ ██║   ██║   ███████╗██████╔╝█████╗  █████╗  ██║  ██║
    //  ██   ██║██║   ██║██║██║╚██╗██║   ██║   ╚════██║██╔═══╝ ██╔══╝  ██╔══╝  ██║  ██║
    //  ╚█████╔╝╚██████╔╝██║██║ ╚████║   ██║   ███████║██║     ███████╗███████╗██████╔╝
    //   ╚════╝  ╚═════╝ ╚═╝╚═╝  ╚═══╝   ╚═╝   ╚══════╝╚═╝     ╚══════╝╚══════╝╚═════╝ 
    //                      
    [Obsolete("Deprecated", false)]
    public class JointSpeed : GH_MutableComponent
    {
        public JointSpeed() : base(
            "JointSpeed",
            "JointSpeed",
            "Change the maximum joint angular rotation speed value. Movement will be constrained so that the fastest joint rotates below this threshold.",
            "Machina",
            "Action")
        { }
        public override GH_Exposure Exposure => GH_Exposure.hidden;
        public override Guid ComponentGuid => new Guid("b39c9746-43f0-4fdb-bab4-37b920690dbc");
        protected override System.Drawing.Bitmap Icon => null;

        protected override bool ShallowInputMutation => true;

        protected override void RegisterMutableInputParams(GH_MutableInputParamManager mpManager)
        {
            // Absolute
            mpManager.AddComponentNames(false, "JointSpeedTo", "JointSpeedTo", "Set the maximum joint angular rotation speed value. Movement will be constrained so that the fastest joint rotates below this threshold.");
            mpManager.AddParameter(false, typeof(Param_Number), "JointSpeed", "JS", "Maximum joint angular rotation speed value in deg/s. Decreasing the total to zero or less will reset it back to the robot's default.", GH_ParamAccess.item);

            // Relative
            mpManager.AddComponentNames(true, "JointSpeed", "JointSpeed", "Increase the maximum joint angular rotation speed value. Movement will be constrained so that the fastest joint rotates below this threshold.");
            mpManager.AddParameter(true, typeof(Param_Number), "JointSpeedInc", "JS", "Maximum joint angular rotation speed increment in deg/s. Decreasing the total to zero or less will reset it back to the robot's default.", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Action", "A", "JointSpeed Action", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Obsolete component, please update Speed instead.");
        }
    }

    //  ██████╗  ██████╗ ████████╗ █████╗ ████████╗██╗ ██████╗ ███╗   ██╗███████╗██████╗ ███████╗███████╗██████╗ 
    //  ██╔══██╗██╔═══██╗╚══██╔══╝██╔══██╗╚══██╔══╝██║██╔═══██╗████╗  ██║██╔════╝██╔══██╗██╔════╝██╔════╝██╔══██╗
    //  ██████╔╝██║   ██║   ██║   ███████║   ██║   ██║██║   ██║██╔██╗ ██║███████╗██████╔╝█████╗  █████╗  ██║  ██║
    //  ██╔══██╗██║   ██║   ██║   ██╔══██║   ██║   ██║██║   ██║██║╚██╗██║╚════██║██╔═══╝ ██╔══╝  ██╔══╝  ██║  ██║
    //  ██║  ██║╚██████╔╝   ██║   ██║  ██║   ██║   ██║╚██████╔╝██║ ╚████║███████║██║     ███████╗███████╗██████╔╝
    //  ╚═╝  ╚═╝ ╚═════╝    ╚═╝   ╚═╝  ╚═╝   ╚═╝   ╚═╝ ╚═════╝ ╚═╝  ╚═══╝╚══════╝╚═╝     ╚══════╝╚══════╝╚═════╝ 
    //                                                                            
    [Obsolete("Deprecated", false)]
    public class RotationSpeed : GH_MutableComponent
    {
        public RotationSpeed() : base(
            "RotationSpeed",
            "RotationSpeed",
            "Changes the TCP angular rotation speed value new Actions will be ran at.",
            "Machina",
            "Action")
        { }
        public override GH_Exposure Exposure => GH_Exposure.hidden;
        public override Guid ComponentGuid => new Guid("fa13c9cf-f136-4d12-ae90-89baa41ce928");
        protected override System.Drawing.Bitmap Icon => null;

        protected override bool ShallowInputMutation => true;

        protected override void RegisterMutableInputParams(GH_MutableInputParamManager mpManager)
        {
            // Absolute
            mpManager.AddComponentNames(false, "RotationSpeedTo", "RotationSpeedTo", "Increases the TCP angular rotation speed value new Actions  will run at.");
            mpManager.AddParameter(false, typeof(Param_Number), "RotationSpeedInc", "RS", "TCP angular rotation speed increment in deg/s. Decreasing the total to zero or less will reset it back to the robot's default.", GH_ParamAccess.item);

            // Relative
            mpManager.AddComponentNames(true, "RotationSpeed", "RotationSpeed", "Sets the TCP angular rotation speed value new Actions will run at.");
            mpManager.AddParameter(true, typeof(Param_Number), "RotationSpeedInc", "RS", "TCP angular rotation speed value in deg/s. Setting this value to zero or less will reset it back to the robot's default.", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Action", "A", "RotationSpeed Action", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Obsolete component, please update Speed instead.");
        }
    }


    //  ████████╗ ██████╗  ██████╗ ██╗        ███╗   ██╗███████╗██╗    ██╗
    //  ╚══██╔══╝██╔═══██╗██╔═══██╗██║        ████╗  ██║██╔════╝██║    ██║
    //     ██║   ██║   ██║██║   ██║██║        ██╔██╗ ██║█████╗  ██║ █╗ ██║
    //     ██║   ██║   ██║██║   ██║██║        ██║╚██╗██║██╔══╝  ██║███╗██║
    //     ██║   ╚██████╔╝╚██████╔╝███████╗██╗██║ ╚████║███████╗╚███╔███╔╝
    //     ╚═╝    ╚═════╝  ╚═════╝ ╚══════╝╚═╝╚═╝  ╚═══╝╚══════╝ ╚══╝╚══╝ 
    //               
    [Obsolete("Deprecated", false)]
    public class ToolCreate : GH_Component
    {
        public ToolCreate() : base(
            "Tool.Create",
            "Tool.Create",
            "Create a new Tool object.",
            "Machina",
            "Tool")
        { }
        public override GH_Exposure Exposure => GH_Exposure.hidden;
        public override Guid ComponentGuid => new Guid("19e1c38a-94f8-41b6-b5a5-0a549fdf0123");
        protected override System.Drawing.Bitmap Icon => null;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "T", "Tool name", GH_ParamAccess.item, "ToolExMachina");
            pManager.AddPlaneParameter("BasePlane", "BP", "Base Plane where the Tool will be attached to the Robot", GH_ParamAccess.item, Plane.WorldXY);
            pManager.AddPlaneParameter("TCPPlane", "TP", "Plane of the Tool Tip Center (TCP)", GH_ParamAccess.item, Plane.WorldXY);
            pManager.AddNumberParameter("Weight", "W", "Tool weight in Kg", GH_ParamAccess.item, 1);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Tool", "T", "New Tool object", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //string name = "";
            //Plane bpl = Plane.WorldXY;
            //Plane tcppl = Plane.WorldXY;
            //double w = 0;

            //if (!DA.GetData(0, ref name)) return;
            //if (!DA.GetData(1, ref bpl)) return;
            //if (!DA.GetData(2, ref tcppl)) return;
            //if (!DA.GetData(3, ref w)) return;

            //// Create a TCP plane as 
            //Rhino.Geometry.Transform rel = Rhino.Geometry.Transform.ChangeBasis(Plane.WorldXY, bpl);
            //if (!tcppl.Transform(rel))
            //{
            //    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid input planes");
            //    return;
            //}

            //Point3d cog = 0.5 * tcppl.Origin;
            //Machina.Tool tool = Machina.Tool.Create(name,
            //    tcppl.OriginX, tcppl.OriginY, tcppl.OriginZ,
            //    tcppl.XAxis.X, tcppl.XAxis.Y, tcppl.XAxis.Z, tcppl.YAxis.X, tcppl.YAxis.Y, tcppl.YAxis.Z,
            //    w,
            //    cog.X, cog.Y, cog.Z);

            //DA.SetData(0, tool);

            AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Obsolete component, please update \"DefineTool\" instead.");
        }
    }

}
