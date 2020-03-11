# ScreenCapture

VARIAN社製治療計画装置Eclipse上でスクリーンキャプチャを行うスクリプトです。  

## 導入方法

本スクリプトはbinary-plugin型となるため、事前にビルドして実行ファイルを生成する必要があります。  
ビルドで生成された「ScreenCapture.esapi.dll」をEclipse上で実行します。  

動作検証：Eclipse version 15.6  

## 操作方法

- 「Tools」-->「Scripts」から「ScreenCapture.esapi.dll」を選択する。
- 「Favorites」の「Add」ボタンを押す。
- 「Keyboard Shortcut」を入力状態にして、ショートカットキーを登録します。
 例）[CTRL]+[SHIFT]+[C]
- スクリプトは必ず**設定したショートカットキー**で実行します。
- 0.5秒間の待機後に画面キャプチャが行われます。
- デフォルト設定は以下の通りです。　(＊)の項目は設定変更をスクリプト「ScreenCapturePreference」で行えます。
  - キャプチャ領域：アクティブウィンドウ　(＊)
  - 保存場所：デスクトップ　(＊)
  - 保存形式：JPEG
  - 保存ファイル名：SC_患者ID_コースID_プランID_年月日_時分秒.jpg

## 各種設定
- 設定はユーザー毎かつ端末共通です。
- 各種設定の変更はスクリプト「ScreenCapturePreference」で行います。

## UI画面

なし
