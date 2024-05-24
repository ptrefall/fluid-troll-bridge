using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    public float LastTimeVisited { get; set; } = -1.0f; // Setting this to -1 so that on first tick it wins over context time potentially being 0.
}
