using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRunner
{
    static class Defines
    {
        public static string Version = "V0.1.1";

        public enum Mode { Debug,Release};
        public static Mode CurrentMode = Mode.Release;

    }
}
