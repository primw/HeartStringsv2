using System;
using System.Collections.Generic;

namespace UnityEngine.U2D
{
    [Serializable]
    public class Spline
    {
        private static readonly string KErrorMessage = "Internal error: Point too close to neighbor";
        private static readonly float KEpsilon = 0.01f;
        [SerializeField]
        private bool m_IsOpenEnded;
        [SerializeField]
        private List<ShapeControlPoint> m_ControlPoints = new List<ShapeControlPoint>();

        public bool isOpenEnded
        {
            get
            {
                if (GetPointCount() < 3)
                    return true;

                return m_IsOpenEnded;
            }
            set { m_IsOpenEnded = value; }
        }

        private bool IsPositionValid(int index, int next, Vector3 point)
        {
            int prev = (index == 0) ? (m_ControlPoints.Count - 1) : (index - 1);
            next = (next >= m_ControlPoints.Count) ? 0 : next;
            if (prev >= 0)
            {
                Vector3 diff = m_ControlPoints[prev].position - point;
                if (diff.magnitude < KEpsilon)
                    return false;
            }
            if (next < m_ControlPoints.Count)
            {
                Vector3 diff = m_ControlPoints[next].position - point;
                if (diff.magnitude < KEpsilon)
                    return false;
            }
            return true;
        }

        public void Clear()
        {
            m_ControlPoints.Clear();
        }

        public int GetPointCount()
        {
            return m_ControlPoints.Count;
        }

        public void InsertPointAt(int index, Vector3 point)
        {
            if (!IsPositionValid(index, index, point))
                throw new ArgumentException(KErrorMessage);
            m_ControlPoints.Insert(index, new ShapeControlPoint { position = point, height = 1.0f, corner = true });
        }

        public void RemovePointAt(int index)
        {
            if (m_ControlPoints.Count > 2)
                m_ControlPoints.RemoveAt(index);
        }

        public Vector3 GetPosition(int index)
        {
            return m_ControlPoints[index].position;
        }

        public void SetPosition(int index, Vector3 point)
        {
            if (!IsPositionValid(index, index + 1, point))
                throw new ArgumentException(KErrorMessage);
            ShapeControlPoint newPoint = m_ControlPoints[index];
            newPoint.position = point;
            m_ControlPoints[index] = newPoint;
        }

        public Vector3 GetLeftTangent(int index)
        {
            ShapeTangentMode mode = GetTangentMode(index);

            if (mode == ShapeTangentMode.Linear)
                return Vector3.zero;

            return m_ControlPoints[index].leftTangent;
        }

        public void SetLeftTangent(int index, Vector3 tangent)
        {
            ShapeTangentMode mode = GetTangentMode(index);

            if (mode == ShapeTangentMode.Linear)
                return;

            ShapeControlPoint newPoint = m_ControlPoints[index];
            newPoint.leftTangent = tangent;
            m_ControlPoints[index] = newPoint;
        }

        public Vector3 GetRightTangent(int index)
        {
            ShapeTangentMode mode = GetTangentMode(index);

            if (mode == ShapeTangentMode.Linear)
                return Vector3.zero;

            return m_ControlPoints[index].rightTangent;
        }

        public void SetRightTangent(int index, Vector3 tangent)
        {
            ShapeTangentMode mode = GetTangentMode(index);

            if (mode == ShapeTangentMode.Linear)
                return;

            ShapeControlPoint newPoint = m_ControlPoints[index];
            newPoint.rightTangent = tangent;
            m_ControlPoints[index] = newPoint;
        }

        public ShapeTangentMode GetTangentMode(int index)
        {
            return m_ControlPoints[index].mode;
        }

        public void SetTangentMode(int index, ShapeTangentMode mode)
        {
            ShapeControlPoint newPoint = m_ControlPoints[index];
            newPoint.mode = mode;
            m_ControlPoints[index] = newPoint;
        }

        public float GetHeight(int index)
        {
            return m_ControlPoints[index].height;
        }

        public void SetHeight(int index, float value)
        {
            m_ControlPoints[index].height = value;
        }

        public int GetSpriteIndex(int index)
        {
            return m_ControlPoints[index].spriteIndex;
        }

        public void SetSpriteIndex(int index, int value)
        {
            m_ControlPoints[index].spriteIndex = value;
        }

        public bool GetCorner(int index)
        {
            return m_ControlPoints[index].corner;
        }

        public void SetCorner(int index, bool value)
        {
            m_ControlPoints[index].corner = value;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (int)2166136261;

                for (int i = 0; i < GetPointCount(); ++i)
                {
                    hashCode = hashCode * 16777619 ^ m_ControlPoints[i].GetHashCode();
                }

                hashCode = hashCode * 16777619 ^ m_IsOpenEnded.GetHashCode();

                return hashCode;
            }
        }
    }
}
