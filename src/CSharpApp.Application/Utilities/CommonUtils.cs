using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpApp.Application.Utilities
{
    public static class CommonUtils
    {
        public static bool IsRunningFromUnitTest()
        {
            var isUnitTest = Environment.GetEnvironmentVariable("IS_UNIT_TEST");
            return isUnitTest == "true";
        }
    }
}
