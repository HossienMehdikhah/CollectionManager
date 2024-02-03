﻿using System.Collections.Frozen;
namespace CollectionManager.Core;

public static class ConstContainer
{
    public static FrozenDictionary<string, int> PersianMonths = new Dictionary<string, int>
    {
        {"فروردین", 1},
        {"اردیبهشت", 2 },
        { "خرداد",3 },
        {"تیر",4 },
        {"مرداد",5 },
        {"شهریور",6},
        {"مهر",7 },
        {"آبان",8 },
        {"آذر",9 },
        {"دی",10 },
        {"بهمن",11 },
        {"اسنفند",12 },
    }.ToFrozenDictionary();
}
