using System;
using System.Collections.Generic;

namespace Common
{
    public class Helpers
    {
        public static float FromPixels(float pixels)
        {
            return pixels * 0.103f;
        }

        public static string DateName(DateTime date)
        {
            return $"{date.Day} {MonthesR[date.Month]} {date.Year}г.";
        }

        public static string DatePoints(DateTime date)
        {
            return $"{date.Day}.{date.Month}.{date.Year}";
        }

        public static List<string> MonthesR = new List<string>()
        {
            "января",
            "февраля",
            "марта",
            "апреля",
            "майя",
            "июня",
            "июля",
            "августа",
            "сентября",
            "октября",
            "ноября",
            "декабря ",
            
        };
        
        public static List<string> Monthes = new List<string>()
        {
            "Январь",
            "Февраль",
            "Март",
            "Апрель",
            "Май",
            "Июнь",
            "Июль",
            "Август",
            "Сентябрь",
            "Октябрь",
            "Ноябрь",
            "Декабрь ",
            
        };
    }
}