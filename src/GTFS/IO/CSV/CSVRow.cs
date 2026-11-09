using System;

namespace GTFS.IO.CSV
{
    internal static class CSVRow
    {
        internal static void Parse(string line, char separator, ref string[] fields)
        {
            if (fields == null) fields = new string[20];

            int start = 0;
            int count = 0;
            bool quoted = false;
            for (int i = 0; i <= line.Length; i++)
            {
                if (i < line.Length)
                {
                    if (line[i] == '"') quoted = !quoted;
                    if (line[i] != separator || quoted) continue;
                }

                if (count == fields.Length) Array.Resize(ref fields, fields.Length * 2);
                int length = i - start;
                if (length >= 2 && line[start] == '"' && line[i - 1] == '"')
                {
                    fields[count++] = line.Substring(start + 1, length - 2).Replace("\"\"", "\"");
                }
                else
                {
                    fields[count++] = line.Substring(start, length);
                }
                start = i + 1;
            }

            if (count != fields.Length) Array.Resize(ref fields, count);
        }
    }
}
