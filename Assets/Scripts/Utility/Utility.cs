using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour
{
    public static int WrapAround (int max, int current, int increment)
    {
        int temp = current + increment;

        if (temp >= max)
        {
            temp = 0;
        }
        else if (temp < 0)
        {
            temp = max - 1;
        }

            return temp;
    }

}
