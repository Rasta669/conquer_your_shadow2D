//using System.Collections;
//using UnityEngine;

//public class ShadowManager : MonoBehaviour
//{
//    public GameObject shadow; // Assign your shadow GameObject here
//    public float delayTime = 1f; // Delay time before enabling the shadow

//    void Start()
//    {
//        StartCoroutine(ActivateShadowAfterDelay());
//    }

//    private IEnumerator ActivateShadowAfterDelay()
//    {
//        yield return new WaitForSeconds(delayTime); // Wait for the delay time
//        shadow.SetActive(true); // Enable the shadow
//        ShadowFollower shadowFollower = shadow.GetComponent<ShadowFollower>();
//        shadowFollower?.EnableShadow(); // Start the shadow movement replay
//        Debug.Log("Shadow activated after delay.");
//    }
//}
