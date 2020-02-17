using System.Collections;
using System;
using System.Linq;
using System.Xml.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using UnityEngine;

public static class RandomFunctions
{
    public static int Random(this double[] a, double r)
    {
    double sum = a.Sum();
    for (int j = 0; j < a.Length; j++) a[j] /= sum;

    int i = 0;
    double x = 0;

    while (i < a.Length)
    {
        x += a[i];
        if (r <= x) return i;
        i++;
    }

    return 0;
    }
}
