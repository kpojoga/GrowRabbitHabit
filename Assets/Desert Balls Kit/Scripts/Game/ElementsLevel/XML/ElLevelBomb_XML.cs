using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElLevelBomb_XML : BaseElLevel_XML
{
    public float RadiusBoom;

    public ElLevelBomb_XML()
    {
        TypeElement = ForEnum.GetElTypeElement(this.GetType());
    }


    public override bool HasVal(TypeVal _TypeVal)
    {
        switch (_TypeVal)
        {
            case TypeVal.Radius:
                return true;
        }
        return base.HasVal(_TypeVal);
    }

    public override void SetVal(TypeVal _TypeVal, object val)
    {
        base.SetVal(_TypeVal, val);
        switch (_TypeVal)
        {
            case TypeVal.Radius:
                RadiusBoom = (float)val;
                break;
        }
    }

    public override object GetVal(TypeVal _TypeVal)
    {
        switch (_TypeVal)
        {
            case TypeVal.Radius:
                return RadiusBoom;
        }
        return base.GetVal(_TypeVal);
    }

    public override string GetNameVal(TypeVal _TypeVal)
    {
        switch (_TypeVal)
        {
            case TypeVal.Radius:
                return "Radius Boom";
        }
        return base.GetNameVal(_TypeVal);
    }

    public override void SetValuesGameObject()
    {
        base.SetValuesGameObject();
        if (_BaseElLevel != null)
        {
            ((ElLevelBomb)_BaseElLevel).RadiusBoom = RadiusBoom;
        }
    }

    public override void Default()
    {
        base.Default();

        RadiusBoom = 1.5f;
    }
}
