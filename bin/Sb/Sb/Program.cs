using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Sb
{
    internal class Program
    {
        static void Main(string[] args)
        {
           
            if (args.Length == 0)
            {
                ShowOptions();
            }
            else
            {
                switch (args[0])
                {
                    case "-msb2txt":
                        MsbTxt(args);
                        break;

                    case "-mxe2txt":
                        Mxe2Txt(args);
                        break;
                    case "-mtp2txt":
                        Mtp2Txt(args);
                        break;

                    case "-txt2msb":
                        Txt2Msb(args);
                        break;

                    case "-txt2mxe":
                        Txt2Mxe(args);
                        break;
                   
                    case "-txt2mtp":
                        Txt2Mtp(args);
                        break;


                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }

           
        }

        public static void ShowOptions()
        {
            Console.WriteLine("Sb");
            Console.WriteLine("decode mxe  : Sb.exe -mxe2txt *.mxe *.txt");
            Console.WriteLine("decode mtp  : Sb.exe -mxe2txt *.mtp *.txt");
            Console.WriteLine("encode mxe  : Sb.exe -txt2mxe *.mxe *.txt [new mxe]");
            Console.WriteLine("encode mtp  : Sb.exe -txt2mtp *.mtp *.txt [new mtp]");
                              

        }
        public static void MsbTxt(string[] args)
        {
            Console.WriteLine("decode : {0} to {1} ", args[1], args[2]);
            MSB m = new MSB(args[1]);
            m.WriteTxt(args[2]);



        }

        public static void Txt2Msb(string[] args)
        {
            Console.WriteLine("encode : {0} to {1} ", args[2], args[3]);


            MSB m = new MSB(args[1]);
            m.WriteMsb(args[2], args[3]);

        }
        public static void Mxe2Txt(string[] args)
        {
            Console.WriteLine(  "decode : {0} to {1} ", args[1], args[2]);
           
                MXE m = new MXE(args[1]);
                m.WriteTxt(args[2]);
         

        }

        public static void Txt2Mxe(string[] args)
        {
            Console.WriteLine("encode : {0} to {1} ", args[2], args[3]);
            
                MXE m = new MXE(args[1]);
                m.WriteMxe(args[2], args[3]);
            


        }
        public static void Mtp2Txt(string[] args)
        {
            Console.WriteLine("decode : {0} to {1} ", args[1], args[2]);
            MTP m = new MTP(args[1]);
            m.WriteTxt(args[2]);
            


        }

        public static void Txt2Mtp(string[] args)
        {
            Console.WriteLine("encode : {0} to {1} ", args[2], args[3]);


            MTP m = new MTP(args[1]);
            m.WriteMtp(args[2], args[3]);

        }
    }   

}
