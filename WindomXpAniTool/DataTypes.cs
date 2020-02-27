using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindomXpAniTool
{
    class Quaternion
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public Quaternion()
        {

        }

        public Quaternion(float _x, float _y, float _z, float _w)
        {
            x = _x;
            y = _y;
            z = _z;
            w = _w;
        }
    }

    class Vector3
    {
        public float x;
        public float y;
        public float z;

        public Vector3()
        {

        }

        public Vector3(float _x, float _y, float _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }
    }

    class Matrix4x4
    {
        public float m00;
        public float m01;
        public float m02;
        public float m03;
        public float m10;
        public float m11;
        public float m12;
        public float m13;
        public float m20;
        public float m21;
        public float m22;
        public float m23;
        public float m30;
        public float m31;
        public float m32;
        public float m33;

        public Quaternion GetRotation()
        {
            var qw = Math.Sqrt(1f + m00 + m11 + m22) / 2;
            var w = 4 * qw;
            var qx = (m21 - m12) / w;
            var qy = (m02 - m20) / w;
            var qz = (m10 - m01) / w;

            if (float.IsNaN((float)qw))
                qw = 0.0f;

            if (float.IsNaN((float)qx))
                qx = 0.0f;

            if (float.IsNaN((float)qy))
                qy = 0.0f;

            if (float.IsNaN((float)qz))
                qz = 0.0f;

            return new Quaternion((float)qx, (float)qy, (float)qz, (float)qw);
        }

        public Vector3 GetPosition()
        {
            float x = m03;
            float y = m13;
            float z = m23;

            if (float.IsNaN(x))
                x = 0.0f;

            if (float.IsNaN(y))
                y = 0.0f;

            if (float.IsNaN(z))
                z = 0.0f;

            return new Vector3(x, y, z);
        }

        public Vector3 GetScale()
        {
            var x = Math.Sqrt(m00 * m00 + m01 * m01 + m02 * m02);
            var y = Math.Sqrt(m10 * m10 + m11 * m11 + m12 * m12);
            var z = Math.Sqrt(m20 * m20 + m21 * m21 + m22 * m22);

            if (float.IsNaN((float)x))
                x = 0.0f;

            if (float.IsNaN((float)y))
                y = 0.0f;

            if (float.IsNaN((float)z))
                z = 0.0f;

            return new Vector3((float)x, (float)y, (float)z);
        }
    }
}
