using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent( typeof( Camera ) )]
public class AdjustWalls : MonoBehaviour
{
    public static Vector2 bounds { get; private set; }


    public GameObject leftWall;
    public GameObject rightWall;
    public GameObject upWall;
    public GameObject downWall;



    void Start() {

        UpdateWalls();
    }

    void Update() {

        UpdateWalls();
    }

    void UpdateWalls() {

        float mapHeight = camera.orthographicSize;
        float mapWidth = camera.orthographicSize * camera.aspect;

        leftWall.transform.position = new Vector3( -mapWidth - 0.5f, 0f );
        rightWall.transform.position = new Vector3( mapWidth + 0.5f, 0f );
        upWall.transform.position = new Vector3( 0f, mapHeight + 0.5f, 0f );
        downWall.transform.position = new Vector3( 0f, -mapHeight - 0.5f, 0f );

        bounds = new Vector2( mapWidth, mapHeight );
    }
}
