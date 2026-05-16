using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MoonLanding : MonoBehaviour
{
    public string sceneToLoad = "Moon_scene";
    public float landingDuration = 3f;

    private bool isLanding = false;

    void OnTriggerEnter(Collider other)
    {
        if (isLanding) return;

        if (other.transform.root.CompareTag("Player"))
        {
            Debug.Log("Landing sequence started...");
            StartCoroutine(LandSequence(other.transform.root));
        }
    }

    IEnumerator LandSequence(Transform player)
    {
        isLanding = true;

        // 🔧 Get ship correctly (from parent of XR Origin)
        ShipMovement ship = player.GetComponentInParent<ShipMovement>();

        if (ship != null)
        {
            ship.canFly = false;
        }

        float timer = 0f;

        Vector3 startPos = player.position;
        Vector3 targetPos = transform.position + Vector3.up * 2f;

        while (timer < landingDuration)
        {
            timer += Time.deltaTime;
            float t = timer / landingDuration;

            player.position = Vector3.Lerp(startPos, targetPos, t);

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        // 🚀 Load scene
        SceneManager.LoadScene(sceneToLoad);
    }
}