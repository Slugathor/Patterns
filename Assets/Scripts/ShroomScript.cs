using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShroomScript : MonoBehaviour
{
    [SerializeField] float shroomDuration=7;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            StartCoroutine(Shroomify(other.gameObject));
        }
    }

    IEnumerator Shroomify(GameObject player)
    {
        GameManager.instance.shroomed = true;
        // "Disable" shroom gameobject by disabling its renderers and collider
        List<Renderer> renderers = this.gameObject.GetComponentsInChildren<Renderer>().ToList();
        renderers.ForEach(renderer => { renderer.enabled = false; });
        this.gameObject.GetComponent<Collider>().enabled = false;

        PlayerController playerController = player.GetComponentInParent<PlayerController>();
        // save the normal movement keys, so they can be returned when the effect ends
        Dictionary<KeyCode, PlayerController.MoveCommand> normalMoveKeys = playerController.keyToCommand;

        // swap W & S  and A & D  functionalities
        playerController.keyToCommand = new Dictionary<KeyCode, PlayerController.MoveCommand>
        {
            { KeyCode.W, playerController.SKey },
            { KeyCode.A, playerController.DKey },
            { KeyCode.S, playerController.WKey },
            { KeyCode.D, playerController.AKey }
        };

        // GameManager takes care of the UI element, so pass some info
        GameManager.instance.shroomedDuration = shroomDuration;
        GameManager.instance.shroomStart = Time.timeAsDouble;

        // return here after shroomDuration to return normal keys and destroy the shroom
        yield return new WaitForSeconds(shroomDuration);
        playerController.keyToCommand = normalMoveKeys;
        GameManager.instance.shroomed = false;
        Destroy(gameObject);
    }
}
