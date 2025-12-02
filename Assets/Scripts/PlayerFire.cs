using UnityEngine;

/// <summary>
/// 1. 총알 발사, 파편 튀기 ( 레이로 충돌 처리 )
/// 2. 수류탄 투척
/// </summary>
public class PlayerFire : MonoBehaviour
{
    public Transform firePos;                   //총 발사 위치
    public GameObject bulletImpactFactory;      //총알 파편 프리팹
    public GameObject bombFactory;              //폭탄 프리팹
    public float throwPower = 10;               //던지는 힘


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //총알 및 수류탄 발사
        Fire();
    }

    /// <summary>
    /// 총알 및 수류탄 발사
    /// </summary>
    void Fire()
    {
        //마우스 왼쪽 버튼으로 RayCast로 총알 발사
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);       //시작위치, 방향
            RaycastHit hit;

            //레이랑 출동했니
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("충돌 오브젝트 " + hit.collider.name);

                //충돌 지점에 총알 파편 생성
                GameObject bulletImpact = Instantiate(bulletImpactFactory);
                bulletImpact.transform.position = hit.point;            //hit한 포인트로 위치 지정


                //파편이 부딪힌 지점이 향하는 방향으로 튀게 해줘야 한다
                //Hit 정보 안에 노멀 벡터의 값도 알 수 있다.
                //법선 벡터 (or 노멀 벡터)는 평면에 수직인 벡터
                bulletImpact.transform.forward = hit.normal;        //평면에 수직인 벡터 << 이걸 적으면 벽면고 ㅏ수직인 방향으로 파편이 튀게 만들어 줄 수 있음.ㅣ
            }

            // 레이어 마스크 사용 충돌처리(최적화)
            // tag보다 약 20배 빠르다
            // 총 32비트를 사용하기 때문에 32개 까지 추가 가능
            //int layer = gameObject.layer;
            //layer = 1 << 6;
            ////0000 0000 0000 0001 => 0000 0000 0010 0000  레이어 확인 위치를 6번으로 옮겨준다?

            ////0000 0000 0000 1000 => Enemy
            ////0000 0000 1000 0000 => Boss
            ////0000 1000 0000 0000 => Player 이라고 할 떄
            //layer = 1 << 4 | 1 << 8 | 1 << 12;
            ////0000 1000 1000 1000 << 위의 내용을 토대로 이렇게 만들어 준다는 뜻/ 모두다 충돌 처리를 하겠다 라는 뜻 (Enemy, Boss, Player 1의 위치로 다 옮겨짐)


            //if (Physics.Raycast(ray, out hit, 100, layer)) // 이 레이어에 적용된 것들 전부다 충돌
            //{
            //    //if플레이어라면
            //    //if적이라면
            //    //if보스라면
            //}

            //if(Physics.Raycast(ray,out hit, 100, ~layer))   //이 레이어에 적용된 것들 제외하고 충돌
            //{

            //}
        }

        //폭탄 던지기
        if (Input.GetKeyDown(KeyCode.E))
        {
            //폭탄 오브젝트 생성
            GameObject bomb = Instantiate(bombFactory);
            bomb.transform.position = firePos.position;

            //폭탄은 플레이어가 던지기 때문에 폭탄이 들고 있는 리지드바디를 이용하면 된다
            Rigidbody rb = bomb.GetComponent<Rigidbody>();

            ////던지는 방법 1 : 전방으로 물리적인 힘을 가하는 방법
            //rb.AddForce(Camera.main.transform.forward *  throwPower, ForceMode.Impulse);


            //ForceMode.Acceleration;   => 연속적인 힘을 가할 때 ( 질량 영향 X )
            //ForceMode.Force;          => 연속적인 힘          ( 질량 영향 O )
            //ForceMode.VelocityChange; => 순간적인 힘을 가할 때 ( 질량 영향 X )
            //ForceMode.Impulse;        => 순간적인 힘          ( 질량 영향 O )

            //45도 정도의 각도로 발사
            //벡터의 덧셈 (up + forward)
            //각도를 낮추고 싶을 때 => forward의 길이를 늘려준다
            //각도를 높이고 싶을 때 => up의 길이를 늘려준다
            //Vector3 dir = Camera.main.transform.forward + Camera.main.transform.up; << 45도
            Vector3 dir = Camera.main.transform.forward * 3f + Camera.main.transform.up;
            dir.Normalize();
            rb.AddForce(dir * throwPower, ForceMode.Impulse);
           
        }



        //스나이퍼 모드 , 화각으로 줌인, 줌아웃을 표현하는 방법
        if (Input.GetMouseButtonDown(1))
        {
            Camera.main.fieldOfView = 20f;      //3배 확대한다는 뜻.
        }
        if (Input.GetMouseButtonUp(1))
        {
            Camera.main.fieldOfView = 60f;      //FOV는 보통 60이 기본값
        }
    }
}