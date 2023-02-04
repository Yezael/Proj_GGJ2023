using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using UnityEngine;
using System;

public static class StringComparer 
{
    
    public static bool CompareTexts(string txt1, string txt2)
    {
        var normalized1 = RemoveDiacritics(txt1);
        var normalized2 = RemoveDiacritics(txt2);
        var areEqual = normalized1.Equals(normalized2, StringComparison.InvariantCultureIgnoreCase);
        return areEqual;
    }

    static string RemoveDiacritics(string text)
    {
        return string.Concat(
            text.Normalize(NormalizationForm.FormD)
            .Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) !=
                                          UnicodeCategory.NonSpacingMark)
          ).Normalize(NormalizationForm.FormC);
    }

}
