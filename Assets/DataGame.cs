using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Data/File")]
public class DataGame : ScriptableObject
{
    [SerializeField]
    public List<TypeShapeColor> TypeShape = new List<TypeShapeColor>();
  
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public Sprite[] LoadShape(string s)
    {
        foreach(TypeShapeColor type in TypeShape)
        {
            if(type.name == s)
            {
                return type.Type;
            }
        }
        return null;
    }
   
        
}
[System.Serializable]

public class TypeShapeColor
{
    public string name;
    public Sprite[] Type;


}

