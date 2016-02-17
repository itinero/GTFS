using System.Collections.Generic;
using GTFS.Entities;

namespace GTFS.Test
{
    /// <summary>
    /// Compares instances of type <see cref="FeedInfo"/> for equality.
    /// </summary>
    public sealed class FeedInfoEqualityComparer : IEqualityComparer<FeedInfo>
    {
        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        /// <param name="x">The first object of type <see cref="FeedInfo"/> to compare.</param>
        /// <param name="y">The second object of type <see cref="FeedInfo"/> to compare.</param>
        public bool Equals(FeedInfo x, FeedInfo y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;

            return string.Equals(x.PublisherName, y.PublisherName) &&
                   string.Equals(x.PublisherUrl, y.PublisherUrl) &&
                   string.Equals(x.Lang, y.Lang) &&
                   string.Equals(x.StartDate, y.StartDate) &&
                   string.Equals(x.EndDate, y.EndDate) &&
                   Equals(x.Tag, y.Tag) &&
                   string.Equals(x.Version, y.Version);
        }

        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        /// <returns>
        /// A hash code for the specified object.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object"/> for which a hash code is to be returned.</param>
        /// <exception cref="T:System.ArgumentNullException">The type of <paramref name="obj"/> is a reference type and <paramref name="obj"/> is null.</exception>
        public int GetHashCode(FeedInfo obj)
        {
            unchecked
            {
                var hashCode = (obj.PublisherName != null ? obj.PublisherName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.PublisherUrl != null ? obj.PublisherUrl.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.Lang != null ? obj.Lang.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.StartDate != null ? obj.StartDate.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.EndDate != null ? obj.EndDate.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.Tag != null ? obj.Tag.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.Version != null ? obj.Version.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}