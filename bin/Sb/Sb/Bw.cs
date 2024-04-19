using System.IO;
namespace Sb
{
    class Bw : BinaryWriter
    {
        public Bw(Stream stream) : base(stream)
        {

        }
        public void WritePadding(int padding, long pos)
        {
            while ((base.BaseStream.Position - pos) % (long)padding != 0L)
            {
                base.Write((byte)0);
            }
        }
        public void WritePadding(int padding, long pos,sbyte val)
        {
            while ((base.BaseStream.Position - pos) % (long)padding != 0L)
            {
                base.Write((sbyte)val);
            }
        }
        public void WriteInt32BE(uint val)
        {
            base.Write((byte)(val / 16777216 & (int)byte.MaxValue));
            base.Write((byte)(val / 65536 & (int)byte.MaxValue));
            base.Write((byte)(val / 256 & (int)byte.MaxValue));
            base.Write((byte)(val & (int)byte.MaxValue));

        }

    }
}