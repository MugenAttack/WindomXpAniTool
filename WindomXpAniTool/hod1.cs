using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace WindomXpAniTool
{
    struct hod1_Part
    {
        public int treeDepth;
        public int childCount;
        public string name;
        public Matrix4x4 transform;
    }

    class hod1
    {
        string filename;
        public List<hod1_Part> parts;
        public hod1(string name)
        {
            filename = name;
        }
        public bool loadFromBinary(ref BinaryReader br)
        {
            string signature = new string(br.ReadChars(3));
            if (signature == "HOD")
            {
                parts = new List<hod1_Part>();
                int partCount = br.ReadInt32();
                for (int i = 0; i < partCount; i++)
                {
                    hod1_Part nPart = new hod1_Part();
                    nPart.treeDepth = br.ReadInt32();
                    nPart.childCount = br.ReadInt32();
                    nPart.name = ASCIIEncoding.ASCII.GetString(br.ReadBytes(256)).TrimEnd('\0');
                    nPart.transform = new Matrix4x4();
                    nPart.transform.m00 = br.ReadInt32();
                    nPart.transform.m01 = br.ReadInt32();
                    nPart.transform.m02 = br.ReadInt32();
                    nPart.transform.m03 = br.ReadInt32();
                    nPart.transform.m10 = br.ReadInt32();
                    nPart.transform.m11 = br.ReadInt32();
                    nPart.transform.m12 = br.ReadInt32();
                    nPart.transform.m13 = br.ReadInt32();
                    nPart.transform.m20 = br.ReadInt32();
                    nPart.transform.m21 = br.ReadInt32();
                    nPart.transform.m22 = br.ReadInt32();
                    nPart.transform.m23 = br.ReadInt32();
                    nPart.transform.m30 = br.ReadInt32();
                    nPart.transform.m31 = br.ReadInt32();
                    nPart.transform.m32 = br.ReadInt32();
                    nPart.transform.m33 = br.ReadInt32();
                    parts.Add(nPart);
                }
            }
            else
                return false;

            return true;
        }

        public hod2v0 convertToHod2v0()
        {
            hod2v0 hod = new hod2v0(filename);
            hod.parts = new List<hod2v0_Part>();
            for (int i = 0; i < parts.Count; i++)
            {
                hod2v0_Part nPart = new hod2v0_Part();
                nPart.treeDepth = parts[i].treeDepth;
                nPart.childCount = parts[i].childCount;
                nPart.name = parts[i].name;
                nPart.rotation = parts[i].transform.GetRotation();
                nPart.scale = parts[i].transform.GetScale();
                nPart.position = parts[i].transform.GetPosition();
                nPart.flag = 1;
                nPart.unk = new Vector3(1.0f, 1.0f, 1.0f);
                hod.parts.Add(nPart);  
            }
            return hod;
        }

        public hod2v1 convertToHod2v1()
        {
            hod2v1 hod = new hod2v1(filename);
            hod.parts = new List<hod2v1_Part>();
            for (int i = 0; i < parts.Count; i++)
            {
                hod2v1_Part nPart = new hod2v1_Part();
                nPart.treeDepth = parts[i].treeDepth;
                nPart.childCount = parts[i].childCount;
                nPart.name = parts[i].name;
                nPart.rotation = parts[i].transform.GetRotation();
                nPart.scale = parts[i].transform.GetScale();
                nPart.position = parts[i].transform.GetPosition();
                nPart.unk1 = parts[i].transform.GetRotation();
                nPart.unk2 = parts[i].transform.GetRotation();
                nPart.unk3 = parts[i].transform.GetRotation();
                hod.parts.Add(nPart);
            }
            return hod;
        }
    }
}
