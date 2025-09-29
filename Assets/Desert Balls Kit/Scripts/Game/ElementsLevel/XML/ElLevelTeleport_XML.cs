using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElLevelTeleport_XML : BaseElLevel_XML
{
    public Vector2 PositionOut;
    public float RoatateOut;


    public ElLevelTeleport_XML()
    {
        TypeElement = ForEnum.GetElTypeElement(this.GetType());
    }


    public override bool HasVal(TypeVal _TypeVal)
    {
        switch (_TypeVal)
        {
            case TypeVal.Point2:
            case TypeVal.Rotate2:
                return true;
        }
        return base.HasVal(_TypeVal);
    }

    public override void SetVal(TypeVal _TypeVal, object val)
    {
        base.SetVal(_TypeVal, val);
        switch (_TypeVal)
        {
            case TypeVal.Point2:
                PositionOut = (Vector2)val;
                break;
            case TypeVal.Rotate2:
                RoatateOut = (float)val;
                break;
        }
    }

    public override object GetVal(TypeVal _TypeVal)
    {
        switch (_TypeVal)
        {
            case TypeVal.Point2:
                return PositionOut;
            case TypeVal.Rotate2:
                return RoatateOut;
        }
        return base.GetVal(_TypeVal);
    }

    public override string GetNameVal(TypeVal _TypeVal)
    {
        switch (_TypeVal)
        {
            case TypeVal.Point2:
                return "Position Out";
            case TypeVal.Rotate2:
                return "Rotate Out";
        }
        return base.GetNameVal(_TypeVal);
    }

    public override void SetValuesGameObject()
    {
        base.SetValuesGameObject();
        if (_BaseElLevel != null)
        {
            ((ElLevelTeleport)_BaseElLevel).PositionOut = PositionOut;
            ((ElLevelTeleport)_BaseElLevel).RoatateOut = RoatateOut;
        }
    }

    public override void Default()
    {
        base.Default();

        PositionOut = new Vector2(0, -2);
        RoatateOut = 0;
    }
}
