using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WindomXpAniTool
{
    class ani2
    {
        public hod2v0 structure;
        public List<animation> animations;
        public string _filename;
        public bool load(string filename)
        {
            _filename = filename;
            BinaryReader br = new BinaryReader(File.Open(filename, FileMode.Open, FileAccess.Read));
            
            string signature = new string(br.ReadChars(3));
            if (signature == "AN2")
            {
                animations = new List<animation>();
                Encoding ShiftJis = Encoding.GetEncoding("shift_jis");
                string robohod = ShiftJis.GetString(br.ReadBytes(256)).TrimEnd('\0');
                structure = new hod2v0(robohod);
                structure.loadFromBinary(ref br);
                
                int aCount = br.ReadInt32();
                for (int i = 0; i < aCount; i++)
                {
                    animation aData = new animation();
                    aData.loadFromAni(ref br, ref structure);
                    animations.Add(aData);
                }
            }
            else if (signature == "ANI")
            {
                StreamWriter debug = new StreamWriter("debug.txt");
                animations = new List<animation>();
                Encoding ShiftJis = Encoding.GetEncoding("shift_jis");
                string robohod = ShiftJis.GetString(br.ReadBytes(256)).TrimEnd('\0');
                hod1 oldStructure = new hod1(robohod);
                oldStructure.loadFromBinary(ref br);
                structure = oldStructure.convertToHod2v0();
                debug.WriteLine(br.BaseStream.Position.ToString());
                debug.Close();
                for (int i = 0; i < 200; i++)
                { 
                    animation aData = new animation();
                    aData.loadFromAniOld(ref br);
                    animations.Add(aData);
                }
            }
            br.Close();

            return true;
        }

        public void save(string filename)
        {
            Encoding ShiftJis = Encoding.GetEncoding("shift_jis");
            BinaryWriter bw = new BinaryWriter(File.Open(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite));
            bw.Write(ASCIIEncoding.ASCII.GetBytes("AN2"));
            byte[] shiftjistext = ShiftJis.GetBytes(structure.filename);
            bw.Write(shiftjistext);
            bw.BaseStream.Seek(256 - shiftjistext.Length, SeekOrigin.Current);
            structure.saveToBinary(ref bw);

            bw.Write(animations.Count);
            for (int i = 0; i < animations.Count; i++)
            {
                animations[i].saveToAni(ref bw);
            }

            bw.Close();
        }
    }
}
