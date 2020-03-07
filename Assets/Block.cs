using UnityEngine;

public class Block : MonoBehaviour
{
    public int id;

    public Type type = Type.normal;

    public int[] neighbors;
    
    public enum Type
    {
        normal,
        white
    }
}
