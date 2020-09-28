using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineObjectPool : MonoBehaviour
{
    public void FreeAllLines()
    {
        foreach (var line in ActiveLineRenderObjects)
        {
            line.SetActive(false);
            FreeLineRenderObjects.Add(line);
        }
        ActiveLineRenderObjects.Clear();
    }

    public LineRenderer NextFreeLineRenderer()
    {

        GameObject lineRendererObject = null;
        LineRenderer lineRenderer = null;

        if (FreeLineRenderObjects.Count > 0)
        {
            lineRendererObject = FreeLineRenderObjects[0];
            FreeLineRenderObjects.RemoveAt(0);
            lineRendererObject.SetActive(true);
            lineRenderer = lineRendererObject.GetComponent<LineRenderer>();
        }
        else
        {
            lineRendererObject = Instantiate(LineRendererPrefab);
            lineRenderer = lineRendererObject.GetComponent<LineRenderer>();
        }

        if (lineRendererObject)
        {
            ActiveLineRenderObjects.Add(lineRendererObject);
        }

        return lineRenderer;
    }

    public float OutlineOffset = 0.06f;
    public float OutlineExtent = 0.08f;

    public GameObject LineRendererPrefab;

    private List<GameObject> ActiveLineRenderObjects = new List<GameObject>();
    private List<GameObject> FreeLineRenderObjects = new List<GameObject>();
}