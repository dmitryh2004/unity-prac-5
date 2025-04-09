using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationController : MonoBehaviour
{
    public float timerDuration = 9f; //Время таймера в секундах
    public int startBombCount = 10; //Число бомб, спавнящихся в начале игры
    public float startBombExplodeRadius = 5f;
    public int bombDamage = 25;
    float bombExplodeRadius;
    int currentBombCount;
    int healBombCount = 0; //Число лечебных бомб
    public List<Transform> roofSegments;
    public Transform bombSpawnerObject;
    BombSpawn bombSpawner;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentBombCount = startBombCount;
        bombExplodeRadius = startBombExplodeRadius;
        bombSpawner = bombSpawnerObject.GetComponent<BombSpawn>();
        StartBombTimer();
    }

    private void StartBombTimer()
    {
        // Запускаем корутину, которая управляет таймером
        StartCoroutine(BombTimerRoutine());
    }

    private void BombTimerOnExpired()
    {
        SpawnBombs(currentBombCount);
    }

    private IEnumerator BombTimerRoutine()
    {
        // Ждем пока не истечет время таймера
        yield return new WaitForSeconds(timerDuration);

        // Выполняем действия после истечения времени
        BombTimerOnExpired();

        // Перезапускаем таймер, при этом уменьшая время на 0.1 секунды
        if (timerDuration > 1.0f)
        {
            timerDuration -= 0.1f;
            // Увеличение количества бомб по мере снижения времени
            if (timerDuration < 6.0f)
            {
                currentBombCount = startBombCount + 1;
            }
            if (timerDuration < 4.5f)
            {
                currentBombCount = startBombCount + 2;
                healBombCount = 1;
            }
            if (timerDuration < 3.5f)
            {
                currentBombCount = startBombCount + 3;
                healBombCount = 1;
                bombExplodeRadius = startBombExplodeRadius + 1f;
            }
            if (timerDuration < 2.0f)
            {
                currentBombCount = startBombCount + 5;
                healBombCount = 2;
                bombExplodeRadius = startBombExplodeRadius + 2f;
            }
        }
        StartBombTimer();
    }

    private void StartLampTimer()
    {
        // Запускаем корутину, которая управляет таймером
        StartCoroutine(LampTimerRoutine());
    }

    private void LampTimerOnExpired()
    {
        foreach (Transform element in roofSegments)
        {
            Transform lamp = element.GetChild(1);
            LightController lampController = lamp.GetComponent<LightController>();
            lampController.ChangeLightColor(Color.white);
            lampController.ChangeEmissionColor(Color.white);
            lampController.ChangeLightIntensity(1);
        }
    }

    private IEnumerator LampTimerRoutine()
    {
        // Ждем пока не истечет время таймера
        yield return new WaitForSeconds(0.5f);

        // Выполняем действия после истечения времени
        LampTimerOnExpired();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnBombs(int count)
    {
        List<Transform> elements = selectRandomRoofSegments(count);
        for (int i = 0; i < count; i++) //перемешивание мест спавна, чтобы около игрока спавнились не только лечебные бомбы
        {
            int a = Random.Range(0, count), b = Random.Range(0, count);
            Transform temp = elements[a];
            elements[a] = elements[b];
            elements[b] = temp;
        }

        int number = 0;
        foreach (Transform element in elements)
        {
            Transform lamp = element.GetChild(1);
            LightController lampController = lamp.GetComponent<LightController>();
            lampController.ChangeLightColor(Color.blue);
            lampController.ChangeEmissionColor(Color.blue);
            lampController.ChangeLightIntensity(4);
            float explodeRadius = bombExplodeRadius;
            int damage = bombDamage;

            if (number < healBombCount) damage *= -1;
            bombSpawner.SpawnBomb(element, timerDuration, damage, explodeRadius);
            number += 1;
        }

        StartLampTimer();
    }

    List<Transform> selectRandomRoofSegments(int count)
    {
        List<Transform> tempList = new List<Transform>();
        List<Transform> res = new List<Transform>();
        foreach(Transform a in roofSegments) {
            tempList.Add(a);
        }

        if (count > tempList.Count) count = tempList.Count;

        // всегда выбираем ближайшую к игроку позицию, чтобы он не стоял в углу
        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;

        int nearestRoofSegmentIndex = 0;
        float nearestDistance = 50f;

        for (int i = 0; i < tempList.Count; i++)
        {
            float curDistance = Vector3.Distance(playerPos, tempList[i].position);
            if (nearestDistance > curDistance)
            {
                nearestRoofSegmentIndex = i;
                nearestDistance = curDistance;
            }
        }

        res.Add(tempList[nearestRoofSegmentIndex]);
        tempList.Remove(tempList[nearestRoofSegmentIndex]);

        // выбираем count - 1 случайных позиций (1 уже выбрана)
        for (int i = 1; i < count; i++)
        {
            int index = Random.Range(0, tempList.Count);
            res.Add(tempList[index]);
            tempList.Remove(tempList[index]);
        }

        return res;
    }
}
