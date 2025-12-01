using UnityEngine;

/// <summary>
/// 1. 플레이어 이동
/// 2. 점프
/// </summary>
public class PlayerMove : MonoBehaviour
{
    public float speed = 10f;           //이동속도
    CharacterController cc;             //캐릭터 컨트롤러 컴포넌트

    //중력 적용
    public float gravity = -10f;         //중력 값
    float velocityY = 0;                //낙하 속도
    float jumpPower = 15f;              //점프 파워
    int jumpCount = 0;                  //점프 카운트
    public int jumpMaxCount = 2;               //최대 점프 가능 횟수

    void Start()
    {
        // 캐릭터 컴포넌트 가져오기
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move1();
    }

    void Move1()
    {
        //Translate 
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3 (h, 0, v);
        dir.Normalize();
        //transform.Translate(dir * speed * Time.deltaTime);

        //카메라가 보는 방향을 기준으로 움직이려 할 때
        dir = Camera.main.transform.TransformDirection(dir);

        //캐릭터 컨트롤러 컴포넌트를 이용한  Move
        //cc.Move(dir * speed * Time.deltaTime);

        //공중부양 해결해야함 (중력값 조절)
        velocityY += gravity * Time.deltaTime;
        dir.y = velocityY;

        //cc로 움직이기
        cc.Move(dir * speed * Time.deltaTime);

        // 땅에 닿아있는 상태라면 수직 속도를 0으로 초기화해주기
        //if (cc.isGrounded) //땅에 닿아있으면,
        //{
        //    velocityY = 0;
        //}

        if(cc.collisionFlags == CollisionFlags.Below)       //Capsule 아래부분이 충돌처리가 됐을 때,
        {
            velocityY = 0;
            jumpCount = 0;
        }
        else
        {
            velocityY += gravity * Time.deltaTime;
            dir.y = velocityY;
        }

        if(cc.collisionFlags == CollisionFlags.Above) //머리 닿을 때 (위)
        {
            velocityY += gravity * Time.deltaTime;
            dir.y = velocityY;
        }  
        //if(cc.collisionFlags == CollisionFlags.Side)      //옆면 닿을 때 (옆)
        //if(cc.collisionFlags == CollisionFlags.Below)     //발이 닿을 때 (아래)


        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < jumpMaxCount)
        {
            jumpCount++;
            velocityY = jumpPower;
        }
    }


}
