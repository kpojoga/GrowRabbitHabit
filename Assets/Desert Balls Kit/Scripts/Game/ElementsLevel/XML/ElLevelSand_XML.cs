using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElLevelSand_XML : BaseElLevel_XML
{
    public Vector2 Point1;


    public ElLevelSand_XML()
    {
        TypeElement = ForEnum.GetElTypeElement(this.GetType());
    }


    public override bool HasVal(TypeVal _TypeVal)
    {
        switch (_TypeVal)
        {
            case TypeVal.Point1:
                return true;
        }
        return base.HasVal(_TypeVal);
    }

    public override void SetVal(TypeVal _TypeVal, object val)
    {
        base.SetVal(_TypeVal, val);
        switch (_TypeVal)
        {
            case TypeVal.Point1:
                Point1 = (Vector2)val;
                break;
        }
    }

    public override object GetVal(TypeVal _TypeVal)
    {
        switch (_TypeVal)
        {
            case TypeVal.Point1:
                return Point1;
        }
        return base.GetVal(_TypeVal);
    }

    public override string GetNameVal(TypeVal _TypeVal)
    {
        switch (_TypeVal)
        {
            case TypeVal.Point1:
                return "Size";
        }
        return base.GetNameVal(_TypeVal);
    }

    public override void SetValuesGameObject()
    {
        base.SetValuesGameObject();
        if (_BaseElLevel != null)
        {
            ((ElLevelSand)_BaseElLevel).Size = Point1;
        }
    }

    public override void Default()
    {
        base.Default();

        Point1 = new Vector2(1, 1);
    }
}
