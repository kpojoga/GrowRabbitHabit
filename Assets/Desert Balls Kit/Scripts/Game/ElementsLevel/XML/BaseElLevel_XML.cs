using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.ComponentModel;


public enum TypeVal
{
    Position,
    Rotate,
    Point1,
    Point2,
    Rotate1,
    Rotate2,
    Radius,
    typeSB,
    typeLR,
    Float1,
    Int1,
    Bool1
}

public enum TypeSB
{
    SMALL,
    BIG
}

public enum TypeLR
{
    LEFT,
    RIGHT
}

[XmlInclude(typeof(ElLevelRectangle_XML))]
[XmlInclude(typeof(ElLevelSand_XML))]
[XmlInclude(typeof(ElLevelCircle_XML))]
[XmlInclude(typeof(ElLevelSwing_XML))]
[XmlInclude(typeof(ElLevelMill_XML))]
[XmlInclude(typeof(ElLevelRectangleDynamic_XML))]
[XmlInclude(typeof(ElLevelFanKill_XML))]
[XmlInclude(typeof(ElLevelBalls_XML))]
[XmlInclude(typeof(ElLevelBigBall_XML))]
[XmlInclude(typeof(ElLevelRectangleBreaking_XML))]
[XmlInclude(typeof(ElLevelSandCutCircle_XML))]
[XmlInclude(typeof(ElLevelSandCutRectangle_XML))]
[XmlInclude(typeof(ElLevelBigPin_XML))]
[XmlInclude(typeof(ElLevelLittlePinKill_XML))]
[XmlInclude(typeof(ElLevelTeleport_XML))]
[XmlInclude(typeof(ElLevelBomb_XML))]
public class BaseElLevel_XML
{
    public ElTypeElement TypeElement;
    public Vector2 Position;
    public float Rotate;

    public bool CanRemove = true;

    
    [XmlIgnore]
    public BaseElLevel _BaseElLevel;

    public virtual bool HasVal(TypeVal _TypeVal)
    {
        switch (_TypeVal)
        {
            case TypeVal.Position:
            case TypeVal.Rotate:
                return true;
        }
        return false;
    }

    public virtual void SetVal(TypeVal _TypeVal, object val)
    {
        switch (_TypeVal)
        {
            case TypeVal.Position:
                Position = (Vector2)val;
                break;
            case TypeVal.Rotate:
                Rotate = (float)val;
                break;
        }
    }

    public virtual object GetVal(TypeVal _TypeVal)
    {
        switch (_TypeVal)
        {
            case TypeVal.Position:
                return Position;
            case TypeVal.Rotate:
                return Rotate;
        }
        return null;
    }

    public virtual string GetNameVal(TypeVal _TypeVal)
    {
        switch (_TypeVal)
        {
            case TypeVal.Position:
                return "Position";
            case TypeVal.Rotate:
                return "Rotate";
        }
        return "";
    }

    public virtual void SetValuesGameObject()
    {
        if (_BaseElLevel != null)
        {
            _BaseElLevel.Position = Position;
            _BaseElLevel.Rotate = Rotate;
        }
    }

    public virtual void Default()
    {
        Position = Vector2.zero;
        Rotate = 0;
    }
}