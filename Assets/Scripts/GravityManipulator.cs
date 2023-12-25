using System;
using UnityEngine;

public class GravityManipulator : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float offset = 2.4f;
    [SerializeField] private GameObject hologram;

    public AXIS current_gravity_axis { get; private set; }

    private bool isKeyDown = false;

    public Quaternion hologram_rotation;
    public Vector3 hologram_position;

    public Action<AXIS> onGravityChanged;

    public enum AXIS
    {
        DOWN,
        UP,
        LEFT,
        RIGHT
    }
    private float SnapToNearest90(float angle)
    {
        return Mathf.Round(angle / 90) * 90;
    }
    private void Update()
    {
        hologram.SetActive(true);
        hologram.transform.position = new Vector3(playerTransform.localPosition.x, playerTransform.localPosition.y + offset, playerTransform.localPosition.z);
        hologram.transform.rotation = playerTransform.localRotation;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            isKeyDown = true;
            float snappedY = SnapToNearest90(playerTransform.localEulerAngles.y);
            hologram.transform.localRotation = Quaternion.Euler(180, snappedY, 0);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            isKeyDown = true;
            float snappedY = SnapToNearest90(playerTransform.localEulerAngles.y);
            hologram.transform.localRotation = Quaternion.Euler(0, snappedY, 0);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            isKeyDown = true;
            float snappedY = SnapToNearest90(playerTransform.localEulerAngles.y);
            hologram.transform.localRotation = Quaternion.Euler(0, snappedY, -90);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            isKeyDown = true;
            float snappedY = SnapToNearest90(playerTransform.localEulerAngles.y);
            hologram.transform.localRotation = Quaternion.Euler(0, snappedY, 90);
        }
        else
        {
            isKeyDown = false;
            hologram.SetActive(false);
        }
        if (isKeyDown && Input.GetKeyDown(KeyCode.Return))
        {
            hologram_rotation = hologram.transform.localRotation;
            hologram_position = hologram.transform.localPosition;

            Physics.gravity = hologram.transform.up * -10f;

            onGravityChanged?.Invoke(current_gravity_axis);
        }
    }
}
