using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// 사용 가능한 오브젝트가 가지는 인터페이스
/// </summary>
interface IUseavleObject
{
    /// <summary>
    /// 오브젝트가 사용될 때 사용될 함수
    /// </summary>
    void Use(); // 오브젝트 사용
}
