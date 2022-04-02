using UnityEngine;

public class Portal : MonoBehaviour
{
    // Start is called before the first frame update

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

    private bool CanEnterPortal()
    {
        return GameController.Instance.MissingHumansToKill == 0;
    }
}
