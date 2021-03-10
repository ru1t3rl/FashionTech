using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    /// <summary> Converts given bitmask to layer number </summary>
    /// <returns> layer number/index </returns>
    public static int ToInteger(this LayerMask bitmask)
    {
        bitmask = bitmask.value;
        int result = bitmask > 0 ? 0 : 31;
        while (bitmask > 1)
        {
            bitmask = bitmask >> 1;
            result++;
        }

        return result;
    }

    public static List<int> ToMultipleLayers(this LayerMask bitMask)
    {
        bool done = false;
        List<int> masks = new List<int>();

        bitMask = bitMask.value;

        again:
        int result = bitMask > 0 ? 0 : 31;
        while (bitMask > 1 || masks.Contains(result))
        {
            bitMask = bitMask >> 1;
            result++;

            if (result >= 31)
            {
                done = true;
                break;
            }
        }

        if (!done)
            masks.Add(result);

        if (!done)
            goto again;

        return masks;
    }
}