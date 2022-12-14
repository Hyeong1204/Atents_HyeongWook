using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public Transform[] bgSlots;

    public float scorlloingSpeed = 2.5f;
    
    const float Background_Width = 13.6f;

    protected virtual void Awake()
    {        
        bgSlots = new Transform[transform.childCount];

        for(int i = 0; i < transform.childCount; i++)           // 정확한 인덱스가 필요할 때 유리
        {
            bgSlots[i] = transform.GetChild(i);
        }
    }

    protected virtual void Update()
    {
        float minusX = transform.position.x - Background_Width;
        //foreach(Transform slot in bgSlots)     // 속도가 그냥 for보다 빠름
        //{
        //    slot.Translate(scorlloingSpeed * Time.deltaTime *  -transform.right);

        //    if(slot.position.x < minusX)
        //    {
        //        slot.Translate(Background_Width * bgSlots.Length * transform.right);        // 오른쪽으로 Background_Width에 3배(bgSlots.Length에 3개가 들어 있으니까) 만큼 이동
        //    }
        //}

        for(int i = 0; i < bgSlots.Length; i++)
        {
            bgSlots[i].Translate(scorlloingSpeed * Time.deltaTime * -transform.right);

            if (bgSlots[i].position.x < minusX)
            {
                // 오른쪽으로 Background_Width에 3배(bgSlots.Length에 3개가 들어 있으니까) 만큼 이동
                MoveRightEnd(i);
            }
        }
    }

    protected virtual void MoveRightEnd(int index)
    {
        bgSlots[index].Translate(Background_Width * bgSlots.Length * transform.right);
    }
}
