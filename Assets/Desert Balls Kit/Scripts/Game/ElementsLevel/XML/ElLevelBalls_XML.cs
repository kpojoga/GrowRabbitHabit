using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElLevelBalls_XML : BaseElLevel_XML
{
    public float Radius;
    public bool Sleeping;


    public ElLevelBalls_XML()
    {
        TypeElement = ForEnum.GetElTypeElement(this.GetType());
    }


    public override bool HasVal(TypeVal _TypeVal)
    {
        switch (_TypeVal)
        {
            case TypeVal.Radius:
            case TypeVal.Bool1:
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
            case TypeVal.Bool1:
                Sleeping = (bool)val;
                break;
        }
    }

    public override object GetVal(TypeVal _TypeVal)
    {
        switch (_TypeVal)
        {
            case TypeVal.Radius:
                return Radius;
            case TypeVal.Bool1:
                return Sleeping;
        }
        return base.GetVal(_TypeVal);
    }

    public override string GetNameVal(TypeVal _TypeVal)
    {
        switch (_TypeVal)
        {
            case TypeVal.Radius:
                return "Radius";
            case TypeVal.Bool1:
                return "Sleeping";
        }
        return base.GetNameVal(_TypeVal);
    }

    public override void SetValuesGameObject()
    {
        base.SetValuesGameObject();
        if (_BaseElLevel != null)
        {
            ((ElLevelBalls)_BaseElLevel).Radius = Radius;
            ((ElLevelBalls)_BaseElLevel).Sleeping = Sleeping;
        }
    }

    public override void Default()
    {
        base.Default();

        Radius = 0.4f;
        Sleeping = true;
    }
}
