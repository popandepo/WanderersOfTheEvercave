using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    private GameObject _player;

    private Vector3 _playerPosition;

    // Start is called before the first frame update
    private void Awake()
    {
        Invoke(nameof(MoveCameraToPlayer), 1f);
    }

    // Update is called once per frame
    private void Update()
    {
        float xAxisValue = Input.GetAxis("Horizontal");
        float yAxisValue = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(xAxisValue, yAxisValue, 0.0f));
    }

    private void MoveCameraToPlayer()
    {
        _player = GameObject.FindGameObjectWithTag("Player");

        _playerPosition = _player.transform.position;

        Vector3 newCamPos = new Vector3(_playerPosition.x, _playerPosition.y, -10);

        transform.position = newCamPos;
    }
}