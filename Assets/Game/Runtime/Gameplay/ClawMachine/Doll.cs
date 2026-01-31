using System;
using System.Collections;
using UnityEngine;

public class Doll : MonoBehaviour
{
    public int dollId;

    private Collider2D col;
    private Transform oldParent;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        oldParent = transform.parent;
    }

    public void OnGrabbed()
    {
        if (col) col.enabled = false;
    }

    public void OnForceDropped()
    {
        transform.SetParent(oldParent);
        if (col) col.enabled = true;
    }

    public void PlayDrop(float exitY)
    {
        StartCoroutine(Drop(exitY));
    }

    private IEnumerator Drop(float exitY)
    {
        while (transform.position.y > exitY)
        {
            transform.position += Vector3.down * (4 * Time.deltaTime);
            yield return null;
        }
    }

    public void OnReleasedAtExit()
    {
        // 成功奖励逻辑
        Destroy(gameObject);
    }
}