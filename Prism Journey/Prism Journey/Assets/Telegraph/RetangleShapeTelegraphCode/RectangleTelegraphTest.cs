using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class RectangTelegraphTester : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public GameObject telegraphPrefab;

    [Header("Shape")]
    public float telegraphWidth = 6f;
    public float telegraphLength = 10f;
    public float yOffset = 0.05f;

    [Header("Tracking")]
    public float trackingDuration = 1.2f;
    public float trackingSpeed = 4f;

    [Header("Fill")]
    public float fillDuration = 1f;

    private bool isPlaying;

    [Header("tesingUse")]
    public Transform spawnPosition;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V) && !isPlaying)
        {
            StartCoroutine(PlayTelegraph());
        }

    }



    //IEnumerator = a function that can pause and continue later
/* Ex:
  Debug.Log("Start");
  yield return new WaitForSeconds(2f);
  Debug.Log("End");

//it pauses for 2 seconds, then continues

yield return null = wait 1 frame  
yield return new WaitForSeconds(1f)= wait 1 second
yield return StartCoroutine(...)=wait until that coroutine finishes
*/
private IEnumerator PlayTelegraph()
{
isPlaying = true;

// 1. Initial forward direction from boss
// Value keep the object on ground
GameObject telegraph = Instantiate(telegraphPrefab, spawnPosition.position, Quaternion.identity);// spwan telegrah
telegraph.transform.rotation = Quaternion.Euler(90, 180, 0);// set the rotation in to x 90 degree , and the y control the edge start to fill
RectangleTelegrahVisual visual = telegraph.GetComponent<RectangleTelegrahVisual>();//grap the refference for the fill telegraph
if (visual != null)
{
    visual.Setup(telegraphWidth, telegraphLength);
    visual.SetFillPercent(0f);
}

// 2. Track player for a short time
float trackingTimer = 0f;

while (trackingTimer < trackingDuration)
{
    trackingTimer += Time.deltaTime;


    if (player != null)
    {
        Vector3 toPlayerZOnly = new Vector3(player.transform.position.x, 0.05f, 0); //use only to track player x axis position  and lift on y axis

        // Fucntion that tracking two point and move with liner speed
            telegraph.transform.position = Vector3.Lerp(
                telegraph.transform.position,
                 toPlayerZOnly,
                Time.deltaTime * trackingSpeed
            );           
    }
    yield return null;
}


// 4. Lock and fill from left to right
float fillTimer = 0f;

while (fillTimer < fillDuration)
{
    fillTimer += Time.deltaTime;

    float percent = Mathf.Clamp01(fillTimer / fillDuration); // calculate the current percent the fill reach  and set to 1 if it over max value

    if (visual != null)
    {
        visual.SetFillPercent(percent);
    }

    yield return null;
}

Debug.Log("Telegraph complete and locked.");

Destroy(telegraph, 1f);

isPlaying = false;
}



}





