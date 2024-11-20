using System.Collections;
using UnityEngine;

public class EnemyRangeAttack : MonoBehaviour, IAttack
{
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform fireballPoint;
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private float fireballSpeed;

    private Enemy enemy;
    private AudioSource audioSource;
    private GameObject playerRef;
    private Character playerCharacter;
    private bool isAttacking;
    private float lastAttackTime;

    public void Initialize(GameObject player)
    {
        playerRef = player;
        playerCharacter = player.GetComponent<Character>();
        audioSource = GetComponent<AudioSource>();
        enemy = GetComponent<Enemy>(); // Obtener Enemy
    }

    public void AttemptAttack()
    {
        if (!isAttacking && Time.time >= lastAttackTime + enemy.GetAttackCooldown() / enemy.GetAttackSpeed())
        {
            isAttacking = true;
            lastAttackTime = Time.time;
            animator.SetBool("IsAttacking", true);
            animator.speed = enemy.GetAttackSpeed();

            PlayAttackSound();
            ShootFireball();

            if (playerCharacter != null)
            {
                playerCharacter.TakeDamage((int)enemy.GetAttackPower());
            }

            StartCoroutine(ResetAttack());
        }
    }

    public void StopAttacking()
    {
        if (isAttacking)
        {
            isAttacking = false;
            animator.SetBool("IsAttacking", false);
            animator.speed = 1f;
        }
    }

    private void PlayAttackSound()
    {
        if (audioSource != null && attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(1f / enemy.GetAttackSpeed());
        isAttacking = false;
        animator.SetBool("IsAttacking", false);
        animator.speed = 1f;
    }

    private void ShootFireball()
    {
        var fireball = Instantiate(fireballPrefab, fireballPoint.position, fireballPoint.rotation);
        fireball.GetComponent<Rigidbody>().velocity = fireballPoint.forward * fireballSpeed;
        Debug.Log("BOLA CALIENTE");
    }
}
