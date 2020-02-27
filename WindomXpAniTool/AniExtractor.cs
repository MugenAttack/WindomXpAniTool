using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace WindomXpAniTool
{
    class AniExtractor
    {
        string _filename;
        AniExtractor(string filename)
        {
            _filename = filename;
        }

        public bool ExtractData()
        {
            using (BinaryReader br = new BinaryReader(File.Open(_filename, FileMode.Open)))
            {
                FileInfo fi = new FileInfo(_filename);
                string folder = fi.DirectoryName;
                folder += "\\Animations";


                string signature = new string(br.ReadChars(3));
                if (signature == "AN2")
                {
                    
                    string robohod = new string(br.ReadChars(256));
                    robohod.Trim((char)0x00);
                    long hodPos = br.BaseStream.Position;
                    signature = new string(br.ReadChars(3));
                    int type = br.ReadInt32();
                    int partCount = 0;
                    if (signature == "HD2" && type == 0)
                    {
                        partCount = br.ReadInt32();
                        br.BaseStream.Seek(hodPos, SeekOrigin.Begin);
                        char[] basehod = br.ReadChars(11 + (partCount * 399));
                        using (BinaryWriter bw = new BinaryWriter(File.Open(Path.Combine(folder,"robohod"), FileMode.CreateNew)))
                        {
                            bw.Write(basehod);
                        }
                        
                    }
                    else
                        return false;

                    
                }
            }


            return true;
        }
    }
}
