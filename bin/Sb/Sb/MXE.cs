using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Sb
{


    //hanya scanner
    public class MXE
    {
        private Br reader;
        //hdr
        byte[] magic;
        byte[] magic1;
        int eofOffset;
        int eofOffset1;
        int mxeOffset;
        int unk; 
        int unk1;
        int unk2;
        int unk3;
        int pfOffset1;
        int count;
        int f1Offset;
        int ptrOff;
        int ptr1Off;
        int ptr2Off;
        public List<string> listText = new List<string>();
        public List<int> listPos = new List<int>();
        public List<List<int>> offsets = new List<List<int>>();
        public List<string> listNewText = new List<string>();
        public int[] listPtr;
        public int stringBufferSize;
        int MXECSize;
        byte[] newStringBuffer;
        int newStringBufferSize;
        int diffSize;
        int ptRef;
        byte[] head;
        byte[] foot;
        long ptrStart;


        /// PAKAI INI BUAT BENERIN NGEBUG DI  GAME_INFO_TOWN
       
        bool FIXMXE = false;
        public MXE(string mxeName)
        {
            if (Path.GetFileName(mxeName) == "GAME_INFO_TOWN.MXE")
            { 
                FIXMXE = true;  
            }
            reader = new Br(new FileStream(mxeName, FileMode.Open, FileAccess.Read));
            magic = reader.ReadBytes(4);
            eofOffset = reader.ReadInt32();
            mxeOffset = reader.ReadInt32();
            unk = reader.ReadInt32();
            if ((unk & 0xFFFF0000) == 0x100C0000)
            {
                return;
            }
            f1Offset = reader.ReadInt32();
            reader.BaseStream.Seek(mxeOffset, SeekOrigin.Begin);
            magic1 = reader.ReadBytes(4);
            eofOffset1 = reader.ReadInt32();
            pfOffset1 = reader.ReadInt32();
            unk1 = reader.ReadInt32();
            unk2 =reader.ReadInt32();
            MXECSize = reader.ReadInt32();
            reader.BaseStream.Seek(mxeOffset+ pfOffset1, SeekOrigin.Begin);
            unk3 =reader.ReadInt32();
            ptrOff = reader.ReadInt32();
            reader.ReadInt32();
            ptr2Off = reader.ReadInt32();   
            reader.BaseStream.Seek(mxeOffset + ptrOff, SeekOrigin.Begin);
            reader.ReadInt32(); 
            count = reader.ReadInt32();
            ptr1Off = reader.ReadInt32();
            reader.BaseStream.Seek(mxeOffset + ptr1Off, SeekOrigin.Begin);
            int ptrSizes = 0;
            for (int i = 0; i < count; i++)
            {
               int idx =  reader.ReadInt32();
               int unk2 = reader.ReadInt32();
               int ptrSize = reader.ReadInt32();
               int pad = (ptrSize % 0x10);
               if (pad !=0) ptrSize += (0x10-pad);
               int unk3 = reader.ReadInt32();
               ptrSizes += ptrSize;


            }
           
            ptrStart = reader.BaseStream.Position;
            head = reader.GetBytes(0, (int)ptrStart);
            if (ptr2Off>0)
            {
                reader.BaseStream.Seek(ptr2Off+mxeOffset+12, SeekOrigin.Begin);
                int c = reader.ReadInt32();
                int s = reader.ReadInt32(); 
                
               
                reader.BaseStream.Seek(s + mxeOffset, SeekOrigin.Begin);
                for (int i = 0; i < c; i++)
                {
                    reader.BaseStream.Seek(4, SeekOrigin.Current);
                }
                reader.ReadPadding(0x10,0);
                ptrSizes = (int)reader.BaseStream.Position - (int)ptrStart;
               
                reader.BaseStream.Seek(ptrStart, SeekOrigin.Begin);
                
            }
            
            listPtr =new int[ptrSizes/4];
            for (int i = 0; i < ptrSizes / 4; i++)
            {
                listPtr[i] = reader.ReadInt32();

            }
            
            
            long stringspos = reader.BaseStream.Position;
            reader.BaseStream.Seek(stringspos, SeekOrigin.Begin);
            ptRef = (int)stringspos - mxeOffset;
            stringBufferSize = MXECSize+mxeOffset + pfOffset1 - (int)stringspos;
           // Console.WriteLine("---{0,0:X8}", ptRef);
            while (reader.BaseStream.Position< MXECSize+ mxeOffset+ pfOffset1)
            {
                int pos = (int)reader.BaseStream.Position-mxeOffset;
                var r = Enumerable.Range(0, listPtr.Length).Where(i => listPtr[i] == pos).ToList();
                offsets.Add(r);
                byte[] byteJis = reader.GetBinaryNullTerm();
                string str = Encoding.GetEncoding(932).GetString(byteJis);
                listText.Add(str);
                listPos.Add(pos);
                
            }
            foot = reader.ReadBytes((int)reader.BaseStream.Length - (int)reader.BaseStream.Position);
        }
        public void WriteTxt(string txtName)
        {
            if (listText.Count == 0)
            {
                return;
            }
            StreamWriter writer = new StreamWriter(new FileStream(txtName, FileMode.Create, FileAccess.Write));
            int i = 0;
            writer.WriteLine(string.Format("[{0,0:d4}]\n*****************", listText.Count));
            foreach (string text in listText)
            {
                
                writer.Write(string.Format("[{0,0:d8}]", i));
                writer.Write("\n");
                writer.WriteLine(text);
                writer.WriteLine("--------------------------------");
                i++;
            }
            writer.Close();
        }
        private void LoadTXt(string name)
        {
            
            StreamReader reader = new StreamReader(new FileStream(name, FileMode.Open, FileAccess.Read));
            string line = "";
            reader.ReadLine();
            reader.ReadLine();
            for (int i = 0; i < listText.Count; i++)
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
                    else
                    {
                        text += line + "\n";
                    }
                    
                }
                text = text.Substring(0, text.Length - 1);  
                listNewText.Add(text);
            }

        }
        public void WriteMxe(string txtName, string newMxeName)
        {
            LoadTXt(txtName);
            GenStringBuffer();
            WriteFile(newMxeName);




        }
        private void GenStringBuffer()
        {
            MemoryStream memoryStream = new MemoryStream();
            Bw writer = new Bw(memoryStream);
           
            for (int i = 0; i < listText.Count; i++)
            {
                int pos = (int)writer.BaseStream.Position+ptRef;
               
                foreach (var index in offsets[i])
                {
                        listPtr[index] = pos;
                }
                
                byte[] binaryText;
                string texts = listNewText[i].Replace("\\r", "\r").Replace("\\n", "\n");
                binaryText = Encoding.GetEncoding(932).GetBytes(texts);
                writer.Write(binaryText);
                writer.Write((byte)0);

            }
            writer.WritePadding(0x10, 0, 0);
            writer.Flush();
            newStringBuffer = memoryStream.ToArray();
            writer.Close();
            newStringBufferSize = newStringBuffer.Length;
            diffSize = newStringBufferSize - stringBufferSize;
        }
        private void WriteFile(string name)
        {
            Bw writer = new Bw(new FileStream(name, FileMode.Create, FileAccess.Write));
            //hdr
            writer.Write(head);

            writer.BaseStream.Seek(4, SeekOrigin.Begin);
            writer.Write(eofOffset + diffSize);
           
            writer.BaseStream.Seek(0x24, SeekOrigin.Begin);
            writer.Write(eofOffset1 + diffSize);

            writer.BaseStream.Seek(0x34, SeekOrigin.Begin);
            writer.Write(MXECSize+diffSize);
           
           
            writer.BaseStream.Seek(ptrStart, SeekOrigin.Begin);
            
            foreach (int o in listPtr)
            {
                writer.Write(o);
            }
           
            writer.Write(newStringBuffer);
            writer.Write(foot);
            //GAME_INFO_TOWN
            if (FIXMXE)
            {
                int[] POS = { 0x44B8, 0x4538, 0x46B8, 0x4798, 0x4818, 0x4838, 0x49F8, };
                int[] VAL = { 0x9CAC, 0x9D0a, 0x9Dd5, 0x9dda, 0x9dde, 0x9ddf, 0x9de8, };
                for (int j = 0; j < 7; j++)
                {
                    writer.BaseStream.Seek(POS[j], SeekOrigin.Begin);
                    writer.Write(VAL[j]);
                }
            }
            writer.Flush();
            writer.Close(); 
        }
    }
}
