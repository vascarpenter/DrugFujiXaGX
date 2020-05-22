using System;

using AppKit;
using Foundation;
using ObjCRuntime;

namespace DrugFujiXaGX
{
    public partial class ViewController : NSViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Do any additional setup after loading the view.
        }

        public override NSObject RepresentedObject
        {
            get
            {
                return base.RepresentedObject;
            }
            set
            {
                base.RepresentedObject = value;
                // Update the view, if already loaded.
            }
        }

        // OS independent function

        /**
         * 文字列の全置換を行います.
         * @param str - ソース文字列
         * @param target - 検索文字列
         * @param replacement - 置換される文字列
         * @return 置換されたソース文字列
         */
        public string replaceAll(String str, String target, String replacement)
        {   // C# has Replace function
            return str.Replace(target, replacement);
        }

        /**
         * 文字列が左からeqstrと等しい場合にtrueを返します.その際に足りているかどうかの長さもチェックします。
         * @param t - ソース文字列
         * @param eqstr - 検索文字列
         * @return trueなら等しい
         */
        public Boolean isEqualToString(String t, String eqstr)
        {
            int len = eqstr.Length;
            return (t.Length > len && t.Substring(0, len).Equals(eqstr));
        }

        /**
         * フィールドを変換する このアプリケーションの主体.
         */
        public string convertString(string str)
        {
            String tx = str;
            String[,] drugs = new String[50, 15];
            int drugNumber = 0;
            int minorNumber = 0;
            StringTokenizer st = new StringTokenizer(tx, "\n");

            if (!st.hasMoreTokens())
            {
                return "";
            }
            String t2 = st.nextToken();
            //		 【外来院外処方】 2016/03/01(火) 循内  外来
            //		 依頼 01版： 2016/03/01(火) 11:45 医師）XXXXXX ㈪後期本人
            //		  作成： 2016/03/01(火) 11:45 作成者：医師）XXXXXX

            if (t2.Length < 4 || (t2.IndexOf("処方") < 0 && t2.IndexOf("持参薬報告") < 0))
            {
                return "";
            }
            if (!st.hasMoreTokens())
            {
                return "";
            }
            // 2行飛ばす
            String t = st.nextToken();
            if (!st.hasMoreTokens())
            {
                return "";
            }
            t = st.nextToken();
            if (!st.hasMoreTokens())
            {
                return "";
            }
            if (t.IndexOf("代行") >= 0)
            {
                t = st.nextToken();     // 代行の場合1行さらに飛ばす
            }

            while (st.hasMoreTokens())
            {
                t = st.nextToken();
                if (t.Trim().Equals("")) // 空の行
                    continue;
                if (isEqualToString(t, " 【服用"))     // 【服用開始日：2016/03/01(火)】 DEBUG 20170427
                    continue;
                if (isEqualToString(t, " 後発薬"))     //  後発薬品への変更可
                    continue;
                if (isEqualToString(t, " 持込数量"))        //  持込数量 DEBUG 20160519 length<5
                    continue;
                if (isEqualToString(t, "  【服用"))
                    continue;
                if (isEqualToString(t, " 中止"))
                    continue;
                if (isEqualToString(t, " 採用の有無"))       //  採用の有無 DEBUG 20160519 length<6
                    continue;
                if (isEqualToString(t, " 【同成分"))        //  【同成分 DEBUG 20160519 length<5
                    continue;
                if (isEqualToString(t, " 自院が処方"))       //  自院が処方した薬剤   20180816
                    continue;
                if (isEqualToString(t, " 自院以外が処方"))     //  自院以外が処方した薬剤   20181002
                    continue;
                if (isEqualToString(t, " 入院の契機"))       //   入院の契機となった傷病(以外)の治療   20180816
                    continue;
                if (isEqualToString(t, "薬剤コ"))      //  薬剤コ 原内科消化器科医院処方  20180816
                    continue;
                if (isEqualToString(t, " ○"))       //   ○処方医院
                    continue;
                if (isEqualToString(t, " １包化"))     //    ★１包化
                    continue;
                if (isEqualToString(t, " ★１包"))     //    ★１包化
                    continue;
                if (isEqualToString(t, " ☆１包"))     //    ★１包化
                    continue;
                if (isEqualToString(t, "  [院外] 1日1回を"))
                    continue;
                if (isEqualToString(t, "  〔院外〕１日１回を"))
                    continue;

                //1 ﾌﾛｾﾐﾄﾞ錠[ﾃﾊﾞ] 20mg （ﾗｼｯｸｽ錠20mg） (局) 1 錠
                //			 セララ錠 50mg 1 錠
                //			 １日１回　朝食後 7 日分
                //			 【服用開始日：2016/03/01(火) 昼】

                String t0 = replaceAll(t, "後発医薬品への変更可", "");
                t = replaceAll(t0, "シートで（分包除外）", "");
                t0 = replaceAll(t, "全分包", "");
                t = replaceAll(t0, "朝　食", "朝食");
                t0 = replaceAll(t, "昼　食", "昼食");
                t = replaceAll(t0, "夕　食", "夕食");
                t0 = replaceAll(t, "(局)", "");
                t = replaceAll(t0, "◎", "");        // 抗凝固薬についてるやつ
                t0 = replaceAll(t, "○", "");
                t = replaceAll(t0, "\r", "");
                t = replaceAll(t, "【般】", "");

                //System.out.println("Parsing: " + t + "\n");

                if (Char.IsDigit(t[0]))     // 行頭が数字
                {
                    StringTokenizer stt = new StringTokenizer(t, " ");
                    if (!stt.hasMoreTokens()) continue;
                    t0 = stt.nextToken();
                    if (t0[t0.Length - 1] == '.')
                    {   // メディカルネット
                        t0 = t0.Substring(0, t0.Length - 1);
                        //					System.out.println("Parsing: medicalnet" + t0 + "\n");
                    }
                    int n = int.Parse(t0) - 1;
                    if (n < 0) continue;
                    drugNumber = n;
                    minorNumber = 0;
                    t = t.Substring(t.IndexOf(" ") + 1);
                    //	System.out.println("Next: " + t + "\n");
                }


                if (isEqualToString(t, " 1日")
                        || isEqualToString(t, " １日")
                        || isEqualToString(t, " １週")                    // 追加 20170807
                        || isEqualToString(t, "  1日")                   // 信州メディカルネット
                        || isEqualToString(t, "  １日")               // 信州メディカルネット
                        || isEqualToString(t, " １回")
                        || isEqualToString(t, " 発作")
                        || isEqualToString(t, "  便秘")               // 信州メディカルネット
                        || isEqualToString(t, " 胸痛")
                        || isEqualToString(t, " 頭痛")                    // 追加 20170807
                        || isEqualToString(t, " 嘔気")                    // 追加 20180930
                        || isEqualToString(t, " 発熱")                    // 追加 20180930
                        || isEqualToString(t, " 動悸")                    // 追加 20181002
                        || isEqualToString(t, " むくみ")
                        || isEqualToString(t, " 風邪")
                        || isEqualToString(t, " 腹痛")
                        || isEqualToString(t, " 疼痛")
                        || isEqualToString(t, " 不穏")                    // 追加 20171002
                        || isEqualToString(t, " 不眠")                    // 追加 20170807
                        || isEqualToString(t, " 便秘")                    // 追加 20170902
                        || isEqualToString(t, " 落ち着かない")    // 追加 20170807
                        || isEqualToString(t, " 痛い")
                        || isEqualToString(t, " 医師")
                        || isEqualToString(t, " ｽﾗｲﾃﾞｨﾝｸﾞ")         // 追加 20200515
                        || isEqualToString(t, " 咳嗽")
                        || (t.Length > 4 && t.Substring(2, 3).Equals("眼"))              // 点眼薬　追加 20180806
                            )
                {   // 用法用量
                    StringTokenizer stt = new StringTokenizer(t, " ");
                    t0 = "";

                    while (stt.hasMoreTokens())
                    {
                        String t1 = stt.nextToken();
                        t0 += t1 + " ";
                        if (t1.IndexOf("前") >= 0 || t1.IndexOf("後") >= 0 || t1.IndexOf("点眼") >= 0)
                            break;      // 後  28 日分の日数を削除
                    }
                    //System.out.println("Added Ref: " + t0.trim() + "\n");
                    drugs[drugNumber, 0] = t0.Trim();
                }
                else
                {
                    // System.out.println("Added drug: " + t.trim() + "\n");
                    drugs[drugNumber, minorNumber + 1] = t.Trim();
                    minorNumber++;
                }

            }

            t = "";
            int i = 0;
            while (drugs[i, 0] != null)
            {
                int j = 1;
                while (drugs[i, j] != null)
                {
                    t += drugs[i, j] + " / " + drugs[i, 0] + "\n";
                    j++;
                }
                i++;
            }

            return t;
        }

        // OS specific GUI responder

        partial void convertButton(Foundation.NSObject sender)
        {
            string str = GetClipboardString();
            string outstr = convertString(str);

            NSMutableAttributedString attrstr = new NSMutableAttributedString(outstr);
            outTextView.TextStorage.SetString(attrstr);

            SetClipboardString(outstr);
        }

        // OS specific function

        private static string[] pboardTypes = new string[] { "NSStringPboardType" };

        public string GetClipboardString()
        {
            return NSPasteboard.GeneralPasteboard.GetStringForType(pboardTypes[0]);
        }


        public void SetClipboardString(string str)
        {
            NSPasteboard.GeneralPasteboard.DeclareTypes(pboardTypes, null);
            NSPasteboard.GeneralPasteboard.SetStringForType(str, pboardTypes[0]);
        }
    }
}
