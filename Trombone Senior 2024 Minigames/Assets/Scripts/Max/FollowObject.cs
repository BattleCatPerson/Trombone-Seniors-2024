using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class FollowObject : MonoBehaviour
{
    [SerializeField] Vector2 offset;
    [SerializeField] Transform target;
    [SerializeField] int layer;
    [SerializeField] Transform sprite;
    [SerializeField] Vector2 point;
    private void Start()
    {
        layer = 1 << layer;
    }
    void Update()
    {
        //raycast ground, make y level yOffset from that raycast point! eASTY!
        transform.position = target.position + Vector3.right * offset.x + Vector3.up * 10f;
        point = (Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, layer).point);
        sprite.position = point + Vector2.up * offset.y;
    }
}
