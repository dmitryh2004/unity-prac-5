using UnityEngine;

public class BombSpawn : MonoBehaviour
{
    [SerializeField] private GameObject _BombBodyPrefab;
    Transform player;
    PlayerScore playerScore;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerScore = player.GetComponent<PlayerScore>();
    }

    public void SpawnBomb(Transform roofElement, float lifetime, int damage, float explodeRadius)
    {
        Vector3 spawnPosition = roofElement.position;
        spawnPosition.y -= 1;
        GameObject bomb = Instantiate(_BombBodyPrefab, spawnPosition, Quaternion.Euler(0, 0, 0));
        //добавить скрипт жизни бомбы
        BombLifeTime bombLifeTime = bomb.AddComponent<BombLifeTime>();
        bombLifeTime.explodeRadius = explodeRadius;
        bombLifeTime.damage = damage;
        bombLifeTime.lifetime = lifetime;
        playerScore.ModifyScore(1);

        //пнуть бомбу в случайную сторону со случайной силой
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        randomDirection.Normalize();

        float force = Random.Range(0f, 1f);

        bomb.GetComponent<Rigidbody>().AddForce(randomDirection * force, ForceMode.Impulse);
    }
}
