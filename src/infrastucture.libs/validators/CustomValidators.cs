using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace infrastructure.libs.validators
{
    // TODO: make it singleton and measure :-)
    public static class CustomValidators
    {
        public static void NotNull(object theObj, string msg)
        {
            if (theObj == null)
            {
                if (string.IsNullOrEmpty(msg))
                {
                    msg = "A object instance can't be null.";
                }

                throw new ArgumentNullException(msg);
            }
        }

        public static void StringNotNullorEmpty(string str, string msg)
        {
            if (string.IsNullOrEmpty(str))
            {
                if (string.IsNullOrEmpty(msg))
                {
                    msg = "A object instance can't be null.";
                }

                throw new ArgumentNullException(msg);
            }
        }

        public static void NotNull(object theObj)
        {
            NotNull(theObj, null);
        }

        public static void IsTrue(bool isTrue, string msg)
        {
            if (!isTrue)
            {
                throw new ArgumentException(msg);
            }
        }

        public static void IntNotNegative(int value, string msg)
        {
            if (value < 0)
            {
                throw new ArgumentException(msg);
            }
        }

        public static void LongIsZero(long value, string msg)
        {
            if (value < 1)
            {
                throw new ArgumentException(msg);
            }
        }

        public static bool AreEqual(object a, object b)
        {
            if (a == null || b == null)
                return false;
            return a.Equals(b);
        }

        public static void DateTimeIsInFuture(object theObj, string msg)
        {
            if (theObj == null)
            {
                msg = "A object instance can't be null.";
                throw new ArgumentNullException(msg);
            }

            if (theObj.GetType() != typeof(DateTime))
            {
                msg = "A object instance is not of DateTime type.";
                throw new ArgumentNullException(msg);
            }

            if (((DateTime)theObj) <= DateTime.Now)
            {
                throw new ArgumentNullException(msg);
            }
        }
    }
}
