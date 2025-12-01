using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    //플레이어 좌우 회전 처리
    public float speed = 480f;

    //회전 각도 직접 제어
    float angleX;

    // Update is called once per frame
    void Update()
    {
        Rotate();
    }

    void Rotate()
    {
        float h = Input.GetAxisRaw("Mouse X");
        angleX += h * speed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, angleX, 0);
    }
}
