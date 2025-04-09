using System.Collections;
using UnityEngine;

public class BombLifeTime : MonoBehaviour
{
    public float explodeRadius;
    float remainingTime;
    public float lifetime;
    public int damage;
    Material material;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        remainingTime = lifetime;
        material = GetComponent<MeshRenderer>().material;
        StartTimer();
    }

    private void StartTimer()
    {
        // «апускаем корутину, котора€ управл€ет таймером
        StartCoroutine(TimerRoutine());
    }

    private void OnExpired()
    {
        Explode();
    }

    private IEnumerator TimerRoutine()
    {
        while (remainingTime > 0f)
        {
            yield return new WaitForSeconds(0.01f);

            // ”меньшаем оставшеес€ врем€
            remainingTime -= 0.01f;
        }

        OnExpired();
    }

    // Update is called once per frame
    void Update()
    {
        float ratio = remainingTime / lifetime;
        Vector3 rgb;
        if (damage > 0)
        {
            rgb = new Vector3(1.0f, ratio, ratio); //подсветка красным (бомба опасна)
        }
        else
        {
            rgb = new Vector3(ratio, 1.0f, ratio); //подсветка зеленым (бомба неопасна)
        }
        material.color = new Color(rgb.x, rgb.y, rgb.z);
    }

    void Explode()
    {
        //проверить на наличие игрока р€дом
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance < explodeRadius)
        {
            PlayerHealth ph = player.GetComponent<PlayerHealth>();
            int realDamage = (int) Mathf.Floor(damage * (1 - Mathf.Pow(distance / explodeRadius, 2)));
            if (realDamage >= 0)
            {
                ph.TakeDamage(realDamage);

                Movement playerMovement = player.GetComponent<Movement>();

                Vector3 direction = player.transform.position - transform.position;
                direction.Normalize();
                playerMovement.AddForce(5 * direction * realDamage / damage);
            }
            else
            {
                ph.Heal(-realDamage);
            }
        }

        Transform audioSourceObject = transform.Find("ExplodeSoundSource");
        AudioSource audioSource = audioSourceObject.GetComponent<AudioSource>();

        GetComponent<Renderer>().enabled = false; // суперкласс дл€ всех видов рендера
        GetComponent<Collider>().enabled = false; // суперкласс дл€ всех коллайдеров

        // «апускаем звук
        audioSource.Play();

        Destroy(gameObject, audioSource.clip.length);
    }
}
