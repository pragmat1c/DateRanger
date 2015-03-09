using System;
using System.Collections.Generic;

namespace DateRanger
{
    /// <summary>
    /// Class RelativeDateTime provides a DateTime relative to now along with a 
    /// friendly name.
    /// Translates common plain English dates and times (ex: "beginning of this week")
    /// into an exact DateTime.
    /// 
    /// </summary>
    /// <remarks>
    /// Typesafe Enum Pattern:  http://www.javacamp.org/designPattern/enum.html
    /// </remarks>
    [Serializable]
    public sealed class RelativeDateTime : IEquatable<RelativeDateTime>
    {
        /// <summary>
        /// The human friendly name. Examples: Now, 7 Days Ago, etc.
        /// </summary>
        public string Name { get; private set; }
        public Func<DateTime> GetDateTime { get; private set; }

        private static readonly SortedList<string, RelativeDateTime> Instance =
            new SortedList<string, RelativeDateTime>();

        public static readonly RelativeDateTime Now =
            new RelativeDateTime("Now", () => DateTime.Now);

        public static readonly RelativeDateTime StartOfYesterday =
            new RelativeDateTime("Start of Yesterday", Day.StartOfYesterday);

        public static readonly RelativeDateTime EndOfYesterday =
            new RelativeDateTime("End of Yesterday", Day.EndOfYesterday);

        public static readonly RelativeDateTime StartOfTomorrow =
            new RelativeDateTime("Start of Tomorrow", Day.StartOfTomorrow);

        public static readonly RelativeDateTime EndOfTomorrow =
            new RelativeDateTime("End of Tomorrow", Day.EndOfTomorrow);

        public static readonly RelativeDateTime AnyTimeInPast =
            new RelativeDateTime("Any Time in Past", () => DateTime.MinValue);
        public static readonly RelativeDateTime AnyTimeInFuture =
            new RelativeDateTime("Any Time in Future", () => DateTime.MaxValue);

        public static readonly RelativeDateTime StartOfLastWeek =
            new RelativeDateTime("Start of Last Week", Week.StartOfLast);
        public static readonly RelativeDateTime StartOfThisWeek =
            new RelativeDateTime("Start of This Week", Week.StartOfCurrent);
        public static readonly RelativeDateTime StartOfNextWeek =
            new RelativeDateTime("Start of Next Week", Week.StartOfNext);

        public static readonly RelativeDateTime SevenDaysAgo =
            new RelativeDateTime("7 Days Ago", () => DateTime.Now.AddDays(-7));
        public static readonly RelativeDateTime FourteenDaysAgo =
            new RelativeDateTime("14 Days Ago", () => DateTime.Now.AddDays(-14));
        public static readonly RelativeDateTime TwentyeightDaysAgo =
            new RelativeDateTime("28 Days Ago", () => DateTime.Now.AddDays(-28));

        public static readonly RelativeDateTime SevenDaysFromNow =
            new RelativeDateTime("7 Days From Now", () => DateTime.Now.AddDays(7));
        public static readonly RelativeDateTime FourteenDaysFromNow =
            new RelativeDateTime("14 Days From Now", () => DateTime.Now.AddDays(14));
        public static readonly RelativeDateTime TwentyeightDaysFromNow =
            new RelativeDateTime("28 Days From Now", () => DateTime.Now.AddDays(28));

        public static readonly RelativeDateTime EndOfLastWeek =
            new RelativeDateTime("End of Last Week", Week.EndOfLast);
        public static readonly RelativeDateTime EndOfThisWeek =
            new RelativeDateTime("End of This Week", Week.EndOfCurrent);
        public static readonly RelativeDateTime EndOfNextWeek =
            new RelativeDateTime("End of Next Week", Week.EndOfNext);

        public static readonly RelativeDateTime StartOfLastMonth =
            new RelativeDateTime("Start of Last Month", Month.StartOfLast);
        public static readonly RelativeDateTime StartOfThisMonth =
            new RelativeDateTime("Start of This Month", Month.StartOfCurrent);
        public static readonly RelativeDateTime StartOfNextMonth =
            new RelativeDateTime("Start of Next Month", Month.StartOfNext);

        public static readonly RelativeDateTime EndOfLastMonth =
            new RelativeDateTime("End of Last Month", Month.EndOfLast);
        public static readonly RelativeDateTime EndOfThisMonth =
            new RelativeDateTime("End of This Month", Month.EndOfCurrent);
        public static readonly RelativeDateTime EndOfNextMonth =
            new RelativeDateTime("End of Next Month", Month.EndOfNext);

        public static readonly RelativeDateTime StartOfLastYear =
            new RelativeDateTime("Start of Last Year", Year.StartOfLast);
        public static readonly RelativeDateTime StartOfThisYear =
            new RelativeDateTime("Start of This Year", Year.StartOfCurrent);
        public static readonly RelativeDateTime StartOfNextYear =
            new RelativeDateTime("Start of Next Year", Year.StartOfNext);

        public static readonly RelativeDateTime EndOfLastYear =
            new RelativeDateTime("End of Last Year", Year.EndOfLast);
        public static readonly RelativeDateTime EndOfThisYear =
            new RelativeDateTime("End of This Year", Year.EndOfCurrent);
        public static readonly RelativeDateTime EndOfNextYear =
            new RelativeDateTime("End of Next Year", Year.EndOfNext);
        

        private RelativeDateTime(string name, Func<DateTime> dateFunc)
        {
            Name = name;
            GetDateTime = dateFunc;
            Instance[name.ToLower()] = this;
        }

        /// <summary>
        /// Sequence of <see cref="RelativeDateTime"/> items. Guaranteed to be in
        /// sorted order by <see cref="Name"/>.
        /// </summary>
        public static IEnumerable<RelativeDateTime> Items
        {
            get {
                return Instance.Values;
            }
        }

        public static bool TryParse(string str, out RelativeDateTime relativeDateTime)
        {
            if (Instance.TryGetValue(str.ToLower(), out relativeDateTime))
            {
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return Name;
        }

        public static explicit operator RelativeDateTime(string str)
        {
            RelativeDateTime result;
            if (Instance.TryGetValue(str.ToLower(), out result))
            {
                return result;
            }
            throw new InvalidCastException();
        }


        #region Equality
        
        public override bool Equals(Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            var ot = obj as RelativeDateTime;
            if ((Object)ot == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (Name == ot.Name);
        }

        public bool Equals(RelativeDateTime other)
        {
            //Check whether the compared object is null.
            if (ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            return Name.Equals(other.Name);
        }

        public override int GetHashCode()
        {
            //Get hash code for the Name field if it is not null.
            return (Name == null ? 0 : Name.GetHashCode());
        }

        public static bool operator ==(RelativeDateTime a, RelativeDateTime b)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.Name == b.Name;
        }

        public static bool operator !=(RelativeDateTime a, RelativeDateTime b)
        {
            return !(a == b);
        }

        #endregion

    }
}
