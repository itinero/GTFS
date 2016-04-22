// The MIT License (MIT)

// Copyright (c) 2014 Ben Abelshausen

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using GTFS.Attributes;
using GTFS.Entities.Enumerations;

namespace GTFS.Entities
{
    /// <summary>
    /// Represents fare information for a transit organization's routes.
    /// </summary>
    [FileName("fare_attribute")]
    public class FareAttribute : GTFSEntity
    {
        /// <summary>
        /// Gets or sets an ID that uniquely identifies a fare class. The fareid is dataset unique.
        /// </summary>
        [Required]
        [FieldName("fare_id")]
        public string FareId { get; set; }

        /// <summary>
        /// Gets or sets the fare price, in the unit specified by CurrencyType.
        /// </summary>
        [Required]
        [FieldName("price")]
        public string Price { get; set; }

        /// <summary>
        /// Gets or sets the currency used to pay the fare. Uses the ISO 4217 alphabetical currency codes which can be found at the following URL: http://www.iso.org/iso/home/standards/iso4217.htm. 
        /// </summary>
        [Required]
        [FieldName("currency_type")]
        public string CurrencyType { get; set; }

        /// <summary>
        /// Gets or sets when the fare must be paid.
        /// </summary>
        [Required]
        [FieldName("payment_method")]
        public PaymentMethodType PaymentMethod { get; set; }

        /// <summary>
        /// Gets or sets the number of transfers permitted on this fare.
        /// </summary>
        [Required]
        [FieldName("transfers")]
        public uint? Transfers { get; set; }

        /// <summary>
        /// Gets or sets the length of time in seconds before a transfer expires.
        /// </summary>
        [FieldName("transfer_duration")]
        public string TransferDuration { get; set; }


        /// <summary>
        /// Serves as a hash function.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 23;
                hash = hash * 29 + (this.CurrencyType ?? string.Empty).GetHashCode();
                hash = hash * 29 + (this.FareId ?? string.Empty).GetHashCode();
                hash = hash * 29 + this.PaymentMethod.GetHashCode();
                hash = hash * 29 + (this.Price ?? string.Empty).GetHashCode();
                hash = hash * 29 + (this.TransferDuration ?? string.Empty).GetHashCode();
                hash = hash * 29 + this.Transfers.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Returns true if the given object contains the same data.
        /// </summary>
        public override bool Equals(object obj)
        {
            var other = (obj as FareAttribute);
            if (other != null)
            {
                return (this.CurrencyType ?? string.Empty) == (other.CurrencyType ?? string.Empty) &&
                    (this.FareId ?? string.Empty) == (other.CurrencyType ?? string.Empty) &&
                    this.PaymentMethod == other.PaymentMethod &&
                    (this.Price ?? string.Empty) == (other.Price ?? string.Empty) &&
                    (this.TransferDuration ?? string.Empty) == (other.TransferDuration ?? string.Empty) &&
                    this.Transfers == other.Transfers;
            }
            return false;
        }
    }
}