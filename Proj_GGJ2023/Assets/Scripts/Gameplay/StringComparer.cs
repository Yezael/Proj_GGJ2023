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

    public static bool CompareParcialText(string parcialAnswer, string neededAnswer)
    {
        var normalizedParcialA = RemoveDiacritics(parcialAnswer).ToLower();
        var normalizedNeddedA = RemoveDiacritics(neededAnswer).ToLower();
        var amount = normalizedParcialA.Length;
        if (amount > normalizedNeddedA.Length) return false;
        for (int i = 0; i < amount; i++)
        {
            if(!normalizedParcialA[i].Equals(normalizedNeddedA[i]))
            {
                return false;
            }
        }
        return true;
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
