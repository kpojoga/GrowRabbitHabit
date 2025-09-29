using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElLevelSandCutRectangle_XML : BaseElLevel_XML
{
    public Vector2 Point1;
    public Vector2 Point2;


    public ElLevelSandCutRectangle_XML()
    {
        TypeElement = ForEnum.GetElTypeElement(this.GetType());
    }


    public override bool HasVal(TypeVal _TypeVal)
    {
        switch (_TypeVal)
        {
            case TypeVal.Point1:
            case TypeVal.Point2:
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
            case TypeVal.Point2:
                Point2 = (Vector2)val;
                break;
        }
    }

    public override object GetVal(TypeVal _TypeVal)
    {
        switch (_TypeVal)
        {
            case TypeVal.Point1:
                return Point1;
            case TypeVal.Point2:
                return Point2;
        }
        return base.GetVal(_TypeVal);
    }

    public override string GetNameVal(TypeVal _TypeVal)
    {
        switch (_TypeVal)
        {
            case TypeVal.Point1:
                return "Point 1";
            case TypeVal.Point2:
                return "Point 2";
        }
        return base.GetNameVal(_TypeVal);
    }

    public override void SetValuesGameObject()
    {
        base.SetValuesGameObject();
        if (_BaseElLevel != null)
        {
            ((ElLevelSandCutRectangle)_BaseElLevel).Point1 = Point1;
            ((ElLevelSandCutRectangle)_BaseElLevel).Point2 = Point2;
        }
    }

    public override void Default()
    {
        base.Default();

        Point1 = new Vector2(0.5f, 0.5f);
        Point2 = new Vector2(-0.5f, -0.5f);
    }
}
