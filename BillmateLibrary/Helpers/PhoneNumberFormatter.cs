﻿using System;

namespace BillMate.Helpers
{
    public class PhoneNumberFormatter
    {
        public static string FormatPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                return string.Empty;

            if (phoneNumber.Length > 20)
                return phoneNumber;
            phoneNumber = new System.Text.RegularExpressions.Regex(@"\D")
                .Replace(phoneNumber, string.Empty);
            phoneNumber = phoneNumber.TrimStart('1');
            if (phoneNumber.Length == 7)
                return Convert.ToInt64(phoneNumber).ToString("###-####");
            if (phoneNumber.Length == 10)
                return Convert.ToInt64(phoneNumber).ToString("(###) ###-####");
            if (phoneNumber.Length > 10)
                return String.Format("{0:(###) ###-####}", phoneNumber);

            return phoneNumber;
        }
    }
}
