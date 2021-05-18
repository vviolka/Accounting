using System;

namespace Common
{
    public static class SetProperties
    {
        public static void SetProperty(ref string val, string value)
        {
            if (val == value)
                return;
            val = value;
        }
        
        public static void SetProperty(ref bool val, bool value)
        {
            if (val == value)
                return;
            val = value;
        }
        
        public static void SetProperty(ref int val, int value)
                {
                    if (val == value)
                        return;
                    val = value;
                }
                
                
                public static void SetProperty(ref float val, float value)
                        {
                            if (val == value)
                                return;
                            val = value;
                        }
                        
                        
                public static void SetProperty(ref DateTime val, DateTime value)
                {
                    if (val == value)
                        return;
                    val = value;
                }
    }
}   