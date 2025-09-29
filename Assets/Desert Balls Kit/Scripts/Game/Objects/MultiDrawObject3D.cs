using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// create a shape for the sand
public class MultiDrawObject3D : BaseDrawObject3D
{
    class CrossPoint
    {
        public Vector3 NewPoint;
        public int idContour;
        public int j;
        public int j_prev;
        public int last_j;
        public int last_j_prev;
        public bool isIN;
        public bool isSelect;
    }

    public float Z = 0; // Z axis offset for drawing

    Vector2 Size = Vector2.zero;

    List<CrossPoint> _CrossPoint = new List<CrossPoint>(); // figure outlines
    List<int> _DelContours = new List<int>(); // 


    // create a shape
    public void Create(Vector2 _Size, Vector2 _SizeREAL)
    {
        Size = _Size;
        SizeREAL = _SizeREAL;

        points.Clear();
        points.Add(new List<Vector3>());

        points.Last().Add(Expantions.Round(new Vector3(-Size.x, Size.y, Z)));
        points.Last().Add(Expantions.Round(new Vector3(-Size.x, -Size.y, Z)));
        points.Last().Add(Expantions.Round(new Vector3(Size.x, -Size.y, Z)));
        points.Last().Add(Expantions.Round(new Vector3(Size.x, Size.y, Z)));

        Draw();
    }

    // cut a round hole in the figure
    public bool AddContour(Vector2 pos1, Vector2 pos2, float R)
    {
        bool isEdit = false;
        if (points.Count > 0)
        {
            Vector2 transform_pos = transform.position;
            Bounds bounds1 = new Bounds(pos1, new Vector3(R, R, 0) * 2);
            Bounds bounds2 = new Bounds(pos2, new Vector3(R, R, 0) * 2);
            Bounds boundsAll = new Bounds();
            boundsAll.SetMinMax(
                new Vector3(Mathf.Min(bounds1.min.x, bounds2.min.x), Mathf.Min(bounds1.min.y, bounds2.min.y), Z)
                , new Vector3(Mathf.Max(bounds1.max.x, bounds2.max.x), Mathf.Max(bounds1.max.y, bounds2.max.y), Z + H));

            if (meshRenderer.bounds.Intersects(boundsAll))
            {
                pos1 = pos1 - transform_pos;
                pos2 = pos2 - transform_pos;

                points.Add(new List<Vector3>());

                if (Expantions.Equals(pos1, pos2))
                {
                    for (int a = 0; a < 360; a += 18)
                    {
                        points.Last().Add(Expantions.Round((Vector3)pos1 + Quaternion.Euler(0, 0, a) * new Vector2(R, 0)) + new Vector3(0, 0, Z));
                    }
                }
                else
                {
                    int hA = 15;
                    Vector3 _norm = (pos2 - pos1).normalized;
                    int A = Mathf.RoundToInt(Vector3.SignedAngle(Vector3.right, _norm, Vector3.forward)) + 180;

                    Vector3 pos = pos1;
                    int start_a = 0;
                    int end_a = 0;
                    int _A = A - 90;

                    for (int i = 0; i < 2; i++)
                    {
                        start_a = _A;
                        end_a = _A + 180;
                        if (_A % hA != 0)
                        {
                            start_a = (start_a + hA) / hA * hA;
                            end_a = end_a / hA * hA;
                        }

                        _A = (_A < 0) ? 360 + _A : (_A > 360 ? _A - 360 : _A);
                        start_a = (start_a < 0) ? 360 + start_a : (start_a > 360 ? start_a - 360 : start_a);
                        end_a = (end_a < 0) ? 360 + end_a : (end_a > 360 ? end_a - 360 : end_a);

                        if (_A != start_a)
                        {
                            points.Last().Add(Expantions.Round(pos + Quaternion.Euler(0, 0, _A) * new Vector2(R, 0)) + new Vector3(0, 0, Z));
                        }

                        if (start_a < end_a)
                        {
                            for (int a = start_a; a <= end_a; a += hA)
                            {
                                points.Last().Add(Expantions.Round(pos + Quaternion.Euler(0, 0, a) * new Vector2(R, 0)) + new Vector3(0, 0, Z));
                            }
                        }
                        else
                        {
                            for (int a = start_a; a < 360; a += hA)
                            {
                                points.Last().Add(Expantions.Round(pos + Quaternion.Euler(0, 0, a) * new Vector2(R, 0)) + new Vector3(0, 0, Z));
                            }
                            for (int a = 0; a <= end_a; a += hA)
                            {
                                points.Last().Add(Expantions.Round(pos + Quaternion.Euler(0, 0, a) * new Vector2(R, 0)) + new Vector3(0, 0, Z));
                            }
                        }

                        pos = pos2;
                        _A = A + 90;
                    }
                }

                if (!Expantions.HasСlockwise(points.Last()))
                    points.Last().Reverse();

                isEdit = ReContours();
                Draw();
            }
        }
        return isEdit;
    }

    // cut a rectangular hole in the figure
    public void AddContour(Vector2 pos1, Vector2 pos2, Vector2 pos3, Vector2 pos4)
    {
        if (points.Count > 0)
        {
            Vector2 transform_pos = transform.position;
            Bounds boundsAll = new Bounds();
            boundsAll.SetMinMax(
                new Vector3(Mathf.Min(Mathf.Min(pos1.x, pos2.x), Mathf.Min(pos3.x, pos4.x)), Mathf.Min(Mathf.Min(pos1.y, pos2.y), Mathf.Min(pos3.y, pos4.y)), 0) + new Vector3(0, 0, Z)
                , new Vector3(Mathf.Max(Mathf.Max(pos1.x, pos2.x), Mathf.Max(pos3.x, pos4.x)), Mathf.Max(Mathf.Max(pos1.y, pos2.y), Mathf.Max(pos3.y, pos4.y)), 0) + new Vector3(0, 0, Z + H));

            if (meshRenderer.bounds.Intersects(boundsAll) && !Expantions.Equals(pos1, pos2) && !Expantions.Equals(pos3, pos4))
            {
                pos1 = pos1 - transform_pos;
                pos2 = pos2 - transform_pos;
                pos3 = pos3 - transform_pos;
                pos4 = pos4 - transform_pos;

                points.Add(new List<Vector3>());

                points.Last().Add((Vector3)pos1 + new Vector3(0, 0, Z));
                points.Last().Add((Vector3)pos2 + new Vector3(0, 0, Z));
                points.Last().Add((Vector3)pos3 + new Vector3(0, 0, Z));
                points.Last().Add((Vector3)pos4 + new Vector3(0, 0, Z));

                if (!Expantions.HasСlockwise(points.Last()))
                    points.Last().Reverse();

                ReContours();
                Draw();
            }
        }
    }

    // update contours after adding holes
    bool ReContours()
    {
        bool isEdit = false;

        _CrossPoint.Clear();
        int CountInside = 0;
        int IdContourInside = -1;
        int IdContourLast = points.Count - 1;

        int CountPrev_CrossPoint;
        int _j_prev;
        int _j_next;
        int j_prev;
        int j_next;
        bool HasIntersections;
        int intersections_num;
        bool prev_under;
        Vector3 pR0;
        Vector3 pR1;
        Vector3 pR3;
        Vector3 pB0;
        Vector3 pB1;
        Vector3 pB3;
        Vector3 crossPoint;
        Vector3 crossPoint_tmp;
        float A_pR0;
        float A_pR1;
        float A_pB0;
        float A_pB1;
        bool R0_IN;
        bool R0_LINE;
        bool R1_IN;
        bool R1_LINE;
        bool cur_under;
        Vector2 a;
        Vector2 b;
        float t;

        bool IsEr = false;


        // we find intersections with the cut contour
        for (int i = 0; i < IdContourLast; i++)
        {
            CountPrev_CrossPoint = _CrossPoint.Count;
            _j_prev = points[IdContourLast].Count - 1;
            HasIntersections = true;
            for (int _j = 0; _j < points[IdContourLast].Count; _j++)
            {
                _j_next = _j + 1 >= points[IdContourLast].Count ? 0 : _j + 1;

                j_prev = points[i].Count - 1;
                intersections_num = 0;
                prev_under = points[i][j_prev].y < points[IdContourLast][_j].y;

                for (int j = 0; j < points[i].Count; j++)
                {
                    j_next = j + 1 >= points[i].Count ? 0 : j + 1;

                    pR0 = (Vector2)points[i][j_prev];
                    pR1 = (Vector2)points[i][j];
                    pR3 = (Vector2)points[i][j_next];
                    pB0 = (Vector2)points[IdContourLast][_j_prev];
                    pB1 = (Vector2)points[IdContourLast][_j];
                    pB3 = (Vector2)points[IdContourLast][_j_next];

                    crossPoint = Expantions.getPointOfIntersection(pR1, pR0, pB1, pB0);
                    if (Expantions.Equals(pR0, crossPoint) || crossPoint.z != 0)
                    {
                        crossPoint_tmp = Expantions.getPointOfIntersection(pR1, pR0, pB1, pB3);
                        if (crossPoint_tmp.z == 0 && Expantions.Equals(pB1, crossPoint_tmp))
                            crossPoint = pB1;
                    }
                    if (Expantions.Equals(pR0, crossPoint) || crossPoint.z != 0)
                    {
                        crossPoint_tmp = Expantions.getPointOfIntersection(pR1, pR3, pB1, pB0);
                        if (crossPoint_tmp.z == 0 && Expantions.Equals(pR1, crossPoint_tmp))
                            crossPoint = pR1;
                    }
                    if (Expantions.Equals(pR0, crossPoint) || crossPoint.z != 0)
                    {
                        crossPoint_tmp = Expantions.getPointOfIntersection(pR1, pR3, pB1, pB3);
                        if (crossPoint_tmp.z == 0
                            && Expantions.Equals(pR1, crossPoint_tmp) && Expantions.Equals(pB1, crossPoint_tmp))
                            crossPoint = pR1;
                    }
                    if (crossPoint.z != -1 && !Expantions.Equals(pR0, crossPoint) && !Expantions.Equals(pB0, crossPoint))
                    {
                        crossPoint.z = 0;

                        if (Expantions.Equals(pR1, crossPoint))
                            pR1 = pR3;

                        if (Expantions.Equals(pB1, crossPoint))
                            pB1 = pB3;

                        if (!Expantions.Equals(pR0, pR1))
                        {
                            A_pR0 = Vector3.SignedAngle(Vector3.up, pR0 - crossPoint, Vector3.forward) + 180;
                            A_pR1 = Vector3.SignedAngle(Vector3.up, pR1 - crossPoint, Vector3.forward) + 180;
                            A_pB0 = Vector3.SignedAngle(Vector3.up, pB0 - crossPoint, Vector3.forward) + 180;
                            A_pB1 = Vector3.SignedAngle(Vector3.up, pB1 - crossPoint, Vector3.forward) + 180;

                            R0_IN = false;
                            R0_LINE = false;
                            R1_IN = false;
                            R1_LINE = false;


                            if (A_pB1 < A_pB0)
                            {
                                if ((A_pR0 >= A_pB0 && A_pR0 <= 360) || (A_pR0 >= 0 && A_pR0 <= A_pB1))
                                    R0_IN = true;
                                if ((A_pR1 >= A_pB0 && A_pR1 <= 360) || (A_pR1 >= 0 && A_pR1 <= A_pB1))
                                    R1_IN = true;
                            }
                            else
                            {
                                if (A_pR0 >= A_pB0 && A_pR0 <= A_pB1)
                                    R0_IN = true;
                                if (A_pR1 >= A_pB0 && A_pR1 <= A_pB1)
                                    R1_IN = true;
                            }

                            if (A_pR0 == A_pB1 || A_pR0 == A_pB0)
                                R0_LINE = true;
                            if (A_pR1 == A_pB1 || A_pR1 == A_pB0)
                                R1_LINE = true;


                            if (R0_LINE != R1_LINE
                                && !(R0_IN && R1_IN))
                            {
                                R0_IN = (R0_LINE && R0_IN == R1_IN) ? !R0_IN : R0_IN;
                                R1_IN = (R1_LINE && R0_IN == R1_IN) ? !R1_IN : R1_IN;
                                R0_LINE = false;
                                R1_LINE = false;
                            }

                            if (R0_IN != R1_IN && !R0_LINE && !R1_LINE)
                            {
                                _CrossPoint.Add(new CrossPoint()
                                {
                                    NewPoint = (Vector2)crossPoint,
                                    idContour = i,
                                    j = j,
                                    j_prev = -1,
                                    last_j = _j,
                                    last_j_prev = -1,
                                    isIN = R1_IN,
                                    isSelect = false
                                });
                            }
                        }
                    }


                    cur_under = points[i][j].y < points[IdContourLast][_j].y;

                    a = points[i][j_prev] - points[IdContourLast][_j];
                    b = points[i][j] - points[IdContourLast][_j];

                    t = (a.x * (b.y - a.y) - a.y * (b.x - a.x));
                    if (cur_under && !prev_under && t >= 0)
                        intersections_num += 1;
                    if (!cur_under && prev_under && t <= 0)
                        intersections_num += 1;

                    j_prev = j;
                    prev_under = cur_under;
                }

                _j_prev = _j;

                if (HasIntersections && (intersections_num & 1) == 0)
                    HasIntersections = false;
            }
            if (_CrossPoint.Count - CountPrev_CrossPoint == 1)
                _CrossPoint.RemoveAt(_CrossPoint.Count - 1);
            if (HasIntersections)
            {
                CountInside++;
                IdContourInside = i;

                if (CountPrev_CrossPoint < _CrossPoint.Count)
                    _CrossPoint.RemoveRange(CountPrev_CrossPoint, _CrossPoint.Count - CountPrev_CrossPoint);
            }
        }

        // Group the list by contour index from which new contours will be made
        _DelContours = _CrossPoint.GroupBy(f => f.idContour).Select(f => f.Key).ToList();
        _DelContours.Add(IdContourLast);

        // Find the contours that completely fell into the contour of the cut,
        // but they are not among those who cross the contour of the cut
        for (int i = 0; i < IdContourLast; i++)
        {
            HasIntersections = true;
            for (int j = 0; j < points[i].Count; j++)
            {
                _j_prev = points[IdContourLast].Count - 1;
                intersections_num = 0;
                prev_under = points[IdContourLast][_j_prev].y < points[i][j].y;
                for (int _j = 0; _j < points[IdContourLast].Count; _j++)
                {
                    cur_under = points[IdContourLast][_j].y < points[i][j].y;

                    a = points[IdContourLast][_j_prev] - points[i][j];
                    b = points[IdContourLast][_j] - points[i][j];

                    t = (a.x * (b.y - a.y) - a.y * (b.x - a.x));
                    if (cur_under && !prev_under && t >= 0)
                        intersections_num += 1;
                    if (!cur_under && prev_under && t <= 0)
                        intersections_num += 1;

                    _j_prev = _j;
                    prev_under = cur_under;
                }

                if ((intersections_num & 1) == 0)
                {
                    HasIntersections = false;
                    break;
                }
            }
            if (HasIntersections && _DelContours.Where(f => f == i).DefaultIfEmpty(-1).FirstOrDefault() == -1)
                _DelContours.Add(i);
        }

        // If the intersections were
        if (_CrossPoint.Count > 0)
        {
            points.Add(new List<Vector3>());
            int _Q = 0;
            int cpId0 = 0;
            CrossPoint Cp0 = _CrossPoint[cpId0];
            Vector3 point_prev = Vector3.zero;

            int idContour;
            float dist_pred;
            bool isFirst;
            Vector3 point;
            int cpIdNext;
            float dist;

            // make new contours according to intersections
            while (_Q < _CrossPoint.Count)
            {
                if (!Cp0.isSelect)
                {
                    _CrossPoint[cpId0].isSelect = true;
                    _Q++;

                    point_prev = Cp0.NewPoint;
                    points.Last().Add(Expantions.Round(Cp0.NewPoint) + new Vector3(0, 0, Z));

                    idContour = Cp0.isIN ? IdContourLast : Cp0.idContour;
                    dist_pred = float.MaxValue;
                    isFirst = true;
                    for (int i = Cp0.isIN ? Cp0.last_j : Cp0.j; i < points[idContour].Count; i++)
                    {
                        point = (Vector2)points[idContour][i];
                        cpIdNext = -1;
                        for (int p = 0; p < _CrossPoint.Count; p++)
                        {
                            if (p != cpId0
                                && Cp0.idContour == _CrossPoint[p].idContour
                                && Expantions.HasPointLies(point_prev, point, _CrossPoint[p].NewPoint)
                                )
                            {
                                dist = Vector3.Distance(point_prev, _CrossPoint[p].NewPoint);

                                if (dist < dist_pred)
                                {
                                    cpIdNext = p;
                                    dist_pred = dist;
                                }
                            }
                        }

                        if (cpIdNext == -1)
                        {
                            dist_pred = float.MaxValue;
                            point_prev = point;
                            points.Last().Add(point + new Vector3(0, 0, Z));

                            if (!isFirst & i >= (Cp0.isIN ? Cp0.last_j : Cp0.j))
                            {
                                //Debug.LogWarning("1 - Err = " + _CrossPoint.Count);
                                //foreach (CrossPoint cp in _CrossPoint)
                                //{
                                //    Debug.LogWarning("isIN = " + cp.isIN);
                                //}
                                break;
                            }

                            if (i == points[idContour].Count - 1)
                            {
                                i = -1;
                                isFirst = false;
                            }
                        }
                        else
                        {
                            cpId0 = cpIdNext;
                            Cp0 = _CrossPoint[cpId0];
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < _CrossPoint.Count; i++)
                    {
                        if (!_CrossPoint[i].isSelect)
                        {
                            cpId0 = i;
                            break;
                        }
                    }
                    Cp0 = _CrossPoint[cpId0];

                    if (points.Last().Count < 3)
                        points.Last().Clear();
                    else
                        points.Add(new List<Vector3>());
                }
            }
        }
        // Divide the contour with the inner contour into simpler contours
        else if (CountInside == 1)
        {
            int id_start_in = -1;
            int id_start_out = -1;
            int id_end_in = -1;
            int id_end_out = -1;

            do
            {
                id_start_in = -1;
                id_start_out = -1;
                id_end_in = -1;
                id_end_out = -1;

                for (int i = 0; i < points[IdContourInside].Count; i++)
                {
                    if (id_start_in == -1)
                    {
                        for (int j = 0; j < points[IdContourLast].Count; j++)
                        {
                            if (!HasCrossForContours(i, j, id_end_in, id_end_out, points[IdContourInside], points[IdContourLast]))
                            {
                                id_start_in = i;
                                id_start_out = j;
                                break;
                            }
                        }
                    }
                    else if (id_end_in == -1)
                    {
                        for (int j = 0; j < points[IdContourLast].Count; j++)
                        {
                            if (id_start_out != j
                                && !HasCrossForContours(id_start_in, id_start_out, i, j, points[IdContourInside], points[IdContourLast]))
                            {
                                id_end_in = i;
                                id_end_out = j;
                                break;
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            } while (id_start_in == -1 || id_end_in == -1);


            points.Add(new List<Vector3>());
            points.Add(new List<Vector3>());


            if (id_end_out < id_start_out)
                points[points.Count - 2].AddRange(points[IdContourLast].GetRange(id_end_out, id_start_out - id_end_out + 1));
            else
            {
                points[points.Count - 2].AddRange(points[IdContourLast].GetRange(id_end_out, points[IdContourLast].Count - id_end_out));
                points[points.Count - 2].AddRange(points[IdContourLast].GetRange(0, id_start_out + 1));
            }
            points[points.Count - 2].AddRange(points[IdContourInside].GetRange(id_start_in, id_end_in - id_start_in + 1));


            if (id_end_out < id_start_out)
            {
                points[points.Count - 1].AddRange(points[IdContourLast].GetRange(id_start_out, points[IdContourLast].Count - id_start_out));
                points[points.Count - 1].AddRange(points[IdContourLast].GetRange(0, id_end_out + 1));
            }
            else
                points[points.Count - 1].AddRange(points[IdContourLast].GetRange(id_start_out, id_end_out - id_start_out + 1));
            points[points.Count - 1].AddRange(points[IdContourInside].GetRange(id_end_in, points[IdContourInside].Count - id_end_in));
            points[points.Count - 1].AddRange(points[IdContourInside].GetRange(0, id_start_in + 1));


            if (_DelContours.Where(f => f == IdContourInside).DefaultIfEmpty(-1).FirstOrDefault() == -1)
                _DelContours.Add(IdContourInside);
        }

        // We check new contours for self-intersections and divide these.
        // Check the contour so that it does not have the same points in a row
        // and the point should not lie on the line between the previous point and the next
        // (remove such extra points)
        if (points.Count > IdContourLast + 1)
        {
            isEdit = true;

            for (int i = IdContourLast + 1; i < points.Count; i++)
            {
                for (int j = 0; j < points[i].Count; j++)
                {
                    j_prev = j - 1 < 0 ? points[i].Count - 1 : j - 1;
                    j_next = j + 1 > points[i].Count - 1 ? 0 : j + 1;

                    // same points in a row
                    if (Expantions.Equals(points[i][j], points[i][j_next]))
                    {
                        points[i].RemoveAt(j);
                        j--;
                    }
                    // the point lies on the line between the previous point and the next
                    else if (Expantions.HasPointLies(points[i][j_prev], points[i][j_next], points[i][j]))
                    {
                        points[i].RemoveAt(j);
                        j--;
                    }
                    else
                    {
                        for (int _j = j + 1; _j < points[i].Count; _j++)
                        {
                            _j_next = _j + 1 > points[i].Count - 1 ? 0 : _j + 1;
                            crossPoint = Expantions.getPointOfIntersection(points[i][j], points[i][j_next]
                                , points[i][_j], points[i][_j_next]);
                            // If the same points
                            if (Expantions.Equals(points[i][j], points[i][_j]))
                            {
                                if (_j - j >= 3)
                                {
                                    points.Add(new List<Vector3>());
                                    points.Last().AddRange(points[i].GetRange(j + 1, _j - j));
                                }
                                points[i].RemoveRange(j + 1, _j - j);
                                _j = j;
                            }
                            // If lines intersect
                            else if (crossPoint.z != -1
                                && !Expantions.Equals(points[i][j_next], crossPoint)
                                && !Expantions.Equals(points[i][_j_next], crossPoint))
                            {
                                crossPoint.z = 0;

                                points.Add(new List<Vector3>());
                                points.Last().AddRange(points[i].GetRange(j + 1, _j - j));

                                if (!Expantions.Equals(points[i][j], crossPoint)
                                    && !Expantions.Equals(points[i][_j], crossPoint))
                                {
                                    points[i].RemoveRange(j + 2, _j - (j + 1));
                                    points[i][j + 1] = crossPoint + new Vector3(0, 0, Z);
                                    points.Last().Add(crossPoint + new Vector3(0, 0, Z));
                                }
                                else if (Expantions.Equals(points[i][_j], crossPoint))
                                {
                                    points[i].RemoveRange(j + 2, _j - (j + 1));
                                    points[i][j + 1] = crossPoint + new Vector3(0, 0, Z);
                                }
                                else if (Expantions.Equals(points[i][j], crossPoint))
                                {
                                    points[i].RemoveRange(j + 1, _j - j);
                                    points.Last().Add(crossPoint + new Vector3(0, 0, Z));
                                }

                                _j = j;
                            }
                        }
                    }
                }
                if (points[i].Count < 3)
                {
                    points.RemoveAt(i);
                    i--;
                }
                // we catch an error when the entire old contour is deleted,
                // and only a new cutting contour remains 
                // (if there is this error, then we don’t draw anything with such a contour 
                // and delete the cutting contour and all subsequent new contours after it)
                else if (points[i].Count == points[IdContourLast].Count) 
                {
                    bool isEqAll = true;
                    for (int i0 = 0; i0 < points[i].Count; i0++)
                    {
                        bool isEq = false;
                        for (int j = 0; j < points[IdContourLast].Count; j++)
                        {
                            if (Expantions.Equals(points[i][i0], points[IdContourLast][j]))
                            {
                                isEq = true;
                                break;
                            }
                        }
                        if (!isEq)
                        {
                            isEqAll = false;
                            break;
                        }
                    }
                    if (isEqAll)
                        IsEr = true;
                }
            }
        }

        if (!IsEr)
        {
            // Delete unnecessary contours
            foreach (int id in _DelContours.OrderByDescending(f => f))
            {
                points.RemoveAt(id);
            }
        }
        else
        {
            for (int i = points.Count - 1; i >= IdContourLast; i--)
            {
                points.RemoveAt(i);
            }
        }
        

        return isEdit;
    }

    // Divide the contour with the inner contour into simpler contours
    bool HasCrossForContours(int id_start_in, int id_start_out, int id_end_in, int id_end_out
        , List<Vector3> childCou, List<Vector3> lastCou)
    {
        bool _b = false;

        if (id_end_in != -1 && id_end_out != -1)
        {
            if (Expantions.getPointOfIntersection(childCou[id_start_in], lastCou[id_start_out]
                , childCou[id_end_in], lastCou[id_end_out]).z == 0)
            {
                _b = true;
            }
        }

        if (!_b)
        {
            int id_in = (id_end_out == -1) ? id_start_in : id_end_in;
            int id_out = (id_end_out == -1) ? id_start_out : id_end_out;
            Vector2 p1_start = childCou[id_in];
            Vector2 p1_end = lastCou[id_out];
            int i0;

            for (int i = 0; i < childCou.Count; i++)
            {
                i0 = (i - 1 < 0) ? (childCou.Count - 1) : (i - 1);
                if (id_in != i0 && id_in != i)
                {
                    if (Expantions.getPointOfIntersection(p1_start, p1_end, childCou[i0], childCou[i]).z == 0)
                    {
                        _b = true;
                        break;
                    }
                }
            }

            if (!_b)
            {
                for (int i = 0; i < lastCou.Count; i++)
                {
                    i0 = (i - 1 < 0) ? (lastCou.Count - 1) : (i - 1);
                    if (id_out != i0 && id_out != i)
                    {
                        if (Expantions.getPointOfIntersection(p1_start, p1_end, lastCou[i0], lastCou[i]).z == 0)
                        {
                            _b = true;
                            break;
                        }
                    }
                }
            }
        }

        return _b;
    }


    // drawing a figure
    public override void Draw()
    {
        base.Draw();
    }
}
