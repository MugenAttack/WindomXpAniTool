using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace WindomXpAniTool
{
    struct hod2v1_Part
    {
        public string name;
        public int treeDepth;
        public int childCount;
        public Quaternion rotation;
        public Vector3 scale;
        public Vector3 position;
        public Quaternion unk1;
        public Quaternion unk2;
        public Quaternion unk3;
    }
    class hod2v1
    {
        public string filename;
        //public byte[] data;
        public List<hod2v1_Part> parts;
        public hod2v1(string name)
        {
            filename = name;
        }

        public bool loadFromBinary(ref BinaryReader br, ref hod2v0 structure)
        {
            
            //data = br.ReadBytes(11 + (partCount * 179));
            string signature = new string(br.ReadChars(3));
            int version = br.ReadInt32();
            if (signature == "HD2" && version == 1)
            {
                parts = new List<hod2v1_Part>();
                int partCount = br.ReadInt32();
                for (int i = 0; i < partCount; i++)
                {
                    hod2v1_Part nPart = new hod2v1_Part();
                    nPart.name = structure.parts[i].name;
                    nPart.treeDepth = br.ReadInt32();
                    nPart.childCount = br.ReadInt32();
                    nPart.rotation = new Quaternion();
                    nPart.rotation.x = br.ReadSingle();
                    nPart.rotation.y = br.ReadSingle();
                    nPart.rotation.z = br.ReadSingle();
                    nPart.rotation.w = br.ReadSingle();
                    nPart.scale = new Vector3();
                    nPart.scale.x = br.ReadSingle();
                    nPart.scale.y = br.ReadSingle();
                    nPart.scale.z = br.ReadSingle();
                    nPart.position = new Vector3();
                    nPart.position.x = br.ReadSingle();
                    nPart.position.y = br.ReadSingle();
                    nPart.position.z = br.ReadSingle();
                    nPart.unk1 = new Quaternion();
                    nPart.unk1.x = br.ReadSingle();
                    nPart.unk1.y = br.ReadSingle();
                    nPart.unk1.z = br.ReadSingle();
                    nPart.unk1.w = br.ReadSingle();
                    nPart.unk2 = new Quaternion();
                    nPart.unk2.x = br.ReadSingle();
                    nPart.unk2.y = br.ReadSingle();
                    nPart.unk2.z = br.ReadSingle();
                    nPart.unk2.w = br.ReadSingle();
                    nPart.unk3 = new Quaternion();
                    nPart.unk3.x = br.ReadSingle();
                    nPart.unk3.y = br.ReadSingle();
                    nPart.unk3.z = br.ReadSingle();
                    nPart.unk3.w = br.ReadSingle();
                    br.BaseStream.Seek(83, SeekOrigin.Current);
                    parts.Add(nPart);
                }
            }
            else
                return false;

            return true;
        }

        public void saveToBinary(ref BinaryWriter bw)
        {
            
            //bw.Write(data);
            bw.Write(ASCIIEncoding.ASCII.GetBytes("HD2"));
            bw.Write(1);
            bw.Write(parts.Count);
            for (int i = 0; i < parts.Count; i++)
            {
                bw.Write(parts[i].treeDepth);
                bw.Write(parts[i].childCount);
                bw.Write(parts[i].rotation.x);
                bw.Write(parts[i].rotation.y);
                bw.Write(parts[i].rotation.z);
                bw.Write(parts[i].rotation.w);
                bw.Write(parts[i].scale.x);
                bw.Write(parts[i].scale.y);
                bw.Write(parts[i].scale.z);
                bw.Write(parts[i].position.x);
                bw.Write(parts[i].position.y);
                bw.Write(parts[i].position.z);
                bw.Write(parts[i].unk1.x);
                bw.Write(parts[i].unk1.y);
                bw.Write(parts[i].unk1.z);
                bw.Write(parts[i].unk1.w);
                bw.Write(parts[i].unk2.x);
                bw.Write(parts[i].unk2.y);
                bw.Write(parts[i].unk2.z);
                bw.Write(parts[i].unk2.w);
                bw.Write(parts[i].unk3.x);
                bw.Write(parts[i].unk3.y);
                bw.Write(parts[i].unk3.z);
                bw.Write(parts[i].unk3.w);
                bw.BaseStream.Seek(83, SeekOrigin.Current);
            }
        }

        public void saveToFile(string folder, int type)
        {
            if (type == 0)
            {
                BinaryWriter bw = new BinaryWriter(File.Open(Path.Combine(folder, filename), FileMode.Create));
                saveToBinary(ref bw);
                bw.Close();
            }
            else if (type == 1)
                saveToXML(folder);
        }

        public void saveToXML(string folder)
        {
            XmlWriterSettings xws = new XmlWriterSettings();
            xws.Indent = true;
            using (XmlWriter xw = XmlWriter.Create(Path.Combine(folder, filename + ".xml"), xws))
            {
                xw.WriteStartDocument();
                xw.WriteStartElement("HOD");
                for (int i = 0; i < parts.Count; i++)
                {
                    xw.WriteStartElement("Part");
                    xw.WriteAttributeString("name", parts[i].name);
                    xw.WriteAttributeString("treeDepth", parts[i].treeDepth.ToString());
                    xw.WriteAttributeString("childCount", parts[i].childCount.ToString());

                    xw.WriteStartElement("Rotation");
                    xw.WriteAttributeString("x", parts[i].rotation.x.ToString());
                    xw.WriteAttributeString("y", parts[i].rotation.y.ToString());
                    xw.WriteAttributeString("z", parts[i].rotation.z.ToString());
                    xw.WriteAttributeString("w", parts[i].rotation.w.ToString());
                    xw.WriteEndElement();

                    xw.WriteStartElement("Scale");
                    xw.WriteAttributeString("x", parts[i].scale.x.ToString());
                    xw.WriteAttributeString("y", parts[i].scale.y.ToString());
                    xw.WriteAttributeString("z", parts[i].scale.z.ToString());
                    xw.WriteEndElement();

                    xw.WriteStartElement("Position");
                    xw.WriteAttributeString("x", parts[i].position.x.ToString());
                    xw.WriteAttributeString("y", parts[i].position.y.ToString());
                    xw.WriteAttributeString("z", parts[i].position.z.ToString());
                    xw.WriteEndElement();

                    xw.WriteStartElement("Unk1");
                    xw.WriteAttributeString("x", parts[i].unk1.x.ToString());
                    xw.WriteAttributeString("y", parts[i].unk1.y.ToString());
                    xw.WriteAttributeString("z", parts[i].unk1.z.ToString());
                    xw.WriteAttributeString("w", parts[i].unk1.w.ToString());
                    xw.WriteEndElement();

                    xw.WriteStartElement("Unk2");
                    xw.WriteAttributeString("x", parts[i].unk2.x.ToString());
                    xw.WriteAttributeString("y", parts[i].unk2.y.ToString());
                    xw.WriteAttributeString("z", parts[i].unk2.z.ToString());
                    xw.WriteAttributeString("w", parts[i].unk2.w.ToString());
                    xw.WriteEndElement();

                    xw.WriteStartElement("Unk3");
                    xw.WriteAttributeString("x", parts[i].unk3.x.ToString());
                    xw.WriteAttributeString("y", parts[i].unk3.y.ToString());
                    xw.WriteAttributeString("z", parts[i].unk3.z.ToString());
                    xw.WriteAttributeString("w", parts[i].unk3.w.ToString());
                    xw.WriteEndElement();

                    xw.WriteEndElement();
                }
                xw.WriteEndElement();
                xw.WriteEndDocument();
            }
        }

        public void loadFromFile(string filepath,ref hod2v0 structure)
        {
            FileInfo f = new FileInfo(filepath);
            helper.log(f.Extension);
            if (f.Extension == ".hod")
            {
                BinaryReader br = new BinaryReader(File.Open(filepath, FileMode.Open, FileAccess.Read));
                loadFromBinary(ref br, ref structure);
                br.Close();
            }
            else if (f.Extension == ".xml")
            {
                loadFromXML(filepath);
            }
        }

        public void loadFromXML(string filepath)
        {
            filename = filename.Replace(".xml", "");
            XmlDocument doc = new XmlDocument();
            doc.Load(filepath);
            XmlNode mainNode = doc.SelectSingleNode("HOD");
            XmlNodeList partsList = mainNode.ChildNodes;
            parts = new List<hod2v1_Part>();
            foreach (XmlNode part in partsList)
            {
                hod2v1_Part nPart = new hod2v1_Part();
                float pf;
                int pi;
                nPart.name = part.Attributes["name"].Value;
                if (int.TryParse(part.Attributes["treeDepth"].Value, out pi))
                    nPart.treeDepth = pi;

                if (int.TryParse(part.Attributes["childCount"].Value, out pi))
                    nPart.childCount = pi;

                XmlNode rotation = part.SelectSingleNode("Rotation");
                nPart.rotation = new Quaternion();
                if (float.TryParse(rotation.Attributes["x"].Value, out pf))
                    nPart.rotation.x = pf;
                if (float.TryParse(rotation.Attributes["y"].Value, out pf))
                    nPart.rotation.y = pf;
                if (float.TryParse(rotation.Attributes["z"].Value, out pf))
                    nPart.rotation.z = pf;
                if (float.TryParse(rotation.Attributes["w"].Value, out pf))
                    nPart.rotation.w = pf;

                XmlNode scale = part.SelectSingleNode("Scale");
                nPart.scale = new Vector3();
                if (float.TryParse(scale.Attributes["x"].Value, out pf))
                    nPart.scale.x = pf;
                if (float.TryParse(scale.Attributes["y"].Value, out pf))
                    nPart.scale.y = pf;
                if (float.TryParse(scale.Attributes["z"].Value, out pf))
                    nPart.scale.z = pf;

                XmlNode position = part.SelectSingleNode("Position");
                nPart.position = new Vector3();
                if (float.TryParse(position.Attributes["x"].Value, out pf))
                    nPart.position.x = pf;
                if (float.TryParse(position.Attributes["y"].Value, out pf))
                    nPart.position.y = pf;
                if (float.TryParse(position.Attributes["z"].Value, out pf))
                    nPart.position.z = pf;

                XmlNode unk1 = part.SelectSingleNode("Unk1");
                nPart.unk1 = new Quaternion();
                if (float.TryParse(unk1.Attributes["x"].Value, out pf))
                    nPart.unk1.x = pf;
                if (float.TryParse(unk1.Attributes["y"].Value, out pf))
                    nPart.unk1.y = pf;
                if (float.TryParse(unk1.Attributes["z"].Value, out pf))
                    nPart.unk1.z = pf;
                if (float.TryParse(unk1.Attributes["w"].Value, out pf))
                    nPart.unk1.w = pf;

                XmlNode unk2 = part.SelectSingleNode("Unk2");
                nPart.unk2 = new Quaternion();
                if (float.TryParse(unk2.Attributes["x"].Value, out pf))
                    nPart.unk2.x = pf;
                if (float.TryParse(unk2.Attributes["y"].Value, out pf))
                    nPart.unk2.y = pf;
                if (float.TryParse(unk2.Attributes["z"].Value, out pf))
                    nPart.unk2.z = pf;
                if (float.TryParse(unk2.Attributes["w"].Value, out pf))
                    nPart.unk2.w = pf;

                XmlNode unk3 = part.SelectSingleNode("Unk3");
                nPart.unk3 = new Quaternion();
                if (float.TryParse(unk3.Attributes["x"].Value, out pf))
                    nPart.unk3.x = pf;
                if (float.TryParse(unk3.Attributes["y"].Value, out pf))
                    nPart.unk3.y = pf;
                if (float.TryParse(unk3.Attributes["z"].Value, out pf))
                    nPart.unk3.z = pf;
                if (float.TryParse(unk3.Attributes["w"].Value, out pf))
                    nPart.unk3.w = pf;

                parts.Add(nPart);
            }
        }
    }
}
