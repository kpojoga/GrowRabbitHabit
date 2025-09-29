using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Linq;


public enum ElTypeElement
{
    [HideAdd]
    NONE,

    /// <summary>
    /// Class - ElLevelCircle
    /// </summary>
    [Path("ElLevelCircle"), TypeXML("ElLevelCircle_XML"), Description("Circle Static")]
    CircleStatic,

    /// <summary>
    /// Class - ElLevelRectangle
    /// </summary>
    [Path("ElLevelRectangle"), TypeXML("ElLevelRectangle_XML"), Description("Rectangle Static")]
    RectangleStatic,

    /// <summary>
    /// Class - ElLevelBalls
    /// </summary>
    [Path("ElLevelBalls"), TypeXML("ElLevelBalls_XML")]
    Balls,

    /// <summary>
    /// Class - ElLevelBigBall
    /// </summary>
    [Path("ElLevelBigBall"), TypeXML("ElLevelBigBall_XML"), Description("Big Ball")]
    BigBall,

    /// <summary>
    /// Class - ElLevelRectangleBreaking
    /// </summary>
    [Path("ElLevelRectangleBreaking"), TypeXML("ElLevelRectangleBreaking_XML"), Description("Rectangle Breaking")]
    RectangleBreaking,

    /// <summary>
    /// Class - ElLevelBomb
    /// </summary>
    [Path("ElLevelBomb"), TypeXML("ElLevelBomb_XML")]
    Bomb,

    /// <summary>
    /// Class - ElLevelBigPin
    /// </summary>
    [Path("ElLevelBigPin"), TypeXML("ElLevelBigPin_XML"), Description("Big Pin")]
    BigPin,

    /// <summary>
    /// Class - ElLevelRectangleDynamic
    /// </summary>
    [Path("ElLevelRectangleDynamic"), TypeXML("ElLevelRectangleDynamic_XML"), Description("Rectangle Dynamic")]
    RectangleDynamic,

    /// <summary>
    /// Class - ElLevelFanKill
    /// </summary>
    [Path("ElLevelFanKill"), TypeXML("ElLevelFanKill_XML"), Description("Fan Kill")]
    FanKill,

    /// <summary>
    /// Class - ElLevelTeleport
    /// </summary>
    [Path("ElLevelTeleport"), TypeXML("ElLevelTeleport_XML")]
    Teleport,

    /// <summary>
    /// Class - ElLevelLittlePinKill
    /// </summary>
    [Path("ElLevelLittlePinKill"), TypeXML("ElLevelLittlePinKill_XML"), Description("Little Pin Kill")]
    LittlePinKill,

    /// <summary>
    /// Class - ElLevelSwing
    /// </summary>
    [Path("ElLevelSwing"), TypeXML("ElLevelSwing_XML")]
    Swing,

    /// <summary>
    /// Class - ElLevelMill
    /// </summary>
    [Path("ElLevelMill"), TypeXML("ElLevelMill_XML")]
    Mill,

    /// <summary>
    /// Class - ElLevelSand
    /// </summary>
    [Path("ElLevelSand"), TypeXML("ElLevelSand_XML")]
    Sand,

    /// <summary>
    /// Class - ElLevelSandCutCircle
    /// </summary>
    [Path("ElLevelSandCutCircle"), TypeXML("ElLevelSandCutCircle_XML"), Description("Sand Cut Circle")]
    SandCutCircle,

    /// <summary>
    /// Class - ElLevelSandCutRectangle
    /// </summary>
    [Path("ElLevelSandCutRectangle"), TypeXML("ElLevelSandCutRectangle_XML"), Description("Sand Cut Rectangle")]
    SandCutRectangle
}


public static class ForEnum
{
    public static List<ElTypeElement> GetList()
    {
        return Enum.GetValues(typeof(ElTypeElement)).Cast<ElTypeElement>().Where(v => { return !GetHideAdd(v); }).ToList();
    }

    public static string GetTypeName(ElTypeElement _TypeElement)
    {
        var nm = _TypeElement.ToString();
        var tp = _TypeElement.GetType();
        var field = tp.GetField(nm);
        var attrib = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

        if (attrib != null)
            return attrib.Description;
        else
            return nm;
    }

    public static GameObject GetAsset(ElTypeElement _TypeElement)
    {
        var nm = _TypeElement.ToString();
        var tp = _TypeElement.GetType();
        var field = tp.GetField(nm);
        PathAttribute attribute = Attribute.GetCustomAttribute(field, typeof(PathAttribute)) as PathAttribute;

        if (attribute != null)
        {
            UnityEngine.Object o = Resources.Load(attribute.pathAsset);
            return o != null ? (GameObject)o : null;
        }
        else
            return null;
    }

    public static Texture2D GetIcon(ElTypeElement _TypeElement)
    {
        var nm = _TypeElement.ToString();
        var tp = _TypeElement.GetType();
        var field = tp.GetField(nm);
        PathAttribute attribute = Attribute.GetCustomAttribute(field, typeof(PathAttribute)) as PathAttribute;

        if (attribute != null)
        {
            UnityEngine.Object o = Resources.Load(attribute.pathIcon);
            return o != null ? (Texture2D)o : null;
        }
        else
            return null;
    }

    public static bool GetHideAdd(ElTypeElement _TypeElement)
    {
        var nm = _TypeElement.ToString();
        var tp = _TypeElement.GetType();
        var field = tp.GetField(nm);
        HideAddAttribute attribute = Attribute.GetCustomAttribute(field, typeof(HideAddAttribute)) as HideAddAttribute;

        if (attribute != null)
            return true;
        else
            return false;
    }

    static string GetTypeXML_string(ElTypeElement _TypeElement)
    {
        var nm = _TypeElement.ToString();
        var tp = _TypeElement.GetType();
        var field = tp.GetField(nm);
        TypeXMLAttribute attribute = Attribute.GetCustomAttribute(field, typeof(TypeXMLAttribute)) as TypeXMLAttribute;

        if (attribute != null)
        {
            return attribute.type_name;
        }
        else
            return null;
    }

    public static BaseElLevel_XML GetTypeXML(ElTypeElement _TypeElement)
    {
        string type_name = GetTypeXML_string(_TypeElement);

        if (type_name != null)
        {
            Type type = Type.GetType(type_name);
            System.Reflection.ConstructorInfo ci = type.GetConstructor(Type.EmptyTypes);
            if (ci != null)
            {
                return (BaseElLevel_XML)ci.Invoke(null);
            }
            return null;
        }
        else
            return null;
    }

    public static ElTypeElement GetElTypeElement(Type type)
    {
        ElTypeElement rez = ElTypeElement.NONE;
        foreach (ElTypeElement t in GetList())
        {
            if(type.ToString() == GetTypeXML_string(t))
            {
                rez = t;
            }
        }

        return rez;
    }
}

// Path attribute
[AttributeUsage(AttributeTargets.Field)]
public class PathAttribute : Attribute
{
    public readonly string name;
    public readonly string pathAsset;
    public readonly string pathIcon;

    public PathAttribute(string n)
    {
        name = n;
        pathAsset = "ElementsLevel/ForEdit/" + name;
        pathIcon = "ElementsLevel/ForEdit/Icon/" + name;
    }
}

// Attribute hides item from window ""
[AttributeUsage(AttributeTargets.Field)]
public class HideAddAttribute : Attribute
{
    public HideAddAttribute()
    {
    }
}

// XML class name attribute
[AttributeUsage(AttributeTargets.Field)]
public class TypeXMLAttribute : Attribute
{
    public readonly string type_name;

    public TypeXMLAttribute(string t)
    {
        type_name = t;
    }
}