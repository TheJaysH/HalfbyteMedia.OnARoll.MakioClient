using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalfbyteMedia.OnARoll.MakioClient.Config
{
    class EngineConfig
    {
        public float SlowMoTimeScale { get; set; }
        public float Radius { get; set; }
        public float MinLaunchSpeed { get; set; }
        public float MinCessSlideSpeed { get; set; }
        public float StepSize { get; set; }
        public float DragCoefficient { get; set; }
        public float MassDensity { get; set; }
        public float ReferenceArea { get; set; }
        public float Gravity { get; set; }
        public float ErrorMargin { get; set; }
        public bool DebugMode { get; set; }

        public float MaxJumpPreparationTime { get; set; }
        public float MinJumpPowerFactor { get; set; }
        public float MaxJumpPower { get; set; }
        public float JumpPower { get; set; }
        public float HighestAirPointRelativeTime { get; set; }        
        public int TrickSpinSpeed { get; set; }
        public int MaxFlatSpinSpeed { get; set; }
        public int MaxCorkScrewSpeed { get; set; }
        public int MaxBioSpeed { get; set; }
        public int MaxFlipSpeed { get; set; }
        public float TrickLandingTime { get; set; }
        public float MinLongAirTime { get; set; }
    }
}
