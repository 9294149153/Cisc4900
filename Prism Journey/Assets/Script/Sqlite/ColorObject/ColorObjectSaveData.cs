
using UnityEngine;

public class ColorObjectSaveData : MonoBehaviour
{
    [Header("Unique Save ID")]
    public string objectId;

    [Header("Color State")]
    public ColorObject currentColor;

    [Header("All Possible Colors")]
    public ColorIdentity[] allColors;

    [Header("Collider To Save")]
    public Collider targetCollider;

    private void Awake()
    {
        if (targetCollider == null)
        {
            targetCollider = GetComponent<Collider>();
        }

        if (currentColor == null)
        {
            currentColor=GetComponentInParent<ColorObject>();
        }
    }

    private void Update()
    {
        
    }
    public string GetColorName()
    {
        if (currentColor == null)
        {
            return "";
        }

        return currentColor.GetColorIdentity().currentColorName;
    }

    public bool GetTriggerState()
    {
        if (targetCollider == null)
        {
            return false;
        }

        return targetCollider.isTrigger;
    }

    public void ApplyLoadedData
    (
        Vector3 loadedPosition,
        string loadedColorName,
        bool loadedIsTrigger
    )
    {
        transform.position = loadedPosition;

        ColorIdentity foundColor = FindColorByName(loadedColorName);

        Debug.Log("Loaded color name from DB = [" + loadedColorName + "]");
        Debug.Log("Found color identity = " + foundColor);

        currentColor.SetColor(foundColor);

        if (targetCollider != null)
        {
            targetCollider.isTrigger = loadedIsTrigger;
        }
    }

    private ColorIdentity FindColorByName(string colorName)
    {
        foreach (ColorIdentity color in allColors)
        {
            if (color != null &&
                color.currentColorName == colorName)
            {
                return color;
            }
        }

        Debug.LogWarning("Cannot find ColorIdentity: " + colorName);

        return null;
    }
}
