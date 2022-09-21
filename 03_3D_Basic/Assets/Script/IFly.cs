using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IFly
{
    void Fly(Vector3 flyVector);        // 이 인터페이스를 상속받은 클래스를 날려버리는 함수
}
