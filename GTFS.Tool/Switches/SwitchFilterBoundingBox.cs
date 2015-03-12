using GTFS.Tool.Switches.Processors;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTFS.Tool.Switches
{
    /// <summary>
    /// Represents a switch for filtering data along a bounding box.
    /// </summary>
    public class SwitchFilterBoundingBox : Switch
    {   
        /// <summary>
        /// Returns the switches for this command.
        /// </summary>
        /// <returns></returns>
        public override string[] GetSwitch()
        {
            return new string[] { "--bb", "--bounding-box" };
        }

        /// <summary>
        /// The right bound.
        /// </summary>
        public float Top { get; set; }

        /// <summary>
        /// The top bound.
        /// </summary>
        public float Bottom { get; set; }

        /// <summary>
        /// The left bound.
        /// </summary>
        public float Left { get; set; }

        /// <summary>
        /// The right bound.
        /// </summary>
        public float Right { get; set; }

        /// <summary>
        /// Parse the command arguments for the bounding-box command.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="idx"></param>
        /// <param name="switchOut"></param>
        /// <returns></returns>
        public override int Parse(string[] args, int idx, out Switch switchOut)
        {
            // check next argument.
            if (args.Length < idx + 3)
            {
                throw new SwitchParserException("None", "Invalid bounding-box command!");
            }

            bool topOk = false, bottomOk = false, leftOk = false, rightOk = false;
            float top = 0, bottom = 0, left = 0, right = 0;
            for (int currentArg = idx; currentArg < idx + 4; currentArg++)
            {
                string[] argSplit = args[currentArg].Split('=');

                if (argSplit.Length != 2 ||
                    argSplit[0] == null ||
                    argSplit[1] == null)
                {
                    throw new SwitchParserException(args[currentArg],
                                                         "Invalid boundary condition for boundingbox command!");
                }

                argSplit[0] = argSplit[0].ToLower();
                argSplit[0] = SwitchParser.RemoveQuotes(argSplit[0]);
                argSplit[1] = SwitchParser.RemoveQuotes(argSplit[1]);
                if (argSplit[0] == "top")
                {
                    if (
                        !float.TryParse(argSplit[1], NumberStyles.Float,
                                        System.Globalization.CultureInfo.InvariantCulture, out top))
                    {
                        throw new SwitchParserException(args[currentArg],
                                                             "Invalid boundary condition for boundingbox command!");
                    }
                    topOk = true;
                }
                else if (argSplit[0] == "left")
                {
                    if (
                        !float.TryParse(argSplit[1], NumberStyles.Float,
                                        System.Globalization.CultureInfo.InvariantCulture, out left))
                    {
                        throw new SwitchParserException(args[currentArg],
                                                             "Invalid boundary condition for boundingbox command!");
                    }
                    leftOk = true;
                }
                else if (argSplit[0] == "bottom")
                {
                    if (
                        !float.TryParse(argSplit[1], NumberStyles.Float,
                                        System.Globalization.CultureInfo.InvariantCulture, out bottom))
                    {
                        throw new SwitchParserException(args[currentArg],
                                                             "Invalid boundary condition for boundingbox command!");
                    }
                    bottomOk = true;
                }
                else if (argSplit[0] == "right")
                {
                    if (
                        !float.TryParse(argSplit[1], NumberStyles.Float,
                                        System.Globalization.CultureInfo.InvariantCulture, out right))
                    {
                        throw new SwitchParserException(args[currentArg],
                                                             "Invalid boundary condition for boundingbox command!");
                    }
                    rightOk = true;
                }
                else
                {
                    throw new SwitchParserException(args[currentArg],
                                                         "Invalid boundary condition for boundingbox command!");
                }
            }

            if (!(bottomOk && topOk && leftOk && rightOk))
            {
                throw new SwitchParserException("None",
                                                     "Invalid bounding-box command, at least one of the boundaries is missing!");
            }

            // everything ok, take the next argument as the filename.
            switchOut = new SwitchFilterBoundingBox()
            {
                Top = top,
                Bottom = bottom,
                Left = left,
                Right = right
            };
            return 4;
        }

        /// <summary>
        /// Creates the processor that belongs to this data.
        /// </summary>
        /// <returns></returns>
        public override ProcessorBase CreateProcessor()
        {
            return new ProcessorFeedFilter()
            { // create a bounding box filter.
                Filter = new GTFS.Filters.GTFSFeedStopsFilter((s) => {
                    return (s.Latitude < this.Right && s.Latitude <= this.Left) &&
                        (s.Longitude < this.Top && s.Longitude <= this.Bottom);
                })
            };
        }

        /// <summary>
        /// Returns a description of this command.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("--bounding-box top={0} left={1} right={2} bottom={3}",
                                 this.Top.ToString(System.Globalization.CultureInfo.InvariantCulture),
                                 this.Left.ToString(System.Globalization.CultureInfo.InvariantCulture),
                                 this.Right.ToString(System.Globalization.CultureInfo.InvariantCulture),
                                 this.Bottom.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
    }
}
