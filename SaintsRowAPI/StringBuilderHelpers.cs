using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintsRowAPI
{
    public static class StringBuilderHelpers
    {
        public static void AppendFormatLine(this StringBuilder sb, string format, params object[] args)
        {
            sb.AppendLine(String.Format(format, args));
        }
    }
}
