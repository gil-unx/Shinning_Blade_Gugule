using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sb;
using static System.Net.Mime.MediaTypeNames;
namespace Sb
{
    public class MSB
    {
        private Br reader;
        //hdr
        byte[] magic;
        int eofOffset;
        int ptrOffset;
        int ptrCount;
        byte[] head;
        byte[] tail; 
        byte[] newStringBuffer;
        public List<string> listText = new List<string>();
        List<int>offsets = new List<int>();
        public List<string> listNewText = new List<string>();
        Dictionary<string, int> ptSame = new Dictionary<string, int>();
        public MSB(string msbName)
        {
            reader = new Br(new FileStream(msbName, FileMode.Open, FileAccess.Read));
            magic = reader.ReadBytes(4);
            eofOffset = reader.ReadInt32()+0x20;
            reader.BaseStream.Seek(0x28, SeekOrigin.Begin);
            ptrOffset = reader.ReadInt32()+0x20;
            ptrCount = reader.ReadInt32();
            reader.BaseStream.Seek(ptrOffset, SeekOrigin.Begin);
            for (int i = 0; i < ptrCount; i++)
            {
                int offset = reader.ReadInt32()+ ptrOffset;
                offsets.Add(offset);
                string s = reader.GetJis(offset);
                listText.Add(s);
                
            }
            head = reader.GetBytes(0, ptrOffset);
            tail = reader.GetBytes(eofOffset, 0x10);



        }
        public void WriteTxt(string txtName)
        {

            StreamWriter writer = new StreamWriter(new FileStream(txtName, FileMode.Create, FileAccess.Write));
            int i = 0;
            writer.WriteLine(string.Format("[{0,0:d4}]\n*****************", listText.Count));
            foreach (string text in listText)
            {
                    
               
                writer.Write(string.Format("[{0,0:d8}]\n", i));
                

                writer.WriteLine(text);
                writer.WriteLine("--------------------------------");
                i++;
            }
            writer.Close();
        }
        public void WriteMsb(string txtName, string newMtpName)
        {
            LoadTXt(txtName);
            GenStringBuffer();
            WriteFile(newMtpName);




        }
        private void GenStringBuffer()
        {
            MemoryStream memoryStream = new MemoryStream();
            Bw writer = new Bw(memoryStream);
           
            for (int i = 0; i < ptrCount; i++)
            {

                byte[] binaryText;
     
                
                int offset;
                
                binaryText = Encoding.GetEncoding(932).GetBytes(listNewText[i]);
                if (ptSame.TryGetValue(listNewText[i], out offset))
                {
                }
                else
                {
                    offset = (int)writer.BaseStream.Position+ (ptrCount * 4);
                    ptSame.Add(listNewText[i], offset);
                    writer.Write(binaryText);
                    writer.Write((byte)0);

                }
                offsets[i] = offset;

                
               

               
                
               
            }
            writer.WritePadding(16, 0, 0);
            writer.Flush();
            writer.Close();
            newStringBuffer = memoryStream.ToArray();
            
        }
        private void LoadTXt(string name)
        {

            StreamReader reader = new StreamReader(new FileStream(name, FileMode.Open, FileAccess.Read));
            string line = "";
            reader.ReadLine();
            reader.ReadLine();
            for (int i = 0; i < ptrCount; i++)
            {
                string text = "";
                reader.ReadLine();
                while (true)
                {
                    line = reader.ReadLine();
                    if (line.StartsWith("----------"))
                    {
                        break;
                    }
                    text += line + "\n";
                }
               

                listNewText.Add(text.Substring(0,text.Length-1));
            }

        }
        private void WriteFile(string name)
        {
            Bw writer = new Bw(new FileStream(name, FileMode.Create, FileAccess.Write));
            //hdr
            writer.Write(head);
           
            foreach (int j in offsets)
            {
                writer.Write(j);
            }
           
            writer.Write(newStringBuffer);
            writer.WritePadding(16, 0, 0);
            writer.Write(tail);
            writer.BaseStream.Seek(4, SeekOrigin.Begin);
            writer.Write((int)writer.BaseStream.Length-0x30);
            writer.Flush();
            writer.Close();

        }

    }
}
