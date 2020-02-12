using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlGamePlay : MonoBehaviour
{
    public int countX;
    public int countY;
    public int[,] Board;
    public Vector2 initPoint;
    public float offsetX;
    public float offsetY;
    public GameObject PrebShape;
    public List<GameObject> Cubes = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        Board = new int[countX, countY];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SpawnShape();
        }
    }
    public void SpawnShape()
    {
        int x = Random.Range(0,countX);
        Instantiate(PrebShape, new Vector3(initPoint.x + x * offsetX, initPoint.y),Quaternion.identity,null);
    }
    public void AddCubeIntoBoard(GameObject cube)
    {
        Cubes.Add(cube);
    }
    public void DestroyCube(int x, int y)
    {

    }

}
