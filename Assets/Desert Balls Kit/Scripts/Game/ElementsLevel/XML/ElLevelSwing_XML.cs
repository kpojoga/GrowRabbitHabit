using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElLevelSwing_XML : BaseElLevel_XML
{
    public float AngularDrag;


    public ElLevelSwing_XML()
    {
        TypeElement = ForEnum.GetElTypeElement(this.GetType());
    }


    public override bool HasVal(TypeVal _TypeVal)
    {
        switch (_TypeVal)
        {
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
            case TypeVal.Float1:
                AngularDrag = (float)val;
                break;
        }
    }

    public override object GetVal(TypeVal _TypeVal)
    {
        switch (_TypeVal)
        {
            case TypeVal.Float1:
                return AngularDrag;
        }
        return base.GetVal(_TypeVal);
    }

    public override string GetNameVal(TypeVal _TypeVal)
    {
        switch (_TypeVal)
        {
            case TypeVal.Float1:
                return "Angular Drag";
        }
        return base.GetNameVal(_TypeVal);
    }

    public override void SetValuesGameObject()
    {
        base.SetValuesGameObject();
        if (_BaseElLevel != null)
        {
            ((ElLevelSwing)_BaseElLevel).AngularDrag = AngularDrag;
        }
    }

    public override void Default()
    {
        base.Default();

        AngularDrag = 1;
    }
}
