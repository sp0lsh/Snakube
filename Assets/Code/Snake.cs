using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class Snake : MonoBehaviour
{

    public float moveSpeed = 0.1f;
    public GameObject foodPrefab;

    bool _papu;
    Vector2 _snakeDir = Vector2.right;


    void Start() {

        SpawnFood();
        InvokeRepeating( "Move", moveSpeed, moveSpeed );
    }

    void OnTriggerEnter( Collider coll ) {

        if ( coll.gameObject.tag == "Food" ) {

            Debug.Log( "Food collected" );

            _papu = true;
            SpawnFood();
            Destroy( coll.gameObject );
        }
    }

    void OnCollisionEnter( Collision coll ) {

        if ( coll.gameObject.tag == "Tail" ) {

            Debug.Log( "Fail" );
            
            Application.LoadLevel( Application.loadedLevel );
        }
    }

    void Update() {

        if ( Input.GetKey( KeyCode.W ) || Input.GetKey( KeyCode.UpArrow ) ) {
            if ( _snakeDir != -Vector2.up ) {
                _snakeDir = Vector2.up;
            }
        }

        if ( Input.GetKey( KeyCode.A ) || Input.GetKey( KeyCode.LeftArrow ) ) {
            if ( _snakeDir != Vector2.right ) {
                _snakeDir = -Vector2.right;
            }
        }

        if ( Input.GetKey( KeyCode.D ) || Input.GetKey( KeyCode.RightArrow ) ) {
            if ( _snakeDir != -Vector2.right ) {
                _snakeDir = Vector2.right;
            }
        }

        if ( Input.GetKey( KeyCode.S ) || Input.GetKey( KeyCode.DownArrow ) ) {
            if ( _snakeDir != Vector2.up ) {
                _snakeDir = -Vector2.up;
            }
        }
    }

    void Move() {

        Vector2 wektor = transform.position;
        transform.Translate( _snakeDir );
    }

    void SpawnFood() {

        Vector3 randomFoodPos = new Vector3(
            (int)( 5f * 2f * ( Random.value - 0.5f )),
            (int)( 5f * 2f * ( Random.value - 0.5f )),
            0f
        );

        GameObject newFood = Instantiate( foodPrefab, randomFoodPos, Quaternion.identity ) as GameObject;
    }
}