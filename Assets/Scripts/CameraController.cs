using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    public float cameraZops = -10f;
    public float cameraXOffset = 5f;
    public float cameraYOffset = 1f;

    public float horizontalSpeed = 2f;
    public float verticalSpeed = 10f;

    private Transform _camera;
    private PlayerController _playerController;


    // Start is called before the first frame update
    void Start()
    {
        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player");

        }
        _playerController = player.GetComponent<PlayerController>();
        _camera = Camera.main.transform;
        _camera.position = new Vector3(
            player.transform.position.x + cameraXOffset,
            player.transform.position.y + cameraYOffset,
            player.transform.position.z + cameraZops);



    }

    // Update is called once per frame
    void Update()
    {
        if (_playerController.isFactingRight)
        {
            _camera.position = new Vector3(
                Mathf.Lerp(_camera.position.x, player.transform.position.x + cameraXOffset, horizontalSpeed * Time.deltaTime),
                Mathf.Lerp(_camera.position.y, player.transform.position.y + cameraYOffset, verticalSpeed * Time.deltaTime),
                cameraZops) ;
        }
        else
        {
            _camera.position = new Vector3(
               Mathf.Lerp(_camera.position.x, player.transform.position.x - cameraXOffset, horizontalSpeed * Time.deltaTime),
               Mathf.Lerp(_camera.position.y, player.transform.position.y + cameraYOffset, verticalSpeed * Time.deltaTime),
               cameraZops);
        }


    }
}
