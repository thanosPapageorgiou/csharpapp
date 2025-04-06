using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpApp.Application.Utilities
{
    public static class ConversionUtils
    {
        public static byte[] ConvertToByte(string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }
        public static string ConvertToString(byte[] value)
        {
            string output = string.Empty;
            if (value != null)
            {
                output = Encoding.UTF8.GetString(value);
            }
            return output;
        }
    }
}
