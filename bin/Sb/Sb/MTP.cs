using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sb
{
    class Info
    {
        public int Unk { get; set; }
        public int SpeakerIndex { get; set; }
        public int Offset { get; set; }

        public int Offset1 { get; set; }
        public List<int> OffsetList { get; set; }
        

        public bool DummySize { get; set; }

    }
    public class MTP
    {
        private Br reader;
        //hdr
        byte[] magic;
        int eofOffset;
        int pfOffset;
        int unk1;
        int f1Offset;
        int enrOffset;
        //f1 hdr
        int unk2;
        public int count1;
        int ptrSize;
        public int count2;
        byte[] unk3;
        List<int> ptrOffsets = new List<int>();
        List<Info> infos =new List<Info>();
        byte[] stringBuffer;
        byte[] enr;
        byte[] eof;
        bool isSpeaker = false;
        //
        byte[] newStringBuffer;
        int stringBufferSize;
        int newStringBufferSize;
        int diffSize;
        Dictionary<int, string> ds = new Dictionary<int, string>()
        {
            { -1,"none"},
            {0,"none" },
{0x00000001,"レイジ,レイジ,レイジ"},
{0x00000002,"サクヤ,マキシマ,サクヤ"},
{0x000000DD,"ゴブリン,,ゴブリン"},
{0x00000192,"サクヤ,マキシマ,サクヤ"},
{0x00000010,"フェンリル,,フェンリル"},
{0x00000006,"エルミナ,ローデリア,エルミナ"},
{0x00000003,"リック,エルウッド,リック"},
{0x0000000B,"リン,シャオメイ,リンリン"},
{0x00000004,"ローゼリンデ,フレイア,ローゼリンデ"},
{0x00000005,"アルティナ,メル・シルフィス,アルティナ"},
{0x00000008,"エルウィン,ラナ・シルフィス,ラナ"},
{0x000001F5,"ユキヒメ,ユキヒメ,ユキヒメ"},
{0x000001FB,"エアリィ,マナフレア,エアリィ"},
{0x000001FC,"アミル,アーデット,アミル"},
{0x00000259,"クラントール兵,クラントール兵,クラントール兵"},
{0x000000E1,"イノブタ,,イノブタ"},
{0x000000E7,"イエティ,,ランプスマッシャー"},
{0x00000196,"エルミナ,エルミナ,エルミナ"},
{0x00000198,"ラナ,ラナ,ラナ"},
{0x00000193,"リック,エルウッド,リック"},
{0x0000020D,"村人,村人,ウェーバー"},
{0x0000020E,"村人,村人,ロアン"},
{0x0000020F,"村人,村人,ジュリア"},
{0x00000210,"村人,村人,ダリオ"},
{0x00000211,"村人,村人,アンナ"},
{0x00000212,"村人,村人,グレゴリ"},
{0x000000D3,"キノコお化け,,シュリーカー"},
{0x000000D5,"スコーピオン,,スコーピオン"},
{0x000000D7,"ビー,,スティンガー"},
{0x000000D8,"ビーロック,,ブラストビー"},
{0x000000DB,"ダークスカル,,ダークスカル"},
{0x000000E5,"悪魔の使い,,トーチ"},
{0x000000E9,"ウルフ,,ウルフ"},
{0x000000FD,"ワンアイ,,オートマタ"},
{0x0000006A,"エールブラン,エールブラン,エールブラン"},
{0x000000EF,"タートル,タートル,シェルタートル"},
{0x0000000E,"ゼクティ,アイン,アイン"},
{0x0000000F,"クララクラン,クララクラン,クララクラン"},
{0x00000007,"ミストラル,ネレイス,ミスティ"},
{0x00000009,"アイラ,ブランネージュ・ガルディニアス,アイラ"},
{0x0000000A,"龍那,,龍那"},
{0x0000000C,"カノン,シーナ,カノン"},
{0x0000000D,"トウカ,クレハ,トウカ"},
{0x00000011,"ガデム,,ガデム"},
{0x00000012,"ディラン,ローエン,ディラン"},
{0x00000013,"剛龍鬼,,剛龍鬼"},
{0x00000014,"刃九郎,,刃九郎"},
{0x00000015,"イサリ,ジェラード,イサリ"},
{0x00000016,"ケルベロス,クイーン EX-Xｅｃｔｙ,ケルベロス"},
{0x00000191,"レイジ,シーナ,レイジ"},
{0x00000194,"ローゼリンデ,フレイア,ローゼリンデ"},
{0x00000195,"アルティナ,,アルティナ"},
{0x00000197,"ミストラル,ネレイス,ミスティ"},
{0x00000199,"アイラ,,アイラ"},
{0x0000019A,"龍那,,龍那"},
{0x0000019B,"リン,シャオメイ,リンリン"},
{0x0000019C,"カノン,シーナ,カノン"},
{0x0000019D,"トウカ,クレハ,トウカ"},
{0x0000019E,"ゼクティ,アイン,アイン"},
{0x0000019F,"クララクラン,フィリアス,クララクラン"},
{0x000001A0,"フェンリル,,フェンリル"},
{0x000001A1,"ガデム,,ガデム"},
{0x000001A2,"ディラン,ローエン,ディラン"},
{0x000001A3,"剛龍鬼,,剛龍鬼"},
{0x000001A4,"刃九郎,,刃九郎"},
{0x000001A5,"イサリ,,イサリ"},
{0x000001A6,"ケルベロス,,ケルベロス"},
{0x000001F6,"サクヤ,,サクヤ"},
{0x000001F7,"サクヤ,,サクヤ"},
{0x000001F8,"サクヤ,,サクヤ"},
{0x000001F9,"サクヤ,,サクヤ"},
{0x000001FD,"ネリス,フィリアム,ネリス"},
{0x000001FE,"リン,シャオメイ,リンリン"},
{0x000001FF,"バルドル,,バルドル"},
{0x00000200,"アイザック,,伯爵"},
{0x0000020C,"ケフィア,,ケフィア"},
{0x00000213,"村人,村人,ジェゼフ"},
{0x0000025A,"サクヤ,,サクヤ"},
{0x0000025B,"ツンドラ,,ツンドラ"},
{0x0000025C,"バルカン,,バルカン"},
{0x0000025D,"ヌエ,,ヌエ"},
{0x0000025E,"トレンティア,,トレンティア"},
{0x0000025F,"セレナ,,セレナ"},
{0x00000260,"鋼鱗丸,,鋼鱗丸"},
{0x00000261,"ユキヒメ,,ユキヒメ"},
{0x00000262,"ユキヒメ,,ユキヒメ"},
{0x00000263,"ユキヒメ,,ユキヒメ"},
{0x00000264,"エアリィ,,エアリィ"},
{0x00000265,"アミル,,アミル"},
{0x00000266,"ネリス,,ネリス"},
{0x00000214,"村人,村人,クロード"},
{0x00000215,"村人,村人,フリント"},
{0x00000216,"村人,村人,カタリーナ"},
{0x00000217,"村人,村人,ジュリオン"},
{0x00000218,"村人,村人,セドリック"},
{0x00000219,"村人,村人,テルムッド"},
{0x0000021A,"村人,村人,レストック"},
{0x0000021B,"村人,村人,エヴァンス"},
{0x0000021C,"村人,村人,ファレイン"},
{0x0000021D,"村人,村人,メルフィス"},
{0x0000021E,"村人,村人,ルティーナ"},
{0x0000021F,"村人,村人,アルフェウス"},
{0x00000220,"村人,村人,シーン"},
{0x00000221,"村人,村人,ジュウゾウ"},
{0x00000222,"村人,村人,ヤスベイ"},
{0x00000223,"村人,村人,モップ"},
{0x00000224,"村人,村人,ライガ"},
{0x00000225,"村人,村人,アウラ"},
{0x00000226,"村人,村人,ダニエル"},
{0x00000227,"カイト,キリヤ,カイト"},
{0x00000065,"スレイプニル,,スレイプニル"},
{0x00000066,"アルベリッヒ,,アルベリッヒ"},
{0x00000067,"スルト,,スルト"},
{0x00000068,"ローゼリンデ,,ローゼリンデ"},
{0x00000069,"ファフナー,,ファフナー"},
{0x0000006B,"ベイルグラン,,ベイルグラン"},
{0x0000006C,"ブレイバーン,,ブレイバーン"},
{0x0000006D,"ファンロン,,ファンロン"},
{0x0000006E,"ダークドラゴン,,ダークドラゴン"},
{0x0000006F,"ダークドラゴン,,ダークドラゴン"},
{0x00000070,"ダークドラゴン,,ダークドラゴン"},
{0x00000071,"EXボス,,ＥＦ−０　トール"},
{0x00000072,"ガーデアン,,ナイトギア"},
{0x00000073,"ガーデアン改,,キングギア"},
{0x000000C9,"兵隊長,兵隊長,キャプテン"},
{0x000000CA,"分隊長,分隊長,コマンダー"},
{0x000000CB,"大隊長,大隊長,ジェネラル"},
{0x000000CC,"レッドクリスタル,レッドクリスタル,魔晶のルビー"},
{0x000000CD,"ブルークリスタル,ブルークリスタル,魔晶のサファイア"},
{0x000000CE,"グリーンクリスタル,グリーンクリスタル,魔晶のエメラルド"},
{0x000000CF,"クリスタルリング,クリスタルリング,ダーククリスタル"},
{0x000000D0,"グリーンペースト,グリーンペースト,グリーンペースト"},
{0x000000D1,"レッドペースト,レッドペースト,レッドペースト"},
{0x000000D2,"アイアンペースト,アイアンペースト,シルバーペースト"},
{0x000000D4,"覇王樹,覇王樹,ダリア"},
{0x000000D6,"サンドスコーピオン,,サンドスコーピオン"},
{0x000000D9,"スパイダー,スパイダー,スパイダー"},
{0x000000DA,"サンダースパイダー,サンダースパイダー,サンダースパイダー"},
{0x000000DC,"レッドスカル,レッドスカル,クリムゾンスカル"},
{0x000000DE,"ホブゴブリン,ホブゴブリン,ホブゴブリン"},
{0x000000DF,"ボーン,ボーン,ボーンファイター"},
{0x000000E0,"ボーンレンジャー,ボーンレンジャー,ボーンエリート"},
{0x000000E2,"マンモス,マンモス,スノーボア"},
{0x000000E3,"亡霊騎士,亡霊騎士,亡霊騎士"},
{0x000000E4,"暗黒騎士,暗黒騎士,暗黒騎士"},
{0x000000E6,"死の使い,死の使い,チャコル"},
{0x000000E8,"改造イエティ,改造イエティ,ビルドスマッシャー"},
{0x000000EA,"ホワイトウルフ,ホワイトウルフ,ブリザードウルフ"},
{0x000000EB,"リザード,リザード,リザード"},
{0x000000EC,"ブルーリザード,ブルーリザード,ハイリザード"},
{0x000000ED,"おもちゃの兵隊,おもちゃの兵隊,インターセプター"},
{0x000000EE,"おもちゃの兵隊長,おもちゃの兵隊長,デストロイヤー"},
{0x000000F0,"ファイヤータートル,ファイヤータートル,ヴォルカタートル"},
{0x000000F1,"幽霊ケンタウロス,幽霊ケンタウロス,ダークナイト"},
{0x000000F2,"ゴーレム,ゴーレム,ブリックゴーレム"},
{0x000000F3,"溶岩魔人,溶岩魔人,マグマゴーレム"},
{0x000000F4,"グリーンドラゴン,グリーンドラゴン,魔竜シュツルム"},
{0x000000F5,"レッドドラゴン,レッドドラゴン,魔竜サラマンドル"},
{0x000000F6,"ブルードラゴン,ブルードラゴン,魔竜グレンデル"},
{0x000000F7,"ドラゴニュート,ドラゴニュート,魔竜ダーイン"},
{0x000000F8,"ダークニュート,ダークニュート,魔竜ドヴァリン"},
{0x000000F9,"サイバードラゴン,サイバードラゴン,魔竜ドゥネイル"},
{0x000000FA,"ウィンドドラゴン,ウィンドドラゴン,魔竜ドゥラスロール"},
{0x000000FB,"ニーズヘッグ,ニーズヘッグ,魔竜ニーズヘッグ"},
{0x000000FC,"改造人間,改造人間,ガンドロイド"},
{0x000000FE,"ツーアイ,ツーアイ,オートビット"},
{0x000000FF,"侵入者迎撃マシーン,侵入者迎撃マシーン,クロックワーク"},
{0x00000100,"侵入者迎撃マシーン改,侵入者迎撃マシーン改,コッペリア"},
{0x00000101,"ブルーペースト,ブルーペースト,ブルーペースト"},
{0x00000102,"ボーンハンター,ボーンハンター,ボーンアーチャー"},
{0x00000103,"ゴブリン,,ゴブリンソード"},
{0x00000104,"ホブゴブリン,ホブゴブリン,ホブゴブリンメイス"},
{0x00000105,"ニーズヘッグ改,ニーズヘッグ改,魔竜ムスペル"},
{0x00000106,"イエロードラゴン,イエロードラゴン,魔竜アースグリム"},
{0x00000074,"EXボスオプション1,EXボスオプション1,メギンギョルズ"},
{0x00000075,"EXボスオプション2,EXボスオプション2,ミョルニル"},
{0x00000267,"ユキヒメ,,ユキヒメ"},
{0x00000076,"エールブラン亜種,エールブラン亜種,幻氷の虚竜"},
{0x00000077,"ベイルグラン亜種,,幻樹の虚竜"},
{0x00000078,"ブレイバーン亜種,,幻焔の虚竜"},
{0x00000079,"ファンロン亜種,,幻雷の虚竜"},
{0x00000228,"商人,商人,商人"},
{0x00000268,"兵士,,兵士"},
{0x00000269,"エルフの衛兵,,エルフの衛兵"},
{0x0000026A,"エルフ議員,,エルフ議員"},
{0x0000026B,"義勇兵,,義勇兵"},
{0x00000229,"レイジ(雪姫装備),,レイジ"},
{0x0000022A,"レイジ(雪姫ご機嫌装備),,レイジ"},
{0x0000022B,"リック(エアリィ装備),,リック"},
{0x0000022C,"リック(アミル装備),,リック"},
{0x0000022D,"リック(ネリス装備),,リック"},
{0x00000017,"ラストバトルレイジ,ラストバトルレイジ,レイジ"},
{0x0000022E,"レイジ(雪姫柄装備),,レイジ"},
{0x0000022F,"ダークドラゴン第三形態,,ダークドラゴン"},
{0x0000007A,"スレイプニル,,スレイプニル"},
{0x0000007B,"スレイプニル,,スレイプニル"},
{0x0000007C,"スレイプニル,,スレイプニル"},
{0x0000007D,"アルベリッヒ,,アルベリッヒ"},
{0x0000007E,"アルベリッヒ,,アルベリッヒ"},
{0x0000007F,"スルト,,スルト"},
{0x00000080,"スルト,,スルト"},
{0x00000081,"ローゼリンデ,,ローゼリンデ"},
{0x00000082,"ローゼリンデ,,ローゼリンデ"},
{0x00000083,"ファフナー,,ファフナー"},
{0x00000084,"ファフナー,,ファフナー"},
{0x00000107,"幽霊ケンタウロス,幽霊ケンタウロス,ダークナイト"},
{0x0000026C,"？？？,,？？？"},




        }
 ;
        public List<string> speakers = new List<string>();
        public List<string> listText = new List<string>();

        public List<string> listNewText = new List<string>();
        public MTP(string mtpName)
        {
            reader = new Br(new FileStream(mtpName,FileMode.Open,FileAccess.Read));
            magic = reader.ReadBytes(4);
            eofOffset = reader.ReadInt32();  
            pfOffset = reader.ReadInt32();
            unk1 = reader.ReadInt32();
            f1Offset = reader.ReadInt32();
            enrOffset = reader.ReadInt32();
            reader.BaseStream.Seek(pfOffset, SeekOrigin.Begin);
            unk2 = reader.ReadInt32();
            count1 = reader.ReadInt32();
            ptrSize = reader.ReadInt32();
            count2 = reader.ReadInt32();
            unk3 = reader.ReadBytes(ptrSize*4);
            for (int i = 0; i < count1; i++)
            {
                ptrOffsets.Add(reader.ReadInt32());
            }
            for (int i = 0; i < count1; i++)
            {
                Info info = new Info();
                switch (ptrSize)
                {
                    case 4:
                        isSpeaker = true;   
                        info.Unk = reader.ReadInt32();
                        info.SpeakerIndex = reader.ReadInt32();
                        info.Offset = reader.ReadInt32();
                        info.Offset1 = reader.ReadInt32();
                        try
                        {
                            speakers.Add(ds[info.SpeakerIndex]);
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                       
                        break;
                    case 2:
                        info.Unk = reader.ReadInt32();
                        info.Offset = reader.ReadInt32();
                        break;
                    case 6:
                        info.OffsetList = new List<int>();
                        info.Unk = reader.ReadInt32();
                        info.Offset = reader.ReadInt32();
                        info.OffsetList.Add(reader.ReadInt32());
                        info.OffsetList.Add(reader.ReadInt32());
                        info.OffsetList.Add(reader.ReadInt32());
                        info.OffsetList.Add(reader.ReadInt32());
                        break;
                    default:
                        
                        break;
                }
                infos.Add(info);
            }
            //read to memory
            stringBufferSize = enrOffset - (int)reader.BaseStream.Position + pfOffset;
            stringBuffer = reader.ReadBytes(stringBufferSize);
            enr = reader.ReadBytes(eofOffset - (int)reader.BaseStream.Position);
            eof = reader.ReadBytes(64);
            reader.Close();
            for (int i = 0; i < stringBuffer.Length; i++)
            {
                stringBuffer[i]--;
            }
            reader = new Br(new MemoryStream(stringBuffer));
            for (int i = 0; i < count1; i++)
            {
                Info info = infos[i];
                byte[] binaryString;
                int len;
                string text="";

                if(ptrSize == 6)
                {
                    reader.BaseStream.Seek(info.Offset, SeekOrigin.Begin);
                    len = reader.ReadInt32() & 0xff;
                    if (len == 0)
                    {
                        infos[i].DummySize = true;
                        binaryString = reader.GetBinaryNullTerm();
                    }
                    else
                    {
                        infos[i].DummySize = false;
                        binaryString = reader.ReadBytes(len);


                    }
                    text = Encoding.GetEncoding(932).GetString(binaryString )+ "[s]";
                    for (int j = 0; j < 4; j++)
                    {
                        reader.BaseStream.Seek(info.OffsetList[j], SeekOrigin.Begin);
                        len = reader.ReadInt32() & 0xff;
                        binaryString = reader.ReadBytes(len);

                        text += Encoding.Unicode.GetString(binaryString)+"[s]";
                    }

                }
                else
                {
                    reader.BaseStream.Seek(info.Offset, SeekOrigin.Begin);
                    len = reader.ReadInt32() & 0xff;
                    if (len == 0)
                    {
                        infos[i].DummySize = true;
                        binaryString = reader.GetBinaryNullTerm();
                    }
                    else
                    {
                        infos[i].DummySize = false;
                        binaryString = reader.ReadBytes(len);


                    }
                     text = Encoding.GetEncoding(932).GetString(binaryString);
                }
                listText.Add(text);

            }
            
           
        }
        public void WriteMtp(string txtName,string newMtpName)
        {
            LoadTXt(txtName);
            GenStringBuffer();
            WriteFile(newMtpName);
          
           
           

        }
        private void GenStringBuffer()
        {
            MemoryStream memoryStream = new MemoryStream();
            Bw writer = new Bw(memoryStream);
            int reffPadding = 16-(((ptrSize*4* count2) +(count2*4)+(ptrSize * 4))%16);
            for (int i = 0; i < count1; i++)
            {

                byte[] binaryText;
                int len =0;
                //Console.WriteLine(listNewText[i]);
                if (ptrSize == 6)
                {
					infos[i].Offset = (int)writer.BaseStream.Position;
                    string[] texts = listNewText[i].Replace("\\n","\n").Split(new string[] { "[s]"}, StringSplitOptions.None);
                    binaryText = Encoding.GetEncoding(932).GetBytes(texts[0]);
                    len = binaryText.Length;
                    writer.Write(len);
                    writer.Write(binaryText, 0, len);
                    writer.Write((byte)0);
                    writer.WritePadding(4, 0);
                    for (int j = 0; j < 4; j++)
                    {
                        infos[i].OffsetList[j]=(int)writer.BaseStream.Position;
                        binaryText = Encoding.Unicode.GetBytes(texts[j+1]);
                        len = binaryText.Length;
                        writer.Write(len);
                        writer.Write(binaryText, 0, len);
                        writer.Write((byte)0);
                        writer.WritePadding(4, 0);
                    }
                }
                else
                {
                    infos[i].Offset = (int)writer.BaseStream.Position;
                    binaryText = Encoding.GetEncoding(932).GetBytes(listNewText[i]);
                    len = binaryText.Length - 1;
                    if (infos[i].DummySize)
                    {
                        writer.Write((int)0);
                    }
                    else
                    {
                        writer.Write(len);
                    }

                    writer.Write(binaryText, 0, len);
                    writer.Write((byte)0);
                    writer.WritePadding(4, 0);
                }
            }
            writer.WritePadding(16, reffPadding,(sbyte)-1);
            writer.Flush();
            newStringBuffer = memoryStream.ToArray();
            writer.Close();
            newStringBufferSize = newStringBuffer.Length;
            diffSize = newStringBufferSize - stringBufferSize;
        }
        private void LoadTXt(string name)
        {

            StreamReader reader = new StreamReader(new FileStream(name, FileMode.Open, FileAccess.Read));
            string line = "";
            reader.ReadLine();
            reader.ReadLine();
            for (int i = 0; i < count1; i++)
            {
                string text = "";
                string indexS = reader.ReadLine();
                while (true)
                {
                    line = reader.ReadLine();
                    if (line.StartsWith("----------"))
                    {
                        break;
                    }
                    text += line+"\n";
                }
                //if ((!text.Contains("@"))&(!indexS.Contains("!")))
                //{
                //    
                //    text = Text.Wrap(text);
                //}
                //else
                //{
                //   // Console.WriteLine(  text);
                //}
                listNewText.Add(text);
            }

        }
        private void WriteFile(string name)
        {
            Bw writer = new Bw(new FileStream(name, FileMode.Create, FileAccess.Write));
            //hdr
            writer.Write(magic);
            writer.Write(eofOffset+diffSize);
            writer.Write(pfOffset);
            writer.Write(unk1);
            writer.Write(f1Offset);
            writer.Write(enrOffset+diffSize);
            writer.BaseStream.Seek(pfOffset, SeekOrigin.Begin);
            //f1 hdr
            writer.Write(unk2);
            writer.Write(count1);
            writer.Write(ptrSize);
            writer.Write(count2);
            writer.Write(unk3);
            foreach (int j in ptrOffsets)
            {
                writer.Write(j);
            }
            foreach (Info info in infos)
            {
                switch (ptrSize)
                {
                    case 4:
                        writer.Write(info.Unk);
                        writer.Write(info.SpeakerIndex);
                        writer.Write(info.Offset);
                        writer.Write(info.Offset1);
                        break;
                    case 2:
                        writer.Write(info.Unk);
                        writer.Write(info.Offset);
                        break;
                    case 6:

                        writer.Write(info.Unk);
                        writer.Write(info.Offset);
                        writer.Write(info.OffsetList[0]);
                        writer.Write(info.OffsetList[1]);
                        writer.Write(info.OffsetList[2]);
                        writer.Write(info.OffsetList[3]);
                        
                        break;
                    default:
                        break;
                }
            }
            foreach (byte k in newStringBuffer)
            {
                writer.Write((byte)(k + 1));
            }
            writer.Write(enr);
            writer.Write(eof);
            writer.Flush();
            writer.Close();

        }
        public void WriteTxt(string txtName)
        {
           
            StreamWriter writer = new StreamWriter(new FileStream(txtName, FileMode.Create, FileAccess.Write));
            int i = 0;
            writer.WriteLine(string.Format("[{0,0:d4}]\n*****************",listText.Count));
            foreach (string text in listText)
            {
                if (isSpeaker)
                {
                     writer.Write(string.Format("[{0,0:d8}][{1}]\n", i, speakers[i]));
                    //writer.Write(string.Format("{0}: ",speakers[i]));
                }
                else
                {
                    writer.Write(string.Format("[{0,0:d8}]\n", i));
                }
                
                writer.WriteLine(text);
                writer.WriteLine("--------------------------------");
                i++;
            }
            writer.Close();
        }
    }
}
