
using UnityEngine;

public class PlayerSaveData : MonoBehaviour
{
    [Header("References")]
    public PlayerColor playerColor;

    [Header("Player Runtime Data")]
    public PlayerHealth playerHealth;

    [Header("All Possible Colors")]
    public ColorIdentity[] allColors;

    private void Awake()
    {
        if (playerColor == null)
        {
            playerColor = GetComponent<PlayerColor>();
        }
        if(playerHealth == null)
        {
            playerHealth = GetComponent<PlayerHealth>();
        }
    }

    public void ApplyLoadedData
    (
        Vector3 loadedPosition,
        string loadedColorName,
        float loadedHealth
    )
    {
        CharacterController controller =
            GetComponent<CharacterController>();

        if (controller != null)
        {
            controller.enabled = false;
        }

        transform.position = loadedPosition;

        ColorIdentity loadedColor =
            FindColorByName(loadedColorName);

        if (playerColor != null)
        {
            playerColor.SetColorOnLoad(loadedColor);
        }

        playerHealth.SetHealth ( loadedHealth);

        if (controller != null)
        {
            controller.enabled = true;
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

        Debug.LogWarning
        (
            "Cannot find ColorIdentity: " + colorName
        );

        return null;
    }
}
