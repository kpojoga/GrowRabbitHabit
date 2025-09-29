using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ElLevelSand : BaseElLevel
{
    public Vector2 Size = Vector2.one;

    public Material material;
    public float H = 1;
    public float Z = 0.1f;
    public bool CreatePolygonCollider2D = false;
    List<MultiDrawObject3D> scs = new List<MultiDrawObject3D>();


    public override void Draw()
    {
        GetObj();
        
        base.Draw();
    }

    void GetObj()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            if (Application.isEditor && !Application.isPlaying)
                DestroyImmediate(transform.GetChild(i).gameObject);
            else
                Destroy(transform.GetChild(i).gameObject);
        }

        scs.Clear();

        float h = 0.8f;
        int cX = (int)(Size.x / h) + 1;
        int cY = (int)(Size.y / h) + 1;
        Vector2 Size_H = new Vector2(Size.x / (cX / 2.0f), Size.y / (cY / 2.0f));
        float x_min = -Size.x + Size_H.x * 0.5f;
        float y_min = -Size.y + Size_H.y * 0.5f;
        for (int ix = 0; ix < cX; ix++)
        {
            for (int iy = 0; iy < cY; iy++)
            {
                GameObject a = new GameObject("Sand");
                a.transform.SetParent(transform, false);
                a.transform.localPosition = new Vector3(x_min + Size_H.x * ix, y_min + Size_H.y * iy, 0);
                scs.Add(a.AddComponent<MultiDrawObject3D>());
                scs.Last().material = material;
                scs.Last().H = H;
                scs.Last().Z = Z;
                scs.Last().CreatePolygonCollider2D = CreatePolygonCollider2D;
                scs.Last().Create(Size_H / 2, Vector2.one);

            }
        }
    }

    public bool AddContour(Vector2 pos1, Vector2 pos2, float R)
    {
        bool isEdit = false;
        foreach(MultiDrawObject3D s in scs)
        {
            if (s.AddContour(pos1, pos2, R))
                isEdit = true;
        }
        return isEdit;
    }

    public void AddContour(Vector2 pos1, Vector2 pos2, Vector2 pos3, Vector2 pos4)
    {
        foreach (MultiDrawObject3D s in scs)
        {
            s.AddContour(pos1, pos2, pos3, pos4);
        }
    }
}
