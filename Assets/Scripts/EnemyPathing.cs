using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    private WaveConfig waveConfig;
    private List<Transform> wayPoints;
    private int wayPointsIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
        wayPoints = waveConfig.GetPathPrefab();
        transform.position = wayPoints[0].transform.position;
    }

    public void SetWaveConfig(WaveConfig waveConfig)
    {
        this.waveConfig = waveConfig;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (wayPointsIndex <= wayPoints.Count - 1)
        {
            var targetPosition = wayPoints[wayPointsIndex].transform.position;
            var maxDistanceDelta = waveConfig.GetMoveSpeed() * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, maxDistanceDelta);

            if (transform.position == wayPoints[wayPointsIndex].position)
            {
                wayPointsIndex++;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
