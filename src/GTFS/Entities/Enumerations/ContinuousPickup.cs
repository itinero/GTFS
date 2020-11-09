namespace GTFS.Entities.Enumerations
{
    /// <summary>
    /// Indicates whether a rider can board the transit vehicle anywhere along the vehicle’s travel path. The path is described by shapes.txt on every trip of the route.
    /// </summary>
    public enum ContinuousPickup
    {
        /// <summary>
        /// Continuous stopping pickup.
        /// </summary>
        ContinuousStoppingPickup = 0,

        /// <summary>
        /// No continuous stopping pickup. Default option if field is empty.
        /// </summary>
        None = 1, // default

        /// <summary>
        /// Must phone an agency to arrange continuous stopping pickup.
        /// </summary>
        PhoneForPickup = 2,

        /// <summary>
        /// Must coordinate with a driver to arrange continuous stopping pickup.
        /// </summary>
        DriverForPickup = 3
    }
}
