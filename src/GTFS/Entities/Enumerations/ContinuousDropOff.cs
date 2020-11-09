namespace GTFS.Entities.Enumerations
{
    /// <summary>
    /// Indicates whether a rider can alight from the transit vehicle at any point along the vehicle’s travel path. The path is described by shapes.txt on every trip of the route. 
    /// </summary>
    public enum ContinuousDropOff
    {
        /// <summary>
        /// Continuous stopping drop-off.
        /// </summary>
        ContinuousStoppingDropOff = 0,

        /// <summary>
        /// No continuous stopping drop-off. Default option if field is empty.
        /// </summary>
        None = 1, // default

        /// <summary>
        /// Must phone an agency to arrange continuous stopping drop-off.
        /// </summary>
        PhoneForDropOff = 2,

        /// <summary>
        /// Must coordinate with a driver to arrange continuous stopping drop-off.
        /// </summary>
        DriverForDropOff = 3
    }
}
