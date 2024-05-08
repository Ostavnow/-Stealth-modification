using System.Collections.Generic;
using UnityEngine;
public class MovementPath : MonoBehaviour
{

    public enum PathTypes
    {
        linear,
        loop
    }
    public PathTypes PathType;
    public int movementDirection = 1;
    public int movengTo = 0;
    LineRenderer line;
    public Transform[] PathElements;
    private void Awake() {
        line = GetComponent<LineRenderer>();
    }
    private void Start() {
        line.positionCount = PathElements.Length;
        Abilities.lActivity += ActivityLines;
        
        for(int i = 0;i < PathElements.Length;i++)
        {
            line.SetPosition(i,new Vector3(PathElements[i].position.x,17,PathElements[i].position.z));
        }
        line.enabled = false;
    }
    void OnDestroy()
    {
        Abilities.lActivity -= ActivityLines;
    }

    public void OnDrawGizmos() //Рисует линии между точками
    {
        if(PathElements == null || PathElements.Length < 2)
        {
            return;
        }
        for (int i = 1; i < PathElements.Length; i++)
        {
            Gizmos.DrawLine(PathElements[i - 1].position,PathElements[i].position);
        }
        if(PathType == PathTypes.loop)
        {
            Gizmos.DrawLine(PathElements[0].position,PathElements[PathElements.Length - 1].position);
        }
    }
    public IEnumerator<Transform> GetNextPathPoint() //Находит позиции точек
    {
        if(PathElements == null || PathElements.Length < 1)
        {
            yield break;
        }
        while (true)
        {
            yield return PathElements[movengTo];
            if(PathElements.Length == 1)
            {
                continue;
            } 
            if(PathType == PathTypes.linear)
            {
                if(movengTo <= 0)
                {
                    movementDirection = 1;
                }
                else if(movengTo >= PathElements.Length - 1)
                {
                    movementDirection = -1;
                }
            }
            movengTo = movengTo + movementDirection;
            if(PathType == PathTypes.loop)
            {
                if(movengTo >= PathElements.Length)
                {
                    movengTo = 0;
                }
                if(movengTo < 0)
                {
                    movengTo = PathElements.Length - 1;
                }
            }
        }
    }
    public bool ActivityLines(bool isAst)
    {
        line.enabled = isAst;
        return true;
    }
}
