using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// 카메라를 마우스가 움직이는 방향으로 회전하기
/// </summary>
public class CamRotate : MonoBehaviour
{
    public float speed = 480f;          //회전 속도 ( Time.deltaTime 곱해서 초당 200도로 회전 )
    float angleX, angleY;               //직접 제어할 회전 각도
    void Update()
    {
        //카메라 회전
        Rotate();
    }

    void Rotate()
    {
        float h = Input.GetAxisRaw("Mouse X");      //마우스 가로 움직임
        float v = Input.GetAxisRaw("Mouse Y");      //마우스 세로 움직임
        Vector3 dir = new Vector3(-v, h, 0);        //회전 방향 벡터
        //-v를 한 이유는, 스크린에서는 좌측 상단이 0,0 이고, 우측 하단이 해상도(1920, 1080)이라 아래로 갈수록 값이 커진다. 그래서 반대로 생각해줘야함.
        //회전은 각각의 축을 기준으로 회전을 함.
        //transform.Rotate(-v * speed * Time.deltaTime, h * speed * Time.deltaTime, 0);
        //transform.Rotate(dir * speed * Time.deltaTime);

        //유니티엔진 내부적으로 -각도는 360도를 더한 값으로 변환해서 처리한다.
        //따라서 우리가 직접 각도를 제어해서 사용해야 회전처리가 편하다
        angleX += h * speed * Time.deltaTime;       //가로 움직임으로 X축 회전 각도 변경
        angleY += v * speed * Time.deltaTime;       //세로 움직임으로 Y축 회전 각도 변경
        
        angleY = Mathf.Clamp(angleY, -70, 70);      //Y축 회전 각도 제한

        transform.eulerAngles = new Vector3(-angleY, angleX, 0);
    }

}
