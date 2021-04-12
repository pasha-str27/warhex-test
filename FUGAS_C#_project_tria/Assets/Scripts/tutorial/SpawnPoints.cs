using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
    public int count = 3;
    public GameObject point;

    public Vector2 goal;
    static PlayerControllerTutorial _playerManager;
    void Start()
    {
        if (_playerManager == null)
            _playerManager = GameObject.FindGameObjectWithTag("playerController").GetComponent<PlayerControllerTutorial>();
        StartCoroutine(spawner());
    }

    IEnumerator spawner()
    {
        for(int i=0;i<count;++i)
        {
            yield return new WaitForSeconds(0.3f);
            GameObject newPoint=Instantiate(point, transform.position, Quaternion.identity);
            newPoint.transform.SetParent(this.transform);
            //newPoint.transform.SetParent(this.transform);
            var movePoint_ = newPoint.GetComponent<movePoint>();
            movePoint_.endLine = goal;
            movePoint_.goal = goal;
            _playerManager.spheres.Add(newPoint);
        }
    }
}
