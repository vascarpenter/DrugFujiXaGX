# DrugFujiXaGX

- DrugFujiGX の C# xamarin.mac version
- Visual Studio 2019 Mac/Xcodeで作成
- 単なる動作確認の目的で実際に使うことはない

- 使い方
  - HOPE EGMAIN/GXで、処方のテキスト欄を【外来院外処方】の文字列開始後から 後発薬品への変更可等の   までを選択
  - テキストコピー(CTRL+C)を押す
  - このプログラムを起動
  - Convert Clipboardボタンを押す

- たとえば
```
 【外来院外処方】 2016/03/01(火) XXXX  外来 
依頼 01版： 2016/03/01(火) 11:45 医師）XXXXXX ②後期本人 
 作成： 2016/03/01(火) 11:45 作成者：医師）XXXXX 
 
1 ◎ﾊｰﾌｼﾞｺﾞｷｼﾝKY錠0.125mg (局) 1 錠 
 １日１回　朝食後 28 日分 
 【服用開始日：2016/03/01(火)】   
2 ローコール錠 10mg 1 錠 
 ｵﾒﾌﾟﾛﾄﾝ錠 20mg （ｵﾒﾌﾟﾗｿﾞﾝ錠20mg） 1 錠 
 １日１回　夕食後 28 日分 
 【服用開始日：2016/03/01(火)】   
```
が
```
ﾊｰﾌｼﾞｺﾞｷｼﾝKY錠0.125mg  1 錠 / １日１回　朝食後
ローコール錠 10mg 1 錠 / １日１回　夕食後
ｵﾒﾌﾟﾛﾄﾝ錠 20mg （ｵﾒﾌﾟﾗｿﾞﾝ錠20mg） 1 錠 / １日１回　夕食後
```
という形に編集され、紹介状などにペーストしやすくなる