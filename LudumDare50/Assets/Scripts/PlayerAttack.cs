using DG.Tweening;
using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerStats m_Stats;

    private float m_AttackRate = 1f;
    private float m_TimeToNextAttack = 0;
    public GameObject AttackOrigin;

    public LayerMask ToAttack;


    private void Awake()
    {
        m_Stats = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        // Only allow the player to attack during night
        if(GameController.Instance.GameState != GameState.NIGHT) 
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1"))
        {
            if(CanAttack())
            {
                Attack();
            }
        }
    }

    private bool CanAttack()
    {
        return Time.time >= m_TimeToNextAttack;
    }


    private void Attack()
    {
        transform.DOShakeRotation(0.4f).OnComplete(() =>
        {
            Vector3 or = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(0, or.y, 0);
        });

        m_TimeToNextAttack = Time.time + m_AttackRate;

        Ray r = new Ray(AttackOrigin.transform.position, AttackOrigin.transform.right);

        RaycastHit[] hits = Physics.SphereCastAll(r, 0.5f, 0.5f);

        foreach(RaycastHit h in hits)
        {
            Guard g = h.collider.GetComponent<Guard>();
            if(g != null)
            {
                Debug.Log("Fighting Against Guard!");
            }

            BuildingHitPoint b = h.collider.GetComponent<BuildingHitPoint>();
            if(b != null)
            {
                Debug.Log("Attacking Building");
                b.Owner.GetAttacked(m_Stats.Strength);

                if(b.Owner.DoorDestroyed())
                {
                    StartCoroutine(DoHumanAssasinationAnimation(b.Owner));
                }
            }
        }
    }

    IEnumerator DoHumanAssasinationAnimation(Bulding _toAttack)
    {
        GameController.Instance.SetKillingState();

        Vector3 target = new Vector3(_toAttack.transform.position.x, transform.position.y, _toAttack.transform.position.z);
        Vector3 originPos = transform.position;
    
        transform.DOJump(_toAttack.transform.position, 0.5f, 1, 2);
        yield return new WaitForSeconds(2f);
        Debug.Log("Play some sound here!");
        transform.forward = -transform.forward;
        transform.DOMove(originPos, 1).OnComplete(() => {
            GameController.Instance.ClearKillingState();
        });
    
 
    }
}
