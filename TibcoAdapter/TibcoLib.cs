using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibcoAdapter
{
    public static class TibcoLib
    {
        public static Dictionary<string, string> TibcoToDictionary(string tibcoMessage)
        {
            Dictionary<string, string> resultDictionary = new Dictionary<string, string>();

            var tibcoColumns = tibcoMessage.Split(new char[] { '{', '}', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var tibcoColumn in tibcoColumns)
            {
                if (tibcoColumn.Trim().StartsWith("\"") && tibcoColumn.Trim().EndsWith("\""))
                {
                    var columns = tibcoColumn.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (columns.Length == 2)
                    {
                        string key = columns[0].TrimStart('\"').TrimEnd('\"');
                        string value = columns[1].TrimStart('\"').TrimEnd('\"');
                        resultDictionary.Add(key, value);
                    }
                }
            }

            return resultDictionary;
        }

        public static Dictionary<string, string> McsToDictionary(string mcsMessage)
        {
            Dictionary<string, string> resultDictionary = new Dictionary<string, string>();

            var parameters = mcsMessage.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var parameter in parameters)
            {
                var columns = parameter.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                if (columns.Length == 2)
                    resultDictionary.Add(columns[0], columns[1]);
                else if (columns.Length == 1)
                    resultDictionary.Add(columns[0], string.Empty);
            }

            return resultDictionary;
        }
    }
}
