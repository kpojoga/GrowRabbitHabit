using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElLevelSandCutCircle_XML : BaseElLevel_XML
{
    public float Radius;


    public ElLevelSandCutCircle_XML()
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
                Radius = (float)val;
                break;
        }
    }

    public override object GetVal(TypeVal _TypeVal)
    {
        switch (_TypeVal)
        {
            case TypeVal.Radius:
                return Radius;
        }
        return base.GetVal(_TypeVal);
    }

    public override string GetNameVal(TypeVal _TypeVal)
    {
        switch (_TypeVal)
        {
            case TypeVal.Radius:
                return "Radius";
        }
        return base.GetNameVal(_TypeVal);
    }

    public override void SetValuesGameObject()
    {
        base.SetValuesGameObject();
        if (_BaseElLevel != null)
        {
            ((ElLevelSandCutCircle)_BaseElLevel).Radius = Radius;
        }
    }

    public override void Default()
    {
        base.Default();

        Radius = 0.5f;
    }
}
