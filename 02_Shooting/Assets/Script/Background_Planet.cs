using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background_Planet : MonoBehaviour
{
    public float speed = 1.0f;
    public float minRightEnd = 40.0f;
    public float maxRightEnd = 60.0f;

    const float movePositionX = -10.0f;

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * -transform.right);

        if(transform.position.x < movePositionX)
        {
            transform.position = new Vector3(Random.Range(minRightEnd, maxRightEnd), transform.position.y, 0);
        }
    }
}
