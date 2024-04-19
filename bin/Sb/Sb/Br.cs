﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
namespace Sb
{

    public class Br : BinaryReader
    {
        public Br(Stream stream) : base(stream)
        {

        }
        public Int32 ReadInt32BE()
        {
            var data = base.ReadBytes(4);
            Array.Reverse(data);
            return BitConverter.ToInt32(data, 0);
        }
        public Int16 ReadInt16BE()
        {
            var data = base.ReadBytes(2);
            Array.Reverse(data);
            return BitConverter.ToInt16(data, 0);
        }
        public string GetUtf8(long offset)
        {
            long tmp = base.BaseStream.Position;
            base.BaseStream.Seek(offset, SeekOrigin.Begin);
            var bldr = new StringBuilder();
            int nc;
            while ((nc = base.Read()) > 0)
                bldr.Append((char)nc);
            base.BaseStream.Seek(tmp, SeekOrigin.Begin);
            return bldr.ToString();
        }
        public string GetUtf8()
        {
            var bldr = new StringBuilder();
            int nc;
            while ((nc = base.Read()) > 0)
                bldr.Append((char)nc);

            return bldr.ToString();
        }
        public string GetJis()
        {
            List<byte> bldr = new List<byte>();
            byte nc;
            while ((nc = base.ReadByte()) > 0)
                bldr.Add(nc);
            return Encoding.GetEncoding("shift-jis").GetString(bldr.ToArray());
        }
        public string GetJis(long offset)
        {
            long tmp = base.BaseStream.Position;
            base.BaseStream.Seek(offset, SeekOrigin.Begin);
            List<byte> bldr = new List<byte>();
            byte nc;
            while ((nc = base.ReadByte()) > 0)
                bldr.Add(nc);
            base.BaseStream.Seek(tmp, SeekOrigin.Begin);
            return Encoding.GetEncoding("shift-jis").GetString(bldr.ToArray());
        }
        public byte[] GetBinaryNullTerm()
        {
            List<byte> bldr = new List<byte>();
            byte nc;
            while ((nc = base.ReadByte()) > 0)
                bldr.Add(nc);
            return bldr.ToArray();
        }

        public string GetUtf16(long offset)
        {
            long tmp = base.BaseStream.Position;
            base.BaseStream.Seek(offset, SeekOrigin.Begin);
            var bldr = new StringBuilder();
            int nc;
            while ((nc = base.ReadInt16()) > 0)
                bldr.Append((char)nc);
            base.BaseStream.Seek(tmp, SeekOrigin.Begin);
            return bldr.ToString();
        }

        public void ReadPadding(int padding, long pos)
        {

            while ((base.BaseStream.Position - pos) % (long)padding != 0L)
            {
                base.BaseStream.ReadByte();
            }

        }
        public byte[] GetBytes(long offset, int size)

        {
            long tmp = base.BaseStream.Position;
            base.BaseStream.Seek(offset, SeekOrigin.Begin);
            byte[] v = base.ReadBytes(size);
            base.BaseStream.Seek(tmp, SeekOrigin.Begin);
            return v;
        }

    }
}