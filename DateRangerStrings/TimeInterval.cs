using System;
using System.Collections.Generic;

namespace DateRanger
{
    /// <summary>
    /// Typesafe Enum 
    /// 
    /// http://www.javacamp.org/designPattern/enum.html
    /// </summary>
    public sealed class TimeInterval
    {
        private readonly string name;

        private static readonly Dictionary<string, TimeInterval> instance =
            new Dictionary<string,TimeInterval>();

        public static readonly TimeInterval DAYS = new TimeInterval("Day");
        public static readonly TimeInterval WEEKS = new TimeInterval("Week");
        public static readonly TimeInterval MONTHS = new TimeInterval("Month");
        public static readonly TimeInterval YEARS = new TimeInterval("Year");
        public static readonly TimeInterval MINUTES = new TimeInterval("Minute");
        public static readonly TimeInterval HOURS = new TimeInterval("Hour");
        
        private TimeInterval(string name)
        {
            this.name = name;
            instance[name.ToLower()] = this;
        }

        public static IEnumerable<TimeInterval> Items
        {
            get {
                return instance.Values;
            }
        }

        public override string ToString()
        {
            return name;
        }

        public string ToStringOptionalPlural()
        {
            return name + "(s)";
        }

        public string ToStringPlural()
        {
            return name + "s";
        }


        public static explicit operator TimeInterval(string str)
        {
            TimeInterval result;
            if (instance.TryGetValue(str.ToLower(), out result))
            {
                return result;
            }
            throw new InvalidCastException();
        }

        public static bool TryParse(string str, out TimeInterval ti)
        {
            if(str.EndsWith("s"))
                str = str.Substring(0, str.Length-1);
            else if (str.EndsWith("(s)"))
                str = str.Substring(0, str.Length - 3);

            TimeInterval result;
            if (instance.TryGetValue(str.ToLower(), out result))
            {
                ti = result;
                return true;
            }
            ti = null;
            return false;
        }
    }
}
