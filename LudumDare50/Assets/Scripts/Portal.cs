using UnityEngine;

public class Portal : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject Door;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerStats>() != null)
        {
            if (CanEnterPortal())
            {
                Debug.Log("Player In Portal");
                GameController.Instance.StartDay();
            }
            else
            {
                Debug.Log("You can't enter yet, kill some humans first");
            }
        }
    }


    private void Update()
    {
        if(Vector3.Distance(transform.position, PlayerStats.Instance.transform.position) < 12 && CanEnterPortal())
        {
            if (!Door.activeSelf)
            {
                Debug.Log("Enabling Portal");
                Door.SetActive(true);
            }
        }
        else
        {
            if (Door.activeSelf)
            {
                Debug.Log("Disabling Portal");
                Door.SetActive(false);
            }
        }
    }

    private bool CanEnterPortal()
    {
        return GameController.Instance.MissingHumansToKill == 0;
    }

}
