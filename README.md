## imageClipPaste

コピーした画像を、Microsoft Excelに貼り付けるアプリケーションです。


## 機能
* 一定間隔でクリップボードを監視します。
* 画像がコピーされたら、Excelのワークシートに画像を貼り付けます。
* 貼り付け先のワークシートは、一覧の中から選択します。


## 動作環境
* 環境
 * Windows 7, Windows 8, Windows 8.1
 * Excel 2002, Excel 2003, Excel 2007, Excel 2010, Excel 2013
* 必要なソフトウェア
 * .NET Framework 4.5.1インストール済み環境


## 使い方
#### imageClipPaste を起動します。
![起動画面](https://raw.githubusercontent.com/mizutuu/imageClipPaste/gh-pages/img/usage01.png)

#### クリップボード監視ボタンをクリックします。
![初期画面](https://raw.githubusercontent.com/mizutuu/imageClipPaste/gh-pages/img/usage02.png)

#### 貼り付け先のワークブックを選択します。
![貼り付け先選択画面](https://raw.githubusercontent.com/mizutuu/imageClipPaste/gh-pages/img/usage03.png)

クリップボード監視中は、クリップボード監視ボタンが明滅します。
![監視中画面](https://raw.githubusercontent.com/mizutuu/imageClipPaste/gh-pages/img/usage04.png)

#### 画像をコピーします。
* 貼り付け先のワークブックに、コピーした画像が貼り付けられます。
* ワークブック内の選択されているセルに貼り付けられます。

## 使用上のヒント
* 貼り付け先のExcelが終了された場合は、自動的に監視が止まります。
* Excelが未インストールの環境では、クリップボード監視ボタンはクリックできません。


## インストール
* ダウンロードしたimageClipPasteを解凍してください。


## アンインストール
* インストールで解凍したファイルを削除してください。
* レジストリは未使用です。


## マイルストーン
* 貼り付け先の拡充（PowerPoint, Word）
* エクスプローラー上での画像コピーも監視対象とする


## 利用しているライブラリ
以下のオープンソースライブラリの力を借りて作成されています。
* NetOffice.Excel
  http://netoffice.codeplex.com/
* MahApps.Metro
  http://mahapps.com/
* MVVM Light Toolkit
  http://www.galasoft.ch/mvvm
* NLog
  https://github.com/NLog/NLog/


## ライセンス
本アプリケーションは、MITライセンスです。

