using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Triangle
{
    public Vector3 Vertex1;
    public Vector3 Vertex2;
    public Vector3 Vertex3;
}

public class PatrolScope : MonoBehaviour
{
	#region Fields & Properties
	[SerializeField] protected List<Transform> _corners;
    protected List<Triangle> _triangles;

	#endregion

	#region Methods
	private void Awake()
	{
        _triangles = new List<Triangle>();
		for(int i = 0; i < _corners.Count; i++)
		{
            for (int j = i + 1; j < _corners.Count; j++)
			{
                for (int k = j + 1; k < _corners.Count; k++)
				{
                    _triangles.Add(new Triangle()
                    {
                        Vertex1 = _corners[i].position,
                        Vertex2 = _corners[j].position,
                        Vertex3 = _corners[k].position
                    });
				}
			}
		}
	}

	public Vector3 GetRandomDestination(Vector3 transformPosition)
	{
        Vector3 GenVector = RandomWithinTriangle(_triangles[Random.Range(0, _triangles.Count)]);

		while (Vector3.Distance(transformPosition, GenVector) < 2f)
		{
			GenVector = RandomWithinTriangle(_triangles[Random.Range(0, _triangles.Count)]);
		}
        GenVector.y = transformPosition.y;
		return GenVector;
    }

    public bool IsPointInPolygon(Vector3 point)
    {
        bool inside = false;

        int polygonLength = _corners.Count;
        int i = 0;
        // x, y for tested point.
        float pointX = point.x, pointY = point.z;
        // start / end point for the current polygon segment.
        float startX, startY, endX, endY;
        endX = _corners[polygonLength - 1].position.x;
        endY = _corners[polygonLength - 1].position.z;
        while (i < polygonLength)
        {
            startX = endX; startY = endY;
            
            endX = _corners[i++].position.x;
            endY = _corners[i++].position.z;
            //
            inside ^= (endY > pointY ^ startY > pointY) /* ? pointY inside [startY;endY] segment ? */
                      && /* if so, test if it is under the segment */
                      ((pointX - endX) < (pointY - endY) * (startX - endX) / (startY - endY));
        }
        return inside;
    }

    private Vector3 RandomWithinTriangle(Triangle triangle)
    {
        var r1 = Mathf.Sqrt(Random.Range(0f, 1f));
        var r2 = Random.Range(0f, 1f);
        var m1 = 1 - r1;
        var m2 = r1 * (1 - r2);
        var m3 = r2 * r1;

        Vector2 p1;
        p1.x = triangle.Vertex1.x;
        p1.y = triangle.Vertex1.z;

        Vector2 p2;
        p2.x = triangle.Vertex2.x;
        p2.y = triangle.Vertex2.z;

        Vector2 p3;
        p3.x = triangle.Vertex3.x;
        p3.y = triangle.Vertex3.z;

        Vector2 result = (m1 * p1) + (m2 * p2) + (m3 * p3);
        return new Vector3(result.x, 1f, result.y);
    }

    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {
        if (_corners == null)
		{
            return;
		}


        for(int i = 0; i < _corners.Count; i++)
		{
            DebugExtension.DrawPoint(_corners[i].position);
            for(int j = i + 1; j < _corners.Count; j++)
			{
                Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
                Gizmos.DrawLine(_corners[i].position, _corners[j].position);
            }
		}
    }
    #endregion
}

