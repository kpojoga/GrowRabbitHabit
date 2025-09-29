using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElLevelRectangleBreaking_XML : BaseElLevel_XML
{
    public float Width;
    public float BreakForce;

    public ElLevelRectangleBreaking_XML()
    {
        TypeElement = ForEnum.GetElTypeElement(this.GetType());
    }


    public override bool HasVal(TypeVal _TypeVal)
    {
        switch (_TypeVal)
        {
            case TypeVal.Point1:
            case TypeVal.Float1:
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
                Width = ((Vector2)val).x*2;
                break;
            case TypeVal.Float1:
                BreakForce = (float)val;
                break;
        }
    }

    public override object GetVal(TypeVal _TypeVal)
    {
        switch (_TypeVal)
        {
            case TypeVal.Point1:
                return new Vector2(Width/2, 0);
            case TypeVal.Float1:
                return BreakForce;
        }
        return base.GetVal(_TypeVal);
    }

    public override string GetNameVal(TypeVal _TypeVal)
    {
        switch (_TypeVal)
        {
            case TypeVal.Point1:
                return "Width";
            case TypeVal.Float1:
                return "Break Force";
        }
        return base.GetNameVal(_TypeVal);
    }

    public override void SetValuesGameObject()
    {
        base.SetValuesGameObject();
        if (_BaseElLevel != null)
        {
            ((ElLevelRectangleBreaking)_BaseElLevel).Width = Width;
            ((ElLevelRectangleBreaking)_BaseElLevel).BreakForce = BreakForce;
        }
    }

    public override void Default()
    {
        base.Default();

        Width = 1;
        BreakForce = 8;
    }
}
