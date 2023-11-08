using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Path : MonoBehaviour
{
    [SerializeField]
    List<Transform> pathNodes;

    public Vector3 GetNodePosition(int index)
    {
        if (index >= pathNodes.Count)
        {
            return Vector3.zero;
        }
        else
        {
            return pathNodes[index].position;
        }
    }

    public int NodeCount()
    {
        return pathNodes.Count;
    }

    public void Clear()
    {
        pathNodes.Clear();
    }

    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {
        if (pathNodes == null)
            return;

        Gizmos.color = Color.red;
        foreach (var node in pathNodes)
        {
            if (node != null)
                Gizmos.DrawSphere(node.position, 0.25f);
        }
    }
}

